using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System.Collections;
using Microsoft.UI.Xaml.Interop;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000176 RID: 374
	[DynamicInterfaceCastableImplementation]
	[Guid("346DD6E7-976E-4BC3-815D-ECE243BC0F33")]
	internal interface IBindableVectorView : Microsoft.UI.Xaml.Interop.IBindableVectorView, IEnumerable
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x0002120E File Offset: 0x0001F40E
		internal static ObjectReference<IBindableVectorView.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IBindableVectorView.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00021218 File Offset: 0x0001F418
		unsafe object GetAt(uint index)
		{
			ObjectReference<IBindableVectorView.Vftbl> objectReference = (ObjectReference<IBindableVectorView.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
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

		// Token: 0x06000BEE RID: 3054 RVA: 0x00021298 File Offset: 0x0001F498
		unsafe bool IndexOf(object value, out uint index)
		{
			ObjectReference<IBindableVectorView.Vftbl> objectReference = (ObjectReference<IBindableVectorView.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			uint num = 0U;
			byte b = 0;
			bool result;
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method indexOf_ = objectReference.Vftbl.IndexOf_2;
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

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00021330 File Offset: 0x0001F530
		unsafe uint Microsoft.UI.Xaml.Interop.IBindableVectorView.Size
		{
			get
			{
				ObjectReference<IBindableVectorView.Vftbl> objectReference = (ObjectReference<IBindableVectorView.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				uint result = 0U;
				method get_Size_ = objectReference.Vftbl.get_Size_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32*), thisPtr, &result, get_Size_));
				return result;
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00021383 File Offset: 0x0001F583
		IEnumerator GetEnumerator()
		{
			return IBindableVectorView._helperTable.GetValue((IWinRTObject)this, ([Nullable(1)] IWinRTObject enumerable) => new IEnumerable.FromAbiHelper((IEnumerable)enumerable)).GetEnumerator();
		}

		// Token: 0x04000207 RID: 519
		private static ConditionalWeakTable<IWinRTObject, IEnumerable.FromAbiHelper> _helperTable = new ConditionalWeakTable<IWinRTObject, IEnumerable.FromAbiHelper>();

		// Token: 0x020003B8 RID: 952
		[Guid("346DD6E7-976E-4BC3-815D-ECE243BC0F33")]
		public struct Vftbl
		{
			// Token: 0x170004EC RID: 1260
			// (get) Token: 0x06001A6C RID: 6764 RVA: 0x00048A60 File Offset: 0x00046C60
			// (set) Token: 0x06001A6D RID: 6765 RVA: 0x00048A68 File Offset: 0x00046C68
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

			// Token: 0x170004ED RID: 1261
			// (get) Token: 0x06001A6E RID: 6766 RVA: 0x00048A71 File Offset: 0x00046C71
			// (set) Token: 0x06001A6F RID: 6767 RVA: 0x00048A79 File Offset: 0x00046C79
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

			// Token: 0x170004EE RID: 1262
			// (get) Token: 0x06001A70 RID: 6768 RVA: 0x00048A82 File Offset: 0x00046C82
			// (set) Token: 0x06001A71 RID: 6769 RVA: 0x00048A8A File Offset: 0x00046C8A
			public method IndexOf_2
			{
				get
				{
					return this._IndexOf_2;
				}
				set
				{
					this._IndexOf_2 = value;
				}
			}

			// Token: 0x06001A72 RID: 6770 RVA: 0x00048A94 File Offset: 0x00046C94
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IBindableVectorView.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 3));
				Marshal.StructureToPtr<IBindableVectorView.Vftbl>(IBindableVectorView.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IBindableVectorView.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001A73 RID: 6771 RVA: 0x00048B24 File Offset: 0x00046D24
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetAt_0(IntPtr thisPtr, uint index, IntPtr* result)
			{
				try
				{
					object at = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableVectorView>(thisPtr).GetAt(index);
					*result = MarshalInspectable<object>.FromManaged(at, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A74 RID: 6772 RVA: 0x00048B6C File Offset: 0x00046D6C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_IndexOf_2(IntPtr thisPtr, IntPtr value, uint* index, byte* returnValue)
			{
				*index = 0U;
				*returnValue = 0;
				uint num = 0U;
				try
				{
					bool flag = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableVectorView>(thisPtr).IndexOf(MarshalInspectable<object>.FromAbi(value), out num);
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

			// Token: 0x06001A75 RID: 6773 RVA: 0x00048BC8 File Offset: 0x00046DC8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Size_1(IntPtr thisPtr, uint* value)
			{
				*value = 0U;
				try
				{
					uint size = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableVectorView>(thisPtr).Size;
					*value = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400095F RID: 2399
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000960 RID: 2400
			private unsafe void* _GetAt_0;

			// Token: 0x04000961 RID: 2401
			private unsafe void* _get_Size_1;

			// Token: 0x04000962 RID: 2402
			private unsafe void* _IndexOf_2;

			// Token: 0x04000963 RID: 2403
			private static readonly IBindableVectorView.Vftbl AbiToProjectionVftable = new IBindableVectorView.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_GetAt_0 = ldftn(Do_Abi_GetAt_0),
				_get_Size_1 = ldftn(Do_Abi_get_Size_1),
				_IndexOf_2 = ldftn(Do_Abi_IndexOf_2)
			};

			// Token: 0x04000964 RID: 2404
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
