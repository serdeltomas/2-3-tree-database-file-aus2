using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
	class FIleHandler
	{
		private FileStream _fileData;
		private int _lengthOfRecord;
		private int _count;
		private FileStream _fileTemp;
		private LinkedList<long> _freeSpace;

		public FIleHandler(string pFileName, int pLenOfRec)
		{
			_lengthOfRecord = pLenOfRec;
			_freeSpace = new LinkedList<long>();
			_count = 0;
			try
			{
				_fileData = new FileStream(pFileName + ".asc", FileMode.Create);
				_fileTemp = new FileStream(pFileName + ".txt", FileMode.Create);
			}
			catch
			{
				System.Console.Write("error with file");
			}
		}
		public FIleHandler(string pFileName)
		{
			_lengthOfRecord = 0; // read from file
			_freeSpace = new LinkedList<long>();
			_count = 0;
			try
			{
				_fileData = new FileStream(pFileName + ".asc", FileMode.Create);
				_fileTemp = new FileStream(pFileName + ".txt", FileMode.Create);
			}
			catch
			{
				System.Console.Write("error with file");
			}
		}
		~FIleHandler()
		{
			_fileData.Close();
			_fileTemp.Close();
		}
		public long InsertToFile(byte[] pByteArray)
		{
			//byte[] bA = Encoding.ASCII.GetBytes(sToWrite);
			long where;
			if (_freeSpace.Count() == 0) where = _count;
			else {
				where = _freeSpace.First();
				_freeSpace.RemoveFirst(); }
			_fileData.Seek(where * _lengthOfRecord, SeekOrigin.Begin);
			var nPos = (long)(_fileData.Position/_lengthOfRecord);
			foreach (byte bt in pByteArray) _fileData.WriteByte(bt);
			_fileData.Flush(); 
			_count++;
			return nPos;
		}

		public byte[] ReadFromFile(long pWhere)
		{
			if (_freeSpace.Contains(pWhere) || _fileData.Seek(pWhere * _lengthOfRecord, SeekOrigin.Begin) > _fileData.Seek(-_lengthOfRecord, SeekOrigin.End)) return null;
			_fileData.Seek(pWhere * _lengthOfRecord, SeekOrigin.Begin);
			int b;
			var count = 0;
			byte[] byteArr = new byte[_lengthOfRecord];
			while (((b = _fileData.ReadByte()) != -1) && count != _lengthOfRecord)
			{
				byteArr[count] = (byte)b;
				count++;
			}
			return byteArr;
		}
		public bool DeleteFromFile(long pWhere)
		{
			if (_freeSpace.Contains(pWhere)) return false;//uz neexistuje
			_fileData.Seek(-_lengthOfRecord, SeekOrigin.End);
			if (_fileData.Position == pWhere*_lengthOfRecord)
            {//vymazavaj zozadu
				_fileData.SetLength(_fileData.Length - _lengthOfRecord);
				_count--;
				while (_count != 0 && _freeSpace.Count != 0 && _fileData.Seek(-_lengthOfRecord, SeekOrigin.End) == _freeSpace.Last.Value * _lengthOfRecord)
                {
					_fileData.SetLength(_fileData.Length - _lengthOfRecord);
					_freeSpace.RemoveLast();
					_count--;
                }
				return true;
            }
			foreach(var space in _freeSpace)
            {
				if(space > pWhere)
                {
					_freeSpace.AddBefore(_freeSpace.Find(space), new LinkedListNode<long>(pWhere));
					_count--;
					return true;
                }
            }
			_freeSpace.AddLast(pWhere);
			_count--;
			return true;
		}
		public int GetCount()
        {
			return _count;
        }
	}
}
