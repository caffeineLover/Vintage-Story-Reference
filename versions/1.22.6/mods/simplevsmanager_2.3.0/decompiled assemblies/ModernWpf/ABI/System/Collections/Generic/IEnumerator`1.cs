using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Interop;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Generic
{
	// Token: 0x0200015D RID: 349
	[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
	internal class IEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable, IIterator<T>
	{
		// Token: 0x06000B24 RID: 2852 RVA: 0x0001EEF5 File Offset: 0x0001D0F5
		public static IObjectReference CreateMarshaler(IEnumerator<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IEnumerator<T>.Vftbl>(obj, IEnumerator<T>.PIID);
			}
			return null;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0001EF07 File Offset: 0x0001D107
		public static ObjectReferenceValue CreateMarshaler2(IEnumerator<T> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IEnumerator<T>.PIID);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0001EF14 File Offset: 0x0001D114
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0001EF25 File Offset: 0x0001D125
		public static IEnumerator<T> FromAbi(IntPtr thisPtr)
		{
			if (!(thisPtr == IntPtr.Zero))
			{
				return new IEnumerator<T>(IEnumerator<T>.ObjRefFromAbi(thisPtr));
			}
			return null;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0001EF41 File Offset: 0x0001D141
		internal static IIterator<T> FromAbiInternal(IntPtr thisPtr)
		{
			return new IEnumerator<T>(IEnumerator<T>.ObjRefFromAbi(thisPtr));
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0001EF50 File Offset: 0x0001D150
		public static IntPtr FromManaged(IEnumerator<T> value)
		{
			if (value != null)
			{
				return IEnumerator<T>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0001EF74 File Offset: 0x0001D174
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0001EF7F File Offset: 0x0001D17F
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IIterator<T>>.DisposeAbi(abi);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0001EF87 File Offset: 0x0001D187
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IEnumerator<T>));
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0001EF98 File Offset: 0x0001D198
		public static ObjectReference<IEnumerator<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IEnumerator<T>.Vftbl vftbl = new IEnumerator<T>.Vftbl(thisPtr);
			return ObjectReference<IEnumerator<T>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0001EFC3 File Offset: 0x0001D1C3
		public static implicit operator IEnumerator<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerator<T>(obj);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0001EFD0 File Offset: 0x0001D1D0
		public static implicit operator IEnumerator<T>(ObjectReference<IEnumerator<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerator<T>(obj);
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0001EFDD File Offset: 0x0001D1DD
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0001EFE5 File Offset: 0x0001D1E5
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0001EFF2 File Offset: 0x0001D1F2
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0001EFFF File Offset: 0x0001D1FF
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0001F00C File Offset: 0x0001D20C
		public IEnumerator(IObjectReference obj) : this(obj.As<IEnumerator<T>.Vftbl>())
		{
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0001F01A File Offset: 0x0001D21A
		public IEnumerator(ObjectReference<IEnumerator<T>.Vftbl> obj)
		{
			this._obj = obj;
			this._FromIterator = new IEnumerator<T>.FromAbiHelper(this);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0001F038 File Offset: 0x0001D238
		public bool _MoveNext()
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.MoveNext_2(this.ThisPtr, out b));
			return b > 0;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0001F070 File Offset: 0x0001D270
		public uint GetMany(ref T[] items)
		{
			object obj = null;
			IntPtr intPtr = default(IntPtr);
			uint num = 0U;
			uint result;
			try
			{
				obj = Marshaler<T>.CreateMarshalerArray.Invoke(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray.Invoke(obj);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetMany_3(this.ThisPtr, item, intPtr, out num));
				items = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(item, intPtr));
				result = num;
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray.Invoke(obj);
			}
			return result;
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0001F110 File Offset: 0x0001D310
		public T _Current
		{
			get
			{
				object[] array = new object[2];
				array[0] = this.ThisPtr;
				object[] array2 = array;
				T result;
				try
				{
					this._obj.Vftbl.get_Current_0.DynamicInvokeAbi(array2);
					result = Marshaler<T>.FromAbi.Invoke(array2[1]);
				}
				finally
				{
					Marshaler<T>.DisposeAbi.Invoke(array2[1]);
				}
				return result;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0001F178 File Offset: 0x0001D378
		public bool HasCurrent
		{
			get
			{
				byte b = 0;
				method get_HasCurrent_ = this._obj.Vftbl.Get_HasCurrent_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref b, get_HasCurrent_));
				return b > 0;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0001F1B2 File Offset: 0x0001D3B2
		public bool MoveNext()
		{
			return this._FromIterator.MoveNext();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0001F1BF File Offset: 0x0001D3BF
		public void Reset()
		{
			this._FromIterator.Reset();
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0001F1CC File Offset: 0x0001D3CC
		public void Dispose()
		{
			this._FromIterator.Dispose();
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0001F1D9 File Offset: 0x0001D3D9
		public T Current
		{
			get
			{
				return this._FromIterator.Current;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0001F1E6 File Offset: 0x0001D3E6
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0001F1F3 File Offset: 0x0001D3F3
		// Note: this type is marked as 'beforefieldinit'.
		static IEnumerator()
		{
			IEnumerator<T>.PIID = IEnumerator<T>.Vftbl.PIID;
		}

		// Token: 0x040001EF RID: 495
		public static Guid PIID;

		// Token: 0x040001F0 RID: 496
		protected readonly ObjectReference<IEnumerator<T>.Vftbl> _obj;

		// Token: 0x040001F1 RID: 497
		private IEnumerator<T>.FromAbiHelper _FromIterator;

		// Token: 0x02000389 RID: 905
		public class FromAbiHelper : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x0600195F RID: 6495 RVA: 0x00045F14 File Offset: 0x00044114
			public FromAbiHelper(IObjectReference obj) : this(new IEnumerator<T>(obj))
			{
			}

			// Token: 0x06001960 RID: 6496 RVA: 0x00045F22 File Offset: 0x00044122
			internal FromAbiHelper(IIterator<T> iterator)
			{
				this.m_hadCurrent = true;
				base..ctor();
				this._iterator = iterator;
			}

			// Token: 0x170004CC RID: 1228
			// (get) Token: 0x06001961 RID: 6497 RVA: 0x00045F38 File Offset: 0x00044138
			public T Current
			{
				get
				{
					if (!this.m_isInitialized)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumNotStarted);
					}
					if (!this.m_hadCurrent)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumEnded);
					}
					return this.m_current;
				}
			}

			// Token: 0x170004CD RID: 1229
			// (get) Token: 0x06001962 RID: 6498 RVA: 0x00045F66 File Offset: 0x00044166
			object IEnumerator.Current
			{
				get
				{
					if (!this.m_isInitialized)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumNotStarted);
					}
					if (!this.m_hadCurrent)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumEnded);
					}
					return this.m_current;
				}
			}

			// Token: 0x06001963 RID: 6499 RVA: 0x00045F9C File Offset: 0x0004419C
			public bool MoveNext()
			{
				if (!this.m_hadCurrent)
				{
					return false;
				}
				try
				{
					if (!this.m_isInitialized)
					{
						this.m_hadCurrent = this._iterator.HasCurrent;
						this.m_isInitialized = true;
					}
					else
					{
						this.m_hadCurrent = this._iterator._MoveNext();
					}
					if (this.m_hadCurrent)
					{
						this.m_current = this._iterator._Current;
					}
				}
				catch (Exception ex)
				{
					if (Marshal.GetHRForException(ex) == -2147483636)
					{
						throw new InvalidOperationException(WinRTRuntimeErrorStrings.InvalidOperation_EnumFailedVersion);
					}
					throw;
				}
				return this.m_hadCurrent;
			}

			// Token: 0x06001964 RID: 6500 RVA: 0x00046034 File Offset: 0x00044234
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06001965 RID: 6501 RVA: 0x0004603B File Offset: 0x0004423B
			public void Dispose()
			{
			}

			// Token: 0x040008EF RID: 2287
			private readonly IIterator<T> _iterator;

			// Token: 0x040008F0 RID: 2288
			private bool m_hadCurrent;

			// Token: 0x040008F1 RID: 2289
			private T m_current;

			// Token: 0x040008F2 RID: 2290
			private bool m_isInitialized;
		}

		// Token: 0x0200038A RID: 906
		public sealed class ToAbiHelper : IIterator<T>, IBindableIterator
		{
			// Token: 0x06001966 RID: 6502 RVA: 0x0004603D File Offset: 0x0004423D
			internal ToAbiHelper(IEnumerator<T> enumerator)
			{
				this.m_firstItem = true;
				base..ctor();
				this.m_enumerator = enumerator;
			}

			// Token: 0x170004CE RID: 1230
			// (get) Token: 0x06001967 RID: 6503 RVA: 0x00046053 File Offset: 0x00044253
			public T _Current
			{
				get
				{
					if (this.m_firstItem)
					{
						this.m_firstItem = false;
						this._MoveNext();
					}
					if (!this.m_hasCurrent)
					{
						ExceptionHelpers.ThrowExceptionForHR(-2147483637);
					}
					return this.m_enumerator.Current;
				}
			}

			// Token: 0x170004CF RID: 1231
			// (get) Token: 0x06001968 RID: 6504 RVA: 0x00046088 File Offset: 0x00044288
			public bool HasCurrent
			{
				get
				{
					if (this.m_firstItem)
					{
						this.m_firstItem = false;
						this._MoveNext();
					}
					return this.m_hasCurrent;
				}
			}

			// Token: 0x06001969 RID: 6505 RVA: 0x000460A8 File Offset: 0x000442A8
			public bool _MoveNext()
			{
				try
				{
					this.m_hasCurrent = this.m_enumerator.MoveNext();
				}
				catch (InvalidOperationException)
				{
					ExceptionHelpers.ThrowExceptionForHR(-2147483636);
				}
				return this.m_hasCurrent;
			}

			// Token: 0x0600196A RID: 6506 RVA: 0x000460EC File Offset: 0x000442EC
			public uint GetMany(ref T[] items)
			{
				if (items == null)
				{
					return 0U;
				}
				int num = 0;
				while (num < items.Length && this.HasCurrent)
				{
					items[num] = this._Current;
					this._MoveNext();
					num++;
				}
				if (typeof(T) == typeof(string))
				{
					string[] array = items as string[];
					for (int i = num; i < items.Length; i++)
					{
						array[i] = string.Empty;
					}
				}
				return (uint)num;
			}

			// Token: 0x170004D0 RID: 1232
			// (get) Token: 0x0600196B RID: 6507 RVA: 0x00046166 File Offset: 0x00044366
			public object Current
			{
				get
				{
					return this._Current;
				}
			}

			// Token: 0x0600196C RID: 6508 RVA: 0x00046173 File Offset: 0x00044373
			public bool MoveNext()
			{
				return this._MoveNext();
			}

			// Token: 0x0600196D RID: 6509 RVA: 0x0004617B File Offset: 0x0004437B
			uint IBindableIterator.GetMany(ref object[] items)
			{
				throw new NotImplementedException();
			}

			// Token: 0x040008F3 RID: 2291
			private readonly IEnumerator<T> m_enumerator;

			// Token: 0x040008F4 RID: 2292
			private bool m_firstItem;

			// Token: 0x040008F5 RID: 2293
			private bool m_hasCurrent;
		}

		// Token: 0x0200038B RID: 907
		[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
		public struct Vftbl
		{
			// Token: 0x170004D1 RID: 1233
			// (get) Token: 0x0600196E RID: 6510 RVA: 0x00046182 File Offset: 0x00044382
			// (set) Token: 0x0600196F RID: 6511 RVA: 0x0004618A File Offset: 0x0004438A
			internal method Get_HasCurrent_1
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

			// Token: 0x06001970 RID: 6512 RVA: 0x00046194 File Offset: 0x00044394
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IEnumerator<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.get_Current_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IEnumerator<T>.Vftbl.get_Current_0_Type);
				this.Get_HasCurrent_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.MoveNext_2 = Marshal.GetDelegateForFunctionPointer<IEnumerator_Delegates.MoveNext_2>(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
				this.GetMany_3 = Marshal.GetDelegateForFunctionPointer<IEnumerator_Delegates.GetMany_3>(ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}

			// Token: 0x06001971 RID: 6513 RVA: 0x00046230 File Offset: 0x00044430
			unsafe static Vftbl()
			{
				IEnumerator<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IEnumerator<T>));
				IEnumerator<T>.Vftbl.get_Current_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<T>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IEnumerator<T>.Vftbl._adapterTable = new ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper>();
				IEnumerator<T>.Vftbl.AbiToProjectionVftable = new IEnumerator<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					get_Current_0 = Delegate.CreateDelegate(IEnumerator<T>.Vftbl.get_Current_0_Type, typeof(IEnumerator<T>.Vftbl).GetMethod("Do_Abi_get_Current_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_get_HasCurrent_1 = (void*)Marshal.GetFunctionPointerForDelegate(IEnumerator<T>.Vftbl.DelegateCache = new _get_PropertyAsBoolean(IEnumerator<T>.Vftbl.Do_Abi_get_HasCurrent_1)),
					MoveNext_2 = new IEnumerator_Delegates.MoveNext_2(IEnumerator<T>.Vftbl.Do_Abi_MoveNext_2),
					GetMany_3 = new IEnumerator_Delegates.GetMany_3(IEnumerator<T>.Vftbl.Do_Abi_GetMany_3)
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IEnumerator<T>.Vftbl.AbiToProjectionVftable.get_Current_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IEnumerator<T>.Vftbl.AbiToProjectionVftable.Get_HasCurrent_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IEnumerator_Delegates.MoveNext_2>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.MoveNext_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IEnumerator_Delegates.GetMany_3>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.GetMany_3);
				IEnumerator<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001972 RID: 6514 RVA: 0x000463D8 File Offset: 0x000445D8
			private static IEnumerator<T>.ToAbiHelper FindAdapter(IntPtr thisPtr)
			{
				IEnumerator<T> enumerator = ComWrappersSupport.FindObject<IEnumerator<T>>(thisPtr);
				ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper> adapterTable = IEnumerator<T>.Vftbl._adapterTable;
				IEnumerator<T> enumerator2 = enumerator;
				ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper>.CreateValueCallback createValueCallback;
				if ((createValueCallback = IEnumerator<T>.Vftbl.<>c.<>9__16_0) == null)
				{
					createValueCallback = (IEnumerator<T>.Vftbl.<>c.<>9__16_0 = new ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper>.CreateValueCallback(IEnumerator<T>.Vftbl.<>c.<>9.<FindAdapter>b__16_0));
				}
				return adapterTable.GetValue(enumerator2, createValueCallback);
			}

			// Token: 0x06001973 RID: 6515 RVA: 0x00046418 File Offset: 0x00044618
			private static int Do_Abi_MoveNext_2(IntPtr thisPtr, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IEnumerator<T>.Vftbl.FindAdapter(thisPtr)._MoveNext() ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001974 RID: 6516 RVA: 0x00046464 File Offset: 0x00044664
			private static int Do_Abi_GetMany_3(IntPtr thisPtr, int __itemsSize, IntPtr items, out uint __return_value__)
			{
				__return_value__ = 0U;
				T[] array = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(__itemsSize, items));
				try
				{
					uint many = IEnumerator<T>.Vftbl.FindAdapter(thisPtr).GetMany(ref array);
					Marshaler<T>.CopyManagedArray.Invoke(array, items);
					__return_value__ = many;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001975 RID: 6517 RVA: 0x000464D0 File Offset: 0x000446D0
			private unsafe static int Do_Abi_get_Current_0<TAbi>(void* thisPtr, out TAbi __return_value__)
			{
				T t = default(T);
				__return_value__ = default(TAbi);
				try
				{
					t = IEnumerator<T>.Vftbl.FindAdapter(new IntPtr(thisPtr))._Current;
					__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged.Invoke(t));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001976 RID: 6518 RVA: 0x00046538 File Offset: 0x00044738
			private static int Do_Abi_get_HasCurrent_1(IntPtr thisPtr, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IEnumerator<T>.Vftbl.FindAdapter(thisPtr).HasCurrent ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040008F6 RID: 2294
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008F7 RID: 2295
			public Delegate get_Current_0;

			// Token: 0x040008F8 RID: 2296
			private unsafe void* _get_HasCurrent_1;

			// Token: 0x040008F9 RID: 2297
			public IEnumerator_Delegates.MoveNext_2 MoveNext_2;

			// Token: 0x040008FA RID: 2298
			public IEnumerator_Delegates.GetMany_3 GetMany_3;

			// Token: 0x040008FB RID: 2299
			public static Guid PIID;

			// Token: 0x040008FC RID: 2300
			private static readonly Type get_Current_0_Type;

			// Token: 0x040008FD RID: 2301
			private static readonly IEnumerator<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x040008FE RID: 2302
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040008FF RID: 2303
			private static readonly Delegate DelegateCache;

			// Token: 0x04000900 RID: 2304
			private static ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper> _adapterTable;
		}
	}
}
