using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AC RID: 172
	public class RPVoiceChatServerConfig : IModConfig
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001CDCB File Offset: 0x0001AFCB
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001CDD3 File Offset: 0x0001AFD3
		public int ServerPort { get; set; } = 52525;

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001CDDC File Offset: 0x0001AFDC
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001CDE4 File Offset: 0x0001AFE4
		public string ServerIP { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001CDED File Offset: 0x0001AFED
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001CDF5 File Offset: 0x0001AFF5
		public bool UseCustomNetworkServers { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001CDFE File Offset: 0x0001AFFE
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x0001CE06 File Offset: 0x0001B006
		public bool ManualPortForwarding { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001CE0F File Offset: 0x0001B00F
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x0001CE17 File Offset: 0x0001B017
		public int MaxConnectionAttempts { get; set; } = 5;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001CE20 File Offset: 0x0001B020
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x0001CE28 File Offset: 0x0001B028
		public bool AdditionalContent { get; set; } = true;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001CE31 File Offset: 0x0001B031
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x0001CE39 File Offset: 0x0001B039
		public bool TechnologyContent { get; set; } = true;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001CE42 File Offset: 0x0001B042
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x0001CE4A File Offset: 0x0001B04A
		public bool TelegraphContent
		{
			get
			{
				return this.TechnologyContent;
			}
			set
			{
				this.TechnologyContent = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001CE53 File Offset: 0x0001B053
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x0001CE5B File Offset: 0x0001B05B
		public float MaxAudioGain { get; set; } = 2f;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001CE64 File Offset: 0x0001B064
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0001CE6C File Offset: 0x0001B06C
		public float MaxVolumeLimit { get; set; } = 0.8f;

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001CE75 File Offset: 0x0001B075
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x0001CE7D File Offset: 0x0001B07D
		public int GridCellSizeBlocks { get; set; } = 64;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0001CE86 File Offset: 0x0001B086
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x0001CE8E File Offset: 0x0001B08E
		public int NormalBitrate { get; set; } = 40960;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001CE97 File Offset: 0x0001B097
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x0001CE9F File Offset: 0x0001B09F
		public int BroadcastBitrate { get; set; } = 16384;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x0001CEB0 File Offset: 0x0001B0B0
		public int TelegraphMaxConnectionDistance { get; set; } = 20;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001CEB9 File Offset: 0x0001B0B9
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x0001CEC1 File Offset: 0x0001B0C1
		public int TelegraphMaxConnectionsPerNode { get; set; } = 4;

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001CECA File Offset: 0x0001B0CA
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x0001CED2 File Offset: 0x0001B0D2
		public int TelegraphMaxMessageLength { get; set; } = 100;

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001CEDB File Offset: 0x0001B0DB
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x0001CEE3 File Offset: 0x0001B0E3
		public int TelegraphMinDelayBetweenKeysMs { get; set; } = 200;

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001CEEC File Offset: 0x0001B0EC
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x0001CEF4 File Offset: 0x0001B0F4
		public double BellRingCooldownSeconds { get; set; } = 1.5;

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001CEFD File Offset: 0x0001B0FD
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x0001CF05 File Offset: 0x0001B105
		public bool TelegraphGenuineMorseCharacters { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001CF0E File Offset: 0x0001B10E
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x0001CF16 File Offset: 0x0001B116
		public int TelegraphMessageDeletionDelaySeconds { get; set; } = 10;

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001CF1F File Offset: 0x0001B11F
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x0001CF27 File Offset: 0x0001B127
		public int TelegraphNetworkMinPowerPercent { get; set; } = 10;

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001CF30 File Offset: 0x0001B130
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x0001CF38 File Offset: 0x0001B138
		public int TelegraphNetworkMaxEndpoints { get; set; } = 8;

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001CF41 File Offset: 0x0001B141
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x0001CF49 File Offset: 0x0001B149
		public int TelephoneNetworkMinPowerPercent { get; set; } = 25;

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001CF52 File Offset: 0x0001B152
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x0001CF5A File Offset: 0x0001B15A
		public int TelephoneNetworkMaxEndpoints { get; set; } = 16;

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001CF63 File Offset: 0x0001B163
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x0001CF6B File Offset: 0x0001B16B
		public int RadioNetworkMinPowerPercent { get; set; } = 50;

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001CF74 File Offset: 0x0001B174
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x0001CF7C File Offset: 0x0001B17C
		public int RadioNetworkMaxEndpoints { get; set; } = 16;

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0001CF85 File Offset: 0x0001B185
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001CF8D File Offset: 0x0001B18D
		public int RadioEmitterBaseRangeBlocks { get; set; } = 100;

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001CF96 File Offset: 0x0001B196
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0001CF9E File Offset: 0x0001B19E
		public int RadioAntennaPartRangeBonusBlocks { get; set; } = 50;

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001CFA7 File Offset: 0x0001B1A7
		// (set) Token: 0x06000546 RID: 1350 RVA: 0x0001CFAF File Offset: 0x0001B1AF
		public int RadioMicrophoneCaptureDistance { get; set; } = 2;

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001CFB8 File Offset: 0x0001B1B8
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
		public int RadioTalkieRangeBlocks { get; set; } = 32;

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001CFC9 File Offset: 0x0001B1C9
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0001CFD1 File Offset: 0x0001B1D1
		public int RadioReceiverRangeBlocks { get; set; } = 64;

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001CFDA File Offset: 0x0001B1DA
		// (set) Token: 0x0600054C RID: 1356 RVA: 0x0001CFE2 File Offset: 0x0001B1E2
		public int RadioReceiverMaxWiredSpeakers { get; set; } = 4;

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001CFEB File Offset: 0x0001B1EB
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x0001CFF3 File Offset: 0x0001B1F3
		public int TelephoneBroadcastMaxSpeakers { get; set; } = 8;

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001CFFC File Offset: 0x0001B1FC
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x0001D004 File Offset: 0x0001B204
		public int HandbellAudibleDistance { get; set; } = 16;

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0001D00D File Offset: 0x0001B20D
		// (set) Token: 0x06000552 RID: 1362 RVA: 0x0001D015 File Offset: 0x0001B215
		public int CallbellAudibleDistance { get; set; } = 16;

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001D01E File Offset: 0x0001B21E
		// (set) Token: 0x06000554 RID: 1364 RVA: 0x0001D026 File Offset: 0x0001B226
		public int RoyalhornAudibleDistance { get; set; } = 72;

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001D02F File Offset: 0x0001B22F
		// (set) Token: 0x06000556 RID: 1366 RVA: 0x0001D037 File Offset: 0x0001B237
		public int WarhornAudibleDistance { get; set; } = 128;

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001D040 File Offset: 0x0001B240
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x0001D048 File Offset: 0x0001B248
		public int CarillonbellAudibleDistance { get; set; } = 256;

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0001D051 File Offset: 0x0001B251
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x0001D059 File Offset: 0x0001B259
		public int ChurchbellAudibleDistance { get; set; } = 832;

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001D062 File Offset: 0x0001B262
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x0001D06A File Offset: 0x0001B26A
		public int MegaphoneAudibleDistance { get; set; } = 125;

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001D073 File Offset: 0x0001B273
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x0001D07B File Offset: 0x0001B27B
		public int TelephoneAudibleDistance { get; set; } = 2;

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001D084 File Offset: 0x0001B284
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x0001D08C File Offset: 0x0001B28C
		public int SpeakerAudibleDistance { get; set; } = 30;

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001D095 File Offset: 0x0001B295
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x0001D09D File Offset: 0x0001B29D
		public int NametagFallbackRenderRange { get; set; } = 99;

		// Token: 0x06000563 RID: 1379 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
		public RPVoiceChatServerConfig()
		{
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001D204 File Offset: 0x0001B404
		public RPVoiceChatServerConfig(ICoreAPI api, RPVoiceChatServerConfig previousConfig = null)
		{
			if (previousConfig == null)
			{
				return;
			}
			this.ServerPort = previousConfig.ServerPort;
			this.ServerIP = previousConfig.ServerIP;
			this.UseCustomNetworkServers = previousConfig.UseCustomNetworkServers;
			this.ManualPortForwarding = previousConfig.ManualPortForwarding;
			this.MaxConnectionAttempts = previousConfig.MaxConnectionAttempts;
			this.AdditionalContent = previousConfig.AdditionalContent;
			this.TechnologyContent = previousConfig.TechnologyContent;
			this.MaxAudioGain = previousConfig.MaxAudioGain;
			this.MaxVolumeLimit = previousConfig.MaxVolumeLimit;
			this.GridCellSizeBlocks = previousConfig.GridCellSizeBlocks;
			this.NormalBitrate = previousConfig.NormalBitrate;
			this.BroadcastBitrate = previousConfig.BroadcastBitrate;
			this.TelegraphMaxConnectionDistance = previousConfig.TelegraphMaxConnectionDistance;
			this.TelegraphMaxConnectionsPerNode = previousConfig.TelegraphMaxConnectionsPerNode;
			this.TelegraphMaxMessageLength = previousConfig.TelegraphMaxMessageLength;
			this.TelegraphMinDelayBetweenKeysMs = previousConfig.TelegraphMinDelayBetweenKeysMs;
			this.BellRingCooldownSeconds = previousConfig.BellRingCooldownSeconds;
			this.TelegraphGenuineMorseCharacters = previousConfig.TelegraphGenuineMorseCharacters;
			this.TelegraphMessageDeletionDelaySeconds = previousConfig.TelegraphMessageDeletionDelaySeconds;
			this.TelegraphNetworkMinPowerPercent = previousConfig.TelegraphNetworkMinPowerPercent;
			this.TelegraphNetworkMaxEndpoints = previousConfig.TelegraphNetworkMaxEndpoints;
			this.TelephoneNetworkMinPowerPercent = previousConfig.TelephoneNetworkMinPowerPercent;
			this.TelephoneNetworkMaxEndpoints = previousConfig.TelephoneNetworkMaxEndpoints;
			this.RadioNetworkMinPowerPercent = previousConfig.RadioNetworkMinPowerPercent;
			this.RadioNetworkMaxEndpoints = previousConfig.RadioNetworkMaxEndpoints;
			this.RadioEmitterBaseRangeBlocks = previousConfig.RadioEmitterBaseRangeBlocks;
			this.RadioAntennaPartRangeBonusBlocks = previousConfig.RadioAntennaPartRangeBonusBlocks;
			this.RadioMicrophoneCaptureDistance = previousConfig.RadioMicrophoneCaptureDistance;
			this.RadioTalkieRangeBlocks = previousConfig.RadioTalkieRangeBlocks;
			this.RadioReceiverRangeBlocks = previousConfig.RadioReceiverRangeBlocks;
			this.RadioReceiverMaxWiredSpeakers = previousConfig.RadioReceiverMaxWiredSpeakers;
			this.TelephoneBroadcastMaxSpeakers = previousConfig.TelephoneBroadcastMaxSpeakers;
			this.HandbellAudibleDistance = previousConfig.HandbellAudibleDistance;
			this.CallbellAudibleDistance = previousConfig.CallbellAudibleDistance;
			this.RoyalhornAudibleDistance = previousConfig.RoyalhornAudibleDistance;
			this.WarhornAudibleDistance = previousConfig.WarhornAudibleDistance;
			this.CarillonbellAudibleDistance = previousConfig.CarillonbellAudibleDistance;
			this.ChurchbellAudibleDistance = previousConfig.ChurchbellAudibleDistance;
			this.MegaphoneAudibleDistance = previousConfig.MegaphoneAudibleDistance;
			this.TelephoneAudibleDistance = previousConfig.TelephoneAudibleDistance;
			this.SpeakerAudibleDistance = previousConfig.SpeakerAudibleDistance;
			this.NametagFallbackRenderRange = previousConfig.NametagFallbackRenderRange;
		}
	}
}
