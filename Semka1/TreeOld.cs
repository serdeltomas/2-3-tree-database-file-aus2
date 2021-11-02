using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;

namespace Semka1
{
    public class T23Tree<T> where T: IComparable<T>
    {
        private T23Node<T> _root;
        private int _depth;

        // null = dnf;
        public ref T23Node<T> FindExactNode(T keyToFind)
        {
            ref T23Node<T> searchNow = ref _root;
            int compare1, compare2;
            for (int i = 0; i < _depth; i++)
            {
                if (searchNow == null)
                {
                    return ref searchNow;
                }
                
                compare1 = searchNow._data1.CompareTo(keyToFind);
                compare2 = searchNow._data2.CompareTo(keyToFind);
                
                if (compare1 == 0 || compare2 == 0)
                {
                    return ref searchNow;
                }
                if (compare1 > 0)
                {
                    searchNow = searchNow.GetLeftChild();
                }
                else if ((!searchNow._is3Node && compare1 < 0) 
                         || (searchNow._is3Node && compare2 < 0))
                {
                    searchNow = searchNow.GetRightChild();
                }
                else if (searchNow._is3Node && compare1 < 0 && compare2 > 0)
                {
                    searchNow = searchNow.GetMiddleChild();
                }
                else
                {
                    throw new Exception("idk what hapenned in: FindExactNode");
                }
            }
            throw new Exception("should never get here in: FindExactNode");
            //return null;
        }
        
        //null = empty tree
        public ref T23Node<T> FindNodeToInsert(T keyToFind)
        {
            ref var searchNow = ref _root;
            //ref var returnVal = ref searchNow;
            int compare1, compare2;
            if (_root == null)
            {
                return ref _root;
            }
            for (int i = 0; i < _depth; i++)
            {
                if (searchNow._isLeaf)
                {
                    //ref var returnVal = ref searchNow;
                    return ref searchNow;
                }

                compare1 = keyToFind.CompareTo(searchNow._data1);
                compare2 = keyToFind.CompareTo(searchNow._data2);
                
                if (compare1 == 0 || compare2 == 0)
                {
                    throw new Exception("duplicate key error in: FindNodeToInsert");
                }
                if (compare1 < 0)
                {
                    searchNow = ref searchNow.GetLeftChild();
                }
                else if ((!searchNow._is3Node && compare1 > 0) 
                         || (searchNow._is3Node && compare2 > 0))
                {
                    searchNow = ref searchNow.GetRightChild();
                }
                else if (searchNow._is3Node && compare1 > 0 && compare2 < 0)
                {
                    searchNow = ref searchNow.GetMiddleChild();
                }
                else
                {
                    throw new Exception("idk what hapenned in: FindNodeToInsert");
                }
            }
            throw new Exception("should never get here: FindNodeToInsert");
            //return null;
        }

        public bool InsertData(T dataToInsert)
        {
            //System.Console.WriteLine(ReloadParents());
            var firstWorkingNode = new T23Node<T>();
            ref T23Node<T> workingNode = ref firstWorkingNode;
            try
            {
                ref T23Node<T> tryNode = ref FindNodeToInsert(dataToInsert);
                workingNode = ref tryNode;
            }
            catch (NullReferenceException e )
            {
                _root = new T23Node<T>(true, false, dataToInsert); //why did i get here?
                _depth++;
                return true;
            }

            if (workingNode == null) 
            {
                _root = new T23Node<T>(true, false, dataToInsert);
                _depth++;
                return true;
            }
            
            for (int i = 0; i < _depth; i++)
            {
                if (!InsertNode(ref workingNode, dataToInsert)) // inserted but parent needs work
                {
                    if (workingNode.GetParent() == null)
                    {
                        _root = MakeNewRoot(ref workingNode);
                        _depth++;
                        return true;
                    }
                    else // has a parent
                    {
                        dataToInsert = workingNode._extraData;
                        //workingNode._extraData = default(T); //dont delete extra data here and now
                        //var workingNodeOld = workingNode;
                        workingNode = ref workingNode.GetParent();
                    }
                }
                else
                {
                    break;
                }
            } 
            return true;
        }
        
        public bool InsertNode(ref T23Node<T> workNode, T dataNew) //true if no extras created
        {
            if (workNode._isLeaf)
            {
                if (workNode._is3Node)
                {
                    workNode._is3Node = false;
                    if (workNode._data1.CompareTo(dataNew) > 0)
                    {
                        workNode._extraData = workNode._data1;
                        workNode._data1 = dataNew;
                        return false;
                    }
                    else if (workNode._data2.CompareTo(dataNew) < 0)
                    {
                        workNode._extraData = workNode._data2;
                        workNode._data2 = dataNew;
                        return false;
                    }
                    else
                    {
                        workNode._extraData = dataNew;
                        return false;
                    }
                }
                else
                {
                    workNode._is3Node = true;
                    if (workNode._data1.CompareTo(dataNew) > 0)
                    {
                        workNode._data2 = workNode._data1;
                        workNode._data1 = dataNew;
                        return true;
                    }
                    else
                    {
                        workNode._data2 = dataNew;
                        return true;
                    }
                }
            }
            else // not a leaf
            {
                if (workNode._is3Node)
                {
                    workNode._is3Node = false;
                    if (workNode._data1.CompareTo(dataNew) > 0) // data1 > dataNew
                    {
                        if (workNode.GetParent() == null || !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null && workNode.GetMiddleRightExtraChild() == null)
                            { 
                                workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                                var newNode = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetLeftChild()._data1, ref workNode);
                                var newNode2 = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetLeftChild()._data2, ref workNode);
                                workNode.SetLeftChild(ref newNode);
                                workNode.SetMiddleLeftExtraChild(ref newNode2);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                            }
                            return false;
                        }
                        else //has parent which is a 3-node
                        {//should work
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            ref var workNodeKid = ref workNode;
                            ref var workNodeParent = ref workNode.GetParent();
                            //if (workNodeKid._data1.CompareTo(workNodeParent.GetLeftChild()._data1) == 0)
                            //{
                            var newRight = new T23Node<T>(false, false,
                                workNodeKid._data2, ref workNodeParent);
                            newRight.SetRightChild(ref workNodeKid.GetRightChild());
                            newRight.SetLeftChild(ref workNodeKid.GetMiddleChild()); //breaks here at #68 seed1 (not anymore)
                            workNodeParent.SetMiddleLeftExtraChild(ref newRight);
                            newRight.GetLeftChild().SetParent(ref newRight); // breaks here at #7
                            newRight.GetRightChild().SetParent(ref newRight);
                            workNodeKid._data2 = default(T);
                            workNodeKid._is3Node = false;
                            var leftleft = new T23Node<T>(workNodeKid.GetLeftChild()._isLeaf,
                                workNodeKid.GetLeftChild()._is3Node,
                                workNodeKid.GetLeftChild()._data1, ref workNodeKid);
                            var leftright = new T23Node<T>(workNodeKid.GetLeftChild()._isLeaf,
                                workNodeKid.GetLeftChild()._is3Node,
                                workNodeKid.GetLeftChild()._data2, ref workNodeKid);
                            workNodeKid.SetLeftChild(ref leftleft);
                            workNodeKid.SetRightChild(ref leftright);
                            workNodeParent.SetMiddleRightExtraChild(ref workNodeParent.GetMiddleChild());
                            T23Node<T> nullVar = null;
                            workNodeParent.SetMiddleChild(ref nullVar);
                            workNodeKid.SetMiddleChild(ref nullVar);
                            /*}
                            else if (workNodeKid._data1.CompareTo(workNodeParent.GetMiddleChild()._data1) == 0)
                            {
                                
                            }
                            else if (workNodeKid._data1.CompareTo(workNodeParent.GetRightChild()._data1) == 0)
                            {
                                var newRight = new T23Node<T>(false, false,
                                    workNodeKid._data2, ref workNodeParent);
                                newRight.SetRightChild(ref workNodeKid.GetRightChild());
                                newRight.SetLeftChild(ref workNodeKid.GetMiddleChild()); //breaks here at #68 seed1 (not anymore)
                                workNodeParent.SetMiddleLeftExtraChild(ref newRight);
                                newRight.GetLeftChild().SetParent(ref newRight);
                                newRight.GetRightChild().SetParent(ref newRight);
                                workNodeKid._data2 = default(T);
                                workNodeKid._is3Node = false;
                                var Leftleft = new T23Node<T>(workNodeKid.GetLeftChild()._isLeaf,
                                    workNodeKid.GetLeftChild()._is3Node,
                                    workNodeKid.GetLeftChild()._data1, ref workNodeKid);
                                var Leftright = new T23Node<T>(workNodeKid.GetLeftChild()._isLeaf,
                                    workNodeKid.GetLeftChild()._is3Node,
                                    workNodeKid.GetLeftChild()._data2, ref workNodeKid);
                                workNodeKid.SetLeftChild(ref Leftleft);
                                workNodeKid.SetRightChild(ref Leftright);
                                workNodeParent.SetMiddleRightExtraChild(ref workNodeParent.GetMiddleChild());
                                T23Node<T> nullVar = null;
                                workNodeParent.SetMiddleChild(ref nullVar);
                                workNodeKid.SetMiddleChild(ref nullVar);
                            }
                            else
                            {
                                throw new Exception("\n should never end here in: InsertNode");
                            }*/

                            //workNode = ref workNodeKid;
                            return false;
                        }
                    }
                    else if (workNode._data2.CompareTo(dataNew) < 0) //data2 < dataNew
                    {
                        if (workNode.GetParent() == null || !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {//should work
                            workNode._extraData = workNode._data2;
                            workNode._data2 = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null &&
                                workNode.GetMiddleRightExtraChild() == null)
                            {
                                workNode.SetMiddleLeftExtraChild(ref workNode.GetMiddleChild());
                                workNode.SetMiddleRightExtraChild(ref workNode.GetRightChild());
                                //_extraRightMiddleChild._data1 = _extraRightMiddleChild._data2;
                                var newNode = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetRightChild()._data2,
                                    ref workNode.GetMiddleRightExtraChild().GetParent());
                                workNode.SetRightChild(ref newNode);
                                workNode.GetMiddleRightExtraChild()._data2 = default(T);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                                //_rightChild._parent = _extraRightMiddleChild._parent;
                            }
                            return false;
                        }
                        else //has parent which is a 3-node
                        {//not implemented properly
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            workNode = ref workNode.GetParent();
                            var newRight = new T23Node<T>(false, false,
                                workNode.GetLeftChild()._data2, ref workNode);
                            newRight.SetRightChild(ref workNode.GetLeftChild().GetRightChild(), ref newRight);//maybe have father assignment for later
                            newRight.SetLeftChild(ref workNode.GetLeftChild().GetMiddleChild(), ref newRight);//maybe have father assignment for later
                            workNode.SetMiddleLeftExtraChild(ref newRight);
                            workNode.GetLeftChild()._data2 = default(T);
                            workNode.GetLeftChild()._is3Node = false;
                            var leftleft = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data1, ref workNode.GetLeftChild());
                            var leftright = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data2, ref workNode.GetLeftChild());
                            workNode.GetLeftChild().SetLeftChild(ref leftleft);
                            workNode.GetLeftChild().SetRightChild(ref leftright);
                            workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                            
                            T23Node<T> nullVar = null;
                            workNode.SetMiddleChild(ref nullVar);
                            return false;
                        }
                    }
                    else // data1 < dataNew < data2
                    {
                        if (workNode.GetParent() == null ||
                            !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {//should work
                            workNode._extraData = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null &&
                                workNode.GetMiddleRightExtraChild() == null)
                            {
                                workNode.SetMiddleLeftExtraChild(ref workNode.GetMiddleChild());
                                var newNode = new T23Node<T>(workNode.GetMiddleChild()._isLeaf,
                                    workNode.GetMiddleChild()._is3Node,
                                    workNode.GetMiddleChild()._data2, ref workNode.GetMiddleChild().GetParent());
                                workNode.SetMiddleRightExtraChild(ref newNode);
                                workNode.GetMiddleLeftExtraChild()._data2 = default(T);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                            }
                            return false;
                        }
                        else
                        {//not implemented properly
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            workNode = ref workNode.GetParent();
                            var newRight = new T23Node<T>(false, false,
                                workNode.GetLeftChild()._data2, ref workNode);
                            newRight.SetRightChild(ref workNode.GetLeftChild().GetRightChild(), ref newRight);//maybe have father assignment for later
                            newRight.SetLeftChild(ref workNode.GetLeftChild().GetMiddleChild(), ref newRight);//maybe have father assignment for later
                            workNode.SetMiddleLeftExtraChild(ref newRight);
                            workNode.GetLeftChild()._data2 = default(T);
                            workNode.GetLeftChild()._is3Node = false;
                            var leftleft = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data1, ref workNode.GetLeftChild());
                            var leftright = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data2, ref workNode.GetLeftChild());
                            workNode.GetLeftChild().SetLeftChild(ref leftleft);
                            workNode.GetLeftChild().SetRightChild(ref leftright);
                            workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                            
                            T23Node<T> nullVar = null;
                            workNode.SetMiddleChild(ref nullVar);
                            return false;
                        }
                    }
                }
                else //not a 3 node
                {
                    workNode._is3Node = true;
                    if (workNode._data1.CompareTo(dataNew) > 0)//data1 > dataNew
                    {
                        workNode._data2 = workNode._data1;
                        workNode._data1 = dataNew;
                        var left = new T23Node<T>(workNode.GetLeftChild()._isLeaf, false, workNode.GetLeftChild()._data1, ref workNode);
                        var middle = new T23Node<T>(workNode.GetLeftChild()._isLeaf, false, workNode.GetLeftChild()._data2, ref workNode);
                        
                        if (workNode.GetLeftChild().GetMiddleLeftExtraChild() == null &&
                            workNode.GetLeftChild().GetMiddleRightExtraChild() == null)
                        {
                            workNode.SetLeftChild(ref left);
                            workNode.SetMiddleChild(ref middle);
                        }
                        else
                        {
                            ref var extraChildren = ref workNode.GetLeftChild();
                            var leftleft = new T23Node<T>(extraChildren.GetLeftChild()._isLeaf, false,
                                extraChildren.GetLeftChild()._data1, ref left);
                            var leftright = new T23Node<T>(extraChildren.GetLeftChild()._isLeaf, false,
                                extraChildren.GetMiddleLeftExtraChild()._data1, ref left);
                            left.SetLeftChild(ref leftleft);
                            left.SetRightChild(ref leftright);
                            middle.SetLeftChild(ref extraChildren.GetMiddleRightExtraChild());
                            middle.GetLeftChild().SetParent(ref middle);
                            middle.SetRightChild(ref extraChildren.GetRightChild());
                            middle.GetRightChild().SetParent(ref middle);
                            workNode.SetLeftChild(ref left);
                            workNode.SetMiddleChild(ref middle);
                        }
                        return true;
                    }
                    else //data1 < dataNew
                    {
                        workNode._data2 = dataNew;
                        var newNode = new T23Node<T>(workNode.GetRightChild()._isLeaf, false, workNode.GetRightChild()._data1, ref workNode);
                        var newNode2 =new T23Node<T>(workNode.GetRightChild()._isLeaf, false, workNode.GetRightChild()._data2, ref workNode);
                        if (workNode.GetRightChild().GetMiddleLeftExtraChild() == null &&
                            workNode.GetRightChild().GetMiddleRightExtraChild() == null)
                        {
                            workNode.SetMiddleChild(ref newNode);
                            workNode.SetRightChild(ref newNode2);
                        }
                        else
                        {
                            ref var extraChildren = ref workNode.GetRightChild();
                            newNode.SetLeftChild(ref extraChildren.GetLeftChild());
                            newNode.GetLeftChild().SetParent(ref newNode);
                            newNode.SetRightChild(ref extraChildren.GetMiddleLeftExtraChild());
                            newNode.GetRightChild().SetParent(ref newNode);
                            newNode2.SetLeftChild(ref extraChildren.GetMiddleRightExtraChild());
                            newNode2.GetLeftChild().SetParent(ref newNode2);
                            newNode2.SetRightChild(ref extraChildren.GetRightChild());
                            newNode2.GetRightChild().SetParent(ref newNode2);
                            workNode.SetMiddleChild(ref newNode);
                            workNode.SetRightChild(ref newNode2);
                        }
                        return true;
                    }
                }
            }
        }
        public bool InsertNodeBackup(ref T23Node<T> workNode, T dataNew) //true if no extras created
        {
            if (workNode._isLeaf)
            {
                if (workNode._is3Node)
                {
                    workNode._is3Node = false;
                    if (workNode._data1.CompareTo(dataNew) > 0)
                    {
                        workNode._extraData = workNode._data1;
                        workNode._data1 = dataNew;
                        return false;
                    }
                    else if (workNode._data2.CompareTo(dataNew) < 0)
                    {
                        workNode._extraData = workNode._data2;
                        workNode._data2 = dataNew;
                        return false;
                    }
                    else
                    {
                        workNode._extraData = dataNew;
                        return false;
                    }
                }
                else
                {
                    workNode._is3Node = true;
                    if (workNode._data1.CompareTo(dataNew) > 0)
                    {
                        workNode._data2 = workNode._data1;
                        workNode._data1 = dataNew;
                        return true;
                    }
                    else
                    {
                        workNode._data2 = dataNew;
                        return true;
                    }
                }
            }
            else // not a leaf
            {
                if (workNode._is3Node)
                {
                    workNode._is3Node = false;
                    if (workNode._data1.CompareTo(dataNew) > 0) // data1 > dataNew
                    {
                        if (workNode.GetParent() == null || !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null && workNode.GetMiddleRightExtraChild() == null)
                            { 
                                workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                                var newNode = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetLeftChild()._data1, ref workNode);
                                var newNode2 = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetLeftChild()._data2, ref workNode);
                                workNode.SetLeftChild(ref newNode);
                                workNode.SetMiddleLeftExtraChild(ref newNode2);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                            }
                            return false;
                        }
                        else //has parent which is a 3-node
                        {//should work
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            workNode = ref workNode.GetParent();
                            var newRight = new T23Node<T>(false, false,
                                workNode.GetLeftChild()._data2, ref workNode);
                            newRight.SetRightChild(ref workNode.GetLeftChild().GetRightChild());
                            newRight.SetLeftChild(ref workNode.GetLeftChild().GetMiddleChild());//breaks here at #68 seed1 (not anymore)
                            workNode.SetMiddleLeftExtraChild(ref newRight);
                            newRight.GetLeftChild().SetParent(ref newRight);
                            newRight.GetRightChild().SetParent(ref newRight);
                            workNode.GetLeftChild()._data2 = default(T);
                            workNode.GetLeftChild()._is3Node = false;
                            var leftleft = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data1, ref workNode.GetLeftChild());
                            var leftright = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data2, ref workNode.GetLeftChild());
                            workNode.GetLeftChild().SetLeftChild(ref leftleft);
                            workNode.GetLeftChild().SetRightChild(ref leftright);
                            workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                            
                            T23Node<T> nullVar = null;
                            workNode.SetMiddleChild(ref nullVar);
                            workNode.GetLeftChild().SetMiddleChild(ref nullVar);
                            workNode = ref workNode.GetLeftChild();
                            return false;
                        }
                    }
                    else if (workNode._data2.CompareTo(dataNew) < 0) //data2 < dataNew
                    {
                        if (workNode.GetParent() == null || !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {//should work
                            workNode._extraData = workNode._data2;
                            workNode._data2 = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null &&
                                workNode.GetMiddleRightExtraChild() == null)
                            {
                                workNode.SetMiddleLeftExtraChild(ref workNode.GetMiddleChild());
                                workNode.SetMiddleRightExtraChild(ref workNode.GetRightChild());
                                //_extraRightMiddleChild._data1 = _extraRightMiddleChild._data2;
                                var newNode = new T23Node<T>(workNode.GetLeftChild()._isLeaf,
                                    workNode.GetLeftChild()._is3Node,
                                    workNode.GetRightChild()._data2,
                                    ref workNode.GetMiddleRightExtraChild().GetParent());
                                workNode.SetRightChild(ref newNode);
                                workNode.GetMiddleRightExtraChild()._data2 = default(T);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                                //_rightChild._parent = _extraRightMiddleChild._parent;
                            }
                            return false;
                        }
                        else //has parent which is a 3-node
                        {//not implemented properly
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            workNode = ref workNode.GetParent();
                            var newRight = new T23Node<T>(false, false,
                                workNode.GetLeftChild()._data2, ref workNode);
                            newRight.SetRightChild(ref workNode.GetLeftChild().GetRightChild(), ref newRight);//maybe have father assignment for later
                            newRight.SetLeftChild(ref workNode.GetLeftChild().GetMiddleChild(), ref newRight);//maybe have father assignment for later
                            workNode.SetMiddleLeftExtraChild(ref newRight);
                            workNode.GetLeftChild()._data2 = default(T);
                            workNode.GetLeftChild()._is3Node = false;
                            var leftleft = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data1, ref workNode.GetLeftChild());
                            var leftright = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data2, ref workNode.GetLeftChild());
                            workNode.GetLeftChild().SetLeftChild(ref leftleft);
                            workNode.GetLeftChild().SetRightChild(ref leftright);
                            workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                            
                            T23Node<T> nullVar = null;
                            workNode.SetMiddleChild(ref nullVar);
                            return false;
                        }
                    }
                    else // data1 < dataNew < data2
                    {
                        if (workNode.GetParent() == null ||
                            !workNode.GetParent()._is3Node) //has no parent or parent is a 2-node
                        {//should work
                            workNode._extraData = dataNew;
                            if (workNode.GetMiddleLeftExtraChild() == null &&
                                workNode.GetMiddleRightExtraChild() == null)
                            {
                                workNode.SetMiddleLeftExtraChild(ref workNode.GetMiddleChild());
                                var newNode = new T23Node<T>(workNode.GetMiddleChild()._isLeaf,
                                    workNode.GetMiddleChild()._is3Node,
                                    workNode.GetMiddleChild()._data2, ref workNode.GetMiddleChild().GetParent());
                                workNode.SetMiddleRightExtraChild(ref newNode);
                                workNode.GetMiddleLeftExtraChild()._data2 = default(T);
                                T23Node<T> nullVar = null;
                                workNode.SetMiddleChild(ref nullVar);
                            }
                            return false;
                        }
                        else
                        {//not implemented properly
                            workNode._extraData = workNode._data1;
                            workNode._data1 = dataNew;
                            workNode = ref workNode.GetParent();
                            var newRight = new T23Node<T>(false, false,
                                workNode.GetLeftChild()._data2, ref workNode);
                            newRight.SetRightChild(ref workNode.GetLeftChild().GetRightChild(), ref newRight);//maybe have father assignment for later
                            newRight.SetLeftChild(ref workNode.GetLeftChild().GetMiddleChild(), ref newRight);//maybe have father assignment for later
                            workNode.SetMiddleLeftExtraChild(ref newRight);
                            workNode.GetLeftChild()._data2 = default(T);
                            workNode.GetLeftChild()._is3Node = false;
                            var leftleft = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data1, ref workNode.GetLeftChild());
                            var leftright = new T23Node<T>(workNode.GetLeftChild().GetLeftChild()._isLeaf, workNode.GetLeftChild().GetLeftChild()._is3Node,
                                workNode.GetLeftChild().GetLeftChild()._data2, ref workNode.GetLeftChild());
                            workNode.GetLeftChild().SetLeftChild(ref leftleft);
                            workNode.GetLeftChild().SetRightChild(ref leftright);
                            workNode.SetMiddleRightExtraChild(ref workNode.GetMiddleChild());
                            
                            T23Node<T> nullVar = null;
                            workNode.SetMiddleChild(ref nullVar);
                            return false;
                        }
                    }
                }
                else //not a 3 node
                {
                    workNode._is3Node = true;
                    if (workNode._data1.CompareTo(dataNew) > 0)//data1 > dataNew
                    {
                        workNode._data2 = workNode._data1;
                        workNode._data1 = dataNew;
                        var left = new T23Node<T>(workNode.GetLeftChild()._isLeaf, false, workNode.GetLeftChild()._data1, ref workNode);
                        var middle = new T23Node<T>(workNode.GetLeftChild()._isLeaf, false, workNode.GetLeftChild()._data2, ref workNode);
                        
                        if (workNode.GetLeftChild().GetMiddleLeftExtraChild() == null &&
                            workNode.GetLeftChild().GetMiddleRightExtraChild() == null)
                        {
                            workNode.SetLeftChild(ref left);
                            workNode.SetMiddleChild(ref middle);
                        }
                        else
                        {
                            ref var extraChildren = ref workNode.GetLeftChild();
                            var leftleft = new T23Node<T>(extraChildren.GetLeftChild()._isLeaf, false,
                                extraChildren.GetLeftChild()._data1, ref left);
                            var leftright = new T23Node<T>(extraChildren.GetLeftChild()._isLeaf, false,
                                extraChildren.GetMiddleLeftExtraChild()._data1, ref left);
                            left.SetLeftChild(ref leftleft);
                            left.SetRightChild(ref leftright);
                            middle.SetLeftChild(ref extraChildren.GetMiddleRightExtraChild());
                            middle.GetLeftChild().SetParent(ref middle);
                            middle.SetRightChild(ref extraChildren.GetRightChild());
                            middle.GetRightChild().SetParent(ref middle);
                            workNode.SetLeftChild(ref left);
                            workNode.SetMiddleChild(ref middle);
                        }
                        return true;
                    }
                    else //data1 < dataNew
                    {
                        workNode._data2 = dataNew;
                        var newNode = new T23Node<T>(workNode.GetRightChild()._isLeaf, false, workNode.GetRightChild()._data1, ref workNode);
                        var newNode2 =new T23Node<T>(workNode.GetRightChild()._isLeaf, false, workNode.GetRightChild()._data2, ref workNode);
                        if (workNode.GetRightChild().GetMiddleLeftExtraChild() == null &&
                            workNode.GetRightChild().GetMiddleRightExtraChild() == null)
                        {
                            workNode.SetMiddleChild(ref newNode);
                            workNode.SetRightChild(ref newNode2);
                        }
                        else
                        {
                            ref var extraChildren = ref workNode.GetRightChild();
                            newNode.SetLeftChild(ref extraChildren.GetLeftChild());
                            newNode.GetLeftChild().SetParent(ref newNode);
                            newNode.SetRightChild(ref extraChildren.GetMiddleLeftExtraChild());
                            newNode.GetRightChild().SetParent(ref newNode);
                            newNode2.SetLeftChild(ref extraChildren.GetMiddleRightExtraChild());
                            newNode2.GetLeftChild().SetParent(ref newNode2);
                            newNode2.SetRightChild(ref extraChildren.GetRightChild());
                            newNode2.GetRightChild().SetParent(ref newNode2);
                            workNode.SetMiddleChild(ref newNode);
                            workNode.SetRightChild(ref newNode2);
                        }
                        return true;
                    }
                }
            }
        }
        public ref T23Node<T> MakeNewRoot(ref T23Node<T> workNode)
        {
            ref var topNode = ref workNode;
            if (workNode.GetMiddleLeftExtraChild() == null && workNode.GetMiddleRightExtraChild() == null) //has no extra kids 
            {
                var newParent = new T23Node<T>(false, false, workNode._extraData);
                workNode.SetParent(ref newParent);
                var newNode = new T23Node<T>(workNode._isLeaf, false, workNode._data2, ref workNode.GetParent());
                workNode.GetParent().SetRightChild(ref newNode); 
                var newNode2  = new T23Node<T>(workNode._isLeaf, false, workNode._data1, ref workNode.GetParent());
                workNode.GetParent().SetLeftChild(ref newNode2);
                topNode = ref workNode.GetParent();
                //delete old nodes here maybe?
            }
            else //needs splitting
            {
                var newParent = new T23Node<T>(false, false, workNode._extraData);
                workNode.SetParent(ref newParent);
                ref var holdLeftChild  = ref workNode.GetLeftChild();
                ref var holdRightChild  = ref workNode.GetRightChild();
                ref var holdMELeftChild  = ref workNode.GetMiddleLeftExtraChild();
                ref var holdMERightChild  = ref workNode.GetMiddleRightExtraChild();
                var newNode = new T23Node<T>(workNode._isLeaf, false, workNode._data2, ref workNode.GetParent());
                var newNode2 = new T23Node<T>(workNode._isLeaf, false, workNode._data1, ref workNode.GetParent());
                
                topNode = ref workNode.GetParent();
                workNode = ref workNode.GetParent();
                workNode.SetRightChild(ref newNode);
                workNode.SetLeftChild(ref newNode2);
                workNode.GetLeftChild().SetLeftChild(ref holdLeftChild);
                workNode.GetLeftChild().GetLeftChild().SetParent(ref workNode.GetLeftChild());
                workNode.GetRightChild().SetRightChild(ref holdRightChild);
                workNode.GetRightChild().GetRightChild().SetParent(ref workNode.GetRightChild());
                workNode.GetLeftChild().SetRightChild(ref holdMELeftChild);
                workNode.GetLeftChild().GetRightChild().SetParent(ref workNode.GetLeftChild());
                workNode.GetRightChild().SetLeftChild(ref holdMERightChild);
                workNode.GetRightChild().GetLeftChild().SetParent(ref workNode.GetRightChild());
            }

            return ref topNode;
        }
        
        public ref T23Node<T> MakeNewLocalRoot(ref T23Node<T> workNode)
        {
            ref var topNode = ref workNode;
            if (workNode.GetMiddleLeftExtraChild() == null && workNode.GetMiddleRightExtraChild() == null) //has no extra kids 
            {
                var newParent = new T23Node<T>(false, false, workNode._extraData);
                workNode.SetParent(ref newParent);
                var newNode = new T23Node<T>(workNode._isLeaf, false, workNode._data2, ref workNode.GetParent());
                workNode.GetParent().SetRightChild(ref newNode); 
                var newNode2  = new T23Node<T>(workNode._isLeaf, false, workNode._data1, ref workNode.GetParent());
                workNode.GetParent().SetLeftChild(ref newNode2);
                topNode = ref workNode.GetParent();
                //delete old nodes here maybe?
            }
            else //needs splitting
            {
                var newParent = new T23Node<T>(false, false, workNode._extraData);
                workNode.SetParent(ref newParent);
                ref var holdLeftChild  = ref workNode.GetLeftChild();
                ref var holdRightChild  = ref workNode.GetRightChild();
                ref var holdMELeftChild  = ref workNode.GetMiddleLeftExtraChild();
                ref var holdMERightChild  = ref workNode.GetMiddleRightExtraChild();
                var newNode = new T23Node<T>(workNode._isLeaf, false, workNode._data2, ref workNode.GetParent());
                var newNode2 = new T23Node<T>(workNode._isLeaf, false, workNode._data1, ref workNode.GetParent());
                
                topNode = ref workNode.GetParent();
                workNode = ref workNode.GetParent();
                workNode.SetRightChild(ref newNode);
                workNode.SetLeftChild(ref newNode2);
                workNode.GetLeftChild().SetLeftChild(ref holdLeftChild);
                workNode.GetLeftChild().GetLeftChild().SetParent(ref workNode.GetLeftChild());
                workNode.GetRightChild().SetRightChild(ref holdRightChild);
                workNode.GetRightChild().GetRightChild().SetParent(ref workNode.GetRightChild());
                workNode.GetLeftChild().SetRightChild(ref holdMELeftChild);
                workNode.GetLeftChild().GetRightChild().SetParent(ref workNode.GetLeftChild());
                workNode.GetRightChild().SetLeftChild(ref holdMERightChild);
                workNode.GetRightChild().GetLeftChild().SetParent(ref workNode.GetRightChild());
            }

            return ref topNode;
        }
        
        
        public ref T23Node<T> GetRoot()
        {
            return ref _root;
        }
        public void SetRoot(ref T23Node<T> newRoot)
        {
            _root = newRoot;
        }
        
        public int GetDepth()
        {
            return _depth;
        }
        public void SetDepth(int newDepth)
        {
            _depth = newDepth ;
        }
        public void IncDepth()
        {
            _depth++;
        }

        public bool ReloadParents() //returns true if misbehaving parent setting
        {
            if (_root == null)
            {
                return false;
            }
            var queSon = new Queue<T23Node<T>>();
            var quePar = new Queue<T23Node<T>>();
            var watchParent_ = new T23Node<T>();
            ref var watchParent = ref watchParent_;
            bool ret = false;
            watchParent = ref _root;
            quePar.Enqueue(watchParent);
            var count = 0;
            do
            {
                count = 0;
                watchParent = quePar.Dequeue();
                if (watchParent.GetLeftChild() != null)
                {
                    queSon.Enqueue(watchParent.GetLeftChild());
                    quePar.Enqueue(watchParent.GetLeftChild());
                    count++;
                }
                if (watchParent.GetMiddleChild() != null)
                {
                    queSon.Enqueue(watchParent.GetMiddleChild());
                    quePar.Enqueue(watchParent.GetMiddleChild());
                    count++;
                }
                if (watchParent.GetRightChild() != null)
                {
                    queSon.Enqueue( watchParent.GetRightChild());
                    quePar.Enqueue( watchParent.GetRightChild());
                    count++;
                }
                for (int i = 0; i < count; i++)
                {
                    var watchSon = new T23Node<T>();
                    watchSon = queSon.Dequeue();
                    if (watchSon.GetParent()._data1.CompareTo(watchParent._data1) != 0)
                    {
                        watchSon.SetParent(ref watchParent);
                        ret = true;
                    }
                }
            } while (quePar.Count != 0);

            return ret;
        }

        public int CountData()
        {
            if (_root == null)
            {
                return 0;
            }
            var que = new Queue<T23Node<T>>();
            var printNow = new T23Node<T>();
            printNow = _root;
            que.Enqueue(_root);
            int count = 0;
            do
            {
                printNow = que.Dequeue();
                if (printNow._data1 != null)
                {
                    count++;
                }
                if (printNow._data2 != null)
                {
                    count++;
                }
                
                if (printNow.GetLeftChild() != null)
                {
                    que.Enqueue(printNow.GetLeftChild());
                }
                if (printNow.GetMiddleChild() != null)
                {
                    que.Enqueue(printNow.GetMiddleChild());
                }
                if (printNow.GetRightChild() != null)
                {
                    que.Enqueue(printNow.GetRightChild());
                }
            } while (que.Count != 0);

            return count;
        }
        
        public override string ToString()
        {
            if (_root == null)
            {
                return "";
            }
            var que = new Queue<T23Node<T>>();
            string ret = "\n";
            var printNow = new T23Node<T>();
            printNow = _root;
            que.Enqueue(_root);
            int count = 1;
            do
            {
                printNow = que.Dequeue();
                if (printNow.GetLevel() == count)
                {
                    count++;
                    ret += "\n";
                }
                
                ret += printNow.ToString();
                if (printNow.GetLeftChild() != null)
                {
                    que.Enqueue(printNow.GetLeftChild());
                }
                if (printNow.GetMiddleChild() != null)
                {
                    que.Enqueue(printNow.GetMiddleChild());
                }
                if (printNow.GetRightChild() != null)
                {
                    que.Enqueue(printNow.GetRightChild());
                }
            } while (que.Count != 0);

            return ret;
        }
    }
}