using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using ABI.System.Collections.Generic;
using Microsoft.UI.Xaml.Interop;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections
{
	// Token: 0x0200014C RID: 332
	[DynamicInterfaceCastableImplementation]
	[Guid("036D2C08-DF29-41AF-8AA2-D774BE62BA6F")]
	internal interface IEnumerable : IEnumerable, IBindableIterable
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x0001D924 File Offset: 0x0001BB24
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IEnumerable));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0001D935 File Offset: 0x0001BB35
		internal static ObjectReference<IEnumerable.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return ObjectReference<IEnumerable.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0001D94C File Offset: 0x0001BB4C
		private static IEnumerable.FromAbiHelper _AbiHelper(IWinRTObject _this)
		{
			return (IEnumerable.FromAbiHelper)_this.GetOrCreateTypeHelperData(typeof(IEnumerable).TypeHandle, () => new IEnumerable.FromAbiHelper((IEnumerable)_this));
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001D994 File Offset: 0x0001BB94
		unsafe IBindableIterator First()
		{
			ObjectReference<IEnumerable.Vftbl> objectReference = (ObjectReference<IEnumerable.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IBindableIterator result;
			try
			{
				method first_ = objectReference.Vftbl.First_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), thisPtr, &intPtr, first_));
				result = MarshalInterface<IBindableIterator>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<IBindableIterator>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0001DA14 File Offset: 0x0001BC14
		IEnumerator GetEnumerator()
		{
			return IEnumerable._AbiHelper((IWinRTObject)this).GetEnumerator();
		}

		// Token: 0x02000364 RID: 868
		public sealed class AdaptiveFromAbiHelper : IEnumerable.FromAbiHelper, IEnumerable
		{
			// Token: 0x0600187B RID: 6267 RVA: 0x00044224 File Offset: 0x00042424
			public AdaptiveFromAbiHelper(Type runtimeType, IWinRTObject winRTObject) : base(winRTObject)
			{
				Type type = (runtimeType.IsGenericType && runtimeType.GetGenericTypeDefinition() == typeof(IEnumerable)) ? runtimeType : runtimeType.GetInterface("System.Collections.Generic.IEnumerable`1");
				if (type != null)
				{
					MethodInfo getEnumerator = type.GetMethod("GetEnumerator");
					this._enumerator = ((IWinRTObject obj) => (IEnumerator)getEnumerator.Invoke(obj, null));
				}
			}

			// Token: 0x0600187C RID: 6268 RVA: 0x00044298 File Offset: 0x00042498
			public override IEnumerator GetEnumerator()
			{
				if (this._enumerator == null)
				{
					return base.GetEnumerator();
				}
				return this._enumerator.Invoke(this._winrtObject);
			}

			// Token: 0x040008A9 RID: 2217
			private readonly Func<IWinRTObject, IEnumerator> _enumerator;
		}

		// Token: 0x02000365 RID: 869
		public class FromAbiHelper : IEnumerable
		{
			// Token: 0x0600187D RID: 6269 RVA: 0x000442BA File Offset: 0x000424BA
			public FromAbiHelper(IEnumerable iterable)
			{
				this._iterable = iterable;
			}

			// Token: 0x0600187E RID: 6270 RVA: 0x000442C9 File Offset: 0x000424C9
			protected FromAbiHelper(IWinRTObject winrtObject)
			{
				this._iterable = null;
				this._winrtObject = winrtObject;
			}

			// Token: 0x0600187F RID: 6271 RVA: 0x000442DF File Offset: 0x000424DF
			private IWinRTObject GetIterable()
			{
				return ((IWinRTObject)this._iterable) ?? this._winrtObject;
			}

			// Token: 0x06001880 RID: 6272 RVA: 0x000442F6 File Offset: 0x000424F6
			public virtual IEnumerator GetEnumerator()
			{
				return new IEnumerator<object>.FromAbiHelper(new IEnumerable.FromAbiHelper.NonGenericToGenericIterator(((IBindableIterable)this.GetIterable()).First()));
			}

			// Token: 0x040008AA RID: 2218
			private readonly IEnumerable _iterable;

			// Token: 0x040008AB RID: 2219
			protected readonly IWinRTObject _winrtObject;

			// Token: 0x02000428 RID: 1064
			private sealed class NonGenericToGenericIterator : IIterator<object>
			{
				// Token: 0x06001C3F RID: 7231 RVA: 0x0004B83D File Offset: 0x00049A3D
				public NonGenericToGenericIterator(IBindableIterator iterator)
				{
					this.iterator = iterator;
				}

				// Token: 0x17000526 RID: 1318
				// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0004B84C File Offset: 0x00049A4C
				public object _Current
				{
					get
					{
						return this.iterator.Current;
					}
				}

				// Token: 0x17000527 RID: 1319
				// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0004B859 File Offset: 0x00049A59
				public bool HasCurrent
				{
					get
					{
						return this.iterator.HasCurrent;
					}
				}

				// Token: 0x06001C42 RID: 7234 RVA: 0x0004B866 File Offset: 0x00049A66
				public bool _MoveNext()
				{
					return this.iterator.MoveNext();
				}

				// Token: 0x06001C43 RID: 7235 RVA: 0x0004B873 File Offset: 0x00049A73
				public uint GetMany(ref object[] items)
				{
					throw new NotSupportedException();
				}

				// Token: 0x04000A41 RID: 2625
				private readonly IBindableIterator iterator;
			}
		}

		// Token: 0x02000366 RID: 870
		public sealed class ToAbiHelper : IBindableIterable
		{
			// Token: 0x06001881 RID: 6273 RVA: 0x00044312 File Offset: 0x00042512
			internal ToAbiHelper(IEnumerable enumerable)
			{
				this.m_enumerable = enumerable;
			}

			// Token: 0x06001882 RID: 6274 RVA: 0x00044321 File Offset: 0x00042521
			IBindableIterator IBindableIterable.First()
			{
				return IEnumerable.ToAbiHelper.MakeBindableIterator(this.m_enumerable.GetEnumerator());
			}

			// Token: 0x06001883 RID: 6275 RVA: 0x00044333 File Offset: 0x00042533
			internal static IBindableIterator MakeBindableIterator(IEnumerator enumerator)
			{
				return new IEnumerator<object>.ToAbiHelper(new IEnumerable.ToAbiHelper.NonGenericToGenericEnumerator(enumerator));
			}

			// Token: 0x040008AC RID: 2220
			private readonly IEnumerable m_enumerable;

			// Token: 0x02000429 RID: 1065
			private sealed class NonGenericToGenericEnumerator : IEnumerator<object>, IEnumerator, IDisposable
			{
				// Token: 0x06001C44 RID: 7236 RVA: 0x0004B87A File Offset: 0x00049A7A
				public NonGenericToGenericEnumerator(IEnumerator enumerator)
				{
					this.enumerator = enumerator;
				}

				// Token: 0x17000528 RID: 1320
				// (get) Token: 0x06001C45 RID: 7237 RVA: 0x0004B889 File Offset: 0x00049A89
				public object Current
				{
					get
					{
						return this.enumerator.Current;
					}
				}

				// Token: 0x06001C46 RID: 7238 RVA: 0x0004B896 File Offset: 0x00049A96
				public bool MoveNext()
				{
					return this.enumerator.MoveNext();
				}

				// Token: 0x06001C47 RID: 7239 RVA: 0x0004B8A3 File Offset: 0x00049AA3
				public void Reset()
				{
					this.enumerator.Reset();
				}

				// Token: 0x06001C48 RID: 7240 RVA: 0x0004B8B0 File Offset: 0x00049AB0
				public void Dispose()
				{
				}

				// Token: 0x04000A42 RID: 2626
				private readonly IEnumerator enumerator;
			}
		}

		// Token: 0x02000367 RID: 871
		[Guid("036D2C08-DF29-41AF-8AA2-D774BE62BA6F")]
		public struct Vftbl
		{
			// Token: 0x170004A8 RID: 1192
			// (get) Token: 0x06001884 RID: 6276 RVA: 0x00044340 File Offset: 0x00042540
			// (set) Token: 0x06001885 RID: 6277 RVA: 0x00044348 File Offset: 0x00042548
			public method First_0
			{
				get
				{
					return this._First_0;
				}
				set
				{
					this._First_0 = value;
				}
			}

			// Token: 0x06001886 RID: 6278 RVA: 0x00044354 File Offset: 0x00042554
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(IEnumerable.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IEnumerable.Vftbl>(IEnumerable.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				IEnumerable.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001887 RID: 6279 RVA: 0x000443C8 File Offset: 0x000425C8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_First_0(IntPtr thisPtr, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					IBindableIterator bindableIterator = IEnumerable.ToAbiHelper.MakeBindableIterator(ComWrappersSupport.FindObject<IEnumerable>(thisPtr).GetEnumerator());
					*result = MarshalInterface<IBindableIterator>.FromManaged(bindableIterator);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040008AD RID: 2221
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040008AE RID: 2222
			private unsafe void* _First_0;

			// Token: 0x040008AF RID: 2223
			private static readonly IEnumerable.Vftbl AbiToProjectionVftable = new IEnumerable.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_First_0 = ldftn(Do_Abi_First_0)
			};

			// Token: 0x040008B0 RID: 2224
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
