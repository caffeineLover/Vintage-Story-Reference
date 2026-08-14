using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000110 RID: 272
	[Guid("00000146-0000-0000-C000-000000000046")]
	internal sealed class IGlobalInterfaceTable : WinRT.Interop.IGlobalInterfaceTable
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x0001AE62 File Offset: 0x00019062
		public static ObjectReference<IGlobalInterfaceTable.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IGlobalInterfaceTable.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001AE6A File Offset: 0x0001906A
		public static implicit operator IGlobalInterfaceTable(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IGlobalInterfaceTable(obj);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001AE77 File Offset: 0x00019077
		public static implicit operator IGlobalInterfaceTable(ObjectReference<IGlobalInterfaceTable.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IGlobalInterfaceTable(obj);
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001AE84 File Offset: 0x00019084
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001AE91 File Offset: 0x00019091
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0001AE9E File Offset: 0x0001909E
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0001AEAB File Offset: 0x000190AB
		public IGlobalInterfaceTable(IObjectReference obj) : this(obj.As<IGlobalInterfaceTable.Vftbl>())
		{
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001AEB9 File Offset: 0x000190B9
		public IGlobalInterfaceTable(ObjectReference<IGlobalInterfaceTable.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0001AEC8 File Offset: 0x000190C8
		public IntPtr RegisterInterfaceInGlobal(IntPtr ptr, Guid riid)
		{
			method registerInterfaceInGlobal = this._obj.Vftbl.RegisterInterfaceInGlobal;
			IntPtr result;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ptr, ref riid, ref result, registerInterfaceInGlobal));
			return result;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0001AF00 File Offset: 0x00019100
		public void RevokeInterfaceFromGlobal(IntPtr cookie)
		{
			method revokeInterfaceFromGlobal = this._obj.Vftbl.RevokeInterfaceFromGlobal;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), this.ThisPtr, cookie, revokeInterfaceFromGlobal));
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001AF34 File Offset: 0x00019134
		public IObjectReference GetInterfaceFromGlobal(IntPtr cookie, Guid riid)
		{
			method getInterfaceFromGlobal = this._obj.Vftbl.GetInterfaceFromGlobal;
			IntPtr intPtr;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, cookie, ref riid, ref intPtr, getInterfaceFromGlobal));
			IObjectReference objectReferenceForInterface;
			try
			{
				objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return objectReferenceForInterface;
		}

		// Token: 0x040001AD RID: 429
		internal static readonly Guid IID = new Guid(326, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x040001AE RID: 430
		private readonly ObjectReference<IGlobalInterfaceTable.Vftbl> _obj;

		// Token: 0x02000326 RID: 806
		[Guid("00000146-0000-0000-C000-000000000046")]
		public struct Vftbl
		{
			// Token: 0x1700047B RID: 1147
			// (get) Token: 0x0600178E RID: 6030 RVA: 0x000421B8 File Offset: 0x000403B8
			public method RegisterInterfaceInGlobal
			{
				get
				{
					return this._RegisterInterfaceInGlobal;
				}
			}

			// Token: 0x1700047C RID: 1148
			// (get) Token: 0x0600178F RID: 6031 RVA: 0x000421C0 File Offset: 0x000403C0
			public method RevokeInterfaceFromGlobal
			{
				get
				{
					return this._RevokeInterfaceFromGlobal;
				}
			}

			// Token: 0x1700047D RID: 1149
			// (get) Token: 0x06001790 RID: 6032 RVA: 0x000421C8 File Offset: 0x000403C8
			public method GetInterfaceFromGlobal
			{
				get
				{
					return this._GetInterfaceFromGlobal;
				}
			}

			// Token: 0x0400080F RID: 2063
			public IUnknownVftbl IUnknownVftbl;

			// Token: 0x04000810 RID: 2064
			private unsafe void* _RegisterInterfaceInGlobal;

			// Token: 0x04000811 RID: 2065
			private unsafe void* _RevokeInterfaceFromGlobal;

			// Token: 0x04000812 RID: 2066
			private unsafe void* _GetInterfaceFromGlobal;
		}
	}
}
