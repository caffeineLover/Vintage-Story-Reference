using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Audio;
using RPVoiceChat.Audio.Input;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Networking;
using RPVoiceChat.Server;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000036 RID: 54
	public class RadioProgramBroadcastSystem : ModSystem
	{
		// Token: 0x06000146 RID: 326 RVA: 0x000094D4 File Offset: 0x000076D4
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			this.routing = api.ModLoader.GetModSystem<RadioVoiceRoutingSystem>(true);
			api.Event.RegisterGameTickListener(new Action<float>(this.OnServerTick), 100, 0);
			FfmpegLocator.BeginEnsureAvailable();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000950F File Offset: 0x0000770F
		public void BindGameServer(GameServer server)
		{
			this.gameServer = server;
			server.SetProgramMicAudioSink(new Func<AudioPacket, bool>(this.TryConsumeMicAudio));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000952C File Offset: 0x0000772C
		public bool TryConsumeMicAudio(AudioPacket packet)
		{
			if (packet == null || string.IsNullOrWhiteSpace(packet.PlayerId) || packet.AudioData == null || packet.AudioData.Length == 0)
			{
				return false;
			}
			string routeKey;
			RadioProgramBroadcastSystem.ProgramSession session;
			if (!this.micOperatorToRouteKey.TryGetValue(packet.PlayerId, ref routeKey) || !this.sessions.TryGetValue(routeKey, ref session))
			{
				return false;
			}
			short[] micSamples = RadioProgramBroadcastSystem.DecodeMicPacket(session, packet);
			if (micSamples.Length == 0)
			{
				return true;
			}
			session.MicBuffer.EnqueueSamples(micSamples);
			if (!session.HasHlsCapture)
			{
				this.EmitMicOnlyFrames(session, micSamples);
			}
			return true;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000095B0 File Offset: 0x000077B0
		private void OnServerTick(float dt)
		{
			if (this.sapi == null || this.routing == null)
			{
				return;
			}
			foreach (BlockEntityRadioMixingConsole blockEntityRadioMixingConsole in Enumerable.ToList<BlockEntityRadioMixingConsole>(RadioBlockIndex.GetLoadedMixingConsoles(this.sapi.World)))
			{
				blockEntityRadioMixingConsole.PublishProgramPresence();
			}
			this.RefreshMicOperatorBindings();
			HashSet<string> activeRouteKeys = new HashSet<string>();
			foreach (RadioRfProgramPresence program in Enumerable.ToList<RadioRfProgramPresence>(RadioRfPresenceRegistry.GetPrograms()))
			{
				string routeKey = RadioProgramRouteKey.ForMixingConsole(program.Pos);
				if (!string.IsNullOrWhiteSpace(routeKey))
				{
					if (!program.IsOnAir || program.NetworkId == 0L)
					{
						this.StopSession(routeKey);
						this.routing.ClearProgramRoute(routeKey);
					}
					else
					{
						IEnumerable<BlockEntitySpeaker> speakers = null;
						BlockEntityRadioMixingConsole loadedConsole = this.ResolveLoadedConsole(program.Pos);
						if (loadedConsole != null)
						{
							speakers = RadioWireNetworkHelper.FindSpeakers(loadedConsole);
						}
						List<VoiceRoute> routes = RadioWiredRouteBuilder.BuildRoutesForNetwork(this.sapi, program.NetworkId, speakers, program.Dimension);
						if (routes.Count == 0)
						{
							this.StopSession(routeKey);
							this.routing.ClearProgramRoute(routeKey);
						}
						else
						{
							this.routing.SetProgramRoutes(routeKey, routes);
							activeRouteKeys.Add(routeKey);
							if (!this.sessions.ContainsKey(routeKey))
							{
								this.TryStartSession(program, routeKey, loadedConsole);
							}
						}
					}
				}
			}
			foreach (string routeKey2 in Enumerable.ToArray<string>(this.sessions.Keys))
			{
				if (!activeRouteKeys.Contains(routeKey2))
				{
					this.StopSession(routeKey2);
					this.routing.ClearProgramRoute(routeKey2);
				}
			}
			this.FlushPendingPackets();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00009790 File Offset: 0x00007990
		private BlockEntityRadioMixingConsole ResolveLoadedConsole(BlockPos pos)
		{
			if (!(pos == null))
			{
				ICoreServerAPI coreServerAPI = this.sapi;
				if (((coreServerAPI != null) ? coreServerAPI.World : null) != null)
				{
					return this.sapi.World.BlockAccessor.GetBlockEntity(pos) as BlockEntityRadioMixingConsole;
				}
			}
			return null;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000097CC File Offset: 0x000079CC
		private void RefreshMicOperatorBindings()
		{
			Dictionary<string, string> nextBindings = new Dictionary<string, string>();
			foreach (RadioRfProgramPresence program in RadioRfPresenceRegistry.GetPrograms())
			{
				if (program.IsOnAir)
				{
					string routeKey = RadioProgramRouteKey.ForMixingConsole(program.Pos);
					BlockEntityRadioMixingConsole mixingConsole = this.ResolveLoadedConsole(program.Pos);
					if (mixingConsole != null)
					{
						foreach (BlockEntityRadioMicrophone microphone in RadioWireNetworkHelper.FindMicrophones(mixingConsole))
						{
							if (microphone.IsTransmitting && !string.IsNullOrWhiteSpace(microphone.ActiveOperatorPlayerUid))
							{
								nextBindings[microphone.ActiveOperatorPlayerUid] = routeKey;
							}
						}
					}
				}
			}
			foreach (string playerUid in Enumerable.ToArray<string>(this.micOperatorToRouteKey.Keys))
			{
				if (!nextBindings.ContainsKey(playerUid))
				{
					string text;
					this.micOperatorToRouteKey.TryRemove(playerUid, ref text);
				}
			}
			foreach (KeyValuePair<string, string> entry in nextBindings)
			{
				this.micOperatorToRouteKey[entry.Key] = entry.Value;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000993C File Offset: 0x00007B3C
		private void TryStartSession(RadioRfProgramPresence program, string routeKey, BlockEntityRadioMixingConsole loadedConsole)
		{
			RadioProgramBroadcastSystem.ProgramSession session = new RadioProgramBroadcastSystem.ProgramSession
			{
				RouteKey = routeKey,
				EncoderCodec = new OpusCodec(48000, 1),
				MicDecoder = new OpusCodec(48000, 1),
				SequenceNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 100L
			};
			string hlsUrl = program.HlsStreamUrl ?? "";
			if (!string.IsNullOrWhiteSpace(hlsUrl))
			{
				RadioHlsStreamCapture capture = new RadioHlsStreamCapture();
				session.Capture = capture;
				session.FrameHandler = delegate(short[] musicFrame)
				{
					this.OnMusicFrame(routeKey, musicFrame);
				};
				capture.OnPcmFrame += session.FrameHandler;
				if (!capture.TryStart(hlsUrl))
				{
					string error = capture.LastError;
					capture.Dispose();
					Logger.server.Warning("[RadioProgram] Failed to start HLS for " + routeKey + ": " + error);
					if (!RadioProgramBroadcastSystem.HasActiveMicOnGraph(loadedConsole))
					{
						this.NotifyOperator(program, "Radio.MixingConsole.Error.FfmpegMissing");
						return;
					}
					session.Capture = null;
					session.FrameHandler = null;
					Logger.server.Notification("[RadioProgram] Mic-only program bus for " + routeKey + " (HLS unavailable).");
				}
				else
				{
					Logger.server.Notification("[RadioProgram] Mixed program started for " + routeKey + " (HLS + mic bus).");
				}
			}
			else
			{
				if (!RadioProgramBroadcastSystem.HasActiveMicOnGraph(loadedConsole))
				{
					return;
				}
				Logger.server.Notification("[RadioProgram] Mic-only program bus started for " + routeKey + ".");
			}
			this.sessions[routeKey] = session;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009ACD File Offset: 0x00007CCD
		private static bool HasActiveMicOnGraph(BlockEntityRadioMixingConsole mixingConsole)
		{
			if (mixingConsole == null)
			{
				return false;
			}
			return Enumerable.Any<BlockEntityRadioMicrophone>(RadioWireNetworkHelper.FindMicrophones(mixingConsole), (BlockEntityRadioMicrophone microphone) => microphone.IsTransmitting);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00009B00 File Offset: 0x00007D00
		private void NotifyOperator(RadioRfProgramPresence program, string langKey)
		{
			if (this.sapi == null || program == null || string.IsNullOrWhiteSpace(program.ActiveOperatorPlayerUid) || string.IsNullOrWhiteSpace(langKey))
			{
				return;
			}
			IServerPlayer operatorPlayer = this.sapi.World.PlayerByUid(program.ActiveOperatorPlayerUid) as IServerPlayer;
			if (operatorPlayer != null)
			{
				RPVoiceChatMod.SendRadioClientNotification(operatorPlayer, langKey);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00009B54 File Offset: 0x00007D54
		private void StopSession(string routeKey)
		{
			RadioProgramBroadcastSystem.ProgramSession session;
			if (!this.sessions.TryRemove(routeKey, ref session))
			{
				return;
			}
			if (session.FrameHandler != null && session.Capture != null)
			{
				session.Capture.OnPcmFrame -= session.FrameHandler;
			}
			RadioHlsStreamCapture capture = session.Capture;
			if (capture != null)
			{
				capture.Dispose();
			}
			session.MicBuffer.Clear();
			AudioPacket audioPacket;
			while (this.pendingPackets.TryDequeue(ref audioPacket))
			{
			}
			Logger.server.Notification("[RadioProgram] Program bus stopped for " + routeKey);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00009BD4 File Offset: 0x00007DD4
		private void OnMusicFrame(string routeKey, short[] musicFrame)
		{
			RadioProgramBroadcastSystem.ProgramSession session;
			if (!this.sessions.TryGetValue(routeKey, ref session) || musicFrame == null || musicFrame.Length == 0)
			{
				return;
			}
			short[] micFrame = new short[musicFrame.Length];
			session.MicBuffer.ReadFrame(musicFrame.Length, micFrame);
			short[] mixed = new short[musicFrame.Length];
			RadioProgramMixer.MixFrames(musicFrame, micFrame, mixed);
			this.EnqueueMixedFrame(session, mixed);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00009C40 File Offset: 0x00007E40
		private void EmitMicOnlyFrames(RadioProgramBroadcastSystem.ProgramSession session, short[] micSamples)
		{
			int frameSamples = 480;
			int offset = 0;
			while (offset + frameSamples <= micSamples.Length)
			{
				short[] micFrame = new short[frameSamples];
				Array.Copy(micSamples, offset, micFrame, 0, frameSamples);
				short[] array = new short[frameSamples];
				short[] mixed = new short[frameSamples];
				RadioProgramMixer.MixFrames(array, micFrame, mixed);
				this.EnqueueMixedFrame(session, mixed);
				offset += frameSamples;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009CA0 File Offset: 0x00007EA0
		private void EnqueueMixedFrame(RadioProgramBroadcastSystem.ProgramSession session, short[] mixed)
		{
			byte[] encoded = session.EncoderCodec.EncodeForProgramStream(mixed);
			if (encoded == null || encoded.Length == 0)
			{
				return;
			}
			session.SequenceNumber += 1L;
			this.pendingPackets.Enqueue(new AudioPacket
			{
				PlayerId = session.RouteKey,
				AudioData = encoded,
				Length = encoded.Length,
				VoiceLevel = VoiceLevel.Shouting,
				Frequency = 48000,
				Format = 4353,
				SequenceNumber = session.SequenceNumber,
				Codec = "Opus"
			});
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009D34 File Offset: 0x00007F34
		private static short[] DecodeMicPacket(RadioProgramBroadcastSystem.ProgramSession session, AudioPacket packet)
		{
			if (packet.Codec == "Opus")
			{
				return AudioUtils.BytesToShorts(session.MicDecoder.Decode(packet.AudioData));
			}
			if (packet.Codec == "Dummy")
			{
				return AudioUtils.BytesToShorts(packet.AudioData);
			}
			return Array.Empty<short>();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009D90 File Offset: 0x00007F90
		private void FlushPendingPackets()
		{
			AudioPacket audioPacket;
			if (this.gameServer == null)
			{
				while (this.pendingPackets.TryDequeue(ref audioPacket))
				{
				}
				return;
			}
			while (this.pendingPackets.Count > 60 && this.pendingPackets.TryDequeue(ref audioPacket))
			{
			}
			int i = 0;
			AudioPacket packet;
			while (i < 20 && this.pendingPackets.TryDequeue(ref packet))
			{
				this.gameServer.SendAudioToAllClientsInRange(packet);
				i++;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009DFC File Offset: 0x00007FFC
		public override void Dispose()
		{
			foreach (string routeKey in Enumerable.ToArray<string>(this.sessions.Keys))
			{
				this.StopSession(routeKey);
			}
			base.Dispose();
		}

		// Token: 0x04000083 RID: 131
		private readonly ConcurrentDictionary<string, RadioProgramBroadcastSystem.ProgramSession> sessions = new ConcurrentDictionary<string, RadioProgramBroadcastSystem.ProgramSession>();

		// Token: 0x04000084 RID: 132
		private readonly ConcurrentDictionary<string, string> micOperatorToRouteKey = new ConcurrentDictionary<string, string>();

		// Token: 0x04000085 RID: 133
		private readonly ConcurrentQueue<AudioPacket> pendingPackets = new ConcurrentQueue<AudioPacket>();

		// Token: 0x04000086 RID: 134
		private ICoreServerAPI sapi;

		// Token: 0x04000087 RID: 135
		private RadioVoiceRoutingSystem routing;

		// Token: 0x04000088 RID: 136
		private GameServer gameServer;

		// Token: 0x02000121 RID: 289
		private sealed class ProgramSession
		{
			// Token: 0x170001AA RID: 426
			// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0003802F File Offset: 0x0003622F
			public bool HasHlsCapture
			{
				get
				{
					RadioHlsStreamCapture capture = this.Capture;
					return capture != null && capture.IsRunning;
				}
			}

			// Token: 0x040004C6 RID: 1222
			public string RouteKey;

			// Token: 0x040004C7 RID: 1223
			public RadioHlsStreamCapture Capture;

			// Token: 0x040004C8 RID: 1224
			public OpusCodec EncoderCodec;

			// Token: 0x040004C9 RID: 1225
			public OpusCodec MicDecoder;

			// Token: 0x040004CA RID: 1226
			public RadioProgramMicBuffer MicBuffer = new RadioProgramMicBuffer();

			// Token: 0x040004CB RID: 1227
			public Action<short[]> FrameHandler;

			// Token: 0x040004CC RID: 1228
			public long SequenceNumber;
		}
	}
}
