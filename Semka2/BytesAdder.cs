using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
    static class BytesAdder
    {
        public static byte[] AddBytes(byte[] pA, byte[] pB)
        {
            var z = new byte[pA.Length + pB.Length];
            pA.CopyTo(z, 0);
            pB.CopyTo(z, pA.Length);
            return z;
        }
    }
}
