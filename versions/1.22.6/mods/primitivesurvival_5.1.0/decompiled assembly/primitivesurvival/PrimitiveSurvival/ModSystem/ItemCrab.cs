using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000043 RID: 67
	public class ItemCrab : Item
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
		public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
		{
			return null;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001F1E8 File Offset: 0x0001D3E8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			if (byEntity.Controls.Sneak && blockSel != null)
			{
				IPlayer player = byEntity.World.PlayerByUid((byEntity as EntityPlayer).PlayerUID);
				if (!byEntity.World.Claims.TryAccess(player, blockSel.Position, 1))
				{
					return;
				}
				if (!(byEntity is EntityPlayer) || player.WorldData.CurrentGameMode != 2)
				{
					slot.TakeOut(1);
					slot.MarkDirty();
				}
				AssetLocation location = new AssetLocation(this.Code.Domain, base.CodeEndWithoutParts(1));
				EntityProperties type = byEntity.World.GetEntityType(location);
				if (type == null)
				{
					byEntity.World.Logger.Error("ItemCreature: No such entity - {0}", new object[]
					{
						location
					});
					if (this.api.World.Side == 2)
					{
						(this.api as ICoreClientAPI).TriggerIngameError(this, "nosuchentity", "No such entity '{0}' loaded.");
					}
					return;
				}
				Entity entity = byEntity.World.ClassRegistry.CreateEntity(type);
				if (entity != null)
				{
					entity.Pos.X = (double)((float)(blockSel.Position.X + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.X)) + 0.5f);
					entity.Pos.Y = (double)((float)blockSel.Position.Y + (blockSel.DidOffset ? 0f : ((float)blockSel.Face.Normali.Y + 0.1f)));
					entity.Pos.Z = (double)((float)(blockSel.Position.Z + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.Z)) + 0.5f);
					entity.Pos.Yaw = (float)byEntity.World.Rand.NextDouble() * 2f * 3.1415927f;
					entity.Pos.SetFrom(entity.Pos);
					entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
					entity.Attributes.SetString("origin", "playerplaced");
					byEntity.World.SpawnEntity(entity);
					handHandling = 3;
					return;
				}
			}
			else
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0001F43E File Offset: 0x0001D63E
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					HotKeyCode = "sneak",
					ActionLangCode = "heldhelp-place",
					MouseButton = 2
				}
			}, base.GetHeldInteractionHelp(inSlot));
		}
	}
}
