using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200007B RID: 123
	[ProtoContract(ImplicitFields = 1)]
	public class AnnouncePacket : NetworkPacket
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00013019 File Offset: 0x00011219
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00013021 File Offset: 0x00011221
		public string Title { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0001302A File Offset: 0x0001122A
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00013032 File Offset: 0x00011232
		public string Message { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0001303B File Offset: 0x0001123B
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00013043 File Offset: 0x00011243
		public double Duration { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0001304C File Offset: 0x0001124C
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00013054 File Offset: 0x00011254
		public bool ShowBackground { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0001305D File Offset: 0x0001125D
		protected override PacketType Code
		{
			get
			{
				return PacketType.Announce;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00013061 File Offset: 0x00011261
		public AnnouncePacket()
		{
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00013069 File Offset: 0x00011269
		public AnnouncePacket(string title, string message, double duration = 5.0, bool showBackground = true)
		{
			this.Title = title;
			this.Message = message;
			this.Duration = duration;
			this.ShowBackground = showBackground;
		}
	}
}
