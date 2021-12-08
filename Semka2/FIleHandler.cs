using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semka2
{
	public class FIleHandler<T> where T : IDataToFIle<T>
	{
		private T _tInstance;
		private FileStream _fileData;
		private int _lengthOfRecord;
		private long _count;
		private FileStream _fileTemp;
		private LinkedList<long> _freeSpace;

		public FIleHandler(string pFileName, int pLenOfRec)
		{
			_tInstance = default;
			_lengthOfRecord = pLenOfRec;
			_freeSpace = new LinkedList<long>();
			_count = 0;
			try
			{
				_fileData = new FileStream(pFileName + ".bin", FileMode.Create);
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
			_fileData.Flush();
			_fileData.Close();
			_fileTemp.Flush();
			_fileTemp.Close();
		}
		public void SetInstance(T pTInstance) { _tInstance = pTInstance; }
		public long InsertNextWhere() { return _freeSpace.Count() == 0 ? _count : _freeSpace.First(); }
		public long FindWhere(T data) {
			var nString = "";
			long where = -1;
			do {
				where++;
				nString = ReadFromFile(where).ToString();
			} while (data.ToString().CompareTo(nString) != 0 && where < _count);
			return where; 
		}
		public long InsertToFile(T pData)
		{
			//byte[] bA = Encoding.ASCII.GetBytes(sToWrite);
			long where;
			if (_freeSpace.Count() == 0) where = _count;
			else {
				where = _freeSpace.First();
				_freeSpace.RemoveFirst(); }
			_fileData.Seek(where * _lengthOfRecord, SeekOrigin.Begin);
			var nPos = _fileData.Position/_lengthOfRecord;
			foreach (byte bt in pData.ToByteArray()) _fileData.WriteByte(bt);
			_fileData.Flush(); 
			_count++;
			return nPos;
		}
		public long UpdateInFIle(T pData, long pWhere)
        {
			_fileData.Seek(pWhere * _lengthOfRecord, SeekOrigin.Begin);
			var nPos = _fileData.Position / _lengthOfRecord;
			foreach (byte bt in pData.ToByteArray()) _fileData.WriteByte(bt);
			_fileData.Flush(); 
			return nPos;
		}
		public T ReadFromFile(long pWhere)
		{
			if (_freeSpace.Contains(pWhere) || _fileData.Seek(pWhere * _lengthOfRecord, SeekOrigin.Begin) > _fileData.Seek(-_lengthOfRecord, SeekOrigin.End)) return default;
			_fileData.Seek(pWhere * _lengthOfRecord, SeekOrigin.Begin);
			int b;
			var count = 0;
			byte[] byteArr = new byte[_lengthOfRecord];
			while (((b = _fileData.ReadByte()) != -1) && count != _lengthOfRecord)
			{
				byteArr[count] = (byte)b;
				count++;
			}
			if (_tInstance == null) 
				_tInstance = (T)Activator.CreateInstance<T>(); // cool but evil
			return _tInstance.FromByteArray(byteArr);
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
		public bool ReadWholeFile(ref string vypis)
		{
			var count = 0;
			do {
				if (ReadFromFile(count) != null)
					vypis = vypis + ReadFromFile(count).ToString();
				count++;
			} while (count <= _count);
			return true;
		}
		public long GetCount()
        {
			return _count;
        }
	}
}
