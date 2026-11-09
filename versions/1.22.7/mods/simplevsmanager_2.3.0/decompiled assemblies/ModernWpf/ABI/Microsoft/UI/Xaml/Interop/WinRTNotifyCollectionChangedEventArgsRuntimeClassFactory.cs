using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000179 RID: 377
	[ObjectReferenceWrapper("_obj")]
	[Guid("5108EBA4-4892-5A20-8374-A96815E0FD27")]
	internal sealed class WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x00021590 File Offset: 0x0001F790
		public static ObjectReference<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00021598 File Offset: 0x0001F798
		public static implicit operator WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000215A5 File Offset: 0x0001F7A5
		public static implicit operator WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x000215B2 File Offset: 0x0001F7B2
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000215BF File Offset: 0x0001F7BF
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000215CC File Offset: 0x0001F7CC
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000215D9 File Offset: 0x0001F7D9
		public WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(IObjectReference obj) : this(obj.As<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl>())
		{
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000215E7 File Offset: 0x0001F7E7
		public WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000215F8 File Offset: 0x0001F7F8
		public IObjectReference CreateInstanceWithAllParameters(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex, object baseInterface, out IObjectReference innerInterface)
		{
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
			ObjectReferenceValue objectReferenceValue3 = default(ObjectReferenceValue);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			IObjectReference result;
			try
			{
				objectReferenceValue = MarshalInterface<IList>.CreateMarshaler2(newItems, default(Guid));
				objectReferenceValue2 = MarshalInterface<IList>.CreateMarshaler2(oldItems, default(Guid));
				objectReferenceValue3 = MarshalInspectable<object>.CreateMarshaler2(baseInterface, true);
				method createInstanceWithAllParameters_ = this._obj.Vftbl.CreateInstanceWithAllParameters_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Collections.Specialized.NotifyCollectionChangedAction,System.IntPtr,System.IntPtr,System.Int32,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, action, MarshalInterface<IList>.GetAbi(objectReferenceValue), MarshalInterface<IList>.GetAbi(objectReferenceValue2), newIndex, oldIndex, MarshalInspectable<object>.GetAbi(objectReferenceValue3), ref intPtr, ref intPtr2, createInstanceWithAllParameters_));
				innerInterface = ObjectReference<IUnknownVftbl>.FromAbi(intPtr);
				result = ObjectReference<IUnknownVftbl>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalInterface<IList>.DisposeMarshaler(objectReferenceValue);
				MarshalInterface<IList>.DisposeMarshaler(objectReferenceValue2);
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue3);
				MarshalInspectable<object>.DisposeAbi(intPtr);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000216E4 File Offset: 0x0001F8E4
		public ObjectReferenceValue CreateInstanceWithAllParameters(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
		{
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
			IntPtr intPtr = default(IntPtr);
			IntPtr ptr = default(IntPtr);
			ObjectReferenceValue result;
			try
			{
				objectReferenceValue = MarshalInterface<IList>.CreateMarshaler2(newItems, default(Guid));
				objectReferenceValue2 = MarshalInterface<IList>.CreateMarshaler2(oldItems, default(Guid));
				method createInstanceWithAllParameters_ = this._obj.Vftbl.CreateInstanceWithAllParameters_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Collections.Specialized.NotifyCollectionChangedAction,System.IntPtr,System.IntPtr,System.Int32,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, action, MarshalInterface<IList>.GetAbi(objectReferenceValue), MarshalInterface<IList>.GetAbi(objectReferenceValue2), newIndex, oldIndex, IntPtr.Zero, ref intPtr, ref ptr, createInstanceWithAllParameters_));
				result = new ObjectReferenceValue(ptr);
			}
			finally
			{
				objectReferenceValue.Dispose();
				objectReferenceValue2.Dispose();
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x04000209 RID: 521
		private readonly ObjectReference<WinRTNotifyCollectionChangedEventArgsRuntimeClassFactory.Vftbl> _obj;

		// Token: 0x020003BE RID: 958
		[Guid("5108EBA4-4892-5A20-8374-A96815E0FD27")]
		public struct Vftbl
		{
			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x06001A8A RID: 6794 RVA: 0x00048C55 File Offset: 0x00046E55
			public method CreateInstanceWithAllParameters_0
			{
				get
				{
					return this._CreateInstanceWithAllParameters_0;
				}
			}

			// Token: 0x0400096D RID: 2413
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400096E RID: 2414
			private unsafe void* _CreateInstanceWithAllParameters_0;
		}
	}
}
