using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Semka2
{
    public class T23Node<TKey,T> : IDataToFIle<T23Node<TKey, T>> where TKey: IComparable<TKey>, IDataToFIle<TKey> where T : IDataToFIle<T> 
    {
        private FIleHandler<T> _dataFile;
        private long _lData;
        private long _rData;

        private FIleHandler<TKey> _keysFile;
        private long _lKey;
        private long _rKey;

        private FIleHandler<T23Node<TKey, T>> _nodesFile;
        private long _parent;
        private long _lChild;
        private long _mChild;
        private long _rChild;

        private const int FULL_MAX_LEN = 8*sizeof(long);

        public T23Node (TKey keyL, T dataL) {
            _lData = _dataFile.InsertToFile(dataL) ;_lKey = _keysFile.InsertToFile(keyL);
        }
        public T23Node (TKey keyL, T dataL, T23Node<TKey,T> nParent) {
            _lData = _dataFile.InsertToFile(dataL); _lKey = _keysFile.InsertToFile(keyL); _parent = _nodesFile.InsertToFile(nParent);
        }//with parent
        public TKey GetLeftKey() { return _keysFile.ReadFromFile(_lKey); }
        public T GetLeftData() { return _dataFile.ReadFromFile(_lData); }
        public void SetLeft(TKey nKey, T nData) { _lKey = _keysFile.InsertToFile(nKey); _lData = _dataFile.InsertToFile(nData); }
        public void SetLeft(long nKey, long nData) { _lKey = nKey; _lData = nData; }
        public TKey GetRightKey() { return _keysFile.ReadFromFile(_rKey); }
        public T GetRightData() { return _dataFile.ReadFromFile(_rData); }
        public void SetRight(TKey nKey, T nData) { _rKey = _keysFile.InsertToFile(nKey); _rData = _dataFile.InsertToFile(nData); }
        public void SetRight(long nKey, long nData) { _rKey = nKey; _rData = nData; }
        public void ClearRight() { _rKey = -1; _rData = -1; }
        public T23Node<TKey,T> GetParent() { return _nodesFile.ReadFromFile(_parent); }
        public long GetParentPos() { return _parent; }
        public void ClearParent() { _parent = -1; }
        public T23Node<TKey,T> GetLeftChild() { return _nodesFile.ReadFromFile(_lChild); }
        public void SetLeftChild(T23Node<TKey, T> newVal) { _lChild = _nodesFile.InsertToFile(newVal); }
        public void SetLeftChild(long newVal) { _lChild = newVal; }
        public void SetLeftChild(T23Node<TKey,T> newVal, T23Node<TKey,T> nPar) { _lChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_lChild).SetParent(nPar); }
        public void SetLeftChild(long newVal, long nPar) { _lChild = newVal; _nodesFile.ReadFromFile(_lChild).SetParent(nPar); }
        public T23Node<TKey,T> GetMiddleChild() { return _nodesFile.ReadFromFile(_mChild); }
        public void SetMiddleChild(T23Node<TKey,T> newVal) { _mChild = _nodesFile.InsertToFile(newVal); }
        public void SetMiddleChild(long newVal) { _mChild = newVal; }
        public void SetMiddleChild(T23Node<TKey,T> newVal, T23Node<TKey,T> nPar) { _mChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_mChild).SetParent(nPar); }
        public void SetMiddleChild(long newVal, long nPar) { _mChild = newVal; _nodesFile.ReadFromFile(_mChild).SetParent(nPar); }
        private void SetParent(T23Node<TKey, T> nPar) { _parent = _nodesFile.InsertToFile(nPar); }
        private void SetParent(long nPar) { _parent =nPar; }
        public T23Node<TKey,T> GetRightChild() { return _nodesFile.ReadFromFile(_rChild); }
        public void SetRightChild(T23Node<TKey,T> newVal) { _rChild = _nodesFile.InsertToFile(newVal); }
        public void SetRightChild(long newVal) { _rChild = newVal; }
        public void SetRightChild(T23Node<TKey,T> newVal, T23Node<TKey,T> nPar) { _rChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_rChild).SetParent(nPar); }
        public void SetRightChild(long newVal, long nPar) { _rChild = newVal; _nodesFile.ReadFromFile(_rChild).SetParent(nPar); }
        public int GetLevel()
        {
            int count = 0;
            var parent = _parent;
            while (parent!=-1) {
                count++;
                parent = _nodesFile.ReadFromFile(parent).GetParentPos();
            }
            return count;
        }
        public bool IsLeaf() { return _lChild == -1; }
        public bool AddInCaseOfLeaf(TKey nKey, T nData) // true if success
        {
            if (_rData == -1 && _lChild != -1) // not a 3node and a leaf
            {
                if (nKey.CompareTo(_keysFile.ReadFromFile(_lKey)) < 0) // nkey < lkey
                {
                    SetRight(_keysFile.ReadFromFile(_lKey), _dataFile.ReadFromFile(_lData));
                    SetLeft(nKey, nData);
                    return true;
                }
                else // lkey < nkey
                {
                    SetRight(nKey, nData);
                    return true;
                }
            }
            else return false;
        }
        public T23Node<TKey,T> NextNode(TKey nKey) // true if success
        {
            if (nKey.CompareTo(_keysFile.ReadFromFile(_lKey)) < 0) return _nodesFile.ReadFromFile(_lChild); // nkey < lkey
            else if (_rData != -1 && nKey.CompareTo(_keysFile.ReadFromFile(_rKey)) < 0) return _nodesFile.ReadFromFile(_mChild); // lkey < nkey < rkey
            else return _nodesFile.ReadFromFile(_rChild); // rkey < nkey
            
            throw new Exception("in: node: nextnode()");
        }
        public override string ToString()
        {
            string ret = " ";
            if (_lData != -1)
            {
                ret += _lData.ToString();
            }
            else
            {
                ret += "----";
            }
            ret += " , ";
            if (_rData != -1)
            {
                ret += _rData.ToString();
            }
            else
            {
                ret += "----";
            }
            ret += " | ";
            return ret;
        }
        public string VypisVsetko()
        {
            if (!Is3Node()) return _lData.ToString() + "/n";
            return _lData.ToString() + "/n" + _rData.ToString() + "/n";
        }
        public bool Is3Node()
        {
            return _rData != -1;
        }
        public void ResetExceptParent()
        {
            _lChild = -1; _rChild = -1; _mChild = -1;
            _lKey = -1; _lData = -1; ClearRight();
        }
        public T GetData(TKey key)
        {
            if (key.CompareTo(_keysFile.ReadFromFile(_lKey)) == 0) return _dataFile.ReadFromFile(_lData);
            return _dataFile.ReadFromFile(_rData);
        }
        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }
        public T23Node<TKey, T> FromByteArray(byte[] pArray)
        {
            throw new NotImplementedException();
        }
        public int Size()
        {
            throw new NotImplementedException();
        }
    }
}