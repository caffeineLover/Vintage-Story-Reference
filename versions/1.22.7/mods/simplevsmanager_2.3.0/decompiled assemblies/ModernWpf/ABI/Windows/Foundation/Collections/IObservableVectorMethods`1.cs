using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000FB RID: 251
	internal static class IObservableVectorMethods<T>
	{
		// Token: 0x060008E0 RID: 2272 RVA: 0x00019A92 File Offset: 0x00017C92
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> MakeVectorChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>>>(ref IObservableVectorMethods<T>._VectorChanged_, new ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>>(), null);
			return IObservableVectorMethods<T>._VectorChanged_;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00019AAC File Offset: 0x00017CAC
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> _VectorChanged
		{
			get
			{
				return IObservableVectorMethods<T>._VectorChanged_ ?? IObservableVectorMethods<T>.MakeVectorChangedTable();
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00019AC0 File Offset: 0x00017CC0
		public static ValueTuple<Action<Windows.Foundation.Collections.VectorChangedEventHandler<T>>, Action<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> Get_VectorChanged(IObjectReference _genericObj, object _thisObj)
		{
			IObservableVectorMethods<T>.<>c__DisplayClass4_0 CS$<>8__locals1 = new IObservableVectorMethods<T>.<>c__DisplayClass4_0();
			CS$<>8__locals1._genericObj = _genericObj;
			return IObservableVectorMethods<T>._VectorChanged.GetValue(_thisObj, new ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>>.CreateValueCallback(CS$<>8__locals1.<Get_VectorChanged>b__0)).EventActions;
		}

		// Token: 0x04000198 RID: 408
		private static volatile ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>> _VectorChanged_;
	}
}
