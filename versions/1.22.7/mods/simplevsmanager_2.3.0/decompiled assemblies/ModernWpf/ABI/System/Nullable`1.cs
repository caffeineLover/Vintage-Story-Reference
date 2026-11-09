using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.System
{
	// Token: 0x0200011F RID: 287
	[ObjectReferenceWrapper("_obj")]
	[Guid("61C17706-2D65-11E0-9AE8-D48564015472")]
	internal class Nullable<T>
	{
		// Token: 0x060009DE RID: 2526 RVA: 0x0001BA58 File Offset: 0x00019C58
		public static IObjectReference CreateMarshaler(object value)
		{
			if (value != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IUnknownVftbl>(value, Nullable<T>.PIID);
			}
			return null;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001BA6A File Offset: 0x00019C6A
		public static ObjectReferenceValue CreateMarshaler2(object value)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(value, Nullable<T>.PIID);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001BA77 File Offset: 0x00019C77
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001BA88 File Offset: 0x00019C88
		public static object FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			Nullable<T>.Vftbl vftbl = new Nullable<T>.Vftbl(ptr);
			return new Nullable<T>(ObjectReference<Nullable<T>.Vftbl>.FromAbi(ptr, vftbl)).Value;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001BAC4 File Offset: 0x00019CC4
		public unsafe static void CopyManaged(object o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = Nullable<T>.CreateMarshaler2(o).Detach();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public static IntPtr FromManaged(object value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return Nullable<T>.CreateMarshaler2(value).Detach();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001BB0C File Offset: 0x00019D0C
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001BB17 File Offset: 0x00019D17
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001BB1F File Offset: 0x00019D1F
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(Nullable<T>));
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0001BB30 File Offset: 0x00019D30
		public static implicit operator Nullable<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new Nullable<T>(obj);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001BB3D File Offset: 0x00019D3D
		public static implicit operator Nullable<T>(ObjectReference<Nullable<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new Nullable<T>(obj);
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0001BB4A File Offset: 0x00019D4A
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001BB57 File Offset: 0x00019D57
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001BB64 File Offset: 0x00019D64
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001BB71 File Offset: 0x00019D71
		public Nullable(IObjectReference obj) : this(obj.As<Nullable<T>.Vftbl>(Nullable<T>.Vftbl.PIID))
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0001BB84 File Offset: 0x00019D84
		public Nullable(ObjectReference<Nullable<T>.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0001BB94 File Offset: 0x00019D94
		internal unsafe static T GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			object[] array = new object[2];
			array[0] = IntPtr.Zero;
			object[] array2 = array;
			T result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable<T>.PIID, ref zero));
				array2[0] = zero;
				Marshal.GetDelegateForFunctionPointer(*(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(IntPtr)), Nullable<T>.Vftbl.get_Value_0_Type).DynamicInvokeAbi(array2);
				result = Marshaler<T>.FromAbi.Invoke(array2[1]);
			}
			finally
			{
				Marshaler<T>.DisposeAbi.Invoke(array2[1]);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0001BC34 File Offset: 0x00019E34
		public T Value
		{
			get
			{
				object[] array = new object[2];
				array[0] = this.ThisPtr;
				object[] array2 = array;
				T result;
				try
				{
					this._obj.Vftbl.get_Value_0.DynamicInvokeAbi(array2);
					result = Marshaler<T>.FromAbi.Invoke(array2[1]);
				}
				finally
				{
					Marshaler<T>.DisposeAbi.Invoke(array2[1]);
				}
				return result;
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0001BC9C File Offset: 0x00019E9C
		// Note: this type is marked as 'beforefieldinit'.
		static Nullable()
		{
			Nullable<T>.PIID = Nullable<T>.Vftbl.PIID;
		}

		// Token: 0x040001C3 RID: 451
		public static Guid PIID;

		// Token: 0x040001C4 RID: 452
		protected readonly ObjectReference<Nullable<T>.Vftbl> _obj;

		// Token: 0x02000336 RID: 822
		[Guid("61C17706-2D65-11E0-9AE8-D48564015472")]
		public struct Vftbl
		{
			// Token: 0x060017D1 RID: 6097 RVA: 0x000428B8 File Offset: 0x00040AB8
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.get_Value_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], Nullable<T>.Vftbl.get_Value_0_Type);
			}

			// Token: 0x060017D2 RID: 6098 RVA: 0x00042908 File Offset: 0x00040B08
			// Note: this type is marked as 'beforefieldinit'.
			unsafe static Vftbl()
			{
				Nullable<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(Nullable<T>));
				Nullable<T>.Vftbl.get_Value_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<T>.AbiType.MakeByRefType(),
					typeof(int)
				});
			}

			// Token: 0x0400082D RID: 2093
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400082E RID: 2094
			public Delegate get_Value_0;

			// Token: 0x0400082F RID: 2095
			public static Guid PIID;

			// Token: 0x04000830 RID: 2096
			public static readonly Type get_Value_0_Type;
		}
	}
}
