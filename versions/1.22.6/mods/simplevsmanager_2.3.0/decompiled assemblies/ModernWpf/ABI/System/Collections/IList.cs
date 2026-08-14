using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Interop;
using WinRT;

namespace ABI.System.Collections
{
	// Token: 0x0200014E RID: 334
	[DynamicInterfaceCastableImplementation]
	[Guid("393DE7DE-6FD0-4C0D-BB71-47244A113E93")]
	internal interface IList : IList, ICollection, IEnumerable, IBindableVector
	{
		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001DA26 File Offset: 0x0001BC26
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IList));
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001DA37 File Offset: 0x0001BC37
		internal static ObjectReference<IList.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return ObjectReference<IList.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001DA50 File Offset: 0x0001BC50
		private static IList.FromAbiHelper _VectorToList(IWinRTObject _this)
		{
			return (IList.FromAbiHelper)_this.GetOrCreateTypeHelperData(typeof(IList).TypeHandle, () => new IList.FromAbiHelper((IBindableVector)_this));
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001DA98 File Offset: 0x0001BC98
		unsafe object GetAt(uint index)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method getAt_ = objectReference.Vftbl.GetAt_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr*), thisPtr, index, &intPtr, getAt_));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001DB18 File Offset: 0x0001BD18
		unsafe IBindableVectorView GetView()
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IBindableVectorView result;
			try
			{
				method getView_ = objectReference.Vftbl.GetView_2;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), thisPtr, &intPtr, getView_));
				result = MarshalInterface<IBindableVectorView>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<IBindableVectorView>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001DB98 File Offset: 0x0001BD98
		unsafe bool IndexOf(object value, out uint index)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			uint num = 0U;
			byte b = 0;
			bool result;
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method indexOf_ = objectReference.Vftbl.IndexOf_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.UInt32*,System.Byte*), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), &num, &b, indexOf_));
				index = num;
				result = (b > 0);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
			return result;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001DC30 File Offset: 0x0001BE30
		void SetAt(uint index, object value)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method setAt_ = objectReference.Vftbl.SetAt_4;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr), thisPtr, index, MarshalInspectable<object>.GetAbi(objectReferenceValue), setAt_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001DCB4 File Offset: 0x0001BEB4
		void InsertAt(uint index, object value)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method insertAt_ = objectReference.Vftbl.InsertAt_5;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr), thisPtr, index, MarshalInspectable<object>.GetAbi(objectReferenceValue), insertAt_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0001DD38 File Offset: 0x0001BF38
		void RemoveAt(uint index)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAt_ = objectReference.Vftbl.RemoveAt_6;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32), thisPtr, index, removeAt_));
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0001DD88 File Offset: 0x0001BF88
		void Append(object value)
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method append_ = objectReference.Vftbl.Append_7;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), append_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0001DE08 File Offset: 0x0001C008
		void RemoveAtEnd()
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAtEnd_ = objectReference.Vftbl.RemoveAtEnd_8;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, removeAtEnd_));
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x0001DE58 File Offset: 0x0001C058
		void Clear()
		{
			ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method clear_ = objectReference.Vftbl.Clear_9;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, clear_));
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0001DEA8 File Offset: 0x0001C0A8
		unsafe uint IBindableVector.Size
		{
			get
			{
				ObjectReference<IList.Vftbl> objectReference = (ObjectReference<IList.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IList).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				uint result = 0U;
				method get_Size_ = objectReference.Vftbl.get_Size_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32*), thisPtr, &result, get_Size_));
				return result;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0001DEFB File Offset: 0x0001C0FB
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x0001DF0E File Offset: 0x0001C10E
		object IList.Item
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this)[index];
			}
			set
			{
				IList._VectorToList((IWinRTObject)this)[index] = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0001DF22 File Offset: 0x0001C122
		bool IList.IsFixedSize
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this).IsFixedSize;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0001DF34 File Offset: 0x0001C134
		bool IList.IsReadOnly
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this).IsReadOnly;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0001DF46 File Offset: 0x0001C146
		int ICollection.Count
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this).Count;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0001DF58 File Offset: 0x0001C158
		bool ICollection.IsSynchronized
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this).IsSynchronized;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0001DF6A File Offset: 0x0001C16A
		object ICollection.SyncRoot
		{
			get
			{
				return IList._VectorToList((IWinRTObject)this).SyncRoot;
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001DF7C File Offset: 0x0001C17C
		int Add(object value)
		{
			return IList._VectorToList((IWinRTObject)this).Add(value);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0001DF8F File Offset: 0x0001C18F
		void Clear()
		{
			IList._VectorToList((IWinRTObject)this).Clear();
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0001DFA1 File Offset: 0x0001C1A1
		bool Contains(object value)
		{
			return IList._VectorToList((IWinRTObject)this).Contains(value);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		int IndexOf(object value)
		{
			return IList._VectorToList((IWinRTObject)this).IndexOf(value);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0001DFC7 File Offset: 0x0001C1C7
		void Insert(int index, object value)
		{
			IList._VectorToList((IWinRTObject)this).Insert(index, value);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0001DFDB File Offset: 0x0001C1DB
		void Remove(object value)
		{
			IList._VectorToList((IWinRTObject)this).Remove(value);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0001DFEE File Offset: 0x0001C1EE
		void RemoveAt(int index)
		{
			IList._VectorToList((IWinRTObject)this).RemoveAt(index);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0001E001 File Offset: 0x0001C201
		void CopyTo(Array array, int index)
		{
			IList._VectorToList((IWinRTObject)this).CopyTo(array, index);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0001E015 File Offset: 0x0001C215
		IEnumerator GetEnumerator()
		{
			return IList._VectorToList((IWinRTObject)this).GetEnumerator();
		}

		// Token: 0x0200036A RID: 874
		public sealed class FromAbiHelper : IList, ICollection, IEnumerable
		{
			// Token: 0x0600188E RID: 6286 RVA: 0x00044432 File Offset: 0x00042632
			public FromAbiHelper(IBindableVector vector)
			{
				this._vector = vector;
			}

			// Token: 0x170004A9 RID: 1193
			// (get) Token: 0x0600188F RID: 6287 RVA: 0x00044441 File Offset: 0x00042641
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004AA RID: 1194
			// (get) Token: 0x06001890 RID: 6288 RVA: 0x00044444 File Offset: 0x00042644
			public object SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004AB RID: 1195
			// (get) Token: 0x06001891 RID: 6289 RVA: 0x00044448 File Offset: 0x00042648
			public int Count
			{
				get
				{
					uint size = this._vector.Size;
					if (2147483647U < size)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
					}
					return (int)size;
				}
			}

			// Token: 0x06001892 RID: 6290 RVA: 0x00044478 File Offset: 0x00042678
			public void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Arg_RankMultiDimNotSupported);
				}
				int lowerBound = array.GetLowerBound(0);
				int count = this.Count;
				int length = array.GetLength(0);
				if (arrayIndex < lowerBound)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				if (count > length - (arrayIndex - lowerBound))
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_InsufficientSpaceToCopyCollection);
				}
				if (arrayIndex - lowerBound > length)
				{
					throw new ArgumentException(WinRTRuntimeErrorStrings.Argument_IndexOutOfArrayBounds);
				}
				uint num = 0U;
				while ((ulong)num < (ulong)((long)count))
				{
					array.SetValue(this._vector.GetAt(num), (long)((ulong)num + (ulong)((long)arrayIndex)));
					num += 1U;
				}
			}

			// Token: 0x170004AC RID: 1196
			public object this[int index]
			{
				get
				{
					return this.Indexer_Get(index);
				}
				set
				{
					this.Indexer_Set(index, value);
				}
			}

			// Token: 0x06001895 RID: 6293 RVA: 0x00044529 File Offset: 0x00042729
			internal object Indexer_Get(int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return IList.FromAbiHelper.GetAt(this._vector, (uint)index);
			}

			// Token: 0x06001896 RID: 6294 RVA: 0x00044546 File Offset: 0x00042746
			internal void Indexer_Set(int index, object value)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				IList.FromAbiHelper.SetAt(this._vector, (uint)index, value);
			}

			// Token: 0x06001897 RID: 6295 RVA: 0x00044564 File Offset: 0x00042764
			public int Add(object value)
			{
				this._vector.Append(value);
				uint size = this._vector.Size;
				if (2147483647U < size)
				{
					throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
				}
				return (int)(size - 1U);
			}

			// Token: 0x06001898 RID: 6296 RVA: 0x000445A0 File Offset: 0x000427A0
			public bool Contains(object item)
			{
				uint num;
				return this._vector.IndexOf(item, out num);
			}

			// Token: 0x06001899 RID: 6297 RVA: 0x000445BB File Offset: 0x000427BB
			public void Clear()
			{
				this._vector.Clear();
			}

			// Token: 0x170004AD RID: 1197
			// (get) Token: 0x0600189A RID: 6298 RVA: 0x000445C8 File Offset: 0x000427C8
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004AE RID: 1198
			// (get) Token: 0x0600189B RID: 6299 RVA: 0x000445CB File Offset: 0x000427CB
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600189C RID: 6300 RVA: 0x000445D0 File Offset: 0x000427D0
			public int IndexOf(object item)
			{
				uint num;
				if (!this._vector.IndexOf(item, out num))
				{
					return -1;
				}
				if (2147483647U < num)
				{
					throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
				}
				return (int)num;
			}

			// Token: 0x0600189D RID: 6301 RVA: 0x00044603 File Offset: 0x00042803
			public void Insert(int index, object item)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				IList.FromAbiHelper.InsertAtHelper(this._vector, (uint)index, item);
			}

			// Token: 0x0600189E RID: 6302 RVA: 0x00044624 File Offset: 0x00042824
			public void Remove(object item)
			{
				uint num;
				if (this._vector.IndexOf(item, out num))
				{
					if (2147483647U < num)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
					}
					IList.FromAbiHelper.RemoveAtHelper(this._vector, num);
				}
			}

			// Token: 0x0600189F RID: 6303 RVA: 0x00044660 File Offset: 0x00042860
			public void RemoveAt(int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				IList.FromAbiHelper.RemoveAtHelper(this._vector, (uint)index);
			}

			// Token: 0x060018A0 RID: 6304 RVA: 0x00044680 File Offset: 0x00042880
			private static object GetAt(IBindableVector _this, uint index)
			{
				object at;
				try
				{
					at = _this.GetAt(index);
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

			// Token: 0x060018A1 RID: 6305 RVA: 0x000446C4 File Offset: 0x000428C4
			private static void SetAt(IBindableVector _this, uint index, object value)
			{
				try
				{
					_this.SetAt(index, value);
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

			// Token: 0x060018A2 RID: 6306 RVA: 0x00044708 File Offset: 0x00042908
			private static void InsertAtHelper(IBindableVector _this, uint index, object item)
			{
				try
				{
					_this.InsertAt(index, item);
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

			// Token: 0x060018A3 RID: 6307 RVA: 0x0004474C File Offset: 0x0004294C
			private static void RemoveAtHelper(IBindableVector _this, uint index)
			{
				try
				{
					_this.RemoveAt(index);
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

			// Token: 0x060018A4 RID: 6308 RVA: 0x00044790 File Offset: 0x00042990
			public IEnumerator GetEnumerator()
			{
				return ((IEnumerable)((IWinRTObject)this._vector)).GetEnumerator();
			}

			// Token: 0x040008B2 RID: 2226
			private readonly IBindableVector _vector;
		}

		// Token: 0x0200036B RID: 875
		public sealed class ToAbiHelper : IBindableVector, IEnumerable
		{
			// Token: 0x060018A5 RID: 6309 RVA: 0x000447A7 File Offset: 0x000429A7
			public ToAbiHelper(IList list)
			{
				this._list = list;
			}

			// Token: 0x060018A6 RID: 6310 RVA: 0x000447B8 File Offset: 0x000429B8
			public object GetAt(uint index)
			{
				IList.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				object result;
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

			// Token: 0x170004AF RID: 1199
			// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00044808 File Offset: 0x00042A08
			public uint Size
			{
				get
				{
					return (uint)this._list.Count;
				}
			}

			// Token: 0x060018A8 RID: 6312 RVA: 0x00044815 File Offset: 0x00042A15
			IBindableVectorView IBindableVector.GetView()
			{
				return new IList.ToAbiHelper.ListToBindableVectorViewAdapter(this._list);
			}

			// Token: 0x060018A9 RID: 6313 RVA: 0x00044824 File Offset: 0x00042A24
			public bool IndexOf(object value, out uint index)
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

			// Token: 0x060018AA RID: 6314 RVA: 0x0004484C File Offset: 0x00042A4C
			public void SetAt(uint index, object value)
			{
				IList.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				try
				{
					this._list[(int)index] = value;
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw innerException.GetExceptionForHR(-2147483637, WinRTRuntimeErrorStrings.ArgumentOutOfRange_Index);
				}
			}

			// Token: 0x060018AB RID: 6315 RVA: 0x0004489C File Offset: 0x00042A9C
			public void InsertAt(uint index, object value)
			{
				IList.ToAbiHelper.EnsureIndexInt32(index, this._list.Count + 1);
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

			// Token: 0x060018AC RID: 6316 RVA: 0x000448E8 File Offset: 0x00042AE8
			public void RemoveAt(uint index)
			{
				IList.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
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

			// Token: 0x060018AD RID: 6317 RVA: 0x00044930 File Offset: 0x00042B30
			public void Append(object value)
			{
				this._list.Add(value);
			}

			// Token: 0x060018AE RID: 6318 RVA: 0x00044940 File Offset: 0x00042B40
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

			// Token: 0x060018AF RID: 6319 RVA: 0x00044985 File Offset: 0x00042B85
			public void Clear()
			{
				this._list.Clear();
			}

			// Token: 0x060018B0 RID: 6320 RVA: 0x00044992 File Offset: 0x00042B92
			private static void EnsureIndexInt32(uint index, int listCapacity)
			{
				if (2147483647U <= index || index >= (uint)listCapacity)
				{
					ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index", WinRTRuntimeErrorStrings.ArgumentOutOfRange_IndexLargerThanMaxValue);
					ex.SetHResult(-2147483637);
					throw ex;
				}
			}

			// Token: 0x060018B1 RID: 6321 RVA: 0x000449BB File Offset: 0x00042BBB
			public IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x040008B3 RID: 2227
			private IList _list;

			// Token: 0x0200042A RID: 1066
			internal sealed class ListToBindableVectorViewAdapter : IBindableVectorView, IEnumerable
			{
				// Token: 0x06001C49 RID: 7241 RVA: 0x0004B8B2 File Offset: 0x00049AB2
				internal ListToBindableVectorViewAdapter(IList list)
				{
					if (list == null)
					{
						throw new ArgumentNullException("list");
					}
					this.list = list;
				}

				// Token: 0x06001C4A RID: 7242 RVA: 0x0004B8CF File Offset: 0x00049ACF
				private static void EnsureIndexInt32(uint index, int listCapacity)
				{
					if (2147483647U <= index || index >= (uint)listCapacity)
					{
						ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index", WinRTRuntimeErrorStrings.ArgumentOutOfRange_IndexLargerThanMaxValue);
						ex.SetHResult(-2147483637);
						throw ex;
					}
				}

				// Token: 0x06001C4B RID: 7243 RVA: 0x0004B8F8 File Offset: 0x00049AF8
				public IBindableIterator First()
				{
					return IEnumerable.ToAbiHelper.MakeBindableIterator(this.list.GetEnumerator());
				}

				// Token: 0x06001C4C RID: 7244 RVA: 0x0004B90C File Offset: 0x00049B0C
				public object GetAt(uint index)
				{
					IList.ToAbiHelper.ListToBindableVectorViewAdapter.EnsureIndexInt32(index, this.list.Count);
					object result;
					try
					{
						result = this.list[(int)index];
					}
					catch (ArgumentOutOfRangeException innerException)
					{
						throw innerException.GetExceptionForHR(-2147483637, WinRTRuntimeErrorStrings.ArgumentOutOfRange_Index);
					}
					return result;
				}

				// Token: 0x17000529 RID: 1321
				// (get) Token: 0x06001C4D RID: 7245 RVA: 0x0004B95C File Offset: 0x00049B5C
				public uint Size
				{
					get
					{
						return (uint)this.list.Count;
					}
				}

				// Token: 0x06001C4E RID: 7246 RVA: 0x0004B96C File Offset: 0x00049B6C
				public bool IndexOf(object value, out uint index)
				{
					int num = this.list.IndexOf(value);
					if (-1 == num)
					{
						index = 0U;
						return false;
					}
					index = (uint)num;
					return true;
				}

				// Token: 0x06001C4F RID: 7247 RVA: 0x0004B993 File Offset: 0x00049B93
				public IEnumerator GetEnumerator()
				{
					return this.list.GetEnumerator();
				}

				// Token: 0x04000A43 RID: 2627
				private readonly IList list;
			}
		}

		// Token: 0x0200036C RID: 876
		[Guid("393DE7DE-6FD0-4C0D-BB71-47244A113E93")]
		public struct Vftbl
		{
			// Token: 0x170004B0 RID: 1200
			// (get) Token: 0x060018B2 RID: 6322 RVA: 0x000449C8 File Offset: 0x00042BC8
			// (set) Token: 0x060018B3 RID: 6323 RVA: 0x000449D0 File Offset: 0x00042BD0
			public method GetAt_0
			{
				get
				{
					return this._GetAt_0;
				}
				set
				{
					this._GetAt_0 = value;
				}
			}

			// Token: 0x170004B1 RID: 1201
			// (get) Token: 0x060018B4 RID: 6324 RVA: 0x000449D9 File Offset: 0x00042BD9
			// (set) Token: 0x060018B5 RID: 6325 RVA: 0x000449E1 File Offset: 0x00042BE1
			public method get_Size_1
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

			// Token: 0x170004B2 RID: 1202
			// (get) Token: 0x060018B6 RID: 6326 RVA: 0x000449EA File Offset: 0x00042BEA
			// (set) Token: 0x060018B7 RID: 6327 RVA: 0x000449F2 File Offset: 0x00042BF2
			public method GetView_2
			{
				get
				{
					return this._GetView_2;
				}
				set
				{
					this._GetView_2 = value;
				}
			}

			// Token: 0x170004B3 RID: 1203
			// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000449FB File Offset: 0x00042BFB
			// (set) Token: 0x060018B9 RID: 6329 RVA: 0x00044A03 File Offset: 0x00042C03
			public method IndexOf_3
			{
				get
				{
					return this._IndexOf_3;
				}
				set
				{
					this._IndexOf_3 = value;
				}
			}

			// Token: 0x170004B4 RID: 1204
			// (get) Token: 0x060018BA RID: 6330 RVA: 0x00044A0C File Offset: 0x00042C0C
			// (set) Token: 0x060018BB RID: 6331 RVA: 0x00044A14 File Offset: 0x00042C14
			public method SetAt_4
			{
				get
				{
					return this._SetAt_4;
				}
				set
				{
					this._SetAt_4 = value;
				}
			}

			// Token: 0x170004B5 RID: 1205
			// (get) Token: 0x060018BC RID: 6332 RVA: 0x00044A1D File Offset: 0x00042C1D
			// (set) Token: 0x060018BD RID: 6333 RVA: 0x00044A25 File Offset: 0x00042C25
			public method InsertAt_5
			{
				get
				{
					return this._InsertAt_5;
				}
				set
				{
					this._InsertAt_5 = value;
				}
			}

			// Token: 0x170004B6 RID: 1206
			// (get) Token: 0x060018BE RID: 6334 RVA: 0x00044A2E File Offset: 0x00042C2E
			// (set) Token: 0x060018BF RID: 6335 RVA: 0x00044A36 File Offset: 0x00042C36
			public method RemoveAt_6
			{
				get
				{
					return this._RemoveAt_6;
				}
				set
				{
					this._RemoveAt_6 = value;
				}
			}

			// Token: 0x170004B7 RID: 1207
			// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00044A3F File Offset: 0x00042C3F
			// (set) Token: 0x060018C1 RID: 6337 RVA: 0x00044A47 File Offset: 0x00042C47
			public method Append_7
			{
				get
				{
					return this._Append_7;
				}
				set
				{
					this._Append_7 = value;
				}
			}

			// Token: 0x170004B8 RID: 1208
			// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00044A50 File Offset: 0x00042C50
			// (set) Token: 0x060018C3 RID: 6339 RVA: 0x00044A58 File Offset: 0x00042C58
			public method RemoveAtEnd_8
			{
				get
				{
					return this._RemoveAtEnd_8;
				}
				set
				{
					this._RemoveAtEnd_8 = value;
				}
			}

			// Token: 0x170004B9 RID: 1209
			// (get) Token: 0x060018C4 RID: 6340 RVA: 0x00044A61 File Offset: 0x00042C61
			// (set) Token: 0x060018C5 RID: 6341 RVA: 0x00044A69 File Offset: 0x00042C69
			public method Clear_9
			{
				get
				{
					return this._Clear_9;
				}
				set
				{
					this._Clear_9 = value;
				}
			}

			// Token: 0x060018C6 RID: 6342 RVA: 0x00044A74 File Offset: 0x00042C74
			unsafe static Vftbl()
			{
				IList.Vftbl.AbiToProjectionVftable = new IList.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_GetAt_0 = ldftn(Do_Abi_GetAt_0),
					_get_Size_1 = ldftn(Do_Abi_get_Size_1),
					_GetView_2 = ldftn(Do_Abi_GetView_2),
					_IndexOf_3 = ldftn(Do_Abi_IndexOf_3),
					_SetAt_4 = ldftn(Do_Abi_SetAt_4),
					_InsertAt_5 = ldftn(Do_Abi_InsertAt_5),
					_RemoveAt_6 = ldftn(Do_Abi_RemoveAt_6),
					_Append_7 = ldftn(Do_Abi_Append_7),
					_RemoveAtEnd_8 = ldftn(Do_Abi_RemoveAtEnd_8),
					_Clear_9 = ldftn(Do_Abi_Clear_9)
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IList.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 10));
				Marshal.StructureToPtr<IList.Vftbl>(IList.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IList.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060018C7 RID: 6343 RVA: 0x00044B68 File Offset: 0x00042D68
			private static IBindableVector FindAdapter(IntPtr thisPtr)
			{
				IList list2 = ComWrappersSupport.FindObject<IList>(thisPtr);
				return IList.Vftbl._adapterTable.GetValue(list2, ([Nullable(1)] IList list) => new IList.ToAbiHelper(list));
			}

			// Token: 0x060018C8 RID: 6344 RVA: 0x00044BA8 File Offset: 0x00042DA8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetAt_0(IntPtr thisPtr, uint index, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					object at = IList.Vftbl.FindAdapter(thisPtr).GetAt(index);
					*result = MarshalInspectable<object>.FromManaged(at, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018C9 RID: 6345 RVA: 0x00044BF8 File Offset: 0x00042DF8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetView_2(IntPtr thisPtr, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					IBindableVectorView view = IList.Vftbl.FindAdapter(thisPtr).GetView();
					*result = MarshalInterface<IBindableVectorView>.FromManaged(view);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CA RID: 6346 RVA: 0x00044C48 File Offset: 0x00042E48
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_IndexOf_3(IntPtr thisPtr, IntPtr value, uint* index, byte* returnValue)
			{
				*index = 0U;
				*returnValue = 0;
				uint num = 0U;
				try
				{
					bool flag = IList.Vftbl.FindAdapter(thisPtr).IndexOf(MarshalInspectable<object>.FromAbi(value), out num);
					*index = num;
					*returnValue = (flag ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CB RID: 6347 RVA: 0x00044CA4 File Offset: 0x00042EA4
			[UnmanagedCallersOnly]
			private static int Do_Abi_SetAt_4(IntPtr thisPtr, uint index, IntPtr value)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).SetAt(index, MarshalInspectable<object>.FromAbi(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CC RID: 6348 RVA: 0x00044CE8 File Offset: 0x00042EE8
			[UnmanagedCallersOnly]
			private static int Do_Abi_InsertAt_5(IntPtr thisPtr, uint index, IntPtr value)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).InsertAt(index, MarshalInspectable<object>.FromAbi(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CD RID: 6349 RVA: 0x00044D2C File Offset: 0x00042F2C
			[UnmanagedCallersOnly]
			private static int Do_Abi_RemoveAt_6(IntPtr thisPtr, uint index)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).RemoveAt(index);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CE RID: 6350 RVA: 0x00044D68 File Offset: 0x00042F68
			[UnmanagedCallersOnly]
			private static int Do_Abi_Append_7(IntPtr thisPtr, IntPtr value)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).Append(MarshalInspectable<object>.FromAbi(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018CF RID: 6351 RVA: 0x00044DAC File Offset: 0x00042FAC
			[UnmanagedCallersOnly]
			private static int Do_Abi_RemoveAtEnd_8(IntPtr thisPtr)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).RemoveAtEnd();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018D0 RID: 6352 RVA: 0x00044DE8 File Offset: 0x00042FE8
			[UnmanagedCallersOnly]
			private static int Do_Abi_Clear_9(IntPtr thisPtr)
			{
				try
				{
					IList.Vftbl.FindAdapter(thisPtr).Clear();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060018D1 RID: 6353 RVA: 0x00044E24 File Offset: 0x00043024
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Size_1(IntPtr thisPtr, uint* value)
			{
				*value = 0U;
				try
				{
					uint size = IList.Vftbl.FindAdapter(thisPtr).Size;
					*value = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040008B4 RID: 2228
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008B5 RID: 2229
			private unsafe void* _GetAt_0;

			// Token: 0x040008B6 RID: 2230
			private unsafe void* _get_Size_1;

			// Token: 0x040008B7 RID: 2231
			private unsafe void* _GetView_2;

			// Token: 0x040008B8 RID: 2232
			private unsafe void* _IndexOf_3;

			// Token: 0x040008B9 RID: 2233
			private unsafe void* _SetAt_4;

			// Token: 0x040008BA RID: 2234
			private unsafe void* _InsertAt_5;

			// Token: 0x040008BB RID: 2235
			private unsafe void* _RemoveAt_6;

			// Token: 0x040008BC RID: 2236
			private unsafe void* _Append_7;

			// Token: 0x040008BD RID: 2237
			private unsafe void* _RemoveAtEnd_8;

			// Token: 0x040008BE RID: 2238
			private unsafe void* _Clear_9;

			// Token: 0x040008BF RID: 2239
			private static readonly IList.Vftbl AbiToProjectionVftable;

			// Token: 0x040008C0 RID: 2240
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008C1 RID: 2241
			private static ConditionalWeakTable<IList, IList.ToAbiHelper> _adapterTable = new ConditionalWeakTable<IList, IList.ToAbiHelper>();
		}
	}
}
