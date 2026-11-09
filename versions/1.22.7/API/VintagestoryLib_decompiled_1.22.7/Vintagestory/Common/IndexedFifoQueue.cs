using System;
using System.Collections;
using System.Collections.Generic;

namespace Vintagestory.Common;

internal class IndexedFifoQueue<T> : IEnumerable<T>, IEnumerable where T : ILongIndex
{
	private readonly Dictionary<long, T> elementsByIndex;

	private readonly T[] elements;

	private int start;

	private int end;

	private bool isfull;

	public int Count
	{
		get
		{
			if (end >= start && (!isfull || end != start))
			{
				return end - start;
			}
			return elements.Length - start + end;
		}
	}

	public int Capacity => elements.Length;

	public IndexedFifoQueue(int capacity)
	{
		elements = new T[capacity];
		elementsByIndex = new Dictionary<long, T>(capacity);
	}

	public bool IsFull()
	{
		return isfull;
	}

	public T GetByIndex(long index)
	{
		elementsByIndex.TryGetValue(index, out var val);
		return val;
	}

	public T GetAtPosition(int position)
	{
		return elements[(start + position) % elements.Length];
	}

	public void Enqueue(T elem)
	{
		if (Count >= elements.Length - 1)
		{
			throw new Exception("Indexed Fifo Queue overflow");
		}
		elements[end] = elem;
		elementsByIndex[elem.Index] = elem;
		end++;
		if (end >= elements.Length)
		{
			end = 0;
		}
		isfull = start == end;
	}

	public T Dequeue()
	{
		T elem = elements[start];
		elements[start] = default(T);
		elementsByIndex.Remove(elem.Index);
		if (start != end)
		{
			start++;
		}
		if (start >= elements.Length)
		{
			start = 0;
		}
		isfull = false;
		return elem;
	}

	internal void Requeue()
	{
		T elem = Dequeue();
		if (elem != null)
		{
			Enqueue(elem);
		}
	}

	public T Peek()
	{
		return elements[start];
	}

	public bool Remove(long index)
	{
		bool found = false;
		elementsByIndex.Remove(index);
		for (int i = 0; i < Count; i++)
		{
			int pos = (i + start) % elements.Length;
			if (elements[pos].Index == index)
			{
				found = true;
			}
			if (found)
			{
				int nextPos = (pos + 1) % elements.Length;
				if (elements[nextPos] == null)
				{
					break;
				}
				elements[pos] = elements[nextPos];
			}
		}
		if (found)
		{
			end--;
			if (end < 0)
			{
				end += elements.Length;
			}
			elements[end] = default(T);
			isfull = false;
		}
		return found;
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < Count; i++)
		{
			int pos = i + start;
			if (pos >= elements.Length)
			{
				pos -= elements.Length;
			}
			yield return elements[pos];
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Clear()
	{
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i] = default(T);
		}
		elementsByIndex.Clear();
		start = 0;
		end = 0;
		isfull = false;
	}
}
