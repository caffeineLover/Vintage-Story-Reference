using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000040 RID: 64
	public static class RadioRfPresenceRegistry
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000A54C File Offset: 0x0000874C
		public static void Clear()
		{
			RadioRfPresenceRegistry.emittersByKey.Clear();
			RadioRfPresenceRegistry.receiversByKey.Clear();
			RadioRfPresenceRegistry.programsByKey.Clear();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A56C File Offset: 0x0000876C
		public static void LoadFromSave(byte[] data)
		{
			RadioRfPresenceRegistry.Clear();
			if (data == null || data.Length == 0)
			{
				return;
			}
			RadioRfPresenceData payload = SerializerUtil.Deserialize<RadioRfPresenceData>(data);
			if (payload == null)
			{
				return;
			}
			if (payload.Emitters != null)
			{
				foreach (RadioRfEmitterPresence presence in payload.Emitters)
				{
					RadioRfPresenceRegistry.UpsertEmitter(presence);
				}
			}
			if (payload.Receivers != null)
			{
				foreach (RadioRfReceiverPresence presence2 in payload.Receivers)
				{
					RadioRfPresenceRegistry.UpsertReceiver(presence2);
				}
			}
			if (payload.Programs != null)
			{
				foreach (RadioRfProgramPresence presence3 in payload.Programs)
				{
					RadioRfPresenceRegistry.UpsertProgram(presence3);
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A66C File Offset: 0x0000886C
		public static byte[] ToSaveBytes()
		{
			RadioRfPresenceData radioRfPresenceData = new RadioRfPresenceData();
			IEnumerable<RadioRfEmitterPresence> values = RadioRfPresenceRegistry.emittersByKey.Values;
			Func<RadioRfEmitterPresence, RadioRfEmitterPresence> func;
			if ((func = RadioRfPresenceRegistry.<>O.<0>__CloneEmitter) == null)
			{
				func = (RadioRfPresenceRegistry.<>O.<0>__CloneEmitter = new Func<RadioRfEmitterPresence, RadioRfEmitterPresence>(RadioRfPresenceRegistry.CloneEmitter));
			}
			radioRfPresenceData.Emitters = Enumerable.ToList<RadioRfEmitterPresence>(Enumerable.Where<RadioRfEmitterPresence>(Enumerable.Select<RadioRfEmitterPresence, RadioRfEmitterPresence>(values, func), (RadioRfEmitterPresence entry) => ((entry != null) ? entry.Pos : null) != null));
			IEnumerable<RadioRfReceiverPresence> values2 = RadioRfPresenceRegistry.receiversByKey.Values;
			Func<RadioRfReceiverPresence, RadioRfReceiverPresence> func2;
			if ((func2 = RadioRfPresenceRegistry.<>O.<1>__CloneReceiver) == null)
			{
				func2 = (RadioRfPresenceRegistry.<>O.<1>__CloneReceiver = new Func<RadioRfReceiverPresence, RadioRfReceiverPresence>(RadioRfPresenceRegistry.CloneReceiver));
			}
			radioRfPresenceData.Receivers = Enumerable.ToList<RadioRfReceiverPresence>(Enumerable.Where<RadioRfReceiverPresence>(Enumerable.Select<RadioRfReceiverPresence, RadioRfReceiverPresence>(values2, func2), (RadioRfReceiverPresence entry) => ((entry != null) ? entry.Pos : null) != null));
			IEnumerable<RadioRfProgramPresence> values3 = RadioRfPresenceRegistry.programsByKey.Values;
			Func<RadioRfProgramPresence, RadioRfProgramPresence> func3;
			if ((func3 = RadioRfPresenceRegistry.<>O.<2>__CloneProgram) == null)
			{
				func3 = (RadioRfPresenceRegistry.<>O.<2>__CloneProgram = new Func<RadioRfProgramPresence, RadioRfProgramPresence>(RadioRfPresenceRegistry.CloneProgram));
			}
			radioRfPresenceData.Programs = Enumerable.ToList<RadioRfProgramPresence>(Enumerable.Where<RadioRfProgramPresence>(Enumerable.Select<RadioRfProgramPresence, RadioRfProgramPresence>(values3, func3), (RadioRfProgramPresence entry) => ((entry != null) ? entry.Pos : null) != null));
			return SerializerUtil.Serialize<RadioRfPresenceData>(radioRfPresenceData);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A790 File Offset: 0x00008990
		public static void UpsertEmitter(RadioRfEmitterPresence presence)
		{
			if (((presence != null) ? presence.Pos : null) == null)
			{
				return;
			}
			string key = RadioRfPresenceRegistry.PosKey(presence.Pos);
			RadioRfPresenceRegistry.emittersByKey[key] = RadioRfPresenceRegistry.CloneEmitter(presence);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A7CF File Offset: 0x000089CF
		public static void RemoveEmitter(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			RadioRfPresenceRegistry.emittersByKey.Remove(RadioRfPresenceRegistry.PosKey(pos));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A7EC File Offset: 0x000089EC
		public static void UpsertReceiver(RadioRfReceiverPresence presence)
		{
			if (((presence != null) ? presence.Pos : null) == null)
			{
				return;
			}
			string key = RadioRfPresenceRegistry.PosKey(presence.Pos);
			RadioRfPresenceRegistry.receiversByKey[key] = RadioRfPresenceRegistry.CloneReceiver(presence);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A82B File Offset: 0x00008A2B
		public static void RemoveReceiver(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			RadioRfPresenceRegistry.receiversByKey.Remove(RadioRfPresenceRegistry.PosKey(pos));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A848 File Offset: 0x00008A48
		public static void UpdateWiredSourceFrequency(long networkId, string frequency)
		{
			if (networkId == 0L)
			{
				return;
			}
			string normalized = RadioFrequencyUtil.Normalize(frequency);
			foreach (RadioRfEmitterPresence entry in RadioRfPresenceRegistry.emittersByKey.Values)
			{
				if (entry != null && !entry.IsRepeater && entry.NetworkId == networkId)
				{
					entry.Frequency = normalized;
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A8C0 File Offset: 0x00008AC0
		public static IEnumerable<RadioRfEmitterPresence> GetEmitters()
		{
			return Enumerable.Where<RadioRfEmitterPresence>(RadioRfPresenceRegistry.emittersByKey.Values, (RadioRfEmitterPresence entry) => ((entry != null) ? entry.Pos : null) != null);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public static IEnumerable<RadioRfReceiverPresence> GetReceivers()
		{
			return Enumerable.Where<RadioRfReceiverPresence>(RadioRfPresenceRegistry.receiversByKey.Values, (RadioRfReceiverPresence entry) => ((entry != null) ? entry.Pos : null) != null);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A920 File Offset: 0x00008B20
		public static IEnumerable<string> GetActiveFrequenciesForNetwork(long networkId)
		{
			RadioRfPresenceRegistry.<GetActiveFrequenciesForNetwork>d__14 <GetActiveFrequenciesForNetwork>d__ = new RadioRfPresenceRegistry.<GetActiveFrequenciesForNetwork>d__14(-2);
			<GetActiveFrequenciesForNetwork>d__.<>3__networkId = networkId;
			return <GetActiveFrequenciesForNetwork>d__;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A930 File Offset: 0x00008B30
		public static IEnumerable<string> EnumerateClaimedTransmitFrequencies(BlockPos excludePos)
		{
			RadioRfPresenceRegistry.<EnumerateClaimedTransmitFrequencies>d__15 <EnumerateClaimedTransmitFrequencies>d__ = new RadioRfPresenceRegistry.<EnumerateClaimedTransmitFrequencies>d__15(-2);
			<EnumerateClaimedTransmitFrequencies>d__.<>3__excludePos = excludePos;
			return <EnumerateClaimedTransmitFrequencies>d__;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A940 File Offset: 0x00008B40
		public static void UpsertProgram(RadioRfProgramPresence presence)
		{
			if (((presence != null) ? presence.Pos : null) == null)
			{
				return;
			}
			RadioRfPresenceRegistry.programsByKey[RadioRfPresenceRegistry.PosKey(presence.Pos)] = RadioRfPresenceRegistry.CloneProgram(presence);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A972 File Offset: 0x00008B72
		public static void RemoveProgram(BlockPos pos)
		{
			if (pos == null)
			{
				return;
			}
			RadioRfPresenceRegistry.programsByKey.Remove(RadioRfPresenceRegistry.PosKey(pos));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000A98F File Offset: 0x00008B8F
		public static IEnumerable<RadioRfProgramPresence> GetPrograms()
		{
			return Enumerable.Where<RadioRfProgramPresence>(RadioRfPresenceRegistry.programsByKey.Values, (RadioRfProgramPresence entry) => ((entry != null) ? entry.Pos : null) != null);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000A9C0 File Offset: 0x00008BC0
		private static string PosKey(BlockPos pos)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.X);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Y);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Z);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.dimension);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000AA38 File Offset: 0x00008C38
		private static RadioRfEmitterPresence CloneEmitter(RadioRfEmitterPresence source)
		{
			if (source == null)
			{
				return null;
			}
			RadioRfEmitterPresence radioRfEmitterPresence = new RadioRfEmitterPresence();
			BlockPos pos = source.Pos;
			radioRfEmitterPresence.Pos = ((pos != null) ? pos.Copy() : null);
			radioRfEmitterPresence.Dimension = source.Dimension;
			radioRfEmitterPresence.NetworkId = source.NetworkId;
			radioRfEmitterPresence.IsRepeater = source.IsRepeater;
			radioRfEmitterPresence.Frequency = (source.Frequency ?? "");
			radioRfEmitterPresence.RangeBlocks = source.RangeBlocks;
			radioRfEmitterPresence.IsActive = source.IsActive;
			return radioRfEmitterPresence;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		private static RadioRfReceiverPresence CloneReceiver(RadioRfReceiverPresence source)
		{
			if (source == null)
			{
				return null;
			}
			RadioRfReceiverPresence radioRfReceiverPresence = new RadioRfReceiverPresence();
			BlockPos pos = source.Pos;
			radioRfReceiverPresence.Pos = ((pos != null) ? pos.Copy() : null);
			radioRfReceiverPresence.Dimension = source.Dimension;
			radioRfReceiverPresence.TunedFrequency = (source.TunedFrequency ?? "");
			radioRfReceiverPresence.PlaybackRangeBlocks = source.PlaybackRangeBlocks;
			radioRfReceiverPresence.IsEnabled = source.IsEnabled;
			radioRfReceiverPresence.AcousticPoints = Enumerable.ToList<RadioRfAcousticPresence>(Enumerable.Select<RadioRfAcousticPresence, RadioRfAcousticPresence>(Enumerable.Where<RadioRfAcousticPresence>(source.AcousticPoints ?? new List<RadioRfAcousticPresence>(), (RadioRfAcousticPresence point) => ((point != null) ? point.Pos : null) != null), (RadioRfAcousticPresence point) => new RadioRfAcousticPresence
			{
				Pos = point.Pos.Copy(),
				Dimension = point.Dimension,
				RangeBlocks = point.RangeBlocks
			}));
			return radioRfReceiverPresence;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000AB84 File Offset: 0x00008D84
		private static RadioRfProgramPresence CloneProgram(RadioRfProgramPresence source)
		{
			if (source == null)
			{
				return null;
			}
			RadioRfProgramPresence radioRfProgramPresence = new RadioRfProgramPresence();
			BlockPos pos = source.Pos;
			radioRfProgramPresence.Pos = ((pos != null) ? pos.Copy() : null);
			radioRfProgramPresence.Dimension = source.Dimension;
			radioRfProgramPresence.NetworkId = source.NetworkId;
			radioRfProgramPresence.IsOnAir = source.IsOnAir;
			radioRfProgramPresence.HlsStreamUrl = (source.HlsStreamUrl ?? "");
			radioRfProgramPresence.ActiveOperatorPlayerUid = (source.ActiveOperatorPlayerUid ?? "");
			return radioRfProgramPresence;
		}

		// Token: 0x040000AA RID: 170
		public const string PresenceDataKey = "rpvc:radio-rf-presence";

		// Token: 0x040000AB RID: 171
		private static readonly Dictionary<string, RadioRfEmitterPresence> emittersByKey = new Dictionary<string, RadioRfEmitterPresence>();

		// Token: 0x040000AC RID: 172
		private static readonly Dictionary<string, RadioRfReceiverPresence> receiversByKey = new Dictionary<string, RadioRfReceiverPresence>();

		// Token: 0x040000AD RID: 173
		private static readonly Dictionary<string, RadioRfProgramPresence> programsByKey = new Dictionary<string, RadioRfProgramPresence>();

		// Token: 0x02000124 RID: 292
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040004D1 RID: 1233
			public static Func<RadioRfEmitterPresence, RadioRfEmitterPresence> <0>__CloneEmitter;

			// Token: 0x040004D2 RID: 1234
			public static Func<RadioRfReceiverPresence, RadioRfReceiverPresence> <1>__CloneReceiver;

			// Token: 0x040004D3 RID: 1235
			public static Func<RadioRfProgramPresence, RadioRfProgramPresence> <2>__CloneProgram;
		}
	}
}
