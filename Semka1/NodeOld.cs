using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace Semka1
{
    public class T23Node<T> where T: IComparable<T>
    {
        public T _data1 { get; set; }
        public T _data2 { get; set; }
        public T _extraData { get; set; }
        
        private T23Node<T> _parent;
        private T23Node<T> _leftChild;
        private T23Node<T> _middleChild;
        private T23Node<T> _middleLeftExtraChild;
        private T23Node<T> _middleRightExtraChild;
        private T23Node<T> _rightChild;
        
        public bool _is3Node { get; set; }
        public bool _isLeaf { get; set; }

        public T23Node (){}
        public T23Node (T23Node<T> copyThis)
        {
            _data1 = copyThis._data1;
            _data2 = copyThis._data2;
            _extraData = copyThis._extraData;
            _parent = copyThis._parent;
            _leftChild = copyThis._leftChild;
            _rightChild = copyThis._rightChild;
            _middleChild = copyThis._middleChild;
            _middleLeftExtraChild = copyThis._middleLeftExtraChild;
            _middleRightExtraChild = copyThis._middleRightExtraChild;
            _is3Node = copyThis._is3Node;
            _isLeaf = copyThis._isLeaf;
        }
        public T23Node (bool isLeaf, bool is3Node)
        {
            _isLeaf = isLeaf;
            _is3Node = is3Node;
        }
        public T23Node (bool isLeaf, bool is3Node, T data1)
        {
            _isLeaf = isLeaf;
            _is3Node = is3Node;
            _data1 = data1;
        }
        public T23Node (bool isLeaf, bool is3Node, T data1, ref T23Node<T> parent)
        {
            _isLeaf = isLeaf;
            _is3Node = is3Node;
            _data1 = data1;
            _parent = parent;
        }
        
        public ref T23Node<T> GetParent()
        {
            return ref _parent;
        }
        public void SetParent(ref T23Node<T> newParent)
        {
            _parent = newParent;
        }
        public ref T23Node<T> GetRightChild()
        {
            return ref _rightChild;
        }
        public void SetRightChild(ref T23Node<T> newVal)
        {
            _rightChild = newVal;
        }
        public void SetRightChild(ref T23Node<T> newVal, ref T23Node<T> childsNewParent)
        {
            _leftChild = newVal;
            _leftChild.SetParent(ref childsNewParent);
        }
        public ref T23Node<T> GetLeftChild()
        {
            return ref _leftChild;
        }
        public void SetLeftChild(ref T23Node<T> newVal)
        {
            _leftChild = newVal;
        }
        public void SetLeftChild(ref T23Node<T> newVal, ref T23Node<T> childsNewParent)
        {
            _leftChild = newVal;
            _leftChild.SetParent(ref childsNewParent);
        }
        public ref T23Node<T> GetMiddleChild()
        {
            return ref _middleChild;
        }
        public void SetMiddleChild(ref T23Node<T> newVal)
        {
            _middleChild = newVal;
        }
        public ref T23Node<T> GetMiddleLeftExtraChild()
        {
            return ref _middleLeftExtraChild;
        }
        public void SetMiddleLeftExtraChild(ref T23Node<T> newVal)
        {
            _middleLeftExtraChild = newVal;
        }
        public ref T23Node<T> GetMiddleRightExtraChild()
        {
            return ref _middleRightExtraChild;
        }
        public void SetMiddleRightExtraChild(ref T23Node<T> newVal)
        {
            _middleRightExtraChild = newVal;
        }

        public int GetLevel()
        {
            int count = 0;
            var parent = _parent;
            while (parent!=null)
            {
                count++;
                parent = parent.GetParent();
            }
            return count;
        }

        public ref T23Node<T> Split()
        {
            return ref _parent;
        }
        public override string ToString()
        {
            string ret = " ";
            if (_data1 != null)
            {
                ret += _data1.ToString();
            }
            else
            {
                ret += "----";
            }
            ret += " , ";
            if (_data2 != null)
            {
                ret += _data2.ToString();
            }
            else
            {
                ret += "----";
            }
            ret += " | ";
            return ret;
        }
    }
}