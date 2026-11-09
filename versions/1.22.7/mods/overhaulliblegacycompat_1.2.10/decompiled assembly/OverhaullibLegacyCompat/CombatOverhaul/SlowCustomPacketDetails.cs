using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CombatOverhaul
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct SlowCustomPacketDetails : IEquatable<SlowCustomPacketDetails>
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002B2C File Offset: 0x00000D2C
		public SlowCustomPacketDetails(string ChannelName, int ChannelId, int MessageId, string MessageTypeName, int PayloadBytes, string HandlerDescription)
		{
			this.ChannelName = ChannelName;
			this.ChannelId = ChannelId;
			this.MessageId = MessageId;
			this.MessageTypeName = MessageTypeName;
			this.PayloadBytes = PayloadBytes;
			this.HandlerDescription = HandlerDescription;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002B5B File Offset: 0x00000D5B
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002B63 File Offset: 0x00000D63
		public string ChannelName { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002B6C File Offset: 0x00000D6C
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002B74 File Offset: 0x00000D74
		public int ChannelId { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002B7D File Offset: 0x00000D7D
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002B85 File Offset: 0x00000D85
		public int MessageId { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002B8E File Offset: 0x00000D8E
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002B96 File Offset: 0x00000D96
		public string MessageTypeName { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002B9F File Offset: 0x00000D9F
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002BA7 File Offset: 0x00000DA7
		public int PayloadBytes { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002BB0 File Offset: 0x00000DB0
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public string HandlerDescription { get; set; }

		// Token: 0x06000047 RID: 71 RVA: 0x00002BC4 File Offset: 0x00000DC4
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SlowCustomPacketDetails");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C10 File Offset: 0x00000E10
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("ChannelName = ");
			builder.Append(this.ChannelName);
			builder.Append(", ChannelId = ");
			builder.Append(this.ChannelId.ToString());
			builder.Append(", MessageId = ");
			builder.Append(this.MessageId.ToString());
			builder.Append(", MessageTypeName = ");
			builder.Append(this.MessageTypeName);
			builder.Append(", PayloadBytes = ");
			builder.Append(this.PayloadBytes.ToString());
			builder.Append(", HandlerDescription = ");
			builder.Append(this.HandlerDescription);
			return true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002CDE File Offset: 0x00000EDE
		[CompilerGenerated]
		public static bool operator !=(SlowCustomPacketDetails left, SlowCustomPacketDetails right)
		{
			return !(left == right);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CEA File Offset: 0x00000EEA
		[CompilerGenerated]
		public static bool operator ==(SlowCustomPacketDetails left, SlowCustomPacketDetails right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002CF4 File Offset: 0x00000EF4
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<string>.Default.GetHashCode(this.<ChannelName>k__BackingField) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<ChannelId>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<MessageId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<MessageTypeName>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<PayloadBytes>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<HandlerDescription>k__BackingField);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D84 File Offset: 0x00000F84
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is SlowCustomPacketDetails && this.Equals((SlowCustomPacketDetails)obj);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D9C File Offset: 0x00000F9C
		[CompilerGenerated]
		public bool Equals(SlowCustomPacketDetails other)
		{
			return EqualityComparer<string>.Default.Equals(this.<ChannelName>k__BackingField, other.<ChannelName>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<ChannelId>k__BackingField, other.<ChannelId>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<MessageId>k__BackingField, other.<MessageId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<MessageTypeName>k__BackingField, other.<MessageTypeName>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<PayloadBytes>k__BackingField, other.<PayloadBytes>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<HandlerDescription>k__BackingField, other.<HandlerDescription>k__BackingField);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E39 File Offset: 0x00001039
		[CompilerGenerated]
		public void Deconstruct(out string ChannelName, out int ChannelId, out int MessageId, out string MessageTypeName, out int PayloadBytes, out string HandlerDescription)
		{
			ChannelName = this.ChannelName;
			ChannelId = this.ChannelId;
			MessageId = this.MessageId;
			MessageTypeName = this.MessageTypeName;
			PayloadBytes = this.PayloadBytes;
			HandlerDescription = this.HandlerDescription;
		}
	}
}
