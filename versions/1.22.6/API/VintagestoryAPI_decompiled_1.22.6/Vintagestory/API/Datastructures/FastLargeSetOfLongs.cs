using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.API.Datastructures;

public class FastLargeSetOfLongs : IEnumerable<long>, IEnumerable
{
	private int count;

	private readonly long[][] buckets;

	private readonly int[] sizes;

	private readonly int mask;

	public int Count => count;

	public void Clear()
	{
		count = 0;
		for (int j = 0; j < sizes.Length; j++)
		{
			sizes[j] = 0;
		}
	}

	public FastLargeSetOfLongs(int numbuckets)
	{
		int num = numbuckets - 1;
		int num2 = num | (num >> 1);
		int num3 = num2 | (num2 >> 2);
		int num4 = num3 | (num3 >> 4);
		int num5 = num4 | (num4 >> 8);
		numbuckets = (num5 | (num5 >> 16)) + 1;
		sizes = new int[numbuckets];
		buckets = new long[numbuckets][];
		for (int i = 0; i < buckets.Length; i++)
		{
			buckets[i] = new long[7];
		}
		mask = numbuckets - 1;
	}

	/// <summary>
	/// Return false if the set already contained this value; return true if the Add was successful
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public bool Add(long value)
	{
		int j = (int)value & mask;
		long[] set = buckets[j];
		int siz = sizes[j];
		int i = siz;
		while (--i >= 0)
		{
			if (set[i] == value)
			{
				return false;
			}
		}
		if (siz >= set.Length)
		{
			set = expandArray(j);
		}
		set[siz++] = value;
		sizes[j] = siz;
		count++;
		return true;
	}

	private long[] expandArray(int j)
	{
		long[] set = buckets[j];
		long[] newArray = new long[set.Length * 3 / 2 + 1];
		for (int i = 0; i < set.Length; i++)
		{
			newArray[i] = set[i];
		}
		buckets[j] = newArray;
		return newArray;
	}

	public IEnumerator<long> GetEnumerator()
	{
		for (int j = 0; j < buckets.Length; j++)
		{
			int size = sizes[j];
			long[] set = buckets[j];
			for (int i = 0; i < size; i++)
			{
				yield return set[i];
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
