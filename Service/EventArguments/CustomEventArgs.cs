using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.EventArguments
{
    public class CustomEventArgs : EventArgs
    {
        public string Message { get; set; }

        public CustomEventArgs(string message)
        {
            Message = message;
        }
    }
}
