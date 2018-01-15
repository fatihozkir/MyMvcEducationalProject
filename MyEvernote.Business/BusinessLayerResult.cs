using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Entities.Messages;

namespace MyEvernote.Business
{
    public class BusinessLayerResult<T> where T:class
    {
        public BusinessLayerResult()
        {
            Errors=new List<ErrorMessage>();
        }
        public List<ErrorMessage> Errors { get; set; }
        public T Result { get; set; }

        public void AddError(ErrorMessageCodes code, string message)
        {
            Errors.Add(new ErrorMessage{Code = code,Message = message});
        }
    }
}
