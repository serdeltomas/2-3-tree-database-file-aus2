using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.FileIO;

namespace Semka2
{
    public class TwoThreeTree<TKey, T> where TKey : IComparable<TKey> where T : ICsv
    {
        private TwoThreeNode<TKey, T> _root;
        //private int _depth;//not counting atm
        private int _count;
//INSERT block -----------------------------------------------------------------------------------------------------------
        private ref TwoThreeNode<TKey, T> FindNodeToInsert(TKey keyToFind)
        {
            ref var searchNow = ref _root;
            if (_root == null) return ref _root;
            while(!searchNow.IsLeaf()){
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0 
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    throw new DuplicateWaitObjectException("duplicate key error in: tree: FindNodeToInsert");
                searchNow = ref searchNow.NextNode(keyToFind);
            }
            return ref searchNow;
        }
        public bool Insert(TKey nKey,T nData){
            if(_root == null)
            {
                _root = new TwoThreeNode<TKey, T>(nKey,nData); 
                _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            ref var node = ref FindNodeToInsert(nKey);
            if (InsertLeaf(ref node,nKey,ref nData)) { 
                _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            while(node.GetParent() != null)
            {
                ref var nodeKid = ref node;
                node = ref node.GetParent(); //caution with ref
                if(InsertInternal(ref node, ref nodeKid) || node == null) { 
                    _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            }
            _root = node; _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
        }
        public bool Insert(TKey nKey, ref T nData)
        {
            if (_root == null)
            {
                _root = new TwoThreeNode<TKey, T>(nKey, nData);
                _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
            }
            ref var node = ref FindNodeToInsert(nKey);
            if (InsertLeaf(ref node, nKey, ref nData))
            {
                _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
            }
            while (node.GetParent() != null)
            {
                ref var nodeKid = ref node;
                node = ref node.GetParent(); //caution with ref
                if (InsertInternal(ref node, ref nodeKid) || node == null)
                {
                    _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
                }
            }
            _root = node; _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
        }
        private bool InsertLeaf(ref TwoThreeNode<TKey, T> node,TKey nKey, ref T nData) {
            if (node.IsLeaf() && !node.Is3Node()) { // insert into this node easy; return true;
                if (node.GetLeftKey().CompareTo(nKey) < 0) { node.SetRight(nKey,nData);return true;} // lkey < nkey
                node.SetRight(node.GetLeftKey(),node.GetLeftData()); node.SetLeft(nKey, nData); return true; // nkey < lkey
            }
            if (node.IsLeaf() && node.Is3Node()) { //split into a tree; return false;
                if (node.GetLeftKey().CompareTo(nKey) > 0) { // nkey < lkey
                    node.SetLeftChild(new TwoThreeNode<TKey, T>(nKey,nData, ref node));
                    node.SetRightChild(new TwoThreeNode<TKey, T>(node.GetRightKey(),node.GetRightData(),ref node));
                    node.ClearRight(); return false; 
                }
                if (node.GetRightKey().CompareTo(nKey) < 0) { // rkey < nkey
                    node.SetRightChild(new TwoThreeNode<TKey, T>(nKey,nData,ref node));
                    node.SetLeftChild(new TwoThreeNode<TKey, T>(node.GetLeftKey(),node.GetLeftData(),ref node));
                    node.SetLeft(node.GetRightKey(), node.GetRightData());
                    node.ClearRight(); return false; 
                }
                //lkey < nkey < rkey
                node.SetRightChild(new TwoThreeNode<TKey, T>(node.GetRightKey(),node.GetRightData(),ref node));
                node.SetLeftChild(new TwoThreeNode<TKey, T>(node.GetLeftKey(),node.GetLeftData(),ref node));
                node.SetLeft(nKey, nData); node.ClearRight(); return false;
            }
            throw new Exception("shouldnt be here in: tree: insertLeaf");
        }
        private bool InsertInternal(ref TwoThreeNode<TKey, T> node, ref TwoThreeNode<TKey, T> nodeIns)
        {
            if (!node.Is3Node()){ Insert2Node(ref node, nodeIns); return true;}
            if (node.Is3Node()){ Insert3Node(ref node, nodeIns); return false;}
            throw new Exception("shouldnt be here in: tree: insertInternal");
        }
        private void Insert2Node(ref TwoThreeNode<TKey, T> node, TwoThreeNode<TKey, T> nodeIns)
        {//find where to put nodeinsert < <> >; put data there; reload children;
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // lkey < nkey
                node.SetRight(nodeIns.GetLeftKey(),nodeIns.GetLeftData());
                if(nodeIns.GetLeftChild() != null) node.SetMiddleChild(nodeIns.GetLeftChild(), ref node);
                if(nodeIns.GetRightChild() != null) node.SetRightChild(nodeIns.GetRightChild(), ref node);
            }
            else { // nkey < lkey
                node.SetRight(node.GetLeftKey(), node.GetLeftData());
                node.SetLeft(nodeIns.GetLeftKey(), nodeIns.GetLeftData());
                if(nodeIns.GetLeftChild() != null) node.SetLeftChild(nodeIns.GetLeftChild(), ref node);
                if(nodeIns.GetRightChild() != null) node.SetMiddleChild(nodeIns.GetRightChild(), ref node);
            }
        }
        private void Insert3Node(ref TwoThreeNode<TKey, T> node, TwoThreeNode<TKey, T> nodeIns){
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) > 0) { // nkey < lkey
                node.SetLeftChild(new TwoThreeNode<TKey, T>(node.GetRightKey(),node.GetRightData(),ref node));
                if(node.GetMiddleChild() != null) node.GetLeftChild().SetLeftChild(node.GetMiddleChild(),ref node.GetLeftChildRef());
                if(node.GetRightChild() != null) node.GetLeftChild().SetRightChild(node.GetRightChild(),ref node.GetLeftChildRef());
                node.SetRightChild(node.GetLeftChild());
                node.SetLeftChild(nodeIns,ref node);
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
            else if (node.GetRightKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // rkey < nkey
                node.SetRightChild(new TwoThreeNode<TKey, T>(node.GetLeftKey(),node.GetLeftData(),ref node));
                if(node.GetLeftChild() != null) node.GetRightChild().SetLeftChild(node.GetLeftChild(),ref node.GetRightChildRef());
                if(node.GetMiddleChild() != null) node.GetRightChild().SetRightChild(node.GetMiddleChild(),ref node.GetRightChildRef());
                node.SetLeftChild(node.GetRightChild());
                node.SetRightChild(nodeIns,ref node);
                node.SetLeft(node.GetRightKey(), node.GetRightData());
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
            else { //lkey < nkey < rkey
                node.SetMiddleChild(new TwoThreeNode<TKey, T>(node.GetLeftKey(),node.GetLeftData(),ref node));
                if(node.GetLeftChild() != null) node.GetMiddleChild().SetLeftChild(node.GetLeftChild(),ref node.GetMiddleChildRef());
                if(nodeIns.GetLeftChild() != null) node.GetMiddleChild().SetRightChild(nodeIns.GetLeftChild(),ref node.GetMiddleChildRef());
                if(node.GetMiddleChild() != null) node.SetLeftChild(node.GetMiddleChild(),ref node);
                node.SetMiddleChild(new TwoThreeNode<TKey, T>(node.GetRightKey(),node.GetRightData(),ref node));
                if(nodeIns.GetRightChild() != null) node.GetMiddleChild().SetLeftChild(nodeIns.GetRightChild(),ref node.GetMiddleChildRef());
                if(node.GetRightChild() != null) node.GetMiddleChild().SetRightChild(node.GetRightChild(),ref node.GetMiddleChildRef());
                if(node.GetMiddleChild() != null) node.SetRightChild(node.GetMiddleChild(),ref node);
                node.SetLeft(nodeIns.GetLeftKey(),nodeIns.GetLeftData());
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
        }
//insert block end -----------------------------------------------------------------------------------------------------------
//DELETE block----------------------------------------------------------------------------------------------------------
        private ref TwoThreeNode<TKey, T> FindNodeToDelete(TKey keyToFind)
        {
            ref var searchNow = ref _root;
            do {
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    return ref searchNow;
                searchNow = ref searchNow.NextNode(keyToFind);
            } while (!searchNow.IsLeaf());
            if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                return ref searchNow;
            throw new Exception("didnt find a match(or loop is not right) in: tree: FindNodeToDelete");
            //return ref searchNow;
        }
        public bool Delete(TKey key)
        {
            ref var node = ref FindNodeToDelete(key);
            if (!node.IsLeaf()) { node = ref DeleteSwitch(ref node, key); }//node switch with inordernext;
            if(DeleteLeaf(ref node,key)) { 
                _count--; 
                if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
	
            while(node != null && node.GetParent() != null && node.GetParent().GetParent() != null)
            {
                ref var parent = ref node.GetParent();
                if(DeleteInternal(ref node.GetParent())) { 
                    _count--; if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
                if (node != null) node = node.GetParent();
                else node = parent;
            }
            SetRoot(node); //replace curr root with only child
            _count--; if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;
	
        }
        private bool DeleteLeaf(ref TwoThreeNode<TKey, T> node, TKey key){
            if (node.Is3Node()) { //remove entry;return true;
                if (key.CompareTo(node.GetLeftKey()) != 0){ node.SetRight(default(TKey),default(T));return true;}
                node.SetLeft(node.GetRightKey(),node.GetRightData()); node.SetRight(default(TKey),default(T)); return true;
            }
            if (!node.Is3Node() && node.GetParent() == null) { _root = null; return true; } //its a root delete last entry; set null root; return true;
            return DeleteInternal(ref node);
        }
        private bool DeleteInternal(ref TwoThreeNode<TKey, T> node){
            ref var nodeParent = ref node.GetParent();
            ref var nodeSteal = ref nodeParent.GetLeftChildRef(); //only as a declaration
            if(!nodeParent.Is3Node()){
                if(nodeParent.GetLeftChild() == node)
                {
                    if (nodeParent.GetRightChild().Is3Node()) { //steal from nodeParent.GetRightChild(); rotate ;return true;
                        nodeSteal = ref nodeParent.GetRightChildRef();
                        if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(),ref node);
                        node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                        nodeParent.SetLeft(nodeSteal.GetLeftKey(),nodeSteal.GetLeftData());
                        nodeSteal.SetLeftChild(nodeSteal.GetMiddleChild());
                        nodeSteal.SetLeft(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                        nodeSteal.SetMiddleChild(null);
                        nodeSteal.ClearRight();
                        return true;
                    }
                    //merge with papa; return false;
                    ref var nodeRight = ref nodeParent.GetRightChildRef();
                    nodeRight.SetRight(nodeRight.GetLeftKey(),nodeRight.GetLeftData());
                    nodeRight.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeRight.SetMiddleChild(nodeRight.GetLeftChild());
                    if(node.GetLeftChild() != null) nodeRight.SetLeftChild(node.GetLeftChild(),ref nodeRight);
                    nodeParent.SetLeftChild(nodeRight);
                    nodeParent.SetRightChild(null);
                    return false;
                }
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate ;return true;
                    nodeSteal = ref nodeParent.GetLeftChildRef();
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(),ref node);
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeParent.SetLeft(nodeSteal.GetRightKey(),nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                //merge with papa; return false;
                ref var nodeLeft = ref nodeParent.GetLeftChildRef();
                nodeLeft.SetRight(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChild());
                if(node.GetLeftChild() != null) nodeLeft.SetRightChild(node.GetLeftChild(),ref nodeLeft);
                nodeParent.GetRightChild().ResetExceptParent();
                node = nodeLeft;
                return false;
            }
            nodeSteal = ref nodeParent.GetMiddleChildRef();
            if (nodeParent.GetLeftChild() == node)
            {
                if (nodeParent.GetMiddleChild().Is3Node())
                { //steal from nodeParent.GetMiddleChild() left; rotate; return true;
                    //left rotation
                    if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(),ref node);
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeParent.SetLeft(nodeSteal.GetLeftKey(),nodeSteal.GetLeftData());
                    nodeSteal.SetLeftChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetLeft(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                else//parent.middle child is not a 3node
                {
                    ref var nodeMid = ref nodeParent.GetMiddleChildRef();
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    node.SetRight(nodeMid.GetLeftKey(),nodeMid.GetLeftData());
                    if(nodeMid.GetLeftChild() != null) node.SetMiddleChild(nodeMid.GetLeftChild(),ref node);
                    if(nodeMid.GetRightChild() != null) node.SetRightChild(nodeMid.GetRightChild(),ref node);
                    nodeParent.SetMiddleChild(null);
                    nodeParent.SetLeft(nodeParent.GetRightKey(),nodeParent.GetRightData());
                    nodeParent.ClearRight();
                    return true;
                }
            }
            if (nodeParent.GetRightChild() == node)
            {
                if (nodeParent.GetMiddleChild().Is3Node())
                {
                    //steal from nodeParent.GetMiddleChild() right; rotate; return true;
                    //right rotation
                    node.SetRightChild(node.GetLeftChild()); //ever get here: haha prob duplicate
                    if (nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(), ref node);
                    node.SetLeft(nodeParent.GetRightKey(), nodeParent.GetRightData());
                    nodeParent.SetRight(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                else//parent.middle child is not a 3node
                {
                    ref var nodeMid = ref nodeParent.GetMiddleChildRef();
                    node.SetLeft(nodeMid.GetLeftKey(),nodeMid.GetLeftData());
                    node.SetRight(nodeParent.GetRightKey(),nodeParent.GetRightData());
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeMid.GetLeftChild() != null) node.SetMiddleChild(nodeMid.GetRightChild(),ref node);
                    if(nodeMid.GetRightChild() != null) node.SetLeftChild(nodeMid.GetLeftChild(),ref node);
                    nodeParent.SetMiddleChild(null);
                    nodeParent.ClearRight();
                    return true;
                }
            }
            if(nodeParent.GetMiddleChild() == node) {
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate; return true;
                    nodeSteal = ref nodeParent.GetLeftChildRef(); //right rotation
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(),ref node);
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeParent.SetLeft(nodeSteal.GetRightKey(),nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                if(nodeParent.GetRightChild().Is3Node()){ //steal from nodeParent.GetRightChild(); rotate; return true;
                    nodeSteal = ref nodeParent.GetRightChildRef(); //left rotation
                    if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(),ref node);
                    node.SetLeft(nodeParent.GetRightKey(),nodeParent.GetRightData());
                    nodeParent.SetRight(nodeSteal.GetLeftKey(),nodeSteal.GetLeftData());
                    nodeSteal.SetLeftChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetLeft(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                ref var nodeLeft = ref nodeParent.GetLeftChildRef(); //left rotation - kindof
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChild());
                if(node.GetLeftChild() != null) nodeLeft.SetRightChild(node.GetLeftChild(),ref nodeLeft);
                nodeLeft.SetRight(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                nodeParent.SetLeft(nodeParent.GetRightKey(),nodeParent.GetRightData());
                nodeParent.ClearRight();
                nodeParent.SetMiddleChild(null);
                return true;//idk if true
            }
            throw new Exception("shouldnt be here in: tree: DeleteInternal");
            //merge with papa; return false;
        }
        private ref TwoThreeNode<TKey, T> DeleteSwitch(ref TwoThreeNode<TKey, T> node, TKey key)
        {
            if (key.CompareTo(node.GetLeftKey()) == 0) {//key is leftval
                var tempData = node.GetLeftData();
                ref var switchNode = ref this.InOrderNextNodeRef(ref node, key);
                node.SetLeft(switchNode.GetLeftKey(),switchNode.GetLeftData());
                switchNode.SetLeft(key,tempData);
                return ref switchNode;
            }
            else { //key is rightval
                var tempData = node.GetRightData();
                ref var switchNode = ref this.InOrderNextNodeRef(ref node, key);
                node.SetRight(switchNode.GetLeftKey(),switchNode.GetLeftData());
                switchNode.SetLeft(key,tempData);
                return ref switchNode;
            }
        }
//delete block end -----------------------------------------------------------------------------------------------------------
        public ref TwoThreeNode<TKey, T> GetRoot()
        {
            return ref _root;
        }
        public void SetRoot(TwoThreeNode<TKey, T> newRoot)
        {
            _root = newRoot;
            if(_root != null) _root.ClearParent();
        }
        public int Count()
        {
            return _count;
        }
        public bool Contains(TKey keyToFind)
        {
            if (_root == null) return false;
            var searchNow = _root;
            if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                return true;
            if (searchNow.IsLeaf()) return false;
            do {
                searchNow = searchNow.NextNode(keyToFind);
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    return true;
            } while (!searchNow.IsLeaf());
            return false;
        }
        public T GetData(TKey keyToFind)
        {
            if (_root == null) return default(T);
            var searchNow = _root;
            if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0) return searchNow.GetLeftData();
            if (keyToFind.CompareTo(searchNow.GetRightKey()) == 0) return searchNow.GetRightData();
            if (searchNow.IsLeaf()) return default(T);
            do
            {
                searchNow = searchNow.NextNode(keyToFind);
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0) return searchNow.GetLeftData();
                if(keyToFind.CompareTo(searchNow.GetRightKey()) == 0) return searchNow.GetRightData();
            } while (!searchNow.IsLeaf());
            //throw new Exception("didnt find key in: tree: GetData");
            return default(T);
        }
        public ref T GetDataRef(TKey keyToFind)
        {
            ref var searchNow = ref _root;
            if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0) return ref searchNow.GetLeftDataRef();
            if (keyToFind.CompareTo(searchNow.GetRightKey()) == 0) return ref searchNow.GetRightDataRef();
            do
            {
                searchNow = searchNow.NextNode(keyToFind);
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0) return ref searchNow.GetLeftDataRef();
                if (keyToFind.CompareTo(searchNow.GetRightKey()) == 0) return ref searchNow.GetRightDataRef();
            } while (!searchNow.IsLeaf());
            throw new Exception("didnt find key in: tree: GetData");
        }
        public int CountEntries() {
            if (_root == null) return 0;
            var que = new Queue<TwoThreeNode<TKey,T>>();
            que.Enqueue(_root);
            int count = 0;
            do {
                var printNow = que.Dequeue();
                if (printNow.GetLeftData() != null) count++;
                if (printNow.GetRightData() != null) count++;
                if (printNow.GetLeftChild() != null) que.Enqueue(printNow.GetLeftChild());
                if (printNow.GetMiddleChild() != null) que.Enqueue(printNow.GetMiddleChild());
                if (printNow.GetRightChild() != null) que.Enqueue(printNow.GetRightChild());
            } while (que.Count != 0);
            return count;
        }
        public void GetAllLeaves(TwoThreeNode<TKey,T> n, ref ArrayList ll){
            if (n == null)
                return;
            if (n.Is3Node()){
                if(n.IsLeaf()) { ll.Add(n); return; }
                GetAllLeaves(n.GetLeftChild(),ref ll);
                GetAllLeaves(n.GetMiddleChild(),ref ll);
                GetAllLeaves(n.GetRightChild(),ref ll);
            }
            else {
                if(n.IsLeaf()){ ll.Add(n); return; }
                GetAllLeaves(n.GetLeftChild(),ref ll);
                GetAllLeaves(n.GetRightChild(),ref ll);
            }
        }
        public bool IsAllLeavesSameDepth()
        {
            var listofleaves = new ArrayList();
            GetAllLeaves(_root, ref listofleaves);
            if (listofleaves.Count == 0) return true;
            var control = ((TwoThreeNode<TKey, T>) listofleaves[0]).GetLevel();
            foreach (var list in listofleaves) {
                if (control != ((TwoThreeNode<TKey, T>) list).GetLevel()) return false;
            }
            return true;
        }
        public override string ToString()
        {
            if (_root == null) return "";
            var que = new Queue<TwoThreeNode<TKey, T>>();
            string ret = "\n";
            que.Enqueue(_root);
            int count = 1;
            do
            {
                var printNow = que.Dequeue();
                //if (printNow.GetLevel() == count) { count++; ret += "\n"; }
                if (printNow.GetLevel() == count) { count++; ret += ""; }
                ret += printNow.ToString();
                if (printNow.GetLeftChild() != null) que.Enqueue(printNow.GetLeftChild());
                if (printNow.GetMiddleChild() != null) que.Enqueue(printNow.GetMiddleChild());
                if (printNow.GetRightChild() != null) que.Enqueue(printNow.GetRightChild());
            } while (que.Count != 0);
            return ret;
        }
        public string VypisVsetko()
        {
            if (_root == null) return "";
            var sb = new StringBuilder();
            sb.Append("pocet: " + Count() + "\n");
            var inorder = InOrder();
            foreach (T io in inorder) sb.Append(io.ToString());
            return sb.ToString();
        }
        public DataTable ToDataTable()
        {
            DataTable table = new DataTable();
            //columns  
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("NAME", typeof(string));
            table.Columns.Add("CITY", typeof(string));

            //data  
            table.Rows.Add(111, "Devesh", "Ghaziabad");
            table.Rows.Add(222, "ROLI", "KANPUR");
            table.Rows.Add(102, "ROLI", "MAINPURI");
            table.Rows.Add(212, "DEVESH", "KANPUR");
            table.Rows.Add(102, "NIKHIL", "GZB");
            table.Rows.Add(212, "HIMANSHU", "NOIDa");
            table.Rows.Add(102, "AVINASH", "NOIDa");
            table.Rows.Add(212, "BHUPPI", "GZB");

            return table;
        }
        //----------------------------------------traversals--------------------------------------
        public void InOrderRecursion(TwoThreeNode<TKey, T> n, ref ArrayList ll)
        {
            if (n == null)
                return;
            if (n.Is3Node())
            {
                this.InOrderRecursion(n.GetLeftChild(), ref ll);
                ll.Add(n.GetLeftKey());
                this.InOrderRecursion(n.GetMiddleChild(), ref ll);
                ll.Add(n.GetRightKey());
                this.InOrderRecursion(n.GetRightChild(), ref ll);
            }
            else
            {
                this.InOrderRecursion(n.GetLeftChild(), ref ll);
                ll.Add(n.GetLeftKey());
                this.InOrderRecursion(n.GetRightChild(), ref ll);
            }
        }
        public ref TwoThreeNode<TKey, T> InOrderNextNodeRef(ref TwoThreeNode<TKey, T> n, TKey k)
        {
            if(n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = ref n.GetMiddleChildRef();
            else n = ref n.GetRightChildRef();
            while (!n.IsLeaf()) {
                n = ref n.GetLeftChildRef();
            }
            return ref n;
        }
        public List<T> LevelOrder(TwoThreeNode<TKey, T> n)
        {
            var listLO = new List<T>();
            if (_root == null) return listLO;
            var que = new Queue<TwoThreeNode<TKey, T>>();
            que.Enqueue(_root);
            do {
                var printNow = que.Dequeue();
                listLO.Add(printNow.GetLeftData());
                listLO.Add(printNow.GetRightData());
                if (printNow.GetLeftChild() != null) que.Enqueue(printNow.GetLeftChild());
                if (printNow.GetMiddleChild() != null) que.Enqueue(printNow.GetMiddleChild());
                if (printNow.GetRightChild() != null) que.Enqueue(printNow.GetRightChild());
            } while (que.Count != 0);
            return listLO;
        }
        public List<T> InOrder()
        {
            var listIO = new List<T>();
            if (_root == null) return listIO;
            //get all the way left
            var node = _root;
            while (!node.IsLeaf()) { node = node.GetLeftChild(); }
            var key = node.GetLeftKey();
            listIO.Add(node.GetLeftData());
            while (node != null)
            {
                var nextData = InOrderNextData(node, key);
                if(nextData != null) listIO.Add(nextData);
                var oldKey = key;
                key = InOrderNextKey(node, key);
                node = InOrderNextNode(node, oldKey);
            }
            return listIO;
        }

        public TwoThreeNode<TKey, T> FindNodeMin(TKey minK)
        {
            ref var searchNow = ref _root;
            if (_root == null) return _root;
            while (!searchNow.IsLeaf())
            {
                if (minK.CompareTo(searchNow.GetLeftKey()) == 0
                    || minK.CompareTo(searchNow.GetRightKey()) == 0)
                    return searchNow;
                searchNow = ref searchNow.NextNode(minK);
            }
            return searchNow;
        }
        public List<T> InOrder(TKey minK, TKey maxK)
        {
            var listIO = new List<T>();
            if (_root == null) return listIO;
            //find node to start
            var node = FindNodeMin(minK);
            TKey key;
            if (minK.CompareTo(node.GetLeftKey()) <= 0 || !node.Is3Node())//minkey <= lkey or node isnt 3-node
            { key = node.GetLeftKey(); }
            else { key = node.GetRightKey(); }
            if (minK.CompareTo(key) <= 0 && maxK.CompareTo(key) >= 0) {listIO.Add(node.GetData(key)); } //adds only if min <= key <= max
            while (node != null) 
            { 
                var nextData = InOrderNextData(node, key);                
                var oldKey = key;
                key = InOrderNextKey(node, key);
                node = InOrderNextNode(node, oldKey);
                if (maxK.CompareTo(key) < 0) break; //if key > maxkey end
                if (nextData != null) listIO.Add(nextData);
            }
            return listIO;
        }
        public List<TKey> InOrderKeys()
        {
            var listIO = new List<TKey>();
            if (_root == null) return listIO;
            //get all the way left
            var node = _root;
            while (!node.IsLeaf()) { node = node.GetLeftChild(); }
            var key = node.GetLeftKey();
            listIO.Add(key);
            while (node != null)
            {
                //var nextData = InOrderNextData(node, key);
                var oldKey = key;
                key = InOrderNextKey(node, oldKey);
                node = InOrderNextNode(node, oldKey);
                if(key != null) listIO.Add(key);
            }
            return listIO;
        }
        public T InOrderNextData(TwoThreeNode<TKey, T> n, TKey k)
        {
            if (!n.IsLeaf())
            {
                if (n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = n.GetMiddleChild();
                else n = n.GetRightChild();
                while (!n.IsLeaf())
                {
                    n = n.GetLeftChild();
                }
                return n.GetLeftData();
            }
            else // n is a leaf
            {
                if (n.Is3Node() && k.CompareTo(n.GetLeftKey()) == 0) return n.GetRightData();
                if (n.GetParent() == null) return default(T); //koniec
                while (n.GetParent() != null)
                {
                    var nPar = n.GetParent();
                    if (nPar.Is3Node())
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0) return nPar.GetLeftData();
                        if (nPar.GetMiddleChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetMiddleChild().GetRightKey()) == 0) return nPar.GetRightData();
                        k = nPar.GetRightKey();
                    }
                    else
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0) return nPar.GetLeftData();
                        k = nPar.GetLeftKey();
                    }
                    // n is right child
                    n = nPar;
                }
                // im in root, n is root and algo is done
                return default(T); // koniec
            }
        }
        public TKey InOrderNextKey(TwoThreeNode<TKey, T> n, TKey k)
        {
            if (!n.IsLeaf())
            {
                if (n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = n.GetMiddleChild();
                else n = n.GetRightChild();
                while (!n.IsLeaf())
                {
                    n = n.GetLeftChild();
                }
                return n.GetLeftKey();
            }
            else // n is a leaf
            {
                if (n.Is3Node() && k.CompareTo(n.GetLeftKey()) == 0) return n.GetRightKey();
                if (n.GetParent() == null) return default(TKey); //koniec
                while (n.GetParent() != null)
                {
                    var nPar = n.GetParent();
                    if (nPar.Is3Node())
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0) return nPar.GetLeftKey();
                        if (nPar.GetMiddleChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetMiddleChild().GetRightKey()) == 0) return nPar.GetRightKey();
                        k = nPar.GetRightKey();
                    }
                    else
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0) return nPar.GetLeftKey();
                        k = nPar.GetLeftKey();
                    }
                    // n is right child
                    n = nPar;
                }
                // im in root, n is root and algo is done
                return default(TKey); // koniec
            }
        }
        public TwoThreeNode<TKey, T> InOrderNextNode(TwoThreeNode<TKey, T> n, TKey k)
        {
            if (!n.IsLeaf())
            {
                if (n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = n.GetMiddleChild();
                else n = n.GetRightChild();
                while (!n.IsLeaf())
                {
                    n = n.GetLeftChild();
                }
                return n;
            }
            else // n is a leaf
            {
                if (n.Is3Node() && k.CompareTo(n.GetLeftKey()) == 0) return n;
                if (n.GetParent() == null) return null; //koniec
                while (n.GetParent() != null)
                {
                    var nPar = n.GetParent();
                    if (nPar.Is3Node())
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0 ||
                            nPar.GetMiddleChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetMiddleChild().GetRightKey()) == 0) return nPar;
                        k = nPar.GetRightKey();
                    }
                    else
                    {
                        if (nPar.GetLeftChild().GetLeftKey().CompareTo(k) == 0 ||
                            k.CompareTo(nPar.GetLeftChild().GetRightKey()) == 0) return nPar;
                        k = nPar.GetLeftKey();
                    }
                    // n is right child
                    n = nPar;
                }
                // im in root, n is root and algo is done
                return null; // koniec
            }
        }
    }
}