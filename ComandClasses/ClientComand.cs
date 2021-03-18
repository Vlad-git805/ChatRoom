using System;
using System.Collections.Generic;
using System.Linq;
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

    public class Client_Server_Command
    {
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
