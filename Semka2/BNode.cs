using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Semka2
{
    public class BNode<TKey,T> : IDataToFIle<BNode<TKey, T>> where TKey: IComparable<TKey>, IDataToFIle<TKey> where T : IDataToFIle<T> 
    {
        private long _pos = -1;

        private FIleHandler<TKey> _keysFile;
        private long _lKey = -1;
        private long _rKey = -1;

        private FIleHandler<T> _dataFile;
        private long _lData = -1;
        private long _rData = -1;

        private FIleHandler<BNode<TKey, T>> _nodesFile;
        private long _parent = -1;
        private long _lChild = -1;
        private long _mChild = -1;
        private long _rChild = -1;

        private const int FULL_MAX_LEN = 9*sizeof(long) + sizeof(char);

        public BNode() { }
        public BNode ( long keyL, long dataL, FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, FIleHandler<BNode<TKey, T>> pNodesFile, long pPos) {
            _pos = pPos; _lData = dataL; _lKey = keyL; _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = pNodesFile; 
        }
        public BNode (long keyL, long dataL, long nParent, FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, FIleHandler<BNode<TKey, T>> pNodesFile, long pPos) {
            _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = pNodesFile;
            _pos = pPos; _lData = dataL; _lKey = keyL; _parent = nParent;
        }//with parent
        public BNode (long pPos, long pLkey, long pRkey, long pLdata, long pRdata, long pParent, long pLchild, long pMchild, long pRchild
            , FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, FIleHandler<BNode<TKey, T>> pNodesFile) {
            _pos = pPos; _lKey = pLkey; _rKey = pRkey; _lData = pLdata; _rData = pRdata; _parent = pParent; _lChild = pLchild; _mChild = pMchild; _rChild = pRchild;
            _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = pNodesFile;
        }
        public BNode(FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, FIleHandler<BNode<TKey, T>> pNodesFile) {
            _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = pNodesFile;
        }
        public void AddFiles(FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, FIleHandler<BNode<TKey, T>> pNodesFile) {
            _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = pNodesFile; }
        public void SetPos(long nPos) { _pos = nPos; }
        public long GetPos() { return _pos; }
        public TKey GetLeftKey() { return _lKey != -1 ? _keysFile.ReadFromFile(_lKey) : default; }
        public long GetLeftKeyPos() { return _lKey; }
        public T GetLeftData() { return _lData != -1 ? _dataFile.ReadFromFile(_lData) : default; }
        public long GetLeftDataPos() { return _lData; }
        //public void SetLeft(TKey nKey, T nData) { _lKey = _keysFile.InsertToFile(nKey); _lData = _dataFile.InsertToFile(nData); }
        public void SetLeft(long nKey, long nData) { _lKey = nKey; _lData = nData; }
        public TKey GetRightKey() { return _rKey != -1 ? _keysFile.ReadFromFile(_rKey) : default; }
        public long GetRightKeyPos() { return _rKey; }
        public T GetRightData() { return _rData != -1 ? _dataFile.ReadFromFile(_rData): default; }
        public long GetRightDataPos() { return _rData; }
        //public void SetRight(TKey nKey, T nData) { _rKey = _keysFile.InsertToFile(nKey); _rData = _dataFile.InsertToFile(nData); }
        public void SetRight(long nKey, long nData) { _rKey = nKey; _rData = nData; }
        public void ClearRight() { _rKey = -1; _rData = -1; }
        public BNode<TKey,T> GetParent() { return _parent != -1 ? _nodesFile.ReadFromFile(_parent) : null; }
        public long GetParentPos() { return _parent; }
        public void ClearParent() { _parent = -1; }
        public BNode<TKey,T> GetLeftChild() { return _lChild != -1 ? _nodesFile.ReadFromFile(_lChild) : null; }
        public long GetLeftChildPos() { return _lChild; }
        //public void SetLeftChild(BNode<TKey, T> newVal) { _lChild = _nodesFile.InsertToFile(newVal); }
        public void SetLeftChild(long newVal) { _lChild = newVal; if (_lChild != -1) _nodesFile.UpdateInFIle(this, _pos); }
        //public void SetLeftChild(BNode<TKey,T> newVal, BNode<TKey,T> nPar) { _lChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_lChild).SetParent(nPar); }
        public void SetLeftChild(long newVal, long nPar) { _lChild = newVal; if (_lChild != -1) { _nodesFile.UpdateInFIle(this,_pos); _nodesFile.ReadFromFile(_lChild).SetParent(nPar); } }
        public BNode<TKey,T> GetMiddleChild() { return _mChild != -1 ? _nodesFile.ReadFromFile(_mChild) : null; }
        public long GetMiddleChildPos() { return _mChild; }
        //public void SetMiddleChild(BNode<TKey,T> newVal) { _mChild = _nodesFile.InsertToFile(newVal); }
        public void SetMiddleChild(long newVal) { _mChild = newVal; if (_mChild != -1) _nodesFile.UpdateInFIle(this, _pos); }
        //public void SetMiddleChild(BNode<TKey,T> newVal, BNode<TKey,T> nPar) { _mChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_mChild).SetParent(nPar); }
        public void SetMiddleChild(long newVal, long nPar) { _mChild = newVal; if (_mChild != -1) { _nodesFile.UpdateInFIle(this, _pos); _nodesFile.ReadFromFile(_mChild).SetParent(nPar); } }
        //private void SetParent(BNode<TKey, T> nPar) { _parent = _nodesFile.InsertToFile(nPar); }
        private void SetParent(long nPar) { _parent =nPar; if (_parent != -1) _nodesFile.UpdateInFIle(this, _pos); }
        public BNode<TKey,T> GetRightChild() { return _rChild != -1 ? _nodesFile.ReadFromFile(_rChild) : null; }
        public long GetRightChildPos() { return _rChild; }
        //public void SetRightChild(BNode<TKey,T> newVal) { _rChild = _nodesFile.InsertToFile(newVal); }
        public void SetRightChild(long newVal) { _rChild = newVal; if (_rChild != -1) _nodesFile.UpdateInFIle(this, _pos); }
        //public void SetRightChild(BNode<TKey,T> newVal, BNode<TKey,T> nPar) { _rChild = _nodesFile.InsertToFile(newVal); _nodesFile.ReadFromFile(_rChild).SetParent(nPar); }
        public void SetRightChild(long newVal, long nPar) { _rChild = newVal; if (_rChild != -1) { _nodesFile.UpdateInFIle(this, _pos); _nodesFile.ReadFromFile(_rChild).SetParent(nPar); } }
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
        public bool IsLeaf() { return _lChild == -1 && _rChild == -1; }
        public BNode<TKey,T> NextNode(TKey nKey) // true if success
        {
            if (nKey.CompareTo(_keysFile.ReadFromFile(_lKey)) < 0) return _nodesFile.ReadFromFile(_lChild); // nkey < lkey
            else if (_rData != -1 && nKey.CompareTo(_keysFile.ReadFromFile(_rKey)) < 0) return _nodesFile.ReadFromFile(_mChild); // lkey < nkey < rkey
            else return _nodesFile.ReadFromFile(_rChild); // rkey < nkey
            
            throw new Exception("in: node: nextnode()");
        }
        public override string ToString()
        {
            /*string ret = "";
            ret += _pos + "   ";
            ret += _lKey + " ";
            ret += _lData + " ";
            ret += _rKey + " ";
            ret += _rData + "   ";
            ret += _parent + " ";
            ret += _lChild + " ";
            ret += _mChild + " ";
            ret += _rChild + "\n";
            return ret;*/
            string ret = " ";
            if (_lData != -1)
            {
                ret += _dataFile.ReadFromFile(_lData);
            }
            else
            {
                ret += "----";
            }
            ret += " , ";
            if (_rData != -1)
            {
                ret += _dataFile.ReadFromFile(_rData);
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
            var rt = new byte[0];
            //pos
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_pos));
            //keys
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_lKey));
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_rKey));
            //data
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_lData));
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_rData));
            //nodes
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_parent));
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_lChild));
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_mChild));
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes(_rChild));
            //newline
            rt = BytesAdder.AddBytes(rt, BitConverter.GetBytes('\n'));
            return rt;
        }
        public BNode<TKey, T> FromByteArray(byte[] pArray)
        {
            //pos
            var pos = BitConverter.ToInt64(pArray, 0);
            //keys
            var key1 = BitConverter.ToInt64(pArray, sizeof(long));
            var key2 = BitConverter.ToInt64(pArray, 2 * sizeof(long));
            //data
            var data1 = BitConverter.ToInt64(pArray, 3 * sizeof(long));
            var data2 = BitConverter.ToInt64(pArray, 4 * sizeof(long));
            //nodes
            var node1 = BitConverter.ToInt64(pArray, 5 * sizeof(long));
            var node2 = BitConverter.ToInt64(pArray, 6 * sizeof(long));
            var node3 = BitConverter.ToInt64(pArray, 7 * sizeof(long));
            var node4 = BitConverter.ToInt64(pArray, 8 * sizeof(long));
            //return
            return new BNode<TKey, T>(pos, key1, key2, data1, data2, node1, node2, node3, node4, _keysFile, _dataFile, _nodesFile);
        }
        public int Size()
        {
            return FULL_MAX_LEN;
        }
    }
}