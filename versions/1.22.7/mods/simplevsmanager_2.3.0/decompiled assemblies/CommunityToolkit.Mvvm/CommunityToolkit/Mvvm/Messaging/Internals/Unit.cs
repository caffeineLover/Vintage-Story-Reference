using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Internals
{
	// Token: 0x0200001D RID: 29
	internal readonly struct Unit : IEquatable<Unit>
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000046FA File Offset: 0x000028FA
		[MethodImpl(256)]
		public bool Equals(Unit other)
		{
			return true;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046FD File Offset: 0x000028FD
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return obj is Unit;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004708 File Offset: 0x00002908
		[MethodImpl(256)]
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
