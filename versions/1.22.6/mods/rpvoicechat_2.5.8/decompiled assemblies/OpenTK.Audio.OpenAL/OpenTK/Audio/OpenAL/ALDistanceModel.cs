using System;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000025 RID: 37
	public enum ALDistanceModel
	{
		// Token: 0x040000BD RID: 189
		None,
		// Token: 0x040000BE RID: 190
		InverseDistance = 53249,
		// Token: 0x040000BF RID: 191
		InverseDistanceClamped,
		// Token: 0x040000C0 RID: 192
		LinearDistance,
		// Token: 0x040000C1 RID: 193
		LinearDistanceClamped,
		// Token: 0x040000C2 RID: 194
		ExponentDistance,
		// Token: 0x040000C3 RID: 195
		ExponentDistanceClamped
	}
}
