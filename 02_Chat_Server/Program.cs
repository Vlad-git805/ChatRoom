using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private static List<IPEndPoint> members = new List<IPEndPoint>();

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
                    Console.WriteLine("\tWaiting for a message...");
                    byte[] bytes = server.Receive(ref groupEP);

                    Task.Run(() => Client_server(bytes/*, semaphore*/));
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

        static void Client_server(byte[] bytes/*, Semaphore semaphore*/)
        {
            string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            char[] wordsSplit = new char[] { ' ' };
            string[] words = msg.Split(wordsSplit, StringSplitOptions.RemoveEmptyEntries);
            string name = words[0];

            //if (!semaphore.WaitOne(200))
            //{
            //    string str = name + " Wheit please a fiew minutes to connect";
            //    byte[] byt = Encoding.ASCII.GetBytes(str);
            //    server.Send(byt, byt.Length, groupEP);
            //    return;
            //}

            bool isSuccesful;
            if (msg == name + " <connect>")
            {
                //if(members.Count+1 > 2)
                //{
                //    string str = name + " Wheit please a fiew minutes to connect";
                //    byte[] byt = Encoding.ASCII.GetBytes(str);
                //    server.Send(byt, byt.Length, groupEP);
                //    return;
                //}

                isSuccesful = AddMember(groupEP);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Request to connect from {groupEP} at {DateTime.Now.ToShortTimeString()}\n");
                if (isSuccesful)
                {
                    Console.WriteLine($"Operation completed succesful!\n");
                }
                msg = name + " conect!";
                byte[] bytess = Encoding.ASCII.GetBytes(msg);
                foreach (var m in members)
                {
                    try
                    {
                        if (m != groupEP)
                            server.Send(bytess, bytess.Length, m);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error with {m}: {ex.Message}\n");
                    }
                }
                foreach (var item in members)
                {
                    Console.WriteLine(item.Port + " " + item.Address);
                }
            }
            else if (msg == name + " <remove>")
            {
                isSuccesful = RemoveMember(groupEP);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Request to leave from {groupEP} at {DateTime.Now.ToShortTimeString()}\n");
                if (isSuccesful)
                {
                    Console.WriteLine($"Operation completed succesful!\n");
                }
                msg = name + " leave!";
                byte[] bytess = Encoding.ASCII.GetBytes(msg);
                foreach (var m in members)
                {
                    try
                    {
                        if (m != groupEP)
                            server.Send(bytess, bytess.Length, m);
                        //semaphore.Release();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error with {m}: {ex.Message}\n");
                    }
                }
            }
            else
            {
                bool ok = false;
                foreach (var item in members)
                {
                    if(groupEP.Port == item.Port)
                    {
                        ok = true;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Message from {groupEP} at {DateTime.Now.ToShortTimeString()}: {msg}\n");

                if (ok == true)
                {
                    foreach (var m in members)
                    {
                        try
                        {
                            server.Send(bytes, bytes.Length, m);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Error with {m}: {ex.Message}\n");
                        }
                    }
                }
            }
            Console.ResetColor();
        }

        static bool AddMember(IPEndPoint endPoint)
        {
            var member = members.FirstOrDefault(m => m.ToString() == endPoint.ToString());
            if (member == null)
            {
                members.Add(endPoint);
                return true;
            }
            return false;


        }
        static bool RemoveMember(IPEndPoint endPoint)
        {
            var member = members.FirstOrDefault(m => m.ToString() == endPoint.ToString());
            if (member != null)
            {
                members.Remove(member);
                return true;
            }
            return false;
        }
    }
}
