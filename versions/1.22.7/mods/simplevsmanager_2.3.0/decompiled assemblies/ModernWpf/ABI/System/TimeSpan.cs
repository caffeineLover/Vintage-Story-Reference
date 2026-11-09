using System;

namespace ABI.System
{
	// Token: 0x02000135 RID: 309
	internal struct TimeSpan
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x0001CA58 File Offset: 0x0001AC58
		public static TimeSpan.Marshaler CreateMarshaler(TimeSpan value)
		{
			return new TimeSpan.Marshaler
			{
				__abi = new TimeSpan
				{
					Duration = value.Ticks
				}
			};
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		public static TimeSpan GetAbi(TimeSpan.Marshaler m)
		{
			return m.__abi;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0001CA94 File Offset: 0x0001AC94
		public static TimeSpan FromAbi(TimeSpan value)
		{
			return TimeSpan.FromTicks(value.Duration);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0001CAA1 File Offset: 0x0001ACA1
		public unsafe static void CopyAbi(TimeSpan.Marshaler arg, IntPtr dest)
		{
			*(TimeSpan*)dest.ToPointer() = TimeSpan.GetAbi(arg);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001CAB8 File Offset: 0x0001ACB8
		public static TimeSpan FromManaged(TimeSpan value)
		{
			return new TimeSpan
			{
				Duration = value.Ticks
			};
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001CADC File Offset: 0x0001ACDC
		public unsafe static void CopyManaged(TimeSpan arg, IntPtr dest)
		{
			*(TimeSpan*)dest.ToPointer() = TimeSpan.FromManaged(arg);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
		public static void DisposeMarshaler(TimeSpan.Marshaler m)
		{
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001CAF2 File Offset: 0x0001ACF2
		public static void DisposeAbi(TimeSpan abi)
		{
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
		public static string GetGuidSignature()
		{
			return "struct(Windows.Foundation.TimeSpan;i8)";
		}

		// Token: 0x040001D9 RID: 473
		public long Duration;

		// Token: 0x0200034B RID: 843
		public struct Marshaler
		{
			// Token: 0x0400087E RID: 2174
			public TimeSpan __abi;
		}
	}
}
