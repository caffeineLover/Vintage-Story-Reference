using System;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorPlayerRevivable : EntityBehavior
{
	private EntityPlayer entityPlayer;

	public EntityBehaviorPlayerRevivable(Entity entity)
		: base(entity)
	{
		entityPlayer = entity as EntityPlayer;
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		if (entity.Alive)
		{
			return;
		}
		double hoursleft = entityPlayer.RevivableIngameHoursLeft();
		if (hoursleft < 1.0)
		{
			if (hoursleft * 60.0 >= 0.0)
			{
				infotext.AppendLine(Lang.Get("Mortally wounded, alive for {0} ingame minutes.", (int)(hoursleft * 60.0)));
			}
		}
		else
		{
			infotext.AppendLine(Lang.Get("Mortally wounded, alive for {0} more hours", (int)hoursleft));
		}
	}

	public void AttemptRevive()
	{
		if (!(entityPlayer.RevivableIngameHoursLeft() <= 0.0))
		{
			entityPlayer.Attributes.SetBool("noSatietyRestoreOnRevive", value: true);
			entityPlayer.Revive();
			entityPlayer.LastReviveTotalHours = entityPlayer.World.Calendar.TotalHours - 2.0;
		}
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
	{
		WorldInteraction[] wis = base.GetInteractionHelp(world, es, player, ref handled);
		if (!entity.Alive && entityPlayer.RevivableIngameHoursLeft() > 0.0)
		{
			if (wis == null)
			{
				wis = Array.Empty<WorldInteraction>();
			}
			wis = wis.Append(new WorldInteraction
			{
				ActionLangCode = "reviveplayer",
				MouseButton = EnumMouseButton.Right,
				HotKeyCode = "ctrl",
				Itemstacks = EntityBehaviorHealth.GetAllHealingItems(world.Api),
				RequireFreeHand = true
			});
		}
		return wis;
	}

	public override string PropertyName()
	{
		return "playerrevivable";
	}
}
