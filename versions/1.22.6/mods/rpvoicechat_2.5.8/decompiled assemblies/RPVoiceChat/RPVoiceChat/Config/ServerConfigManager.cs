using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.Util;

namespace RPVoiceChat.Config
{
	// Token: 0x02000084 RID: 132
	public static class ServerConfigManager
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00017393 File Offset: 0x00015593
		public static RPVoiceChatServerConfig Config
		{
			get
			{
				return ModConfig.ServerConfig;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001739A File Offset: 0x0001559A
		public static float MaxAudioGain
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 2f;
				}
				return config.MaxAudioGain;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000173B0 File Offset: 0x000155B0
		public static float MaxVolumeLimit
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 0.8f;
				}
				return config.MaxVolumeLimit;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000173C6 File Offset: 0x000155C6
		public static int GridCellSizeBlocks
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 64;
				}
				return config.GridCellSizeBlocks;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000173D9 File Offset: 0x000155D9
		public static int NormalBitrate
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 40960;
				}
				return config.NormalBitrate;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000173EF File Offset: 0x000155EF
		public static int BroadcastBitrate
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 16384;
				}
				return config.BroadcastBitrate;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00017405 File Offset: 0x00015605
		public static int MaxConnectionAttempts
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 5;
				}
				return config.MaxConnectionAttempts;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00017417 File Offset: 0x00015617
		public static int TelegraphMaxConnectionDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 20;
				}
				return config.TelegraphMaxConnectionDistance;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001742A File Offset: 0x0001562A
		public static int TelegraphMaxConnectionsPerNode
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 4;
				}
				return config.TelegraphMaxConnectionsPerNode;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0001743C File Offset: 0x0001563C
		public static int TelegraphMaxMessageLength
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 100;
				}
				return config.TelegraphMaxMessageLength;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001744F File Offset: 0x0001564F
		public static int TelegraphMinDelayBetweenKeysMs
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 200;
				}
				return config.TelegraphMinDelayBetweenKeysMs;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00017465 File Offset: 0x00015665
		public static double BellRingCooldownSeconds
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 1.5;
				}
				return config.BellRingCooldownSeconds;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001747F File Offset: 0x0001567F
		public static bool TelegraphGenuineMorseCharacters
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				return config != null && config.TelegraphGenuineMorseCharacters;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00017491 File Offset: 0x00015691
		public static int TelegraphMessageDeletionDelaySeconds
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 10;
				}
				return config.TelegraphMessageDeletionDelaySeconds;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x000174A4 File Offset: 0x000156A4
		public static int TelegraphNetworkMinPowerPercent
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 10;
				}
				return config.TelegraphNetworkMinPowerPercent;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000174B7 File Offset: 0x000156B7
		public static int TelegraphNetworkMaxEndpoints
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 8;
				}
				return config.TelegraphNetworkMaxEndpoints;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x000174C9 File Offset: 0x000156C9
		public static int TelephoneNetworkMinPowerPercent
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 25;
				}
				return config.TelephoneNetworkMinPowerPercent;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x000174DC File Offset: 0x000156DC
		public static int TelephoneNetworkMaxEndpoints
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 16;
				}
				return config.TelephoneNetworkMaxEndpoints;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000174EF File Offset: 0x000156EF
		public static int RadioNetworkMinPowerPercent
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 50;
				}
				return config.RadioNetworkMinPowerPercent;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00017502 File Offset: 0x00015702
		public static int RadioNetworkMaxEndpoints
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 16;
				}
				return config.RadioNetworkMaxEndpoints;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00017515 File Offset: 0x00015715
		public static int HandbellAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 16;
				}
				return config.HandbellAudibleDistance;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00017528 File Offset: 0x00015728
		public static int CallbellAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 16;
				}
				return config.CallbellAudibleDistance;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0001753B File Offset: 0x0001573B
		public static int RoyalhornAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 72;
				}
				return config.RoyalhornAudibleDistance;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001754E File Offset: 0x0001574E
		public static int WarhornAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 128;
				}
				return config.WarhornAudibleDistance;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00017564 File Offset: 0x00015764
		public static int CarillonbellAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 256;
				}
				return config.CarillonbellAudibleDistance;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001757A File Offset: 0x0001577A
		public static int ChurchbellAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 832;
				}
				return config.ChurchbellAudibleDistance;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00017590 File Offset: 0x00015790
		public static int MegaphoneAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 125;
				}
				return config.MegaphoneAudibleDistance;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x000175A3 File Offset: 0x000157A3
		public static int TelephoneAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 2;
				}
				return config.TelephoneAudibleDistance;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x000175B5 File Offset: 0x000157B5
		public static int SpeakerAudibleDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 30;
				}
				return config.SpeakerAudibleDistance;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000175C8 File Offset: 0x000157C8
		public static int NametagFallbackRenderRange
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 99;
				}
				return config.NametagFallbackRenderRange;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000175DC File Offset: 0x000157DC
		public static void ValidateConfig()
		{
			if (ServerConfigManager.Config == null)
			{
				return;
			}
			if (ServerConfigManager.MaxAudioGain < 0.1f || ServerConfigManager.MaxAudioGain > 10f)
			{
				Logger server = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("MaxAudioGain (");
				defaultInterpolatedStringHandler.AppendFormatted<float>(ServerConfigManager.MaxAudioGain);
				defaultInterpolatedStringHandler.AppendLiteral(") should be between 0.1 and 10. Using default (2.0).");
				server.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				ServerConfigManager.Config.MaxAudioGain = 2f;
			}
			if (ServerConfigManager.MaxVolumeLimit < 0.1f || ServerConfigManager.MaxVolumeLimit > 1f)
			{
				Logger server2 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(69, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("MaxVolumeLimit (");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(ServerConfigManager.MaxVolumeLimit);
				defaultInterpolatedStringHandler2.AppendLiteral(") should be between 0.1 and 1.0. Using default (0.8).");
				server2.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
				ServerConfigManager.Config.MaxVolumeLimit = 0.8f;
			}
			if (ServerConfigManager.GridCellSizeBlocks < 8 || ServerConfigManager.GridCellSizeBlocks > 512)
			{
				Logger server3 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(70, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("GridCellSizeBlocks (");
				defaultInterpolatedStringHandler3.AppendFormatted<int>(ServerConfigManager.GridCellSizeBlocks);
				defaultInterpolatedStringHandler3.AppendLiteral(") should be between 8 and 512. Using default (64).");
				server3.Warning(defaultInterpolatedStringHandler3.ToStringAndClear());
				ServerConfigManager.Config.GridCellSizeBlocks = 64;
			}
			else if ((ServerConfigManager.GridCellSizeBlocks & ServerConfigManager.GridCellSizeBlocks - 1) != 0)
			{
				int powerOfTwo;
				for (powerOfTwo = 1; powerOfTwo < ServerConfigManager.GridCellSizeBlocks; powerOfTwo <<= 1)
				{
				}
				int adjusted = (powerOfTwo > 512) ? 512 : powerOfTwo;
				Logger server4 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(55, 2);
				defaultInterpolatedStringHandler4.AppendLiteral("GridCellSizeBlocks (");
				defaultInterpolatedStringHandler4.AppendFormatted<int>(ServerConfigManager.GridCellSizeBlocks);
				defaultInterpolatedStringHandler4.AppendLiteral(") should be a power of two. Using ");
				defaultInterpolatedStringHandler4.AppendFormatted<int>(adjusted);
				defaultInterpolatedStringHandler4.AppendLiteral(".");
				server4.Warning(defaultInterpolatedStringHandler4.ToStringAndClear());
				ServerConfigManager.Config.GridCellSizeBlocks = adjusted;
			}
			if (ServerConfigManager.NormalBitrate < 8192 || ServerConfigManager.NormalBitrate > 131072)
			{
				Logger server5 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5;
				defaultInterpolatedStringHandler5..ctor(75, 1);
				defaultInterpolatedStringHandler5.AppendLiteral("NormalBitrate (");
				defaultInterpolatedStringHandler5.AppendFormatted<int>(ServerConfigManager.NormalBitrate);
				defaultInterpolatedStringHandler5.AppendLiteral(") should be between 8 and 128 kbps. Using default (40 kbps).");
				server5.Warning(defaultInterpolatedStringHandler5.ToStringAndClear());
				ServerConfigManager.Config.NormalBitrate = 40960;
			}
			if (ServerConfigManager.BroadcastBitrate < 4096 || ServerConfigManager.BroadcastBitrate > 65536)
			{
				Logger server6 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6;
				defaultInterpolatedStringHandler6..ctor(77, 1);
				defaultInterpolatedStringHandler6.AppendLiteral("BroadcastBitrate (");
				defaultInterpolatedStringHandler6.AppendFormatted<int>(ServerConfigManager.BroadcastBitrate);
				defaultInterpolatedStringHandler6.AppendLiteral(") should be between 4 and 64 kbps. Using default (16 kbps).");
				server6.Warning(defaultInterpolatedStringHandler6.ToStringAndClear());
				ServerConfigManager.Config.BroadcastBitrate = 16384;
			}
			if (ServerConfigManager.TelegraphMaxConnectionDistance < 5 || ServerConfigManager.TelegraphMaxConnectionDistance > 100)
			{
				Logger server7 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7;
				defaultInterpolatedStringHandler7..ctor(82, 1);
				defaultInterpolatedStringHandler7.AppendLiteral("TelegraphMaxConnectionDistance (");
				defaultInterpolatedStringHandler7.AppendFormatted<int>(ServerConfigManager.TelegraphMaxConnectionDistance);
				defaultInterpolatedStringHandler7.AppendLiteral(") should be between 5 and 100. Using default (20).");
				server7.Warning(defaultInterpolatedStringHandler7.ToStringAndClear());
				ServerConfigManager.Config.TelegraphMaxConnectionDistance = 20;
			}
			if (ServerConfigManager.TelegraphMaxConnectionsPerNode < 1 || ServerConfigManager.TelegraphMaxConnectionsPerNode > 10)
			{
				Logger server8 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8;
				defaultInterpolatedStringHandler8..ctor(80, 1);
				defaultInterpolatedStringHandler8.AppendLiteral("TelegraphMaxConnectionsPerNode (");
				defaultInterpolatedStringHandler8.AppendFormatted<int>(ServerConfigManager.TelegraphMaxConnectionsPerNode);
				defaultInterpolatedStringHandler8.AppendLiteral(") should be between 1 and 10. Using default (4).");
				server8.Warning(defaultInterpolatedStringHandler8.ToStringAndClear());
				ServerConfigManager.Config.TelegraphMaxConnectionsPerNode = 4;
			}
			if (ServerConfigManager.TelegraphMaxMessageLength < 10 || ServerConfigManager.TelegraphMaxMessageLength > 1000)
			{
				Logger server9 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9;
				defaultInterpolatedStringHandler9..ctor(80, 1);
				defaultInterpolatedStringHandler9.AppendLiteral("TelegraphMaxMessageLength (");
				defaultInterpolatedStringHandler9.AppendFormatted<int>(ServerConfigManager.TelegraphMaxMessageLength);
				defaultInterpolatedStringHandler9.AppendLiteral(") should be between 10 and 1000. Using default (100).");
				server9.Warning(defaultInterpolatedStringHandler9.ToStringAndClear());
				ServerConfigManager.Config.TelegraphMaxMessageLength = 100;
			}
			if (ServerConfigManager.BellRingCooldownSeconds < 0.1 || ServerConfigManager.BellRingCooldownSeconds > 10.0)
			{
				Logger server10 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler10;
				defaultInterpolatedStringHandler10..ctor(79, 1);
				defaultInterpolatedStringHandler10.AppendLiteral("BellRingCooldownSeconds (");
				defaultInterpolatedStringHandler10.AppendFormatted<double>(ServerConfigManager.BellRingCooldownSeconds);
				defaultInterpolatedStringHandler10.AppendLiteral(") should be between 0.1 and 10.0. Using default (1.5).");
				server10.Warning(defaultInterpolatedStringHandler10.ToStringAndClear());
				ServerConfigManager.Config.BellRingCooldownSeconds = 1.5;
			}
			if (ServerConfigManager.TelegraphMessageDeletionDelaySeconds < 1 || ServerConfigManager.TelegraphMessageDeletionDelaySeconds > 300)
			{
				Logger server11 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler11;
				defaultInterpolatedStringHandler11..ctor(88, 1);
				defaultInterpolatedStringHandler11.AppendLiteral("TelegraphMessageDeletionDelaySeconds (");
				defaultInterpolatedStringHandler11.AppendFormatted<int>(ServerConfigManager.TelegraphMessageDeletionDelaySeconds);
				defaultInterpolatedStringHandler11.AppendLiteral(") should be between 1 and 300. Using default (10).");
				server11.Warning(defaultInterpolatedStringHandler11.ToStringAndClear());
				ServerConfigManager.Config.TelegraphMessageDeletionDelaySeconds = 10;
			}
			if (ServerConfigManager.TelegraphNetworkMinPowerPercent < 0 || ServerConfigManager.TelegraphNetworkMinPowerPercent > 100)
			{
				Logger server12 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler12;
				defaultInterpolatedStringHandler12..ctor(83, 1);
				defaultInterpolatedStringHandler12.AppendLiteral("TelegraphNetworkMinPowerPercent (");
				defaultInterpolatedStringHandler12.AppendFormatted<int>(ServerConfigManager.TelegraphNetworkMinPowerPercent);
				defaultInterpolatedStringHandler12.AppendLiteral(") should be between 0 and 100. Using default (10).");
				server12.Warning(defaultInterpolatedStringHandler12.ToStringAndClear());
				ServerConfigManager.Config.TelegraphNetworkMinPowerPercent = 10;
			}
			if (ServerConfigManager.TelephoneNetworkMinPowerPercent < 0 || ServerConfigManager.TelephoneNetworkMinPowerPercent > 100)
			{
				Logger server13 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler13;
				defaultInterpolatedStringHandler13..ctor(83, 1);
				defaultInterpolatedStringHandler13.AppendLiteral("TelephoneNetworkMinPowerPercent (");
				defaultInterpolatedStringHandler13.AppendFormatted<int>(ServerConfigManager.TelephoneNetworkMinPowerPercent);
				defaultInterpolatedStringHandler13.AppendLiteral(") should be between 0 and 100. Using default (25).");
				server13.Warning(defaultInterpolatedStringHandler13.ToStringAndClear());
				ServerConfigManager.Config.TelephoneNetworkMinPowerPercent = 25;
			}
			if (ServerConfigManager.RadioNetworkMinPowerPercent < 0 || ServerConfigManager.RadioNetworkMinPowerPercent > 100)
			{
				Logger server14 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler14;
				defaultInterpolatedStringHandler14..ctor(79, 1);
				defaultInterpolatedStringHandler14.AppendLiteral("RadioNetworkMinPowerPercent (");
				defaultInterpolatedStringHandler14.AppendFormatted<int>(ServerConfigManager.RadioNetworkMinPowerPercent);
				defaultInterpolatedStringHandler14.AppendLiteral(") should be between 0 and 100. Using default (50).");
				server14.Warning(defaultInterpolatedStringHandler14.ToStringAndClear());
				ServerConfigManager.Config.RadioNetworkMinPowerPercent = 50;
			}
			if (ServerConfigManager.TelegraphNetworkMaxEndpoints < 1 || ServerConfigManager.TelegraphNetworkMaxEndpoints > 256)
			{
				Logger server15 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler15;
				defaultInterpolatedStringHandler15..ctor(79, 1);
				defaultInterpolatedStringHandler15.AppendLiteral("TelegraphNetworkMaxEndpoints (");
				defaultInterpolatedStringHandler15.AppendFormatted<int>(ServerConfigManager.TelegraphNetworkMaxEndpoints);
				defaultInterpolatedStringHandler15.AppendLiteral(") should be between 1 and 256. Using default (8).");
				server15.Warning(defaultInterpolatedStringHandler15.ToStringAndClear());
				ServerConfigManager.Config.TelegraphNetworkMaxEndpoints = 8;
			}
			if (ServerConfigManager.TelephoneNetworkMaxEndpoints < 1 || ServerConfigManager.TelephoneNetworkMaxEndpoints > 256)
			{
				Logger server16 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler16;
				defaultInterpolatedStringHandler16..ctor(80, 1);
				defaultInterpolatedStringHandler16.AppendLiteral("TelephoneNetworkMaxEndpoints (");
				defaultInterpolatedStringHandler16.AppendFormatted<int>(ServerConfigManager.TelephoneNetworkMaxEndpoints);
				defaultInterpolatedStringHandler16.AppendLiteral(") should be between 1 and 256. Using default (16).");
				server16.Warning(defaultInterpolatedStringHandler16.ToStringAndClear());
				ServerConfigManager.Config.TelephoneNetworkMaxEndpoints = 16;
			}
			if (ServerConfigManager.RadioNetworkMaxEndpoints < 1 || ServerConfigManager.RadioNetworkMaxEndpoints > 256)
			{
				Logger server17 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler17;
				defaultInterpolatedStringHandler17..ctor(76, 1);
				defaultInterpolatedStringHandler17.AppendLiteral("RadioNetworkMaxEndpoints (");
				defaultInterpolatedStringHandler17.AppendFormatted<int>(ServerConfigManager.RadioNetworkMaxEndpoints);
				defaultInterpolatedStringHandler17.AppendLiteral(") should be between 1 and 256. Using default (16).");
				server17.Warning(defaultInterpolatedStringHandler17.ToStringAndClear());
				ServerConfigManager.Config.RadioNetworkMaxEndpoints = 16;
			}
			if (ServerConfigManager.HandbellAudibleDistance < 1 || ServerConfigManager.HandbellAudibleDistance > 1000)
			{
				Logger server18 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler18;
				defaultInterpolatedStringHandler18..ctor(76, 1);
				defaultInterpolatedStringHandler18.AppendLiteral("HandbellAudibleDistance (");
				defaultInterpolatedStringHandler18.AppendFormatted<int>(ServerConfigManager.HandbellAudibleDistance);
				defaultInterpolatedStringHandler18.AppendLiteral(") should be between 1 and 1000. Using default (16).");
				server18.Warning(defaultInterpolatedStringHandler18.ToStringAndClear());
				ServerConfigManager.Config.HandbellAudibleDistance = 16;
			}
			if (ServerConfigManager.CallbellAudibleDistance < 1 || ServerConfigManager.CallbellAudibleDistance > 1000)
			{
				Logger server19 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler19;
				defaultInterpolatedStringHandler19..ctor(76, 1);
				defaultInterpolatedStringHandler19.AppendLiteral("CallbellAudibleDistance (");
				defaultInterpolatedStringHandler19.AppendFormatted<int>(ServerConfigManager.CallbellAudibleDistance);
				defaultInterpolatedStringHandler19.AppendLiteral(") should be between 1 and 1000. Using default (16).");
				server19.Warning(defaultInterpolatedStringHandler19.ToStringAndClear());
				ServerConfigManager.Config.CallbellAudibleDistance = 16;
			}
			if (ServerConfigManager.RoyalhornAudibleDistance < 1 || ServerConfigManager.RoyalhornAudibleDistance > 1000)
			{
				Logger server20 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler20;
				defaultInterpolatedStringHandler20..ctor(77, 1);
				defaultInterpolatedStringHandler20.AppendLiteral("RoyalhornAudibleDistance (");
				defaultInterpolatedStringHandler20.AppendFormatted<int>(ServerConfigManager.RoyalhornAudibleDistance);
				defaultInterpolatedStringHandler20.AppendLiteral(") should be between 1 and 1000. Using default (72).");
				server20.Warning(defaultInterpolatedStringHandler20.ToStringAndClear());
				ServerConfigManager.Config.RoyalhornAudibleDistance = 72;
			}
			if (ServerConfigManager.WarhornAudibleDistance < 1 || ServerConfigManager.WarhornAudibleDistance > 1000)
			{
				Logger server21 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler21;
				defaultInterpolatedStringHandler21..ctor(76, 1);
				defaultInterpolatedStringHandler21.AppendLiteral("WarhornAudibleDistance (");
				defaultInterpolatedStringHandler21.AppendFormatted<int>(ServerConfigManager.WarhornAudibleDistance);
				defaultInterpolatedStringHandler21.AppendLiteral(") should be between 1 and 1000. Using default (128).");
				server21.Warning(defaultInterpolatedStringHandler21.ToStringAndClear());
				ServerConfigManager.Config.WarhornAudibleDistance = 128;
			}
			if (ServerConfigManager.CarillonbellAudibleDistance < 1 || ServerConfigManager.CarillonbellAudibleDistance > 1000)
			{
				Logger server22 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler22;
				defaultInterpolatedStringHandler22..ctor(81, 1);
				defaultInterpolatedStringHandler22.AppendLiteral("CarillonbellAudibleDistance (");
				defaultInterpolatedStringHandler22.AppendFormatted<int>(ServerConfigManager.CarillonbellAudibleDistance);
				defaultInterpolatedStringHandler22.AppendLiteral(") should be between 1 and 1000. Using default (256).");
				server22.Warning(defaultInterpolatedStringHandler22.ToStringAndClear());
				ServerConfigManager.Config.CarillonbellAudibleDistance = 256;
			}
			if (ServerConfigManager.ChurchbellAudibleDistance < 1 || ServerConfigManager.ChurchbellAudibleDistance > 1000)
			{
				Logger server23 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler23;
				defaultInterpolatedStringHandler23..ctor(79, 1);
				defaultInterpolatedStringHandler23.AppendLiteral("ChurchbellAudibleDistance (");
				defaultInterpolatedStringHandler23.AppendFormatted<int>(ServerConfigManager.ChurchbellAudibleDistance);
				defaultInterpolatedStringHandler23.AppendLiteral(") should be between 1 and 1000. Using default (832).");
				server23.Warning(defaultInterpolatedStringHandler23.ToStringAndClear());
				ServerConfigManager.Config.ChurchbellAudibleDistance = 832;
			}
			if (ServerConfigManager.MegaphoneAudibleDistance < 1 || ServerConfigManager.MegaphoneAudibleDistance > 1000)
			{
				Logger server24 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler24;
				defaultInterpolatedStringHandler24..ctor(78, 1);
				defaultInterpolatedStringHandler24.AppendLiteral("MegaphoneAudibleDistance (");
				defaultInterpolatedStringHandler24.AppendFormatted<int>(ServerConfigManager.MegaphoneAudibleDistance);
				defaultInterpolatedStringHandler24.AppendLiteral(") should be between 1 and 1000. Using default (125).");
				server24.Warning(defaultInterpolatedStringHandler24.ToStringAndClear());
				ServerConfigManager.Config.MegaphoneAudibleDistance = 125;
			}
			if (ServerConfigManager.TelephoneAudibleDistance < 1 || ServerConfigManager.TelephoneAudibleDistance > 1000)
			{
				Logger server25 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler25;
				defaultInterpolatedStringHandler25..ctor(76, 1);
				defaultInterpolatedStringHandler25.AppendLiteral("TelephoneAudibleDistance (");
				defaultInterpolatedStringHandler25.AppendFormatted<int>(ServerConfigManager.TelephoneAudibleDistance);
				defaultInterpolatedStringHandler25.AppendLiteral(") should be between 1 and 1000. Using default (2).");
				server25.Warning(defaultInterpolatedStringHandler25.ToStringAndClear());
				ServerConfigManager.Config.TelephoneAudibleDistance = 2;
			}
			if (ServerConfigManager.SpeakerAudibleDistance < 1 || ServerConfigManager.SpeakerAudibleDistance > 1000)
			{
				Logger server26 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler26;
				defaultInterpolatedStringHandler26..ctor(75, 1);
				defaultInterpolatedStringHandler26.AppendLiteral("SpeakerAudibleDistance (");
				defaultInterpolatedStringHandler26.AppendFormatted<int>(ServerConfigManager.SpeakerAudibleDistance);
				defaultInterpolatedStringHandler26.AppendLiteral(") should be between 1 and 1000. Using default (30).");
				server26.Warning(defaultInterpolatedStringHandler26.ToStringAndClear());
				ServerConfigManager.Config.SpeakerAudibleDistance = 30;
			}
			if (ServerConfigManager.NametagFallbackRenderRange < 1 || ServerConfigManager.NametagFallbackRenderRange > 1000)
			{
				Logger server27 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler27;
				defaultInterpolatedStringHandler27..ctor(79, 1);
				defaultInterpolatedStringHandler27.AppendLiteral("NametagFallbackRenderRange (");
				defaultInterpolatedStringHandler27.AppendFormatted<int>(ServerConfigManager.NametagFallbackRenderRange);
				defaultInterpolatedStringHandler27.AppendLiteral(") should be between 1 and 1000. Using default (99).");
				server27.Warning(defaultInterpolatedStringHandler27.ToStringAndClear());
				ServerConfigManager.Config.NametagFallbackRenderRange = 99;
			}
		}
	}
}
