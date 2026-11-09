using System;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x0200015F RID: 351
	internal static class IListMethods<T>
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0001F200 File Offset: 0x0001D400
		public static int get_Count(IObjectReference obj)
		{
			uint num = IVectorMethods<T>.get_Size(obj);
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
			}
			return (int)num;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0001F228 File Offset: 0x0001D428
		public static bool get_IsReadOnly(IObjectReference obj)
		{
			return false;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0001F22B File Offset: 0x0001D42B
		public static void Add(IObjectReference obj, T item)
		{
			IVectorMethods<T>.Append(obj, item);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0001F234 File Offset: 0x0001D434
		public static void Clear(IObjectReference obj)
		{
			IVectorMethods<T>.Clear(obj);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0001F23C File Offset: 0x0001D43C
		public static bool Contains(IObjectReference obj, T item)
		{
			uint num;
			return IVectorMethods<T>.IndexOf(obj, item, ref num);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0001F254 File Offset: 0x0001D454
		public static void CopyTo(IObjectReference obj, T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array.Length <= arrayIndex && IListMethods<T>.get_Count(obj) > 0)
			{
				throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_IndexOutOfArrayBounds);
			}
			if (array.Length - arrayIndex < IListMethods<T>.get_Count(obj))
			{
				throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_InsufficientSpaceToCopyCollection);
			}
			int num = IListMethods<T>.get_Count(obj);
			for (int i = 0; i < num; i++)
			{
				array[i + arrayIndex] = IListMethods<T>.GetAtHelper(obj, (uint)i);
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0001F2D4 File Offset: 0x0001D4D4
		public static bool Remove(IObjectReference obj, T item)
		{
			uint num;
			if (!IVectorMethods<T>.IndexOf(obj, item, ref num))
			{
				return false;
			}
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
			}
			IListMethods<T>.RemoveAtHelper(obj, num);
			return true;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0001F309 File Offset: 0x0001D509
		public static T Indexer_Get(IObjectReference obj, int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return IListMethods<T>.GetAtHelper(obj, (uint)index);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0001F321 File Offset: 0x0001D521
		public static void Indexer_Set(IObjectReference obj, int index, T value)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			IListMethods<T>.SetAtHelper(obj, (uint)index, value);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0001F33C File Offset: 0x0001D53C
		public static int IndexOf(IObjectReference obj, T item)
		{
			uint num;
			if (!IVectorMethods<T>.IndexOf(obj, item, ref num))
			{
				return -1;
			}
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
			}
			return (int)num;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0001F36A File Offset: 0x0001D56A
		public static void Insert(IObjectReference obj, int index, T item)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			IListMethods<T>.InsertAtHelper(obj, (uint)index, item);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0001F383 File Offset: 0x0001D583
		public static void RemoveAt(IObjectReference obj, int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			IListMethods<T>.RemoveAtHelper(obj, (uint)index);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0001F39C File Offset: 0x0001D59C
		internal static T GetAtHelper(IObjectReference obj, uint index)
		{
			T at;
			try
			{
				at = IVectorMethods<T>.GetAt(obj, index);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				throw;
			}
			return at;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0001F3E0 File Offset: 0x0001D5E0
		private static void SetAtHelper(IObjectReference obj, uint index, T value)
		{
			try
			{
				IVectorMethods<T>.SetAt(obj, index, value);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				throw;
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0001F424 File Offset: 0x0001D624
		private static void InsertAtHelper(IObjectReference obj, uint index, T item)
		{
			try
			{
				IVectorMethods<T>.InsertAt(obj, index, item);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				throw;
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0001F468 File Offset: 0x0001D668
		internal static void RemoveAtHelper(IObjectReference obj, uint index)
		{
			try
			{
				IVectorMethods<T>.RemoveAt(obj, index);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex.HResult)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				throw;
			}
		}
	}
}
