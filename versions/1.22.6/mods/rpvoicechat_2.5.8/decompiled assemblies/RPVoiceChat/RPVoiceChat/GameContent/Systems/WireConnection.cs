using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x0200008E RID: 142
	public class WireConnection : IEquatable<WireConnection>
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00018B50 File Offset: 0x00016D50
		public IWireConnectable Node1 { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00018B58 File Offset: 0x00016D58
		public IWireConnectable Node2 { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00018B60 File Offset: 0x00016D60
		public BlockPos BlockPos1 { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00018B68 File Offset: 0x00016D68
		public BlockPos BlockPos2 { get; }

		// Token: 0x060004A0 RID: 1184 RVA: 0x00018B70 File Offset: 0x00016D70
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

		// Token: 0x060004A1 RID: 1185 RVA: 0x00018BCD File Offset: 0x00016DCD
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

		// Token: 0x060004A2 RID: 1186 RVA: 0x00018BFC File Offset: 0x00016DFC
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

		// Token: 0x060004A3 RID: 1187 RVA: 0x00018C5C File Offset: 0x00016E5C
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

		// Token: 0x060004A4 RID: 1188 RVA: 0x00018CF9 File Offset: 0x00016EF9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as WireConnection);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00018D08 File Offset: 0x00016F08
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
