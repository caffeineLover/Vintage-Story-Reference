using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Util;

public static class ArrayExtensions
{
	public static T Nearest<T>(this T[] array, Func<T, double> getDistance)
	{
		double nearestDist = double.MaxValue;
		T nearest = default(T);
		for (int i = 0; i < array.Length; i++)
		{
			double d = getDistance(array[i]);
			if (d < nearestDist)
			{
				nearestDist = d;
				nearest = array[i];
			}
		}
		return nearest;
	}

	public static List<T> InRange<T>(this T[] array, Func<T, double> getDistance, double range)
	{
		List<T> found = new List<T>();
		for (int i = 0; i < array.Length; i++)
		{
			if (getDistance(array[i]) < range)
			{
				found.Add(array[i]);
			}
		}
		return found;
	}

	public static bool Contains<T>(this T[] array, Func<T, bool> predicate)
	{
		return array.IndexOf(predicate) >= 0;
	}

	public static int IndexOf<T>(this T[] array, Func<T, bool> predicate)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (predicate(array[i]))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (object.Equals(value, array[i]))
			{
				return i;
			}
		}
		return -1;
	}

	public static bool Contains<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (object.Equals(array[i], value))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Creates a new copy of the array with <paramref name="value" /> removed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static T[] Remove<T>(this T[] array, T value)
	{
		List<T> list = new List<T>(array);
		list.Remove(value);
		return list.ToArray();
	}

	[Obsolete("Use RemoveAt instead")]
	public static T[] RemoveEntry<T>(this T[] array, int index)
	{
		return array.RemoveAt(index);
	}

	/// <summary>
	/// Creates a new copy of array with element at index removed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static T[] RemoveAt<T>(this T[] array, int index)
	{
		T[] cut = GC.AllocateUninitializedArray<T>(array.Length - 1);
		if (index == 0)
		{
			if (cut.Length == 0)
			{
				return cut;
			}
			Array.Copy(array, 1, cut, 0, array.Length - 1);
		}
		else if (index == array.Length - 1)
		{
			Array.Copy(array, cut, array.Length - 1);
		}
		else
		{
			Array.Copy(array, 0, cut, 0, index);
			Array.Copy(array, index + 1, cut, index, array.Length - index - 1);
		}
		return cut;
	}

	/// <summary>
	/// Creates a new copy of the array with <paramref name="value" /> appened to the end of the array
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static T[] Append<T>(this T[] array, T value)
	{
		T[] grown = GC.AllocateUninitializedArray<T>(array.Length + 1);
		Array.Copy(array, grown, array.Length);
		grown[array.Length] = value;
		return grown;
	}

	/// <summary>
	/// Creates a new copy of the array with <paramref name="value" /> inserted at the given index
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="value"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	public static T[] InsertAt<T>(this T[] array, T value, int index)
	{
		T[] grown = GC.AllocateUninitializedArray<T>(array.Length + 1);
		if (index > 0)
		{
			Array.Copy(array, grown, index);
		}
		grown[index] = value;
		Array.Copy(array, index, grown, index + 1, array.Length - index);
		return grown;
	}

	/// <summary>
	/// Creates a new copy of the array with <paramref name="value" /> appended to the end of the array
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static T[] Append<T>(this T[] array, params T[] value)
	{
		if (array == null)
		{
			return null;
		}
		if (value == null || value.Length == 0)
		{
			return array;
		}
		int oldLength = array.Length;
		T[] grown = GC.AllocateUninitializedArray<T>(oldLength + value.Length);
		Array.Copy(array, grown, oldLength);
		for (int i = 0; i < value.Length; i++)
		{
			grown[oldLength + i] = value[i];
		}
		return grown;
	}

	/// <summary>
	/// Creates a new copy of the array with <paramref name="values" /> appended to the end of the array
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array"></param>
	/// <param name="values"></param>
	/// <returns></returns>
	public static T[] Append<T>(this T[] array, IEnumerable<T> values)
	{
		if (array == null)
		{
			return null;
		}
		if (values == null)
		{
			return array;
		}
		int oldLength = array.Length;
		T[] grown = GC.AllocateUninitializedArray<T>(oldLength + values.Count());
		Array.Copy(array, grown, oldLength);
		int i = 0;
		foreach (T val in values)
		{
			grown[oldLength + i] = val;
		}
		return grown;
	}

	/// <summary>
	/// Adds the values to this array starting at the specified index
	/// </summary>
	public static T[] InsertAt<T>(this T[] array, int index, params T[] values)
	{
		if (array == null)
		{
			return null;
		}
		if (values == null || values.Length == 0)
		{
			return array;
		}
		if (index + values.Length > array.Length)
		{
			return array.Append(values);
		}
		for (int i = 0; i < values.Length; i++)
		{
			array[index + i] = values[i];
		}
		return array;
	}

	public static T[] Fill<T>(this T[] originalArray, T with)
	{
		for (int i = 0; i < originalArray.Length; i++)
		{
			originalArray[i] = with;
		}
		return originalArray;
	}

	public static T[] Fill<T>(this T[] originalArray, fillCallback<T> fillCallback)
	{
		for (int i = 0; i < originalArray.Length; i++)
		{
			originalArray[i] = fillCallback(i);
		}
		return originalArray;
	}

	/// <summary>
	/// Performs a Fisher-Yates shuffle in linear time or O(n)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="rand"></param>
	/// <param name="array"></param>
	public static T[] Shuffle<T>(this T[] array, Random rand)
	{
		int n = array.Length;
		while (n > 1)
		{
			int k = rand.Next(n);
			n--;
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
		return array;
	}

	/// <summary>
	/// Performs a Fisher-Yates shuffle in linear time or O(n)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="rand"></param>
	/// <param name="array"></param>
	public static T[] Shuffle<T>(this T[] array, LCGRandom rand)
	{
		int n = array.Length;
		while (n > 1)
		{
			int k = rand.NextInt(n);
			n--;
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
		return array;
	}

	/// <summary>
	/// Test whether every element of the array matches
	/// </summary>
	public static bool DeepEquals<T>(this T[] array, T[] other)
	{
		if (array == null || other == null)
		{
			if (array == null)
			{
				return other == null;
			}
			return false;
		}
		if (array.Length != other.Length)
		{
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Equals(other[i]))
			{
				return false;
			}
		}
		return true;
	}
}
