using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000155 RID: 341
	[DynamicInterfaceCastableImplementation]
	internal interface IReadOnlyCollection<T> : IReadOnlyCollection<T>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x0001E3F4 File Offset: 0x0001C5F4
		private static IReadOnlyCollection<T> CreateHelper(IWinRTObject _this)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair))
			{
				Type type = typeof(IReadOnlyDictionary).MakeGenericType(typeFromHandle.GetGenericArguments());
				if (_this.IsInterfaceImplemented(type.TypeHandle, false))
				{
					return (IReadOnlyCollection<T>)typeof(IReadOnlyDictionaryImpl<, >).MakeGenericType(typeFromHandle.GetGenericArguments()).GetConstructor(36, null, new Type[]
					{
						typeof(IObjectReference)
					}, null).Invoke(new object[]
					{
						_this.NativeObject
					});
				}
			}
			Type typeFromHandle2 = typeof(IReadOnlyList<T>);
			if (_this.IsInterfaceImplemented(typeFromHandle2.TypeHandle, false))
			{
				return new IReadOnlyListImpl<T>(_this.NativeObject);
			}
			throw new InvalidOperationException("IReadOnlyCollection helper can not determine derived type.");
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0001E4D0 File Offset: 0x0001C6D0
		private static IReadOnlyCollection<T> GetHelper(IWinRTObject _this)
		{
			IReadOnlyCollection<T>.<>c__DisplayClass1_0 CS$<>8__locals1 = new IReadOnlyCollection<T>.<>c__DisplayClass1_0();
			CS$<>8__locals1._this = _this;
			return (IReadOnlyCollection<T>)CS$<>8__locals1._this.GetOrCreateTypeHelperData(typeof(IReadOnlyCollection<T>).TypeHandle, new Func<object>(CS$<>8__locals1.<GetHelper>b__0));
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0001E515 File Offset: 0x0001C715
		int IReadOnlyCollection<!0>.Count
		{
			get
			{
				return IReadOnlyCollection<T>.GetHelper((IWinRTObject)this).Count;
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0001E527 File Offset: 0x0001C727
		IEnumerator GetEnumerator()
		{
			return IReadOnlyCollection<T>.GetHelper((IWinRTObject)this).GetEnumerator();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0001E539 File Offset: 0x0001C739
		IEnumerator<T> GetEnumerator()
		{
			return IReadOnlyCollection<T>.GetHelper((IWinRTObject)this).GetEnumerator();
		}
	}
}
