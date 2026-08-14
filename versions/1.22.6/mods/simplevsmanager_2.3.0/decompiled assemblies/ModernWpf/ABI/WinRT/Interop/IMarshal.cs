using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000112 RID: 274
	[Guid("00000003-0000-0000-c000-000000000046")]
	internal sealed class IMarshal
	{
		// Token: 0x0600099B RID: 2459
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		private static extern int CoCreateFreeThreadedMarshaler(IntPtr outer, out IntPtr marshalerPtr);

		// Token: 0x0600099C RID: 2460 RVA: 0x0001B097 File Offset: 0x00019297
		internal static ObjectReference<IMarshal.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IMarshal.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001B09F File Offset: 0x0001929F
		public static implicit operator IMarshal(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IMarshal(obj);
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001B0AC File Offset: 0x000192AC
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0001B0B4 File Offset: 0x000192B4
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001B0C1 File Offset: 0x000192C1
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001B0CE File Offset: 0x000192CE
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001B0DB File Offset: 0x000192DB
		public IMarshal(IObjectReference obj) : this(obj.As<IMarshal.Vftbl>(IMarshal.IID))
		{
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001B0EE File Offset: 0x000192EE
		internal IMarshal(ObjectReference<IMarshal.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001B100 File Offset: 0x00019300
		public unsafe void GetUnmarshalClass(Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlFlags, Guid* pCid)
		{
			method getUnmarshalClass_ = this._obj.Vftbl.GetUnmarshalClass_0;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr,WinRT.Interop.MSHCTX,System.IntPtr,WinRT.Interop.MSHLFLAGS,System.Guid*), this.ThisPtr, riid, pv, dwDestContext, pvDestContext, mshlFlags, pCid, getUnmarshalClass_));
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001B138 File Offset: 0x00019338
		public unsafe void GetMarshalSizeMax(Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags, uint* pSize)
		{
			method getMarshalSizeMax_ = this._obj.Vftbl.GetMarshalSizeMax_1;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr,WinRT.Interop.MSHCTX,System.IntPtr,WinRT.Interop.MSHLFLAGS,System.UInt32*), this.ThisPtr, riid, pv, dwDestContext, pvDestContext, mshlflags, pSize, getMarshalSizeMax_));
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001B170 File Offset: 0x00019370
		public unsafe void MarshalInterface(IntPtr pStm, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags)
		{
			method marshalInterface_ = this._obj.Vftbl.MarshalInterface_2;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Guid*,System.IntPtr,WinRT.Interop.MSHCTX,System.IntPtr,WinRT.Interop.MSHLFLAGS), this.ThisPtr, pStm, riid, pv, dwDestContext, pvDestContext, mshlflags, marshalInterface_));
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001B1A8 File Offset: 0x000193A8
		public unsafe void UnmarshalInterface(IntPtr pStm, Guid* riid, IntPtr* ppv)
		{
			method unmarshalInterface_ = this._obj.Vftbl.UnmarshalInterface_3;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Guid*,System.IntPtr*), this.ThisPtr, pStm, riid, ppv, unmarshalInterface_));
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001B1DC File Offset: 0x000193DC
		public void ReleaseMarshalData(IntPtr pStm)
		{
			method releaseMarshalData_ = this._obj.Vftbl.ReleaseMarshalData_4;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), this.ThisPtr, pStm, releaseMarshalData_));
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001B20C File Offset: 0x0001940C
		public void DisconnectObject(uint dwReserved)
		{
			method disconnectObject_ = this._obj.Vftbl.DisconnectObject_5;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32), this.ThisPtr, dwReserved, disconnectObject_));
		}

		// Token: 0x040001B1 RID: 433
		internal static readonly Guid IID = new Guid(3, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x040001B2 RID: 434
		private static readonly string NotImplemented_NativeRoutineNotFound = "A native library routine was not found: {0}.";

		// Token: 0x040001B3 RID: 435
		internal static readonly Lazy<Guid> IID_InProcFreeThreadedMarshaler = new Lazy<Guid>(new Func<Guid>(IMarshal.Vftbl.GetInProcFreeThreadedMarshalerIID));

		// Token: 0x040001B4 RID: 436
		private readonly ObjectReference<IMarshal.Vftbl> _obj;

		// Token: 0x02000329 RID: 809
		[Guid("00000003-0000-0000-c000-000000000046")]
		public struct Vftbl
		{
			// Token: 0x06001793 RID: 6035 RVA: 0x000421E4 File Offset: 0x000403E4
			unsafe static Vftbl()
			{
				*(IMarshal.Vftbl*)((void*)IMarshal.Vftbl.AbiToProjectionVftablePtr) = new IMarshal.Vftbl
				{
					IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
					GetUnmarshalClass_0 = ldftn(Do_Abi_GetUnmarshalClass_0),
					GetMarshalSizeMax_1 = ldftn(Do_Abi_GetMarshalSizeMax_1),
					MarshalInterface_2 = ldftn(Do_Abi_MarshalInterface_2),
					UnmarshalInterface_3 = ldftn(Do_Abi_UnmarshalInterface_3),
					ReleaseMarshalData_4 = ldftn(Do_Abi_ReleaseMarshalData_4),
					DisconnectObject_5 = ldftn(Do_Abi_DisconnectObject_5)
				};
			}

			// Token: 0x06001794 RID: 6036 RVA: 0x00042288 File Offset: 0x00040488
			private static void EnsureHasFreeThreadedMarshaler()
			{
				if (IMarshal.Vftbl.t_freeThreadedMarshaler != null)
				{
					return;
				}
				try
				{
					IntPtr intPtr;
					Marshal.ThrowExceptionForHR(IMarshal.CoCreateFreeThreadedMarshaler(IntPtr.Zero, out intPtr));
					using (ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.Attach(ref intPtr))
					{
						IMarshal.Vftbl.t_freeThreadedMarshaler = new IMarshal(objectReference);
					}
				}
				catch (DllNotFoundException ex)
				{
					throw new NotImplementedException(string.Format(IMarshal.NotImplemented_NativeRoutineNotFound, "CoCreateFreeThreadedMarshaler"), ex);
				}
			}

			// Token: 0x06001795 RID: 6037 RVA: 0x00042304 File Offset: 0x00040504
			internal unsafe static Guid GetInProcFreeThreadedMarshalerIID()
			{
				IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
				Guid iid = IUnknownVftbl.IID;
				Guid result;
				IMarshal.Vftbl.t_freeThreadedMarshaler.GetUnmarshalClass(&iid, IntPtr.Zero, MSHCTX.InProc, IntPtr.Zero, MSHLFLAGS.Normal, &result);
				return result;
			}

			// Token: 0x06001796 RID: 6038 RVA: 0x0004233C File Offset: 0x0004053C
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_GetUnmarshalClass_0(IntPtr thisPtr, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlFlags, Guid* pCid)
			{
				*pCid = default(Guid);
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.GetUnmarshalClass(riid, pv, dwDestContext, pvDestContext, mshlFlags, pCid);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001797 RID: 6039 RVA: 0x00042388 File Offset: 0x00040588
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_GetMarshalSizeMax_1(IntPtr thisPtr, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags, uint* pSize)
			{
				*pSize = 0U;
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.GetMarshalSizeMax(riid, pv, dwDestContext, pvDestContext, mshlflags, pSize);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001798 RID: 6040 RVA: 0x000423D0 File Offset: 0x000405D0
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_MarshalInterface_2(IntPtr thisPtr, IntPtr pStm, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags)
			{
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.MarshalInterface(pStm, riid, pv, dwDestContext, pvDestContext, mshlflags);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001799 RID: 6041 RVA: 0x00042414 File Offset: 0x00040614
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_UnmarshalInterface_3(IntPtr thisPtr, IntPtr pStm, Guid* riid, IntPtr* ppv)
			{
				*ppv = default(IntPtr);
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.UnmarshalInterface(pStm, riid, ppv);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600179A RID: 6042 RVA: 0x00042458 File Offset: 0x00040658
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private static int Do_Abi_ReleaseMarshalData_4(IntPtr thisPtr, IntPtr pStm)
			{
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.ReleaseMarshalData(pStm);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600179B RID: 6043 RVA: 0x00042494 File Offset: 0x00040694
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private static int Do_Abi_DisconnectObject_5(IntPtr thisPtr, uint dwReserved)
			{
				try
				{
					IMarshal.Vftbl.EnsureHasFreeThreadedMarshaler();
					IMarshal.Vftbl.t_freeThreadedMarshaler.DisconnectObject(dwReserved);
				}
				catch (Exception ex)
				{
					return Marshal.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000817 RID: 2071
			internal IUnknownVftbl IUnknownVftbl;

			// Token: 0x04000818 RID: 2072
			public method GetUnmarshalClass_0;

			// Token: 0x04000819 RID: 2073
			public method GetMarshalSizeMax_1;

			// Token: 0x0400081A RID: 2074
			public method MarshalInterface_2;

			// Token: 0x0400081B RID: 2075
			public method UnmarshalInterface_3;

			// Token: 0x0400081C RID: 2076
			public method ReleaseMarshalData_4;

			// Token: 0x0400081D RID: 2077
			public method DisconnectObject_5;

			// Token: 0x0400081E RID: 2078
			public static readonly IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IMarshal.Vftbl), Marshal.SizeOf<IUnknownVftbl>() + sizeof(IntPtr) * 6);

			// Token: 0x0400081F RID: 2079
			[ThreadStatic]
			private static IMarshal t_freeThreadedMarshaler;
		}
	}
}
