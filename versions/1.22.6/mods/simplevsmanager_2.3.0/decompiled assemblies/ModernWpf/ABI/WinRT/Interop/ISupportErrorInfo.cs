using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010A RID: 266
	[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
	internal class ISupportErrorInfo : WinRT.Interop.ISupportErrorInfo
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x0001A988 File Offset: 0x00018B88
		public static ObjectReference<ISupportErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ISupportErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001A990 File Offset: 0x00018B90
		public static implicit operator ISupportErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ISupportErrorInfo(obj);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001A99D File Offset: 0x00018B9D
		public static implicit operator ISupportErrorInfo(ObjectReference<ISupportErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ISupportErrorInfo(obj);
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0001A9AA File Offset: 0x00018BAA
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001A9B7 File Offset: 0x00018BB7
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001A9C4 File Offset: 0x00018BC4
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001A9D1 File Offset: 0x00018BD1
		public ISupportErrorInfo(IObjectReference obj) : this(obj.As<ISupportErrorInfo.Vftbl>())
		{
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001A9DF File Offset: 0x00018BDF
		public ISupportErrorInfo(ObjectReference<ISupportErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		public unsafe bool InterfaceSupportsErrorInfo(Guid riid)
		{
			method interfaceSupportsErrorInfo_ = this._obj.Vftbl.InterfaceSupportsErrorInfo_0;
			return calli(System.Int32(System.IntPtr,System.Guid*), this.ThisPtr, &riid, interfaceSupportsErrorInfo_) == 0;
		}

		// Token: 0x040001A8 RID: 424
		protected readonly ObjectReference<ISupportErrorInfo.Vftbl> _obj;

		// Token: 0x02000323 RID: 803
		[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
		public struct Vftbl
		{
			// Token: 0x17000478 RID: 1144
			// (get) Token: 0x06001786 RID: 6022 RVA: 0x0004203C File Offset: 0x0004023C
			// (set) Token: 0x06001787 RID: 6023 RVA: 0x00042044 File Offset: 0x00040244
			public method InterfaceSupportsErrorInfo_0
			{
				get
				{
					return this._InterfaceSupportsErrorInfo_0;
				}
				set
				{
					this._InterfaceSupportsErrorInfo_0 = value;
				}
			}

			// Token: 0x06001788 RID: 6024 RVA: 0x00042050 File Offset: 0x00040250
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<ISupportErrorInfo.Vftbl>()));
				Marshal.StructureToPtr<ISupportErrorInfo.Vftbl>(ISupportErrorInfo.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				ISupportErrorInfo.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001789 RID: 6025 RVA: 0x000420B0 File Offset: 0x000402B0
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_InterfaceSupportsErrorInfo_0(IntPtr thisPtr, Guid* guid)
			{
				int result;
				try
				{
					result = (ComWrappersSupport.FindObject<WinRT.Interop.ISupportErrorInfo>(thisPtr).InterfaceSupportsErrorInfo(*guid) ? 0 : 1);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					result = ExceptionHelpers.GetHRForException(ex);
				}
				return result;
			}

			// Token: 0x04000805 RID: 2053
			internal IUnknownVftbl IUnknownVftbl;

			// Token: 0x04000806 RID: 2054
			private unsafe void* _InterfaceSupportsErrorInfo_0;

			// Token: 0x04000807 RID: 2055
			private static readonly ISupportErrorInfo.Vftbl AbiToProjectionVftable = new ISupportErrorInfo.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				_InterfaceSupportsErrorInfo_0 = ldftn(Do_Abi_InterfaceSupportsErrorInfo_0)
			};

			// Token: 0x04000808 RID: 2056
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
