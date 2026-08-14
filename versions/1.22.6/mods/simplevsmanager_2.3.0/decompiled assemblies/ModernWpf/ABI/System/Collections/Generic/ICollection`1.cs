using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000156 RID: 342
	[DynamicInterfaceCastableImplementation]
	internal interface ICollection<T> : ICollection<!0>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x0001E54C File Offset: 0x0001C74C
		private static ICollection<T> CreateHelper(IWinRTObject _this)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair))
			{
				Type type = typeof(IDictionary).MakeGenericType(typeFromHandle.GetGenericArguments());
				if (_this.IsInterfaceImplemented(type.TypeHandle, false))
				{
					return (ICollection<!0>)typeof(IDictionaryImpl<, >).MakeGenericType(typeFromHandle.GetGenericArguments()).GetConstructor(36, null, new Type[]
					{
						typeof(IObjectReference)
					}, null).Invoke(new object[]
					{
						_this.NativeObject
					});
				}
			}
			Type typeFromHandle2 = typeof(IList<!0>);
			if (_this.IsInterfaceImplemented(typeFromHandle2.TypeHandle, false))
			{
				return new IListImpl<T>(_this.NativeObject);
			}
			throw new InvalidOperationException("ICollection helper can not determine derived type.");
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0001E628 File Offset: 0x0001C828
		private static ICollection<T> GetHelper(IWinRTObject _this)
		{
			ICollection<T>.<>c__DisplayClass1_0 CS$<>8__locals1 = new ICollection<T>.<>c__DisplayClass1_0();
			CS$<>8__locals1._this = _this;
			return (ICollection<!0>)CS$<>8__locals1._this.GetOrCreateTypeHelperData(typeof(ICollection<!0>).TypeHandle, new Func<object>(CS$<>8__locals1.<GetHelper>b__0));
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0001E66D File Offset: 0x0001C86D
		int ICollection<!0>.Count
		{
			get
			{
				return ICollection<T>.GetHelper((IWinRTObject)this).Count;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0001E67F File Offset: 0x0001C87F
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return ICollection<T>.GetHelper((IWinRTObject)this).IsReadOnly;
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0001E691 File Offset: 0x0001C891
		void Add(T item)
		{
			ICollection<T>.GetHelper((IWinRTObject)this).Add(item);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		void Clear()
		{
			ICollection<T>.GetHelper((IWinRTObject)this).Clear();
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0001E6B6 File Offset: 0x0001C8B6
		bool Contains(T item)
		{
			return ICollection<T>.GetHelper((IWinRTObject)this).Contains(item);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0001E6C9 File Offset: 0x0001C8C9
		void CopyTo(T[] array, int arrayIndex)
		{
			ICollection<T>.GetHelper((IWinRTObject)this).CopyTo(array, arrayIndex);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0001E6DD File Offset: 0x0001C8DD
		bool Remove(T item)
		{
			return ICollection<T>.GetHelper((IWinRTObject)this).Remove(item);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0001E6F0 File Offset: 0x0001C8F0
		IEnumerator GetEnumerator()
		{
			return ICollection<T>.GetHelper((IWinRTObject)this).GetEnumerator();
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0001E702 File Offset: 0x0001C902
		IEnumerator<T> GetEnumerator()
		{
			return ICollection<T>.GetHelper((IWinRTObject)this).GetEnumerator();
		}
	}
}
