using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Messages
{
    public class AddEditMessage<T> where T : HasId
    {
        public T Entity { get; set; }
        public Action SuccessAction { get; set; }

        public AddEditMessage(T entity, Action successAction)
        {
            Entity = entity;
            SuccessAction = successAction;
        }
    }
}
