using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Semka2
{
    public class Test
    {
        public void TestInsertString(int nOfInsertions)
        {
            var names = new string[]
            {
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony"
            };
            //var rand = new Random(1);
            var rand = new Random();
            
            var stromOsoby = new T23Tree<KeyStr,Osoba>();
            var cisla = new Int32[nOfInsertions+1];
            var keysTest = new ArrayList(nOfInsertions);
            var keysTree = new ArrayList(nOfInsertions);
            System.Console.Write("INSERTING " + nOfInsertions.ToString() + " items");
            for (int i = 1; i <= nOfInsertions; i++)
            {
                string cislotry;
                do
                {
                    cislotry = rand.Next(Int32.MaxValue).ToString("0000000000");
                } while (keysTest.Contains(cislotry));

                KeyStr actKey = new KeyStr(cislotry);
                keysTest.Add(cislotry);

                stromOsoby.Insert(actKey, new Osoba(names[rand.Next(names.Length)], "Mrkva",
                    cislotry, 1991, 01, 01));

                //System.Console.WriteLine(stromOsoby);
                //System.Console.WriteLine("-strom-" + stromOsoby.CountEntries() + "-array-" + i.ToString());
                if (i % 100000 == 0) System.Console.Write("|");
                else if (i % 10000 == 0) System.Console.Write(",");
                else if (i % 1000 == 0) System.Console.Write(".");
                /*{//test inside loop
                    Console.Write("inserted " + cislotry +"\nLength Test.....");
                    Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
                    Console.Write("Sequence Test.....");
                    keysTree.Clear();
                    stromOsoby.InOrder(stromOsoby.GetRoot(), ref keysTree);
                    var foundBad = false;
                    keysTest.Sort();
                    for (int j = 0; j < keysTest.Count; j++)
                    {
                        if (((string)keysTest[j]).CompareTo(keysTree[j].ToString()) != 0)
                        {
                            foundBad = true;
                            break;
                        }
                    }

                    Console.WriteLine(foundBad ? "FAIL" : "PASS");
                    Console.Write("List Depth Test.....");
                    Console.WriteLine(stromOsoby.IsAllLeavesSameDepth() ? "PASS" : "FAIL");
                }*/
            }

            System.Console.WriteLine("DONE");
            //System.Console.WriteLine(stromOsoby);
            Console.Write("Length Test.....");
            Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
            Console.Write("Sequence Test.....");
            keysTree.Clear();
            stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
            var keysTreeNotRecursive = stromOsoby.InOrder();
            var foundMistake = false;
            keysTest.Sort();
            for (int i = 0; i < nOfInsertions; i++)
            {
                if (((string)keysTest[i]).CompareTo(keysTreeNotRecursive[i].ToString()) != 0) {foundMistake = true; break; }
            }
            Console.WriteLine(foundMistake ? "FAIL" : "PASS");
            Console.Write("List Depth Test.....");
            Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
            
            
        }
        public void TestInsertInt(int nOfInsertions)
        {
            var names = new string[]
            {
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony"
            };
            //var rand = new Random(1);
            var rand = new Random();
            
            var stromOsoby = new T23Tree<KeyInt,Osoba>();
            var cisla = new Int32[nOfInsertions+1];
            var keysTest = new List<KeyInt>(nOfInsertions);
            //var keysTree = new ArrayList(nOfInsertions);
            System.Console.Write("INSERTING " + nOfInsertions.ToString() + " items");
            for (int i = 1; i <= nOfInsertions; i++)
            {
                int cislotry = 0;
                KeyInt actKey = new KeyInt(cislotry);
                do
                {
                    cislotry = rand.Next(Int32.MaxValue);
                    actKey = new KeyInt(cislotry);
                } while (keysTest.Contains(actKey));

                
                keysTest.Add(actKey);

                stromOsoby.Insert(actKey, new Osoba(names[rand.Next(names.Length)], "Mrkva",
                    cislotry.ToString(), 1991, 01, 01));

                //System.Console.WriteLine(stromOsoby);
                //System.Console.WriteLine("-strom-" + stromOsoby.CountEntries() + "-array-" + i.ToString());
                if (i % 100000 == 0) System.Console.Write("|");
                else if (i % 10000 == 0) System.Console.Write(",");
                else if (i % 1000 == 0) System.Console.Write(".");
                /*{//test inside loop
                    Console.Write("inserted " + cislotry +"\nLength Test.....");
                    Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
                    Console.Write("Sequence Test.....");
                    var keysTreeNotRecursiv = stromOsoby.InOrderKeys();
                    var foundBad = false;
                    keysTest.Sort();
                    for (int j = 0; j < keysTest.Count; j++)
                    {
                        if (keysTreeNotRecursiv[j].CompareTo(keysTest[j]) != 0)
                        {foundBad = true; break; }
                    }
                    Console.WriteLine(foundBad ? "FAIL" : "PASS");
                    Console.Write("List Depth Test.....");
                    Console.WriteLine(stromOsoby.IsAllLeavesSameDepth() ? "PASS" : "FAIL");
                }*/
            }

            System.Console.WriteLine("DONE");
            //System.Console.WriteLine(stromOsoby);
            Console.Write("Length Test.....");
            Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
            Console.Write("Sequence Test.....");
            //keysTree.Clear();
            //stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
            var keysTreeNotRecursive = stromOsoby.InOrderKeys();
            var foundMistake = false;
            keysTest.Sort();
            for (int i = 0; i < nOfInsertions; i++)
            {
                if (keysTreeNotRecursive[i].CompareTo(keysTest[i]) != 0)
                {foundMistake = true; break; }
            }
            Console.WriteLine(foundMistake ? "FAIL" : "PASS");
            Console.Write("List Depth Test.....");
            Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
            
            
        }
        public void TestAll(int nOfInsertions,double deletionsFraction, double findsFraction)
        {
            var names = new string[]
            {
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony"
            };
            //var rand = new Random(20);
            var rand = new Random();
            var iAll = 0;
            var iDel = 0;
            var iIns = 0;
            var iFind = 0;
            var stromOsoby = new T23Tree<KeyStr,Osoba>();
            var keysTest = new ArrayList(nOfInsertions);
            var keysTree = new ArrayList(nOfInsertions);
            Console.Write("INSERT:DELETE:FIND(" + nOfInsertions + ":" + (int)(nOfInsertions*deletionsFraction) + ":" + (int)(nOfInsertions * findsFraction) + ")");
            for (int i = 1; i <= nOfInsertions; i++)
            {
                var countNow = keysTest.Count;
                var nd = rand.NextDouble();
                if (nd < deletionsFraction && countNow != 0)
                {
                    var delWhat = rand.Next(countNow);
                    var nKey = new KeyStr(keysTest[delWhat].ToString());
                    stromOsoby.Delete(nKey);
                    keysTest.RemoveAt(delWhat);
                    /*{//tests
                        Console.Write("justDeleted "+ nKey +"\nLength Test.....T" + stromOsoby.Count() + "-R" + stromOsoby.CountEntries() + "-A"+ keysTest.Count + "..");
                        Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
                        Console.Write("Sequence Test.....");
                        keysTree.Clear();
                        stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
                        var foundMistake = false;
                        keysTest.Sort();
                        var count = keysTest.Count;
                        for (int j= 0; j < count; j++)
                        {
                            if(!keysTest[j].Equals(keysTree[j].ToString())) {foundMistake = true; break; }
                        }
                        Console.WriteLine(foundMistake ? "FAIL" : "PASS");
                        Console.Write("List Depth Test.....");
                        Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
                    }*/
                    iDel++;
                }
                countNow = keysTest.Count;
                var nf = rand.NextDouble();
                if (nf < findsFraction && countNow != 0)
                {
                    var findWhat = rand.Next(countNow);
                    var nKey = new KeyStr(keysTest[findWhat].ToString());
                    stromOsoby.GetData(nKey);
                    /*{//tests
                        Console.Write("justFound "+ nKey +"\nLength Test.....T" + stromOsoby.Count() + "-R" + stromOsoby.CountEntries() + "-A"+ keysTest.Count + "..");
                        Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
                        Console.Write("Sequence Test.....");
                        keysTree.Clear();
                        stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
                        var foundMistake = false;
                        keysTest.Sort();
                        var count = keysTest.Count;
                        for (int j= 0; j < count; j++)
                        {
                            if(!keysTest[j].Equals(keysTree[j].ToString())) {foundMistake = true; break; }
                        }
                        Console.WriteLine(foundMistake ? "FAIL" : "PASS");
                        Console.Write("List Depth Test.....");
                        Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
                    }*/
                    iFind++;
                }
                /*else
                {*/
                string cislotry;
                do
                {
                    cislotry = rand.Next(Int32.MaxValue).ToString("0000000000");
                } while (keysTest.Contains(cislotry));

                KeyStr actKey = new KeyStr(cislotry);
                keysTest.Add(cislotry);
                stromOsoby.Insert(actKey, new Osoba(names[rand.Next(names.Length)],
                    "Mrkva", cislotry, 1991, 01, 01));
                /*{//tests
                    Console.Write("justInserted "+ cislotry +"\nLength Test.....T" +stromOsoby.Count() + "-R" + stromOsoby.CountEntries() +"-A"+ keysTest.Count + "..");
                    Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
                    Console.Write("Sequence Test.....");
                    keysTree.Clear();
                    stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
                    var foundMistake = false;
                    keysTest.Sort();
                    var count = keysTest.Count;
                    for (int k = 0; k < count; k++)
                    {
                        if(!keysTest[k].Equals(keysTree[k].ToString())) {foundMistake = true; break; }
                    }
                    Console.WriteLine(foundMistake ? "FAIL" : "PASS");
                    Console.Write("List Depth Test.....");
                    Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
                }*/
                iIns++;
                //}

                iAll++;
                if (i % 100000 == 0) System.Console.Write("|");
                else if (i % 10000 == 0) System.Console.Write(",");
                else if (i % 1000 == 0) System.Console.Write(".");
            }

            System.Console.WriteLine("DONE " + iAll);
            //System.Console.WriteLine(stromOsoby);
            Console.Write("Length Test.....T" +stromOsoby.Count() + "-R" + stromOsoby.CountEntries() +"-A"+ keysTest.Count + "..");
            Console.WriteLine(stromOsoby.CountEntries() == keysTest.Count ? "PASS" : "FAIL");
            Console.Write("Sequence Test.....");
            keysTree.Clear();
            stromOsoby.InOrderRecursion(stromOsoby.GetRoot(), ref keysTree);
            var keysTreeNotRecursive = stromOsoby.InOrder();
            var foundMistak = false;
            keysTest.Sort();
            var countt = keysTest.Count;
            for (int i = 0; i < countt; i++)
            {
                if(!keysTest[i].Equals(keysTreeNotRecursive[i].GetRodCislo())) {foundMistak = true; break; }
            }
            Console.WriteLine(foundMistak ? "FAIL" : "PASS");
            Console.Write("List Depth Test.....");
            Console.WriteLine(stromOsoby.IsAllLeavesSameDepth()? "PASS" : "FAIL");
            Console.WriteLine("all:" + iAll + " ins:" + iIns + " del:" + iDel + " find:" + iFind);
        }
        public void TestDeleteBasic()
        {
            var names = new string[]
            {
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony"
            };
            //var rand = new Random(1);
            int nOfInsertions =3;
            var rand = new Random();
            
            var stromOsoby = new T23Tree<KeyStr,Osoba>();
            var cisla = new Int32[nOfInsertions+1];
            var keysTest = new ArrayList(nOfInsertions);
            var keysTree = new ArrayList(nOfInsertions);
            Console.Write("INSERTING " + nOfInsertions.ToString() + " items");
            for (int i = 1; i <= nOfInsertions; i++)
            {
                string cislotry = i.ToString("0000000000");
                KeyStr actKey = new KeyStr(cislotry);
                keysTest.Add(cislotry);
                stromOsoby.Insert(actKey, new Osoba(names[rand.Next(names.Length)], "Mrkva", 
                    cislotry, 1991, 01, 01));
                
                Console.WriteLine(stromOsoby);
                //System.Console.WriteLine("-strom-" + stromOsoby.CountEntries() + "-array-" + i.ToString());
                if(i%100000 ==0) Console.Write("|");
                else if(i%10000 ==0) Console.Write(",");
                else if(i%1000 ==0) Console.Write(".");
            }
            Console.WriteLine("DONE");
            
            
            KeyStr delKey = new KeyStr("0000000002");
            stromOsoby.Delete(delKey);
            Console.WriteLine(stromOsoby);
            delKey = new KeyStr("0000000003");
            stromOsoby.Delete(delKey);
            Console.WriteLine(stromOsoby);
            delKey = new KeyStr("0000000001");
            stromOsoby.Delete(delKey);
            Console.WriteLine(stromOsoby);
        }
        public void TestInsertFile(int nOfInsertions)
        {
            var firstNames = new string[]{
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony",
                "Olivia", "Emma", "Ava", "Charlotte", "Sophia", "Amelia", "Isabella", "Lucas", "Mia", "Henry", "Evelyn", "Harper"};
            var lastNames = new string[]{"Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez",
                "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson",
                "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams",
                "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts" };
            //var rand = new Random(1);
            var rand = new Random();

            var dataTest = new ArrayList(nOfInsertions);
            var keysFile = new ArrayList(nOfInsertions);
            var suborHandler = new FIleHandler("_test",new DummyClass().Size());

            System.Console.Write("INSERTING " + nOfInsertions.ToString() + " items");
            for (int i = 1; i <= nOfInsertions; i++)
            {
                var dummy = new DummyClass(firstNames[rand.Next(firstNames.Length)], lastNames[rand.Next(lastNames.Length)], rand.Next());
                dataTest.Add(dummy.ToString());
                keysFile.Add(suborHandler.InsertToFile(dummy.ToByteArray()));

                if (i % 100000 == 0) System.Console.Write("|");
                else if (i % 10000 == 0) System.Console.Write(",");
                else if (i % 1000 == 0) System.Console.Write(".");
            }
            //tests
            System.Console.WriteLine("DONE");
            Console.Write("Length Test.....");
            Console.WriteLine(suborHandler.GetCount() == dataTest.Count ? "PASS" : "FAIL");
            Console.Write("Sequence Test.....");
            var foundMistake = false;
            var dum = new DummyClass();
            for (int i = 0; i < nOfInsertions; i++)
            {
                if (suborHandler.ReadFromFile((long)keysFile[i]) == null) continue;
                if (dum.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString().CompareTo((string)dataTest[i]) != 0)
                    { foundMistake = true; Console.WriteLine("\n" + i + "\n" + dum.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString() + "\n" + dataTest[i].ToString());
                    break; }
            }
            Console.WriteLine(foundMistake ? "FAIL" : "PASS");
        }
        public void TestAllFile(int nOfInsertions, double deletionsFraction, double findsFraction)
        {
            var firstNames = new string[]{
                "Noah" ,"Liam" ,"William" ,"Mason" , "James" ,"Benjamin" ,"Jacob" , "Michael" ,"Elijah" ,"Ethan" ,
                "Alexander" ,"Oliver" ,"Daniel" , "Lucas" ,"Matthew" ,"Aiden" , "Jackson" ,"Logan" ,"David" ,"Joseph" ,
                "Samuel" ,"Henry" ,"Owen" ,"Sebastian" , "Gabriel" ,"Carter" ,"Jayden" ,"John" , "Luke" ,"Anthony",
                "Olivia", "Emma", "Ava", "Charlotte", "Sophia", "Amelia", "Isabella", "Lucas", "Mia", "Henry", "Evelyn", "Harper"};
            var lastNames = new string[]{"Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez",
                "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson",
                "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams",
                "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts" };
            //var rand = new Random(1);
            var rand = new Random();
            var iAll = 0;
            var iDel = 0;
            var iIns = 0;
            var iFind = 0;
            var dataTest = new ArrayList(nOfInsertions);
            var keysFile = new ArrayList(nOfInsertions);
            var suborHandler = new FIleHandler("_test", new DummyClass().Size());

            Console.Write("INSERT:DELETE:FIND(" + nOfInsertions + ":" + (int)(nOfInsertions * deletionsFraction) + ":" + (int)(nOfInsertions * findsFraction) + ")");
            for (int j = 1; j <= nOfInsertions; j++)
            {
                var countNow = dataTest.Count;
                var nd = rand.NextDouble();
                if (nd < deletionsFraction && countNow != 0)
                {
                    var delWhat = rand.Next(countNow);
                    suborHandler.DeleteFromFile((long)keysFile[delWhat]);
                    keysFile.RemoveAt(delWhat);
                    dataTest.RemoveAt(delWhat);
                    iDel++;

                    //tests 
                    /*
                    Console.Write("Del - " + delWhat);
                    Console.Write("Length Test.....");
                    Console.WriteLine(suborHandler.GetCount() == dataTest.Count ? "PASS" : "FAIL");
                    Console.Write("Containment Test.....");
                    var foundMistak = false;
                    var dumm = new DummyClass();
                    for (int i = 0; i < dataTest.Count; i++)
                    {
                        if (suborHandler.ReadFromFile((long)keysFile[i]) == null) continue;
                        if (!dataTest.Contains(dumm.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString()))
                        {
                            foundMistak = true; Console.WriteLine("\n" + i + "\n" + dumm.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString() + "\n" + dataTest[i].ToString());
                            break;
                        }
                    }
                    Console.WriteLine(foundMistak ? "FAIL" : "PASS");
                    Console.WriteLine("all:" + iAll + " ins:" + iIns + " del:" + iDel + " find:" + iFind);
                    */
                }

                countNow = dataTest.Count;
                var nf = rand.NextDouble();
                if (nf < findsFraction && countNow != 0)
                {
                    var findWhat = rand.Next(countNow);
                    suborHandler.ReadFromFile((long)keysFile[findWhat]);
                    iFind++;

                    /*//tests 
                    Console.Write("Find - " + findWhat);
                    Console.Write("Length Test.....");
                    Console.WriteLine(suborHandler.GetCount() == dataTest.Count ? "PASS" : "FAIL");
                    Console.Write("Containment Test.....");
                    var foundMistak = false;
                    var dumm = new DummyClass();
                    for (int i = 0; i < dataTest.Count; i++)
                    {
                        if (suborHandler.ReadFromFile((long)keysFile[i]) == null) continue;
                        if (!dataTest.Contains(dumm.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString()))
                        {
                            foundMistak = true; Console.WriteLine("\n" + i + "\n" + dumm.FromByteArray(suborHandler.ReadFromFile((long)keysFile[i])).ToString() + "\n" + dataTest[i].ToString());
                            break;
                        }
                    }
                    Console.WriteLine(foundMistak ? "FAIL" : "PASS");
                    Console.WriteLine("all:" + iAll + " ins:" + iIns + " del:" + iDel + " find:" + iFind);
                    */
                }
                var dummy = new DummyClass(firstNames[rand.Next(firstNames.Length)], lastNames[rand.Next(lastNames.Length)], rand.Next());
                dataTest.Add(dummy.ToString());
                keysFile.Add(suborHandler.InsertToFile(dummy.ToByteArray()));

                if (j % 100000 == 0) System.Console.Write("|");
                else if (j % 10000 == 0) System.Console.Write(",");
                else if (j % 1000 == 0) System.Console.Write(".");

                iIns++;
                iAll++;
            }
            //tests
            System.Console.WriteLine("DONE");
            Console.Write("Length Test.....");
            Console.WriteLine(suborHandler.GetCount() == dataTest.Count ? "PASS" : "FAIL");
            Console.Write("Containment Test.....");
            var foundMistake = false;
            var dum = new DummyClass();
            for (int j = 0; j < dataTest.Count; j++)
            {
                if (suborHandler.ReadFromFile((long)keysFile[j]) == null) continue;
                if (!dataTest.Contains(dum.FromByteArray(suborHandler.ReadFromFile((long)keysFile[j])).ToString()))
                {
                    foundMistake = true; Console.WriteLine("\n" + j + "\n" + dum.FromByteArray(suborHandler.ReadFromFile((long)keysFile[j])).ToString() + "\n" + dataTest[j].ToString());
                    break;
                }
                if (j % 100000 == 0) System.Console.Write("|");
                else if (j % 10000 == 0) System.Console.Write(",");
                else if (j % 1000 == 0) System.Console.Write(".");
            }
            Console.WriteLine(foundMistake ? "FAIL" : "PASS");
            Console.WriteLine("all:" + iAll + " ins:" + iIns + " del:" + iDel + " find:" + iFind);
        }

    }
}