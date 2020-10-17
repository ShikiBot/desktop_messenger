using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace _4yatClient
{
    public partial class _4at : Form
    {
        Client2Server C2S;
        string filePathSend, filePathSave, WinUser;
        bool chk = true, chk2 = true;

        public _4at(Client2Server server)
        {
            InitializeComponent();
            C2S = server;
            C2S.SendMessage(Client2Server.ClientKeys.AUTORISATION, server.userName + ";" + server.userPas);   
            timer1.Enabled = true;
            openFileDialog1.AddExtension = true;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            WinUser = ((string)collection.Cast<ManagementBaseObject>().First()["UserName"]).Split('\\')[1];
        }

        private void _4at_Load(object sender, EventArgs e)
        {
            this.Text = "Чатик Бота";
            userList.Height = this.Height - 35;
            userList.Location = new Point(this.Width - userList.Width - 15, 0);
            textBox1.Location = new Point(5, this.Height - 67);
            textBox1.Width = this.Width - userList.Width - 56;
            button1.Location = new Point(textBox1.Location.X + textBox1.Width + 3, textBox1.Location.Y);
            chatSpace.Location = new Point(5, 0);
            chatSpace.Width = textBox1.Width + button1.Width + 3;
            chatSpace.Height = userList.Height - 25;
            chatSpace.AppendText(C2S.userName + " вошел в чат" + Environment.NewLine, Color.Green); 
        }

        private void _4at_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            Application.Exit();
        }

        private void _4at_SizeChanged(object sender, EventArgs e)
        {
            userList.Height = this.Height - 35;
            userList.Location = new Point(this.Width - userList.Width - 15, 0);
            textBox1.Location = new Point(5, this.Height - 67);
            textBox1.Width = this.Width - userList.Width - 56;
            button1.Location = new Point(textBox1.Location.X + textBox1.Width + 3, textBox1.Location.Y);
            chatSpace.Location = new Point(5, 0);
            chatSpace.Width = textBox1.Width + button1.Width + 3;
            chatSpace.Height = userList.Height - 25;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && timer1.Enabled == true)
            {
                //личное сообщение
                if (textBox1.Text.Contains(" ") && textBox1.Text.Split(':').Length > 1)
                {
                    string[] s = textBox1.Text.Split(':');
                    string us = s[0];
                    s = s.Where(x => !x.Contains(" ")).ToArray();
                    chatSpace.AppendText(us + ": " + String.Join(",", s) + Environment.NewLine, Color.HotPink);
                    C2S.SendMessage(Client2Server.ClientKeys.PRIVATE, textBox1.Text.Remove(0, 10));
                }
                //общее сообщение
                else
                {
                    chatSpace.AppendText(C2S.userName, Color.Blue);
                    chatSpace.AppendText(": " + textBox1.Text + Environment.NewLine);
                    C2S.SendMessage(Client2Server.ClientKeys.BROADCAST, textBox1.Text);
                }
                textBox1.Text = "";
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            var s = await C2S.GetMessageAsync();            
            if (s.Item1 == ((int)Client2Server.ServerKeys.USERLIST).ToString())  //get userList
            {
                string[] users = s.Item2.Split(';');
                foreach (string x in users)
                    userList.Items.Add(x);
                chk2 = true;
            }
            else if (s.Item1 == ((int)Client2Server.ServerKeys.USERADD).ToString()) //get new user
            {
                userList.Items.Add(s.Item2);
                chatSpace.AppendText(s.Item2, Color.Green);
                chatSpace.AppendText(" присоединился к чату", Color.Green);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == ((int)Client2Server.ServerKeys.USERREMOVE).ToString()) //remowe disconnected user
            {
                userList.Items.Remove(s.Item2);
                chatSpace.AppendText(s.Item2, Color.DarkRed);
                chatSpace.AppendText(" покинул чат", Color.DarkRed);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == ((int)Client2Server.ServerKeys.PRIVATE).ToString()) //private message
            {
                chatSpace.AppendText("Шепот от " + s.Item2, Color.HotPink);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == ((int)Client2Server.ServerKeys.BROADCAST).ToString())
            {
                string[] ss = s.Item2.Split(':');
                string sss = "";
                for (int i = 1; i < ss.Length; i++) sss += ss[i];
                chatSpace.AppendText(ss[0].ToString(), Color.Blue);
                chatSpace.AppendText(": " + sss);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.FILEINFO))
            {
                DialogResult result = MessageBox.Show("Пользователь " + s.Item2.Split(':')[0] + " хочет отправить Вам файл "
                    + s.Item2.Split(':')[1] + " размером " + NotByteLength(s.Item2.Split(':')[2]) + "\nПринять его?", "Извещение о файле",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    filePathSave = s.Item2.Split(':')[1];
                    chatSpace.AppendText("Прием файла от " + s.Item2.Split(':')[0] + ": " + s.Item2.Split(':')[1], Color.DarkViolet);
                    chatSpace.AppendText(Environment.NewLine);
                    C2S.SendMessage(Client2Server.ClientKeys.FILEACCEPT, s.Item2.Split(':')[0]);
                }
                if (result == DialogResult.No)
                    C2S.SendMessage(Client2Server.ClientKeys.FILEREMOVE, s.Item2.Split(':')[0]);
            }
            else if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.FILE))
            {
                string filename = @"C:\Users\" + WinUser + @"\Downloads\" + filePathSave;
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
                byte[] data = Encoding.UTF8.GetBytes(s.Item2.Remove(0, 1));
                fs.Seek(0, SeekOrigin.End);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
            else if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.FILEACCEPT))
            {
                chatSpace.AppendText("Пользователь принял файл", Color.DarkViolet);
                FileStream fs = new FileInfo(filePathSend).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                await Task.Factory.StartNew(() => FileRead(fs, C2S));
                //FileRead(fs, C2S);
                chatSpace.AppendText(Environment.NewLine);
            }
            
            else if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.FILEREMOVE))
            {
                chatSpace.AppendText("Пользователь отклонил файл", Color.DarkViolet);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.FILEREADY))
            {
                chatSpace.AppendText("Файл сохранен", Color.DarkViolet);
                chatSpace.AppendText(Environment.NewLine);
            }
            else if (s.Item1 == "" && chk)
            {
                chatSpace.AppendText(s.Item2);
                chatSpace.AppendText(Environment.NewLine);
                chk = false;
                timer1.Stop();
                timer2.Start();
                userList.Items.Clear();
            }
        }

        private void userList_DoubleClick(object sender, EventArgs e)
        {
            if (userList.SelectedItem != null)
            {
                textBox1.Text += "Шепот для ";
                textBox1.Text += userList.SelectedItem;
                textBox1.Text += ": ";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(" ") && textBox1.Text.Split(':').Length > 1)
            {
                string[] s = textBox1.Text.Split(':');
                string us = s[0];
                if (s[1].Length > 1)
                {
                    s = s.Where(x => !x.Contains(" ")).ToArray();
                    chatSpace.AppendText(us + ": " + String.Join(",", s) + Environment.NewLine, Color.HotPink);
                    C2S.SendMessage(Client2Server.ClientKeys.PRIVATE, textBox1.Text.Remove(0, 10));
                }
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                s = openFileDialog1.FileName.Split('\\');
                chatSpace.AppendText("Отправка файла для " + us.Remove(0, 10) + ": " + s[s.Length-1] + Environment.NewLine, Color.DarkViolet);
                C2S.SendMessage(Client2Server.ClientKeys.FILEINFO, us.Remove(0, 10) + ":" + s[s.Length-1] + ":" + (int)(new FileInfo(openFileDialog1.FileName).Length));                
                textBox1.Text = "";
                filePathSend = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("Выберите пользователя", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            bool isRecon = await C2S.ReconnectAsync();
            if (isRecon && chk2)
            {
                timer2.Stop();
                timer1.Start();
                chk = true;
                chk2 = false;
                chatSpace.AppendText("Подключение восстановлено");
                chatSpace.AppendText(Environment.NewLine);
            }
        }

        static void FileRead(FileStream fs, Client2Server C2S)
        {
            byte[] buf = new byte[1400];
            while (fs.Read(buf, 0, 1400) > 0)
                C2S.SendMessage(Client2Server.ClientKeys.FILE, Encoding.UTF8.GetString(buf));
            fs.Close();
            C2S.SendMessage(Client2Server.ClientKeys.FILEREADY, "");
        }

        string NotByteLength(string byteLength)
        {
            double length = Convert.ToSingle(byteLength);
            string nbl = length / (1024 * 1024) > 1 ? Math.Round(length / (1024 * 1024), 2) + " Мб" :
                length / 1024 > 1 ? Math.Round(length / 1024, 2) + " Кб" :
                length + " б";
            return nbl;
        }
    }
}
