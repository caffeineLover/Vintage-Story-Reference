using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000018 RID: 24
	public class BlockDeadfall : Block
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00012218 File Offset: 0x00010418
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			if (entity.Code.Path.StartsWith("butterfly"))
			{
				return;
			}
			if (isImpact)
			{
				string state = this.api.World.BlockAccessor.GetBlock(pos, 0).FirstCodePart(1);
				if (BlockDeadfall.Rnd.Next(100) < ModConfig.Loaded.DeadfallBaitStolenPercent && entity.Code.Path != "player")
				{
					BESnare bedc = world.BlockAccessor.GetBlockEntity(pos) as BESnare;
					if (bedc != null)
					{
						bedc.StealBait(pos);
					}
					return;
				}
				if (BlockDeadfall.Rnd.Next(100) < ModConfig.Loaded.DeadfallTrippedPercent)
				{
					BESnare bedc2 = world.BlockAccessor.GetBlockEntity(pos) as BESnare;
					if (bedc2 != null)
					{
						bedc2.TripTrap(pos);
					}
					world.PlaySoundAt(this.tickSound, entity.Pos.X, entity.Pos.Y, entity.Pos.Z, null, true, 32f, 1f);
					return;
				}
				double maxanimalheight = (double)ModConfig.Loaded.DeadfallMaxAnimalHeight;
				int maxdamage = ModConfig.Loaded.DeadfallMaxDamageBaited;
				if (state == "set")
				{
					maxdamage = ModConfig.Loaded.DeadfallMaxDamageSet;
				}
				if (state != "tripped")
				{
					int dmg = 3;
					if (entity.Properties.EyeHeight < maxanimalheight)
					{
						dmg = new Random().Next(5, maxdamage);
					}
					entity.ReceiveDamage(new DamageSource
					{
						SourceEntity = null,
						Type = 2
					}, (float)dmg);
					BEDeadfall bedc3 = world.BlockAccessor.GetBlockEntity(pos) as BEDeadfall;
					if (bedc3 != null)
					{
						bedc3.TripTrap(pos);
					}
					world.PlaySoundAt(this.tickSound, entity.Pos.X, entity.Pos.Y, entity.Pos.Z, null, true, 32f, 1f);
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000123F4 File Offset: 0x000105F4
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
			bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag)
			{
				Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath = block.Code.Path;
				newPath = newPath.Replace("north", facing);
				block = this.api.World.GetBlock(block.CodeWithPath(newPath));
				this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
			}
			return flag;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00012494 File Offset: 0x00010694
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
			string path = block.Code.Path;
			if (path.Contains("-tripped"))
			{
				path = path.Replace("-tripped", "-set");
				block = world.GetBlock(block.CodeWithPath(path));
				world.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
				return true;
			}
			BEDeadfall bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEDeadfall;
			return bedc == null || bedc.OnInteract(byPlayer);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00012528 File Offset: 0x00010728
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, int slot, bool tripped)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(0f, 0f, 0f), 0, 0, 0, default(int?), null);
			if (slot == 0)
			{
				if (tripped)
				{
					mesh.Translate(-0.1f, 0f, -0.3f);
				}
				else
				{
					mesh.Translate(-0.1f, 0f, -0.2f);
				}
			}
			mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, this.Shape.rotateY * 0.017453292f, 0f);
			return mesh;
		}

		// Token: 0x040000C2 RID: 194
		protected static readonly Random Rnd = new Random();

		// Token: 0x040000C3 RID: 195
		private readonly AssetLocation tickSound = new AssetLocation("game", "tick");
	}
}
