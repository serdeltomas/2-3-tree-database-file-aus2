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
        public static byte[] SubArray(this byte[] array, int offset, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(array, offset, result, 0, length);
            return result;
        }
    }
}
