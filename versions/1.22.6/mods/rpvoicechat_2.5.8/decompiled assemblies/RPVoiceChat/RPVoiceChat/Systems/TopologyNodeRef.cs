using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000033 RID: 51
	[ProtoContract(ImplicitFields = 1)]
	public class TopologyNodeRef
	{
		// Token: 0x0600012A RID: 298 RVA: 0x0000874A File Offset: 0x0000694A
		public TopologyNodeRef()
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000875D File Offset: 0x0000695D
		public TopologyNodeRef(TopologyNodeKind kind, string key)
		{
			this.Kind = kind;
			this.Key = (key ?? "");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008788 File Offset: 0x00006988
		public static TopologyNodeRef FromBlock(BlockPos pos)
		{
			if (pos == null)
			{
				return null;
			}
			TopologyNodeKind kind = TopologyNodeKind.Block;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 3);
			defaultInterpolatedStringHandler.AppendLiteral("block:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.X);
			defaultInterpolatedStringHandler.AppendLiteral("|");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Y);
			defaultInterpolatedStringHandler.AppendLiteral("|");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Z);
			return new TopologyNodeRef(kind, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008801 File Offset: 0x00006A01
		public static TopologyNodeRef FromPlayer(string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return null;
			}
			return new TopologyNodeRef(TopologyNodeKind.Player, "player:" + playerUid);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008820 File Offset: 0x00006A20
		public static TopologyNodeRef FromEntity(long entityId)
		{
			TopologyNodeKind kind = TopologyNodeKind.Entity;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("entity:");
			defaultInterpolatedStringHandler.AppendFormatted<long>(entityId);
			return new TopologyNodeRef(kind, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008858 File Offset: 0x00006A58
		public static TopologyNodeRef FromKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (key.StartsWith("block:"))
			{
				return new TopologyNodeRef(TopologyNodeKind.Block, key);
			}
			if (key.StartsWith("player:"))
			{
				return new TopologyNodeRef(TopologyNodeKind.Player, key);
			}
			if (key.StartsWith("entity:"))
			{
				return new TopologyNodeRef(TopologyNodeKind.Entity, key);
			}
			return new TopologyNodeRef(TopologyNodeKind.Block, key);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000088B8 File Offset: 0x00006AB8
		public BlockPos ToBlockPos()
		{
			if (this.Kind != TopologyNodeKind.Block || string.IsNullOrEmpty(this.Key) || !this.Key.StartsWith("block:"))
			{
				return null;
			}
			string[] parts = this.Key.Substring("block:".Length).Split('|', 0);
			if (parts.Length != 3)
			{
				return null;
			}
			int x;
			int y;
			int z;
			if (!int.TryParse(parts[0], ref x) || !int.TryParse(parts[1], ref y) || !int.TryParse(parts[2], ref z))
			{
				return null;
			}
			return new BlockPos(x, y, z);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008944 File Offset: 0x00006B44
		public string ToPlayerUid()
		{
			if (this.Kind != TopologyNodeKind.Player || string.IsNullOrEmpty(this.Key) || !this.Key.StartsWith("player:"))
			{
				return null;
			}
			return this.Key.Substring("player:".Length);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00008990 File Offset: 0x00006B90
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.Key);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000089A0 File Offset: 0x00006BA0
		public override bool Equals(object obj)
		{
			TopologyNodeRef other = obj as TopologyNodeRef;
			return other != null && this.Kind == other.Kind && this.Key == other.Key;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000089D8 File Offset: 0x00006BD8
		public override int GetHashCode()
		{
			return HashCode.Combine<TopologyNodeKind, string>(this.Kind, this.Key);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000089EC File Offset: 0x00006BEC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TopologyNodeKind>(this.Kind);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(this.Key);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000074 RID: 116
		public TopologyNodeKind Kind;

		// Token: 0x04000075 RID: 117
		public string Key = "";
	}
}
