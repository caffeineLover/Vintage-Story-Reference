using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000047 RID: 71
	public readonly struct PlayerPositionData
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x0001A09E File Offset: 0x0001829E
		public PlayerPositionData(double height, bool onGround, long timeStamp)
		{
			this.Height = height;
			this.OnGround = onGround;
			this.TimeStamp = timeStamp;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001A0B8 File Offset: 0x000182B8
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(7, 3);
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Height, "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.OnGround);
			defaultInterpolatedStringHandler.AppendLiteral(") at ");
			defaultInterpolatedStringHandler.AppendFormatted<TimeSpan>(TimeSpan.FromMilliseconds(this.TimeStamp));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040001A7 RID: 423
		public readonly double Height;

		// Token: 0x040001A8 RID: 424
		public readonly bool OnGround;

		// Token: 0x040001A9 RID: 425
		public readonly long TimeStamp;
	}
}
