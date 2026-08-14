using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200011C RID: 284
	[DynamicInterfaceCastableImplementation]
	[EditorBrowsable(1)]
	[Guid("30D5A829-7FA4-4026-83BB-D75BAE4EA99E")]
	internal interface IDisposable : IDisposable
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x0001B964 File Offset: 0x00019B64
		internal static ObjectReference<IDisposable.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IDisposable.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001B96C File Offset: 0x00019B6C
		void Dispose()
		{
			ObjectReference<IDisposable.Vftbl> objectReference = (ObjectReference<IDisposable.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IDisposable).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			method close_ = objectReference.Vftbl.Close_0;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, close_));
		}

		// Token: 0x02000333 RID: 819
		[Guid("30D5A829-7FA4-4026-83BB-D75BAE4EA99E")]
		public struct Vftbl
		{
			// Token: 0x17000487 RID: 1159
			// (get) Token: 0x060017C7 RID: 6087 RVA: 0x00042740 File Offset: 0x00040940
			// (set) Token: 0x060017C8 RID: 6088 RVA: 0x00042748 File Offset: 0x00040948
			public method Close_0
			{
				get
				{
					return this._Close_0;
				}
				set
				{
					this._Close_0 = value;
				}
			}

			// Token: 0x060017C9 RID: 6089 RVA: 0x00042754 File Offset: 0x00040954
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IDisposable.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IDisposable.Vftbl>(IDisposable.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IDisposable.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017CA RID: 6090 RVA: 0x000427C8 File Offset: 0x000409C8
			[UnmanagedCallersOnly]
			private static int Do_Abi_Close_0(IntPtr thisPtr)
			{
				try
				{
					ComWrappersSupport.FindObject<IDisposable>(thisPtr).Dispose();
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000826 RID: 2086
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000827 RID: 2087
			private unsafe void* _Close_0;

			// Token: 0x04000828 RID: 2088
			private static readonly IDisposable.Vftbl AbiToProjectionVftable = new IDisposable.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Close_0 = ldftn(Do_Abi_Close_0)
			};

			// Token: 0x04000829 RID: 2089
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x02000426 RID: 1062
			// (Invoke) Token: 0x06001C3A RID: 7226
			private delegate int CloseDelegate(IntPtr thisPtr);
		}
	}
}
