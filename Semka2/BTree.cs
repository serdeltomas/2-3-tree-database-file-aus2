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
    public class BTree<TKey, T> where TKey : IComparable<TKey>, IDataToFIle<TKey> where T : ICsv, IDataToFIle<T> //where BNode<TKey, T> :IDataToFIle<BNode<TKey, T>>
    {
        private BNode<TKey, T> _root;
        private FIleHandler<TKey> _keysFile;
        private FIleHandler<T> _dataFile;
        private FIleHandler<BNode<TKey, T>> _nodesFile;
        //private int _depth;//not counting atm
        private int _count;

        public BTree() { }
        public BTree(FIleHandler<TKey> pKeysFile, FIleHandler<T> pDataFile, string pNodesFileName) {
            _keysFile = pKeysFile; _dataFile = pDataFile; _nodesFile = new FIleHandler<BNode<TKey, T>>(pNodesFileName, new BNode<TKey,T>().Size());
            _nodesFile.SetInstance(new BNode<TKey, T>(_keysFile,_dataFile,_nodesFile));
        }
        public FIleHandler<BNode<TKey, T>> GetNodesFile() { return _nodesFile; }
        //INSERT block -----------------------------------------------------------------------------------------------------------
        private BNode<TKey, T> FindNodeToInsert(TKey keyToFind)
        {
            var searchNow = _root;
            if (_root == null) return _root;
            while(!searchNow.IsLeaf()){
                if (keyToFind.CompareTo(searchNow.GetLeftKey()) == 0 
                    || keyToFind.CompareTo(searchNow.GetRightKey()) == 0)
                    throw new DuplicateWaitObjectException("duplicate key error in: tree: FindNodeToInsert");
                searchNow = searchNow.NextNode(keyToFind);
            }
            return searchNow;
        }
        public bool Insert(TKey nKey,T nData){
            var nKeyPos = _keysFile.InsertToFile(nKey);
            var nDataPos = _dataFile.InsertToFile(nData);
            if (_root == null)
            {
                _root = new BNode<TKey, T>(nKeyPos, nDataPos, _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere());
                _nodesFile.InsertToFile(_root);
                _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;}
            var node =  FindNodeToInsert(nKey);
            if (InsertLeaf(node,nKeyPos,nDataPos,nKey)) { 
                if(_root.GetParent() != null)
                    SetRoot(_root.GetParent());//not gud if i get her
                _nodesFile.UpdateInFIle(node, node.GetPos()); _count++; return true;
            }
            while(node.GetParent() != null)
            {
                var nodeKid = node;
                node = node.GetParent();
                if (InsertInternal(node, nodeKid) || node == null) {
                    if (_root.GetParentPos() != -1)
                        SetRoot(_root.GetParent());//not gud if i get her i think
                    if (node.GetParentPos() == -1)
                        SetRoot(node);
                    else
                        _nodesFile.DeleteFromFile(nodeKid.GetPos());
                    _count++; return true;}
            }
            _root = node; _count++; if(_root.GetParent() != null) SetRoot(_root.GetParent()); return true;
        }
        private bool InsertLeaf(BNode<TKey, T> node,long nKeyPos, long nDataPos, TKey nKey) {
            if (node.IsLeaf() && !node.Is3Node()) { // insert into this node easy; return true;
                if (node.GetLeftKey().CompareTo(nKey) < 0) { node.SetRight(nKeyPos,nDataPos);return true;} // lkey < nkey
                node.SetRight(node.GetLeftKeyPos(),node.GetLeftDataPos()); node.SetLeft(nKeyPos, nDataPos); return true; // nkey < lkey
            }
            if (node.IsLeaf() && node.Is3Node()) { //split into a tree; return false;
                if (node.GetLeftKey().CompareTo(nKey) > 0) { // nkey < lkey
                    var nPos1 = _nodesFile.InsertToFile(new BNode<TKey, T>(nKeyPos, nDataPos, node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                    node.SetLeftChild(nPos1);
                    var nPos2 = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetRightKeyPos(), node.GetRightDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                    node.SetRightChild(nPos2);
                    node.ClearRight();
                    _nodesFile.UpdateInFIle(node,node.GetPos());
                    return false; 
                }
                if (node.GetRightKey().CompareTo(nKey) < 0) { // rkey < nkey
                    var nPos1 = _nodesFile.InsertToFile(new BNode<TKey, T>(nKeyPos, nDataPos, node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                    node.SetRightChild(nPos1);
                    var nPos2 = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetLeftKeyPos(), node.GetLeftDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                    node.SetLeftChild(nPos2);
                    node.SetLeft(node.GetRightKeyPos(), node.GetRightDataPos());
                    node.ClearRight();
                    _nodesFile.UpdateInFIle(node, node.GetPos()); return false; 
                }
                //lkey < nkey < rkey
                var nPos = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetRightKeyPos(), node.GetRightDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetRightChild(nPos);
                var nPoss = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetLeftKeyPos(), node.GetLeftDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetLeftChild(nPoss);
                node.SetLeft(nKeyPos, nDataPos); node.ClearRight();
                _nodesFile.UpdateInFIle(node, node.GetPos()); return false;
            }
            throw new Exception("shouldnt be here in: tree: insertLeaf");
        }
        private bool InsertInternal(BNode<TKey, T> node, BNode<TKey, T> nodeIns)
        {
            if (!node.Is3Node()){ Insert2Node(node, nodeIns); return true;}
            if (node.Is3Node()){ Insert3Node(node, nodeIns); return false;}
            throw new Exception("shouldnt be here in: tree: insertInternal");
        }
        private void Insert2Node(BNode<TKey, T> node, BNode<TKey, T> nodeIns)
        {//find where to put nodeinsert < <> >; put data there; reload children;
            long delWhat;
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // lkey < nkey
                node.SetRight(nodeIns.GetLeftKeyPos(),nodeIns.GetLeftDataPos());
                delWhat = node.GetRightChildPos();
                if (nodeIns.GetLeftChildPos() != -1) node.SetMiddleChild(nodeIns.GetLeftChildPos(), node.GetPos());
                if(nodeIns.GetRightChildPos() != -1) node.SetRightChild(nodeIns.GetRightChildPos(), node.GetPos());
            }
            else { // nkey < lkey
                node.SetRight(node.GetLeftKeyPos(), node.GetLeftDataPos());
                node.SetLeft(nodeIns.GetLeftKeyPos(), nodeIns.GetLeftDataPos());
                delWhat = node.GetLeftChildPos();
                if (nodeIns.GetLeftChildPos() != -1) node.SetLeftChild(nodeIns.GetLeftChildPos(), node.GetPos());
                if(nodeIns.GetRightChildPos() != -1) node.SetMiddleChild(nodeIns.GetRightChildPos(), node.GetPos());
            }
            _nodesFile.UpdateInFIle(node, node.GetPos());
            _nodesFile.UpdateInFIle(nodeIns, nodeIns.GetPos());
            _nodesFile.DeleteFromFile(delWhat);
        }
        private void Insert3Node(BNode<TKey, T> node, BNode<TKey, T> nodeIns){
            if (node.GetLeftKey().CompareTo(nodeIns.GetLeftKey()) > 0) { // nkey < lkey
                var nPos = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetRightKeyPos(), node.GetRightDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetLeftChild(nPos);
                if(node.GetMiddleChildPos() != -1) node.GetLeftChild().SetLeftChild(node.GetMiddleChildPos(),node.GetLeftChildPos());
                if(node.GetRightChildPos() != -1) node.GetLeftChild().SetRightChild(node.GetRightChildPos(),node.GetLeftChildPos());
                node.SetRightChild(node.GetLeftChildPos(), node.GetPos());
                node.SetLeftChild(nodeIns.GetPos(), node.GetPos());
                node.ClearRight();
                node.SetMiddleChild(-1);
                //node.ClearParent();
            }
            else if (node.GetRightKey().CompareTo(nodeIns.GetLeftKey()) < 0) { // rkey < nkey
                var nPos = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetLeftKeyPos(), node.GetLeftDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetRightChild(nPos);
                if(node.GetLeftChildPos() != -1) node.GetRightChild().SetLeftChild(node.GetLeftChildPos(),node.GetRightChildPos());
                if(node.GetMiddleChildPos() != -1) node.GetRightChild().SetRightChild(node.GetMiddleChildPos(),node.GetRightChildPos());
                node.SetLeftChild(node.GetRightChildPos());
                node.SetRightChild(nodeIns.GetPos(), node.GetPos());
                node.SetLeft(node.GetRightKeyPos(), node.GetRightDataPos());
                node.ClearRight();
                node.SetMiddleChild(-1);
                //node.ClearParent();
            }
            else { //lkey < nkey < rkey
                var nPos1 = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetLeftKeyPos(), node.GetLeftDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetMiddleChild(nPos1);
                if(node.GetLeftChildPos() != -1) node.GetMiddleChild().SetLeftChild(node.GetLeftChildPos(), node.GetMiddleChildPos());
                if(nodeIns.GetLeftChildPos() != -1) node.GetMiddleChild().SetRightChild(nodeIns.GetLeftChildPos(), node.GetMiddleChildPos());
                if(node.GetMiddleChildPos() != -1) node.SetLeftChild(node.GetMiddleChildPos(), node.GetPos());
                var nPos2 = _nodesFile.InsertToFile(new BNode<TKey, T>(node.GetRightKeyPos(), node.GetRightDataPos(), node.GetPos(), _keysFile, _dataFile, _nodesFile, _nodesFile.InsertNextWhere()));
                node.SetMiddleChild(nPos2);
                if(nodeIns.GetRightChildPos() != -1) node.GetMiddleChild().SetLeftChild(nodeIns.GetRightChildPos(), node.GetMiddleChildPos());
                if(node.GetRightChildPos() != -1) node.GetMiddleChild().SetRightChild(node.GetRightChildPos(), node.GetMiddleChildPos());
                if(node.GetMiddleChildPos() != -1) node.SetRightChild(node.GetMiddleChildPos(), node.GetPos());
                node.SetLeft(nodeIns.GetLeftKeyPos(),nodeIns.GetLeftDataPos());
                node.ClearRight();
                node.SetMiddleChild(-1);
                //node.ClearParent();
            }
            _nodesFile.UpdateInFIle(node, node.GetPos());
            _nodesFile.UpdateInFIle(nodeIns, nodeIns.GetPos());
        }
//insert block end -----------------------------------------------------------------------------------------------------------
//DELETE block----------------------------------------------------------------------------------------------------------
        private BNode<TKey, T> FindNodeToDelete(TKey keyToFind)
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
        private bool DeleteLeaf(BNode<TKey, T> node, TKey key){
            if (node.Is3Node()) { //remove entry;return true;
                if (key.CompareTo(node.GetLeftKey()) != 0){ node.SetRight(-1,-1);return true;}
                node.SetLeft(node.GetRightKeyPos(),node.GetRightDataPos()); node.SetRight(-1,-1); return true;
            }
            if (!node.Is3Node() && node.GetParentPos() == -1) { _root = null; return true; } //its a root delete last entry; set null root; return true;
            return DeleteInternal(node);
        }
        private bool DeleteInternal(BNode<TKey, T> node){
            var nodeParent = node.GetParent();
            var nodeSteal = nodeParent.GetLeftChild(); //only as a declaration
            if(!nodeParent.Is3Node()){
                if(nodeParent.GetLeftChild() == node)
                {
                    if (nodeParent.GetRightChild().Is3Node()) { //steal from nodeParent.GetRightChild(); rotate ;return true;
                        nodeSteal = nodeParent.GetRightChild();
                        if(nodeSteal.GetLeftChildPos() != -1) node.SetRightChild(nodeSteal.GetLeftChildPos(), node.GetPos());
                        node.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                        nodeParent.SetLeft(nodeSteal.GetLeftKeyPos(),nodeSteal.GetLeftDataPos());
                        nodeSteal.SetLeftChild(nodeSteal.GetMiddleChildPos());
                        nodeSteal.SetLeft(nodeSteal.GetRightKeyPos(), nodeSteal.GetRightDataPos());
                        nodeSteal.SetMiddleChild(-1);
                        nodeSteal.ClearRight();
                        return true;
                    }
                    //merge with papa; return false;
                    var nodeRight = nodeParent.GetRightChild();
                    nodeRight.SetRight(nodeRight.GetLeftKeyPos(),nodeRight.GetLeftDataPos());
                    nodeRight.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                    nodeRight.SetMiddleChild(nodeRight.GetLeftChildPos());
                    if(node.GetLeftChildPos() != -1) nodeRight.SetLeftChild(node.GetLeftChildPos(), nodeRight.GetPos());
                    nodeParent.SetLeftChild(nodeRight.GetPos());
                    nodeParent.SetRightChild(-1);
                    return false;
                }
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate ;return true;
                    nodeSteal = nodeParent.GetLeftChild();
                    node.SetRightChild(node.GetLeftChildPos());
                    if(nodeSteal.GetRightChildPos() != -1) node.SetLeftChild(nodeSteal.GetRightChildPos(), node.GetPos());
                    node.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                    nodeParent.SetLeft(nodeSteal.GetRightKeyPos(),nodeSteal.GetRightDataPos());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChildPos());
                    nodeSteal.SetMiddleChild(-1);
                    nodeSteal.ClearRight();
                    return true;
                }
                //merge with papa; return false;
                var nodeLeft = nodeParent.GetLeftChild();
                nodeLeft.SetRight(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChildPos());
                if(node.GetLeftChildPos() != -1) nodeLeft.SetRightChild(node.GetLeftChildPos(), nodeLeft.GetPos());
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
                    if(nodeSteal.GetLeftChildPos() != -1) node.SetRightChild(nodeSteal.GetLeftChildPos(), node.GetPos());
                    node.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                    nodeParent.SetLeft(nodeSteal.GetLeftKeyPos(),nodeSteal.GetLeftDataPos());
                    nodeSteal.SetLeftChild(nodeSteal.GetMiddleChildPos());
                    nodeSteal.SetLeft(nodeSteal.GetRightKeyPos(), nodeSteal.GetRightDataPos());
                    nodeSteal.SetMiddleChild(-1);
                    nodeSteal.ClearRight();
                    return true;
                }
                else//parent.middle child is not a 3node
                {
                    var nodeMid = nodeParent.GetMiddleChild();
                    node.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                    node.SetRight(nodeMid.GetLeftKeyPos(),nodeMid.GetLeftDataPos());
                    if(nodeMid.GetLeftChildPos() != -1) node.SetMiddleChild(nodeMid.GetLeftChildPos(), node.GetPos());
                    if(nodeMid.GetRightChildPos() != -1) node.SetRightChild(nodeMid.GetRightChildPos(), node.GetPos());
                    nodeParent.SetMiddleChild(-1);
                    nodeParent.SetLeft(nodeParent.GetRightKeyPos(),nodeParent.GetRightDataPos());
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
                    node.SetRightChild(node.GetLeftChildPos()); //ever get here: haha prob duplicate
                    if (nodeSteal.GetRightChildPos() != -1) node.SetLeftChild(nodeSteal.GetRightChildPos(), node.GetPos());
                    node.SetLeft(nodeParent.GetRightKeyPos(), nodeParent.GetRightDataPos());
                    nodeParent.SetRight(nodeSteal.GetRightKeyPos(), nodeSteal.GetRightDataPos());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChildPos());
                    nodeSteal.SetMiddleChild(-1);
                    nodeSteal.ClearRight();
                    return true;
                }
                else//parent.middle child is not a 3node
                {
                    var nodeMid = nodeParent.GetMiddleChild();
                    node.SetLeft(nodeMid.GetLeftKeyPos(),nodeMid.GetLeftDataPos());
                    node.SetRight(nodeParent.GetRightKeyPos(),nodeParent.GetRightDataPos());
                    node.SetRightChild(node.GetLeftChildPos());
                    if(nodeMid.GetLeftChildPos() != -1) node.SetMiddleChild(nodeMid.GetRightChildPos(), node.GetPos());
                    if(nodeMid.GetRightChildPos() != -1) node.SetLeftChild(nodeMid.GetLeftChildPos(), node.GetPos());
                    nodeParent.SetMiddleChild(-1);
                    nodeParent.ClearRight();
                    return true;
                }
            }
            if(nodeParent.GetMiddleChild() == node) {
                if (nodeParent.GetLeftChild().Is3Node()) { //steal from nodeParent.GetLeftChild(); rotate; return true;
                    nodeSteal = nodeParent.GetLeftChild(); //right rotation
                    node.SetRightChild(node.GetLeftChildPos());
                    if(nodeSteal.GetRightChildPos() != -1) node.SetLeftChild(nodeSteal.GetRightChildPos(), node.GetPos());
                    node.SetLeft(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                    nodeParent.SetLeft(nodeSteal.GetRightKeyPos(),nodeSteal.GetRightDataPos());
                    nodeSteal.SetRightChild(nodeSteal.GetMiddleChildPos());
                    nodeSteal.SetMiddleChild(-1);
                    nodeSteal.ClearRight();
                    return true;
                }
                if(nodeParent.GetRightChild().Is3Node()){ //steal from nodeParent.GetRightChild(); rotate; return true;
                    nodeSteal = nodeParent.GetRightChild(); //left rotation
                    if(nodeSteal.GetLeftChildPos() != -1) node.SetRightChild(nodeSteal.GetLeftChildPos(), node.GetPos());
                    node.SetLeft(nodeParent.GetRightKeyPos(),nodeParent.GetRightDataPos());
                    nodeParent.SetRight(nodeSteal.GetLeftKeyPos(),nodeSteal.GetLeftDataPos());
                    nodeSteal.SetLeftChild(nodeSteal.GetMiddleChildPos());
                    nodeSteal.SetLeft(nodeSteal.GetRightKeyPos(), nodeSteal.GetRightDataPos());
                    nodeSteal.SetMiddleChild(-1);
                    nodeSteal.ClearRight();
                    return true;
                }
                var nodeLeft = nodeParent.GetLeftChild(); //left rotation - kindof
                nodeLeft.SetMiddleChild(nodeLeft.GetRightChildPos());
                if(node.GetLeftChildPos() != -1) nodeLeft.SetRightChild(node.GetLeftChildPos(), nodeLeft.GetPos());
                nodeLeft.SetRight(nodeParent.GetLeftKeyPos(),nodeParent.GetLeftDataPos());
                nodeParent.SetLeft(nodeParent.GetRightKeyPos(),nodeParent.GetRightDataPos());
                nodeParent.ClearRight();
                nodeParent.SetMiddleChild(-1);
                return true;//idk if true
            }
            throw new Exception("shouldnt be here in: tree: DeleteInternal");
            //merge with papa; return false;
        }
        private BNode<TKey, T> DeleteSwitch(BNode<TKey, T> node, TKey key)
        {
            if (key.CompareTo(node.GetLeftKey()) == 0) {//key is leftval
                var tempData = node.GetLeftDataPos();
                var switchNode = this.InOrderNextNode(node, key);
                node.SetLeft(switchNode.GetLeftKeyPos(),switchNode.GetLeftDataPos());
                switchNode.SetLeft(_keysFile.FindWhere(key),tempData);
                return switchNode;
            }
            else { //key is rightval
                var tempData = node.GetRightDataPos();
                var switchNode = this.InOrderNextNode(node, key);
                node.SetRight(switchNode.GetLeftKeyPos(),switchNode.GetLeftDataPos());
                switchNode.SetLeft(_keysFile.FindWhere(key), tempData);
                return switchNode;
            }
        }
//delete block end -----------------------------------------------------------------------------------------------------------
        public BNode<TKey, T> GetRoot()
        {
            return _root;
        }
        public void SetRoot(BNode<TKey, T> newRoot)
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
            var que = new Queue<BNode<TKey,T>>();
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
        public void GetAllLeaves(BNode<TKey,T> n, ref ArrayList ll){
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
            var control = ((BNode<TKey, T>) listofleaves[0]).GetLevel();
            foreach (var list in listofleaves) {
                if (control != ((BNode<TKey, T>) list).GetLevel()) return false;
            }
            return true;
        }
        public override string ToString()
        {
            if (_root == null) return "";
            var que = new Queue<BNode<TKey, T>>();
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
        //----------------------------------------traversals--------------------------------------
        public void InOrderRecursion(BNode<TKey, T> n, ref ArrayList ll)
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
        public ref BNode<TKey, T> InOrderNextNodeRef(ref BNode<TKey, T> n, TKey k)
        {
            if(n.Is3Node() && n.GetLeftKey().CompareTo(k) == 0) n = n.GetMiddleChild();
            else n = n.GetRightChild();
            while (!n.IsLeaf()) {
                n = n.GetLeftChild();
            }
            return ref n;
        }
        public List<T> LevelOrder(BNode<TKey, T> n)
        {
            var listLO = new List<T>();
            if (_root == null) return listLO;
            var que = new Queue<BNode<TKey, T>>();
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
        public BNode<TKey, T> FindNodeMin(TKey minK)
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
        public T InOrderNextData(BNode<TKey, T> n, TKey k)
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
        public TKey InOrderNextKey(BNode<TKey, T> n, TKey k)
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
        public BNode<TKey, T> InOrderNextNode(BNode<TKey, T> n, TKey k)
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