using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ValidationConfig.Configure();  // Define as regras uma vez
        
            Terminal terminal = new Terminal();
            terminal.Start();
        }
    }
}