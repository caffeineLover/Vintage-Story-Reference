using System;

namespace WinRT
{
	// Token: 0x02000198 RID: 408
	internal class MarshalGeneric<T>
	{
		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002562C File Offset: 0x0002382C
		private static Func<T, object> BindCreateMarshaler()
		{
			MarshalGeneric<T>.<>c__DisplayClass6_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass6_0();
			CS$<>8__locals1.createMarshaler = MarshalGeneric<T>.HelperType.GetMethod("CreateMarshaler", 56);
			return new Func<T, object>(CS$<>8__locals1.<BindCreateMarshaler>b__0);
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00025655 File Offset: 0x00023855
		internal static Func<T, object> CreateMarshaler2
		{
			get
			{
				if (!MarshalGeneric<T>.MarshalByObjectReferenceValueSupported)
				{
					return MarshalGeneric<T>.CreateMarshaler;
				}
				return MarshalGeneric<T>.CreateMarshaler2Lazy.Value;
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002566E File Offset: 0x0002386E
		private static Func<T, object> BindCreateMarshaler2()
		{
			MarshalGeneric<T>.<>c__DisplayClass10_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass10_0();
			CS$<>8__locals1.createMarshaler = (Func<T, ObjectReferenceValue>)MarshalGeneric<T>.HelperType.GetMethod("CreateMarshaler2", 56).CreateDelegate(typeof(Func<T, ObjectReferenceValue>));
			return new Func<T, object>(CS$<>8__locals1.<BindCreateMarshaler2>b__0);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000256AB File Offset: 0x000238AB
		private static Func<object, object> BindGetAbi()
		{
			MarshalGeneric<T>.<>c__DisplayClass13_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass13_0();
			CS$<>8__locals1.getAbi = MarshalGeneric<T>.HelperType.GetMethod("GetAbi", 56);
			return new Func<object, object>(CS$<>8__locals1.<BindGetAbi>b__0);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000256D4 File Offset: 0x000238D4
		private static Action<object, IntPtr> BindCopyAbi()
		{
			MarshalGeneric<T>.<>c__DisplayClass16_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass16_0();
			CS$<>8__locals1.copyAbi = MarshalGeneric<T>.HelperType.GetMethod("CopyAbi", 56);
			if (CS$<>8__locals1.copyAbi == null)
			{
				return null;
			}
			return new Action<object, IntPtr>(CS$<>8__locals1.<BindCopyAbi>b__0);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002571A File Offset: 0x0002391A
		private static Func<object, T> BindFromAbi()
		{
			MarshalGeneric<T>.<>c__DisplayClass19_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass19_0();
			CS$<>8__locals1.fromAbi = MarshalGeneric<T>.HelperType.GetMethod("FromAbi", 56);
			return new Func<object, T>(CS$<>8__locals1.<BindFromAbi>b__0);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00025743 File Offset: 0x00023943
		private static Func<T, object> BindFromManaged()
		{
			MarshalGeneric<T>.<>c__DisplayClass22_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass22_0();
			CS$<>8__locals1.fromManaged = MarshalGeneric<T>.HelperType.GetMethod("FromManaged", 56);
			return new Func<T, object>(CS$<>8__locals1.<BindFromManaged>b__0);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002576C File Offset: 0x0002396C
		private static Action<T, IntPtr> BindCopyManaged()
		{
			MarshalGeneric<T>.<>c__DisplayClass25_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass25_0();
			CS$<>8__locals1.copyManaged = MarshalGeneric<T>.HelperType.GetMethod("CopyManaged", 56);
			if (CS$<>8__locals1.copyManaged == null)
			{
				return null;
			}
			return new Action<T, IntPtr>(CS$<>8__locals1.<BindCopyManaged>b__0);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x000257B2 File Offset: 0x000239B2
		private static Action<object> BindDisposeMarshaler()
		{
			MarshalGeneric<T>.<>c__DisplayClass28_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass28_0();
			CS$<>8__locals1.disposeMarshaler = MarshalGeneric<T>.HelperType.GetMethod("DisposeMarshaler", 56);
			return new Action<object>(CS$<>8__locals1.<BindDisposeMarshaler>b__0);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000257DC File Offset: 0x000239DC
		private static Action<object> BindDisposeAbi()
		{
			MarshalGeneric<T>.<>c__DisplayClass31_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass31_0();
			CS$<>8__locals1.disposeAbi = MarshalGeneric<T>.HelperType.GetMethod("DisposeAbi", 56);
			if (CS$<>8__locals1.disposeAbi == null)
			{
				return null;
			}
			return new Action<object>(CS$<>8__locals1.<BindDisposeAbi>b__0);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00025824 File Offset: 0x00023A24
		private static Func<T[], object> BindCreateMarshalerArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass34_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass34_0();
			CS$<>8__locals1.createMarshalerArray = MarshalGeneric<T>.HelperType.GetMethod("CreateMarshalerArray", 56);
			if (CS$<>8__locals1.createMarshalerArray == null)
			{
				return null;
			}
			return new Func<T[], object>(CS$<>8__locals1.<BindCreateMarshalerArray>b__0);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002586C File Offset: 0x00023A6C
		private static Func<object, ValueTuple<int, IntPtr>> BindGetAbiArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass37_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass37_0();
			CS$<>8__locals1.getAbiArray = MarshalGeneric<T>.HelperType.GetMethod("GetAbiArray", 56);
			if (CS$<>8__locals1.getAbiArray == null)
			{
				return null;
			}
			return new Func<object, ValueTuple<int, IntPtr>>(CS$<>8__locals1.<BindGetAbiArray>b__0);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000258B4 File Offset: 0x00023AB4
		private static Func<object, T[]> BindFromAbiArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass40_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass40_0();
			CS$<>8__locals1.fromAbiArray = MarshalGeneric<T>.HelperType.GetMethod("FromAbiArray", 56);
			if (CS$<>8__locals1.fromAbiArray == null)
			{
				return null;
			}
			return new Func<object, T[]>(CS$<>8__locals1.<BindFromAbiArray>b__0);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x000258FC File Offset: 0x00023AFC
		private static Func<T[], ValueTuple<int, IntPtr>> BindFromManagedArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass43_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass43_0();
			CS$<>8__locals1.fromManagedArray = MarshalGeneric<T>.HelperType.GetMethod("FromManagedArray", 56);
			if (CS$<>8__locals1.fromManagedArray == null)
			{
				return null;
			}
			return new Func<T[], ValueTuple<int, IntPtr>>(CS$<>8__locals1.<BindFromManagedArray>b__0);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00025944 File Offset: 0x00023B44
		private static Action<object> BindDisposeMarshalerArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass46_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass46_0();
			CS$<>8__locals1.disposeMarshalerArray = MarshalGeneric<T>.HelperType.GetMethod("DisposeMarshalerArray", 56);
			if (CS$<>8__locals1.disposeMarshalerArray == null)
			{
				return null;
			}
			return new Action<object>(CS$<>8__locals1.<BindDisposeMarshalerArray>b__0);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002598C File Offset: 0x00023B8C
		private static Action<object> BindDisposeAbiArray()
		{
			MarshalGeneric<T>.<>c__DisplayClass49_0 CS$<>8__locals1 = new MarshalGeneric<T>.<>c__DisplayClass49_0();
			CS$<>8__locals1.disposeAbiArray = MarshalGeneric<T>.HelperType.GetMethod("DisposeAbiArray", 56);
			if (CS$<>8__locals1.disposeAbiArray == null)
			{
				return null;
			}
			return new Action<object>(CS$<>8__locals1.<BindDisposeAbiArray>b__0);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000259D4 File Offset: 0x00023BD4
		private unsafe static void CopyManagedFallback(T value, IntPtr dest)
		{
			if (MarshalGeneric<T>.MarshalByObjectReferenceValueSupported)
			{
				*(IntPtr*)dest.ToPointer() = ((value == null) ? IntPtr.Zero : ((ObjectReferenceValue)MarshalGeneric<T>.CreateMarshaler2.Invoke(value)).Detach());
				return;
			}
			*(IntPtr*)dest.ToPointer() = ((value == null) ? IntPtr.Zero : ((IObjectReference)MarshalGeneric<T>.CreateMarshaler.Invoke(value)).GetRef());
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00025A44 File Offset: 0x00023C44
		internal static void CopyManagedArray(T[] array, IntPtr data)
		{
			MarshalInterfaceHelper<T>.CopyManagedArray(array, data, MarshalGeneric<T>.CopyManagedLazy.Value ?? new Action<T, IntPtr>(MarshalGeneric<T>.CopyManagedFallback));
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00025A70 File Offset: 0x00023C70
		// Note: this type is marked as 'beforefieldinit'.
		static MarshalGeneric()
		{
			MarshalGeneric<T>.HelperType = typeof(T).GetHelperType();
			MarshalGeneric<T>.AbiType = typeof(T).GetAbiType();
			MarshalGeneric<T>.MarshalerType = typeof(T).GetMarshalerType();
			MarshalGeneric<T>.MarshalByObjectReferenceValueSupported = (typeof(T).GetMarshaler2Type() == typeof(ObjectReferenceValue));
			MarshalGeneric<T>.CreateMarshaler = new Func<T, object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_0);
			MarshalGeneric<T>.CreateMarshalerLazy = new Lazy<Func<T, object>>(new Func<Func<T, object>>(MarshalGeneric<T>.BindCreateMarshaler));
			MarshalGeneric<T>.CreateMarshaler2Lazy = new Lazy<Func<T, object>>(new Func<Func<T, object>>(MarshalGeneric<T>.BindCreateMarshaler2));
			MarshalGeneric<T>.GetAbi = (MarshalGeneric<T>.MarshalByObjectReferenceValueSupported ? new Func<object, object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_1) : new Func<object, object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_2));
			MarshalGeneric<T>.GetAbiLazy = new Lazy<Func<object, object>>(new Func<Func<object, object>>(MarshalGeneric<T>.BindGetAbi));
			MarshalGeneric<T>.CopyAbi = new Action<object, IntPtr>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_3);
			MarshalGeneric<T>.CopyAbiLazy = new Lazy<Action<object, IntPtr>>(new Func<Action<object, IntPtr>>(MarshalGeneric<T>.BindCopyAbi));
			MarshalGeneric<T>.FromAbi = new Func<object, T>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_4);
			MarshalGeneric<T>.FromAbiLazy = new Lazy<Func<object, T>>(new Func<Func<object, T>>(MarshalGeneric<T>.BindFromAbi));
			MarshalGeneric<T>.FromManaged = new Func<T, object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_5);
			MarshalGeneric<T>.FromManagedLazy = new Lazy<Func<T, object>>(new Func<Func<T, object>>(MarshalGeneric<T>.BindFromManaged));
			MarshalGeneric<T>.CopyManaged = new Action<T, IntPtr>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_6);
			MarshalGeneric<T>.CopyManagedLazy = new Lazy<Action<T, IntPtr>>(new Func<Action<T, IntPtr>>(MarshalGeneric<T>.BindCopyManaged));
			MarshalGeneric<T>.DisposeMarshaler = (MarshalGeneric<T>.MarshalByObjectReferenceValueSupported ? new Action<object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_7) : new Action<object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_8));
			MarshalGeneric<T>.DisposeMarshalerLazy = new Lazy<Action<object>>(new Func<Action<object>>(MarshalGeneric<T>.BindDisposeMarshaler));
			MarshalGeneric<T>.DisposeAbi = new Action<object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_9);
			MarshalGeneric<T>.DisposeAbiLazy = new Lazy<Action<object>>(new Func<Action<object>>(MarshalGeneric<T>.BindDisposeAbi));
			MarshalGeneric<T>.CreateMarshalerArray = new Func<T[], object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_10);
			MarshalGeneric<T>.CreateMarshalerArrayLazy = new Lazy<Func<T[], object>>(new Func<Func<T[], object>>(MarshalGeneric<T>.BindCreateMarshalerArray));
			MarshalGeneric<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_11);
			MarshalGeneric<T>.GetAbiArrayLazy = new Lazy<Func<object, ValueTuple<int, IntPtr>>>(new Func<Func<object, ValueTuple<int, IntPtr>>>(MarshalGeneric<T>.BindGetAbiArray));
			MarshalGeneric<T>.FromAbiArray = new Func<object, T[]>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_12);
			MarshalGeneric<T>.FromAbiArrayLazy = new Lazy<Func<object, T[]>>(new Func<Func<object, T[]>>(MarshalGeneric<T>.BindFromAbiArray));
			MarshalGeneric<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_13);
			MarshalGeneric<T>.FromManagedArrayLazy = new Lazy<Func<T[], ValueTuple<int, IntPtr>>>(new Func<Func<T[], ValueTuple<int, IntPtr>>>(MarshalGeneric<T>.BindFromManagedArray));
			MarshalGeneric<T>.DisposeMarshalerArray = new Action<object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_14);
			MarshalGeneric<T>.DisposeMarshalerArrayLazy = new Lazy<Action<object>>(new Func<Action<object>>(MarshalGeneric<T>.BindDisposeMarshalerArray));
			MarshalGeneric<T>.DisposeAbiArray = new Action<object>(MarshalGeneric<T>.<>c.<>9.<.cctor>b__53_15);
			MarshalGeneric<T>.DisposeAbiArrayLazy = new Lazy<Action<object>>(new Func<Action<object>>(MarshalGeneric<T>.BindDisposeAbiArray));
		}

		// Token: 0x0400023E RID: 574
		protected static readonly Type HelperType;

		// Token: 0x0400023F RID: 575
		protected static readonly Type AbiType;

		// Token: 0x04000240 RID: 576
		protected static readonly Type MarshalerType;

		// Token: 0x04000241 RID: 577
		private static readonly bool MarshalByObjectReferenceValueSupported;

		// Token: 0x04000242 RID: 578
		public static readonly Func<T, object> CreateMarshaler;

		// Token: 0x04000243 RID: 579
		private static readonly Lazy<Func<T, object>> CreateMarshalerLazy;

		// Token: 0x04000244 RID: 580
		private static readonly Lazy<Func<T, object>> CreateMarshaler2Lazy;

		// Token: 0x04000245 RID: 581
		public static readonly Func<object, object> GetAbi;

		// Token: 0x04000246 RID: 582
		private static readonly Lazy<Func<object, object>> GetAbiLazy;

		// Token: 0x04000247 RID: 583
		public static readonly Action<object, IntPtr> CopyAbi;

		// Token: 0x04000248 RID: 584
		private static readonly Lazy<Action<object, IntPtr>> CopyAbiLazy;

		// Token: 0x04000249 RID: 585
		public static readonly Func<object, T> FromAbi;

		// Token: 0x0400024A RID: 586
		private static readonly Lazy<Func<object, T>> FromAbiLazy;

		// Token: 0x0400024B RID: 587
		public static readonly Func<T, object> FromManaged;

		// Token: 0x0400024C RID: 588
		private static readonly Lazy<Func<T, object>> FromManagedLazy;

		// Token: 0x0400024D RID: 589
		public static readonly Action<T, IntPtr> CopyManaged;

		// Token: 0x0400024E RID: 590
		private static readonly Lazy<Action<T, IntPtr>> CopyManagedLazy;

		// Token: 0x0400024F RID: 591
		public static readonly Action<object> DisposeMarshaler;

		// Token: 0x04000250 RID: 592
		private static readonly Lazy<Action<object>> DisposeMarshalerLazy;

		// Token: 0x04000251 RID: 593
		internal static readonly Action<object> DisposeAbi;

		// Token: 0x04000252 RID: 594
		private static readonly Lazy<Action<object>> DisposeAbiLazy;

		// Token: 0x04000253 RID: 595
		internal static readonly Func<T[], object> CreateMarshalerArray;

		// Token: 0x04000254 RID: 596
		private static readonly Lazy<Func<T[], object>> CreateMarshalerArrayLazy;

		// Token: 0x04000255 RID: 597
		internal static readonly Func<object, ValueTuple<int, IntPtr>> GetAbiArray;

		// Token: 0x04000256 RID: 598
		private static readonly Lazy<Func<object, ValueTuple<int, IntPtr>>> GetAbiArrayLazy;

		// Token: 0x04000257 RID: 599
		internal static readonly Func<object, T[]> FromAbiArray;

		// Token: 0x04000258 RID: 600
		private static readonly Lazy<Func<object, T[]>> FromAbiArrayLazy;

		// Token: 0x04000259 RID: 601
		internal static readonly Func<T[], ValueTuple<int, IntPtr>> FromManagedArray;

		// Token: 0x0400025A RID: 602
		private static readonly Lazy<Func<T[], ValueTuple<int, IntPtr>>> FromManagedArrayLazy;

		// Token: 0x0400025B RID: 603
		internal static readonly Action<object> DisposeMarshalerArray;

		// Token: 0x0400025C RID: 604
		private static readonly Lazy<Action<object>> DisposeMarshalerArrayLazy;

		// Token: 0x0400025D RID: 605
		internal static readonly Action<object> DisposeAbiArray;

		// Token: 0x0400025E RID: 606
		private static readonly Lazy<Action<object>> DisposeAbiArrayLazy;
	}
}
