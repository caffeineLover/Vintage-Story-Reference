using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200005E RID: 94
	[ProtoContract(ImplicitFields = 1)]
	public class AnnouncePacket : NetworkPacket
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000F54D File Offset: 0x0000D74D
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000F555 File Offset: 0x0000D755
		public string Title { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000F55E File Offset: 0x0000D75E
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000F566 File Offset: 0x0000D766
		public string Message { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000F56F File Offset: 0x0000D76F
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000F577 File Offset: 0x0000D777
		public double Duration { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000F580 File Offset: 0x0000D780
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000F588 File Offset: 0x0000D788
		public bool ShowBackground { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000F591 File Offset: 0x0000D791
		protected override PacketType Code
		{
			get
			{
				return PacketType.Announce;
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F595 File Offset: 0x0000D795
		public AnnouncePacket()
		{
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F59D File Offset: 0x0000D79D
		public AnnouncePacket(string title, string message, double duration = 5.0, bool showBackground = true)
		{
			this.Title = title;
			this.Message = message;
			this.Duration = duration;
			this.ShowBackground = showBackground;
		}
	}
}
