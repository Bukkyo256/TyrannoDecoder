using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyrannoDecoder;

namespace TyrannoDecoderAscii
{
    class Program
    {
        static void Main(string[] args)
        {
            Coder c = new Coder(Encoding.ASCII);
            c.ActionSelector(args);
        }
    }
}
