using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyrannoDecoder;

namespace TyrannoDecoderAnsi
{
    class Program
    {
        static void Main(string[] args)
        {
            Coder c = new Coder();
            c.ActionSelector(args);
            Console.ReadKey();
        }
    }
}
