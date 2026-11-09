using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000047 RID: 71
	public class ItemHoeExtended : ItemHoe
	{
		// Token: 0x060002BE RID: 702 RVA: 0x00020004 File Offset: 0x0001E204
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			if (api.Side != 2)
			{
				return;
			}
			ICoreAPI api2 = api;
			ObjectCacheUtil.Delete(api, "hoeInteractions");
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "hoeInteractions", delegate()
			{
				List<ItemStack> stacks = new List<ItemStack>();
				List<ItemStack> farmlandStacks = new List<ItemStack>();
				foreach (Block block in api.World.Blocks)
				{
					if (!(block.Code == null))
					{
						if (block.Code.Path.StartsWith("soil"))
						{
							stacks.Add(new ItemStack(block, 1));
						}
						if (block.Code.Path.StartsWith("farmland"))
						{
							farmlandStacks.Add(new ItemStack(block, 1));
						}
					}
				}
				return new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "heldhelp-till",
						MouseButton = 2,
						Itemstacks = stacks.ToArray()
					},
					new WorldInteraction
					{
						HotKeyCode = "shift",
						ActionLangCode = Lang.Get("handhelp-trench", Array.Empty<object>()),
						MouseButton = 2,
						Itemstacks = farmlandStacks.ToArray()
					}
				};
			});
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00020074 File Offset: 0x0001E274
		public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			if (blockSel == null)
			{
				return;
			}
			if (byEntity.Controls.ShiftKey && byEntity.Controls.CtrlKey)
			{
				base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
				return;
			}
			BlockPos pos = blockSel.Position;
			Block block = byEntity.World.BlockAccessor.GetBlock(pos);
			if (byEntity.World.BlockAccessor.GetBlock(pos.UpCopy(1)).Id != 0)
			{
				ICoreClientAPI coreClientAPI = this.api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "covered", Lang.Get("Requires no block above", Array.Empty<object>()));
				}
				handHandling = 4;
				return;
			}
			byEntity.Attributes.SetInt("didtill", 0);
			if (block.Code.Path.StartsWith("soil"))
			{
				handHandling = 4;
			}
			if (block.Code.Path.StartsWith("farmland") && byEntity.Controls.ShiftKey)
			{
				handHandling = 4;
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0002016C File Offset: 0x0001E36C
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (blockSel == null)
			{
				return false;
			}
			if (byEntity.Controls.ShiftKey && byEntity.Controls.CtrlKey)
			{
				return false;
			}
			if (byEntity.World.BlockAccessor.GetBlock(blockSel.Position.UpCopy(1)).BlockId != 0)
			{
				return false;
			}
			IPlayer byPlayer = (byEntity as EntityPlayer).Player;
			if (byEntity.World is IClientWorldAccessor)
			{
				ModelTransform modelTransform = new ModelTransform();
				modelTransform.EnsureDefaultValues();
				float rotateToTill = GameMath.Clamp(secondsUsed * 18f, 0f, 2f);
				float scrape = GameMath.SmoothStep(2.5f * GameMath.Clamp(secondsUsed - 0.35f, 0f, 1f));
				float scrapeShake = (secondsUsed > 0.35f && secondsUsed < 0.75f) ? (GameMath.Sin(secondsUsed * 50f) / 60f) : 0f;
				float rotateWithReset = Math.Max(0f, rotateToTill - GameMath.Clamp(24f * (secondsUsed - 0.75f), 0f, 2f));
				float scrapeWithReset = Math.Max(0f, scrape - Math.Max(0f, 20f * (secondsUsed - 0.75f)));
				modelTransform.Origin.Set(0f, 0f, 0.5f);
				modelTransform.Rotation.Set(0f, rotateWithReset * 45f, 0f);
				modelTransform.Translation.Set(scrapeShake, 0f, scrapeWithReset / 2f);
			}
			if (secondsUsed > 0.35f && secondsUsed < 0.87f)
			{
				Vec3d dir = new Vec3d().AheadCopy(1.0, 0f, byEntity.Pos.Yaw - 3.1415927f);
				Vec3d pos = blockSel.Position.ToVec3d().Add(0.5 + dir.X, 1.03, 0.5 + dir.Z);
				pos.X -= dir.X * (double)secondsUsed * 1.0 / 0.75 * 1.2000000476837158;
				pos.Z -= dir.Z * (double)secondsUsed * 1.0 / 0.75 * 1.2000000476837158;
				byEntity.World.SpawnCubeParticles(blockSel.Position, pos, 0.25f, 3, 0.5f, byPlayer, null);
			}
			if (secondsUsed > 0.6f && byEntity.Attributes.GetInt("didtill", 0) == 0 && byEntity.World.Side == 1)
			{
				byEntity.Attributes.SetInt("didtill", 1);
				this.DoTill(secondsUsed, slot, byEntity, blockSel, entitySel);
			}
			return secondsUsed < 1f;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00020449 File Offset: 0x0001E649
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
		{
			return false;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0002044C File Offset: 0x0001E64C
		public override void DoTill(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (blockSel == null)
			{
				return;
			}
			BlockPos pos = blockSel.Position;
			Block block = byEntity.World.BlockAccessor.GetBlock(pos);
			if (block.Code.Path.Contains("fallow"))
			{
				base.DoTill(secondsUsed, slot, byEntity, blockSel, entitySel);
				return;
			}
			if (block.Code.Path.StartsWith("farmland"))
			{
				string groundIrrigationAsset = "primitivesurvival:furrowedland-" + block.LastCodePart(0) + "-free";
				Block channelBlock = byEntity.World.GetBlock(new AssetLocation(groundIrrigationAsset));
				byEntity.World.BlockAccessor.SetBlock(channelBlock.BlockId, blockSel.Position, 0);
				byEntity.World.BlockAccessor.MarkBlockDirty(pos, null);
				if (block.Sounds != null)
				{
					byEntity.World.PlaySoundAt(block.Sounds.Place, (double)pos.X, (double)pos.Y, (double)pos.Z, 0, null, 1f);
				}
				IPlayer byPlayer2 = (byEntity as EntityPlayer).Player;
				slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, byPlayer2.InventoryManager.ActiveHotbarSlot, 1, true);
				if (slot.Empty)
				{
					byEntity.World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), byEntity.Pos.X, byEntity.Pos.Y, byEntity.Pos.Z, null, true, 32f, 1f);
				}
				return;
			}
			if (!block.Code.Path.StartsWith("soil"))
			{
				return;
			}
			string fertility = block.LastCodePart(1);
			Block farmland = byEntity.World.GetBlock(new AssetLocation("farmland-dry-" + fertility));
			IPlayer byPlayer3 = (byEntity as EntityPlayer).Player;
			if (farmland == null || byPlayer3 == null)
			{
				return;
			}
			if (block.Sounds != null)
			{
				byEntity.World.PlaySoundAt(block.Sounds.Place, (double)pos.X, (double)pos.Y, (double)pos.Z, 0, null, 1f);
			}
			byEntity.World.BlockAccessor.SetBlock(farmland.BlockId, pos);
			slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, byPlayer3.InventoryManager.ActiveHotbarSlot, 1, true);
			if (slot.Empty)
			{
				byEntity.World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), byEntity.Pos.X, byEntity.Pos.Y, byEntity.Pos.Z, null, true, 32f, 1f);
			}
			BlockEntityFarmland farmland2 = byEntity.World.BlockAccessor.GetBlockEntity(pos) as BlockEntityFarmland;
			if (farmland2 != null)
			{
				farmland2.OnCreatedFromSoil(block, null);
			}
			byEntity.World.BlockAccessor.MarkBlockDirty(pos, null);
		}

		// Token: 0x04000121 RID: 289
		private WorldInteraction[] interactions;
	}
}
