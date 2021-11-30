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
        private string _a2;
        private int _a3;
        private const int A1_LEN = 15;
        private const int A2_LEN = 15;
        private const int A3_LEN = 10;
        private const int FULL_LEN = A1_LEN + A2_LEN + A3_LEN;
        public DummyClass(string p1, string p2, int p3)
        {
            _a1 = p1;
            _a2 = p2;
            _a3 = p3;
        }
        public DummyClass()
        {
            _a1 = "";
            _a2 = "";
            _a3 = 0;
        }   

        public DummyClass FromByteArray(byte[] pArray)
        {
            var sToCut = Encoding.ASCII.GetString(pArray);
            var a1 = sToCut.Substring(0, A1_LEN).Trim();
            var a2 = sToCut.Substring(A1_LEN, A2_LEN).Trim();
            var a3 = sToCut.Substring(A1_LEN + A2_LEN, A3_LEN);
            return new DummyClass(a1, a2, int.Parse(a3));
        }

        public int Size()
        {
            return FULL_LEN;
        }

        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(_a1.PadLeft(A1_LEN) + _a2.PadLeft(A2_LEN) + _a3.ToString(new String('0',A3_LEN)));
        }

        override public string ToString()
        {
            return _a1 + " " + _a2 + " " + _a3;
        }
    }
}
