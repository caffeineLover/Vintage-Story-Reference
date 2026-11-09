using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000193 RID: 403
	[ObjectReferenceWrapper("_obj")]
	[Guid("AF86E2E0-B12D-4c6a-9C5A-D7AA65101E90")]
	internal class IInspectable : IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x06000CAF RID: 3247 RVA: 0x00024960 File Offset: 0x00022B60
		public static IInspectable FromAbi(IntPtr thisPtr)
		{
			return new IInspectable(ObjectReference<IInspectable.Vftbl>.FromAbi(thisPtr));
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0002496D File Offset: 0x00022B6D
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002497A File Offset: 0x00022B7A
		public static implicit operator IInspectable(IObjectReference obj)
		{
			return obj.As<IInspectable.Vftbl>();
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00024987 File Offset: 0x00022B87
		public static implicit operator IInspectable(ObjectReference<IInspectable.Vftbl> obj)
		{
			return new IInspectable(obj);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002498F File Offset: 0x00022B8F
		public ObjectReference<I> As<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0002499C File Offset: 0x00022B9C
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x000249A4 File Offset: 0x00022BA4
		public IInspectable(IObjectReference obj) : this(obj.As<IInspectable.Vftbl>())
		{
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000249B2 File Offset: 0x00022BB2
		public IInspectable(ObjectReference<IInspectable.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x000249C4 File Offset: 0x00022BC4
		public unsafe string GetRuntimeClassName(bool noThrow = false)
		{
			IntPtr hstring = default(IntPtr);
			string result;
			try
			{
				method getRuntimeClassName = this._obj.Vftbl.GetRuntimeClassName;
				int num = calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &hstring, getRuntimeClassName);
				if (num != 0)
				{
					if (noThrow)
					{
						return null;
					}
					Marshal.ThrowExceptionForHR(num);
				}
				uint num2;
				result = new string(Platform.WindowsGetStringRawBuffer(hstring, &num2), 0, (int)num2);
			}
			finally
			{
				Platform.WindowsDeleteString(hstring);
			}
			return result;
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00024A40 File Offset: 0x00022C40
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00024A48 File Offset: 0x00022C48
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00024A4B File Offset: 0x00022C4B
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00024A67 File Offset: 0x00022C67
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00024A7B File Offset: 0x00022C7B
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00024A97 File Offset: 0x00022C97
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x04000239 RID: 569
		private readonly ObjectReference<IInspectable.Vftbl> _obj;

		// Token: 0x0400023A RID: 570
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x0400023B RID: 571
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020003CF RID: 975
		[Guid("AF86E2E0-B12D-4c6a-9C5A-D7AA65101E90")]
		public struct Vftbl
		{
			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x06001ABA RID: 6842 RVA: 0x00049060 File Offset: 0x00047260
			// (set) Token: 0x06001ABB RID: 6843 RVA: 0x00049068 File Offset: 0x00047268
			public method GetIids
			{
				get
				{
					return this._GetIids;
				}
				set
				{
					this._GetIids = value;
				}
			}

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x06001ABC RID: 6844 RVA: 0x00049071 File Offset: 0x00047271
			// (set) Token: 0x06001ABD RID: 6845 RVA: 0x00049079 File Offset: 0x00047279
			public method GetRuntimeClassName
			{
				get
				{
					return this._GetRuntimeClassName;
				}
				set
				{
					this._GetRuntimeClassName = value;
				}
			}

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00049082 File Offset: 0x00047282
			// (set) Token: 0x06001ABF RID: 6847 RVA: 0x0004908A File Offset: 0x0004728A
			public method GetTrustLevel
			{
				get
				{
					return this._GetTrustLevel;
				}
				set
				{
					this._GetTrustLevel = value;
				}
			}

			// Token: 0x06001AC0 RID: 6848 RVA: 0x00049094 File Offset: 0x00047294
			static Vftbl()
			{
				Marshal.StructureToPtr<IInspectable.Vftbl>(IInspectable.Vftbl.AbiToProjectionVftable, IInspectable.Vftbl.AbiToProjectionVftablePtr, false);
			}

			// Token: 0x06001AC1 RID: 6849 RVA: 0x00049104 File Offset: 0x00047304
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetIids(IntPtr pThis, int* iidCount, IntPtr* iids)
			{
				*iidCount = 0;
				*iids = IntPtr.Zero;
				try
				{
					ref IntPtr ptr = ref *iids;
					ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<Guid>.FromManagedArray(ComWrappersSupport.GetInspectableInfo(pThis).IIDs);
					*iidCount = valueTuple.Item1;
					ptr = valueTuple.Item2;
				}
				catch (Exception ex)
				{
					return ex.HResult;
				}
				return 0;
			}

			// Token: 0x06001AC2 RID: 6850 RVA: 0x0004915C File Offset: 0x0004735C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetRuntimeClassName(IntPtr pThis, IntPtr* className)
			{
				*className = default(IntPtr);
				try
				{
					string runtimeClassName = ComWrappersSupport.GetInspectableInfo(pThis).RuntimeClassName;
					*className = MarshalString.FromManaged(runtimeClassName);
				}
				catch (Exception ex)
				{
					return ex.HResult;
				}
				return 0;
			}

			// Token: 0x06001AC3 RID: 6851 RVA: 0x000491A4 File Offset: 0x000473A4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetTrustLevel(IntPtr pThis, TrustLevel* trustLevel)
			{
				*trustLevel = TrustLevel.BaseTrust;
				return 0;
			}

			// Token: 0x0400098E RID: 2446
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x0400098F RID: 2447
			private unsafe void* _GetIids;

			// Token: 0x04000990 RID: 2448
			private unsafe void* _GetRuntimeClassName;

			// Token: 0x04000991 RID: 2449
			private unsafe void* _GetTrustLevel;

			// Token: 0x04000992 RID: 2450
			public static readonly IInspectable.Vftbl AbiToProjectionVftable = new IInspectable.Vftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				_GetIids = ldftn(Do_Abi_GetIids),
				_GetRuntimeClassName = ldftn(Do_Abi_GetRuntimeClassName),
				_GetTrustLevel = ldftn(Do_Abi_GetTrustLevel)
			};

			// Token: 0x04000993 RID: 2451
			public static readonly IntPtr AbiToProjectionVftablePtr = Marshal.AllocHGlobal(Marshal.SizeOf<IInspectable.Vftbl>());
		}
	}
}
