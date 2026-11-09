using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Server;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000042 RID: 66
	public static class RadioRfTransmissionService
	{
		// Token: 0x06000185 RID: 389 RVA: 0x0000AED8 File Offset: 0x000090D8
		public static List<RadioTransmissionPoint> CollectWiredTransmissionPoints(ICoreServerAPI sapi)
		{
			List<RadioTransmissionPoint> points = new List<RadioTransmissionPoint>();
			if (sapi == null)
			{
				return points;
			}
			foreach (RadioRfEmitterPresence emitter in RadioRfPresenceRegistry.GetEmitters())
			{
				if (!emitter.IsRepeater && emitter.IsActive && emitter.RangeBlocks > 0)
				{
					string frequency = RadioFrequencyUtil.Normalize(emitter.Frequency);
					if (frequency.Length != 0)
					{
						points.Add(RadioRfTransmissionService.ToTransmissionPoint(emitter, frequency, false));
					}
				}
			}
			return points;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000AF64 File Offset: 0x00009164
		public static List<RadioTransmissionPoint> CollectActiveTransmissionPoints(ICoreServerAPI sapi)
		{
			List<RadioTransmissionPoint> points = RadioRfTransmissionService.CollectWiredTransmissionPoints(sapi);
			if (sapi == null)
			{
				return points;
			}
			using (IEnumerator<RadioRfEmitterPresence> enumerator = RadioRfPresenceRegistry.GetEmitters().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RadioRfEmitterPresence repeater = enumerator.Current;
					if (repeater.IsRepeater && repeater.IsActive && repeater.RangeBlocks > 0 && !(repeater.Pos == null))
					{
						string frequency = RadioFrequencyUtil.Normalize(repeater.Frequency);
						if (frequency.Length != 0)
						{
							Vec3d repeaterPos = repeater.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
							if (Enumerable.Any<RadioTransmissionPoint>(points, (RadioTransmissionPoint source) => source.Dimension == repeater.Dimension && RadioFrequencyUtil.Matches(source.Frequency, frequency) && repeaterPos.DistanceTo(source.Position) <= (float)source.RangeBlocks))
							{
								points.Add(RadioRfTransmissionService.ToTransmissionPoint(repeater, frequency, true));
							}
						}
					}
				}
			}
			return points;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000B094 File Offset: 0x00009294
		public static List<VoiceRoute> BuildVoiceRoutesForTransmissionPoints(IEnumerable<RadioTransmissionPoint> points)
		{
			List<VoiceRoute> routes = new List<VoiceRoute>();
			foreach (RadioTransmissionPoint point in points)
			{
				if (!(point.Position == null) && point.RangeBlocks > 0 && !string.IsNullOrEmpty(point.Frequency))
				{
					routes.Add(new VoiceRoute(point.Position, point.RangeBlocks, point.Dimension, point.Frequency, false));
				}
			}
			return routes;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B12C File Offset: 0x0000932C
		public static void AppendReceiverRelayRoutes(ICoreServerAPI sapi, ICollection<VoiceRoute> routes, IEnumerable<string> frequencies)
		{
			if (routes == null)
			{
				return;
			}
			HashSet<string> frequencySet = new HashSet<string>();
			foreach (string frequency in (frequencies ?? Enumerable.Empty<string>()))
			{
				string normalized = RadioFrequencyUtil.Normalize(frequency);
				if (normalized.Length > 0)
				{
					frequencySet.Add(normalized);
				}
			}
			if (frequencySet.Count == 0)
			{
				return;
			}
			foreach (RadioRfReceiverPresence receiver in RadioRfPresenceRegistry.GetReceivers())
			{
				if (receiver != null && receiver.IsEnabled)
				{
					string tuned = RadioFrequencyUtil.Normalize(receiver.TunedFrequency);
					if (tuned.Length != 0 && frequencySet.Contains(tuned))
					{
						if (receiver.AcousticPoints != null && receiver.AcousticPoints.Count > 0)
						{
							using (List<RadioRfAcousticPresence>.Enumerator enumerator3 = receiver.AcousticPoints.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									RadioRfAcousticPresence acoustic = enumerator3.Current;
									if (!(((acoustic != null) ? acoustic.Pos : null) == null) && acoustic.RangeBlocks > 0)
									{
										routes.Add(new VoiceRoute(acoustic.Pos.ToVec3d().Add(0.5, 0.5, 0.5), acoustic.RangeBlocks, acoustic.Dimension, tuned, true));
									}
								}
								continue;
							}
						}
						if (!(receiver.Pos == null) && receiver.PlaybackRangeBlocks > 0)
						{
							routes.Add(new VoiceRoute(receiver.Pos.ToVec3d().Add(0.5, 0.5, 0.5), receiver.PlaybackRangeBlocks, receiver.Dimension, tuned, true));
						}
					}
				}
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B360 File Offset: 0x00009560
		private static RadioTransmissionPoint ToTransmissionPoint(RadioRfEmitterPresence emitter, string frequency, bool isRepeaterRelay)
		{
			return new RadioTransmissionPoint(emitter.Pos.ToVec3d().Add(0.5, 0.5, 0.5), emitter.RangeBlocks, frequency, emitter.Dimension, isRepeaterRelay);
		}
	}
}
