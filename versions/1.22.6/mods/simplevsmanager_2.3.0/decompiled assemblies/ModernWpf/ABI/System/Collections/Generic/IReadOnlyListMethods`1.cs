using System;
using ABI.Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000166 RID: 358
	internal static class IReadOnlyListMethods<T>
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x000205DC File Offset: 0x0001E7DC
		public static int get_Count(IObjectReference obj)
		{
			uint num = IVectorViewMethods<T>.get_Size(obj);
			if (2147483647U < num)
			{
				throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
			}
			return (int)num;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00020604 File Offset: 0x0001E804
		public static T Indexer_Get(IObjectReference obj, int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			T at;
			try
			{
				at = IVectorViewMethods<T>.GetAt(obj, (uint)index);
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
	}
}
