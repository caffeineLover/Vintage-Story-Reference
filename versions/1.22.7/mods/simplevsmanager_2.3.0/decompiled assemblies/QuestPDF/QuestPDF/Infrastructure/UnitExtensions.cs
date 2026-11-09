using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200006E RID: 110
	internal static class UnitExtensions
	{
		// Token: 0x060002BE RID: 702 RVA: 0x00007054 File Offset: 0x00005254
		public static float ToPoints(this float value, Unit unit)
		{
			UnitExtensions.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.unit = unit;
			return value * UnitExtensions.<ToPoints>g__GetConversionFactor|2_0(ref CS$<>8__locals1);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007074 File Offset: 0x00005274
		[CompilerGenerated]
		internal static float <ToPoints>g__GetConversionFactor|2_0(ref UnitExtensions.<>c__DisplayClass2_0 A_0)
		{
			float result;
			switch (A_0.unit)
			{
			case Unit.Point:
				result = 1f;
				break;
			case Unit.Meter:
				result = 2834.6458f;
				break;
			case Unit.Centimetre:
				result = 28.346457f;
				break;
			case Unit.Millimetre:
				result = 2.8346457f;
				break;
			case Unit.Feet:
				result = 864f;
				break;
			case Unit.Inch:
				result = 72f;
				break;
			case Unit.Mil:
				result = 0.072f;
				break;
			default:
				throw new ArgumentOutOfRangeException("unit", A_0.unit, null);
			}
			return result;
		}

		// Token: 0x04000194 RID: 404
		private const float InchToCentimetre = 2.54f;

		// Token: 0x04000195 RID: 405
		private const float InchToPoints = 72f;
	}
}
