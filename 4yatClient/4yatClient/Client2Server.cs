using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Cryptography;
using System;
using System.ComponentModel;
using System.Reflection;
using Crypt;
using System.Threading;

//с->к
//0 отклонение регистрации
//3 сообщение
//4 личка
//6 ключ
//7 список всех юзеров
//8 сообщение о входе юзера
//9 сообщение о дисконнекте
//h - прием информации о файле
//f - прием файла
//a - прием информации о приеме файле
//r - прием информации об отказе от файла

//к -> c
//null - пустое сообщение(проверка подключения к серверу)
//1 - регистрация
//2 - авторизация
//3 - сообщение
//4 - личка
//h - передача информации о файле
//f - передача файла
//a - передача информации о приеме файле
//r - передача информации об отказе от файла


namespace _4yatClient
{
    public class Client2Server
    {
        public string userName { get; }
        public string userPas { get; }
        private string host;
        private int port;
        private string key;
        static TcpClient client;
        static NetworkStream stream;
        CryptoClass Cr;

        public enum ClientKeys
        {            
            REGISTRATION = 1,
            AUTORISATION = 2,
            BROADCAST = 3,
            PRIVATE = 4,
            [Description("h")]
            FILEINFO = 11,
            [Description("f")]
            FILE = 12,
            [Description("r")]
            FILEREMOVE = 13,
            [Description("a")]
            FILEACCEPT = 14,
            [Description("s")]
            FILEREADY = 15,
            [Description(null)]
            NULL = 16
        }

        public enum ServerKeys
        {
            REJECT = 0,
            BROADCAST = 3,
            PRIVATE = 4,
            KEY = 6,
            USERLIST = 7,
            USERADD = 8,
            USERREMOVE = 9,
            [Description("h")]
            FILEINFO = 11,
            [Description("f")]
            FILE = 12,
            [Description("r")]
            FILEREMOVE = 13,
            [Description("a")]
            FILEACCEPT = 14,
            [Description("s")]
            FILEREADY = 15
        }        

        public static string GetDescription(Enum enumElement)
        {
            Type type = enumElement.GetType();
            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return enumElement.ToString();
        }

        public Client2Server(string user, string password, string server, int port)
        {
            var md5 = MD5.Create();
            this.userName = user;
            this.userPas = Encoding.Unicode.GetString(md5.ComputeHash(Encoding.Unicode.GetBytes(password)));
            this.host = server;
            this.port = port;
            client = new TcpClient();
            client.Connect(this.host, this.port);
            stream = client.GetStream();
            Cr = new CryptoClass();
        }

        public Client2Server(string server, int port)
        {
            client = new TcpClient();
            client.Connect(server, port);
            key = "";
        }

        async public Task<bool> ReconnectAsync()
        {
            return await Task.Run(() => Reconnect());
        }

        public bool Reconnect()
        {
            bool ret;
            //sleep потока чтобы на сервер не отправилось несколько попыток авторизации
            Thread.Sleep(2000);
            //попытка подключения
            try
            {
                //обновление объекта класса TcpClient
                client = new TcpClient();
                //коннект к серверу
                client.Connect(this.host, this.port);
                //получение потока сервер-клиента
                stream = client.GetStream();
                //отправка запроса авторизации
                SendMessage(ClientKeys.AUTORISATION, this.userName + ";" + this.userPas);
                //возврат - подключение восстановлено
                ret = true;
            }
            catch
            {
                //возврат - подключение не восстановлено
                ret = false;
            }
            return ret;
        }

        public void SendMessage(Enum enumElement, string message)
        {
            byte[] data;
            switch (enumElement)
            {
                case ClientKeys.FILEINFO:
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(GetDescription(ClientKeys.FILEINFO) + message, key));
                    break;
                case ClientKeys.FILE: 
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(GetDescription(ClientKeys.FILE) + RemoveNulls(message), key));
                    break; 
                case ClientKeys.FILEREMOVE: 
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(GetDescription(ClientKeys.FILEREMOVE) + message, key));
                    break; 
                case ClientKeys.FILEACCEPT: 
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(GetDescription(ClientKeys.FILEACCEPT) + message, key));
                    break; 
                case ClientKeys.FILEREADY: 
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(GetDescription(ClientKeys.FILEREADY), key));
                    break;
                case ClientKeys.AUTORISATION:
                    data = Encoding.Unicode.GetBytes((int)ClientKeys.AUTORISATION + message);
                    break;
                case ClientKeys.NULL:  
                    data = new byte[] { };
                    break; 
                default:
                    data = Encoding.Unicode.GetBytes(Cr.Encrypt(Convert.ToInt32(enumElement) + message, key));
                    break;
            }
            stream.Write(data, 0, data.Length);
        }       

        public void SendReg(string user, string pass)
        {
            //создание объекта для хеширования
            var md5 = MD5.Create();
            //получение потока между клиентом и сервером
            stream = client.GetStream();
            //формирование сообщения (ключ регистрации, имя пользователя, захешированый пароль)
            byte[] data = Encoding.Unicode.GetBytes((int)ClientKeys.REGISTRATION + user + ";"
                + Encoding.Unicode.GetString(md5.ComputeHash(Encoding.Unicode.GetBytes(pass))));
            //запись данных в поток
            stream.Write(data, 0, data.Length);
        }

        async public Task<(string, string)> GetMessageAsync()
        {
            return await Task.Run(() => GetMessage());
        }

        public (string, string) GetMessage()
        {
            try
            {
                byte[] data = new byte[256]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                //пока есть данные в потоке, запоминай их
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                //переведи полученные данные в формат строки
                string message = builder.ToString();
                //отлови пустые сообщения
                if (message.Length == 0) return (null, null);
                //если сервер отослал ключ шифрования, то запомни его
                else if (Convert.ToString(message[0]) == ((int)ServerKeys.KEY).ToString())
                {
                    key = message.Remove(0, 1);
                    key = key.Remove(32, key.Length-32);
                    message = message.Remove(0, 33);
                    message = Cr.Decrypt(message, key);
                }
                //если ключ уже есть, расшифруй сообщение
                else if (this.key.Length > 0)
                    message = RemoveNulls(Cr.Decrypt(message, key));
                return (Convert.ToString(message[0]), message.Remove(0, 1));
            }
            catch
            {
                //В случае потери ответа от сервера, отключись и напиши
                //об этом пользователю
                Disconnect();
                return ("", "Выполнено отключение от чата, попытка переподключения");
            }
        }

        public void Disconnect()
        {
            if (stream != null) stream.Close();//отключение потока
            if (client != null) client.Close();//отключение клиента
        }

        private string RemoveNulls(string message)
        {
            int i = message.IndexOf('\0');
            if (i > 0)
            {
                message = message.Remove(i, 1);
                return RemoveNulls(message);
            }
            else
            {
                return message;
            }
        }
    }
}
