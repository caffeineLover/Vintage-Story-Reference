using System;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorMilkable : EntityBehavior
{
	private float aggroChance;

	private bool aggroTested;

	private bool clientCanContinueMilking;

	private EntityBehaviorMultiply bhmul;

	[DocumentAsJson("Optional", "milkportion", false)]
	private JsonItemStack liquidJsonStack;

	[DocumentAsJson("Optional", "21", false)]
	private float lactatingDaysAfterBirth = 21f;

	[DocumentAsJson("Optional", "10", false)]
	private float yieldLitres = 10f;

	private long lastIsMilkingStateTotalMs;

	private ILoadedSound milkSound;

	private double lastMilkedTotalHours
	{
		get
		{
			return entity.WatchedAttributes.GetFloat("lastMilkedTotalHours");
		}
		set
		{
			entity.WatchedAttributes.SetFloat("lastMilkedTotalHours", (float)value);
		}
	}

	public bool IsBeingMilked => entity.World.ElapsedMilliseconds - lastIsMilkingStateTotalMs < 1000;

	public EntityBehaviorMilkable(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		liquidJsonStack = attributes["liquidStack"].AsObject(new JsonItemStack
		{
			Type = EnumItemClass.Item,
			Code = "milkportion"
		});
		liquidJsonStack.StackSize = 999999;
		lactatingDaysAfterBirth = attributes["lactatingDaysAfterBirth"].AsFloat(21f);
		yieldLitres = attributes["yieldLitres"].AsFloat(10f);
	}

	public override string PropertyName()
	{
		return "milkable";
	}

	public override void OnEntityLoaded()
	{
		init();
	}

	public override void OnEntitySpawn()
	{
		init();
	}

	private void init()
	{
		bhmul = entity.GetBehavior<EntityBehaviorMultiply>();
		if (entity.World.Side != EnumAppSide.Client)
		{
			entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.OnShouldExecuteTask += (IAiTask _) => !IsBeingMilked;
			EntityBehaviorMultiply entityBehaviorMultiply = bhmul;
			if (entityBehaviorMultiply != null)
			{
				entityBehaviorMultiply.TotalDaysLastBirth = Math.Min(bhmul.TotalDaysLastBirth, entity.World.Calendar.TotalDays);
			}
			lastMilkedTotalHours = Math.Min(lastMilkedTotalHours, entity.World.Calendar.TotalHours);
		}
	}

	public bool TryBeginMilking()
	{
		lastIsMilkingStateTotalMs = entity.World.ElapsedMilliseconds;
		if (!CanMilk())
		{
			return false;
		}
		int generation = entity.WatchedAttributes.GetInt("generation");
		aggroChance = Math.Min(1f - (float)generation / 3f, 0.95f);
		aggroTested = false;
		clientCanContinueMilking = true;
		if (entity.World.Side == EnumAppSide.Server)
		{
			AiTaskManager taskManager = entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager;
			taskManager.StopTask<AiTaskWander>();
			taskManager.StopTask<AiTaskSeekEntity>();
			taskManager.StopTask<AiTaskSeekFoodAndEat>();
			taskManager.StopTask<AiTaskStayCloseToEntity>();
		}
		else if (entity.World is IClientWorldAccessor cworld)
		{
			milkSound?.Dispose();
			milkSound = cworld.LoadSound(new SoundParams
			{
				DisposeOnFinish = true,
				Location = new AssetLocation("sounds/creature/sheep/milking.ogg"),
				Position = entity.Pos.XYZFloat,
				SoundType = EnumSoundType.Sound
			});
			milkSound.Start();
		}
		return true;
	}

	protected bool CanMilk()
	{
		if (bhmul == null)
		{
			return false;
		}
		if ((double)entity.WatchedAttributes.GetFloat("stressLevel") > 0.1)
		{
			(entity.Api as ICoreClientAPI)?.TriggerIngameError(this, "notready", Lang.Get("Currently too stressed to be milkable"));
			return false;
		}
		IGameCalendar cal = entity.World.Calendar;
		if (Math.Max(0.0, cal.TotalDays - bhmul.TotalDaysLastBirth) >= (double)lactatingDaysAfterBirth)
		{
			return false;
		}
		return !(cal.TotalHours - lastMilkedTotalHours < (double)cal.HoursPerDay);
	}

	public bool CanContinueMilking(IPlayer milkingPlayer, float secondsUsed)
	{
		if (!CanMilk())
		{
			return false;
		}
		lastIsMilkingStateTotalMs = entity.World.ElapsedMilliseconds;
		if (entity.World.Side == EnumAppSide.Client)
		{
			if (!clientCanContinueMilking)
			{
				milkSound?.Stop();
				milkSound?.Dispose();
			}
			else
			{
				milkSound.SetPosition((float)entity.Pos.X, (float)entity.Pos.Y, (float)entity.Pos.Z);
			}
			return clientCanContinueMilking;
		}
		if (secondsUsed > 1f && !aggroTested && entity.Api is ICoreServerAPI sapi)
		{
			aggroTested = true;
			if (entity.World.Rand.NextDouble() < (double)aggroChance)
			{
				entity.GetBehavior<EntityBehaviorEmotionStates>()?.TryTriggerState("aggressiveondamage", 1L);
				entity.WatchedAttributes.SetFloat("stressLevel", Math.Max(entity.WatchedAttributes.GetFloat("stressLevel"), 0.25f));
				if (entity.Properties.Sounds.ContainsKey("hurt"))
				{
					entity.World.PlaySoundAt(entity.Properties.Sounds["hurt"], entity);
				}
				sapi.Network.SendEntityPacket(milkingPlayer as IServerPlayer, entity.EntityId, 1337);
				return false;
			}
		}
		return true;
	}

	public void MilkingComplete(ItemSlot slot, EntityAgent byEntity)
	{
		CollectibleObject collectibleObject = slot.Itemstack?.Collectible;
		BlockLiquidContainerBase lcblock = collectibleObject as BlockLiquidContainerBase;
		if (lcblock == null)
		{
			return;
		}
		if (entity.World.Side == EnumAppSide.Server)
		{
			lastMilkedTotalHours = entity.World.Calendar.TotalHours;
			lcblock.SplitStackAndPerformAction(byEntity, slot, (ItemStack stack) => lcblock.TryPutLiquid(stack, GetMilkStack(entity.World), yieldLitres));
		}
		milkSound?.Stop();
		milkSound?.Dispose();
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data, ref EnumHandling handled)
	{
		if (packetid == 1337)
		{
			clientCanContinueMilking = false;
			(entity.Api as ICoreClientAPI)?.TriggerIngameError(this, "notready", Lang.Get("Became stressed from the milking attempt. Not milkable while stressed."));
		}
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		if (!entity.Alive || bhmul == null)
		{
			return;
		}
		double lactatingDaysLeft = (double)lactatingDaysAfterBirth - Math.Max(0.0, entity.World.Calendar.TotalDays - bhmul.TotalDaysLastBirth);
		if (lactatingDaysLeft <= 0.0)
		{
			return;
		}
		if (entity.World.Calendar.TotalHours - lastMilkedTotalHours >= (double)entity.World.Calendar.HoursPerDay)
		{
			if ((double)entity.WatchedAttributes.GetFloat("stressLevel") > 0.1)
			{
				infotext.AppendLine(Lang.Get("Lactating for {0} days, currently too stressed to be milkable.", (int)lactatingDaysLeft));
				return;
			}
			int num = entity.WatchedAttributes.GetInt("generation");
			string key = ((num >= 3) ? "Lactating for {0} days, can be milked." : ((num != 0) ? "Lactating for {0} days, can be milked, but might become aggressive." : "Lactating for {0} days, can be milked, but will become aggressive."));
			infotext.AppendLine(Lang.Get(key, (int)lactatingDaysLeft));
		}
		else
		{
			infotext.AppendLine(Lang.Get("Lactating for {0} days.", (int)lactatingDaysLeft));
		}
	}

	public ItemStack GetMilkStack(IWorldAccessor world)
	{
		liquidJsonStack.ResolvedItemstack = null;
		liquidJsonStack.Resolve(world, "milking liquid stack");
		return liquidJsonStack.ResolvedItemStack;
	}
}
