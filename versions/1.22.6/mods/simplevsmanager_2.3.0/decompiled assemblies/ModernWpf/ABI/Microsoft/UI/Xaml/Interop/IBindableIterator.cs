using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Interop;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000174 RID: 372
	[DynamicInterfaceCastableImplementation]
	[Guid("6A1D6C07-076D-49F2-8314-F52C9C9A8331")]
	internal interface IBindableIterator : Microsoft.UI.Xaml.Interop.IBindableIterator
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x000210D0 File Offset: 0x0001F2D0
		internal static ObjectReference<IBindableIterator.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IBindableIterator.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000210D8 File Offset: 0x0001F2D8
		unsafe bool MoveNext()
		{
			ObjectReference<IBindableIterator.Vftbl> objectReference = (ObjectReference<IBindableIterator.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			byte b = 0;
			method moveNext_ = objectReference.Vftbl.MoveNext_2;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte*), thisPtr, &b, moveNext_));
			return b > 0;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002112E File Offset: 0x0001F32E
		uint GetMany(ref object[] items)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00021138 File Offset: 0x0001F338
		unsafe object Microsoft.UI.Xaml.Interop.IBindableIterator.Current
		{
			get
			{
				ObjectReference<IBindableIterator.Vftbl> objectReference = (ObjectReference<IBindableIterator.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				IntPtr intPtr = default(IntPtr);
				object result;
				try
				{
					method get_Current_ = objectReference.Vftbl.get_Current_0;
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), thisPtr, &intPtr, get_Current_));
					result = MarshalInspectable<object>.FromAbi(intPtr);
				}
				finally
				{
					MarshalInspectable<object>.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x000211B8 File Offset: 0x0001F3B8
		unsafe bool Microsoft.UI.Xaml.Interop.IBindableIterator.HasCurrent
		{
			get
			{
				ObjectReference<IBindableIterator.Vftbl> objectReference = (ObjectReference<IBindableIterator.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Microsoft.UI.Xaml.Interop.IBindableIterator).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				byte b = 0;
				method get_HasCurrent_ = objectReference.Vftbl.get_HasCurrent_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte*), thisPtr, &b, get_HasCurrent_));
				return b > 0;
			}
		}

		// Token: 0x020003B3 RID: 947
		[Guid("6A1D6C07-076D-49F2-8314-F52C9C9A8331")]
		public struct Vftbl
		{
			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00048860 File Offset: 0x00046A60
			// (set) Token: 0x06001A50 RID: 6736 RVA: 0x00048868 File Offset: 0x00046A68
			public method get_Current_0
			{
				get
				{
					return this._get_Current_0;
				}
				set
				{
					this._get_Current_0 = value;
				}
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00048871 File Offset: 0x00046A71
			// (set) Token: 0x06001A52 RID: 6738 RVA: 0x00048879 File Offset: 0x00046A79
			public method get_HasCurrent_1
			{
				get
				{
					return this._get_HasCurrent_1;
				}
				set
				{
					this._get_HasCurrent_1 = value;
				}
			}

			// Token: 0x170004EA RID: 1258
			// (get) Token: 0x06001A53 RID: 6739 RVA: 0x00048882 File Offset: 0x00046A82
			// (set) Token: 0x06001A54 RID: 6740 RVA: 0x0004888A File Offset: 0x00046A8A
			public method MoveNext_2
			{
				get
				{
					return this._MoveNext_2;
				}
				set
				{
					this._MoveNext_2 = value;
				}
			}

			// Token: 0x170004EB RID: 1259
			// (get) Token: 0x06001A55 RID: 6741 RVA: 0x00048893 File Offset: 0x00046A93
			// (set) Token: 0x06001A56 RID: 6742 RVA: 0x0004889B File Offset: 0x00046A9B
			public method GetMany_3
			{
				get
				{
					return this._GetMany_3;
				}
				set
				{
					this._GetMany_3 = value;
				}
			}

			// Token: 0x06001A57 RID: 6743 RVA: 0x000488A4 File Offset: 0x00046AA4
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IBindableIterator.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IBindableIterator.Vftbl>(IBindableIterator.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IBindableIterator.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001A58 RID: 6744 RVA: 0x00048940 File Offset: 0x00046B40
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_MoveNext_2(IntPtr thisPtr, byte* result)
			{
				*result = 0;
				try
				{
					*result = (ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableIterator>(thisPtr).MoveNext() ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A59 RID: 6745 RVA: 0x0004898C File Offset: 0x00046B8C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetMany_3(IntPtr thisPtr, int __itemsSize, IntPtr items, uint* result)
			{
				*result = 0U;
				int hrforException;
				try
				{
					throw new NotImplementedException();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					hrforException = ExceptionHelpers.GetHRForException(ex);
				}
				return hrforException;
			}

			// Token: 0x06001A5A RID: 6746 RVA: 0x000489C4 File Offset: 0x00046BC4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Current_0(IntPtr thisPtr, IntPtr* value)
			{
				*value = default(IntPtr);
				try
				{
					object obj = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableIterator>(thisPtr).Current;
					*value = MarshalInspectable<object>.FromManaged(obj, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A5B RID: 6747 RVA: 0x00048A14 File Offset: 0x00046C14
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_HasCurrent_1(IntPtr thisPtr, byte* value)
			{
				*value = 0;
				try
				{
					*value = (ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Interop.IBindableIterator>(thisPtr).HasCurrent ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000958 RID: 2392
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000959 RID: 2393
			private unsafe void* _get_Current_0;

			// Token: 0x0400095A RID: 2394
			private unsafe void* _get_HasCurrent_1;

			// Token: 0x0400095B RID: 2395
			private unsafe void* _MoveNext_2;

			// Token: 0x0400095C RID: 2396
			private unsafe void* _GetMany_3;

			// Token: 0x0400095D RID: 2397
			private static readonly IBindableIterator.Vftbl AbiToProjectionVftable = new IBindableIterator.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_get_Current_0 = ldftn(Do_Abi_get_Current_0),
				_get_HasCurrent_1 = ldftn(Do_Abi_get_HasCurrent_1),
				_MoveNext_2 = ldftn(Do_Abi_MoveNext_2),
				_GetMany_3 = ldftn(Do_Abi_GetMany_3)
			};

			// Token: 0x0400095E RID: 2398
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
