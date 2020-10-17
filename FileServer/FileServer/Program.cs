using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Crypt;

namespace FileServer
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера
        protected internal string user { get; private set; }
        protected internal string key { get; private set; }

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                if (message[0] == '1') //регистрация
                {
                    message = message.Remove(0, 1);
                    if (!FileWrite(message).Result) //отклонение регистрации
                    {
                        byte[] reject = Encoding.Unicode.GetBytes("0");
                        Stream.Write(reject, 0, reject.Length);
                    }
                }

                if (message[0] == '2') //авторизация
                {
                    message = message.Remove(0, 1);
                    if (FileRead(message).Result == 2) //обработать несколько юзеров с одним ником
                    {
                        CryptoClass enc = new CryptoClass();
                        key = enc.GetStringKey(); //генерация ключа
                        server.Keys(key);
                        Console.WriteLine(key);
                        string keymess = "6" + key;
                        byte[] keym = Encoding.Unicode.GetBytes(keymess);
                        Stream.Write(keym, 0, keym.Length); //отправка ключа
                        string[] name = message.Split(';');//список юзеров
                        userName = name[0];
                        user = userName;
                        message = "8" + userName;//connect                       
                    // посылаем сообщение о входе в чат всем подключенным пользователям
                        server.BroadcastMessage(message, this.Id);
                        
                        //список всех юзеров для новоприбывшего
                        string snames = "7";
                        server.Usernames(user);
                        List<string> unames = server.Usrnms;
                        for (int i = 0; i < unames.Count; i++)
                        {
                            snames += unames[i] + ";";
                        }
                        snames = snames.Remove(snames.Length - 1);                        
                        snames = enc.Encrypt(snames, key);
                        byte[] lnames = Encoding.Unicode.GetBytes(snames);
                        Stream.Write(lnames, 0, lnames.Length);
                        Console.WriteLine(message.Remove(0, 1) + " вошёл в чат");
                        string[] info = new string[3]; //0 получатель файла, 1 имя.расширение файла, 2 размер файла
                        while (true) //обмен сообщениями
                        {
                            try
                            {
                                message = GetMessage();
                                message = enc.Decrypt(message, key);
                                if (message[0] == '3')//обычное сообщение
                                {
                                    message = message.Remove(0, 1);
                                    message = String.Format("3"+ userName+":"+ message);
                                    Console.WriteLine(message);
                                    server.BroadcastMessage(message, this.Id);
                                }
                                if (message[0] == '4')//личка
                                {
                                    message = message.Remove(0, 1);
                                    string PrivateMsg = message.Substring(message.IndexOf(':') + 1);
                                    string PrivateName = message.Substring(0, message.IndexOf(':'));
                                    Console.WriteLine(this.userName + " для " + PrivateName + ": " + PrivateMsg);
                                    server.PrivateMessage(PrivateName,"4"+this.userName, PrivateMsg);
                                }                              
                                if (message[0] == 'h')//заголовок файла
                                {
                                    message = message.Remove(0, 1);
                                    info = message.Split(':');                                   
                                    Console.WriteLine("файл от "+this.userName + " для " + info[0] + ": " + info[1]+" "+info[2]);
                                    server.PrivateMessage(info[0], "h" + this.userName, info[1] + ":" + info[2]);
                                }
                                if (message[0] == 'a')//принял передачу
                                {
                                    message = message.Remove(0, 1);
                                    message = RemoveNulls(message);
                                    Console.WriteLine(this.userName + " принял файл от " + message);
                                    server.PrivateMessage(message, "a"+this.userName, "");                                   
                                }
                                if (message[0] == 'r')//отклонил передачу
                                {
                                    message = message.Remove(0, 1);
                                    message = RemoveNulls(message);
                                    Console.WriteLine(this.userName + " отклонил файл от " + message);
                                    server.PrivateMessage(message, "r"+this.userName, "");
                                }
                                if (message[0] == 'f')//приём файла
                                {
                                    Console.WriteLine("прием");
                                    message = message.Remove(0, 1);
                                    //message = RemoveKosyak(message);
                                    GetFile(info[1], message);
                                }
                                if (message[0] == 's')//приём файла
                                {
                                    Console.WriteLine("отправка");
                                    SendFile(info[0], info[1]);
                                    File.Delete(info[1]);
                                }
                            }
                            catch
                            {
                                message = String.Format("9" + userName);//disconnect
                                Console.WriteLine(message.Remove(0, 1) + " покинул чат");
                                server.BroadcastMessage(message, this.Id);
                                break;
                            }
                        }
                    }
                }
                // в бесконечном цикле получаем сообщения от клиента

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }

        private void GetFile(string filename,  string message)
        {
            StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
            sw.Write(message);
            sw.Close();
        }

        private void SendFile(string polname, string filename)
        {
            FileStream fs = new FileInfo(filename).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            int buffersize = 1400;
            byte[] buf = new byte[buffersize];
            while (fs.Read(buf, 0, buffersize) > 0)
            {
                string enc = Encoding.UTF8.GetString(buf);
                enc = RemoveNulls(enc);
                server.PrivateMessage(polname, "f" + enc, "");
            }
            fs.Close();
            string s = "s";
            //byte[] send = Encoding.Unicode.GetBytes(s);
            server.PrivateMessage(polname, s, "");
            //Stream.Write(send, 0, send.Length);
        }

        private string RemoveNulls(string message)
        {
            int i = message.IndexOf('\0');
            if (i>0)
            {
                message = message.Remove(i, 1);
                return RemoveNulls(message);
            }
            else
            {
                return message;
            }
        }

        protected internal async Task<bool> FileWrite(string data)//регистрация
        {
            bool r = false;
            if (FileRead(data).Result == 0)
            {
                byte[] encodedText = Encoding.Unicode.GetBytes("\r\n" + data);

                using (FileStream sourceStream = new FileStream("data.txt",
                    FileMode.Append, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    sourceStream.Close();
                };
                Console.WriteLine("зарегестрирован пользователь " + data);
                r = true;
            }
            return r;
        }
        protected internal async Task<int> FileRead(string data)//чтения файла паролей
        {
            int r = 0;
            string[] wordsD = data.Split(';');
            using (StreamReader reader = File.OpenText("data.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string txt = await reader.ReadLineAsync();
                    string[] words = txt.Split(';');
                    if (words[0] == wordsD[0])
                    {
                        r = 1;// совпадает имя. нужно для регистрации   
                    }
                    if (txt == data)
                    {
                        r = 2;//совпадает вся строка. нужно для авторизации
                        break;
                    }
                }
                reader.Close();
            }
            return r;
        }

    }

    public class ServerObject
    {
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения
        List<string> usernames = new List<string>();
        List<string> keys = new List<string>();
        public List<string> Usrnms
        {
            get
            {
                return usernames;
            }
        }
        public void Usernames(string value)
        {
            usernames.Add(value);
        }

        public List<string> KeysGet
        {
            get
            {
                return keys;
            }
        }
        public void Keys(string value)
        {
            keys.Add(value);
        }

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            usernames.Remove(client.user);
            keys.Remove(client.key);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 1337);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id)
        {
            CryptoClass enc = new CryptoClass();
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    string en = enc.Encrypt(message, keys[i]);
                    byte[] data = Encoding.Unicode.GetBytes(en);
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }

        protected internal void PrivateMessage(string polname, string otprname, string message)//личка
        {
            CryptoClass enc = new CryptoClass();
            if (message != "")
            {
                message = otprname + ":" + message;
            }
            else
            {
                message =otprname;
            }
            int i = usernames.IndexOf(polname);
            Console.WriteLine(message);
            string en = enc.Encrypt(message, keys[i]);
            byte[] data = Encoding.Unicode.GetBytes(en);
            clients[i].Stream.Write(data, 0, data.Length);
        }
        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }

    class Program
    {
        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания   
        static void Main(string[] args)
        {                          
                try
                {
                    server = new ServerObject();
                    listenThread = new Thread(new ThreadStart(server.Listen)); //удалить потоки
                    listenThread.Start(); //старт потока
                }
                catch (Exception ex)
                {
                    server.Disconnect();
                    Console.WriteLine(ex.Message);
                }
            
        }
    }
}
