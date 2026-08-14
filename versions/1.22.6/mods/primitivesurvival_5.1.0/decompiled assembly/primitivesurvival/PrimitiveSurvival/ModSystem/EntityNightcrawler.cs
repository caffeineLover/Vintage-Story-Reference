using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200003B RID: 59
	public class EntityNightcrawler : EntityAgent
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0001CB03 File Offset: 0x0001AD03
		public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
		{
			base.Initialize(properties, api, InChunkIndex3d);
			this.roomRegistry = this.Api.ModLoader.GetModSystem<RoomRegistry>(true);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0001CB28 File Offset: 0x0001AD28
		public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			if (!this.Alive || this.World.Side == 2 || mode == null)
			{
				base.OnInteract(byEntity, slot, hitPosition, mode);
				return;
			}
			Item item = byEntity.World.GetItem(new AssetLocation("primitivesurvival:nightcrawler"));
			if (item != null)
			{
				ItemStack stack = new ItemStack(item, 1);
				if (!byEntity.TryGiveItemStack(stack))
				{
					byEntity.World.SpawnItemEntity(stack, base.Pos.XYZ, null);
				}
			}
			ICoreServerAPI sapi = this.Api as ICoreServerAPI;
			if (sapi != null)
			{
				sapi.World.DespawnEntity(this, new EntityDespawnData
				{
					Reason = 7
				});
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0001CBC8 File Offset: 0x0001ADC8
		public override string GetInfoText()
		{
			string result = base.GetInfoText();
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				result = result + "\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>\n\n";
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0001CC0C File Offset: 0x0001AE0C
		public override void OnGameTick(float dt)
		{
			base.OnGameTick(dt);
			int num = this.cnt;
			this.cnt = num + 1;
			if (num > 500)
			{
				this.cnt = 0;
				BlockPos belowPos = base.Pos.XYZ.AsBlockPos;
				Block blockBelow = this.World.BlockAccessor.GetBlock(belowPos, 0);
				ClimateCondition conds = this.World.BlockAccessor.GetClimateAt(belowPos, 1, 0.0);
				RoomRegistry roomRegistry;
				if ((roomRegistry = this.roomRegistry) == null)
				{
					roomRegistry = (this.roomRegistry = this.Api.ModLoader.GetModSystem<RoomRegistry>(true));
				}
				Room room = roomRegistry.GetRoomForPosition(belowPos);
				float wormtemp = conds.Temperature;
				if (room != null && room.ExitCount == 0)
				{
					if (wormtemp <= 0f)
					{
						wormtemp += 5f;
					}
					else if (wormtemp >= 35f)
					{
						wormtemp -= 5f;
					}
				}
				int escaped = EntityNightcrawler.Rnd.Next(200);
				if (wormtemp <= 0f || wormtemp >= 35f || escaped < 1)
				{
					ICoreServerAPI sapi = this.Api as ICoreServerAPI;
					if (sapi != null)
					{
						sapi.World.DespawnEntity(this, new EntityDespawnData
						{
							Reason = 7
						});
						return;
					}
				}
				else if (blockBelow.FirstCodePart(0) == "farmland")
				{
					BlockEntityFarmland befarmland = this.World.BlockAccessor.GetBlockEntity(belowPos) as BlockEntityFarmland;
					if (befarmland != null && EnumAppSideExtensions.IsServer(this.Api.Side))
					{
						if (befarmland != null)
						{
							befarmland.WaterFarmland(0.3f, true);
						}
						TreeAttribute tree = new TreeAttribute();
						if (befarmland != null)
						{
							befarmland.ToTreeAttributes(tree);
						}
						if (tree != null)
						{
							float slowN = tree.GetFloat("slowN", 0f);
							float slowK = tree.GetFloat("slowK", 0f);
							float slowP = tree.GetFloat("slowP", 0f);
							if (slowN <= 150f)
							{
								slowN += 1f;
							}
							if (slowK <= 150f)
							{
								slowK += 1f;
							}
							if (slowP <= 150f)
							{
								slowP += 1f;
							}
							if (slowN < 150f && slowK < 150f && slowP < 150f)
							{
								tree.SetFloat("slowN", slowN);
								tree.SetFloat("slowK", slowK);
								tree.SetFloat("slowP", slowP);
								if (befarmland != null)
								{
									befarmland.FromTreeAttributes(tree, this.World);
								}
								if (befarmland != null)
								{
									befarmland.MarkDirty(false, null);
								}
								this.World.BlockAccessor.MarkBlockEntityDirty(belowPos);
								return;
							}
							this.World.BlockAccessor.BreakBlock(belowPos, null, 1f);
							Block block = this.World.BlockAccessor.GetBlock(new AssetLocation("primitivesurvival:earthwormcastings"));
							if (block != null)
							{
								this.World.BlockAccessor.SetBlock(block.BlockId, belowPos);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000114 RID: 276
		private int cnt;

		// Token: 0x04000115 RID: 277
		private static readonly Random Rnd = new Random();

		// Token: 0x04000116 RID: 278
		private RoomRegistry roomRegistry;
	}
}
