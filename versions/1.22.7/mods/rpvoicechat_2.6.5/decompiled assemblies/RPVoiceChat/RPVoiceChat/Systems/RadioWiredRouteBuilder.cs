using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Server;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000048 RID: 72
	public static class RadioWiredRouteBuilder
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public static List<VoiceRoute> BuildRoutesForWiredNode(ICoreServerAPI sapi, BEWireNode node)
		{
			if (sapi == null || node == null)
			{
				return new List<VoiceRoute>();
			}
			List<BlockEntitySpeaker> speakers = Enumerable.ToList<BlockEntitySpeaker>(RadioWireNetworkHelper.FindSpeakers(node));
			return RadioWiredRouteBuilder.BuildRoutesForNetwork(sapi, node.NetworkUID, speakers, node.Pos.dimension);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public static List<VoiceRoute> BuildRoutesForNetwork(ICoreServerAPI sapi, long networkId, IEnumerable<BlockEntitySpeaker> speakers = null, int speakerDimension = 0)
		{
			List<VoiceRoute> routes = new List<VoiceRoute>();
			if (sapi == null || networkId == 0L)
			{
				return routes;
			}
			List<BlockEntitySpeaker> list;
			if (speakers == null)
			{
				list = null;
			}
			else
			{
				list = Enumerable.ToList<BlockEntitySpeaker>(Enumerable.Where<BlockEntitySpeaker>(speakers, (BlockEntitySpeaker s) => s != null));
			}
			List<BlockEntitySpeaker> speakerList = list ?? new List<BlockEntitySpeaker>();
			List<string> frequencies = Enumerable.ToList<string>(RadioRfPresenceRegistry.GetActiveFrequenciesForNetwork(networkId));
			IEnumerable<BlockEntityRadioEmitter> loadedEmitters = RadioBlockIndex.GetLoadedEmitters(sapi.World);
			Func<BlockEntityRadioEmitter, bool> <>9__1;
			Func<BlockEntityRadioEmitter, bool> func;
			if ((func = <>9__1) == null)
			{
				func = (<>9__1 = ((BlockEntityRadioEmitter e) => e != null && e.NetworkUID == networkId && e.IsWirelessTransmitting));
			}
			foreach (BlockEntityRadioEmitter emitter in Enumerable.Where<BlockEntityRadioEmitter>(loadedEmitters, func))
			{
				string frequency = RadioFrequencyUtil.Normalize(emitter.GetConsoleFrequency());
				if (frequency.Length > 0 && !Enumerable.Any<string>(frequencies, (string existing) => RadioFrequencyUtil.Matches(existing, frequency)))
				{
					frequencies.Add(frequency);
				}
			}
			if (frequencies.Count == 0 && speakerList.Count == 0)
			{
				return routes;
			}
			if (frequencies.Count > 0)
			{
				List<RadioTransmissionPoint> transmissionPoints = Enumerable.ToList<RadioTransmissionPoint>(Enumerable.Where<RadioTransmissionPoint>(RadioRfTransmissionService.CollectActiveTransmissionPoints(sapi), (RadioTransmissionPoint point) => Enumerable.Any<string>(frequencies, (string frequency) => RadioFrequencyUtil.Matches(frequency, point.Frequency))));
				routes.AddRange(RadioRfTransmissionService.BuildVoiceRoutesForTransmissionPoints(transmissionPoints));
				RadioRfTransmissionService.AppendReceiverRelayRoutes(sapi, routes, frequencies);
			}
			foreach (BlockEntitySpeaker speaker in speakerList)
			{
				List<VoiceRoute> list2 = routes;
				Vec3d emissionPos = speaker.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
				int voiceEmissionRangeBlocks = speaker.VoiceEmissionRangeBlocks;
				BlockPos pos = speaker.Pos;
				list2.Add(new VoiceRoute(emissionPos, voiceEmissionRangeBlocks, (pos != null) ? pos.dimension : speakerDimension));
			}
			return routes;
		}
	}
}
