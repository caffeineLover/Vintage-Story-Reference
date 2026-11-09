using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	public static class ShieldAutoPatcher
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0001B576 File Offset: 0x00019776
		[NullableContext(2)]
		public static bool IsCombatOverhaulEnabled(ICoreAPI api)
		{
			return (api != null && api.ModLoader.IsModEnabled("combatoverhaul")) || (api != null && api.ModLoader.IsModEnabled("combatoverhaulfork"));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001B5A4 File Offset: 0x000197A4
		public static void Patch(ICoreAPI api)
		{
			if (!ShieldAutoPatcher.IsCombatOverhaulEnabled(api))
			{
				return;
			}
			int patched = 0;
			foreach (Item item in api.World.Items)
			{
				if (CollectibleClassifier.IsShield(item) && !CollectibleClassifier.HasMeleeWeaponActions(item))
				{
					try
					{
						ShieldAutoPatcher.EnsureCombatAttributes(item);
						if (ShieldAutoPatcher.AttachMeleeBehavior(item, api))
						{
							patched++;
						}
					}
					catch (Exception exception)
					{
						Type typeFromHandle = typeof(ShieldAutoPatcher);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Error while patching shield '");
						defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(item.Code);
						defaultInterpolatedStringHandler.AppendLiteral("':\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
						LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			if (patched > 0)
			{
				ILogger logger = api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(85, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("[OverhaullibLegacyCompat] Applied Combat Overhaul shield behavior to ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(patched);
				defaultInterpolatedStringHandler2.AppendLiteral(" shield item(s).");
				logger.Notification(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001B6C0 File Offset: 0x000198C0
		private static void EnsureCombatAttributes(Item item)
		{
			JsonObject attributes2 = item.Attributes;
			JObject attributes = (((attributes2 != null) ? attributes2.Token : null) as JObject) ?? new JObject();
			item.Attributes = new JsonObject(attributes);
			JObject defaultOffHandStance = ShieldAutoPatcher.CreateDefaultOffHandStance(item);
			JObject modes = attributes["Modes"] as JObject;
			if (modes != null && Enumerable.Any<JProperty>(modes.Properties()))
			{
				foreach (JProperty jproperty in modes.Properties())
				{
					JObject modeStats = jproperty.Value as JObject;
					if (modeStats != null && modeStats["OffHandStance"] == null)
					{
						modeStats["OffHandStance"] = defaultOffHandStance.DeepClone();
					}
				}
				return;
			}
			if (attributes["OffHandStance"] == null)
			{
				attributes["OffHandStance"] = defaultOffHandStance;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001B7A4 File Offset: 0x000199A4
		private static JObject CreateDefaultOffHandStance(Item item)
		{
			AssetLocation code = item.Code;
			object obj;
			if (code == null)
			{
				obj = 0;
			}
			else
			{
				string path = code.Path;
				obj = ((path != null) ? new bool?(path.Contains("blackguard")) : default(bool?)).GetValueOrDefault();
			}
			object obj2 = obj;
			string heavySound = (obj2 != null) ? "game:sounds/held/shieldblock-metal-heavy" : "game:sounds/held/shieldblock-wood-heavy";
			string lightSound = (obj2 != null) ? "game:sounds/held/shieldblock-metal-light" : "game:sounds/held/shieldblock-wood-light";
			int parryTier = (obj2 != null) ? 8 : 4;
			int blockTier = (obj2 != null) ? 4 : 2;
			int staggerTier = (obj2 != null) ? 8 : 4;
			JObject jobject = new JObject();
			jobject["CanAttack"] = false;
			jobject["CanParry"] = false;
			jobject["CanBlock"] = true;
			jobject["CanSprint"] = true;
			jobject["SpeedPenalty"] = 0f;
			jobject["BlockSpeedPenalty"] = -0.1f;
			jobject["ParryCooldownMs"] = 600;
			jobject["BlockCooldownMs"] = 0;
			JObject jobject2 = jobject;
			string text = "Parry";
			JObject jobject3 = new JObject();
			jobject3["Zones"] = new JArray(new object[]
			{
				"Head",
				"Face",
				"Neck",
				"Torso",
				"LeftArm",
				"RightArm",
				"LeftHand",
				"RightHand",
				"LeftLeg",
				"RightLeg",
				"LeftFoot",
				"RightFoot"
			});
			jobject3["Directions"] = new JArray(new object[]
			{
				30,
				45,
				30,
				45
			});
			jobject3["Sound"] = heavySound;
			JObject jobject4 = jobject3;
			string text2 = "BlockTier";
			JObject jobject5 = new JObject();
			jobject5["BluntAttack"] = parryTier;
			jobject5["SlashingAttack"] = parryTier;
			jobject5["PiercingAttack"] = parryTier;
			jobject4[text2] = jobject5;
			jobject3["StaggerTier"] = staggerTier;
			jobject3["StaggerTimeMs"] = 2000;
			jobject2[text] = jobject3;
			JObject jobject6 = jobject;
			string text3 = "Block";
			jobject3 = new JObject();
			jobject3["Zones"] = new JArray(new object[]
			{
				"Face",
				"Neck",
				"Torso",
				"LeftArm",
				"RightArm",
				"LeftHand",
				"RightHand",
				"LeftLeg",
				"RightLeg"
			});
			jobject3["Directions"] = new JArray(new object[]
			{
				30,
				60,
				45,
				60
			});
			jobject3["Sound"] = lightSound;
			JObject jobject7 = jobject3;
			string text4 = "BlockTier";
			JObject jobject8 = new JObject();
			jobject8["BluntAttack"] = blockTier;
			jobject8["SlashingAttack"] = blockTier;
			jobject8["PiercingAttack"] = blockTier;
			jobject7[text4] = jobject8;
			jobject6[text3] = jobject3;
			jobject["BlockAnimation"] = "combatoverhaul:shield-light-parry";
			jobject["ReadyAnimation"] = "combatoverhaul:shield-light-ready";
			jobject["IdleAnimation"] = "combatoverhaul:shield-light-ready";
			return jobject;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001BB64 File Offset: 0x00019D64
		private static bool AttachMeleeBehavior(Item item, ICoreAPI api)
		{
			if (CollectibleClassifier.HasMeleeWeaponActions(item))
			{
				return false;
			}
			MeleeWeaponBehavior behavior = new MeleeWeaponBehavior(item);
			behavior.Initialize(new JsonObject(new JObject()));
			behavior.OnLoaded(api);
			item.CollectibleBehaviors = Enumerable.ToArray<CollectibleBehavior>(Enumerable.Append<CollectibleBehavior>(item.CollectibleBehaviors ?? Array.Empty<CollectibleBehavior>(), behavior));
			return true;
		}
	}
}
