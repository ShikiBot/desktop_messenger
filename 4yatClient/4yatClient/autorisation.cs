using System;
using System.Drawing;
using System.Windows.Forms;

namespace _4yatClient
{
    public partial class autorisation : Form
    {
        public autorisation()
        {
            InitializeComponent();
            this.Text = "Авторизация";
            this.Width = 400;
            this.Height = 570;
            nicname.Text = "Введите Ваше имя:";
            nicname.Location = new Point(this.Width / 2 - nicname.Width / 2, 100);
            passwd.Text = "Введите пароль:";
            passwd.Location = new Point(this.Width / 2 - passwd.Width / 2, 200);
            server.Text = "IP:port";
            server.Location = new Point(this.Width / 2 - server.Width / 2, 300);
            ok.Width = 300;
            ok.Text = "В чат";
            ok.Location = new Point(this.Width / 2 - ok.Width / 2, 400);
            ok2.Width = 300;
            ok2.Text = "Регистрация";
            ok2.Location = new Point(this.Width / 2 - ok.Width / 2, 460);
            ipp.Location = new Point(this.Width / 2 - ipp.Width / 2, 340);
            name.Location = new Point(this.Width / 2 - name.Width / 2, 140);
            pas1.Location = new Point(this.Width / 2 - name.Width / 2, 240);
            pas1.PasswordChar = '*';
        }
        
        private void ok_Click(object sender, EventArgs e)
        {
            //если пользователь ввел не все данные (логин, пароль, адрес сервера)
            //то напомни ему чтобы ввел
            if (name.Text.Length == 0 || pas1.Text.Length == 0 || ipp.Text.Length == 0)
                MessageBox.Show("Введите значения во все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    //парсинг введенных пользователем данных
                    string login = name.Text;
                    string pas = pas1.Text;
                    string ip = ipp.Text.Split(':')[0];
                    int port = Convert.ToInt32(ipp.Text.Split(':')[1]);
                    //создание объекта для общения с сервером, подключение к нему
                    Client2Server C2S = new Client2Server(login, pas, ip, port);
                    //отсылание серверу пустого сообщения для проверки связи
                    C2S.SendMessage(Client2Server.ClientKeys.NULL, "");
                    //если все прошло успешно открывай форму с чатом
                    _4at ch = new _4at(C2S);
                    ch.Show(this);
                    this.Hide();
                }
                catch
                {
                    //если не получилось то либо сервер не поднят, либо юзер ошибся с данными
                    MessageBox.Show("Ошибка подключения к серверу\nОшибка адреса сервера или " +
                        "сервер отсутсвует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ok2_Click(object sender, EventArgs e)
        {
            registration rg = new registration();
            rg.Show(this);
        }
    }
}
