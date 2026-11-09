using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.API.Datastructures;

public class FastSetOfLongs : IEnumerable<long>, IEnumerable
{
	private int size;

	private long[] set;

	private long last = long.MinValue;

	public int Count => size;

	public void Clear()
	{
		size = 0;
		last = long.MinValue;
	}

	public FastSetOfLongs()
	{
		set = new long[27];
	}

	/// <summary>
	/// Return false if the set already contained this value; return true if the Add was successful
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public bool Add(long value)
	{
		if (value == last && size > 0)
		{
			return false;
		}
		last = value;
		int i = size;
		while (--i >= 0)
		{
			if (set[i] == value)
			{
				return false;
			}
		}
		if (size >= set.Length)
		{
			expandArray();
		}
		set[size++] = value;
		return true;
	}

	private void expandArray()
	{
		long[] newArray = new long[set.Length * 3 / 2 + 1];
		for (int i = 0; i < set.Length; i++)
		{
			newArray[i] = set[i];
		}
		set = newArray;
	}

	public IEnumerator<long> GetEnumerator()
	{
		for (int i = 0; i < size; i++)
		{
			yield return set[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
