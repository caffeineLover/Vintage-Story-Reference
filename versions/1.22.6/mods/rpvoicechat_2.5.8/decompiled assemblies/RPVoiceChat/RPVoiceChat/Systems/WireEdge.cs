using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003C RID: 60
	[ProtoContract(ImplicitFields = 1)]
	public class WireEdge
	{
		// Token: 0x06000190 RID: 400 RVA: 0x0000AB50 File Offset: 0x00008D50
		public WireEdge()
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000AB58 File Offset: 0x00008D58
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

		// Token: 0x06000192 RID: 402 RVA: 0x0000ABBE File Offset: 0x00008DBE
		public bool Connects(BlockPos pos)
		{
			return pos != null && (pos.Equals(this.PosA) || pos.Equals(this.PosB));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000ABE8 File Offset: 0x00008DE8
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

		// Token: 0x06000194 RID: 404 RVA: 0x0000AC54 File Offset: 0x00008E54
		public override bool Equals(object obj)
		{
			WireEdge other = obj as WireEdge;
			return other != null && this.PosA != null && this.PosB != null && other.PosA != null && other.PosB != null && this.PosA.Equals(other.PosA) && this.PosB.Equals(other.PosB);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000ACC9 File Offset: 0x00008EC9
		public override int GetHashCode()
		{
			BlockPos posA = this.PosA;
			int num = (posA != null) ? posA.GetHashCode() : 0;
			BlockPos posB = this.PosB;
			return HashCode.Combine<int, int>(num, (posB != null) ? posB.GetHashCode() : 0);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000ACF4 File Offset: 0x00008EF4
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

		// Token: 0x04000092 RID: 146
		public BlockPos PosA;

		// Token: 0x04000093 RID: 147
		public BlockPos PosB;
	}
}
