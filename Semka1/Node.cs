using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Semka1
{
    public class T23Node<TKey,T> where TKey: IComparable<TKey>
    {
        private T _lData;
        private T _rData;
        private TKey _lKey;
        private TKey _rKey;

        private T23Node<TKey,T> _parent;
        private T23Node<TKey,T> _lChild;
        private T23Node<TKey,T> _mChild;
        private T23Node<TKey,T> _rChild;

        public T23Node() { _lChild = _mChild = _rChild = null; }
        public T23Node (T23Node<TKey,T> copyThis) {
            _lData = copyThis.GetLeftData(); _rData = copyThis.GetRightData(); _lKey = copyThis.GetLeftKey();
            _rKey = copyThis.GetRightKey(); _parent = copyThis.GetParent(); _lChild = copyThis.GetLeftChild();
            _mChild = copyThis.GetMiddleChild(); _rChild = copyThis.GetRightChild();
        }
        public T23Node (TKey keyL, T dataL) {
            _lData = dataL;_lKey = keyL;
        }
        public T23Node (TKey keyL, T dataL, ref T23Node<TKey,T> nParent) {
            _lData = dataL; _lKey = keyL; _parent = nParent;
        }//with parent
        public T23Node (TKey keyL, T dataL, TKey keyR, T dataR) {
            _lData = dataL; _rData = dataR; _lKey = keyL; _rKey = keyR;
        }
        public T23Node (TKey keyL, T dataL, TKey keyR, T dataR,
                     ref T23Node<TKey,T> childL, ref T23Node<TKey,T> childM, ref T23Node<TKey,T> childR) {
                     _lData = dataL; _rData = dataR; _lKey = keyL; _rKey = keyR;
                     _lChild = childL; _mChild = childM; _rChild = childR;
                 }
        public T23Node (TKey keyL, T dataL, TKey keyR, T dataR, ref T23Node<TKey,T> nParent,//with parent
            ref T23Node<TKey,T> childL, ref T23Node<TKey,T> childM, ref T23Node<TKey,T> childR) {
            _lData = dataL; _rData = dataR; _lKey = keyL; _rKey = keyR;
            _parent = nParent; _lChild = childL; _mChild = childM; _rChild = childR;
        }
        
        public TKey GetLeftKey() { return _lKey; }
        public T GetLeftData() { return _lData; }
        public ref T GetLeftDataRef() { return ref _lData; }
        public void SetLeft(TKey nKey, T nData) { _lKey = nKey; _lData = nData; }
        public TKey GetRightKey() { return _rKey; }
        public T GetRightData() { return _rData; }
        public ref T GetRightDataRef() { return ref _rData; }
        public void SetRight(TKey nKey, T nData) { _rKey = nKey; _rData = nData; }
        public void ClearRight() { _rKey = default(TKey); _rData = default(T); }
        public ref T23Node<TKey,T> GetParent() { return ref _parent; }
        public void SetParent(ref T23Node<TKey,T> newParent) { _parent = newParent; }
        public void ClearParent() { _parent = null; }
        public T23Node<TKey,T> GetLeftChild() { return _lChild; }
        public ref T23Node<TKey,T> GetLeftChildRef() { return ref _lChild; }
        public void SetLeftChild(T23Node<TKey,T> newVal) { _lChild = newVal; } 
        public void SetLeftChild(T23Node<TKey,T> newVal, ref T23Node<TKey,T> nPar) { _lChild = newVal; _lChild.SetParent(ref nPar); } 
        public T23Node<TKey,T> GetMiddleChild() { return _mChild; }
        public ref T23Node<TKey,T> GetMiddleChildRef() { return ref _mChild; }
        public void SetMiddleChild(T23Node<TKey,T> newVal) { _mChild = newVal; }
        public void SetMiddleChild(T23Node<TKey,T> newVal, ref T23Node<TKey,T> nPar) { _mChild = newVal; _mChild.SetParent(ref nPar); } 
        public T23Node<TKey,T> GetRightChild() { return _rChild; }
        public ref T23Node<TKey,T> GetRightChildRef() { return ref _rChild; }
        public void SetRightChild(T23Node<TKey,T> newVal) { _rChild = newVal; }
        public void SetRightChild(T23Node<TKey,T> newVal, ref T23Node<TKey,T> nPar) { _rChild = newVal; _rChild.SetParent(ref nPar); } 
        public int GetLevel()
        {
            int count = 0;
            var parent = _parent;
            while (parent!=null) {
                count++;
                parent = parent.GetParent();
            }
            return count;
        }
        public bool IsLeaf() { return _lChild == null; }

        public bool AddInCaseOfLeaf(TKey nKey, T nData) // true if success
        {
            if (_rData == null && _lChild != null) // not a 3node and a leaf
            {
                if (nKey.CompareTo(_lKey) < 0) // nkey < lkey
                {
                    SetRight(_lKey, _lData);
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
        public ref T23Node<TKey,T> NextNode(TKey nKey) // true if success
        {
            if (nKey.CompareTo(_lKey) < 0) return ref _lChild; // nkey < lkey
            else if (_rData != null && nKey.CompareTo(_rKey) < 0) return ref _mChild; // lkey < nkey < rkey
            else return ref _rChild; // rkey < nkey
            
            throw new Exception("in: node: nextnode()");
        }
        public override string ToString()
        {
            string ret = " ";
            if (_lData != null)
            {
                ret += _lData.ToString();
            }
            else
            {
                ret += "----";
            }
            ret += " , ";
            if (_rData != null)
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
            return _rData != null;
        }
        public void ResetExceptParent()
        {
            _lChild = null; _rChild = null; _mChild = null;
            _lKey = default(TKey); _lData = default(T); ClearRight();
        }
        public T GetData(TKey key)
        {
            if (key.CompareTo(_lKey) == 0) return _lData;
            return _rData;
        }
    }
}