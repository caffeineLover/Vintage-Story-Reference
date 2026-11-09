using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000057 RID: 87
	[ProtoContract(ImplicitFields = 1)]
	public class WireEdge
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		public WireEdge()
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E4EC File Offset: 0x0000C6EC
		public WireEdge(BlockPos a, BlockPos b)
		{
			if (WireEdge.ComparePos(a, b) <= 0)
			{
				this.PosA = ((a != null) ? a.Copy() : null);
				this.PosB = ((b != null) ? b.Copy() : null);
				return;
			}
			this.PosA = ((b != null) ? b.Copy() : null);
			this.PosB = ((a != null) ? a.Copy() : null);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E552 File Offset: 0x0000C752
		public bool Connects(BlockPos pos)
		{
			return pos != null && (pos.Equals(this.PosA) || pos.Equals(this.PosB));
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E57C File Offset: 0x0000C77C
		public BlockPos GetOther(BlockPos pos)
		{
			if (pos == null || this.PosA == null || this.PosB == null)
			{
				return null;
			}
			if (pos.Equals(this.PosA))
			{
				return this.PosB.Copy();
			}
			if (pos.Equals(this.PosB))
			{
				return this.PosA.Copy();
			}
			return null;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		public override bool Equals(object obj)
		{
			WireEdge other = obj as WireEdge;
			return other != null && this.PosA != null && this.PosB != null && other.PosA != null && other.PosB != null && this.PosA.Equals(other.PosA) && this.PosB.Equals(other.PosB);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E65D File Offset: 0x0000C85D
		public override int GetHashCode()
		{
			BlockPos posA = this.PosA;
			int num = (posA != null) ? posA.GetHashCode() : 0;
			BlockPos posB = this.PosB;
			return HashCode.Combine<int, int>(num, (posB != null) ? posB.GetHashCode() : 0);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000E688 File Offset: 0x0000C888
		private static int ComparePos(BlockPos a, BlockPos b)
		{
			if (a == null && b == null)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			int cmp = a.X.CompareTo(b.X);
			if (cmp != 0)
			{
				return cmp;
			}
			cmp = a.Y.CompareTo(b.Y);
			if (cmp != 0)
			{
				return cmp;
			}
			return a.Z.CompareTo(b.Z);
		}

		// Token: 0x040000DD RID: 221
		public BlockPos PosA;

		// Token: 0x040000DE RID: 222
		public BlockPos PosB;
	}
}
