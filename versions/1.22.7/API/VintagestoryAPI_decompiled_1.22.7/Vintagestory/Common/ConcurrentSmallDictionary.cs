using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Vintagestory.Common;

/// <summary>
/// Use like any IDictionary. Similar to a FastSmallDictionary, but this one is thread-safe for simultaneous reads and writes - will not throw a ConcurrentModificationException
/// <br />This also inherently behaves as an OrderedDictionary (though without the OrderedDictionary extension methods such as ValuesOrdered, those can be added in future if required)
/// <br />Low-lock: there is no lock or interlocked operation except when adding new keys or when removing entries
/// <br />Low-memory: and contains only a single null field, if it is empty
/// <br />Two simultaneous writes, with the same key, at the same time, on different threads: small chance of throwing an intentional ConcurrentModificationException if both have the same keys, otherwise it's virtually impossible for us to preserve the rule that the Dictionary holds exactly one entry per key
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ConcurrentSmallDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	private DTable<TKey, TValue> contents;

	public ICollection<TKey> Keys
	{
		get
		{
			DTable<TKey, TValue> contents = this.contents;
			if (contents != null)
			{
				return contents.KeysCopy();
			}
			return Array.Empty<TKey>();
		}
	}

	public ICollection<TValue> Values
	{
		get
		{
			DTable<TKey, TValue> contents = this.contents;
			if (contents != null)
			{
				return contents.ValuesCopy();
			}
			return Array.Empty<TValue>();
		}
	}

	public bool IsReadOnly => false;

	int ICollection<KeyValuePair<TKey, TValue>>.Count
	{
		get
		{
			DTable<TKey, TValue> contents = this.contents;
			if (contents != null)
			{
				return contents.count;
			}
			return 0;
		}
	}

	/// <summary>
	/// Amount of entries currently in the Dictionary
	/// </summary>
	public int Count
	{
		get
		{
			DTable<TKey, TValue> contents = this.contents;
			if (contents != null)
			{
				return contents.count;
			}
			return 0;
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			return contents.GetValue(key);
		}
		set
		{
			Add(key, value);
		}
	}

	public ConcurrentSmallDictionary(int capacity)
	{
		if (capacity == 0)
		{
			contents = null;
		}
		else
		{
			contents = new DTable<TKey, TValue>(capacity);
		}
	}

	public ConcurrentSmallDictionary()
		: this(4)
	{
	}

	public bool IsEmpty()
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents != null)
		{
			return contents.count == 0;
		}
		return true;
	}

	public void Add(TKey key, TValue value)
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents == null)
		{
			if (Interlocked.CompareExchange(ref this.contents, new DTable<TKey, TValue>(key, value), contents) != contents)
			{
				Add(key, value);
			}
		}
		else if (!contents.ReplaceIfKeyExists(key, value))
		{
			if (!contents.Add(key, value) && Interlocked.CompareExchange(ref this.contents, new DTable<TKey, TValue>(contents, key, value), contents) != contents)
			{
				Add(key, value);
			}
			this.contents.DuplicateKeyCheck(key);
		}
	}

	public TValue TryGetValue(TKey key)
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents != null)
		{
			return contents.TryGetValue(key);
		}
		return default(TValue);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents == null)
		{
			value = default(TValue);
			return false;
		}
		return contents.TryGetValue(key, out value);
	}

	public bool ContainsKey(TKey key)
	{
		return this.contents?.ContainsKey(key) ?? false;
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		Add(item.Key, item.Value);
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		if (!contents.TryGetValue(item.Key, out var valueFound))
		{
			return false;
		}
		return item.Value.Equals(valueFound);
	}

	public bool Remove(TKey key)
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents == null)
		{
			return false;
		}
		int i = contents.IndexOf(key);
		if (i < 0)
		{
			return false;
		}
		if (Interlocked.CompareExchange(ref this.contents, new DTable<TKey, TValue>(contents, i), contents) != contents)
		{
			return Remove(key);
		}
		return true;
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents == null)
		{
			return false;
		}
		int i = contents.IndexOf(item.Key, item.Value);
		if (i < 0)
		{
			return false;
		}
		if (Interlocked.CompareExchange(ref this.contents, new DTable<TKey, TValue>(contents, i), contents) != contents)
		{
			return Remove(item);
		}
		return true;
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		contents.CopyTo(array, arrayIndex);
	}

	/// <summary>
	/// Threadsafe: but this might occasionally enumerate over a value which has, meanwhile, been removed from the Dictionary by a different thread
	/// Iterate over .Keys or .Values instead if an instantaneous snapshot is required (which will also therefore be a historical snapshot, if another thread meanwhile makes changes)
	/// </summary>
	/// <returns></returns>
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		DTable<TKey, TValue> contents = this.contents;
		if (contents != null)
		{
			int end_snapshot = contents.count;
			for (int pos = 0; pos < end_snapshot; pos++)
			{
				yield return new KeyValuePair<TKey, TValue>(contents.keys[pos], contents.values[pos]);
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Wipes the contents and resets the count.
	/// </summary>
	public void Clear()
	{
		contents = null;
	}
}
