using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000B7 RID: 183
	public class WireConnection : IEquatable<WireConnection>
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001F02C File Offset: 0x0001D22C
		public IWireConnectable Node1 { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001F034 File Offset: 0x0001D234
		public IWireConnectable Node2 { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001F03C File Offset: 0x0001D23C
		public BlockPos BlockPos1 { get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001F044 File Offset: 0x0001D244
		public BlockPos BlockPos2 { get; }

		// Token: 0x060005BB RID: 1467 RVA: 0x0001F04C File Offset: 0x0001D24C
		public WireConnection(IWireConnectable node1, IWireConnectable node2)
		{
			this.Node1 = node1;
			this.Node2 = node2;
			object obj;
			if (node1 == null)
			{
				obj = null;
			}
			else
			{
				BlockPos position = node1.Position;
				obj = ((position != null) ? position.Copy() : null);
			}
			this.BlockPos1 = obj;
			object obj2;
			if (node2 == null)
			{
				obj2 = null;
			}
			else
			{
				BlockPos position2 = node2.Position;
				obj2 = ((position2 != null) ? position2.Copy() : null);
			}
			this.BlockPos2 = obj2;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001F0A9 File Offset: 0x0001D2A9
		public BEWireNode GetOtherNode(IWireConnectable from)
		{
			if (from == this.Node1)
			{
				return this.Node2 as BEWireNode;
			}
			if (from == this.Node2)
			{
				return this.Node1 as BEWireNode;
			}
			return null;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001F0D8 File Offset: 0x0001D2D8
		public BlockPos GetOtherBlockPos(BlockPos fromPos)
		{
			if (fromPos == null || this.BlockPos1 == null || this.BlockPos2 == null)
			{
				return null;
			}
			if (fromPos.Equals(this.BlockPos1))
			{
				return this.BlockPos2;
			}
			if (fromPos.Equals(this.BlockPos2))
			{
				return this.BlockPos1;
			}
			return null;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001F138 File Offset: 0x0001D338
		public bool Equals(WireConnection other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.BlockPos1 == null || this.BlockPos2 == null || other.BlockPos1 == null || other.BlockPos2 == null)
			{
				return false;
			}
			bool flag = this.BlockPos1.Equals(other.BlockPos1) && this.BlockPos2.Equals(other.BlockPos2);
			bool matchInverse = this.BlockPos1.Equals(other.BlockPos2) && this.BlockPos2.Equals(other.BlockPos1);
			return flag || matchInverse;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001F1D5 File Offset: 0x0001D3D5
		public override bool Equals(object obj)
		{
			return this.Equals(obj as WireConnection);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
		public override int GetHashCode()
		{
			BlockPos blockPos = this.BlockPos1;
			int num = (blockPos != null) ? blockPos.GetHashCode() : 0;
			BlockPos blockPos2 = this.BlockPos2;
			int hash2 = (blockPos2 != null) ? blockPos2.GetHashCode() : 0;
			return num ^ hash2;
		}
	}
}
