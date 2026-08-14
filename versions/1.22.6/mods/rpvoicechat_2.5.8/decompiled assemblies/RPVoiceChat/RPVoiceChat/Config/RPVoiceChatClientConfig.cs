using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x02000082 RID: 130
	public class RPVoiceChatClientConfig : IModConfig
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000169E8 File Offset: 0x00014BE8
		// (set) Token: 0x060003DE RID: 990 RVA: 0x000169F0 File Offset: 0x00014BF0
		public float OutputVoice { get; set; } = 1f;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000169F9 File Offset: 0x00014BF9
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00016A01 File Offset: 0x00014C01
		public float OutputBlock { get; set; } = 0.6f;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00016A0A File Offset: 0x00014C0A
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00016A12 File Offset: 0x00014C12
		public float OutputItem { get; set; } = 0.6f;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00016A1B File Offset: 0x00014C1B
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x00016A23 File Offset: 0x00014C23
		public float InputGain { get; set; } = 1f;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00016A2C File Offset: 0x00014C2C
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00016A34 File Offset: 0x00014C34
		public float InputThreshold { get; set; } = 0.36f;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00016A3D File Offset: 0x00014C3D
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00016A45 File Offset: 0x00014C45
		public string InputDevice { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00016A4E File Offset: 0x00014C4E
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00016A56 File Offset: 0x00014C56
		public bool Denoising { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00016A5F File Offset: 0x00014C5F
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x00016A67 File Offset: 0x00014C67
		public float DenoisingSensitivity { get; set; } = 0.5f;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00016A70 File Offset: 0x00014C70
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x00016A78 File Offset: 0x00014C78
		public float DenoisingStrength { get; set; } = 0.1f;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00016A81 File Offset: 0x00014C81
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x00016A89 File Offset: 0x00014C89
		public bool ShowHud { get; set; } = true;

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00016A92 File Offset: 0x00014C92
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x00016A9A File Offset: 0x00014C9A
		public bool IsMinimalHud { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00016AA3 File Offset: 0x00014CA3
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00016AAB File Offset: 0x00014CAB
		public int ActiveConfigTab { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00016AB4 File Offset: 0x00014CB4
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00016ABC File Offset: 0x00014CBC
		public bool PushToTalkEnabled { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00016AC5 File Offset: 0x00014CC5
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x00016ACD File Offset: 0x00014CCD
		public bool IsMuted { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00016AD6 File Offset: 0x00014CD6
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00016ADE File Offset: 0x00014CDE
		public bool IsMonoMode { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00016AE7 File Offset: 0x00014CE7
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00016AEF File Offset: 0x00014CEF
		public bool Loopback { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00016AF8 File Offset: 0x00014CF8
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00016B00 File Offset: 0x00014D00
		public bool DevicesVoiceFeedback { get; set; } = true;

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00016B09 File Offset: 0x00014D09
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x00016B11 File Offset: 0x00014D11
		public bool Muffling { get; set; } = true;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00016B1A File Offset: 0x00014D1A
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x00016B22 File Offset: 0x00014D22
		public bool ChannelGuessing { get; set; } = true;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00016B2B File Offset: 0x00014D2B
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x00016B33 File Offset: 0x00014D33
		public bool FirstTimeUse { get; set; } = true;

		// Token: 0x06000405 RID: 1029 RVA: 0x00016B3C File Offset: 0x00014D3C
		public RPVoiceChatClientConfig()
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00016BC0 File Offset: 0x00014DC0
		public RPVoiceChatClientConfig(ICoreAPI api, RPVoiceChatClientConfig previousConfig = null)
		{
			if (previousConfig == null)
			{
				return;
			}
			this.OutputVoice = previousConfig.OutputVoice;
			this.OutputBlock = previousConfig.OutputBlock;
			this.OutputItem = previousConfig.OutputItem;
			this.InputGain = previousConfig.InputGain;
			this.InputThreshold = previousConfig.InputThreshold;
			this.InputDevice = previousConfig.InputDevice;
			this.Denoising = previousConfig.Denoising;
			this.DenoisingSensitivity = previousConfig.DenoisingSensitivity;
			this.DenoisingStrength = previousConfig.DenoisingStrength;
			this.ShowHud = previousConfig.ShowHud;
			this.IsMinimalHud = previousConfig.IsMinimalHud;
			this.ActiveConfigTab = previousConfig.ActiveConfigTab;
			this.PushToTalkEnabled = previousConfig.PushToTalkEnabled;
			this.IsMuted = previousConfig.IsMuted;
			this.IsMonoMode = previousConfig.IsMonoMode;
			this.Loopback = previousConfig.Loopback;
			this.DevicesVoiceFeedback = previousConfig.DevicesVoiceFeedback;
			this.Muffling = previousConfig.Muffling;
			this.ChannelGuessing = previousConfig.ChannelGuessing;
			this.FirstTimeUse = previousConfig.FirstTimeUse;
		}
	}
}
