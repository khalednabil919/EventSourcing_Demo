using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing_Demo
{
    public static class IsValidString
    {
        public static bool IsValid(this string s)
        {
            foreach (char c in s)
            {
                if (!((int)c >= 48 && (int)c <= 57))
                    return false;
            }
            return true;
        }
        public static int Quantity(this string s)
        {
            return Convert.ToInt32(s);
        }
    }
}
