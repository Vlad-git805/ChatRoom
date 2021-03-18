using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComandClasses
{
    //public enum CommandType
    //{
    //    REMOVE,   
    //    CONNECT,    
    //    SEND_MESSAGE,   
    //    CREATE_ROOM,
    //}
    [Serializable]
    public class Client_Command
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
    [Serializable]
    public class Server_Command
    {
        public IPEndPoint iPEndPoint { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }

    //public class ServerCommand
    //{
    //    public CommandType Type { get; set; }
    //    public string PersonName { get; set; }

    //    public ServerCommand(CommandType type)
    //    {
    //        this.Type = type;
    //    }
    //}
}
