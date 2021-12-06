using System;

namespace Semka2
{
    class KeyStr : IComparable<KeyStr> , IDataToFIle<KeyStr>
    {
        private string _keyVal;
        private const int KEY_MAX_LEN = 20;
        private const int FULL_MAX_LEN = KEY_MAX_LEN * sizeof(char);

        public KeyStr(string key)
        {
            _keyVal = key;
        }

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
            throw new NotImplementedException();
        }

        public KeyStr FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
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