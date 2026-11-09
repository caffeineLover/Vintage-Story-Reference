using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RPVoiceChat.GameContent.BlockEntityBehavior;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockEntityBellHammer : BlockEntity
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002335B File Offset: 0x0002155B
		[Nullable(2)]
		private BEBehaviorAnimatable Animatable
		{
			[NullableContext(2)]
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00023363 File Offset: 0x00021563
		[Nullable(2)]
		private BlockEntityAnimationUtil AnimUtil
		{
			[NullableContext(2)]
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00023376 File Offset: 0x00021576
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0002337E File Offset: 0x0002157E
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00023386 File Offset: 0x00021586
		public float PowerPercent { get; private set; }

		// Token: 0x06000692 RID: 1682 RVA: 0x000233B0 File Offset: 0x000215B0
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.DisableConsumerInstancedRenderer();
			if (api.Side == 2)
			{
				Block block = base.Block;
				string text;
				if (block == null)
				{
					text = null;
				}
				else
				{
					CompositeShape shape = block.Shape;
					if (shape == null)
					{
						text = null;
					}
					else
					{
						AssetLocation @base = shape.Base;
						text = ((@base != null) ? @base.Path : null);
					}
				}
				string shapePath = text ?? "block/bellhammer/bellhammer";
				Block block2 = base.Block;
				bool flag;
				string v;
				if (block2 == null)
				{
					flag = false;
				}
				else
				{
					RelaxedReadOnlyDictionary<string, string> variant = block2.Variant;
					flag = ((variant != null) ? new bool?(variant.TryGetValue("v", ref v)) : default(bool?)).GetValueOrDefault();
				}
				if (flag && string.Equals(v, "down", 5) && (shapePath == null || shapePath.IndexOf("bellhammer_chains", 5) < 0))
				{
					shapePath = "block/bellhammer/bellhammer_chains";
				}
				Block block3 = base.Block;
				bool flag2;
				string side;
				if (block3 == null)
				{
					flag2 = false;
				}
				else
				{
					RelaxedReadOnlyDictionary<string, string> variant2 = block3.Variant;
					flag2 = ((variant2 != null) ? new bool?(variant2.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
				}
				float num2;
				if (flag2)
				{
					float num;
					if (!(side == "north"))
					{
						if (!(side == "east"))
						{
							if (!(side == "south"))
							{
								if (!(side == "west"))
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
								num = 270f;
							}
						}
						else
						{
							num = 0f;
						}
					}
					else
					{
						num = 90f;
					}
					num2 = num;
				}
				else
				{
					num2 = 0f;
				}
				float rotDeg = num2;
				this.InitializeClientAnimator(shapePath, rotDeg);
				ICoreClientAPI capi = api as ICoreClientAPI;
				if (capi != null)
				{
					this._mechPartRenderer = new RotatingMechPartRenderer(this, capi, new AssetLocation("rpvoicechat:shapes/block/bellhammer/bellhammer_mechpart.json"), this.GetMechPartBaseRotY());
				}
			}
			if (api.Side == 1)
			{
				ICoreServerAPI coreServerAPI = api as ICoreServerAPI;
				if (coreServerAPI != null)
				{
					coreServerAPI.Event.RegisterGameTickListener(new Action<float>(this.OnServerGameTick), 100, 0);
				}
				this.TryDiscoverNetwork();
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00023588 File Offset: 0x00021788
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			if (base.Block == null)
			{
				return false;
			}
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			bool flag;
			if (animUtil == null)
			{
				flag = false;
			}
			else
			{
				Dictionary<string, AnimationMetaData> activeAnimationsByAnimCode = animUtil.activeAnimationsByAnimCode;
				int? num = (activeAnimationsByAnimCode != null) ? new int?(activeAnimationsByAnimCode.Count) : default(int?);
				int num2 = 0;
				flag = (num.GetValueOrDefault() > num2 & num != null);
			}
			if (!flag)
			{
				BlockEntityAnimationUtil animUtil2 = this.AnimUtil;
				if (((animUtil2 != null) ? animUtil2.animator : null) == null || this.AnimUtil.animator.ActiveAnimationCount <= 0)
				{
					CompositeShape blockShape = base.Block.Shape;
					if (((blockShape != null) ? blockShape.Base : null) == null)
					{
						return false;
					}
					AssetLocation shapeLoc = blockShape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
					Shape shape = Shape.TryGet(this.Api, shapeLoc);
					if (shape == null)
					{
						return false;
					}
					MeshData mesh;
					tesselator.TesselateShape(base.Block, shape, ref mesh, new Vec3f(blockShape.rotateX, blockShape.rotateY, blockShape.rotateZ), blockShape.QuantityElements, blockShape.SelectiveElements);
					mesher.AddMeshData(mesh, 1);
					return true;
				}
			}
			return true;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000236A0 File Offset: 0x000218A0
		public void TryDiscoverNetwork()
		{
			Block block = base.Block;
			string sideStr;
			if (((block != null) ? block.Variant : null) == null || !base.Block.Variant.TryGetValue("side", ref sideStr))
			{
				return;
			}
			BlockFacing frontFace = BlockFacing.FromCode(sideStr);
			if (frontFace == null)
			{
				return;
			}
			BlockFacing connectorFace = frontFace.Opposite;
			BEBehaviorMPBase mechBase = base.GetBehavior<BEBehaviorMPBase>();
			if (mechBase == null)
			{
				return;
			}
			mechBase.CreateJoinAndDiscoverNetwork(connectorFace);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00023700 File Offset: 0x00021900
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			RotatingMechPartRenderer mechPartRenderer = this._mechPartRenderer;
			if (mechPartRenderer != null)
			{
				mechPartRenderer.Dispose();
			}
			this._mechPartRenderer = null;
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this._animationEndCallbackId >= 0L)
			{
				this.Api.World.UnregisterCallback(this._animationEndCallbackId);
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00023762 File Offset: 0x00021962
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			RotatingMechPartRenderer mechPartRenderer = this._mechPartRenderer;
			if (mechPartRenderer != null)
			{
				mechPartRenderer.Dispose();
			}
			this._mechPartRenderer = null;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00023784 File Offset: 0x00021984
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetBool("enabled", this._enabled);
			tree.SetFloat("powerPercent", this.PowerPercent);
			tree.SetBool("rpvc:bhGearActive", this._syncedGearActive);
			tree.SetInt("rpvc:bhStrikeSequence", this._strikeSequence);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000237DC File Offset: 0x000219DC
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this._enabled = tree.GetBool("enabled", false);
			this.PowerPercent = tree.GetFloat("powerPercent", 0f);
			this._syncedGearActive = tree.GetBool("rpvc:bhGearActive", false);
			this._strikeSequence = tree.GetInt("rpvc:bhStrikeSequence", 0);
			this.DisableConsumerInstancedRenderer();
			if (worldForResolving.Side == 2)
			{
				Block block = base.Block;
				string text;
				if (block == null)
				{
					text = null;
				}
				else
				{
					CompositeShape shape = block.Shape;
					if (shape == null)
					{
						text = null;
					}
					else
					{
						AssetLocation @base = shape.Base;
						text = ((@base != null) ? @base.Path : null);
					}
				}
				string shapePath = text ?? "block/bellhammer/bellhammer";
				Block block2 = base.Block;
				bool flag;
				string v;
				if (block2 == null)
				{
					flag = false;
				}
				else
				{
					RelaxedReadOnlyDictionary<string, string> variant = block2.Variant;
					flag = ((variant != null) ? new bool?(variant.TryGetValue("v", ref v)) : default(bool?)).GetValueOrDefault();
				}
				if (flag && string.Equals(v, "down", 5) && (shapePath == null || shapePath.IndexOf("bellhammer_chains", 5) < 0))
				{
					shapePath = "block/bellhammer/bellhammer_chains";
				}
				Block block3 = base.Block;
				bool flag2;
				string side;
				if (block3 == null)
				{
					flag2 = false;
				}
				else
				{
					RelaxedReadOnlyDictionary<string, string> variant2 = block3.Variant;
					flag2 = ((variant2 != null) ? new bool?(variant2.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
				}
				float num2;
				if (flag2)
				{
					float num;
					if (!(side == "north"))
					{
						if (!(side == "east"))
						{
							if (!(side == "south"))
							{
								if (!(side == "west"))
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
								num = 270f;
							}
						}
						else
						{
							num = 0f;
						}
					}
					else
					{
						num = 90f;
					}
					num2 = num;
				}
				else
				{
					num2 = 0f;
				}
				float rotDeg = num2;
				this.InitializeClientAnimator(shapePath, rotDeg);
				if (this._syncedGearActive)
				{
					this.StartAnimationIfNotRunning("gear");
				}
				else
				{
					this.StopAnimation("gear");
				}
				if (this._clientLastStrikeSequence < 0)
				{
					this._clientLastStrikeSequence = this._strikeSequence;
					return;
				}
				if (this._strikeSequence != this._clientLastStrikeSequence)
				{
					this.PlaySingleShotAnimation("strike");
					this._clientLastStrikeSequence = this._strikeSequence;
				}
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000239FC File Offset: 0x00021BFC
		private void OnServerGameTick(float dt)
		{
			if (!this._enabled || base.Block == null)
			{
				return;
			}
			ICoreAPI api = this.Api;
			BlockEntityBellHammer blockEntityBellHammer;
			if (api == null)
			{
				blockEntityBellHammer = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				if (world == null)
				{
					blockEntityBellHammer = null;
				}
				else
				{
					IBlockAccessor blockAccessor = world.BlockAccessor;
					blockEntityBellHammer = ((blockAccessor != null) ? blockAccessor.GetBlockEntity(this.Pos) : null);
				}
			}
			if (blockEntityBellHammer != this)
			{
				return;
			}
			float speed = this.GetTrueSpeed();
			this.PowerPercent = speed;
			int percentDisplay = (int)(speed * 100f);
			bool gearActiveNow = this._enabled && speed >= 0.25f;
			if (gearActiveNow != this._syncedGearActive)
			{
				this._syncedGearActive = gearActiveNow;
				this.MarkDirty(false, null);
			}
			if (percentDisplay != this._lastSyncedPowerPercent)
			{
				this._lastSyncedPowerPercent = percentDisplay;
				this.MarkDirty(false, null);
			}
			if (speed < 0.25f)
			{
				return;
			}
			BlockPos bellPos = this.GetAdjacentBellPosition();
			if (bellPos == null)
			{
				this._hadBellLastTick = false;
				return;
			}
			if (!this._hadBellLastTick)
			{
				this._hadBellLastTick = true;
				this._lastTotalHours = this.Api.World.Calendar.TotalHours;
				this._strikeProgress = 0f;
			}
			if (this._animationPlaying)
			{
				return;
			}
			double nowTotalHours = this.Api.World.Calendar.TotalHours;
			float gameHoursThisTick = (float)(nowTotalHours - this._lastTotalHours);
			this._lastTotalHours = nowTotalHours;
			if (gameHoursThisTick > 0f && gameHoursThisTick < 1f)
			{
				this._strikeProgress += gameHoursThisTick * BlockEntityBellHammer.ComputeStrikeRatePerGameHour(speed);
			}
			if (this._strikeProgress < 1f)
			{
				return;
			}
			this._strikeProgress -= 1f;
			this.StartStrikeSequence(bellPos);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00023B88 File Offset: 0x00021D88
		private float GetTrueSpeed()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return 0f;
			}
			return GameMath.Clamp(consumer.TrueSpeed, 0f, 1f);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00023BBC File Offset: 0x00021DBC
		private static float ComputeStrikeRatePerGameHour(float speed)
		{
			if (speed <= 0.25f)
			{
				return 1f;
			}
			float t = (speed - 0.25f) / 0.75f;
			return 1f + t * 59f;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00023BF4 File Offset: 0x00021DF4
		[NullableContext(2)]
		private BlockPos GetAdjacentBellPosition()
		{
			BlockFacing face = this.GetBellDirection();
			if (face == null)
			{
				return null;
			}
			IBlockAccessor ba = this.Api.World.BlockAccessor;
			BlockPos front = this.Pos.AddCopy(face);
			if (BlockEntityBellHammer.IsBellBlock(ba.GetBlock(front)))
			{
				return front;
			}
			BlockPos front2 = front.AddCopy(face);
			if (BlockEntityBellHammer.IsChurchBellBlock(ba.GetBlock(front2)))
			{
				return front2;
			}
			return null;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00023C54 File Offset: 0x00021E54
		private BlockFacing GetBellDirection()
		{
			return BlockFacing.FromCode(this.GetBlockSide());
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00023C64 File Offset: 0x00021E64
		private static bool IsBellBlock(Block block)
		{
			if (block == null)
			{
				return false;
			}
			AssetLocation code = block.Code;
			string path = ((code != null) ? code.Path : null) ?? "";
			return path.StartsWith("carillonbell") || path.StartsWith("churchbell");
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00023CAC File Offset: 0x00021EAC
		private static bool IsChurchBellBlock(Block block)
		{
			if (block == null)
			{
				return false;
			}
			AssetLocation code = block.Code;
			return (((code != null) ? code.Path : null) ?? "").StartsWith("churchbell");
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00023CD8 File Offset: 0x00021ED8
		private string GetBlockSide()
		{
			Block block = base.Block;
			if (((block != null) ? block.Variant : null) == null)
			{
				return "north";
			}
			string side;
			if (!base.Block.Variant.TryGetValue("side", ref side))
			{
				return "north";
			}
			return side;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00023D20 File Offset: 0x00021F20
		private void StartStrikeSequence(BlockPos bellPos)
		{
			this._animationPlaying = true;
			this._strikeSequence++;
			this._syncedGearActive = false;
			this.MarkDirty(true, null);
			if (this._animationEndCallbackId >= 0L)
			{
				this.Api.World.UnregisterCallback(this._animationEndCallbackId);
			}
			this._animationEndCallbackId = this.Api.World.RegisterCallback(delegate(float _)
			{
				this._animationEndCallbackId = -1L;
				this._animationPlaying = false;
				ICoreAPI api = this.Api;
				BlockEntityBellHammer blockEntityBellHammer;
				if (api == null)
				{
					blockEntityBellHammer = null;
				}
				else
				{
					IWorldAccessor world = api.World;
					if (world == null)
					{
						blockEntityBellHammer = null;
					}
					else
					{
						IBlockAccessor blockAccessor = world.BlockAccessor;
						blockEntityBellHammer = ((blockAccessor != null) ? blockAccessor.GetBlockEntity(this.Pos) : null);
					}
				}
				if (blockEntityBellHammer == this)
				{
					this.TriggerBell(bellPos);
					this._syncedGearActive = (this._enabled && this.PowerPercent >= 0.25f);
					this.MarkDirty(false, null);
				}
			}, 400);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00023DAC File Offset: 0x00021FAC
		private void TriggerBell(BlockPos bellPos)
		{
			BlockEntity be = this.Api.World.BlockAccessor.GetBlockEntity(bellPos);
			if (be == null)
			{
				return;
			}
			BlockEntityCarillonBell carillon = be as BlockEntityCarillonBell;
			if (carillon != null)
			{
				carillon.OnRung();
				return;
			}
			BEBehaviorSoundable behavior = be.GetBehavior<BEBehaviorSoundable>();
			if (behavior == null)
			{
				return;
			}
			behavior.OnRung();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00023DF8 File Offset: 0x00021FF8
		public bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return true;
			}
			this._enabled = !this._enabled;
			if (this._enabled)
			{
				this.TryDiscoverNetwork();
				this._lastTotalHours = this.Api.World.Calendar.TotalHours;
				this._strikeProgress = 0f;
			}
			else
			{
				this._syncedGearActive = false;
				this.StopAnimation("gear");
			}
			this.MarkDirty(false, null);
			return true;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00023E84 File Offset: 0x00022084
		private void InitializeClientAnimator(string shapePath, float rotDeg)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null && !string.IsNullOrWhiteSpace(shapePath))
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/" + shapePath + ".json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, shapePath, Array.Empty<string>());
						}
					}
					animUtil.InitializeAnimator(shapePath, null, null, new Vec3f(0f, rotDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00023F5C File Offset: 0x0002215C
		private void StartAnimationIfNotRunning(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			if (animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				return;
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00023F9D File Offset: 0x0002219D
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00023FB0 File Offset: 0x000221B0
		private void PlaySingleShotAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			if (animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				animUtil.StopAnimation(animationCode);
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00023FF8 File Offset: 0x000221F8
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			base.GetBlockInfo(forPlayer, dsc);
			string enabledStr = this._enabled ? UIUtils.I18n("BellHammer.Enabled") : UIUtils.I18n("BellHammer.Disabled");
			dsc.AppendLine(enabledStr);
			dsc.AppendLine(UIUtils.I18n("BellHammer.Power", new object[]
			{
				(int)(this.PowerPercent * 100f)
			}));
			if (this.GetAdjacentBellPosition() == null)
			{
				dsc.AppendLine(UIUtils.I18n("BellHammer.NoBell"));
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00024080 File Offset: 0x00022280
		private void DisableConsumerInstancedRenderer()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return;
			}
			consumer.Shape = null;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000240A0 File Offset: 0x000222A0
		private float GetMechPartBaseRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = true;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = !((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				return 0f;
			}
			float result;
			if (!(side == "north"))
			{
				if (!(side == "east"))
				{
					if (!(side == "south"))
					{
						if (!(side == "west"))
						{
							result = 0f;
						}
						else
						{
							result = 180f;
						}
					}
					else
					{
						result = 270f;
					}
				}
				else
				{
					result = 0f;
				}
			}
			else
			{
				result = 90f;
			}
			return result;
		}

		// Token: 0x04000321 RID: 801
		private const string StrikeAnimationCode = "strike";

		// Token: 0x04000322 RID: 802
		private const string GearAnimationCode = "gear";

		// Token: 0x04000323 RID: 803
		public const float MinSpeedThreshold = 0.25f;

		// Token: 0x04000324 RID: 804
		public const float StrikeRateAtMinSpeed = 1f;

		// Token: 0x04000325 RID: 805
		public const float StrikeRateAtMaxSpeed = 60f;

		// Token: 0x04000326 RID: 806
		public const float AnimationToBellDelaySeconds = 0.4f;

		// Token: 0x04000327 RID: 807
		private bool _enabled;

		// Token: 0x04000328 RID: 808
		private float _strikeProgress;

		// Token: 0x04000329 RID: 809
		private double _lastTotalHours;

		// Token: 0x0400032A RID: 810
		private bool _animationPlaying;

		// Token: 0x0400032B RID: 811
		private bool _hadBellLastTick;

		// Token: 0x0400032C RID: 812
		private long _animationEndCallbackId = -1L;

		// Token: 0x0400032D RID: 813
		private int _lastSyncedPowerPercent = -1;

		// Token: 0x0400032E RID: 814
		private int _strikeSequence;

		// Token: 0x0400032F RID: 815
		private int _clientLastStrikeSequence = -1;

		// Token: 0x04000330 RID: 816
		private bool _syncedGearActive;

		// Token: 0x04000331 RID: 817
		[Nullable(2)]
		private RotatingMechPartRenderer _mechPartRenderer;
	}
}
