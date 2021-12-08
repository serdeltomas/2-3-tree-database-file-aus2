using System;

namespace Semka2
{
    class KeyStr : IComparable<KeyStr> , IDataToFIle<KeyStr>
    {
        private string _keyVal;
        private const int KEY_MAX_LEN = 20;
        private const int FULL_MAX_LEN = sizeof(int) + (KEY_MAX_LEN + 1) * sizeof(char);

        public KeyStr(string key)
        {
            _keyVal = key;
        }
        public KeyStr() { }

        public string GetKey()
        {
            return _keyVal;
        }

        public int CompareTo(KeyStr? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(_keyVal, other._keyVal, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return _keyVal;
        }

        public byte[] ToByteArray()
        {
            var rt = new byte[0];
            //meno
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_keyVal.Length));
            var a1 = _keyVal.PadLeft(KEY_MAX_LEN);
            foreach (var ch in a1) { rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(ch)); }
            //newline
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes('\n'));
            return rt;
        }

        public KeyStr FromByteArray(byte[] pArray)
        {
            var a1 = "";
            for (int i = 0; i < KEY_MAX_LEN; i++)
                if (i >= KEY_MAX_LEN - BitConverter.ToInt32(pArray, 0))
                    a1 = a1 + BitConverter.ToChar(pArray, sizeof(Int32) + i * sizeof(char));
            return new KeyStr(a1);
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }
    }
    class KeyInt : IComparable<KeyInt>, IDataToFIle<KeyInt>
    {
        private int _keyVal;
        private const int FULL_MAX_LEN = sizeof(int);

        public KeyInt(int key)
        {
            _keyVal = key;
        }

        public int GetKey()
        {
            return _keyVal;
        }

        public int CompareTo(KeyInt? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _keyVal.CompareTo(other._keyVal);
        }

        public override string ToString()
        {
            return _keyVal.ToString();
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public KeyInt FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }
    }
    class KeyDat : IComparable<KeyDat>, IDataToFIle<KeyDat>
    {
        private DateTime _keyVal;
        private int _keyValPlus;
        private const int KEY_MAX_LEN = 20;
        private const int FULL_MAX_LEN = KEY_MAX_LEN + sizeof(int);

        public KeyDat(DateTime key, int keyPlus)
        {
            _keyVal = key;
            _keyValPlus = keyPlus;
        }

        public DateTime GetKey()
        {
            return _keyVal;
        }

        public int CompareTo(KeyDat? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            if (_keyVal.CompareTo(other._keyVal) == 0) return _keyValPlus.CompareTo(other._keyValPlus);
            return _keyVal.CompareTo(other._keyVal);

        }

        public override string ToString()
        {
            return _keyVal.ToString();
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public KeyDat FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }
    }
    class KeyRodCis : IComparable<KeyRodCis>, IDataToFIle<KeyRodCis>
    {
        private string _keyVal;
        private int _keyValPlus;
        private const int KEY_MAX_LEN = 10;
        private const int FULL_MAX_LEN = KEY_MAX_LEN * sizeof(char) + sizeof(int);

        public KeyRodCis(string key, int keyPlus)
        {
            _keyVal = key;
            _keyValPlus = keyPlus;
        }

        public string GetKey()
        {
            return _keyVal;
        }

        public int CompareTo(KeyRodCis? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            if (_keyVal.CompareTo(other._keyVal) == 0) return _keyValPlus.CompareTo(other._keyValPlus);
            return _keyVal.CompareTo(other._keyVal);

        }

        public override string ToString()
        {
            return _keyVal.ToString();
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public KeyRodCis FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
        }

        public int Size()
        {
            return FULL_MAX_LEN;
        }
    }
}