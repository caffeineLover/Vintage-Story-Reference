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
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	public class BlockEntityBellHammer : BlockEntity
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001BD2F File Offset: 0x00019F2F
		[Nullable(2)]
		private BEBehaviorAnimatable Animatable
		{
			[NullableContext(2)]
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001BD37 File Offset: 0x00019F37
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0001BD4A File Offset: 0x00019F4A
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001BD52 File Offset: 0x00019F52
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x0001BD5A File Offset: 0x00019F5A
		public float PowerPercent { get; private set; }

		// Token: 0x06000534 RID: 1332 RVA: 0x0001BD84 File Offset: 0x00019F84
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

		// Token: 0x06000535 RID: 1333 RVA: 0x0001BF5C File Offset: 0x0001A15C
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

		// Token: 0x06000536 RID: 1334 RVA: 0x0001C074 File Offset: 0x0001A274
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

		// Token: 0x06000537 RID: 1335 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
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

		// Token: 0x06000538 RID: 1336 RVA: 0x0001C136 File Offset: 0x0001A336
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

		// Token: 0x06000539 RID: 1337 RVA: 0x0001C158 File Offset: 0x0001A358
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetBool("enabled", this._enabled);
			tree.SetFloat("powerPercent", this.PowerPercent);
			tree.SetBool("rpvc:bhGearActive", this._syncedGearActive);
			tree.SetInt("rpvc:bhStrikeSequence", this._strikeSequence);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001C1B0 File Offset: 0x0001A3B0
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

		// Token: 0x0600053B RID: 1339 RVA: 0x0001C3D0 File Offset: 0x0001A5D0
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

		// Token: 0x0600053C RID: 1340 RVA: 0x0001C55C File Offset: 0x0001A75C
		private float GetTrueSpeed()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return 0f;
			}
			return GameMath.Clamp(consumer.TrueSpeed, 0f, 1f);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001C590 File Offset: 0x0001A790
		private static float ComputeStrikeRatePerGameHour(float speed)
		{
			if (speed <= 0.25f)
			{
				return 1f;
			}
			float t = (speed - 0.25f) / 0.75f;
			return 1f + t * 59f;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001C5C8 File Offset: 0x0001A7C8
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

		// Token: 0x0600053F RID: 1343 RVA: 0x0001C628 File Offset: 0x0001A828
		private BlockFacing GetBellDirection()
		{
			return BlockFacing.FromCode(this.GetBlockSide());
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001C638 File Offset: 0x0001A838
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

		// Token: 0x06000541 RID: 1345 RVA: 0x0001C680 File Offset: 0x0001A880
		private static bool IsChurchBellBlock(Block block)
		{
			if (block == null)
			{
				return false;
			}
			AssetLocation code = block.Code;
			return (((code != null) ? code.Path : null) ?? "").StartsWith("churchbell");
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001C6AC File Offset: 0x0001A8AC
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

		// Token: 0x06000543 RID: 1347 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
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

		// Token: 0x06000544 RID: 1348 RVA: 0x0001C780 File Offset: 0x0001A980
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

		// Token: 0x06000545 RID: 1349 RVA: 0x0001C7CC File Offset: 0x0001A9CC
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

		// Token: 0x06000546 RID: 1350 RVA: 0x0001C858 File Offset: 0x0001AA58
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

		// Token: 0x06000547 RID: 1351 RVA: 0x0001C930 File Offset: 0x0001AB30
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

		// Token: 0x06000548 RID: 1352 RVA: 0x0001C971 File Offset: 0x0001AB71
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001C984 File Offset: 0x0001AB84
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

		// Token: 0x0600054A RID: 1354 RVA: 0x0001C9CC File Offset: 0x0001ABCC
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

		// Token: 0x0600054B RID: 1355 RVA: 0x0001CA54 File Offset: 0x0001AC54
		private void DisableConsumerInstancedRenderer()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return;
			}
			consumer.Shape = null;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001CA74 File Offset: 0x0001AC74
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

		// Token: 0x04000274 RID: 628
		private const string StrikeAnimationCode = "strike";

		// Token: 0x04000275 RID: 629
		private const string GearAnimationCode = "gear";

		// Token: 0x04000276 RID: 630
		public const float MinSpeedThreshold = 0.25f;

		// Token: 0x04000277 RID: 631
		public const float StrikeRateAtMinSpeed = 1f;

		// Token: 0x04000278 RID: 632
		public const float StrikeRateAtMaxSpeed = 60f;

		// Token: 0x04000279 RID: 633
		public const float AnimationToBellDelaySeconds = 0.4f;

		// Token: 0x0400027A RID: 634
		private bool _enabled;

		// Token: 0x0400027B RID: 635
		private float _strikeProgress;

		// Token: 0x0400027C RID: 636
		private double _lastTotalHours;

		// Token: 0x0400027D RID: 637
		private bool _animationPlaying;

		// Token: 0x0400027E RID: 638
		private bool _hadBellLastTick;

		// Token: 0x0400027F RID: 639
		private long _animationEndCallbackId = -1L;

		// Token: 0x04000280 RID: 640
		private int _lastSyncedPowerPercent = -1;

		// Token: 0x04000281 RID: 641
		private int _strikeSequence;

		// Token: 0x04000282 RID: 642
		private int _clientLastStrikeSequence = -1;

		// Token: 0x04000283 RID: 643
		private bool _syncedGearActive;

		// Token: 0x04000284 RID: 644
		[Nullable(2)]
		private RotatingMechPartRenderer _mechPartRenderer;
	}
}
