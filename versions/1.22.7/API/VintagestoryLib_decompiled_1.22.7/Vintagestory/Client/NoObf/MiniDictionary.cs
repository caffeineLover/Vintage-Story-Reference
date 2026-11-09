using System;
using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.Client.NoObf;

public class MiniDictionary : IDictionary<string, int>, ICollection<KeyValuePair<string, int>>, IEnumerable<KeyValuePair<string, int>>, IEnumerable
{
	private string[] keys;

	private int[] values;

	public int Count;

	public const int NOTFOUND = -1;

	ICollection<string> IDictionary<string, int>.Keys => new ArraySegment<string>(keys, 0, Count);

	ICollection<int> IDictionary<string, int>.Values => new ArraySegment<int>(values, 0, Count);

	int ICollection<KeyValuePair<string, int>>.Count => Count;

	bool ICollection<KeyValuePair<string, int>>.IsReadOnly => false;

	public int this[string key]
	{
		get
		{
			string[] keys = this.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				if (i >= Count)
				{
					return -1;
				}
				if (key == keys[i])
				{
					return values[i];
				}
			}
			return -1;
		}
		set
		{
			string[] keys = this.keys;
			for (int i = 0; i < keys.Length && i < Count; i++)
			{
				if (keys[i] == key)
				{
					values[i] = value;
					return;
				}
			}
			if (Count == keys.Length)
			{
				ExpandArrays();
			}
			this.keys[Count] = key;
			values[Count++] = value;
		}
	}

	public MiniDictionary(int size)
	{
		keys = new string[size];
		values = new int[size];
	}

	private void ExpandArrays()
	{
		string[] keys = this.keys;
		int num = keys.Length + 3;
		string[] newKeys = new string[num];
		int[] newValues = new int[num];
		for (int i = 0; i < keys.Length; i++)
		{
			newKeys[i] = keys[i];
			newValues[i] = values[i];
		}
		values = newValues;
		this.keys = newKeys;
	}

	void IDictionary<string, int>.Add(string key, int value)
	{
		this[key] = value;
	}

	bool IDictionary<string, int>.ContainsKey(string key)
	{
		string[] keys = this.keys;
		for (int i = 0; i < Count; i++)
		{
			if (keys[i] == key)
			{
				return true;
			}
		}
		return false;
	}

	bool IDictionary<string, int>.Remove(string key)
	{
		throw new NotImplementedException();
	}

	bool IDictionary<string, int>.TryGetValue(string key, out int value)
	{
		string[] keys = this.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			if (i >= Count)
			{
				value = -1;
				return false;
			}
			if (key == keys[i])
			{
				value = values[i];
				return true;
			}
		}
		value = -1;
		return false;
	}

	void ICollection<KeyValuePair<string, int>>.Add(KeyValuePair<string, int> item)
	{
		this[item.Key] = item.Value;
	}

	void ICollection<KeyValuePair<string, int>>.Clear()
	{
		string[] keys = this.keys;
		int count = Count;
		Count = 0;
		for (int i = 0; i < keys.Length && i < count; i++)
		{
			keys[i] = null;
		}
	}

	bool ICollection<KeyValuePair<string, int>>.Contains(KeyValuePair<string, int> item)
	{
		string[] keys = this.keys;
		for (int i = 0; i < Count; i++)
		{
			if (keys[i] == item.Key && values[i] == item.Value)
			{
				return true;
			}
		}
		return false;
	}

	void ICollection<KeyValuePair<string, int>>.CopyTo(KeyValuePair<string, int>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	bool ICollection<KeyValuePair<string, int>>.Remove(KeyValuePair<string, int> item)
	{
		throw new NotImplementedException();
	}

	IEnumerator<KeyValuePair<string, int>> IEnumerable<KeyValuePair<string, int>>.GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new NotImplementedException();
	}
}
