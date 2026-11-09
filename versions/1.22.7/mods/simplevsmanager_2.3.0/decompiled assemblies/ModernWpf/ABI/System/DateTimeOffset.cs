using System;

namespace ABI.System
{
	// Token: 0x02000136 RID: 310
	internal struct DateTimeOffset
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x0001CAFC File Offset: 0x0001ACFC
		public static DateTimeOffset.Marshaler CreateMarshaler(DateTimeOffset value)
		{
			return new DateTimeOffset.Marshaler
			{
				__abi = new DateTimeOffset
				{
					UniversalTime = value.UtcTicks - 504911232000000000L
				}
			};
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001CB3A File Offset: 0x0001AD3A
		public static DateTimeOffset GetAbi(DateTimeOffset.Marshaler m)
		{
			return m.__abi;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001CB44 File Offset: 0x0001AD44
		public static DateTimeOffset FromAbi(DateTimeOffset value)
		{
			DateTimeOffset dateTimeOffset;
			dateTimeOffset..ctor(value.UniversalTime + 504911232000000000L, TimeSpan.Zero);
			TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTimeOffset);
			long num = dateTimeOffset.Ticks + utcOffset.Ticks;
			if (num < DateTime.MinValue.Ticks || num > DateTime.MaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException();
			}
			return dateTimeOffset.ToLocalTime();
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
		public unsafe static void CopyAbi(DateTimeOffset.Marshaler arg, IntPtr dest)
		{
			*(DateTimeOffset*)dest.ToPointer() = DateTimeOffset.GetAbi(arg);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0001CBC4 File Offset: 0x0001ADC4
		public static DateTimeOffset FromManaged(DateTimeOffset value)
		{
			return new DateTimeOffset
			{
				UniversalTime = value.UtcTicks - 504911232000000000L
			};
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0001CBF2 File Offset: 0x0001ADF2
		public unsafe static void CopyManaged(DateTimeOffset arg, IntPtr dest)
		{
			*(DateTimeOffset*)dest.ToPointer() = DateTimeOffset.FromManaged(arg);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0001CC06 File Offset: 0x0001AE06
		public static void DisposeMarshaler(DateTimeOffset.Marshaler m)
		{
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001CC08 File Offset: 0x0001AE08
		public static void DisposeAbi(DateTimeOffset abi)
		{
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0001CC0A File Offset: 0x0001AE0A
		public static string GetGuidSignature()
		{
			return "struct(Windows.Foundation.DateTime;i8)";
		}

		// Token: 0x040001DA RID: 474
		public long UniversalTime;

		// Token: 0x040001DB RID: 475
		private const long ManagedUtcTicksAtNativeZero = 504911232000000000L;

		// Token: 0x0200034C RID: 844
		public struct Marshaler
		{
			// Token: 0x0400087F RID: 2175
			public DateTimeOffset __abi;
		}
	}
}
