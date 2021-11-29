using System;

namespace Semka2
{
    class KeyStr : IComparable<KeyStr>
    {
        private string _keyVal;

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

    }
    class KeyInt : IComparable<KeyInt>
    {
        private int _keyVal;

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

    }
    class KeyDat : IComparable<KeyDat>
    {
        private DateTime _keyVal;
        private int _keyValPlus;

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

    }
    class KeyRodCis : IComparable<KeyRodCis>
    {
        private string _keyVal;
        private int _keyValPlus;

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

    }
}