using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeapon : Item, IHasMultipleWeaponLogicModes, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicMoveAnimations, IHasDynamicIdleAnimations, IHasMeleeWeaponActions, IHasServerBlockCallback, ISetsRenderingOffset, IMouseWheelInput, IHeldItemOnMouseWheel, IOnGameTick, IRestrictAction, IHandBookPageCodeProvider, IHandbookGrouping
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x000290BC File Offset: 0x000272BC
		[Nullable(2)]
		public MeleeWeaponClient ClientLogic
		{
			[NullableContext(2)]
			get
			{
				MeleeWeaponClientModesCollection clientModes = this.ClientModes;
				if (clientModes == null)
				{
					return null;
				}
				return clientModes.CurrentMode;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x000290CF File Offset: 0x000272CF
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x000290D7 File Offset: 0x000272D7
		[Nullable(2)]
		public MeleeWeaponServer ServerLogic { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x000290E0 File Offset: 0x000272E0
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x000290E8 File Offset: 0x000272E8
		[Nullable(2)]
		public MeleeWeaponClientModesCollection ClientModes { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x000290F1 File Offset: 0x000272F1
		[Nullable(2)]
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			[NullableContext(2)]
			get
			{
				return this.ClientLogic;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x000290FC File Offset: 0x000272FC
		IEnumerable<IClientWeaponLogic> IHasMultipleWeaponLogicModes.ClientLogicModes
		{
			get
			{
				MeleeWeaponClientModesCollection clientModes = this.ClientModes;
				IEnumerable<IClientWeaponLogic> enumerable;
				if (clientModes == null)
				{
					enumerable = null;
				}
				else
				{
					enumerable = Enumerable.Select<KeyValuePair<string, MeleeWeaponClient>, MeleeWeaponClient>(clientModes.Clients, (KeyValuePair<string, MeleeWeaponClient> entry) => entry.Value);
				}
				return enumerable ?? Array.Empty<MeleeWeaponClient>();
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00029148 File Offset: 0x00027348
		[Nullable(2)]
		IServerRangedWeaponLogic IHasRangedWeaponLogic.ServerWeaponLogic
		{
			[NullableContext(2)]
			get
			{
				return this.ServerLogic;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00029150 File Offset: 0x00027350
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x00029158 File Offset: 0x00027358
		public bool RenderingOffset { get; set; }

		// Token: 0x06000678 RID: 1656 RVA: 0x00029161 File Offset: 0x00027361
		public virtual string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
		{
			return TextureAttributeHandbook.PageCodeForStack(stack, TextureAttributeHandbook.GetTextureAttributes(this));
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002916F File Offset: 0x0002736F
		public AssetLocation GetCodeForHandbookGrouping(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			return ((collectible != null) ? collectible.Code : null) ?? this.Code;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002918D File Offset: 0x0002738D
		public string GetWildcardForHandbookGrouping(string wildcard, ItemStack stack)
		{
			return TextureAttributeHandbook.NormalizeHandbookGroupWildcard(wildcard, stack);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00029196 File Offset: 0x00027396
		public override bool Satisfies(ItemStack thisStack, ItemStack otherStack)
		{
			ICoreAPI api = this.api;
			return TextureAttributeHandbook.SatisfiesIgnoringAttributes((api != null) ? api.World : null, thisStack, otherStack, TextureAttributeHandbook.GetTextureAttributes(this)) || base.Satisfies(thisStack, otherStack);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000291C3 File Offset: 0x000273C3
		public bool RestrictRightHandAction()
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictRightHandAction();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000291D6 File Offset: 0x000273D6
		public bool RestrictLeftHandAction()
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictLeftHandAction();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000291EC File Offset: 0x000273EC
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.ExpandDamageStatTemplates();
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.LoadClientSide(clientAPI);
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this.LoadServerSide(serverAPI);
			}
			this.AltForInteractions = new WorldInteraction
			{
				MouseButton = 255,
				HotKeyCode = "Alt",
				ActionLangCode = "combatoverhaul:interaction-hold-alt"
			};
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00029254 File Offset: 0x00027454
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetIdleAnimation(player, slot, mainHand);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00029280 File Offset: 0x00027480
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetReadyAnimation(player, slot, mainHand);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x000292AC File Offset: 0x000274AC
		public AnimationRequestByCode? GetWalkAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetWalkAnimation(player, slot, mainHand);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000292D8 File Offset: 0x000274D8
		public AnimationRequestByCode? GetRunAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetRunAnimation(player, slot, mainHand);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00029304 File Offset: 0x00027504
		public AnimationRequestByCode? GetSwimAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetSwimAnimation(player, slot, mainHand);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00029330 File Offset: 0x00027530
		public AnimationRequestByCode? GetSwimIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetSwimIdleAnimation(player, slot, mainHand);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00029359 File Offset: 0x00027559
		public override void OnHeldRenderOpaque(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			base.OnHeldRenderOpaque(inSlot, byPlayer);
			if (DebugWindowManager.RenderDebugColliders)
			{
				MeleeWeaponClient clientLogic = this.ClientLogic;
				if (clientLogic == null)
				{
					return;
				}
				clientLogic.RenderDebugCollider(inSlot, byPlayer);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002937C File Offset: 0x0002757C
		public bool CanAttack(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.CanAttack(player, mainHand);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00029391 File Offset: 0x00027591
		public bool CanBlock(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null || !clientLogic.CanBlock(player, mainHand))
			{
				MeleeWeaponClient clientLogic2 = this.ClientLogic;
				return clientLogic2 != null && clientLogic2.CanParry(player, mainHand);
			}
			return true;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000293BE File Offset: 0x000275BE
		public bool CanThrow(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.CanThrow(player, mainHand);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000293D3 File Offset: 0x000275D3
		public void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return;
			}
			clientLogic.OnGameTick(slot, player, ref state, mainHand);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x000293EA File Offset: 0x000275EA
		public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000293F0 File Offset: 0x000275F0
		public override float OnBlockBreaking(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
		{
			return remainingResistance;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000293F4 File Offset: 0x000275F4
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			SplitMaterialWeaponUtil.Normalize(inSlot.Itemstack);
			SplitMaterialWeaponUtil.NormalizeDurabilityIfNeeded(inSlot.Itemstack);
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic != null)
			{
				clientLogic.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			}
			WeaponBuffSystem.AppendTooltip(inSlot.Itemstack, dsc, world, withDebugInfo);
			SplitMaterialWeaponUtil.AppendTooltip(inSlot.Itemstack, dsc);
			dsc.AppendLine("");
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00029460 File Offset: 0x00027660
		public override string GetHeldItemName(ItemStack itemStack)
		{
			return SplitMaterialWeaponUtil.GetHeldItemName(itemStack, base.GetHeldItemName(itemStack));
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00029470 File Offset: 0x00027670
		[return: Nullable(2)]
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			WorldInteraction[] interactionHelp = base.GetHeldInteractionHelp(inSlot);
			if (this.ChangeGripInteraction != null)
			{
				interactionHelp = ((interactionHelp != null) ? ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.ChangeGripInteraction) : null);
			}
			MeleeWeaponClientModesCollection clientModes = this.ClientModes;
			if (clientModes != null && clientModes.Modes.Count > 1)
			{
				interactionHelp = ((interactionHelp != null) ? ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.ModesSelectionInteraction) : null);
			}
			if (interactionHelp == null)
			{
				return null;
			}
			return ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.AltForInteractions);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000294E0 File Offset: 0x000276E0
		public virtual void BlockCallback(IServerPlayer player, ItemSlot slot, bool mainHand, float damageBlocked, int attackTier, int blockTier)
		{
			int durabilityDamage = 1 + Math.Clamp(attackTier - blockTier, 0, attackTier);
			this.DamageItem(player.Entity.World, player.Entity, slot, durabilityDamage, true);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00029517 File Offset: 0x00027717
		public virtual bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.OnMouseWheel(slot, byPlayer, delta);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00029530 File Offset: 0x00027730
		public void OnMouseWheel(EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			if (((clientApi != null) ? clientApi.World.Player : null) == null)
			{
				return;
			}
			float delta = MouseWheelPatch.GetDelta(args);
			if (delta == 0f)
			{
				return;
			}
			if (this.OnMouseWheel(inSlot, this.ClientApi.World.Player, delta))
			{
				args.SetHandled(true);
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00029588 File Offset: 0x00027788
		public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
		{
			MeleeWeaponClientModesCollection clientModes = this.ClientModes;
			if (clientModes == null)
			{
				return 0;
			}
			return clientModes.GetToolMode(byPlayer.Entity, slot);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000295A4 File Offset: 0x000277A4
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = forPlayer.Entity.EntityId;
			if ((num.GetValueOrDefault() == entityId & num != null) && this.ClientModes != null)
			{
				return this.ClientModes.GetToolModes(forPlayer.Entity, slot);
			}
			return null;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002961C File Offset: 0x0002781C
		public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = byPlayer.Entity.EntityId;
			if ((num.GetValueOrDefault() == entityId & num != null) && this.ClientModes != null)
			{
				this.ClientModes.SetToolMode(byPlayer.Entity, slot, toolMode);
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00029694 File Offset: 0x00027894
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			MeleeWeapon.PreserveHandleTexture(allInputslots, outputSlot);
			SplitMaterialWeaponUtil.Normalize(outputSlot.Itemstack);
			SplitMaterialWeaponUtil.NormalizeDurabilityIfNeeded(outputSlot.Itemstack);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000296CC File Offset: 0x000278CC
		private static void PreserveHandleTexture(ItemSlot[] allInputslots, ItemSlot outputSlot)
		{
			if (!outputSlot.Empty)
			{
				ItemStack itemstack = outputSlot.Itemstack;
				AssetLocation assetLocation;
				if (itemstack == null)
				{
					assetLocation = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					assetLocation = ((collectible != null) ? collectible.Code : null);
				}
				if (!(assetLocation == null))
				{
					string handleTexture = null;
					string leatherTexture = null;
					foreach (ItemSlot itemSlot in allInputslots)
					{
						ItemStack inputStack = itemSlot.Itemstack;
						if (!itemSlot.Empty)
						{
							AssetLocation assetLocation2;
							if (inputStack == null)
							{
								assetLocation2 = null;
							}
							else
							{
								CollectibleObject collectible2 = inputStack.Collectible;
								assetLocation2 = ((collectible2 != null) ? collectible2.Code : null);
							}
							if (!(assetLocation2 == null))
							{
								string inputHandleTexture = inputStack.Attributes.GetString("handleTexture", null);
								if (!string.IsNullOrWhiteSpace(inputHandleTexture))
								{
									if (MeleeWeapon.IsLeatherTexture(inputHandleTexture))
									{
										if (leatherTexture == null)
										{
											leatherTexture = inputHandleTexture;
										}
									}
									else if (handleTexture == null)
									{
										handleTexture = inputHandleTexture;
									}
								}
								if (leatherTexture == null)
								{
									leatherTexture = MeleeWeapon.GetLeatherTexture(inputStack);
								}
							}
						}
					}
					string texture = MeleeWeapon.IsSteelLike(outputSlot.Itemstack) ? (leatherTexture ?? handleTexture) : (handleTexture ?? leatherTexture);
					if (!string.IsNullOrWhiteSpace(texture))
					{
						outputSlot.Itemstack.Attributes.SetString("handleTexture", texture);
					}
					return;
				}
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000297D4 File Offset: 0x000279D4
		private static bool IsSteelLike(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			string path = text ?? "";
			return path.Contains("-steel", 4) || path.Contains("-meteoricsteel", 4);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00029825 File Offset: 0x00027A25
		private static bool IsLeatherTexture(string texture)
		{
			return texture.StartsWith("game:block/leather/", 4) || texture.StartsWith("block/leather/", 4);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00029844 File Offset: 0x00027A44
		[return: Nullable(2)]
		private static string GetLeatherTexture(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			AssetLocation code = (collectible != null) ? collectible.Code : null;
			if (((code != null) ? code.Domain : null) != "game" || !code.Path.StartsWith("leather-normal-", 4))
			{
				return null;
			}
			string path = code.Path;
			int length = "leather-normal-".Length;
			string color = path.Substring(length, path.Length - length);
			if (!string.IsNullOrWhiteSpace(color))
			{
				return "game:block/leather/" + color;
			}
			return null;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000298C8 File Offset: 0x00027AC8
		protected virtual void LoadClientSide(ICoreClientAPI clientAPI)
		{
			this.ClientApi = clientAPI;
			this.ClientModes = new MeleeWeaponClientModesCollection(clientAPI, this);
			MeleeWeaponStats Stats = this.Attributes.AsObject<MeleeWeaponStats>(null);
			this.RenderingOffset = Stats.RenderingOffset;
			StanceStats oneHandedStance = Stats.OneHandedStance;
			float? num = (oneHandedStance != null) ? new float?(oneHandedStance.GripMinLength) : default(float?);
			StanceStats oneHandedStance2 = Stats.OneHandedStance;
			float? num2 = (oneHandedStance2 != null) ? new float?(oneHandedStance2.GripMaxLength) : default(float?);
			if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
			{
				StanceStats twoHandedStance = Stats.TwoHandedStance;
				num2 = ((twoHandedStance != null) ? new float?(twoHandedStance.GripMinLength) : default(float?));
				StanceStats twoHandedStance2 = Stats.TwoHandedStance;
				num = ((twoHandedStance2 != null) ? new float?(twoHandedStance2.GripMaxLength) : default(float?));
				if (num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null))
				{
					goto IL_111;
				}
			}
			this.ChangeGripInteraction = new WorldInteraction
			{
				MouseButton = 13,
				ActionLangCode = "combatoverhaul:interaction-grip-change"
			};
			IL_111:
			this.ModesSelectionInteraction = new WorldInteraction
			{
				ActionLangCode = Lang.Get("combatoverhaul:interaction-mode-selection", Array.Empty<object>()),
				HotKeyCodes = new string[]
				{
					"toolmodeselect"
				},
				MouseButton = 255
			};
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00029A2C File Offset: 0x00027C2C
		protected virtual void LoadServerSide(ICoreServerAPI serverAPI)
		{
			this.ServerLogic = new MeleeWeaponServer(serverAPI, this);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00029A3C File Offset: 0x00027C3C
		protected virtual void ExpandDamageStatTemplates()
		{
			JsonObject attributes2 = this.Attributes;
			JObject attributes = ((attributes2 != null) ? attributes2.Token : null) as JObject;
			if (attributes == null)
			{
				return;
			}
			JObject templates = attributes["DamageStatTemplates"] as JObject;
			if (templates == null)
			{
				return;
			}
			JObject modes = attributes["Modes"] as JObject;
			if (modes != null)
			{
				foreach (JProperty mode in modes.Properties())
				{
					this.ExpandDamageStatTemplates(mode.Value as JObject, templates);
				}
				return;
			}
			this.ExpandDamageStatTemplates(attributes, templates);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00029AE4 File Offset: 0x00027CE4
		private void ExpandDamageStatTemplates([Nullable(2)] JObject stats, JObject templates)
		{
			if (stats == null)
			{
				return;
			}
			foreach (JObject attack in Enumerable.OfType<JObject>(stats.DescendantsAndSelf()))
			{
				if (attack["DamageTypes"] == null)
				{
					JArray templateNames = attack["DamageStatsTemplates"] as JArray;
					if (templateNames != null)
					{
						JArray damageTypes = new JArray();
						foreach (JToken jtoken in templateNames)
						{
							string name = Extensions.Value<string>(jtoken);
							if (name != null)
							{
								JObject template = templates[name] as JObject;
								if (template != null)
								{
									damageTypes.Add(this.CreateDamageTypeFromTemplate(template));
								}
							}
						}
						if (damageTypes.Count > 0)
						{
							attack["DamageTypes"] = damageTypes;
						}
					}
				}
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00029BD8 File Offset: 0x00027DD8
		private JObject CreateDamageTypeFromTemplate(JObject template)
		{
			JObject jobject = new JObject();
			string text = "DamageType";
			JToken jtoken = template["DamageType"];
			jobject[text] = (((jtoken != null) ? jtoken.DeepClone() : null) ?? "BluntAttack");
			jobject["Tier"] = this.ResolveTemplateTier(template);
			string text2 = "ArmorPiercingTier";
			JToken jtoken2 = template["ArmorPiercingTier"];
			jobject[text2] = (((jtoken2 != null) ? jtoken2.DeepClone() : null) ?? 0);
			string text3 = "Damage";
			JToken jtoken3 = template["Damage"];
			jobject[text3] = (((jtoken3 != null) ? jtoken3.DeepClone() : null) ?? 0);
			JObject damage = jobject;
			JObject jobject2 = new JObject();
			jobject2["Damage"] = damage;
			string text4 = "Knockback";
			JToken jtoken4 = template["Knockback"];
			jobject2[text4] = (((jtoken4 != null) ? jtoken4.DeepClone() : null) ?? 0);
			string text5 = "DurabilityDamage";
			JToken jtoken5 = template["DurabilityDamage"];
			jobject2[text5] = (((jtoken5 != null) ? jtoken5.DeepClone() : null) ?? 1);
			jobject2["Collider"] = MeleeWeapon.ResolveTemplateCollider(template["Collider"]);
			string text6 = "Radius";
			JToken jtoken6 = template["Radius"];
			jobject2[text6] = (((jtoken6 != null) ? jtoken6.DeepClone() : null) ?? 0.1f);
			string text7 = "StaggerTimeMs";
			JToken jtoken7 = template["StaggerTimeMs"];
			jobject2[text7] = (((jtoken7 != null) ? jtoken7.DeepClone() : null) ?? 0);
			string text8 = "StaggerTier";
			JToken jtoken8 = template["StaggerTier"];
			jobject2[text8] = (((jtoken8 != null) ? jtoken8.DeepClone() : null) ?? 1);
			string text9 = "PushTier";
			JToken jtoken9 = template["PushTier"];
			jobject2[text9] = (((jtoken9 != null) ? jtoken9.DeepClone() : null) ?? 0);
			return jobject2;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00029DC4 File Offset: 0x00027FC4
		private int ResolveTemplateTier(JObject template)
		{
			if (template["Tier"] != null)
			{
				return Extensions.Value<int>(template["Tier"]);
			}
			JObject tierByType = template["TierByType"] as JObject;
			if (tierByType == null)
			{
				return 0;
			}
			AssetLocation code2 = this.Code;
			string code = ((code2 != null) ? code2.ToString() : null) ?? "";
			AssetLocation code3 = this.Code;
			string shortCode = ((code3 != null) ? code3.ToShortString() : null) ?? code;
			foreach (JProperty entry in tierByType.Properties())
			{
				if (WildcardUtil.Match(entry.Name, code) || WildcardUtil.Match(entry.Name, shortCode))
				{
					return Extensions.Value<int>(entry.Value);
				}
			}
			return 0;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00029EA8 File Offset: 0x000280A8
		private static JToken ResolveTemplateCollider([Nullable(2)] JToken collider)
		{
			JArray array = collider as JArray;
			if (array != null)
			{
				return array.DeepClone();
			}
			float[] array2;
			if (((collider != null) ? Extensions.Value<int>(collider) : 0) == 1)
			{
				array2 = new float[]
				{
					0.5f,
					0f,
					0.5f,
					1.3f,
					0f,
					0.5f
				};
			}
			else
			{
				array2 = new float[]
				{
					-2.5f,
					0f,
					0.5f,
					0.5f,
					0f,
					0.5f
				};
			}
			return new JArray(array2);
		}

		// Token: 0x040002E0 RID: 736
		[Nullable(2)]
		protected WorldInteraction AltForInteractions;

		// Token: 0x040002E1 RID: 737
		[Nullable(2)]
		protected WorldInteraction ChangeGripInteraction;

		// Token: 0x040002E2 RID: 738
		[Nullable(2)]
		protected WorldInteraction ModesSelectionInteraction;

		// Token: 0x040002E3 RID: 739
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;
	}
}
