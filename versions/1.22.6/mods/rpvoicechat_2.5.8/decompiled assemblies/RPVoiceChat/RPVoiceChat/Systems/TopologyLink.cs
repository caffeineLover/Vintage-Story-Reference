using System;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200002D RID: 45
	[ProtoContract(ImplicitFields = 1)]
	public class TopologyLink
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00007EEE File Offset: 0x000060EE
		public TopologyLink()
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007F0C File Offset: 0x0000610C
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00007FC2 File Offset: 0x000061C2
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.NodeKeyA) && !string.IsNullOrEmpty(this.NodeKeyB) && this.NodeKeyA != this.NodeKeyB;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007FF4 File Offset: 0x000061F4
		public override bool Equals(object obj)
		{
			TopologyLink other = obj as TopologyLink;
			return other != null && this.NodeKeyA == other.NodeKeyA && this.NodeKeyB == other.NodeKeyB && this.Kind == other.Kind;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008041 File Offset: 0x00006241
		public override int GetHashCode()
		{
			return HashCode.Combine<string, string, TopologyLinkKind>(this.NodeKeyA, this.NodeKeyB, this.Kind);
		}

		// Token: 0x04000067 RID: 103
		public string NodeKeyA = "";

		// Token: 0x04000068 RID: 104
		public string NodeKeyB = "";

		// Token: 0x04000069 RID: 105
		public TopologyLinkKind Kind;
	}
}
