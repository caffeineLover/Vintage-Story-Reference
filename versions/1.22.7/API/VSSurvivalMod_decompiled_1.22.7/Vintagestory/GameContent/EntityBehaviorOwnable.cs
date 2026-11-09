using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class EntityBehaviorOwnable : EntityBehavior
{
	public string Group;

	public EntityBehaviorOwnable(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		Group = attributes["groupCode"].AsString();
		verifyOwnership();
	}

	private void verifyOwnership()
	{
		if (entity.World.Side != EnumAppSide.Server)
		{
			return;
		}
		bool found = false;
		ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("ownedby");
		if (tree != null && entity.World.Api.ModLoader.GetModSystem<ModSystemEntityOwnership>().OwnerShipsByPlayerUid.TryGetValue(tree.GetString("uid", ""), out var ownerships) && ownerships != null && ownerships.TryGetValue(Group, out var ownership))
		{
			if (entity.World.Side == EnumAppSide.Server)
			{
				ownership.Pos = entity.Pos;
			}
			found = ownership.EntityId == entity.EntityId;
		}
		if (!found)
		{
			entity.WatchedAttributes.RemoveAttribute("ownedby");
		}
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		base.GetInfoText(infotext);
		ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("ownedby");
		if (tree == null)
		{
			return;
		}
		infotext.AppendLine(Lang.Get("Owned by {0}", tree.GetString("name")));
		if ((entity.World as IClientWorldAccessor).Player.WorldData.CurrentGameMode != EnumGameMode.Creative)
		{
			EntityBehaviorHealth ebh = entity.GetBehavior<EntityBehaviorHealth>();
			if (ebh != null)
			{
				infotext.AppendLine(Lang.Get("ownableentity-health", ebh.Health, ebh.MaxHealth));
			}
		}
	}

	public override string PropertyName()
	{
		return "ownable";
	}

	public bool IsOwner(EntityAgent byEntity)
	{
		if (byEntity is EntityPlayer byPlayer)
		{
			ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("ownedby");
			if (tree == null)
			{
				return true;
			}
			string uid = tree.GetString("uid");
			if (uid != null && byPlayer.PlayerUID == uid)
			{
				return true;
			}
		}
		return false;
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		if (entity.World.Api is ICoreClientAPI capi)
		{
			ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("ownedby");
			if (tree != null && capi.World.Player.PlayerUID == tree.GetString("uid", "") && entity.World.Api.ModLoader.GetModSystem<ModSystemEntityOwnership>().SelfOwnerShips.TryGetValue(Group, out var ownership))
			{
				ownership.Pos = entity.Pos.Copy();
			}
		}
	}

	public override bool ToleratesDamageFrom(Entity eOther, ref EnumHandling handling)
	{
		if (eOther is EntityAgent eagent && IsOwner(eagent))
		{
			handling = EnumHandling.PreventDefault;
			return true;
		}
		return false;
	}
}
