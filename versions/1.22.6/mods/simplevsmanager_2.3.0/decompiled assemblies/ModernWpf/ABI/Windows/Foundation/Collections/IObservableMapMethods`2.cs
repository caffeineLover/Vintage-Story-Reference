using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F8 RID: 248
	internal static class IObservableMapMethods<K, V>
	{
		// Token: 0x060008C9 RID: 2249 RVA: 0x00019880 File Offset: 0x00017A80
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> MakeMapChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>>>(ref IObservableMapMethods<K, V>._MapChanged_, new ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>>(), null);
			return IObservableMapMethods<K, V>._MapChanged_;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001989A File Offset: 0x00017A9A
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> _MapChanged
		{
			get
			{
				return IObservableMapMethods<K, V>._MapChanged_ ?? IObservableMapMethods<K, V>.MakeMapChangedTable();
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000198AC File Offset: 0x00017AAC
		public static ValueTuple<Action<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>, Action<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> Get_MapChanged(IObjectReference _genericObj, object _thisObj)
		{
			IObservableMapMethods<K, V>.<>c__DisplayClass4_0 CS$<>8__locals1 = new IObservableMapMethods<K, V>.<>c__DisplayClass4_0();
			CS$<>8__locals1._genericObj = _genericObj;
			return IObservableMapMethods<K, V>._MapChanged.GetValue(_thisObj, new ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>>.CreateValueCallback(CS$<>8__locals1.<Get_MapChanged>b__0)).EventActions;
		}

		// Token: 0x04000196 RID: 406
		private static volatile ConditionalWeakTable<object, EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>> _MapChanged_;
	}
}
