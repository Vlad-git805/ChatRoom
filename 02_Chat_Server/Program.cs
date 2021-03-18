using ComandClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_Chat_Server
{
    class Program
    {
        // порт для прослуховування
        private const int port = 8080;
        // список учасників чату
        //private static List<IPEndPoint> members = new List<IPEndPoint>();
        private static List<Server_Command> members = new List<Server_Command>();

        // створення об'єкту UdpClient та встановлюємо порт для прослуховування
        static UdpClient server = new UdpClient(port);
        // створюємо об'єкт для збреження адреси віддаленого хоста
        static IPEndPoint groupEP = null;



        static void Main(string[] args)
        {
            
            //Semaphore semaphore = new Semaphore(2,2);
            try
            {
                while (true)
                {
                    Server_Command server_Command = new Server_Command();
                    Console.WriteLine("\tWaiting for a message...");
                    byte[] bytes = server.Receive(ref groupEP);

                    Client_Command client_Command = (Client_Command)ByteArrayToObject(bytes);
                    server_Command.iPEndPoint = groupEP;
                    server_Command.Name = client_Command.Name;
                    server_Command.Message = client_Command.Message;

                    Task.Run(() => Client_server(server_Command/*, semaphore*/));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                // закриття з'єднання
                server.Close();
            }
        }

        static void Client_server(Server_Command server_Command/*, Semaphore semaphore*/)
        {
            string msg = server_Command.Message;
            string name = server_Command.Name;

            //if (!semaphore.WaitOne(200))
            //{
            //    string str = name + " Wheit please a fiew minutes to connect";
            //    byte[] byt = Encoding.ASCII.GetBytes(str);
            //    server.Send(byt, byt.Length, groupEP);
            //    return;
            //}

            bool isSuccesful;
            if (msg == "<connect>")
            {
                //if(members.Count+1 > 2)
                //{
                //    string str = name + " Wheit please a fiew minutes to connect";
                //    byte[] byt = Encoding.ASCII.GetBytes(str);
                //    server.Send(byt, byt.Length, groupEP);
                //    return;
                //}

                isSuccesful = AddMember(server_Command);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Request to connect from {groupEP} at {DateTime.Now.ToShortTimeString()}\n");
                if (isSuccesful)
                {
                    Console.WriteLine($"Operation completed succesful!\n");
                }
                msg = "conect!";
                foreach (var m in members)
                {
                    try
                    {
                        if (m.iPEndPoint != server_Command.iPEndPoint)
                        {
                            server_Command.Message = msg;
                            byte[] byttes = ObjectToByteArray(server_Command);
                            server.Send(byttes, byttes.Length, m.iPEndPoint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error with {m.Name}: {ex.Message}\n");
                    }
                }
                foreach (var item in members)
                {
                    Console.WriteLine(item.iPEndPoint.Port + " " + item.iPEndPoint.Address);
                }
            }
            else if (msg == "<remove>")
            {
                isSuccesful = RemoveMember(server_Command);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Request to leave from {groupEP} at {DateTime.Now.ToShortTimeString()}\n");
                if (isSuccesful)
                {
                    Console.WriteLine($"Operation completed succesful!\n");
                }
                msg = "leave!";
                foreach (var m in members)
                {
                    try
                    {
                        //if (m.iPEndPoint != server_Command.iPEndPoint)
                        //{
                            server_Command.Message = msg;
                            byte[] byttes = ObjectToByteArray(server_Command);
                            server.Send(byttes, byttes.Length, m.iPEndPoint);
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error with {m.Name}: {ex.Message}\n");
                    }
                }
            }
            else
            {
                bool ok = false;
                foreach (var item in members)
                {
                    if (server_Command.iPEndPoint.Port == item.iPEndPoint.Port)
                    {
                        ok = true;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Message from {groupEP} at {DateTime.Now.ToShortTimeString()}: {server_Command.Message}\n");

                if (ok == true)
                {
                    foreach (var m in members)
                    {
                        try
                        {
                            byte[] byttes = ObjectToByteArray(server_Command);
                            server.Send(byttes, byttes.Length, m.iPEndPoint);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Error with {m.Name}: {ex.Message}\n");
                        }
                    }
                }
            }
            Console.ResetColor();
        }

        static bool AddMember(Server_Command server_Command)
        {
            //var member = members.FirstOrDefault(m => m.iPEndPoint.ToString() == server_Command.iPEndPoint.ToString());
            foreach (var item in members)
            {
                if (item.iPEndPoint == server_Command.iPEndPoint)
                    return false;
            }
            members.Add(server_Command);
            return true;
        }

        //static bool AddMember(IPEndPoint endPoint)
        //{
        //    var member = members.FirstOrDefault(m => m.ToString() == endPoint.ToString());
        //    if (member == null)
        //    {
        //        members.Add(endPoint);
        //        return true;
        //    }
        //    return false;


        //}
        static bool RemoveMember(Server_Command server_Command)
        {
            //var member = members.FirstOrDefault(m => m.iPEndPoint.ToString() == server_Command.iPEndPoint.ToString());
            foreach (var item in members)
            {
                if (item.iPEndPoint.Port == server_Command.iPEndPoint.Port)
                {
                    members.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
