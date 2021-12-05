using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
    class DummyClass : IDataToFIle<DummyClass>
    {
        private string _a1;
        private double _a2;
        private int _a3;
        private const int A1_MAX_LEN = 15;
        private const int A2_MAX_LEN = sizeof(Double);
        private const int A3_MAX_LEN = sizeof(Int32);
        private const int FULL_MAX_LEN = sizeof(Int32) + A1_MAX_LEN * sizeof(char) + A2_MAX_LEN + A3_MAX_LEN;
        public DummyClass(string p1, double p2, int p3)
        {
            _a1 = p1;
            _a2 = p2;
            _a3 = p3;
        }
        public DummyClass()
        {
            _a1 = "";
            _a2 = 0;
            _a3 = 0;
        }   

        public DummyClass FromByteArray(byte[] pArray)
        {
            var a1 = "";
            //System.Console.Write(BitConverter.ToInt32(pArray, 0) + " ");
            for (int i = 0; i < A1_MAX_LEN; i++)
                if (i >= A1_MAX_LEN - BitConverter.ToInt32(pArray, 0))
                    a1 = a1 + BitConverter.ToChar(pArray, sizeof(Int32) + i * sizeof(char));
            //System.Console.Write(" " + BitConverter.ToDouble(pArray, sizeof(Int32) + A1_MAX_LEN * sizeof(char)));
            var a2 = BitConverter.ToDouble(pArray, sizeof(Int32) + A1_MAX_LEN * sizeof(char));
            //System.Console.Write(" " + BitConverter.ToInt32(pArray, sizeof(Int32) + A1_MAX_LEN * sizeof(char) + sizeof(Double)));
            var a3 = BitConverter.ToInt32(pArray, sizeof(Int32) + A1_MAX_LEN * sizeof(char) + sizeof(Double));
            //System.Console.WriteLine();
            return new DummyClass(a1, a2, a3);
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }

        public byte[] ToByteArray()
        {
            var rt = new byte[0];
            rt = AddBytes(rt, BitConverter.GetBytes(_a1.Length));
            var a1 =_a1.PadLeft(A1_MAX_LEN);
            foreach (var ch in a1) {
                rt = AddBytes(rt, BitConverter.GetBytes(ch)); }
            rt = AddBytes(rt, BitConverter.GetBytes(_a2));
            rt = AddBytes(rt, BitConverter.GetBytes(_a3));
            return rt;
                //Encoding.ASCII.GetBytes(_a1.PadLeft(A1_LEN) + _a2.ToString("0000000000.0000000000000000000") + _a3.ToString(new String('0',A3_LEN)));
        }

        override public string ToString()
        {
            return _a1.Length + " " + _a1 + " " + _a2 + " " + _a3;
        }

        private byte[] AddBytes(byte[] pA, byte[] pB)
        {
            var z = new byte[pA.Length + pB.Length];
            pA.CopyTo(z, 0);
            pB.CopyTo(z, pA.Length);
            return z;
        }
    }
}
