using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.Util;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AD RID: 173
	public static class ServerConfigManager
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001D55C File Offset: 0x0001B75C
		public static RPVoiceChatServerConfig Config
		{
			get
			{
				return ModConfig.ServerConfig;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001D563 File Offset: 0x0001B763
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

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001D579 File Offset: 0x0001B779
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0001D58F File Offset: 0x0001B78F
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001D5A2 File Offset: 0x0001B7A2
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001D5B8 File Offset: 0x0001B7B8
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001D5CE File Offset: 0x0001B7CE
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001D5E0 File Offset: 0x0001B7E0
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0001D5F3 File Offset: 0x0001B7F3
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001D605 File Offset: 0x0001B805
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001D618 File Offset: 0x0001B818
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001D62E File Offset: 0x0001B82E
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001D648 File Offset: 0x0001B848
		public static bool TelegraphGenuineMorseCharacters
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				return config != null && config.TelegraphGenuineMorseCharacters;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001D65A File Offset: 0x0001B85A
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

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001D66D File Offset: 0x0001B86D
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001D680 File Offset: 0x0001B880
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001D692 File Offset: 0x0001B892
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001D6A5 File Offset: 0x0001B8A5
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001D6B8 File Offset: 0x0001B8B8
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001D6CB File Offset: 0x0001B8CB
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001D6DE File Offset: 0x0001B8DE
		public static int RadioEmitterBaseRangeBlocks
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 100;
				}
				return config.RadioEmitterBaseRangeBlocks;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001D6F1 File Offset: 0x0001B8F1
		public static int RadioAntennaPartRangeBonusBlocks
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 50;
				}
				return config.RadioAntennaPartRangeBonusBlocks;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001D704 File Offset: 0x0001B904
		public static int RadioMicrophoneCaptureDistance
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 2;
				}
				return config.RadioMicrophoneCaptureDistance;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001D716 File Offset: 0x0001B916
		public static int RadioTalkieRangeBlocks
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 32;
				}
				return config.RadioTalkieRangeBlocks;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001D729 File Offset: 0x0001B929
		public static int RadioReceiverRangeBlocks
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 64;
				}
				return config.RadioReceiverRangeBlocks;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001D73C File Offset: 0x0001B93C
		public static int RadioReceiverMaxWiredSpeakers
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 4;
				}
				return config.RadioReceiverMaxWiredSpeakers;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001D74E File Offset: 0x0001B94E
		public static int TelephoneBroadcastMaxSpeakers
		{
			get
			{
				RPVoiceChatServerConfig config = ServerConfigManager.Config;
				if (config == null)
				{
					return 8;
				}
				return config.TelephoneBroadcastMaxSpeakers;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001D760 File Offset: 0x0001B960
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001D773 File Offset: 0x0001B973
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001D786 File Offset: 0x0001B986
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001D799 File Offset: 0x0001B999
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0001D7AF File Offset: 0x0001B9AF
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001D7C5 File Offset: 0x0001B9C5
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001D7DB File Offset: 0x0001B9DB
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001D7EE File Offset: 0x0001B9EE
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001D800 File Offset: 0x0001BA00
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001D813 File Offset: 0x0001BA13
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

		// Token: 0x0600058A RID: 1418 RVA: 0x0001D828 File Offset: 0x0001BA28
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
			if (ServerConfigManager.RadioReceiverMaxWiredSpeakers < 1 || ServerConfigManager.RadioReceiverMaxWiredSpeakers > 64)
			{
				Logger server18 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler18;
				defaultInterpolatedStringHandler18..ctor(79, 1);
				defaultInterpolatedStringHandler18.AppendLiteral("RadioReceiverMaxWiredSpeakers (");
				defaultInterpolatedStringHandler18.AppendFormatted<int>(ServerConfigManager.RadioReceiverMaxWiredSpeakers);
				defaultInterpolatedStringHandler18.AppendLiteral(") should be between 1 and 64. Using default (4).");
				server18.Warning(defaultInterpolatedStringHandler18.ToStringAndClear());
				ServerConfigManager.Config.RadioReceiverMaxWiredSpeakers = 4;
			}
			if (ServerConfigManager.TelephoneBroadcastMaxSpeakers < 1 || ServerConfigManager.TelephoneBroadcastMaxSpeakers > 64)
			{
				Logger server19 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler19;
				defaultInterpolatedStringHandler19..ctor(79, 1);
				defaultInterpolatedStringHandler19.AppendLiteral("TelephoneBroadcastMaxSpeakers (");
				defaultInterpolatedStringHandler19.AppendFormatted<int>(ServerConfigManager.TelephoneBroadcastMaxSpeakers);
				defaultInterpolatedStringHandler19.AppendLiteral(") should be between 1 and 64. Using default (8).");
				server19.Warning(defaultInterpolatedStringHandler19.ToStringAndClear());
				ServerConfigManager.Config.TelephoneBroadcastMaxSpeakers = 8;
			}
			if (ServerConfigManager.HandbellAudibleDistance < 1 || ServerConfigManager.HandbellAudibleDistance > 1000)
			{
				Logger server20 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler20;
				defaultInterpolatedStringHandler20..ctor(76, 1);
				defaultInterpolatedStringHandler20.AppendLiteral("HandbellAudibleDistance (");
				defaultInterpolatedStringHandler20.AppendFormatted<int>(ServerConfigManager.HandbellAudibleDistance);
				defaultInterpolatedStringHandler20.AppendLiteral(") should be between 1 and 1000. Using default (16).");
				server20.Warning(defaultInterpolatedStringHandler20.ToStringAndClear());
				ServerConfigManager.Config.HandbellAudibleDistance = 16;
			}
			if (ServerConfigManager.CallbellAudibleDistance < 1 || ServerConfigManager.CallbellAudibleDistance > 1000)
			{
				Logger server21 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler21;
				defaultInterpolatedStringHandler21..ctor(76, 1);
				defaultInterpolatedStringHandler21.AppendLiteral("CallbellAudibleDistance (");
				defaultInterpolatedStringHandler21.AppendFormatted<int>(ServerConfigManager.CallbellAudibleDistance);
				defaultInterpolatedStringHandler21.AppendLiteral(") should be between 1 and 1000. Using default (16).");
				server21.Warning(defaultInterpolatedStringHandler21.ToStringAndClear());
				ServerConfigManager.Config.CallbellAudibleDistance = 16;
			}
			if (ServerConfigManager.RoyalhornAudibleDistance < 1 || ServerConfigManager.RoyalhornAudibleDistance > 1000)
			{
				Logger server22 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler22;
				defaultInterpolatedStringHandler22..ctor(77, 1);
				defaultInterpolatedStringHandler22.AppendLiteral("RoyalhornAudibleDistance (");
				defaultInterpolatedStringHandler22.AppendFormatted<int>(ServerConfigManager.RoyalhornAudibleDistance);
				defaultInterpolatedStringHandler22.AppendLiteral(") should be between 1 and 1000. Using default (72).");
				server22.Warning(defaultInterpolatedStringHandler22.ToStringAndClear());
				ServerConfigManager.Config.RoyalhornAudibleDistance = 72;
			}
			if (ServerConfigManager.WarhornAudibleDistance < 1 || ServerConfigManager.WarhornAudibleDistance > 1000)
			{
				Logger server23 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler23;
				defaultInterpolatedStringHandler23..ctor(76, 1);
				defaultInterpolatedStringHandler23.AppendLiteral("WarhornAudibleDistance (");
				defaultInterpolatedStringHandler23.AppendFormatted<int>(ServerConfigManager.WarhornAudibleDistance);
				defaultInterpolatedStringHandler23.AppendLiteral(") should be between 1 and 1000. Using default (128).");
				server23.Warning(defaultInterpolatedStringHandler23.ToStringAndClear());
				ServerConfigManager.Config.WarhornAudibleDistance = 128;
			}
			if (ServerConfigManager.CarillonbellAudibleDistance < 1 || ServerConfigManager.CarillonbellAudibleDistance > 1000)
			{
				Logger server24 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler24;
				defaultInterpolatedStringHandler24..ctor(81, 1);
				defaultInterpolatedStringHandler24.AppendLiteral("CarillonbellAudibleDistance (");
				defaultInterpolatedStringHandler24.AppendFormatted<int>(ServerConfigManager.CarillonbellAudibleDistance);
				defaultInterpolatedStringHandler24.AppendLiteral(") should be between 1 and 1000. Using default (256).");
				server24.Warning(defaultInterpolatedStringHandler24.ToStringAndClear());
				ServerConfigManager.Config.CarillonbellAudibleDistance = 256;
			}
			if (ServerConfigManager.ChurchbellAudibleDistance < 1 || ServerConfigManager.ChurchbellAudibleDistance > 1000)
			{
				Logger server25 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler25;
				defaultInterpolatedStringHandler25..ctor(79, 1);
				defaultInterpolatedStringHandler25.AppendLiteral("ChurchbellAudibleDistance (");
				defaultInterpolatedStringHandler25.AppendFormatted<int>(ServerConfigManager.ChurchbellAudibleDistance);
				defaultInterpolatedStringHandler25.AppendLiteral(") should be between 1 and 1000. Using default (832).");
				server25.Warning(defaultInterpolatedStringHandler25.ToStringAndClear());
				ServerConfigManager.Config.ChurchbellAudibleDistance = 832;
			}
			if (ServerConfigManager.MegaphoneAudibleDistance < 1 || ServerConfigManager.MegaphoneAudibleDistance > 1000)
			{
				Logger server26 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler26;
				defaultInterpolatedStringHandler26..ctor(78, 1);
				defaultInterpolatedStringHandler26.AppendLiteral("MegaphoneAudibleDistance (");
				defaultInterpolatedStringHandler26.AppendFormatted<int>(ServerConfigManager.MegaphoneAudibleDistance);
				defaultInterpolatedStringHandler26.AppendLiteral(") should be between 1 and 1000. Using default (125).");
				server26.Warning(defaultInterpolatedStringHandler26.ToStringAndClear());
				ServerConfigManager.Config.MegaphoneAudibleDistance = 125;
			}
			if (ServerConfigManager.TelephoneAudibleDistance < 1 || ServerConfigManager.TelephoneAudibleDistance > 1000)
			{
				Logger server27 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler27;
				defaultInterpolatedStringHandler27..ctor(76, 1);
				defaultInterpolatedStringHandler27.AppendLiteral("TelephoneAudibleDistance (");
				defaultInterpolatedStringHandler27.AppendFormatted<int>(ServerConfigManager.TelephoneAudibleDistance);
				defaultInterpolatedStringHandler27.AppendLiteral(") should be between 1 and 1000. Using default (2).");
				server27.Warning(defaultInterpolatedStringHandler27.ToStringAndClear());
				ServerConfigManager.Config.TelephoneAudibleDistance = 2;
			}
			if (ServerConfigManager.SpeakerAudibleDistance < 1 || ServerConfigManager.SpeakerAudibleDistance > 1000)
			{
				Logger server28 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler28;
				defaultInterpolatedStringHandler28..ctor(75, 1);
				defaultInterpolatedStringHandler28.AppendLiteral("SpeakerAudibleDistance (");
				defaultInterpolatedStringHandler28.AppendFormatted<int>(ServerConfigManager.SpeakerAudibleDistance);
				defaultInterpolatedStringHandler28.AppendLiteral(") should be between 1 and 1000. Using default (30).");
				server28.Warning(defaultInterpolatedStringHandler28.ToStringAndClear());
				ServerConfigManager.Config.SpeakerAudibleDistance = 30;
			}
			if (ServerConfigManager.RadioEmitterBaseRangeBlocks < 1 || ServerConfigManager.RadioEmitterBaseRangeBlocks > 10000)
			{
				Logger server29 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler29;
				defaultInterpolatedStringHandler29..ctor(82, 1);
				defaultInterpolatedStringHandler29.AppendLiteral("RadioEmitterBaseRangeBlocks (");
				defaultInterpolatedStringHandler29.AppendFormatted<int>(ServerConfigManager.RadioEmitterBaseRangeBlocks);
				defaultInterpolatedStringHandler29.AppendLiteral(") should be between 1 and 10000. Using default (100).");
				server29.Warning(defaultInterpolatedStringHandler29.ToStringAndClear());
				ServerConfigManager.Config.RadioEmitterBaseRangeBlocks = 100;
			}
			if (ServerConfigManager.RadioAntennaPartRangeBonusBlocks < 0 || ServerConfigManager.RadioAntennaPartRangeBonusBlocks > 10000)
			{
				Logger server30 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler30;
				defaultInterpolatedStringHandler30..ctor(86, 1);
				defaultInterpolatedStringHandler30.AppendLiteral("RadioAntennaPartRangeBonusBlocks (");
				defaultInterpolatedStringHandler30.AppendFormatted<int>(ServerConfigManager.RadioAntennaPartRangeBonusBlocks);
				defaultInterpolatedStringHandler30.AppendLiteral(") should be between 0 and 10000. Using default (50).");
				server30.Warning(defaultInterpolatedStringHandler30.ToStringAndClear());
				ServerConfigManager.Config.RadioAntennaPartRangeBonusBlocks = 50;
			}
			if (ServerConfigManager.RadioMicrophoneCaptureDistance < 1 || ServerConfigManager.RadioMicrophoneCaptureDistance > 1000)
			{
				Logger server31 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler31;
				defaultInterpolatedStringHandler31..ctor(82, 1);
				defaultInterpolatedStringHandler31.AppendLiteral("RadioMicrophoneCaptureDistance (");
				defaultInterpolatedStringHandler31.AppendFormatted<int>(ServerConfigManager.RadioMicrophoneCaptureDistance);
				defaultInterpolatedStringHandler31.AppendLiteral(") should be between 1 and 1000. Using default (2).");
				server31.Warning(defaultInterpolatedStringHandler31.ToStringAndClear());
				ServerConfigManager.Config.RadioMicrophoneCaptureDistance = 2;
			}
			if (ServerConfigManager.RadioTalkieRangeBlocks < 1 || ServerConfigManager.RadioTalkieRangeBlocks > 1000)
			{
				Logger server32 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler32;
				defaultInterpolatedStringHandler32..ctor(75, 1);
				defaultInterpolatedStringHandler32.AppendLiteral("RadioTalkieRangeBlocks (");
				defaultInterpolatedStringHandler32.AppendFormatted<int>(ServerConfigManager.RadioTalkieRangeBlocks);
				defaultInterpolatedStringHandler32.AppendLiteral(") should be between 1 and 1000. Using default (32).");
				server32.Warning(defaultInterpolatedStringHandler32.ToStringAndClear());
				ServerConfigManager.Config.RadioTalkieRangeBlocks = 32;
			}
			if (ServerConfigManager.RadioReceiverRangeBlocks < 1 || ServerConfigManager.RadioReceiverRangeBlocks > 10000)
			{
				Logger server33 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler33;
				defaultInterpolatedStringHandler33..ctor(78, 1);
				defaultInterpolatedStringHandler33.AppendLiteral("RadioReceiverRangeBlocks (");
				defaultInterpolatedStringHandler33.AppendFormatted<int>(ServerConfigManager.RadioReceiverRangeBlocks);
				defaultInterpolatedStringHandler33.AppendLiteral(") should be between 1 and 10000. Using default (64).");
				server33.Warning(defaultInterpolatedStringHandler33.ToStringAndClear());
				ServerConfigManager.Config.RadioReceiverRangeBlocks = 64;
			}
			if (ServerConfigManager.NametagFallbackRenderRange < 1 || ServerConfigManager.NametagFallbackRenderRange > 1000)
			{
				Logger server34 = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler34;
				defaultInterpolatedStringHandler34..ctor(79, 1);
				defaultInterpolatedStringHandler34.AppendLiteral("NametagFallbackRenderRange (");
				defaultInterpolatedStringHandler34.AppendFormatted<int>(ServerConfigManager.NametagFallbackRenderRange);
				defaultInterpolatedStringHandler34.AppendLiteral(") should be between 1 and 1000. Using default (99).");
				server34.Warning(defaultInterpolatedStringHandler34.ToStringAndClear());
				ServerConfigManager.Config.NametagFallbackRenderRange = 99;
			}
		}
	}
}
