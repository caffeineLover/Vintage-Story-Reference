using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x0200015B RID: 347
	[DynamicInterfaceCastableImplementation]
	[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
	internal interface IEnumerable<T> : IEnumerable<!0>, IEnumerable, IIterable<T>
	{
		// Token: 0x06000B19 RID: 2841 RVA: 0x0001EE18 File Offset: 0x0001D018
		public static IObjectReference CreateMarshaler(IEnumerable<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IEnumerable<T>.Vftbl>(obj, IEnumerable<T>.PIID);
			}
			return null;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0001EE2A File Offset: 0x0001D02A
		public static ObjectReferenceValue CreateMarshaler2(IEnumerable<T> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IEnumerable<T>.PIID);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0001EE37 File Offset: 0x0001D037
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0001EE48 File Offset: 0x0001D048
		public static IntPtr FromManaged(IEnumerable<T> value)
		{
			if (value != null)
			{
				return IEnumerable<T>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0001EE6C File Offset: 0x0001D06C
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0001EE77 File Offset: 0x0001D077
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IIterable<T>>.DisposeAbi(abi);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0001EE7F File Offset: 0x0001D07F
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IEnumerable<T>));
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0001EE90 File Offset: 0x0001D090
		public static ObjectReference<IEnumerable<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IEnumerable<T>.Vftbl vftbl = new IEnumerable<T>.Vftbl(thisPtr);
			return ObjectReference<IEnumerable<T>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0001EEBB File Offset: 0x0001D0BB
		IEnumerator<T> GetEnumerator()
		{
			return IEnumerableMethods<T>.GetEnumerator((ObjectReference<IEnumerable<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<!0>).TypeHandle));
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0001EEE1 File Offset: 0x0001D0E1
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0001EEE9 File Offset: 0x0001D0E9
		static IEnumerable()
		{
			IEnumerable<T>.PIID = IEnumerable<T>.Vftbl.PIID;
		}

		// Token: 0x040001EE RID: 494
		public static Guid PIID;

		// Token: 0x02000386 RID: 902
		internal sealed class ToAbiHelper : IIterable<T>
		{
			// Token: 0x06001954 RID: 6484 RVA: 0x00045D97 File Offset: 0x00043F97
			internal ToAbiHelper(IEnumerable<T> enumerable)
			{
				this.m_enumerable = enumerable;
			}

			// Token: 0x06001955 RID: 6485 RVA: 0x00045DA6 File Offset: 0x00043FA6
			public IEnumerator<T> First()
			{
				return this.m_enumerable.GetEnumerator();
			}

			// Token: 0x040008E8 RID: 2280
			private readonly IEnumerable<T> m_enumerable;
		}

		// Token: 0x02000387 RID: 903
		[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
		public struct Vftbl
		{
			// Token: 0x170004CB RID: 1227
			// (get) Token: 0x06001956 RID: 6486 RVA: 0x00045DB3 File Offset: 0x00043FB3
			// (set) Token: 0x06001957 RID: 6487 RVA: 0x00045DBB File Offset: 0x00043FBB
			public method First_0
			{
				get
				{
					return this._first_0;
				}
				set
				{
					this._first_0 = value;
				}
			}

			// Token: 0x06001958 RID: 6488 RVA: 0x00045DC4 File Offset: 0x00043FC4
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IEnumerable<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.First_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x06001959 RID: 6489 RVA: 0x00045E14 File Offset: 0x00044014
			unsafe static Vftbl()
			{
				IEnumerable<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IEnumerable<T>));
				IEnumerable<T>.Vftbl.AbiToProjectionVftable = new IEnumerable<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_first_0 = (void*)Marshal.GetFunctionPointerForDelegate(IEnumerable<T>.Vftbl.DelegateCache = new IEnumerable_Delegates.First_0(IEnumerable<T>.Vftbl.Do_Abi_First_0))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IEnumerable<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IEnumerable<T>.Vftbl.AbiToProjectionVftable.First_0;
				IEnumerable<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600195A RID: 6490 RVA: 0x00045EC8 File Offset: 0x000440C8
			private static int Do_Abi_First_0(IntPtr thisPtr, out IntPtr __return_value__)
			{
				__return_value__ = default(IntPtr);
				try
				{
					IEnumerable<T> enumerable = ComWrappersSupport.FindObject<IEnumerable<T>>(thisPtr);
					__return_value__ = MarshalInterface<IEnumerator<T>>.FromManaged(enumerable.GetEnumerator());
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040008E9 RID: 2281
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008EA RID: 2282
			private unsafe void* _first_0;

			// Token: 0x040008EB RID: 2283
			public static Guid PIID;

			// Token: 0x040008EC RID: 2284
			private static readonly IEnumerable<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x040008ED RID: 2285
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008EE RID: 2286
			private static readonly Delegate DelegateCache;
		}
	}
}
