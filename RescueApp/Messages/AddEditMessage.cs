using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Messages
{
    public class AddEditMessage<T> where T : WithID
    {
        public T Entity { get; set; }
        
        public AddEditMessage(T entity)
        {
            Entity = entity;
        }
    }
}
