using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.API.Datastructures;

public class FastSetOfInts : IEnumerable<int>, IEnumerable
{
	private int size;

	private int[] set;

	public int Count => size;

	public void Clear()
	{
		size = 0;
	}

	public FastSetOfInts()
	{
		set = new int[27];
	}

	/// <summary>
	/// Add four separate components, assumed to be signed int in the range -128 to +127
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <param name="d"></param>
	/// <returns></returns>
	public bool Add(int a, int b, int c, int d)
	{
		return Add(a + 128 + (b + 128 << 8) + (c + 128 << 16) + (d << 24));
	}

	/// <summary>
	/// Return false if the set already contained this value; return true if the Add was successful
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public bool Add(int value)
	{
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

	public void RemoveIfMatches(int a, int b, int c, int d)
	{
		int i = size;
		int match = a + 128 + (b + 128 << 8) + (c + 128 << 16) + (d << 24);
		while (--i >= 0)
		{
			if (set[i] == match)
			{
				RemoveAt(i);
				break;
			}
		}
	}

	private void RemoveAt(int i)
	{
		for (int j = i + 1; j < size; j++)
		{
			set[j - 1] = set[j];
		}
		size--;
	}

	private void expandArray()
	{
		int[] newArray = new int[set.Length * 3 / 2 + 1];
		for (int i = 0; i < set.Length; i++)
		{
			newArray[i] = set[i];
		}
		set = newArray;
	}

	public IEnumerator<int> GetEnumerator()
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
