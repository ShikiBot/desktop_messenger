using System;
using System.Drawing;
using System.Windows.Forms;

namespace _4yatClient
{
    public partial class registration : Form
    {
        Client2Server C2S;
        public registration()
        {
            InitializeComponent();
            this.Text = "Регистрация";
            this.Width = 400;
            this.Height = 570;
            nicname.Text = "Введите Ваше имя:";
            nicname.Location = new Point(this.Width / 2 - nicname.Width / 2, 50);
            passwd.Text = "Введите пароль:";
            passwd.Location = new Point(this.Width / 2 - passwd.Width / 2, 150);
            passwd2.Text = "Повторите пароль:";
            passwd2.Location = new Point(this.Width / 2 - passwd2.Width / 2, 250);
            ok.Width = 300;
            ok.Text = "Начать";
            ok.Location = new Point(this.Width / 2 - ok.Width / 2, 450);
            name.Location = new Point(this.Width / 2 - name.Width / 2, 90);
            pas1.Location = new Point(this.Width / 2 - name.Width / 2, 190);
            pas1.PasswordChar = '*';
            pas2.PasswordChar = '*';
            pas2.Location = new Point(this.Width / 2 - name.Width / 2, 290);
            server.Text = "IP:port";
            server.Location = new Point(this.Width / 2 - server.Width / 2, 350);
            ipp.Location = new Point(this.Width / 2 - ipp.Width / 2, 390);
            pas1Chk.Text = "";
            pas1Chk.Location = new Point(pas1.Location.X + pas1.Width, 190);
            pas2Chk.Text = "";
            pas2Chk.Location = new Point(pas2.Location.X + pas2.Width, 290);
        }

        private async void ok_Click(object sender, EventArgs e)
        {          
            //обработка ошибок пользователя, в том числе проверка политики длинны пароля
            if (name.Text.Length == 0 || pas1.Text.Length == 0 || pas2.Text.Length == 0 || ipp.Text.Length == 0)
                MessageBox.Show("Введите значения во все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (pas1.Text != pas2.Text)
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (pas1.Text.Length < 6)
                MessageBox.Show("Пароль слишком короткий", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    //временное подключение к серверу
                    C2S = new Client2Server(ipp.Text.Split(':')[0], Convert.ToInt32(ipp.Text.Split(':')[1]));
                    //отправка на сервер никнейма и пароля
                    C2S.SendReg(name.Text, pas1.Text);
                    int i = 0;
                    //ожидание ответа от сервера
                    (string, string) s = ("", "");
                    while (i < 100 && s.Item1 != Client2Server.GetDescription(Client2Server.ServerKeys.REJECT))
                    {
                        i++;
                        s = await C2S.GetMessageAsync();
                    }
                    //отключение от сервера
                    C2S.Disconnect();
                    //если сервер отправил сообщение об отклонении регистрации (такой ник уже существует)
                    //то сообщи об этом юзеру
                    if (s.Item1 == Client2Server.GetDescription(Client2Server.ServerKeys.REJECT))
                        MessageBox.Show("Ошибка регистрации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //иначе пользователь зарегестрирован
                    else
                        this.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения к серверу\nОшибка адреса сервера или сервер отсутсвует", 
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pas2_TextChanged(object sender, EventArgs e)
        {
            chk(pas1, pas2, pas1Chk, pas2Chk);
        }

        private void pas1_TextChanged(object sender, EventArgs e)
        {
            chk(pas1, pas2, pas1Chk, pas2Chk);
        }

        public void chk(TextBox p1, TextBox p2, Label pc1, Label pc2)
        {
            if (p1.Text.Length > 5)
            {
                pc1.Text = "✔";
                pc1.ForeColor = Color.LightGreen;
            }
            else
            {
                pc1.Text = "x";
                pc1.ForeColor = Color.DarkRed;
            }
            if (p1.Text == p2.Text && p1.Text.Length > 5)
            {
                pc2.Text = "✔";
                pc2.ForeColor = Color.LightGreen;
            }
            else
            {
                pc2.Text = "x";
                pc2.ForeColor = Color.DarkRed;
            }
        }
    }
}
