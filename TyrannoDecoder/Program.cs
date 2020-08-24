using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TyrannoDecoder
{
    public class Program
    {
        public Program()
        {

        }

        static void Main(string[] args)
        {
            Coder c = new Coder();
            c.ActionSelector(args);
            Console.ReadKey();
        }
    }
}
