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
    public class T23Tree<TKey, T> where TKey : IComparable<TKey>, IDataToFIle<TKey> where T : ICsv, IDataToFIle<T> //where T23Node<TKey, T> :IDataToFIle<T23Node<TKey, T>>
    {
        private T23Node<TKey, T> _root;
        //private int _depth;//not counting atm
        private int _count;
//INSERT block -----------------------------------------------------------------------------------------------------------
        private ref T23Node<TKey, T> FindNodeToInsert(TKey keyToFind)
        {
            ref var searchNow = ref _root;
            if (_root == null) return ref _root;
            while(!searchNow.IsLeaf()){
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0 
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    throw new DuplicateWaitObjectException("duplicate key error in: tree: FindNodeToInsert");
                searchNow = searchNow.NextNode(keyToFind);
            }
            return ref searchNow;
        }
        public bool Insert(TKey nKey,T nData){
            if(_root == null)
            {
                _root = new T23Node<TKey, T>(nKey,nData); 
                _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            var node =  FindNodeToInsert(nKey);
            if (InsertLeaf(node,nKey,nData)) { 
                _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            while(node.GetParent() != null)
            {
                var nodeKid = node;
                node = node.GetParent(); //caution with ref
                if(InsertInternal(node, nodeKid) || node == null) { 
                    _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            }
            _root = node; _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
        }
        public bool Insert(TKey nKey, ref T nData)
        {
            if (_root == null)
            {
                _root = new T23Node<TKey, T>(nKey, nData);
                _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
            }
            ref var node = ref FindNodeToInsert(nKey);
            if (InsertLeaf(node, nKey, nData))
            {
                _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
            }
            while (node.GetParent() != null)
            {
                ref var nodeKid = ref node;
                node = node.GetParent(); //caution with ref
                if (InsertInternal( node, nodeKid) || node == null)
                {
                    _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
                }
            }
            _root = node; _count++; if (_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
        }
        private bool InsertLeaf(T23Node<TKey, T> node,TKey nKey, T nData) {
            if (node.IsLeaf() && !node.Is3Node()) { // insert into this node easy; return true;
                if (node.GetLeftKey().CompareTo(nKey) < 0) { node.SetRight(nKey,nData);return true;} // lkey < nkey
                node.SetRight(node.GetLeftKey(),node.GetLeftData()); node.SetLeft(nKey, nData); return true; // nkey < lkey
            }
            if (node.IsLeaf() && node.Is3Node()) { //split into a tree; return false;
                if (node.GetLeftKey().CompareTo(nKey) > 0) { // nkey < lkey
                    node.SetLeftChild(new T23Node<TKey, T>(nKey,nData, node));
                    node.SetRightChild(new T23Node<TKey, T>(node.GetRightKey(),node.GetRightData(),node));
                    node.ClearRight(); return false; 
                }
                if (node.GetRightKey().CompareTo(nKey) < 0) { // rkey < nkey
                    node.SetRightChild(new T23Node<TKey, T>(nKey,nData,node));
                    node.SetLeftChild(new T23Node<TKey, T>(node.GetLeftKey(),node.GetLeftData(),node));
                    node.SetLeft(node.GetRightKey(), node.GetRightData());
                    node.ClearRight(); return false; 
                }
                //lkey < nkey < rkey
                node.SetRightChild(new T23Node<TKey, T>(node.GetRightKey(),node.GetRightData(),node));
                node.SetLeftChild(new T23Node<TKey, T>(node.GetLeftKey(),node.GetLeftData(),node));
                node.SetLeft(nKey, nData); node.ClearRight(); return false;
            }
            throw new Exception("shouldnt be here in: tree: insertLeaf");
        }
        private bool InsertInternal(T23Node<TKey, T> node, T23Node<TKey, T> nodeIns)
        {
            if (!node.Is3Node()){ Insert2Node(node, nodeIns); return true;}
            if (node.Is3Node()){ Insert3Node(node, nodeIns); return false;}
            throw new Exception("shouldnt be here in: tree: insertInternal");
        }
        private void Insert2Node(T23Node<TKey, T> node, T23Node<TKey, T> nodeIns)
        {//find where to put nodeinsert < <> >; put data there; reload children;
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // lkey < nkey
                node.SetRight(nodeIns.GetLeftKey(),nodeIns.GetLeftData());
                if(nodeIns.GetLeftChild() != null) node.SetMiddleChild(nodeIns.GetLeftChild(), node);
                if(nodeIns.GetRightChild() != null) node.SetRightChild(nodeIns.GetRightChild(), node);
            }
            else { // nkey < lkey
                node.SetRight(node.GetLeftKey(), node.GetLeftData());
                node.SetLeft(nodeIns.GetLeftKey(), nodeIns.GetLeftData());
                if(nodeIns.GetLeftChild() != null) node.SetLeftChild(nodeIns.GetLeftChild(), node);
                if(nodeIns.GetRightChild() != null) node.SetMiddleChild(nodeIns.GetRightChild(), node);
            }
        }
        private void Insert3Node(T23Node<TKey, T> node, T23Node<TKey, T> nodeIns){
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) > 0) { // nkey < lkey
                node.SetLeftChild(new T23Node<TKey, T>(node.GetRightKey(),node.GetRightData(), node));
                if(node.GetMiddleChild() != null) node.GetLeftChild().SetLeftChild(node.GetMiddleChild(),node.GetLeftChild());
                if(node.GetRightChild() != null) node.GetLeftChild().SetRightChild(node.GetRightChild(),node.GetLeftChild());
                node.SetRightChild(node.GetLeftChild());
                node.SetLeftChild(nodeIns,node);
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
            else if (node.GetRightKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // rkey < nkey
                node.SetRightChild(new T23Node<TKey, T>(node.GetLeftKey(),node.GetLeftData(),node));
                if(node.GetLeftChild() != null) node.GetRightChild().SetLeftChild(node.GetLeftChild(),node.GetRightChild());
                if(node.GetMiddleChild() != null) node.GetRightChild().SetRightChild(node.GetMiddleChild(),node.GetRightChild());
                node.SetLeftChild(node.GetRightChild());
                node.SetRightChild(nodeIns, node);
                node.SetLeft(node.GetRightKey(), node.GetRightData());
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
            else { //lkey < nkey < rkey
                node.SetMiddleChild(new T23Node<TKey, T>(node.GetLeftKey(),node.GetLeftData(),node));
                if(node.GetLeftChild() != null) node.GetMiddleChild().SetLeftChild(node.GetLeftChild(), node.GetMiddleChild());
                if(nodeIns.GetLeftChild() != null) node.GetMiddleChild().SetRightChild(nodeIns.GetLeftChild(), node.GetMiddleChild());
                if(node.GetMiddleChild() != null) node.SetLeftChild(node.GetMiddleChild(), node);
                node.SetMiddleChild(new T23Node<TKey, T>(node.GetRightKey(),node.GetRightData(), node));
                if(nodeIns.GetRightChild() != null) node.GetMiddleChild().SetLeftChild(nodeIns.GetRightChild(), node.GetMiddleChild());
                if(node.GetRightChild() != null) node.GetMiddleChild().SetRightChild(node.GetRightChild(), node.GetMiddleChild());
                if(node.GetMiddleChild() != null) node.SetRightChild(node.GetMiddleChild(), node);
                node.SetLeft(nodeIns.GetLeftKey(),nodeIns.GetLeftData());
                node.ClearRight();
                node.SetMiddleChild(null);
                //node.ClearParent();
            }
        }
//insert block end -----------------------------------------------------------------------------------------------------------
//DELETE block----------------------------------------------------------------------------------------------------------
        private T23Node<TKey, T> FindNodeToDelete(TKey keyToFind)
        {
            var searchNow = _root;
            do {
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    return searchNow;
                searchNow = searchNow.NextNode(keyToFind);
            } while (!searchNow.IsLeaf());
            if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0
                || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                return searchNow;
            throw new Exception("didnt find a match(or loop is not right) in: tree: FindNodeToDelete");
            //return ref searchNow;
        }
        public bool Delete(TKey key)
        {
            var node = FindNodeToDelete(key);
            if (!node.IsLeaf()) { node = DeleteSwitch(node, key); }//node switch with inordernext;
            if(DeleteLeaf(node,key)) { 
                _count--; 
                if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
	
            while(node != null && node.GetParent() != null && node.GetParent().GetParent() != null)
            {
                var parent = node.GetParent();
                if(DeleteInternal(node.GetParent())) { 
                    _count--; if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
                if (node != null) node = node.GetParent();
                else node = parent;
            }
            SetRoot(node); //replace curr root with only child
            _count--; if (_count != 0 && _root.GetParent() != null) SetRoot(_root.GetParent()); return true;
	
        }
        private bool DeleteLeaf(T23Node<TKey, T> node, TKey key){
            if (node.Is3Node()) { //remove entry;return true;
                if (key.CompareTo(node.GetLeftKey()) != 0){ node.SetRight(default(TKey),default(T));return true;}
                node.SetLeft(node.GetRightKey(),node.GetRightData()); node.SetRight(default(TKey),default(T)); return true;
            }
            if (!node.Is3Node() && node.GetParent() == null) { _root = null; return true; } //its a root delete last entry; set null root; return true;
            return DeleteInternal(node);
        }
        private bool DeleteInternal(T23Node<TKey, T> node){
            var nodeParent = node.GetParent();
            var nodeSteal = nodeParent.GetLeftChild(); //only as a declaration
            if(!nodeParent.Is3Node()){
                if(nodeParent.GetLeftChild() == node)
                {
                    if (nodeParent.GetRightChild().Is3Node()) { //steal from nodeParent.GetRightChild(); rotate ;return true;
                        nodeSteal = nodeParent.GetRightChild();
                        if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(), node);
                        node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                        nodeParent.SetLeft(nodeSteal.GetLeftKey(),nodeSteal.GetLeftData());
                        nodeSteal.SetLeftChild(nodeSteal.GetMiddleChild());
                        nodeSteal.SetLeft(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                        nodeSteal.SetMiddleChild(null);
                        nodeSteal.ClearRight();
                        return true;
                    }
                    //merge with papa; return false;
                    var nodeRight = nodeParent.GetRightChild();
                    nodeRight.SetRight(nodeRight.GetLeftKey(),nodeRight.GetLeftData());
                    nodeRight.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeRight.SetMiddleChild(nodeRight.GetLeftChild());
                    if(node.GetLeftChild() != null) nodeRight.SetLeftChild(node.GetLeftChild(), nodeRight);
                    nodeParent.SetLeftChild(nodeRight);
                    nodeParent.SetRightChild(null);
                    return false;
                }
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate ;return true;
                    nodeSteal = nodeParent.GetLeftChild();
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(), node);
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeParent.SetLeft(nodeSteal.GetRightKey(),nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                //merge with papa; return false;
                var nodeLeft = nodeParent.GetLeftChild();
                nodeLeft.SetRight(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChild());
                if(node.GetLeftChild() != null) nodeLeft.SetRightChild(node.GetLeftChild(), nodeLeft);
                nodeParent.GetRightChild().ResetExceptParent();
                node = nodeLeft;
                return false;
            }
            nodeSteal = nodeParent.GetMiddleChild();
            if (nodeParent.GetLeftChild() == node)
            {
                if (nodeParent.GetMiddleChild().Is3Node())
                { //steal from nodeParent.GetMiddleChild() left; rotate; return true;
                    //left rotation
                    if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(), node);
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
                    var nodeMid = nodeParent.GetMiddleChild();
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    node.SetRight(nodeMid.GetLeftKey(),nodeMid.GetLeftData());
                    if(nodeMid.GetLeftChild() != null) node.SetMiddleChild(nodeMid.GetLeftChild(), node);
                    if(nodeMid.GetRightChild() != null) node.SetRightChild(nodeMid.GetRightChild(), node);
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
                    if (nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(), node);
                    node.SetLeft(nodeParent.GetRightKey(), nodeParent.GetRightData());
                    nodeParent.SetRight(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                else//parent.middle child is not a 3node
                {
                    var nodeMid = nodeParent.GetMiddleChild();
                    node.SetLeft(nodeMid.GetLeftKey(),nodeMid.GetLeftData());
                    node.SetRight(nodeParent.GetRightKey(),nodeParent.GetRightData());
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeMid.GetLeftChild() != null) node.SetMiddleChild(nodeMid.GetRightChild(), node);
                    if(nodeMid.GetRightChild() != null) node.SetLeftChild(nodeMid.GetLeftChild(), node);
                    nodeParent.SetMiddleChild(null);
                    nodeParent.ClearRight();
                    return true;
                }
            }
            if(nodeParent.GetMiddleChild() == node) {
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate; return true;
                    nodeSteal = nodeParent.GetLeftChild(); //right rotation
                    node.SetRightChild(node.GetLeftChild());
                    if(nodeSteal.GetRightChild() != null) node.SetLeftChild(nodeSteal.GetRightChild(), node);
                    node.SetLeft(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                    nodeParent.SetLeft(nodeSteal.GetRightKey(),nodeSteal.GetRightData());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                if(nodeParent.GetRightChild().Is3Node()){ //steal from nodeParent.GetRightChild(); rotate; return true;
                    nodeSteal = nodeParent.GetRightChild(); //left rotation
                    if(nodeSteal.GetLeftChild() != null) node.SetRightChild(nodeSteal.GetLeftChild(), node);
                    node.SetLeft(nodeParent.GetRightKey(),nodeParent.GetRightData());
                    nodeParent.SetRight(nodeSteal.GetLeftKey(),nodeSteal.GetLeftData());
                    nodeSteal.SetLeftChild(nodeSteal.GetMiddleChild());
                    nodeSteal.SetLeft(nodeSteal.GetRightKey(), nodeSteal.GetRightData());
                    nodeSteal.SetMiddleChild(null);
                    nodeSteal.ClearRight();
                    return true;
                }
                var nodeLeft = nodeParent.GetLeftChild(); //left rotation - kindof
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChild());
                if(node.GetLeftChild() != null) nodeLeft.SetRightChild(node.GetLeftChild(), nodeLeft);
                nodeLeft.SetRight(nodeParent.GetLeftKey(),nodeParent.GetLeftData());
                nodeParent.SetLeft(nodeParent.GetRightKey(),nodeParent.GetRightData());
                nodeParent.ClearRight();
                nodeParent.SetMiddleChild(null);
                return true;//idk if true
            }
            throw new Exception("shouldnt be here in: tree: DeleteInternal");
            //merge with papa; return false;
        }
        private T23Node<TKey, T> DeleteSwitch(T23Node<TKey, T> node, TKey key)
        {
            if (key.CompareTo(node.GetLeftKey()) == 0) {//key is leftval
                var tempData = node.GetLeftData();
                var switchNode = this.InOrderNextNode(node, key);
                node.SetLeft(switchNode.GetLeftKey(),switchNode.GetLeftData());
                switchNode.SetLeft(key,tempData);
                return switchNode;
            }
            else { //key is rightval
                var tempData = node.GetRightData();
                var switchNode = this.InOrderNextNode(node, key);
                node.SetRight(switchNode.GetLeftKey(),switchNode.GetLeftData());
                switchNode.SetLeft(key,tempData);
                return switchNode;
            }
        }
//delete block end -----------------------------------------------------------------------------------------------------------
        public T23Node<TKey, T> GetRoot()
        {
            return _root;
        }
        public void SetRoot(T23Node<TKey, T> newRoot)
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
        public int CountEntries() {
            if (_root == null) return 0;
            var que = new Queue<T23Node<TKey,T>>();
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
        public void GetAllLeaves(T23Node<TKey,T> n, ref ArrayList ll){
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
            var control = ((T23Node<TKey, T>) listofleaves[0]).GetLevel();
            foreach (var list in listofleaves) {
                if (control != ((T23Node<TKey, T>) list).GetLevel()) return false;
            }
            return true;
        }
        public override string ToString()
        {
            if (_root == null) return "";
            var que = new Queue<T23Node<TKey, T>>();
            string ret = "\n";
            que.Enqueue(_root);
            int count = 1;
            do
            {
                var printNow = que.Dequeue();
                if (printNow.GetLevel() == count) { count++; ret += "\n"; }
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
        public void InOrderRecursion(T23Node<TKey, T> n, ref ArrayList ll)
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
        public ref T23Node<TKey, T> InOrderNextNodeRef(ref T23Node<TKey, T> n, TKey k)
        {
            if(n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = n.GetMiddleChild();
            else n = n.GetRightChild();
            while (!n.IsLeaf()) {
                n = n.GetLeftChild();
            }
            return ref n;
        }
        public List<T> LevelOrder(T23Node<TKey, T> n)
        {
            var listLO = new List<T>();
            if (_root == null) return listLO;
            var que = new Queue<T23Node<TKey, T>>();
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

        public T23Node<TKey, T> FindNodeMin(TKey minK)
        {
            ref var searchNow = ref _root;
            if (_root == null) return _root;
            while (!searchNow.IsLeaf())
            {
                if (minK.CompareTo(searchNow.GetLeftKey()) == 0
                    || minK.CompareTo(searchNow.GetRightKey()) == 0)
                    return searchNow;
                searchNow = searchNow.NextNode(minK);
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
        public T InOrderNextData(T23Node<TKey, T> n, TKey k)
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
        public TKey InOrderNextKey(T23Node<TKey, T> n, TKey k)
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
        public T23Node<TKey, T> InOrderNextNode(T23Node<TKey, T> n, TKey k)
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