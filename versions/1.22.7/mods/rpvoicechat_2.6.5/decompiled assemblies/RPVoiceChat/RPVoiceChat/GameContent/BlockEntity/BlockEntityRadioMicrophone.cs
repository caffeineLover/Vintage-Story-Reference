using System;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E7 RID: 231
	public class BlockEntityRadioMicrophone : BEWireNode, IWireTypedNode, IRadioVoiceInput
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00026DD4 File Offset: 0x00024FD4
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00026DD7 File Offset: 0x00024FD7
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00026DDA File Offset: 0x00024FDA
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Radio;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00026DDD File Offset: 0x00024FDD
		public int VoiceCaptureRangeBlocks
		{
			get
			{
				return ServerConfigManager.RadioMicrophoneCaptureDistance;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x00026DE4 File Offset: 0x00024FE4
		public bool IsTransmitting
		{
			get
			{
				return this.isTransmitting;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00026DEC File Offset: 0x00024FEC
		public string ActiveOperatorPlayerUid
		{
			get
			{
				return this.activeOperatorPlayerUid ?? "";
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x00026DFD File Offset: 0x00024FFD
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00026E05 File Offset: 0x00025005
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				BEBehaviorAnimatable animatable = this.Animatable;
				if (animatable == null)
				{
					return null;
				}
				return animatable.animUtil;
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00026E18 File Offset: 0x00025018
		public bool IsBusyForOtherPlayer(string playerUid)
		{
			return this.isTransmitting && !string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid) && this.activeOperatorPlayerUid != playerUid;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00026E3D File Offset: 0x0002503D
		public bool IsOperator(string playerUid)
		{
			return !this.isTransmitting || string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid) || this.activeOperatorPlayerUid == playerUid;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00026E62 File Offset: 0x00025062
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			base.OnConnectionsChanged += new Action(this.OnRadioWireConnectionsChanged);
			if (api.Side == 1)
			{
				RadioBlockIndex.RegisterMicrophone(this.Pos);
				return;
			}
			if (api.Side == 2)
			{
				this.SyncTransmitAnimationState();
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00026EA4 File Offset: 0x000250A4
		private void OnRadioWireConnectionsChanged()
		{
			ICoreServerAPI sapi = this.Api as ICoreServerAPI;
			if (sapi != null)
			{
				WireTopologyConnectivity.NotifyNode(sapi, this);
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00026EC8 File Offset: 0x000250C8
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
			RadioMicrophoneDialog radioMicrophoneDialog = this.dialog;
			if (radioMicrophoneDialog != null && radioMicrophoneDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new RadioMicrophoneDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00026F30 File Offset: 0x00025130
		public void RequestSetTransmitting(bool enabled)
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
				Operation = RadioSettingsOperation.SetMicrophoneTransmit,
				IntValue = ((enabled > false) ? 1 : 0)
			});
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00026F8C File Offset: 0x0002518C
		public bool SetTransmitting(IPlayer byPlayer, bool enabled)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || byPlayer == null)
			{
				return false;
			}
			if (enabled)
			{
				if (this.isTransmitting && !string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid) && this.activeOperatorPlayerUid != byPlayer.PlayerUID)
				{
					return false;
				}
				this.isTransmitting = true;
				this.activeOperatorPlayerUid = byPlayer.PlayerUID;
			}
			else
			{
				if (this.isTransmitting && !string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid) && this.activeOperatorPlayerUid != byPlayer.PlayerUID)
				{
					return false;
				}
				this.ClearTransmissionInternal();
			}
			this.MarkDirty(true, null);
			return true;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00027034 File Offset: 0x00025234
		public void ClearTransmission()
		{
			if (!this.isTransmitting && string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid))
			{
				return;
			}
			string previousOperator = this.activeOperatorPlayerUid;
			this.ClearTransmissionInternal();
			this.MarkDirty(true, null);
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && !string.IsNullOrWhiteSpace(previousOperator))
			{
				RadioVoiceRoutingSystem modSystem = this.Api.ModLoader.GetModSystem<RadioVoiceRoutingSystem>(true);
				if (modSystem == null)
				{
					return;
				}
				modSystem.ClearMicRoute(previousOperator);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000270A7 File Offset: 0x000252A7
		private void ClearTransmissionInternal()
		{
			this.isTransmitting = false;
			this.activeOperatorPlayerUid = "";
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000270BC File Offset: 0x000252BC
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.isTransmitting = tree.GetBool("rpvc:radioMicTransmitting", false);
			this.activeOperatorPlayerUid = tree.GetString("rpvc:radioMicOperatorUid", "");
			RadioMicrophoneDialog radioMicrophoneDialog = this.dialog;
			if (radioMicrophoneDialog != null)
			{
				radioMicrophoneDialog.RefreshData();
			}
			this.SyncTransmitAnimationState();
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00027110 File Offset: 0x00025310
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetBool("rpvc:radioMicTransmitting", this.isTransmitting);
			tree.SetString("rpvc:radioMicOperatorUid", this.activeOperatorPlayerUid ?? "");
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00027144 File Offset: 0x00025344
		public override void OnBlockRemoved()
		{
			this.ClearTransmission();
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioBlockIndex.UnregisterMicrophone(this.Pos);
			}
			base.OnBlockRemoved();
			RadioMicrophoneDialog radioMicrophoneDialog = this.dialog;
			if (radioMicrophoneDialog == null)
			{
				return;
			}
			radioMicrophoneDialog.TryClose();
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00027190 File Offset: 0x00025390
		public override void OnBlockUnloaded()
		{
			this.ClearTransmission();
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioBlockIndex.UnregisterMicrophone(this.Pos);
			}
			base.OnBlockUnloaded();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000271C0 File Offset: 0x000253C0
		private void SyncTransmitAnimationState()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			this.InitializeClientAnimator();
			if (this.AnimUtil == null)
			{
				return;
			}
			if (this.isTransmitting)
			{
				this.StartAnimationIfNotRunning("playing-sound");
				return;
			}
			this.StopAnimation("playing-sound");
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00027218 File Offset: 0x00025418
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					string text;
					if (block == null)
					{
						text = null;
					}
					else
					{
						CompositeShape shape2 = block.Shape;
						if (shape2 == null)
						{
							text = null;
						}
						else
						{
							AssetLocation @base = shape2.Base;
							text = ((@base != null) ? @base.Path : null);
						}
					}
					string shapePath = text ?? "block/radiomicrophone";
					Block block2 = base.Block;
					if (((block2 != null) ? block2.Code : null) != null && !string.IsNullOrWhiteSpace(shapePath))
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/" + shapePath + ".json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, shapePath, Array.Empty<string>());
						}
					}
					animUtil.InitializeAnimator(shapePath, null, null, new Vec3f(0f, this.GetBlockSideRotY(), 0f));
					return;
				}
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00027328 File Offset: 0x00025528
		private void StartAnimationIfNotRunning(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null || animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				return;
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00027368 File Offset: 0x00025568
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002737C File Offset: 0x0002557C
		private float GetBlockSideRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float result;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x0400034F RID: 847
		private const string TransmitAnimationCode = "playing-sound";

		// Token: 0x04000350 RID: 848
		private RadioMicrophoneDialog dialog;

		// Token: 0x04000351 RID: 849
		private bool isTransmitting;

		// Token: 0x04000352 RID: 850
		private string activeOperatorPlayerUid = "";
	}
}
