using System;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200002E RID: 46
	[ProtoContract(ImplicitFields = 1)]
	public class TopologyLink
	{
		// Token: 0x06000115 RID: 277 RVA: 0x0000890A File Offset: 0x00006B0A
		public TopologyLink()
		{
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00008928 File Offset: 0x00006B28
		public TopologyLink(TopologyNodeRef a, TopologyNodeRef b, TopologyLinkKind kind)
		{
			if (a == null || b == null || !a.IsValid || !b.IsValid || a.Key == b.Key)
			{
				this.NodeKeyA = "";
				this.NodeKeyB = "";
				return;
			}
			if (string.CompareOrdinal(a.Key, b.Key) <= 0)
			{
				this.NodeKeyA = a.Key;
				this.NodeKeyB = b.Key;
			}
			else
			{
				this.NodeKeyA = b.Key;
				this.NodeKeyB = a.Key;
			}
			this.Kind = kind;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000089DE File Offset: 0x00006BDE
		[ProtoIgnore]
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.NodeKeyA) && !string.IsNullOrEmpty(this.NodeKeyB) && this.NodeKeyA != this.NodeKeyB;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008A10 File Offset: 0x00006C10
		public override bool Equals(object obj)
		{
			TopologyLink other = obj as TopologyLink;
			return other != null && this.NodeKeyA == other.NodeKeyA && this.NodeKeyB == other.NodeKeyB && this.Kind == other.Kind;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008A5D File Offset: 0x00006C5D
		public override int GetHashCode()
		{
			return HashCode.Combine<string, string, TopologyLinkKind>(this.NodeKeyA, this.NodeKeyB, this.Kind);
		}

		// Token: 0x04000075 RID: 117
		public string NodeKeyA = "";

		// Token: 0x04000076 RID: 118
		public string NodeKeyB = "";

		// Token: 0x04000077 RID: 119
		public TopologyLinkKind Kind;
	}
}
