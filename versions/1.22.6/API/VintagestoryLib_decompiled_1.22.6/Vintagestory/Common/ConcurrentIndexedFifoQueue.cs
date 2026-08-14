using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

internal class ConcurrentIndexedFifoQueue<T> : IEnumerable<T>, IEnumerable where T : ILongIndex
{
	internal readonly ConcurrentDictionary<long, T> elementsByIndex;

	private readonly T[] elements;

	private readonly int length;

	private volatile uint start;

	private volatile uint end;

	private volatile uint startBleedingEdge;

	private volatile uint endBleedingEdge;

	public int Count => (int)(end - start);

	public int Capacity => elements.Length;

	public ConcurrentIndexedFifoQueue(int capacity, int stages)
	{
		capacity = Math.Min(ArrayConvert.GetRoundedUpSize(capacity), 65536);
		elements = new T[capacity];
		length = capacity;
		elementsByIndex = new ConcurrentDictionary<long, T>(stages, capacity);
	}

	public bool IsFull()
	{
		return (int)(end - start) >= length;
	}

	public bool IsEmpty()
	{
		return start == end;
	}

	public T GetByIndex(long index)
	{
		elementsByIndex.TryGetValue(index, out var val);
		return val;
	}

	public void Enqueue(T elem)
	{
		elementsByIndex[elem.Index] = elem;
		EnqueueWithoutAddingToIndex(elem);
	}

	internal void EnqueueWithoutAddingToIndex(T elem)
	{
		uint localcopy = endBleedingEdge;
		if ((int)(localcopy - start) > length - 1)
		{
			throw new Exception("Indexed Fifo Queue overflow. Try increasing servermagicnumbers RequestChunkColumnsQueueSize?");
		}
		uint newEnd;
		while ((newEnd = Interlocked.CompareExchange(ref endBleedingEdge, localcopy + 1, localcopy)) != localcopy)
		{
			localcopy = newEnd;
			if ((int)(newEnd - start) > length - 1)
			{
				throw new Exception("Indexed Fifo Queue overflow. Try increasing servermagicnumbers RequestChunkColumnsQueueSize?");
			}
		}
		elements[(ushort)localcopy % length] = elem;
		Interlocked.Increment(ref end);
		if (!elementsByIndex.ContainsKey(elem.Index))
		{
			throw new Exception("In queue but missed from index!");
		}
	}

	public T DequeueWithoutRemovingFromIndex()
	{
		uint localcopy = startBleedingEdge;
		if ((int)(localcopy - end) >= 0)
		{
			return default(T);
		}
		uint newStart;
		while ((newStart = Interlocked.CompareExchange(ref startBleedingEdge, localcopy + 1, localcopy)) != localcopy)
		{
			localcopy = newStart;
			if ((int)(newStart - end) >= 0)
			{
				return default(T);
			}
		}
		T result = elements[(ushort)localcopy % length];
		Interlocked.Increment(ref start);
		elements[(ushort)localcopy % length] = default(T);
		return result;
	}

	public T Dequeue()
	{
		T elem = DequeueWithoutRemovingFromIndex();
		if (elem != null)
		{
			elementsByIndex.Remove(elem.Index);
		}
		return elem;
	}

	internal void Requeue()
	{
		if (IsFull())
		{
			Interlocked.Increment(ref endBleedingEdge);
			Interlocked.Increment(ref end);
			Interlocked.Increment(ref startBleedingEdge);
			Interlocked.Increment(ref start);
		}
		else
		{
			T elem = DequeueWithoutRemovingFromIndex();
			if (elem != null)
			{
				EnqueueWithoutAddingToIndex(elem);
			}
		}
	}

	public T Peek()
	{
		return elements[(ushort)startBleedingEdge % length];
	}

	public T PeekAtPosition(int position)
	{
		return elements[(ushort)((int)startBleedingEdge + position) % length];
	}

	public bool Remove(long index)
	{
		if (elementsByIndex.TryRemove(index, out var elem))
		{
			elem.FlagToDispose();
			return true;
		}
		return false;
	}

	public IEnumerator<T> GetEnumerator()
	{
		uint end_snapshot = end;
		for (uint pos = startBleedingEdge; pos != end_snapshot; pos++)
		{
			if ((int)(startBleedingEdge - pos) > 0)
			{
				pos = startBleedingEdge;
				if ((int)(pos - end_snapshot) >= 0)
				{
					break;
				}
			}
			yield return elements[(ushort)pos % length];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public ICollection<T> Snapshot()
	{
		return elementsByIndex.Values;
	}

	public void Clear()
	{
		elementsByIndex.Clear();
		start = 0u;
		startBleedingEdge = 0u;
		end = 0u;
		endBleedingEdge = 0u;
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i] = default(T);
		}
	}
}
