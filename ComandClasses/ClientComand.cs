using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComandClasses
{
    [Serializable]
    public class Client_Command
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string From_who_message { get; set; }
        public string To_who_message { get; set; }
    }
    [Serializable]
    public class Server_Command
    {
        public IPEndPoint iPEndPoint { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string To_who_message { get; set; }
    }
}
