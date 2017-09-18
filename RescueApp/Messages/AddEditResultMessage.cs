using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Messages
{
    public class AddEditResultMessage<T>
    {
        public Exception Error { get; set; }
        public T Entity { get; set; }

        public AddEditResultMessage(Exception err, T entity)
        {
            Error = err;
            Entity = entity;
        }
    }
}
