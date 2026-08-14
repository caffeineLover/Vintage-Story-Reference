using System;
using ProtoBuf;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000039 RID: 57
	[ProtoContract(ImplicitFields = 1)]
	public class WireNodeRef
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000A9A5 File Offset: 0x00008BA5
		public WireNodeRef()
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000A9AD File Offset: 0x00008BAD
		public WireNodeRef(BlockPos pos, WireNodeKind kind)
		{
			this.Pos = ((pos != null) ? pos.Copy() : null);
			this.Kind = kind;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A9CE File Offset: 0x00008BCE
		public bool Matches(BEWireNode node)
		{
			return node != null && this.Pos != null && this.Pos.Equals(node.Pos);
		}

		// Token: 0x0400008A RID: 138
		public BlockPos Pos;

		// Token: 0x0400008B RID: 139
		public WireNodeKind Kind;
	}
}
