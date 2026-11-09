using System;
using RPVoiceChat.GameContent.Block;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E5 RID: 229
	public class BlockEntityRadioAntennaPart : BlockEntity, IBlockEntityWithCustomLightPosition
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00025C0F File Offset: 0x00023E0F
		public BlockPos BaseEmitterPos
		{
			get
			{
				return this.baseEmitterPos;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00025C18 File Offset: 0x00023E18
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.ResolveBaseEmitter();
			int intervalMs = (api.Side == 1) ? 250 : 50;
			this.beaconTickId = this.RegisterGameTickListener(new Action<float>(this.OnBeaconTick), intervalMs, 0);
			if (api.Side == 1)
			{
				this.SyncBeaconPowered();
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00025C6E File Offset: 0x00023E6E
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			this.ResolveBaseEmitter();
			this.NotifyBaseEmitterRangeChanged();
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.SyncBeaconPowered();
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00025CA0 File Offset: 0x00023EA0
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.NotifyBaseEmitterRangeChanged();
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00025CAE File Offset: 0x00023EAE
		public override void OnBlockUnloaded()
		{
			if (this.beaconTickId != 0L)
			{
				this.UnregisterGameTickListener(this.beaconTickId);
				this.beaconTickId = 0L;
			}
			base.OnBlockUnloaded();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00025CD2 File Offset: 0x00023ED2
		public Vec3d GetLightOrigin()
		{
			return this.Pos.ToVec3d().Add(BlockEntityRadioAntennaPart.BulbLightLocalOffset.X, BlockEntityRadioAntennaPart.BulbLightLocalOffset.Y, BlockEntityRadioAntennaPart.BulbLightLocalOffset.Z);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00025D04 File Offset: 0x00023F04
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			int x = tree.GetInt("rpvc:radioAntennaBaseX", int.MinValue);
			int y = tree.GetInt("rpvc:radioAntennaBaseY", int.MinValue);
			int z = tree.GetInt("rpvc:radioAntennaBaseZ", int.MinValue);
			this.baseEmitterPos = ((x == int.MinValue) ? null : new BlockPos(x, y, z));
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00025D68 File Offset: 0x00023F68
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			if (this.baseEmitterPos != null)
			{
				tree.SetInt("rpvc:radioAntennaBaseX", this.baseEmitterPos.X);
				tree.SetInt("rpvc:radioAntennaBaseY", this.baseEmitterPos.Y);
				tree.SetInt("rpvc:radioAntennaBaseZ", this.baseEmitterPos.Z);
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00025DCC File Offset: 0x00023FCC
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			if (base.Block == null || this.Api == null)
			{
				return false;
			}
			AssetLocation shapeLoc = (this.Api.World.BlockAccessor.GetBlock(this.Pos.UpCopy(1)) is RadioAntennaPartBlock) ? BlockEntityRadioAntennaPart.PartShapeLoc : BlockEntityRadioAntennaPart.TopShapeLoc;
			Shape shape = Shape.TryGet(this.Api, shapeLoc);
			if (shape == null)
			{
				return false;
			}
			CompositeShape blockShape = base.Block.Shape;
			MeshData mesh;
			tesselator.TesselateShape(base.Block, shape, ref mesh, new Vec3f((blockShape != null) ? blockShape.rotateX : 0f, (blockShape != null) ? blockShape.rotateY : 0f, (blockShape != null) ? blockShape.rotateZ : 0f), (blockShape != null) ? blockShape.QuantityElements : default(int?), (blockShape != null) ? blockShape.SelectiveElements : null);
			mesher.AddMeshData(mesh, 1);
			base.OnTesselation(mesher, tesselator);
			return true;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00025EB8 File Offset: 0x000240B8
		public void ResolveBaseEmitter()
		{
			ICoreAPI api = this.Api;
			bool flag;
			if (api == null)
			{
				flag = (null != null);
			}
			else
			{
				IWorldAccessor world = api.World;
				flag = (((world != null) ? world.BlockAccessor : null) != null);
			}
			if (!flag)
			{
				return;
			}
			BlockPos scan = this.Pos.DownCopy(1);
			while (scan.Y >= 0)
			{
				if (this.Api.World.BlockAccessor.GetBlockEntity(scan) is BlockEntityRadioEmitter)
				{
					this.baseEmitterPos = scan.Copy();
					this.MarkDirty(false, null);
					return;
				}
				BlockEntityRadioAntennaPart part = this.Api.World.BlockAccessor.GetBlockEntity(scan) as BlockEntityRadioAntennaPart;
				if (((part != null) ? part.BaseEmitterPos : null) != null)
				{
					this.baseEmitterPos = part.BaseEmitterPos.Copy();
					this.MarkDirty(false, null);
					return;
				}
				if (part == null)
				{
					break;
				}
				scan = scan.DownCopy(1);
			}
			this.baseEmitterPos = null;
			this.MarkDirty(false, null);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00025F9A File Offset: 0x0002419A
		private void OnBeaconTick(float dt)
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.SyncBeaconPowered();
				return;
			}
			this.PulseBeaconLight();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00025FC0 File Offset: 0x000241C0
		private void SyncBeaconPowered()
		{
			BEBehaviorLightable light = base.GetBehavior<BEBehaviorLightable>();
			if (light == null)
			{
				return;
			}
			bool shouldLit = !(this.Api.World.BlockAccessor.GetBlock(this.Pos.UpCopy(1)) is RadioAntennaPartBlock) && this.EmitterHasSufficientTransmitPower();
			light.SetLightActive(shouldLit);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00026018 File Offset: 0x00024218
		private void PulseBeaconLight()
		{
			BEBehaviorLightable light = base.GetBehavior<BEBehaviorLightable>();
			if (light == null)
			{
				return;
			}
			if (!light.IsLightActive)
			{
				light.SetClientPulseFactor(0f);
				return;
			}
			double phase = (double)this.Api.World.ElapsedMilliseconds / 1000.0 % 2.5 / 2.5;
			float pulse = (float)(0.5 * (1.0 + Math.Sin(phase * 3.141592653589793 * 2.0 - 1.5707963267948966)));
			pulse = GameMath.Clamp((pulse - 0.08f) / 0.92f, 0f, 1f);
			light.SetClientPulseFactor(0.15f + pulse * 0.85f);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000260E0 File Offset: 0x000242E0
		private bool EmitterHasSufficientTransmitPower()
		{
			if (this.baseEmitterPos == null && this.Api != null)
			{
				this.ResolveBaseEmitter();
			}
			if (!(this.baseEmitterPos == null))
			{
				ICoreAPI api = this.Api;
				bool flag;
				if (api == null)
				{
					flag = (null != null);
				}
				else
				{
					IWorldAccessor world = api.World;
					flag = (((world != null) ? world.BlockAccessor : null) != null);
				}
				if (flag)
				{
					BlockEntityRadioEmitter emitter = this.Api.World.BlockAccessor.GetBlockEntity(this.baseEmitterPos) as BlockEntityRadioEmitter;
					return emitter != null && emitter.HasSufficientTransmitPower();
				}
			}
			return false;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00026168 File Offset: 0x00024368
		private void NotifyBaseEmitterRangeChanged()
		{
			if (!(this.baseEmitterPos == null))
			{
				ICoreAPI api = this.Api;
				bool flag;
				if (api == null)
				{
					flag = (null != null);
				}
				else
				{
					IWorldAccessor world = api.World;
					flag = (((world != null) ? world.BlockAccessor : null) != null);
				}
				if (flag)
				{
					BlockEntityRadioEmitter emitter = this.Api.World.BlockAccessor.GetBlockEntity(this.baseEmitterPos) as BlockEntityRadioEmitter;
					if (emitter != null)
					{
						emitter.OnAntennaStackChanged();
					}
					return;
				}
			}
		}

		// Token: 0x04000342 RID: 834
		private static readonly AssetLocation TopShapeLoc = new AssetLocation("rpvoicechat", "shapes/block/radioantenna/radioantenna_top.json");

		// Token: 0x04000343 RID: 835
		private static readonly AssetLocation PartShapeLoc = new AssetLocation("rpvoicechat", "shapes/block/radioantenna/radioantenna_part.json");

		// Token: 0x04000344 RID: 836
		private static readonly Vec3d BulbLightLocalOffset = new Vec3d(0.5, 0.8125, 0.5);

		// Token: 0x04000345 RID: 837
		private const float BeaconPulsePeriodSeconds = 2.5f;

		// Token: 0x04000346 RID: 838
		private BlockPos baseEmitterPos;

		// Token: 0x04000347 RID: 839
		private long beaconTickId;
	}
}
