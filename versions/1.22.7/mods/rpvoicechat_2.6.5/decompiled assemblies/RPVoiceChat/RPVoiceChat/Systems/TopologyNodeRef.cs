using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200004D RID: 77
	[ProtoContract(ImplicitFields = 1)]
	public class TopologyNodeRef
	{
		// Token: 0x060001BB RID: 443 RVA: 0x0000BF12 File Offset: 0x0000A112
		public TopologyNodeRef()
		{
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BF25 File Offset: 0x0000A125
		public TopologyNodeRef(TopologyNodeKind kind, string key)
		{
			this.Kind = kind;
			this.Key = (key ?? "");
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000BF50 File Offset: 0x0000A150
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

		// Token: 0x060001BE RID: 446 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
		public static TopologyNodeRef FromPlayer(string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return null;
			}
			return new TopologyNodeRef(TopologyNodeKind.Player, "player:" + playerUid);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public static TopologyNodeRef FromEntity(long entityId)
		{
			TopologyNodeKind kind = TopologyNodeKind.Entity;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("entity:");
			defaultInterpolatedStringHandler.AppendFormatted<long>(entityId);
			return new TopologyNodeRef(kind, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C020 File Offset: 0x0000A220
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

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C080 File Offset: 0x0000A280
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

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C10C File Offset: 0x0000A30C
		public string ToPlayerUid()
		{
			if (this.Kind != TopologyNodeKind.Player || string.IsNullOrEmpty(this.Key) || !this.Key.StartsWith("player:"))
			{
				return null;
			}
			return this.Key.Substring("player:".Length);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C158 File Offset: 0x0000A358
		[ProtoIgnore]
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.Key);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C168 File Offset: 0x0000A368
		public override bool Equals(object obj)
		{
			TopologyNodeRef other = obj as TopologyNodeRef;
			return other != null && this.Kind == other.Kind && this.Key == other.Key;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000C1A0 File Offset: 0x0000A3A0
		public override int GetHashCode()
		{
			return HashCode.Combine<TopologyNodeKind, string>(this.Kind, this.Key);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TopologyNodeKind>(this.Kind);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(this.Key);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040000BF RID: 191
		public TopologyNodeKind Kind;

		// Token: 0x040000C0 RID: 192
		public string Key = "";
	}
}
