using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Semka2
{
    public class TwoThreeNode<TKey,T> where TKey: IComparable<TKey>
    {
        private T _lData;
        private T _rData;
        private TKey _lKey;
        private TKey _rKey;

        private TwoThreeNode<TKey,T> _parent;
        private TwoThreeNode<TKey,T> _lChild;
        private TwoThreeNode<TKey,T> _mChild;
        private TwoThreeNode<TKey,T> _rChild;

        public TwoThreeNode() { _lChild = _mChild = _rChild = null; }
        public TwoThreeNode (TwoThreeNode<TKey,T> copyThis) {
            _lData = copyThis.GetLeftData(); _rData = copyThis.GetRightData(); _lKey = copyThis.GetLeftKey();
            _rKey = copyThis.GetRightKey(); _parent = copyThis.GetParent(); _lChild = copyThis.GetLeftChild();
            _mChild = copyThis.GetMiddleChild(); _rChild = copyThis.GetRightChild();
        }
        public TwoThreeNode (TKey keyL, T dataL) {
            _lData = dataL;_lKey = keyL;
        }
        public TwoThreeNode (TKey keyL, T dataL, ref TwoThreeNode<TKey,T> nParent) {
            _lData = dataL; _lKey = keyL; _parent = nParent;
        }//with parent
        public TwoThreeNode (TKey keyL, T dataL, TKey keyR, T dataR) {
            _lData = dataL; _rData = dataR; _lKey = keyL; _rKey = keyR;
        }
        public TwoThreeNode (TKey keyL, T dataL, TKey keyR, T dataR,
                     ref TwoThreeNode<TKey,T> childL, ref TwoThreeNode<TKey,T> childM, ref TwoThreeNode<TKey,T> childR) {
                     _lData = dataL; _rData = dataR; _lKey = keyL; _rKey = keyR;
                     _lChild = childL; _mChild = childM; _rChild = childR;
                 }
        public TwoThreeNode (TKey keyL, T dataL, TKey keyR, T dataR, ref TwoThreeNode<TKey,T> nParent,//with parent
            ref TwoThreeNode<TKey,T> childL, ref TwoThreeNode<TKey,T> childM, ref TwoThreeNode<TKey,T> childR) {
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
        public ref TwoThreeNode<TKey,T> GetParent() { return ref _parent; }
        public void SetParent(ref TwoThreeNode<TKey,T> newParent) { _parent = newParent; }
        public void ClearParent() { _parent = null; }
        public TwoThreeNode<TKey,T> GetLeftChild() { return _lChild; }
        public ref TwoThreeNode<TKey,T> GetLeftChildRef() { return ref _lChild; }
        public void SetLeftChild(TwoThreeNode<TKey,T> newVal) { _lChild = newVal; } 
        public void SetLeftChild(TwoThreeNode<TKey,T> newVal, ref TwoThreeNode<TKey,T> nPar) { _lChild = newVal; _lChild.SetParent(ref nPar); } 
        public TwoThreeNode<TKey,T> GetMiddleChild() { return _mChild; }
        public ref TwoThreeNode<TKey,T> GetMiddleChildRef() { return ref _mChild; }
        public void SetMiddleChild(TwoThreeNode<TKey,T> newVal) { _mChild = newVal; }
        public void SetMiddleChild(TwoThreeNode<TKey,T> newVal, ref TwoThreeNode<TKey,T> nPar) { _mChild = newVal; _mChild.SetParent(ref nPar); } 
        public TwoThreeNode<TKey,T> GetRightChild() { return _rChild; }
        public ref TwoThreeNode<TKey,T> GetRightChildRef() { return ref _rChild; }
        public void SetRightChild(TwoThreeNode<TKey,T> newVal) { _rChild = newVal; }
        public void SetRightChild(TwoThreeNode<TKey,T> newVal, ref TwoThreeNode<TKey,T> nPar) { _rChild = newVal; _rChild.SetParent(ref nPar); } 
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
        public ref TwoThreeNode<TKey,T> NextNode(TKey nKey) // true if success
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