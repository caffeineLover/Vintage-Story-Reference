using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200019B RID: 411
	internal struct MarshalInterface<T>
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x000265E4 File Offset: 0x000247E4
		public static T FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return (T)((object)null);
			}
			return MarshalInspectable<T>.FromAbi(ptr);
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00026600 File Offset: 0x00024800
		public static IObjectReference CreateMarshaler(T value)
		{
			if (value == null)
			{
				return null;
			}
			if (value.GetType() == MarshalInterface<T>.HelperType)
			{
				if (MarshalInterface<T>._ToAbi == null)
				{
					MarshalInterface<T>._ToAbi = MarshalInterface<T>.BindToAbi();
				}
				IntPtr @ref = MarshalInterface<T>._ToAbi.Invoke(value).GetRef();
				return ObjectReference<IUnknownVftbl>.Attach(ref @ref);
			}
			if (MarshalInterface<T>._CreateMarshaler == null)
			{
				MarshalInterface<T>._CreateMarshaler = MarshalInterface<T>.BindCreateMarshaler();
			}
			return MarshalInterface<T>._CreateMarshaler.Invoke(value);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00026678 File Offset: 0x00024878
		public static ObjectReferenceValue CreateMarshaler2(T value, Guid iid = default(Guid))
		{
			if (value == null)
			{
				return default(ObjectReferenceValue);
			}
			if (value.GetType() == MarshalInterface<T>.HelperType)
			{
				if (MarshalInterface<T>._ToAbi == null)
				{
					MarshalInterface<T>._ToAbi = MarshalInterface<T>.BindToAbi();
				}
				return MarshalInterface<T>._ToAbi.Invoke(value).AsValue();
			}
			return MarshalInspectable<T>.CreateMarshaler2(value, (iid == default(Guid)) ? GuidGenerator.GetIID(MarshalInterface<T>.HelperType) : iid, true);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000266F7 File Offset: 0x000248F7
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<T>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00026708 File Offset: 0x00024908
		public static IntPtr GetAbi(ObjectReferenceValue value)
		{
			return MarshalInterfaceHelper<T>.GetAbi(value);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00026710 File Offset: 0x00024910
		public static void DisposeAbi(IntPtr thisPtr)
		{
			MarshalInterfaceHelper<T>.DisposeAbi(thisPtr);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00026718 File Offset: 0x00024918
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<T>.DisposeMarshaler(value);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00026720 File Offset: 0x00024920
		public static void DisposeMarshaler(ObjectReferenceValue value)
		{
			MarshalInterfaceHelper<T>.DisposeMarshaler(value);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00026728 File Offset: 0x00024928
		internal static void DisposeMarshaler(object value)
		{
			if (value is ObjectReferenceValue)
			{
				ObjectReferenceValue objectReferenceValue = (ObjectReferenceValue)value;
				MarshalInterface<T>.DisposeMarshaler(objectReferenceValue);
				return;
			}
			MarshalInterface<T>.DisposeMarshaler((IObjectReference)value);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00026758 File Offset: 0x00024958
		public static IntPtr FromManaged(T value)
		{
			return MarshalInterface<T>.CreateMarshaler2(value, default(Guid)).Detach();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0002677C File Offset: 0x0002497C
		public unsafe static void CopyManaged(T value, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = MarshalInterface<T>.CreateMarshaler2(value, default(Guid)).Detach();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x000267A8 File Offset: 0x000249A8
		public static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array)
		{
			Func<T, ObjectReferenceValue> func;
			if ((func = MarshalInterface<T>.<>c.<>9__14_0) == null)
			{
				func = (MarshalInterface<T>.<>c.<>9__14_0 = new Func<T, ObjectReferenceValue>(MarshalInterface<T>.<>c.<>9.<CreateMarshalerArray>b__14_0));
			}
			return MarshalInterfaceHelper<T>.CreateMarshalerArray2(array, func);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000267CF File Offset: 0x000249CF
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.GetAbiArray(box);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000267D7 File Offset: 0x000249D7
		public static T[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.FromAbiArray(box, new Func<IntPtr, T>(MarshalInterface<T>.FromAbi));
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000267EB File Offset: 0x000249EB
		public static void CopyAbiArray(T[] array, object box)
		{
			MarshalInterfaceHelper<T>.CopyAbiArray(array, box, new Func<IntPtr, T>(MarshalInterface<T>.FromAbi));
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00026800 File Offset: 0x00024A00
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(T[] array)
		{
			Func<T, IntPtr> func;
			if ((func = MarshalInterface<T>.<>c.<>9__18_0) == null)
			{
				func = (MarshalInterface<T>.<>c.<>9__18_0 = new Func<T, IntPtr>(MarshalInterface<T>.<>c.<>9.<FromManagedArray>b__18_0));
			}
			return MarshalInterfaceHelper<T>.FromManagedArray(array, func);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00026827 File Offset: 0x00024A27
		public static void CopyManagedArray(T[] array, IntPtr data)
		{
			Action<T, IntPtr> action;
			if ((action = MarshalInterface<T>.<>c.<>9__19_0) == null)
			{
				action = (MarshalInterface<T>.<>c.<>9__19_0 = new Action<T, IntPtr>(MarshalInterface<T>.<>c.<>9.<CopyManagedArray>b__19_0));
			}
			MarshalInterfaceHelper<T>.CopyManagedArray(array, data, action);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0002684F File Offset: 0x00024A4F
		public static void DisposeMarshalerArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeMarshalerArray(box);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00026857 File Offset: 0x00024A57
		public static void DisposeAbiArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeAbiArray(box);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0002685F File Offset: 0x00024A5F
		private static Func<T, IObjectReference> BindToAbi()
		{
			MarshalInterface<T>.<>c__DisplayClass22_0 CS$<>8__locals1 = new MarshalInterface<T>.<>c__DisplayClass22_0();
			CS$<>8__locals1.objField = MarshalInterface<T>.HelperType.GetField("_obj", 38);
			return new Func<T, IObjectReference>(CS$<>8__locals1.<BindToAbi>b__0);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00026888 File Offset: 0x00024A88
		private static Func<T, IObjectReference> BindCreateMarshaler()
		{
			MarshalInterface<T>.<>c__DisplayClass23_0 CS$<>8__locals1 = new MarshalInterface<T>.<>c__DisplayClass23_0();
			Type type = MarshalInterface<T>.HelperType.FindVftblType();
			CS$<>8__locals1.iid = GuidGenerator.GetIID(MarshalInterface<T>.HelperType);
			if (type != null)
			{
				MarshalInterface<T>.<>c__DisplayClass23_1 CS$<>8__locals2 = new MarshalInterface<T>.<>c__DisplayClass23_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				MethodInfo methodInfo = typeof(MarshalInspectable<T>).GetMethod("CreateMarshaler", new Type[]
				{
					typeof(T),
					typeof(Guid),
					typeof(bool)
				}).MakeGenericMethod(new Type[]
				{
					type
				});
				CS$<>8__locals2.createMarshaler = (Func<T, Guid, bool, IObjectReference>)methodInfo.CreateDelegate(typeof(Func<T, Guid, bool, IObjectReference>));
				return new Func<T, IObjectReference>(CS$<>8__locals2.<BindCreateMarshaler>b__1);
			}
			return new Func<T, IObjectReference>(CS$<>8__locals1.<BindCreateMarshaler>b__0);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002694E File Offset: 0x00024B4E
		// Note: this type is marked as 'beforefieldinit'.
		static MarshalInterface()
		{
			MarshalInterface<T>.HelperType = typeof(T).GetHelperType();
		}

		// Token: 0x04000260 RID: 608
		private static readonly Type HelperType;

		// Token: 0x04000261 RID: 609
		private static Func<T, IObjectReference> _ToAbi;

		// Token: 0x04000262 RID: 610
		private static Func<T, IObjectReference> _CreateMarshaler;
	}
}
