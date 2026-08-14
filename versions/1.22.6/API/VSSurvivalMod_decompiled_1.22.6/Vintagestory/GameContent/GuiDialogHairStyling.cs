using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Vintagestory.GameContent;

public class GuiDialogHairStyling : GuiDialogCreateCharacter
{
	public Dictionary<string, int> hairStylingCost;

	private Dictionary<string, string> currentSkin = new Dictionary<string, string>();

	private GuiComposer chcomposer;

	private long entityId;

	protected override bool AllowClassSelection => false;

	protected override bool AllowKeepCurrent => true;

	protected override bool AllowedSkinPartSelection(string code)
	{
		switch (code)
		{
		default:
			return code == "beard";
		case "hairbase":
		case "hairextra":
		case "mustache":
			return true;
		}
	}

	public GuiDialogHairStyling(ICoreClientAPI capi, long entityId, string[] categorycodes, Dictionary<string, int> dictionary)
		: base(capi, null)
	{
		variantCategories = categorycodes;
		hairStylingCost = dictionary;
		this.entityId = entityId;
		currentSkin = getCurrentSkin();
		onBeforeCompose = delegate(GuiComposer composer)
		{
			chcomposer = composer;
			ElementBounds bounds = ElementBounds.Fixed(0, dlgHeight - 25).WithAlignment(EnumDialogArea.LeftFixed).WithFixedPadding(12.0, 6.0);
			ElementBounds bounds2 = ElementBounds.Fixed(0.0, dlgHeight - 55, 130.0, 30.0).WithAlignment(EnumDialogArea.RightFixed);
			composer.AddSmallButton(Lang.Get("Cancel"), TryClose, bounds);
			composer.AddRichtext("Cost: 0 gears", CairoFont.WhiteSmallText(), bounds2, "costline");
		};
	}

	private Dictionary<string, string> getCurrentSkin()
	{
		Dictionary<string, string> currentSkin = new Dictionary<string, string>();
		EntityBehaviorExtraSkinnable skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		SkinnablePart[] availableSkinParts = skinMod.AvailableSkinParts;
		foreach (SkinnablePart skinpart in availableSkinParts)
		{
			if (AllowedSkinPartSelection(skinpart.Code))
			{
				string code = skinpart.Code;
				AppliedSkinnablePartVariant appliedVar = skinMod.AppliedSkinParts.FirstOrDefault((AppliedSkinnablePartVariant sp) => sp.PartCode == code);
				currentSkin[code] = appliedVar.Code;
			}
		}
		return currentSkin;
	}

	protected override bool OnNext()
	{
		int money = InventoryTrader.GetPlayerAssets(capi.World.Player.Entity);
		if (getCost() > money)
		{
			capi.TriggerIngameError(this, "notenoughmoney", Lang.Get("Not enough money"));
			return false;
		}
		capi.Network.GetChannel("hairstyling").SendPacket(new PacketHairStyle
		{
			HairstylingNpcEntityId = entityId,
			Hairstyle = getCurrentSkin()
		});
		didSelect = true;
		TryClose();
		capi.Gui.PlaySound(new AssetLocation("sounds/effect/cashregister"), randomizePitch: false, 0.25f);
		return true;
	}

	public override void OnGuiClosed()
	{
		if (didSelect)
		{
			return;
		}
		EntityBehaviorExtraSkinnable skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		foreach (KeyValuePair<string, string> val in currentSkin)
		{
			skinMod.selectSkinPart(val.Key, val.Value);
		}
	}

	protected override void onToggleSkinPart(string partCode, string variantCode)
	{
		base.onToggleSkinPart(partCode, variantCode);
		chcomposer.GetRichtext("costline").SetNewText(Lang.Get("cost-gear-amount", getCost()), CairoFont.WhiteSmallText());
	}

	protected override void onToggleSkinPart(string partCode, int index)
	{
		base.onToggleSkinPart(partCode, index);
		chcomposer.GetRichtext("costline").SetNewText(Lang.Get("cost-gear-amount", getCost()), CairoFont.WhiteSmallText());
	}

	public int getCost()
	{
		int cost = 0;
		EntityBehaviorExtraSkinnable skinMod = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		SkinnablePart[] availableSkinParts = skinMod.AvailableSkinParts;
		foreach (SkinnablePart skinpart in availableSkinParts)
		{
			if (AllowedSkinPartSelection(skinpart.Code))
			{
				string code = skinpart.Code;
				AppliedSkinnablePartVariant appliedVar = skinMod.AppliedSkinParts.FirstOrDefault((AppliedSkinnablePartVariant sp) => sp.PartCode == code);
				if (currentSkin[code] != appliedVar.Code)
				{
					cost += hairStylingCost[code];
				}
			}
		}
		return cost;
	}
}
