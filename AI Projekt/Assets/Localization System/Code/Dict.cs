using System;
using UnityEngine;
using System.Collections.Generic;

namespace Translation
{
	public class Dict
	{
		private Dictionary<string, string> _values;
		
		public Dict ()
		{
			_values = new Dictionary<string, string>();
		}
		
		public int GetCount()
		{
			return _values.Count;	
		}
		
		public string Get(string key)
		{
			return _values[key];
		}
		
		public void Set(string key, string val)
		{
			_values[key] = val;
		}

		public void Remove (string key)
		{
			_values.Remove(key);
		}
		
		public bool ContainsKey(string key)
		{
			return _values.ContainsKey(key);
		}
		
		public string[] GetKeys()
		{
			string[] arr = new string[GetCount ()];
			int i = 0;
			foreach(string val in _values.Keys)
			{
				arr[i++] = val;
			}
			return arr;
		}
		
		public string[] GetValues()
		{
			string[] arr = new string[GetCount ()];
			int i = 0;
			foreach(string val in _values.Values)
			{
				arr[i++] = val;
			}
			return arr;
		}
	}
}

