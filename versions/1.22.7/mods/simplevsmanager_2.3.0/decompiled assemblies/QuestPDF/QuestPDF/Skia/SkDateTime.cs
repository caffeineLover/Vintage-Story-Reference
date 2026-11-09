using System;

namespace QuestPDF.Skia
{
	// Token: 0x0200000B RID: 11
	internal struct SkDateTime
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000294C File Offset: 0x00000B4C
		public SkDateTime(DateTimeOffset dateTime)
		{
			this.TimeZoneMinutes = (short)dateTime.Offset.TotalMinutes;
			this.Year = (ushort)dateTime.Year;
			this.Month = (byte)dateTime.Month;
			this.DayOfWeek = dateTime.DayOfWeek;
			this.Day = (byte)dateTime.Day;
			this.Hour = (byte)dateTime.Hour;
			this.Minute = (byte)dateTime.Minute;
			this.Second = (byte)dateTime.Second;
		}

		// Token: 0x04000020 RID: 32
		public short TimeZoneMinutes;

		// Token: 0x04000021 RID: 33
		public ushort Year;

		// Token: 0x04000022 RID: 34
		public byte Month;

		// Token: 0x04000023 RID: 35
		public byte DayOfWeek;

		// Token: 0x04000024 RID: 36
		public byte Day;

		// Token: 0x04000025 RID: 37
		public byte Hour;

		// Token: 0x04000026 RID: 38
		public byte Minute;

		// Token: 0x04000027 RID: 39
		public byte Second;
	}
}
