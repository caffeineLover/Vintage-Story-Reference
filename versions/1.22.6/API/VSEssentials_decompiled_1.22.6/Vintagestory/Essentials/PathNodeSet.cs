using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.Essentials;

public class PathNodeSet : IEnumerable<PathNode>, IEnumerable
{
	private int arraySize = 16;

	private PathNode[][] buckets = new PathNode[4][];

	private int[] bucketCount = new int[4];

	private int size;

	public int Count => size;

	public void Clear()
	{
		for (int i = 0; i < 4; i++)
		{
			bucketCount[i] = 0;
		}
		size = 0;
	}

	public PathNodeSet()
	{
		for (int i = 0; i < 4; i++)
		{
			buckets[i] = new PathNode[arraySize];
		}
	}

	public bool Add(PathNode value)
	{
		int bucket = value.Z % 2 * 2 + value.X % 2;
		bucket = (bucket + 4) % 4;
		PathNode[] set = buckets[bucket];
		int size = bucketCount[bucket];
		int i = size;
		while (--i >= 0)
		{
			if (value.Equals(set[i]))
			{
				return false;
			}
		}
		if (size >= arraySize)
		{
			ExpandArrays();
			set = buckets[bucket];
		}
		float fCost = value.fCost;
		i = size - 1;
		while (i >= 0 && (set[i].fCost < fCost || (set[i].fCost == fCost && set[i].hCost < value.hCost)))
		{
			i--;
		}
		i++;
		int j = size;
		while (j > i)
		{
			set[j] = set[--j];
		}
		set[i] = value;
		size++;
		bucketCount[bucket] = size;
		this.size++;
		return true;
	}

	public PathNode RemoveNearest()
	{
		if (size == 0)
		{
			return null;
		}
		PathNode nearestNode = null;
		int bucketToRemoveFrom = 0;
		for (int bucket = 0; bucket < 4; bucket++)
		{
			int endIndex = bucketCount[bucket] - 1;
			if (endIndex >= 0)
			{
				PathNode node = buckets[bucket][endIndex];
				if ((object)nearestNode == null || node.fCost < nearestNode.fCost || (node.fCost == nearestNode.fCost && node.hCost < nearestNode.hCost))
				{
					nearestNode = node;
					bucketToRemoveFrom = bucket;
				}
			}
		}
		bucketCount[bucketToRemoveFrom]--;
		size--;
		return nearestNode;
	}

	public void Remove(PathNode value)
	{
		int bucket = value.Z % 2 * 2 + value.X % 2;
		bucket = (bucket + 4) % 4;
		PathNode[] set = buckets[bucket];
		int size = bucketCount[bucket];
		int i = size;
		while (--i >= 0)
		{
			if (value.Equals(set[i]))
			{
				size = --bucketCount[bucket];
				while (i < size)
				{
					set[i] = set[++i];
				}
				this.size--;
				break;
			}
		}
	}

	public PathNode TryFindValue(PathNode value)
	{
		int bucket = value.Z % 2 * 2 + value.X % 2;
		bucket = (bucket + 4) % 4;
		PathNode[] set = buckets[bucket];
		int i = bucketCount[bucket];
		while (--i >= 0)
		{
			if (value.Equals(set[i]))
			{
				return set[i];
			}
		}
		return null;
	}

	private void ExpandArrays()
	{
		int newSize = arraySize * 3 / 2;
		for (int bucket = 0; bucket < 4; bucket++)
		{
			PathNode[] newArray = new PathNode[newSize];
			int size = bucketCount[bucket];
			PathNode[] set = buckets[bucket];
			for (int i = 0; i < size; i++)
			{
				newArray[i] = set[i];
			}
			buckets[bucket] = newArray;
		}
		arraySize = newSize;
	}

	public IEnumerator<PathNode> GetEnumerator()
	{
		for (int bucket = 0; bucket < 4; bucket++)
		{
			for (int i = 0; i < bucketCount[bucket]; i++)
			{
				yield return buckets[bucket][i];
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
