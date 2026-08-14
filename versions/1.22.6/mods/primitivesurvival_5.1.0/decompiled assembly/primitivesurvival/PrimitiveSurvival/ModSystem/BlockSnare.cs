using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000028 RID: 40
	public class BlockSnare : Block
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x000178E4 File Offset: 0x00015AE4
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			if (entity.Code.Path.StartsWith("butterfly"))
			{
				return;
			}
			if (isImpact)
			{
				if (BlockSnare.Rnd.Next(100) < ModConfig.Loaded.SnareBaitStolenPercent && entity.Code.Path != "player")
				{
					BESnare bedc = world.BlockAccessor.GetBlockEntity(pos) as BESnare;
					if (bedc != null)
					{
						bedc.StealBait(pos);
					}
					return;
				}
				if (BlockSnare.Rnd.Next(100) < ModConfig.Loaded.SnareTrippedPercent)
				{
					BESnare bedc2 = world.BlockAccessor.GetBlockEntity(pos) as BESnare;
					if (bedc2 != null)
					{
						bedc2.TripTrap(pos);
					}
					world.PlaySoundAt(this.tickSound, entity.Pos.X, entity.Pos.Y, entity.Pos.Z, null, true, 32f, 1f);
					return;
				}
				string text = this.api.World.BlockAccessor.GetBlock(pos, 0).FirstCodePart(1);
				double maxanimalheight = (double)ModConfig.Loaded.SnareMaxAnimalHeight;
				int maxdamage = ModConfig.Loaded.SnareMaxDamageBaited;
				if (text == "set")
				{
					maxdamage = ModConfig.Loaded.SnareMaxDamageSet;
				}
				if (text != "tripped")
				{
					int dmg = 3;
					if (entity.Properties.EyeHeight < maxanimalheight)
					{
						dmg = new Random().Next(6, maxdamage);
					}
					entity.ReceiveDamage(new DamageSource
					{
						SourceEntity = null,
						Type = 2
					}, (float)dmg);
					BESnare bedc3 = world.BlockAccessor.GetBlockEntity(pos) as BESnare;
					if (bedc3 != null)
					{
						bedc3.TripTrap(pos);
					}
					world.PlaySoundAt(this.tickSound, entity.Pos.X, entity.Pos.Y, entity.Pos.Z, null, true, 32f, 1f);
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00017ABC File Offset: 0x00015CBC
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			BlockSelection blockSrc = blockSel.Clone();
			blockSrc.Position.Y--;
			Block block = world.BlockAccessor.GetBlock(blockSrc.Position, 0);
			if (block.Fertility <= 0)
			{
				failureCode = Lang.Get("softer-ground-needed", Array.Empty<object>());
				return false;
			}
			string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
			bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag)
			{
				block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath = block.Code.Path;
				newPath = newPath.Replace("north", facing);
				block = this.api.World.GetBlock(block.CodeWithPath(newPath));
				this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
			}
			return flag;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00017BA8 File Offset: 0x00015DA8
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
			BESnare bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BESnare;
			return bedc == null || bedc.OnInteract(byPlayer);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00017C3C File Offset: 0x00015E3C
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

		// Token: 0x040000D8 RID: 216
		protected static readonly Random Rnd = new Random();

		// Token: 0x040000D9 RID: 217
		private readonly AssetLocation tickSound = new AssetLocation("game", "tick");
	}
}
