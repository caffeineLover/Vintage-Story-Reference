using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AB RID: 171
	public class RPVoiceChatClientConfig : IModConfig
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001CA84 File Offset: 0x0001AC84
		public float OutputVoice { get; set; } = 1f;

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001CA8D File Offset: 0x0001AC8D
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001CA95 File Offset: 0x0001AC95
		public float OutputBlock { get; set; } = 0.6f;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0001CA9E File Offset: 0x0001AC9E
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0001CAA6 File Offset: 0x0001ACA6
		public float OutputItem { get; set; } = 0.6f;

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0001CAAF File Offset: 0x0001ACAF
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0001CAB7 File Offset: 0x0001ACB7
		public float InputGain { get; set; } = 1f;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0001CAC0 File Offset: 0x0001ACC0
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0001CAC8 File Offset: 0x0001ACC8
		public float InputThreshold { get; set; } = 0.36f;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001CAD1 File Offset: 0x0001ACD1
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0001CAD9 File Offset: 0x0001ACD9
		public string InputDevice { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0001CAE2 File Offset: 0x0001ACE2
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0001CAEA File Offset: 0x0001ACEA
		public bool Denoising { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001CAF3 File Offset: 0x0001ACF3
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0001CAFB File Offset: 0x0001ACFB
		public float DenoisingSensitivity { get; set; } = 0.5f;

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001CB04 File Offset: 0x0001AD04
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		public float DenoisingStrength { get; set; } = 0.1f;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001CB15 File Offset: 0x0001AD15
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0001CB1D File Offset: 0x0001AD1D
		public bool ShowHud { get; set; } = true;

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001CB26 File Offset: 0x0001AD26
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0001CB2E File Offset: 0x0001AD2E
		public bool IsMinimalHud { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001CB37 File Offset: 0x0001AD37
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0001CB3F File Offset: 0x0001AD3F
		public int ActiveConfigTab { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001CB48 File Offset: 0x0001AD48
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x0001CB50 File Offset: 0x0001AD50
		public bool PushToTalkEnabled { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001CB59 File Offset: 0x0001AD59
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x0001CB61 File Offset: 0x0001AD61
		public bool IsMuted { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001CB6A File Offset: 0x0001AD6A
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001CB72 File Offset: 0x0001AD72
		public bool IsMonoMode { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001CB7B File Offset: 0x0001AD7B
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0001CB83 File Offset: 0x0001AD83
		public bool Loopback { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001CB8C File Offset: 0x0001AD8C
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x0001CB94 File Offset: 0x0001AD94
		public bool DevicesVoiceFeedback { get; set; } = true;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001CB9D File Offset: 0x0001AD9D
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0001CBA5 File Offset: 0x0001ADA5
		public bool Muffling { get; set; } = true;

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001CBAE File Offset: 0x0001ADAE
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0001CBB6 File Offset: 0x0001ADB6
		public bool ChannelGuessing { get; set; } = true;

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001CBBF File Offset: 0x0001ADBF
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0001CBC7 File Offset: 0x0001ADC7
		public bool FirstTimeUse { get; set; } = true;

		// Token: 0x0600050B RID: 1291 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		public RPVoiceChatClientConfig()
		{
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001CC54 File Offset: 0x0001AE54
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
