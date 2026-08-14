using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x02000083 RID: 131
	public class RPVoiceChatServerConfig : IModConfig
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00016D37 File Offset: 0x00014F37
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00016D3F File Offset: 0x00014F3F
		public int ServerPort { get; set; } = 52525;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00016D48 File Offset: 0x00014F48
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00016D50 File Offset: 0x00014F50
		public string ServerIP { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00016D59 File Offset: 0x00014F59
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00016D61 File Offset: 0x00014F61
		public bool UseCustomNetworkServers { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00016D6A File Offset: 0x00014F6A
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00016D72 File Offset: 0x00014F72
		public bool ManualPortForwarding { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00016D7B File Offset: 0x00014F7B
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x00016D83 File Offset: 0x00014F83
		public int MaxConnectionAttempts { get; set; } = 5;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00016D8C File Offset: 0x00014F8C
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00016D94 File Offset: 0x00014F94
		public bool AdditionalContent { get; set; } = true;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00016D9D File Offset: 0x00014F9D
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x00016DA5 File Offset: 0x00014FA5
		public bool TechnologyContent { get; set; } = true;

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00016DAE File Offset: 0x00014FAE
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00016DB6 File Offset: 0x00014FB6
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00016DBF File Offset: 0x00014FBF
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00016DC7 File Offset: 0x00014FC7
		public float MaxAudioGain { get; set; } = 2f;

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00016DD0 File Offset: 0x00014FD0
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00016DD8 File Offset: 0x00014FD8
		public float MaxVolumeLimit { get; set; } = 0.8f;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00016DE1 File Offset: 0x00014FE1
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00016DE9 File Offset: 0x00014FE9
		public int GridCellSizeBlocks { get; set; } = 64;

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00016DF2 File Offset: 0x00014FF2
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x00016DFA File Offset: 0x00014FFA
		public int NormalBitrate { get; set; } = 40960;

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00016E03 File Offset: 0x00015003
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00016E0B File Offset: 0x0001500B
		public int BroadcastBitrate { get; set; } = 16384;

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00016E14 File Offset: 0x00015014
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00016E1C File Offset: 0x0001501C
		public int TelegraphMaxConnectionDistance { get; set; } = 20;

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00016E25 File Offset: 0x00015025
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00016E2D File Offset: 0x0001502D
		public int TelegraphMaxConnectionsPerNode { get; set; } = 4;

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00016E36 File Offset: 0x00015036
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x00016E3E File Offset: 0x0001503E
		public int TelegraphMaxMessageLength { get; set; } = 100;

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00016E47 File Offset: 0x00015047
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00016E4F File Offset: 0x0001504F
		public int TelegraphMinDelayBetweenKeysMs { get; set; } = 200;

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00016E58 File Offset: 0x00015058
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00016E60 File Offset: 0x00015060
		public double BellRingCooldownSeconds { get; set; } = 1.5;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00016E69 File Offset: 0x00015069
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x00016E71 File Offset: 0x00015071
		public bool TelegraphGenuineMorseCharacters { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00016E7A File Offset: 0x0001507A
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00016E82 File Offset: 0x00015082
		public int TelegraphMessageDeletionDelaySeconds { get; set; } = 10;

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00016E8B File Offset: 0x0001508B
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00016E93 File Offset: 0x00015093
		public int TelegraphNetworkMinPowerPercent { get; set; } = 10;

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00016E9C File Offset: 0x0001509C
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x00016EA4 File Offset: 0x000150A4
		public int TelegraphNetworkMaxEndpoints { get; set; } = 8;

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00016EAD File Offset: 0x000150AD
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00016EB5 File Offset: 0x000150B5
		public int TelephoneNetworkMinPowerPercent { get; set; } = 25;

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00016EBE File Offset: 0x000150BE
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x00016EC6 File Offset: 0x000150C6
		public int TelephoneNetworkMaxEndpoints { get; set; } = 16;

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00016ECF File Offset: 0x000150CF
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x00016ED7 File Offset: 0x000150D7
		public int RadioNetworkMinPowerPercent { get; set; } = 50;

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016EE0 File Offset: 0x000150E0
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00016EE8 File Offset: 0x000150E8
		public int RadioNetworkMaxEndpoints { get; set; } = 16;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00016EF1 File Offset: 0x000150F1
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x00016EF9 File Offset: 0x000150F9
		public int HandbellAudibleDistance { get; set; } = 16;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00016F02 File Offset: 0x00015102
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00016F0A File Offset: 0x0001510A
		public int CallbellAudibleDistance { get; set; } = 16;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00016F13 File Offset: 0x00015113
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00016F1B File Offset: 0x0001511B
		public int RoyalhornAudibleDistance { get; set; } = 72;

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00016F24 File Offset: 0x00015124
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00016F2C File Offset: 0x0001512C
		public int WarhornAudibleDistance { get; set; } = 128;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016F35 File Offset: 0x00015135
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x00016F3D File Offset: 0x0001513D
		public int CarillonbellAudibleDistance { get; set; } = 256;

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00016F46 File Offset: 0x00015146
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00016F4E File Offset: 0x0001514E
		public int ChurchbellAudibleDistance { get; set; } = 832;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00016F57 File Offset: 0x00015157
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00016F5F File Offset: 0x0001515F
		public int MegaphoneAudibleDistance { get; set; } = 125;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00016F68 File Offset: 0x00015168
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00016F70 File Offset: 0x00015170
		public int TelephoneAudibleDistance { get; set; } = 2;

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00016F79 File Offset: 0x00015179
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x00016F81 File Offset: 0x00015181
		public int SpeakerAudibleDistance { get; set; } = 30;

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00016F8A File Offset: 0x0001518A
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00016F92 File Offset: 0x00015192
		public int NametagFallbackRenderRange { get; set; } = 99;

		// Token: 0x0600044F RID: 1103 RVA: 0x00016F9C File Offset: 0x0001519C
		public RPVoiceChatServerConfig()
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000170C4 File Offset: 0x000152C4
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
