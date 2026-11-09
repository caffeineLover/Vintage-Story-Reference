using System;
using ProtoBuf;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000053 RID: 83
	[ProtoContract(ImplicitFields = 1)]
	public class WireNodeRef
	{
		// Token: 0x06000218 RID: 536 RVA: 0x0000E295 File Offset: 0x0000C495
		public WireNodeRef()
		{
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000E29D File Offset: 0x0000C49D
		public WireNodeRef(BlockPos pos, WireNodeKind kind)
		{
			this.Pos = ((pos != null) ? pos.Copy() : null);
			this.Kind = kind;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E2BE File Offset: 0x0000C4BE
		public bool Matches(BEWireNode node)
		{
			return node != null && this.Pos != null && this.Pos.Equals(node.Pos);
		}

		// Token: 0x040000D5 RID: 213
		public BlockPos Pos;

		// Token: 0x040000D6 RID: 214
		public WireNodeKind Kind;
	}
}
