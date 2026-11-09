using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorQuenchable : CollectibleBehavior
{
	public class MetalPropertyVariant : WorldPropertyVariant
	{
		[JsonProperty]
		public int quenchMinTemp;

		[JsonProperty]
		public int quenchMaxTemp;

		[JsonProperty]
		public int temperMinTemp;

		[JsonProperty]
		public int temperMaxTemp;

		[JsonProperty]
		public int settledTemperature = 200;
	}

	public class MetalWorldProperties : WorldProperty<MetalPropertyVariant>
	{
	}

	public float BreakChancePerQuench = 0.05f;

	public float TemperShatterMultiplier = 0.8f;

	public float TemperPowerMultiplier = 0.92f;

	protected string metalGroupCode;

	private MetalPropertyVariant metalProps;

	public static bool CoolToTemperature(IWorldAccessor world, ItemSlot slot, Vec3d pos, float dt, float targetTemperature, bool playSizzle = true)
	{
		float stackTemp = slot.Itemstack.Collectible.GetTemperature(world, slot.Itemstack);
		if (stackTemp <= targetTemperature)
		{
			return false;
		}
		CollectibleBehaviorQuenchable behavior = slot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorQuenchable>();
		float nextTemperature = Math.Max(20f, stackTemp - 200f * dt);
		behavior?.IsGettingCooled(world, slot, pos, dt, nextTemperature);
		if (slot.Empty)
		{
			return false;
		}
		slot.Itemstack.Collectible.SetTemperature(world, slot.Itemstack, nextTemperature);
		float tempDiff = stackTemp - targetTemperature;
		if (tempDiff > 90f)
		{
			double width = 0.0;
			Entity.SplashParticleProps.BasePos.Set(pos.X - width / 2.0, pos.Y - 0.75, pos.Z - width / 2.0);
			Entity.SplashParticleProps.AddVelocity.Set(0f, 0f, 0f);
			Entity.SplashParticleProps.QuantityMul = 0.1f;
			world.SpawnParticles(Entity.SplashParticleProps);
		}
		long lastPlayedSizzlesTotalMs = slot.Itemstack.TempAttributes.GetLong("lastPlayedSizzlesTotalMs", -99999L);
		if (playSizzle && tempDiff > 200f && world.Side == EnumAppSide.Client && world.ElapsedMilliseconds - lastPlayedSizzlesTotalMs > 10000)
		{
			world.PlaySoundAt(new AssetLocation("sounds/sizzle"), pos.X, pos.Y, pos.Z);
			slot.Itemstack.TempAttributes.SetLong("lastPlayedSizzlesTotalMs", world.ElapsedMilliseconds);
		}
		return true;
	}

	public CollectibleBehaviorQuenchable(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		MetalWorldProperties metaltypes = api.Assets.Get("worldproperties/block/metal.json").ToObject<MetalWorldProperties>();
		string metalCode = collObj.Variant[metalGroupCode];
		if (metalCode == null)
		{
			api.Logger.Warning("Collectible {0} has quenchable property, but cannot get metal code from metal group code \"{1}\"", collObj.Code, metalGroupCode);
			return;
		}
		metalProps = metaltypes.Variants.FirstOrDefault((MetalPropertyVariant p) => p.Code.ToShortString().Equals(metalCode));
		if (metalProps == null)
		{
			api.Logger.Warning("Collectible {0} has quenchable property, but cannot find metal {1} in metal.json world properties file.", collObj.Code, metalCode);
		}
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		metalGroupCode = properties["metalVariantgroupCode"].AsString("metal");
	}

	public string GetState(ItemStack itemStack)
	{
		return itemStack.Attributes.GetString("metalworkingstate", "settled");
	}

	public void SetState(IWorldAccessor world, ItemStack itemStack, string state)
	{
		if (!(GetState(itemStack) == state))
		{
			itemStack.Attributes.SetString("metalworkingstate", state);
			itemStack.Attributes.SetDouble("statechangetotalhours", world.Calendar.ElapsedHours);
		}
	}

	public float GetShatterChance(IWorldAccessor world, ItemStack itemstack)
	{
		return itemstack.Attributes.GetFloat("shatterchance", BreakChancePerQuench);
	}

	public void SetShatterChance(IWorldAccessor world, ItemStack itemstack, float value)
	{
		itemstack.Attributes.SetFloat("shatterchance", value);
	}

	public float GetPowerValue(IWorldAccessor world, ItemStack itemstack)
	{
		return itemstack.Attributes.GetFloat("powervalue");
	}

	public void SetPowerValue(IWorldAccessor world, ItemStack itemstack, float value)
	{
		itemstack.Attributes.SetFloat("powervalue", value);
	}

	public float GetDurationBonus(IWorldAccessor world, ItemStack itemstack)
	{
		return itemstack.Attributes.GetFloat("durationbonus");
	}

	public void SetDurationBonus(IWorldAccessor world, ItemStack itemstack, float value)
	{
		itemstack.Attributes.SetFloat("durationbonus", value);
	}

	public override void AfterGetTemperature(IWorldAccessor world, ItemStack itemstack, float temperature, ref EnumHandling handling)
	{
		if (metalProps != null)
		{
			updateSettleTimestamps(world, itemstack, temperature);
			string currentState = GetState(itemstack);
			trySettleWorkItem(world, itemstack, temperature, currentState);
		}
	}

	public override void SetTemperature(IWorldAccessor world, ItemStack itemstack, float temperature, bool delayCooldown, ref EnumHandling handling)
	{
		if (metalProps == null)
		{
			return;
		}
		updateSettleTimestamps(world, itemstack, temperature);
		string currentState = GetState(itemstack);
		if (currentState == "overheat")
		{
			if (temperature < (float)metalProps.settledTemperature)
			{
				SetState(world, itemstack, "settled");
			}
			return;
		}
		if (temperature > (float)metalProps.quenchMaxTemp)
		{
			SetState(world, itemstack, "overheat");
		}
		else if (temperature > (float)metalProps.quenchMinTemp)
		{
			SetState(world, itemstack, "quench");
		}
		else if (temperature > (float)metalProps.temperMinTemp)
		{
			if (temperature > (float)metalProps.temperMaxTemp && currentState == "temper")
			{
				SetState(world, itemstack, "settled");
			}
			if (currentState == "settled")
			{
				SetState(world, itemstack, "temper");
			}
		}
		trySettleWorkItem(world, itemstack, temperature, currentState);
	}

	private void updateSettleTimestamps(IWorldAccessor world, ItemStack itemstack, float temperature)
	{
		bool num = temperature > (float)metalProps.quenchMinTemp && temperature < (float)metalProps.quenchMaxTemp;
		bool nowTempering = temperature > (float)metalProps.temperMinTemp && temperature < (float)metalProps.temperMaxTemp;
		if (num)
		{
			itemstack.Attributes.SetDouble("lastinquenchrangetotalhours", world.Calendar.ElapsedHours);
		}
		if (nowTempering)
		{
			itemstack.Attributes.SetDouble("lastintemperrangetotalhours", world.Calendar.ElapsedHours);
		}
	}

	private void IsGettingCooled(IWorldAccessor world, ItemSlot slot, Vec3d pos, float dt, float temperature)
	{
		if (world.Side == EnumAppSide.Client)
		{
			return;
		}
		string currentState = GetState(slot.Itemstack);
		if ((currentState == "quench" || currentState == "overheat") && temperature < collObj.GetTemperature(world, slot.Itemstack) - 2f)
		{
			float shatterChance = GetShatterChance(world, slot.Itemstack);
			if (!slot.Itemstack.TempAttributes.HasAttribute("willbreak"))
			{
				slot.Itemstack.TempAttributes.SetBool("willbreak", world.Rand.NextDouble() < (double)shatterChance);
			}
			if (slot.Itemstack.TempAttributes.GetBool("willbreak") && (temperature <= (float)metalProps.settledTemperature || world.Rand.NextDouble() < 0.05))
			{
				world.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), pos.X, pos.Y, pos.Z, null, randomizePitch: false, 16f);
				world.SpawnCubeParticles(pos, slot.Itemstack, 0.25f, 30);
				slot.Itemstack = null;
				slot.MarkDirty();
			}
		}
	}

	private void trySettleWorkItem(IWorldAccessor world, ItemStack itemstack, float temperature, string currentState)
	{
		if (!(currentState == "settled") && temperature <= (float)metalProps.settledTemperature)
		{
			if (currentState == "quench" && world.Calendar.ElapsedHours - itemstack.Attributes.GetDouble("lastinquenchrangetotalhours", -999.0) < 0.25)
			{
				applyQuenchedStats(world, itemstack);
			}
			if (currentState == "temper" && world.Calendar.ElapsedHours - itemstack.Attributes.GetDouble("lastintemperrangetotalhours", -999.0) > 1.0)
			{
				applyTemperedStats(world, itemstack);
			}
			SetState(world, itemstack, "settled");
			itemstack.TempAttributes.RemoveAttribute("willbreak");
		}
	}

	private void applyTemperedStats(IWorldAccessor world, ItemStack itemstack)
	{
		int temperIteration = itemstack.Attributes.GetInt("temperIteration");
		int quenchIteration = itemstack.Attributes.GetInt("quenchIteration");
		if (temperIteration < quenchIteration)
		{
			float effectiveness = 1f / (1f + (float)temperIteration * 0.05f);
			float newShatterChance = GetShatterChance(world, itemstack) * GameMath.Mix(1f, TemperShatterMultiplier, effectiveness);
			float newPowerValue = GetPowerValue(world, itemstack) * GameMath.Mix(1f, TemperPowerMultiplier, effectiveness);
			SetShatterChance(world, itemstack, newShatterChance);
			SetPowerValue(world, itemstack, newPowerValue);
			List<AppliedCollectibleBuff> buffs = new List<AppliedCollectibleBuff>();
			if (newPowerValue != 0f && collObj.GetBehavior<CollectibleBehaviorBuffable>() != null)
			{
				buffs.Add(new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + newPowerValue,
					StatCode = "attackpower"
				});
				buffs.Add(new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + newPowerValue,
					StatCode = "miningspeed"
				});
			}
			itemstack.Attributes.SetInt("temperIteration", temperIteration + 1);
			if (newPowerValue != 0f && buffs.Count > 0)
			{
				applyBuffs(itemstack, buffs, EnumBuffAddType.ReplaceOnDuplicate);
			}
		}
	}

	private void applyQuenchedStats(IWorldAccessor world, ItemStack itemstack)
	{
		bool num = itemstack.Attributes.GetBool("clayCovered");
		int quenchIteration = itemstack.Attributes.GetInt("quenchIteration");
		float shatterChance = GetShatterChance(world, itemstack);
		SetShatterChance(world, itemstack, shatterChance + BreakChancePerQuench);
		bool updateDurability = false;
		float percentBroken = (float)collObj.GetRemainingDurability(itemstack) / (float)collObj.GetMaxDurability(itemstack);
		List<AppliedCollectibleBuff> buffs = new List<AppliedCollectibleBuff>();
		bool isBuffable = collObj.GetBehavior<CollectibleBehaviorBuffable>() != null;
		if (num)
		{
			float newDurability = GetDurationBonus(world, itemstack) + 0.2f / (1f + (float)quenchIteration * 0.2f);
			SetDurationBonus(world, itemstack, newDurability);
			if (isBuffable)
			{
				updateDurability = true;
				buffs.Add(new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + newDurability,
					StatCode = "maxdurability"
				});
			}
		}
		else
		{
			float newPower = GetPowerValue(world, itemstack) + 0.1f / (1f + (float)quenchIteration * 0.2f);
			SetPowerValue(world, itemstack, newPower);
			if (isBuffable)
			{
				buffs.Add(new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + newPower,
					StatCode = "attackpower"
				});
				buffs.Add(new AppliedCollectibleBuff
				{
					Code = "hardened",
					Multiplier = 1f + newPower,
					StatCode = "miningspeed"
				});
			}
		}
		itemstack.Attributes.SetInt("quenchIteration", quenchIteration + 1);
		itemstack.Attributes.SetBool("clayCovered", value: false);
		if (buffs.Count > 0)
		{
			applyBuffs(itemstack, buffs, EnumBuffAddType.ReplaceOnDuplicate);
		}
		if (updateDurability)
		{
			collObj.SetDurability(itemstack, (int)(percentBroken * (float)collObj.GetMaxDurability(itemstack)));
		}
	}

	private void applyBuffs(ItemStack stack, List<AppliedCollectibleBuff> buffs, EnumBuffAddType mode = EnumBuffAddType.AddStat)
	{
		collObj.GetBehavior<CollectibleBehaviorBuffable>()?.ApplyBuffs(stack, buffs, mode);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		int timesQuenched = inSlot.Itemstack.Attributes.GetInt("quenchIteration");
		int temperIteration = inSlot.Itemstack.Attributes.GetInt("temperIteration");
		dsc.AppendLine(Lang.Get("itemstack-quenchable", metalProps.quenchMinTemp, metalProps.quenchMaxTemp));
		if (timesQuenched > temperIteration)
		{
			dsc.AppendLine(Lang.Get("itemstack-temperable", metalProps.temperMinTemp, metalProps.temperMaxTemp));
		}
		if (inSlot.Itemstack.Attributes.GetBool("clayCovered"))
		{
			dsc.AppendLine(Lang.Get("itemstack-claycovered"));
		}
		int timesTempered = inSlot.Itemstack.Attributes.GetInt("temperIteration");
		if (timesTempered > 0)
		{
			dsc.AppendLine(Lang.Get("quenchable-tempered-amount", timesTempered));
		}
		if (timesQuenched > 0)
		{
			dsc.AppendLine(Lang.Get("quenchable-quenched-amount", timesQuenched));
			dsc.AppendLine(Lang.Get("quenchable-power-gain", GetPowerValue(world, inSlot.Itemstack)));
			dsc.AppendLine(Lang.Get("quenchable-durability-gain", GetDurationBonus(world, inSlot.Itemstack)));
		}
		dsc.AppendLine(Lang.Get("quenchable-shatter-chance", GetShatterChance(world, inSlot.Itemstack)));
		if (GetState(inSlot.Itemstack) == "quench")
		{
			dsc.AppendLine(Lang.Get("quenchable-reached-quenching-temperature"));
		}
		if (GetState(inSlot.Itemstack) == "temper" && timesQuenched > temperIteration)
		{
			dsc.AppendLine(Lang.Get("quenchable-reached-tempering-temperature"));
		}
	}
}
