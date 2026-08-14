using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000178 RID: 376
	[ObjectReferenceWrapper("_obj")]
	[Guid("DA049FF2-D2E0-5FE8-8C7B-F87F26060B6F")]
	internal sealed class INotifyCollectionChangedEventArgs
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x000213C5 File Offset: 0x0001F5C5
		internal static ObjectReference<INotifyCollectionChangedEventArgs.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<INotifyCollectionChangedEventArgs.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000213CD File Offset: 0x0001F5CD
		public static implicit operator INotifyCollectionChangedEventArgs(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new INotifyCollectionChangedEventArgs(obj);
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x000213DA File Offset: 0x0001F5DA
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x000213E2 File Offset: 0x0001F5E2
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000213EF File Offset: 0x0001F5EF
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000213FC File Offset: 0x0001F5FC
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00021409 File Offset: 0x0001F609
		public INotifyCollectionChangedEventArgs(IObjectReference obj) : this(obj.As<INotifyCollectionChangedEventArgs.Vftbl>())
		{
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00021417 File Offset: 0x0001F617
		internal INotifyCollectionChangedEventArgs(ObjectReference<INotifyCollectionChangedEventArgs.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00021428 File Offset: 0x0001F628
		public unsafe NotifyCollectionChangedAction Action
		{
			get
			{
				NotifyCollectionChangedAction result = 0;
				method get_Action_ = this._obj.Vftbl.get_Action_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Collections.Specialized.NotifyCollectionChangedAction*), this.ThisPtr, &result, get_Action_));
				return result;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00021460 File Offset: 0x0001F660
		public unsafe IList NewItems
		{
			get
			{
				IntPtr intPtr = default(IntPtr);
				IList result;
				try
				{
					method get_NewItems_ = this._obj.Vftbl.get_NewItems_1;
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, get_NewItems_));
					result = MarshalInterface<IList>.FromAbi(intPtr);
				}
				finally
				{
					MarshalInterface<IList>.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000214C0 File Offset: 0x0001F6C0
		public unsafe int NewStartingIndex
		{
			get
			{
				int result = 0;
				method get_NewStartingIndex_ = this._obj.Vftbl.get_NewStartingIndex_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32*), this.ThisPtr, &result, get_NewStartingIndex_));
				return result;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000214F8 File Offset: 0x0001F6F8
		public unsafe IList OldItems
		{
			get
			{
				IntPtr intPtr = default(IntPtr);
				IList result;
				try
				{
					method get_OldItems_ = this._obj.Vftbl.get_OldItems_2;
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, get_OldItems_));
					result = MarshalInterface<IList>.FromAbi(intPtr);
				}
				finally
				{
					MarshalInterface<IList>.DisposeAbi(intPtr);
				}
				return result;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00021558 File Offset: 0x0001F758
		public unsafe int OldStartingIndex
		{
			get
			{
				int result = 0;
				method get_OldStartingIndex_ = this._obj.Vftbl.get_OldStartingIndex_4;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32*), this.ThisPtr, &result, get_OldStartingIndex_));
				return result;
			}
		}

		// Token: 0x04000208 RID: 520
		private readonly ObjectReference<INotifyCollectionChangedEventArgs.Vftbl> _obj;

		// Token: 0x020003BD RID: 957
		[Guid("DA049FF2-D2E0-5FE8-8C7B-F87F26060B6F")]
		public struct Vftbl
		{
			// Token: 0x170004EF RID: 1263
			// (get) Token: 0x06001A85 RID: 6789 RVA: 0x00048C2D File Offset: 0x00046E2D
			public method get_Action_0
			{
				get
				{
					return this._get_Action_0;
				}
			}

			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x06001A86 RID: 6790 RVA: 0x00048C35 File Offset: 0x00046E35
			public method get_NewItems_1
			{
				get
				{
					return this._get_NewItems_1;
				}
			}

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x06001A87 RID: 6791 RVA: 0x00048C3D File Offset: 0x00046E3D
			public method get_OldItems_2
			{
				get
				{
					return this._get_OldItems_2;
				}
			}

			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x06001A88 RID: 6792 RVA: 0x00048C45 File Offset: 0x00046E45
			public method get_NewStartingIndex_3
			{
				get
				{
					return this._get_NewStartingIndex_3;
				}
			}

			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x06001A89 RID: 6793 RVA: 0x00048C4D File Offset: 0x00046E4D
			public method get_OldStartingIndex_4
			{
				get
				{
					return this._get_OldStartingIndex_4;
				}
			}

			// Token: 0x04000967 RID: 2407
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000968 RID: 2408
			private unsafe void* _get_Action_0;

			// Token: 0x04000969 RID: 2409
			private unsafe void* _get_NewItems_1;

			// Token: 0x0400096A RID: 2410
			private unsafe void* _get_OldItems_2;

			// Token: 0x0400096B RID: 2411
			private unsafe void* _get_NewStartingIndex_3;

			// Token: 0x0400096C RID: 2412
			private unsafe void* _get_OldStartingIndex_4;
		}
	}
}
