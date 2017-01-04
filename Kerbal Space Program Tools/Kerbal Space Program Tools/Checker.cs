using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kerbal_Space_Program_Tools
{
    public static class WindowCheck
    {
        private static bool opencheck;
        public static bool getOpencheck()
        {
            return opencheck;
        }
        public static void setOpencheck(bool x)
        {
            opencheck = x;
        }
    }
}
