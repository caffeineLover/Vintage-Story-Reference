using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace Jaunt.Items
{
	// Token: 0x02000006 RID: 6
	public class ItemJauntInstrument : Item
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000027F2 File Offset: 0x000009F2
		protected static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027FC File Offset: 0x000009FC
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.lockedGroupCode = this.Attributes["groupCode"].AsString(null);
			this.isLocked = !string.IsNullOrEmpty(this.lockedGroupCode);
			this.disallowedBindings = this.Attributes["disallowedBindings"].AsObject<string[]>(new string[0]);
			string soundLoc = this.Attributes["callSound"].AsString(null) ?? "game:sounds/instrument/elkcall";
			this.callSound = new AssetLocation(soundLoc).WithPathPrefixOnce("sounds/");
			this.capi = (api as ICoreClientAPI);
			List<SkillItem> list = new List<SkillItem>();
			list.Add(new SkillItem
			{
				Code = new AssetLocation("play"),
				Name = Lang.Get("jaunt:instrument-skill-play", Array.Empty<object>())
			});
			List<SkillItem> toolModes = list;
			if (!this.isLocked)
			{
				toolModes.Add(new SkillItem
				{
					Code = new AssetLocation("bind"),
					Name = Lang.Get("jaunt:instrument-skill-bind", Array.Empty<object>())
				});
			}
			this.modes = toolModes.ToArray();
			if (this.capi != null)
			{
				this.modes[0].WithIcon(this.capi, this.capi.Gui.LoadSvgWithPadding(new AssetLocation("jaunt:textures/icons/instrument-play.svg"), 48, 48, 5, new int?(-1)));
				this.modes[0].TexturePremultipliedAlpha = false;
				if (!this.isLocked)
				{
					this.modes[1].WithIcon(this.capi, this.capi.Gui.LoadSvgWithPadding(new AssetLocation("jaunt:textures/icons/instrument-bind.svg"), 48, 48, 5, new int?(-1)));
					this.modes[1].TexturePremultipliedAlpha = false;
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000029C0 File Offset: 0x00000BC0
		public override string GetHeldItemName(ItemStack itemStack)
		{
			TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
			if (this.Code == null)
			{
				return "Invalid block, id " + this.Id.ToString();
			}
			if (this.isLocked && string.IsNullOrEmpty(itemStack.Attributes.GetString("groupCode", null)))
			{
				this.SetBoundEntityType(this.lockedGroupCode, itemStack);
			}
			string groupCode = itemStack.Attributes.GetString("groupCode", null);
			string type = ItemClassMethods.Name(this.ItemClass);
			StringBuilder sb = new StringBuilder();
			StringBuilder stringBuilder = sb;
			string[] array = new string[5];
			int num = 0;
			AssetLocation code = this.Code;
			array[num] = ((code != null) ? code.Domain : null);
			array[1] = ":";
			array[2] = type;
			array[3] = "-";
			int num2 = 4;
			AssetLocation code2 = this.Code;
			array[num2] = ((code2 != null) ? code2.Path : null);
			stringBuilder.Append(Lang.GetMatching(string.Concat(array), Array.Empty<object>()));
			string displayCode;
			if (string.IsNullOrEmpty(groupCode))
			{
				displayCode = Lang.Get("jaunt:groupcode-unbound", Array.Empty<object>());
			}
			else
			{
				displayCode = textInfo.ToTitleCase(groupCode);
			}
			sb.Append(" (" + displayCode + ")");
			CollectibleBehavior[] collectibleBehaviors = this.CollectibleBehaviors;
			for (int i = 0; i < collectibleBehaviors.Length; i++)
			{
				collectibleBehaviors[i].GetHeldItemName(sb, itemStack);
			}
			return sb.ToString();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B14 File Offset: 0x00000D14
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			string groupCode = inSlot.Itemstack.Attributes.GetString("groupCode", null);
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			if (string.IsNullOrEmpty(groupCode))
			{
				groupCode = Lang.Get("jaunt:groupcode-unbound", Array.Empty<object>()).ToLower();
				dsc.AppendLine("\n" + Lang.Get("jaunt:instrument-descappend-unbound", Array.Empty<object>()));
				return;
			}
			dsc.AppendLine("\n" + Lang.Get("jaunt:instrument-descappend-bound", new object[]
			{
				groupCode
			}));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (slot.Itemstack.Attributes.GetInt("toolMode", 0) == 0)
			{
				long ela = this.api.World.ElapsedMilliseconds;
				long prevela = slot.Itemstack.Attributes.GetLong("lastPlayerMs", -99999L);
				if (prevela > ela)
				{
					prevela = ela - 4001L;
				}
				if (ela - prevela <= 4000L)
				{
					return;
				}
				slot.Itemstack.Attributes.SetLong("lastPlayerMs", ela);
				IWorldAccessor world = this.api.World;
				AssetLocation assetLocation = this.callSound;
				EntityPlayer entityPlayer = byEntity as EntityPlayer;
				world.PlaySoundAt(assetLocation, byEntity, (entityPlayer != null) ? entityPlayer.Player : null, 0.75f, 32f, 0.5f);
				if (this.api.Side == 1)
				{
					this.callEntity(slot, byEntity);
				}
			}
			else
			{
				if (entitySel == null || entitySel.Entity == null)
				{
					this.capi.TriggerIngameError(this, "no-entity-selected", Lang.Get("jaunt:ingame-error-no-entitysel", Array.Empty<object>()));
					return;
				}
				if (this.SetBoundEntityType(slot, entitySel.Entity))
				{
					slot.Itemstack.Attributes.SetInt("toolMode", 0);
				}
			}
			handling = 4;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CD7 File Offset: 0x00000ED7
		public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			return this.modes;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002CDF File Offset: 0x00000EDF
		public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
		{
			slot.Itemstack.Attributes.SetInt("toolMode", toolMode);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
		{
			return slot.Itemstack.Attributes.GetInt("toolMode", 0);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D10 File Offset: 0x00000F10
		public void SetBoundEntityType(string groupCode, ItemStack itemStack)
		{
			if (string.IsNullOrEmpty(groupCode))
			{
				return;
			}
			itemStack.Attributes.SetString("groupCode", groupCode);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D2C File Offset: 0x00000F2C
		public bool SetBoundEntityType(ItemSlot slot, Entity entity)
		{
			TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
			EntityBehaviorOwnable behavior = entity.GetBehavior<EntityBehaviorOwnable>();
			string groupCode = (behavior != null) ? behavior.Group : null;
			if (string.IsNullOrEmpty(groupCode))
			{
				return false;
			}
			string displayCode = textInfo.ToTitleCase(groupCode);
			if (MemoryExtensions.Contains<string>(this.disallowedBindings, groupCode))
			{
				ICoreClientAPI coreClientAPI = this.capi;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "disallowed-entity", Lang.Get("jaunt:ingame-error-disallowed-entity", new object[]
					{
						groupCode
					}));
				}
				return false;
			}
			slot.Itemstack.Attributes.SetString("groupCode", groupCode);
			ICoreClientAPI coreClientAPI2 = this.capi;
			if (coreClientAPI2 != null)
			{
				coreClientAPI2.TriggerIngameDiscovery(this, "bound-groupcode", Lang.Get("jaunt:discovery-bound-entity", new object[]
				{
					displayCode
				}));
			}
			return true;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DEC File Offset: 0x00000FEC
		private void callEntity(ItemSlot slot, EntityAgent byEntity)
		{
			string groupCode = slot.Itemstack.Attributes.GetString("groupCode", "");
			IPlayer plr = (byEntity as EntityPlayer).Player;
			Dictionary<string, EntityOwnership> ownerships;
			EntityOwnership ownership;
			if (!this.api.ModLoader.GetModSystem<ModSystemEntityOwnership>(true).OwnerShipsByPlayerUid.TryGetValue(plr.PlayerUID, ref ownerships) || ownerships == null || !ownerships.TryGetValue(groupCode, ref ownership))
			{
				return;
			}
			Entity entity = this.api.World.GetEntityById(ownership.EntityId);
			if (entity == null)
			{
				return;
			}
			EntityBehaviorMortallyWoundable mw = entity.GetBehavior<EntityBehaviorMortallyWoundable>();
			if ((mw != null && mw.HealthState == 2) || (mw != null && mw.HealthState == 3))
			{
				return;
			}
			AiTaskManager tm = entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager;
			AiTaskComeToOwner aitcto = Enumerable.FirstOrDefault<IAiTask>(tm.AllTasks, (IAiTask t) => t is AiTaskComeToOwner) as AiTaskComeToOwner;
			double distToPlayer = entity.Pos.DistanceTo(byEntity.Pos);
			if (slot.Itemstack.Collectible.Durability != 0)
			{
				int damageByDist = (int)Math.Ceiling(distToPlayer / 20.0);
				slot.Itemstack.Collectible.DamageItem(this.api.World, byEntity, slot, Math.Max(1, damageByDist), true);
			}
			if (distToPlayer > (double)aitcto.TeleportMaxRange)
			{
				return;
			}
			IMountable mount = (entity != null) ? entity.GetInterface<IMountable>() : null;
			if (mount != null)
			{
				if (MountableUtil.IsMountedBy(mount, plr.Entity))
				{
					return;
				}
				if (mount.AnyMounted())
				{
					EntityBehaviorRideable behavior = entity.GetBehavior<EntityBehaviorRideable>();
					if (behavior != null)
					{
						behavior.UnmountPassengers();
					}
				}
			}
			entity.AlwaysActive = true;
			entity.State = 0;
			aitcto.allowTeleportCount = 1;
			tm.StopTasks();
			tm.ExecuteTask(aitcto, 0);
		}

		// Token: 0x04000017 RID: 23
		private SkillItem[] modes;

		// Token: 0x04000018 RID: 24
		private ICoreClientAPI capi;

		// Token: 0x04000019 RID: 25
		private bool isLocked;

		// Token: 0x0400001A RID: 26
		private string lockedGroupCode;

		// Token: 0x0400001B RID: 27
		private string[] disallowedBindings;

		// Token: 0x0400001C RID: 28
		private AssetLocation callSound;
	}
}
