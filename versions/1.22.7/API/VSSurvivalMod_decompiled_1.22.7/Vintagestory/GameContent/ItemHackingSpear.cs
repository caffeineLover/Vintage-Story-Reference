using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ItemHackingSpear : ItemSpear
{
	private SkillItem[] hackingModes;

	public override string GetHeldTpHitAnimation(ItemSlot slot, Entity byEntity)
	{
		EntityPlayer eplr = byEntity as EntityPlayer;
		if (GetToolMode(slot, eplr.Player, eplr.BlockSelection) == 1)
		{
			return "hackingspear-callback";
		}
		return base.GetHeldTpHitAnimation(slot, byEntity);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		GetCollectibleBehavior<CollectibleBehaviorAnimationAuthoritative>(withInheritance: true).OnBeginHitEntity += ItemSpear_OnBeginHitEntity;
		ICoreClientAPI capi = api as ICoreClientAPI;
		hackingModes = new SkillItem[2]
		{
			new SkillItem
			{
				Code = "hack",
				Name = "Hack"
			},
			new SkillItem
			{
				Code = "call-back",
				Name = "Call back"
			}
		};
		if (api.Side == EnumAppSide.Client)
		{
			hackingModes[0].WithIcon(capi, capi.Gui.LoadSvgWithPadding(new AssetLocation("textures/icons/call-back.svg"), 48, 48, 5, -1));
			hackingModes[1].WithIcon(capi, capi.Gui.LoadSvgWithPadding(new AssetLocation("textures/icons/hack.svg"), 48, 48, 5, -1));
		}
	}

	private void ItemSpear_OnBeginHitEntity(EntityAgent byEntity, ref EnumHandling handling)
	{
		if (byEntity.World.Side == EnumAppSide.Client)
		{
			return;
		}
		EntityPlayer eplr = byEntity as EntityPlayer;
		EntitySelection entitySel = eplr?.EntitySelection;
		if (byEntity.Attributes.GetInt("didattack") != 0)
		{
			return;
		}
		byEntity.Attributes.SetInt("didattack", 1);
		_ = byEntity.ActiveHandItemSlot;
		if (GetToolMode(byEntity.RightHandItemSlot, eplr.Player, eplr.BlockSelection) == 1)
		{
			callLocust(byEntity);
			handling = EnumHandling.PreventDefault;
		}
		else
		{
			if (entitySel == null)
			{
				return;
			}
			JsonObject attributes = entitySel.Entity.Properties.Attributes;
			bool num = attributes != null && attributes["hackedEntity"].Exists && api.ModLoader.GetModSystem<CharacterSystem>().HasTrait((byEntity as EntityPlayer).Player, "technical");
			ICoreServerAPI sapi = api as ICoreServerAPI;
			if (num)
			{
				sapi.World.PlaySoundAt(new AssetLocation("sounds/player/hackingspearhit.ogg"), entitySel.Entity);
				if (api.World.Rand.NextDouble() < 0.15)
				{
					SpawnEntityInPlaceOf(entitySel.Entity, entitySel.Entity.Properties.Attributes["hackedEntity"].AsString(), byEntity);
					sapi.World.DespawnEntity(entitySel.Entity, new EntityDespawnData
					{
						Reason = EnumDespawnReason.Removed
					});
				}
			}
		}
	}

	private void callLocust(EntityAgent byEntity)
	{
		if (api.Side == EnumAppSide.Client)
		{
			return;
		}
		string uid = (byEntity as EntityPlayer).PlayerUID;
		Entity entity = api.ModLoader.GetModSystem<EntityPartitioning>().GetNearestEntity(byEntity.Pos.XYZ, 1000.0, (Entity e) => e.Alive && e.Properties.Variant.TryGetValue("state", out var value) && value == "hacked" && e.WatchedAttributes.GetString("guardedPlayerUid") == uid, EnumEntitySearchType.Creatures);
		if (entity != null)
		{
			AiTaskManager tm = entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager;
			AiTaskComeToOwner aitcto = tm.AllTasks.FirstOrDefault((IAiTask t) => t is AiTaskComeToOwner) as AiTaskComeToOwner;
			if (!(entity.Pos.DistanceTo(byEntity.Pos) > (double)aitcto.TeleportMaxRange))
			{
				entity.AlwaysActive = true;
				entity.State = EnumEntityState.Active;
				aitcto.allowTeleportCount = 1;
				tm.StopTasks();
				tm.ExecuteTask(aitcto, 0);
			}
		}
	}

	public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
	{
		return hackingModes;
	}

	public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
	{
		return slot.Itemstack.Attributes.GetInt("toolmode");
	}

	public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
	{
		slot.Itemstack.Attributes.SetInt("toolmode", toolMode);
	}

	private void SpawnEntityInPlaceOf(Entity byEntity, string code, EntityAgent causingEntity)
	{
		AssetLocation location = AssetLocation.Create(code, byEntity.Code.Domain);
		EntityProperties type = byEntity.World.GetEntityType(location);
		if (type == null)
		{
			byEntity.World.Logger.Error("ItemCreature: No such entity - {0}", location);
			if (api.World.Side == EnumAppSide.Client)
			{
				(api as ICoreClientAPI).TriggerIngameError(this, "nosuchentity", $"No such entity loaded - '{location}'.");
			}
			return;
		}
		Entity entity = byEntity.World.ClassRegistry.CreateEntity(type);
		if (entity != null)
		{
			entity.Pos.X = byEntity.Pos.X;
			entity.Pos.Y = byEntity.Pos.Y;
			entity.Pos.Z = byEntity.Pos.Z;
			entity.Pos.Motion.X = byEntity.Pos.Motion.X;
			entity.Pos.Motion.Y = byEntity.Pos.Motion.Y;
			entity.Pos.Motion.Z = byEntity.Pos.Motion.Z;
			entity.Pos.Yaw = byEntity.Pos.Yaw;
			entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
			entity.Attributes.SetString("origin", "playerplaced");
			entity.WatchedAttributes.SetLong("guardedEntityId", byEntity.EntityId);
			if (causingEntity is EntityPlayer eplr)
			{
				entity.WatchedAttributes.SetString("guardedPlayerUid", eplr.PlayerUID);
				TreeAttribute tree = new TreeAttribute();
				tree.SetString("uid", eplr.PlayerUID);
				entity.WatchedAttributes["ownedby"] = tree;
			}
			byEntity.World.SpawnEntity(entity);
		}
	}
}
