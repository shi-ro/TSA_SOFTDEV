using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public static class Extentions
    {
        public static int[] ToIntArray(this string str)
        {
            int[] it;
            try
            {
                string[] sp = str.Split(',');
                it = new int[sp.Length];
                for(int i = 0; i < it.Length; i++)
                {
                    it[i] = Int32.Parse(sp[i]);
                }
                
            } catch
            {
                return null;
            }
            return it;
        }
        public static string[] ToStringArray(this string str)
        {
            try
            {
                string[] sp = str.Split(',');
                return sp;
            }
            catch
            {
                return null;
            }
        }
    }
}
