using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017F RID: 383
	internal static class AnimationTimelineTiming
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x0005C285 File Offset: 0x0005A485
		internal static int ToManualInputMilliseconds(double timeMilliseconds)
		{
			if (!double.IsFinite(timeMilliseconds))
			{
				return 0;
			}
			return (int)Math.Clamp(timeMilliseconds, 0.0, 2147483647.0);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0005C2AC File Offset: 0x0005A4AC
		internal static double ClampPlayerKeyframeTime(double requestedMilliseconds, double currentMilliseconds, double previousMilliseconds, double nextMilliseconds)
		{
			double fallback = double.IsFinite(currentMilliseconds) ? Math.Clamp(currentMilliseconds, 0.0, 2147483647.0) : 0.0;
			double minimum = double.IsFinite(previousMilliseconds) ? Math.Clamp(previousMilliseconds + 1.0, 0.0, 2147483647.0) : 0.0;
			double maximum = double.IsFinite(nextMilliseconds) ? Math.Clamp(nextMilliseconds - 1.0, 0.0, 2147483647.0) : 2147483647.0;
			if (maximum < minimum)
			{
				return fallback;
			}
			if (!double.IsFinite(requestedMilliseconds))
			{
				requestedMilliseconds = fallback;
			}
			return Math.Clamp(requestedMilliseconds, minimum, maximum);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005C36C File Offset: 0x0005A56C
		internal static double ClampPlaybackTime(double requestedMilliseconds, double startMilliseconds, double endMilliseconds)
		{
			if (!double.IsFinite(startMilliseconds))
			{
				startMilliseconds = 0.0;
			}
			if (!double.IsFinite(endMilliseconds))
			{
				endMilliseconds = startMilliseconds;
			}
			double minimum = Math.Min(startMilliseconds, endMilliseconds);
			double maximum = Math.Max(startMilliseconds, endMilliseconds);
			if (!double.IsFinite(requestedMilliseconds))
			{
				requestedMilliseconds = minimum;
			}
			return Math.Clamp(requestedMilliseconds, minimum, maximum);
		}

		// Token: 0x040008E0 RID: 2272
		private const double MaximumManualTimeMilliseconds = 2147483647.0;

		// Token: 0x040008E1 RID: 2273
		private const double MinimumKeyframeGapMilliseconds = 1.0;
	}
}
