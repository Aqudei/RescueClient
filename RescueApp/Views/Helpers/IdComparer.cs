using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Helpers
{
    public class IdComparer<T> : IEqualityComparer<T> where T:WithID
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }
        
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
