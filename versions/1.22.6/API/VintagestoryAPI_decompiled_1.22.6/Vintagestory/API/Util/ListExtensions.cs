using System;
using System.Collections.Generic;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Util;

public static class ListExtensions
{
	/// <summary>
	/// Performs a Fisher-Yates shuffle in linear time or O(n)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="rand"></param>
	/// <param name="array"></param>
	public static void Shuffle<T>(this List<T> array, Random rand)
	{
		int n = array.Count;
		while (n > 1)
		{
			int k = rand.Next(n);
			n--;
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	/// <summary>
	/// Performs a Fisher-Yates shuffle in linear time or O(n)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="rand"></param>
	/// <param name="array"></param>
	public static void Shuffle<T>(this List<T> array, IRandom rand)
	{
		int n = array.Count;
		while (n > 1)
		{
			int k = rand.NextInt(n);
			n--;
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static T PopLast<T>(this List<T> items)
	{
		T result = items[items.Count - 1];
		items.RemoveAt(items.Count - 1);
		return result;
	}

	public static bool Contains<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		foreach (TSource item in source)
		{
			if (predicate(item))
			{
				return true;
			}
		}
		return false;
	}

	public static void AddIfNotPresent<T>(this List<T> list, T item)
	{
		int count = list.Count;
		if (item != null)
		{
			T[] listArray = ListAccess<T>.Items(list);
			for (int i = 0; i < listArray.Length && i < count; i++)
			{
				if (item.Equals(listArray[i]))
				{
					return;
				}
			}
		}
		else if (list.Contains(item))
		{
			return;
		}
		list.Add(item);
	}
}
