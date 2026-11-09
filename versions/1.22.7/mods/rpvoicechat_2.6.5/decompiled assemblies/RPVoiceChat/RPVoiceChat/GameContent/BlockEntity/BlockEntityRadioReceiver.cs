using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000EA RID: 234
	public class BlockEntityRadioReceiver : BEWireNode, IWireTypedNode
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000284CD File Offset: 0x000266CD
		public string TunedFrequency
		{
			get
			{
				return this.tunedFrequency ?? "";
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x000284DE File Offset: 0x000266DE
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x000284E6 File Offset: 0x000266E6
		public int PlaybackRangeBlocks
		{
			get
			{
				return this.playbackRangeBlocks;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x000284EE File Offset: 0x000266EE
		public int PlaybackVolumePercent
		{
			get
			{
				return this.playbackVolumePercent;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x000284F6 File Offset: 0x000266F6
		public string HeardStationName
		{
			get
			{
				return this.heardStationName ?? "";
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00028507 File Offset: 0x00026707
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0002850A File Offset: 0x0002670A
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0002850D File Offset: 0x0002670D
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.RadioReceiver;
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00028510 File Offset: 0x00026710
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.playbackRangeBlocks = GameMath.Clamp(this.playbackRangeBlocks, 0, 15);
			this.playbackVolumePercent = GameMath.Clamp(this.playbackVolumePercent, 0, 100);
			if (api.Side == 1)
			{
				this.stationNameListenerId = api.Event.RegisterGameTickListener(new Action<float>(this.OnServerStationNameTick), 1000, 0);
				this.PublishRfPresence();
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00028580 File Offset: 0x00026780
		public bool OnInteract(IPlayer byPlayer)
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				return true;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog != null && radioReceiverDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new RadioReceiverDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000285E8 File Offset: 0x000267E8
		public void RequestSetFrequency(string desired)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetReceiverFrequency,
				Value = (desired ?? "")
			});
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00028648 File Offset: 0x00026848
		public void RequestSetEnabled(bool enabled)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetReceiverEnabled,
				IntValue = ((enabled > false) ? 1 : 0)
			});
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000286A4 File Offset: 0x000268A4
		public void RequestSetPlaybackRange(int rangeBlocks)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetReceiverPlaybackRange,
				IntValue = GameMath.Clamp(rangeBlocks, 0, 15)
			});
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00028704 File Offset: 0x00026904
		public void RequestSetPlaybackVolume(int volumePercent)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetReceiverPlaybackVolume,
				IntValue = GameMath.Clamp(volumePercent, 0, 100)
			});
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00028764 File Offset: 0x00026964
		public static float GetPlaybackVolumeGainAtSource(ICoreClientAPI capi, Vec3d sourceOverride, int dimension)
		{
			bool flag;
			if (capi == null)
			{
				flag = (null != null);
			}
			else
			{
				IClientWorldAccessor world = capi.World;
				flag = (((world != null) ? world.BlockAccessor : null) != null);
			}
			if (!flag || sourceOverride == null)
			{
				return -1f;
			}
			BlockPos blockPos = new BlockPos((int)Math.Floor(sourceOverride.X), (int)Math.Floor(sourceOverride.Y), (int)Math.Floor(sourceOverride.Z));
			blockPos.dimension = dimension;
			BlockEntity blockEntity = capi.World.BlockAccessor.GetBlockEntity(blockPos);
			BlockEntityRadioReceiver receiver = blockEntity as BlockEntityRadioReceiver;
			if (receiver != null)
			{
				return (float)receiver.PlaybackVolumePercent / 100f;
			}
			BlockEntitySpeaker speaker = blockEntity as BlockEntitySpeaker;
			if (speaker != null)
			{
				using (IEnumerator<BlockEntityRadioReceiver> enumerator = RadioWireNetworkHelper.FindReceivers(speaker).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return (float)enumerator.Current.PlaybackVolumePercent / 100f;
					}
				}
			}
			return -1f;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00028854 File Offset: 0x00026A54
		public void SetTunedFrequency(string desired)
		{
			this.tunedFrequency = (desired ?? "").Trim();
			this.RefreshHeardStationName(true);
			this.MarkDirty(false, null);
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog != null)
			{
				radioReceiverDialog.RefreshData();
			}
			this.PublishRfPresence();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00028891 File Offset: 0x00026A91
		public void SetEnabled(bool enabled)
		{
			if (this.isEnabled == enabled)
			{
				return;
			}
			this.isEnabled = enabled;
			this.MarkDirty(true, null);
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog != null)
			{
				radioReceiverDialog.RefreshData();
			}
			this.PublishRfPresence();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000288C4 File Offset: 0x00026AC4
		public void SetPlaybackRange(int rangeBlocks)
		{
			int clamped = GameMath.Clamp(rangeBlocks, 0, 15);
			if (this.playbackRangeBlocks == clamped)
			{
				return;
			}
			this.playbackRangeBlocks = clamped;
			this.MarkDirty(true, null);
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog != null)
			{
				radioReceiverDialog.RefreshData();
			}
			this.PublishRfPresence();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002890C File Offset: 0x00026B0C
		public void SetPlaybackVolume(int volumePercent)
		{
			int clamped = GameMath.Clamp(volumePercent, 0, 100);
			if (this.playbackVolumePercent == clamped)
			{
				return;
			}
			this.playbackVolumePercent = clamped;
			this.MarkDirty(true, null);
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog == null)
			{
				return;
			}
			radioReceiverDialog.RefreshData();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002894C File Offset: 0x00026B4C
		public void PublishRfPresence()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || this.Pos == null)
			{
				return;
			}
			List<RadioRfAcousticPresence> list = new List<RadioRfAcousticPresence>();
			list.Add(new RadioRfAcousticPresence
			{
				Pos = this.Pos.Copy(),
				Dimension = this.Pos.dimension,
				RangeBlocks = this.playbackRangeBlocks
			});
			List<RadioRfAcousticPresence> acousticPoints = list;
			foreach (BlockEntitySpeaker speaker in RadioWireNetworkHelper.FindSpeakers(this))
			{
				if (!(((speaker != null) ? speaker.Pos : null) == null))
				{
					acousticPoints.Add(new RadioRfAcousticPresence
					{
						Pos = speaker.Pos.Copy(),
						Dimension = speaker.Pos.dimension,
						RangeBlocks = speaker.VoiceEmissionRangeBlocks
					});
				}
			}
			RadioRfPresenceRegistry.UpsertReceiver(new RadioRfReceiverPresence
			{
				Pos = this.Pos.Copy(),
				Dimension = this.Pos.dimension,
				TunedFrequency = RadioFrequencyUtil.Normalize(this.tunedFrequency),
				PlaybackRangeBlocks = this.playbackRangeBlocks,
				IsEnabled = this.isEnabled,
				AcousticPoints = acousticPoints
			});
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00028AA4 File Offset: 0x00026CA4
		private void OnServerStationNameTick(float dt)
		{
			this.RefreshHeardStationName(false);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00028AB0 File Offset: 0x00026CB0
		private void RefreshHeardStationName(bool force)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || this.Api.World == null)
			{
				return;
			}
			string resolved = "";
			if (this.isEnabled)
			{
				resolved = RadioStationNameResolver.Resolve(this.Api.World, this.tunedFrequency);
			}
			if (!force && string.Equals(this.heardStationName, resolved, 4))
			{
				return;
			}
			this.heardStationName = resolved;
			this.MarkDirty(false, null);
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog == null)
			{
				return;
			}
			radioReceiverDialog.RefreshData();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00028B3C File Offset: 0x00026D3C
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.tunedFrequency = tree.GetString("rpvc:radioReceiverFrequency", this.tunedFrequency);
			this.isEnabled = tree.GetBool("rpvc:radioReceiverEnabled", false);
			this.playbackRangeBlocks = GameMath.Clamp(tree.GetInt("rpvc:radioReceiverPlaybackRange", 8), 0, 15);
			this.playbackVolumePercent = GameMath.Clamp(tree.GetInt("rpvc:radioReceiverPlaybackVolume", 100), 0, 100);
			this.heardStationName = (tree.GetString("rpvc:radioReceiverHeardStationName", this.heardStationName) ?? "");
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog == null)
			{
				return;
			}
			radioReceiverDialog.RefreshData();
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00028BE0 File Offset: 0x00026DE0
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("rpvc:radioReceiverFrequency", this.tunedFrequency ?? "");
			tree.SetBool("rpvc:radioReceiverEnabled", this.isEnabled);
			tree.SetInt("rpvc:radioReceiverPlaybackRange", this.playbackRangeBlocks);
			tree.SetInt("rpvc:radioReceiverPlaybackVolume", this.playbackVolumePercent);
			tree.SetString("rpvc:radioReceiverHeardStationName", this.heardStationName ?? "");
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00028C5C File Offset: 0x00026E5C
		public override void OnBlockRemoved()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioRfPresenceRegistry.RemoveReceiver(this.Pos);
			}
			this.Unregister();
			base.OnBlockRemoved();
			RadioReceiverDialog radioReceiverDialog = this.dialog;
			if (radioReceiverDialog == null)
			{
				return;
			}
			radioReceiverDialog.TryClose();
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00028CA8 File Offset: 0x00026EA8
		public override void OnBlockUnloaded()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.PublishRfPresence();
			}
			this.Unregister();
			base.OnBlockUnloaded();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00028CD4 File Offset: 0x00026ED4
		private void Unregister()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this.stationNameListenerId != -1L)
			{
				this.Api.Event.UnregisterGameTickListener(this.stationNameListenerId);
				this.stationNameListenerId = -1L;
			}
		}

		// Token: 0x04000361 RID: 865
		public const int MinPlaybackRangeBlocks = 0;

		// Token: 0x04000362 RID: 866
		public const int MaxPlaybackRangeBlocks = 15;

		// Token: 0x04000363 RID: 867
		public const int DefaultPlaybackRangeBlocks = 8;

		// Token: 0x04000364 RID: 868
		public const int MaxWiredSpeakers = 4;

		// Token: 0x04000365 RID: 869
		public const int MinPlaybackVolumePercent = 0;

		// Token: 0x04000366 RID: 870
		public const int MaxPlaybackVolumePercent = 100;

		// Token: 0x04000367 RID: 871
		public const int DefaultPlaybackVolumePercent = 100;

		// Token: 0x04000368 RID: 872
		private RadioReceiverDialog dialog;

		// Token: 0x04000369 RID: 873
		private string tunedFrequency = "100.0";

		// Token: 0x0400036A RID: 874
		private bool isEnabled;

		// Token: 0x0400036B RID: 875
		private int playbackRangeBlocks = 8;

		// Token: 0x0400036C RID: 876
		private int playbackVolumePercent = 100;

		// Token: 0x0400036D RID: 877
		private string heardStationName = "";

		// Token: 0x0400036E RID: 878
		private long stationNameListenerId = -1L;
	}
}
