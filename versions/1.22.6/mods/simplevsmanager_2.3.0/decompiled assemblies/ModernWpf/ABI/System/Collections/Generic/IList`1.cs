using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000161 RID: 353
	[DynamicInterfaceCastableImplementation]
	[Guid("913337E9-11A1-4345-A3A2-4E7F956E222D")]
	internal interface IList<T> : IList<!0>, ICollection<!0>, IEnumerable<!0>, IEnumerable, IVector<T>, IIterable<T>
	{
		// Token: 0x06000B5C RID: 2908 RVA: 0x0001F9AC File Offset: 0x0001DBAC
		public static IObjectReference CreateMarshaler(IList<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IList<T>.Vftbl>(obj, IList<T>.PIID);
			}
			return null;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0001F9BE File Offset: 0x0001DBBE
		public static ObjectReferenceValue CreateMarshaler2(IList<T> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IList<T>.PIID);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0001F9CB File Offset: 0x0001DBCB
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0001F9DC File Offset: 0x0001DBDC
		public static IntPtr FromManaged(IList<T> value)
		{
			if (value != null)
			{
				return IList<T>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0001FA00 File Offset: 0x0001DC00
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0001FA0B File Offset: 0x0001DC0B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IVector<T>>.DisposeAbi(abi);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0001FA13 File Offset: 0x0001DC13
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IList<T>));
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0001FA24 File Offset: 0x0001DC24
		public static ObjectReference<IList<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IList<T>.Vftbl vftbl = new IList<T>.Vftbl(thisPtr);
			return ObjectReference<IList<T>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0001FA50 File Offset: 0x0001DC50
		T GetAt(uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			T result;
			try
			{
				objectReference.Vftbl.GetAt_0.DynamicInvokeAbi(array2);
				result = Marshaler<T>.FromAbi.Invoke(array2[2]);
			}
			finally
			{
				Marshaler<T>.DisposeAbi.Invoke(array2[2]);
			}
			return result;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0001FAE0 File Offset: 0x0001DCE0
		IReadOnlyList<T> GetView()
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IReadOnlyList<T> result;
			try
			{
				method getView_ = objectReference.Vftbl.GetView_2;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, getView_));
				result = MarshalInterface<IReadOnlyList<T>>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<IVectorView<T>>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0001FB60 File Offset: 0x0001DD60
		bool IndexOf(T value, out uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			object[] array = new object[4];
			array[0] = thisPtr;
			object[] array2 = array;
			bool result;
			try
			{
				obj = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[1] = Marshaler<T>.GetAbi.Invoke(obj);
				objectReference.Vftbl.IndexOf_3.DynamicInvokeAbi(array2);
				index = (uint)array2[2];
				result = ((byte)array2[3] > 0);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj);
			}
			return result;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0001FC08 File Offset: 0x0001DE08
		void SetAt(uint index, T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			try
			{
				obj = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[2] = Marshaler<T>.GetAbi.Invoke(obj);
				objectReference.Vftbl.SetAt_4.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj);
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0001FCA0 File Offset: 0x0001DEA0
		void InsertAt(uint index, T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			try
			{
				obj = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[2] = Marshaler<T>.GetAbi.Invoke(obj);
				objectReference.Vftbl.InsertAt_5.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj);
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0001FD38 File Offset: 0x0001DF38
		void RemoveAt(uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAt_ = objectReference.Vftbl.RemoveAt_6;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32), thisPtr, index, removeAt_));
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0001FD88 File Offset: 0x0001DF88
		void Append(T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			try
			{
				obj = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[1] = Marshaler<T>.GetAbi.Invoke(obj);
				objectReference.Vftbl.Append_7.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj);
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0001FE18 File Offset: 0x0001E018
		void RemoveAtEnd()
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAtEnd_ = objectReference.Vftbl.RemoveAtEnd_8;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, removeAtEnd_));
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0001FE68 File Offset: 0x0001E068
		void _Clear()
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method clear_ = objectReference.Vftbl.Clear_9;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, clear_));
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		uint GetMany(uint startIndex, ref T[] items)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			IntPtr intPtr = default(IntPtr);
			uint num = 0U;
			uint result;
			try
			{
				obj = Marshaler<T>.CreateMarshalerArray.Invoke(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray.Invoke(obj);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				method getMany_ = objectReference.Vftbl.GetMany_10;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.Int32,System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, startIndex, item, intPtr, ref num, getMany_));
				items = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(item, intPtr));
				result = num;
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray.Invoke(obj);
			}
			return result;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0001FF84 File Offset: 0x0001E184
		void ReplaceAll(T[] items)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj = null;
			IntPtr intPtr = default(IntPtr);
			try
			{
				obj = Marshaler<T>.CreateMarshalerArray.Invoke(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray.Invoke(obj);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				method replaceAll_ = objectReference.Vftbl.ReplaceAll_11;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr), thisPtr, item, intPtr, replaceAll_));
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray.Invoke(obj);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00020028 File Offset: 0x0001E228
		uint IVector<!0>.Size
		{
			get
			{
				ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				uint result = 0U;
				method getSize_ = objectReference.Vftbl.GetSize_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSize_));
				return result;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0002007A File Offset: 0x0001E27A
		int ICollection<!0>.Count
		{
			get
			{
				return IListMethods<T>.get_Count((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle));
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x000200A0 File Offset: 0x0001E2A0
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return IListMethods<T>.get_IsReadOnly((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle));
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x000200C6 File Offset: 0x0001E2C6
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x000200ED File Offset: 0x0001E2ED
		T IList<!0>.Item
		{
			get
			{
				return IListMethods<T>.Indexer_Get((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), index);
			}
			set
			{
				IListMethods<T>.Indexer_Set((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), index, value);
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00020115 File Offset: 0x0001E315
		int IndexOf(T item)
		{
			return IListMethods<T>.IndexOf((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), item);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002013C File Offset: 0x0001E33C
		void Insert(int index, T item)
		{
			IListMethods<T>.Insert((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), index, item);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00020164 File Offset: 0x0001E364
		void RemoveAt(int index)
		{
			IListMethods<T>.RemoveAt((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), index);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002018B File Offset: 0x0001E38B
		void Add(T item)
		{
			IListMethods<T>.Add((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), item);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000201B2 File Offset: 0x0001E3B2
		void Clear()
		{
			IListMethods<T>.Clear((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle));
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000201D8 File Offset: 0x0001E3D8
		bool Contains(T item)
		{
			return IListMethods<T>.Contains((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), item);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000201FF File Offset: 0x0001E3FF
		void CopyTo(T[] array, int arrayIndex)
		{
			IListMethods<T>.CopyTo((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), array, arrayIndex);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00020227 File Offset: 0x0001E427
		bool Remove(T item)
		{
			return IListMethods<T>.Remove((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList<!0>).TypeHandle), item);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00020250 File Offset: 0x0001E450
		IEnumerator<T> GetEnumerator()
		{
			((IWinRTObject)this).IsInterfaceImplemented(typeof(IEnumerable<!0>).TypeHandle, true);
			return IEnumerableMethods<T>.GetEnumerator((ObjectReference<IList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<!0>).TypeHandle));
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002029D File Offset: 0x0001E49D
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x000202A5 File Offset: 0x0001E4A5
		static IList()
		{
			IList<T>.PIID = IList<T>.Vftbl.PIID;
		}

		// Token: 0x040001F2 RID: 498
		public static Guid PIID;

		// Token: 0x0200038E RID: 910
		public sealed class ToAbiHelper : IVector<!0>, IIterable<T>
		{
			// Token: 0x0600197F RID: 6527 RVA: 0x00046584 File Offset: 0x00044784
			public ToAbiHelper(IList<T> list)
			{
				this._list = list;
			}

			// Token: 0x06001980 RID: 6528 RVA: 0x00046593 File Offset: 0x00044793
			IEnumerator<T> IIterable<!0>.First()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x06001981 RID: 6529 RVA: 0x000465A0 File Offset: 0x000447A0
			private static void EnsureIndexInt32(uint index, int limit = 2147483647)
			{
				if (2147483647U <= index || index >= (uint)limit)
				{
					ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index", WinRTRuntimeErrorStrings.ArgumentOutOfRange_IndexLargerThanMaxValue);
					ex.SetHResult(-2147483637);
					throw ex;
				}
			}

			// Token: 0x06001982 RID: 6530 RVA: 0x000465CC File Offset: 0x000447CC
			public T GetAt(uint index)
			{
				IList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				T result;
				try
				{
					result = this._list[(int)index];
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw innerException.GetExceptionForHR(-2147483637, WinRTRuntimeErrorStrings.ArgumentOutOfRange_Index);
				}
				return result;
			}

			// Token: 0x170004D2 RID: 1234
			// (get) Token: 0x06001983 RID: 6531 RVA: 0x0004661C File Offset: 0x0004481C
			public uint Size
			{
				get
				{
					return (uint)this._list.Count;
				}
			}

			// Token: 0x06001984 RID: 6532 RVA: 0x0004662C File Offset: 0x0004482C
			IReadOnlyList<T> IVector<!0>.GetView()
			{
				IReadOnlyList<T> readOnlyList = this._list as IReadOnlyList<T>;
				if (readOnlyList == null)
				{
					readOnlyList = new ReadOnlyCollection<T>(this._list);
				}
				return readOnlyList;
			}

			// Token: 0x06001985 RID: 6533 RVA: 0x00046658 File Offset: 0x00044858
			public bool IndexOf(T value, out uint index)
			{
				int num = this._list.IndexOf(value);
				if (-1 == num)
				{
					index = 0U;
					return false;
				}
				index = (uint)num;
				return true;
			}

			// Token: 0x06001986 RID: 6534 RVA: 0x00046680 File Offset: 0x00044880
			public void SetAt(uint index, T value)
			{
				IList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				try
				{
					this._list[(int)index] = value;
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw innerException.GetExceptionForHR(-2147483637, WinRTRuntimeErrorStrings.ArgumentOutOfRange_Index);
				}
			}

			// Token: 0x06001987 RID: 6535 RVA: 0x000466D0 File Offset: 0x000448D0
			public void InsertAt(uint index, T value)
			{
				IList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count + 1);
				try
				{
					this._list.Insert((int)index, value);
				}
				catch (ArgumentOutOfRangeException ex)
				{
					ex.SetHResult(-2147483637);
					throw;
				}
			}

			// Token: 0x06001988 RID: 6536 RVA: 0x0004671C File Offset: 0x0004491C
			public void RemoveAt(uint index)
			{
				IList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				try
				{
					this._list.RemoveAt((int)index);
				}
				catch (ArgumentOutOfRangeException ex)
				{
					ex.SetHResult(-2147483637);
					throw;
				}
			}

			// Token: 0x06001989 RID: 6537 RVA: 0x00046764 File Offset: 0x00044964
			public void Append(T value)
			{
				this._list.Add(value);
			}

			// Token: 0x0600198A RID: 6538 RVA: 0x00046774 File Offset: 0x00044974
			public void RemoveAtEnd()
			{
				if (this._list.Count == 0)
				{
					InvalidOperationException ex = new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CannotRemoveLastFromEmptyCollection);
					ex.SetHResult(-2147483637);
					throw ex;
				}
				uint count = (uint)this._list.Count;
				this.RemoveAt(count - 1U);
			}

			// Token: 0x0600198B RID: 6539 RVA: 0x000467B9 File Offset: 0x000449B9
			public void _Clear()
			{
				this._list.Clear();
			}

			// Token: 0x0600198C RID: 6540 RVA: 0x000467C6 File Offset: 0x000449C6
			public uint GetMany(uint startIndex, ref T[] items)
			{
				return IList<T>.ToAbiHelper.GetManyHelper(this._list, startIndex, items);
			}

			// Token: 0x0600198D RID: 6541 RVA: 0x000467D8 File Offset: 0x000449D8
			public void ReplaceAll(T[] items)
			{
				this._list.Clear();
				if (items != null)
				{
					foreach (T t in items)
					{
						this._list.Add(t);
					}
				}
			}

			// Token: 0x0600198E RID: 6542 RVA: 0x00046818 File Offset: 0x00044A18
			private static uint GetManyHelper(IList<T> sourceList, uint startIndex, T[] items)
			{
				if ((ulong)startIndex == (ulong)((long)sourceList.Count))
				{
					return 0U;
				}
				IList<T>.ToAbiHelper.EnsureIndexInt32(startIndex, sourceList.Count);
				if (items == null)
				{
					return 0U;
				}
				uint num = Math.Min((uint)items.Length, (uint)(sourceList.Count - (int)startIndex));
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					items[(int)num2] = sourceList[(int)(num2 + startIndex)];
				}
				if (typeof(T) == typeof(string))
				{
					string[] array = items as string[];
					uint num3 = num;
					while ((ulong)num3 < (ulong)((long)items.Length))
					{
						array[(int)num3] = string.Empty;
						num3 += 1U;
					}
				}
				return num;
			}

			// Token: 0x04000901 RID: 2305
			private IList<T> _list;
		}

		// Token: 0x0200038F RID: 911
		[Guid("913337E9-11A1-4345-A3A2-4E7F956E222D")]
		public struct Vftbl
		{
			// Token: 0x170004D3 RID: 1235
			// (get) Token: 0x0600198F RID: 6543 RVA: 0x000468AB File Offset: 0x00044AAB
			// (set) Token: 0x06001990 RID: 6544 RVA: 0x000468B3 File Offset: 0x00044AB3
			internal method GetSize_1
			{
				get
				{
					return this._get_Size_1;
				}
				set
				{
					this._get_Size_1 = value;
				}
			}

			// Token: 0x170004D4 RID: 1236
			// (get) Token: 0x06001991 RID: 6545 RVA: 0x000468BC File Offset: 0x00044ABC
			// (set) Token: 0x06001992 RID: 6546 RVA: 0x000468C4 File Offset: 0x00044AC4
			public method GetView_2
			{
				get
				{
					return this._getView_2;
				}
				set
				{
					this._getView_2 = value;
				}
			}

			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06001993 RID: 6547 RVA: 0x000468CD File Offset: 0x00044ACD
			// (set) Token: 0x06001994 RID: 6548 RVA: 0x000468D5 File Offset: 0x00044AD5
			public method RemoveAt_6
			{
				get
				{
					return this._removeAt_6;
				}
				set
				{
					this._removeAt_6 = value;
				}
			}

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06001995 RID: 6549 RVA: 0x000468DE File Offset: 0x00044ADE
			// (set) Token: 0x06001996 RID: 6550 RVA: 0x000468E6 File Offset: 0x00044AE6
			public method RemoveAtEnd_8
			{
				get
				{
					return this._removeAtEnd_8;
				}
				set
				{
					this._removeAtEnd_8 = value;
				}
			}

			// Token: 0x170004D7 RID: 1239
			// (get) Token: 0x06001997 RID: 6551 RVA: 0x000468EF File Offset: 0x00044AEF
			// (set) Token: 0x06001998 RID: 6552 RVA: 0x000468F7 File Offset: 0x00044AF7
			public method Clear_9
			{
				get
				{
					return this._clear_9;
				}
				set
				{
					this._clear_9 = value;
				}
			}

			// Token: 0x170004D8 RID: 1240
			// (get) Token: 0x06001999 RID: 6553 RVA: 0x00046900 File Offset: 0x00044B00
			// (set) Token: 0x0600199A RID: 6554 RVA: 0x00046908 File Offset: 0x00044B08
			public method GetMany_10
			{
				get
				{
					return this._getMany_10;
				}
				set
				{
					this._getMany_10 = value;
				}
			}

			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600199B RID: 6555 RVA: 0x00046911 File Offset: 0x00044B11
			// (set) Token: 0x0600199C RID: 6556 RVA: 0x00046919 File Offset: 0x00044B19
			public method ReplaceAll_11
			{
				get
				{
					return this._replaceAll_11;
				}
				set
				{
					this._replaceAll_11 = value;
				}
			}

			// Token: 0x0600199D RID: 6557 RVA: 0x00046924 File Offset: 0x00044B24
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IList<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.GetAt_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IList<T>.Vftbl.GetAt_0_Type);
				this.GetSize_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.GetView_2 = (void*)ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.IndexOf_3 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IList<T>.Vftbl.IndexOf_3_Type);
				this.SetAt_4 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IList<T>.Vftbl.SetAt_4_Type);
				this.InsertAt_5 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)11 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IList<T>.Vftbl.InsertAt_5_Type);
				this.RemoveAt_6 = (void*)ptr[(IntPtr)12 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.Append_7 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)13 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IList<T>.Vftbl.Append_7_Type);
				this.RemoveAtEnd_8 = (void*)ptr[(IntPtr)14 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.Clear_9 = (void*)ptr[(IntPtr)15 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.GetMany_10 = (void*)ptr[(IntPtr)16 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.ReplaceAll_11 = (void*)ptr[(IntPtr)17 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x0600199E RID: 6558 RVA: 0x00046A94 File Offset: 0x00044C94
			unsafe static Vftbl()
			{
				IList<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IList<T>));
				IList<T>.Vftbl.GetAt_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					typeof(uint),
					Marshaler<T>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IList<T>.Vftbl.IndexOf_3_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<T>.AbiType,
					typeof(uint).MakeByRefType(),
					typeof(byte).MakeByRefType(),
					typeof(int)
				});
				IList<T>.Vftbl.SetAt_4_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					typeof(uint),
					Marshaler<T>.AbiType,
					typeof(int)
				});
				IList<T>.Vftbl.InsertAt_5_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					typeof(uint),
					Marshaler<T>.AbiType,
					typeof(int)
				});
				IList<T>.Vftbl.Append_7_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<T>.AbiType,
					typeof(int)
				});
				IList<T>.Vftbl.DelegateCache = new Delegate[7];
				IList<T>.Vftbl._adapterTable = new ConditionalWeakTable<IList<T>, IList<T>.ToAbiHelper>();
				IList<T>.Vftbl.AbiToProjectionVftable = new IList<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					GetAt_0 = Delegate.CreateDelegate(IList<T>.Vftbl.GetAt_0_Type, typeof(IList<T>.Vftbl).GetMethod("Do_Abi_GetAt_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_get_Size_1 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[0] = new _get_PropertyAsUInt32(IList<T>.Vftbl.Do_Abi_get_Size_1)),
					_getView_2 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[1] = new IList_Delegates.GetView_2(IList<T>.Vftbl.Do_Abi_GetView_2)),
					IndexOf_3 = Delegate.CreateDelegate(IList<T>.Vftbl.IndexOf_3_Type, typeof(IList<T>.Vftbl).GetMethod("Do_Abi_IndexOf_3", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					SetAt_4 = Delegate.CreateDelegate(IList<T>.Vftbl.SetAt_4_Type, typeof(IList<T>.Vftbl).GetMethod("Do_Abi_SetAt_4", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					InsertAt_5 = Delegate.CreateDelegate(IList<T>.Vftbl.InsertAt_5_Type, typeof(IList<T>.Vftbl).GetMethod("Do_Abi_InsertAt_5", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_removeAt_6 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[2] = new IList_Delegates.RemoveAt_6(IList<T>.Vftbl.Do_Abi_RemoveAt_6)),
					Append_7 = Delegate.CreateDelegate(IList<T>.Vftbl.Append_7_Type, typeof(IList<T>.Vftbl).GetMethod("Do_Abi_Append_7", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_removeAtEnd_8 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[3] = new IList_Delegates.RemoveAtEnd_8(IList<T>.Vftbl.Do_Abi_RemoveAtEnd_8)),
					_clear_9 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[4] = new IList_Delegates.Clear_9(IList<T>.Vftbl.Do_Abi_Clear_9)),
					_getMany_10 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[5] = new IList_Delegates.GetMany_10(IList<T>.Vftbl.Do_Abi_GetMany_10)),
					_replaceAll_11 = (void*)Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.DelegateCache[6] = new IList_Delegates.ReplaceAll_11(IList<T>.Vftbl.Do_Abi_ReplaceAll_11))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 12));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IList<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.AbiToProjectionVftable.GetAt_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable.GetSize_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable.GetView_2;
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.AbiToProjectionVftable.IndexOf_3);
				ptr[(IntPtr)10 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.AbiToProjectionVftable.SetAt_4);
				ptr[(IntPtr)11 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.AbiToProjectionVftable.InsertAt_5);
				ptr[(IntPtr)12 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable._removeAt_6;
				ptr[(IntPtr)13 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IList<T>.Vftbl.AbiToProjectionVftable.Append_7);
				ptr[(IntPtr)14 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable._removeAtEnd_8;
				ptr[(IntPtr)15 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable._clear_9;
				ptr[(IntPtr)16 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable._getMany_10;
				ptr[(IntPtr)17 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IList<T>.Vftbl.AbiToProjectionVftable._replaceAll_11;
				IList<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600199F RID: 6559 RVA: 0x00046FEC File Offset: 0x000451EC
			private static IVector<T> FindAdapter(IntPtr thisPtr)
			{
				IList<T> list = ComWrappersSupport.FindObject<IList<T>>(thisPtr);
				ConditionalWeakTable<IList<T>, IList<T>.ToAbiHelper> adapterTable = IList<T>.Vftbl._adapterTable;
				IList<T> list2 = list;
				ConditionalWeakTable<IList<T>, IList<T>.ToAbiHelper>.CreateValueCallback createValueCallback;
				if ((createValueCallback = IList<T>.Vftbl.<>c.<>9__46_0) == null)
				{
					createValueCallback = (IList<T>.Vftbl.<>c.<>9__46_0 = new ConditionalWeakTable<IList<T>, IList<T>.ToAbiHelper>.CreateValueCallback(IList<T>.Vftbl.<>c.<>9.<FindAdapter>b__46_0));
				}
				return adapterTable.GetValue(list2, createValueCallback);
			}

			// Token: 0x060019A0 RID: 6560 RVA: 0x0004702C File Offset: 0x0004522C
			private unsafe static int Do_Abi_GetAt_0<TAbi>(void* thisPtr, uint index, out TAbi __return_value__)
			{
				T t = default(T);
				__return_value__ = default(TAbi);
				try
				{
					t = IList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).GetAt(index);
					__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged.Invoke(t));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A1 RID: 6561 RVA: 0x00047094 File Offset: 0x00045294
			private static int Do_Abi_GetView_2(IntPtr thisPtr, out IntPtr __return_value__)
			{
				__return_value__ = default(IntPtr);
				try
				{
					IReadOnlyList<T> view = IList<T>.Vftbl.FindAdapter(thisPtr).GetView();
					__return_value__ = MarshalInterface<IReadOnlyList<T>>.FromManaged(view);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A2 RID: 6562 RVA: 0x000470E4 File Offset: 0x000452E4
			private unsafe static int Do_Abi_IndexOf_3<TAbi>(void* thisPtr, TAbi value, out uint index, out byte __return_value__)
			{
				index = 0U;
				__return_value__ = 0;
				uint num = 0U;
				try
				{
					bool flag = IList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).IndexOf(Marshaler<T>.FromAbi.Invoke(value), ref num);
					index = num;
					__return_value__ = (flag ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A3 RID: 6563 RVA: 0x00047150 File Offset: 0x00045350
			private unsafe static int Do_Abi_SetAt_4<TAbi>(void* thisPtr, uint index, TAbi value)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).SetAt(index, Marshaler<T>.FromAbi.Invoke(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A4 RID: 6564 RVA: 0x000471A4 File Offset: 0x000453A4
			private unsafe static int Do_Abi_InsertAt_5<TAbi>(void* thisPtr, uint index, TAbi value)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).InsertAt(index, Marshaler<T>.FromAbi.Invoke(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A5 RID: 6565 RVA: 0x000471F8 File Offset: 0x000453F8
			private static int Do_Abi_RemoveAt_6(IntPtr thisPtr, uint index)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(thisPtr).RemoveAt(index);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A6 RID: 6566 RVA: 0x00047234 File Offset: 0x00045434
			private unsafe static int Do_Abi_Append_7<TAbi>(void* thisPtr, TAbi value)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).Append(Marshaler<T>.FromAbi.Invoke(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A7 RID: 6567 RVA: 0x00047284 File Offset: 0x00045484
			private static int Do_Abi_RemoveAtEnd_8(IntPtr thisPtr)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(thisPtr).RemoveAtEnd();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A8 RID: 6568 RVA: 0x000472C0 File Offset: 0x000454C0
			private static int Do_Abi_Clear_9(IntPtr thisPtr)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(thisPtr)._Clear();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019A9 RID: 6569 RVA: 0x000472FC File Offset: 0x000454FC
			private static int Do_Abi_GetMany_10(IntPtr thisPtr, uint startIndex, int __itemsSize, IntPtr items, out uint __return_value__)
			{
				__return_value__ = 0U;
				T[] array = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(__itemsSize, items));
				try
				{
					uint many = IList<T>.Vftbl.FindAdapter(thisPtr).GetMany(startIndex, ref array);
					Marshaler<T>.CopyManagedArray.Invoke(array, items);
					__return_value__ = many;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019AA RID: 6570 RVA: 0x00047368 File Offset: 0x00045568
			private static int Do_Abi_ReplaceAll_11(IntPtr thisPtr, int __itemsSize, IntPtr items)
			{
				try
				{
					IList<T>.Vftbl.FindAdapter(thisPtr).ReplaceAll(Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(__itemsSize, items)));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019AB RID: 6571 RVA: 0x000473BC File Offset: 0x000455BC
			private static int Do_Abi_get_Size_1(IntPtr thisPtr, out uint __return_value__)
			{
				__return_value__ = 0U;
				try
				{
					uint size = IList<T>.Vftbl.FindAdapter(thisPtr).Size;
					__return_value__ = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000902 RID: 2306
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000903 RID: 2307
			public Delegate GetAt_0;

			// Token: 0x04000904 RID: 2308
			private unsafe void* _get_Size_1;

			// Token: 0x04000905 RID: 2309
			private unsafe void* _getView_2;

			// Token: 0x04000906 RID: 2310
			public Delegate IndexOf_3;

			// Token: 0x04000907 RID: 2311
			public Delegate SetAt_4;

			// Token: 0x04000908 RID: 2312
			public Delegate InsertAt_5;

			// Token: 0x04000909 RID: 2313
			private unsafe void* _removeAt_6;

			// Token: 0x0400090A RID: 2314
			public Delegate Append_7;

			// Token: 0x0400090B RID: 2315
			private unsafe void* _removeAtEnd_8;

			// Token: 0x0400090C RID: 2316
			private unsafe void* _clear_9;

			// Token: 0x0400090D RID: 2317
			private unsafe void* _getMany_10;

			// Token: 0x0400090E RID: 2318
			private unsafe void* _replaceAll_11;

			// Token: 0x0400090F RID: 2319
			public static Guid PIID;

			// Token: 0x04000910 RID: 2320
			private static readonly Type GetAt_0_Type;

			// Token: 0x04000911 RID: 2321
			private static readonly Type IndexOf_3_Type;

			// Token: 0x04000912 RID: 2322
			private static readonly Type SetAt_4_Type;

			// Token: 0x04000913 RID: 2323
			private static readonly Type InsertAt_5_Type;

			// Token: 0x04000914 RID: 2324
			private static readonly Type Append_7_Type;

			// Token: 0x04000915 RID: 2325
			private static readonly IList<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x04000916 RID: 2326
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x04000917 RID: 2327
			private static readonly Delegate[] DelegateCache;

			// Token: 0x04000918 RID: 2328
			private static ConditionalWeakTable<IList<T>, IList<T>.ToAbiHelper> _adapterTable;
		}
	}
}
