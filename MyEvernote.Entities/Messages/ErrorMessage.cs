using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.Messages
{
    public class ErrorMessage
    {
        public ErrorMessageCodes Code { get; set; }
        public string Message { get; set; }
    }
}
