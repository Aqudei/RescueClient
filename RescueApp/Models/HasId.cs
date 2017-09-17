using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public abstract class HasId
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
