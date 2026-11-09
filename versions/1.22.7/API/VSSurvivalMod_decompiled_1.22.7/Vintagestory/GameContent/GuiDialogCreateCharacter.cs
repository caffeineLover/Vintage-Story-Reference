using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class GuiDialogCreateCharacter : GuiDialog
{
	protected bool didSelect;

	protected IInventory characterInv;

	protected ElementBounds insetSlotBounds;

	protected Action<GuiComposer> onBeforeCompose;

	private CharacterSystem modSys;

	private int currentClassIndex;

	private int curTab;

	private int rows = 7;

	private float charZoom = 1f;

	public bool charNaked = true;

	protected int dlgHeight = 513;

	public string[] variantCategories = new string[1] { "standard" };

	protected float yaw = -1.2707963f;

	protected bool rotateCharacter;

	private Vec4f lighPos = new Vec4f(-1f, -1f, 0f, 0f).NormalizeXYZ();

	private Matrixf mat = new Matrixf();

	protected virtual bool AllowClassSelection => true;

	protected virtual bool AllowKeepCurrent => false;

	public override string ToggleKeyCombinationCode => null;

	public override bool PrefersUngrabbedMouse => true;

	public override float ZSize => (float)GuiElement.scaled(280.0);

	public GuiDialogCreateCharacter(ICoreClientAPI capi, CharacterSystem modSys)
		: base(capi)
	{
		this.modSys = modSys;
	}

	protected virtual bool AllowedSkinPartSelection(string code)
	{
		return true;
	}

	protected void ComposeGuis()
	{
		double pad = GuiElementItemSlotGridBase.unscaledSlotPadding;
		double slotsize = GuiElementPassiveItemSlot.unscaledSlotSize;
		characterInv = capi.World.Player.InventoryManager.GetOwnInventory("character");
		ElementBounds tabBounds = ElementBounds.Fixed(0.0, -25.0, 450.0, 25.0);
		double ypos = 20.0 + pad;
		ElementBounds bgBounds = ElementBounds.FixedSize(717.0, dlgHeight).WithFixedPadding(GuiStyle.ElementToDialogPadding);
		ElementBounds dialogBounds = ElementBounds.FixedSize(757.0, dlgHeight + 40).WithAlignment(EnumDialogArea.CenterMiddle).WithFixedAlignmentOffset(GuiStyle.DialogToScreenPadding, 0.0);
		GuiTab[] tabs = new GuiTab[2]
		{
			new GuiTab
			{
				Name = Lang.Get("tab-skinandvoice"),
				DataInt = 0
			},
			new GuiTab
			{
				Name = Lang.Get("tab-charclass"),
				DataInt = 1
			}
		};
		GuiComposer createCharacterComposer = (Composers["createcharacter"] = capi.Gui.CreateCompo("createcharacter", dialogBounds).AddShadedDialogBG(bgBounds).AddDialogTitleBar((curTab == 0) ? Lang.Get("Customize Skin") : ((curTab == 1) ? Lang.Get("Select character class") : Lang.Get("Select your outfit")), OnTitleBarClose)
			.AddIf(AllowClassSelection)
			.AddHorizontalTabs(tabs, tabBounds, onTabClicked, CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold), CairoFont.WhiteSmallText().WithWeight(FontWeight.Bold), "tabs")
			.EndIf()
			.BeginChildElements(bgBounds));
		EntityBehaviorPlayerInventory bh = capi.World.Player.Entity.GetBehavior<EntityBehaviorPlayerInventory>();
		bh.hideClothing = false;
		if (curTab == 0)
		{
			EntityBehaviorExtraSkinnable? behavior = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
			bh.hideClothing = charNaked;
			(capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer).TesselateShape();
			CairoFont smallfont = CairoFont.WhiteSmallText();
			TextExtents textExt = smallfont.GetTextExtents(Lang.Get("Show dressed"));
			int colorIconSize = 22;
			ElementBounds leftColBounds = ElementBounds.Fixed(0.0, ypos, 204.0, dlgHeight - 59).FixedGrow(2.0 * pad, 2.0 * pad);
			insetSlotBounds = ElementBounds.Fixed(0.0, ypos + 2.0, 265.0, leftColBounds.fixedHeight - 2.0 * pad - 10.0).FixedRightOf(leftColBounds, 10.0);
			ElementBounds.Fixed(0.0, ypos, 54.0, dlgHeight - 59).FixedGrow(2.0 * pad, 2.0 * pad).FixedRightOf(insetSlotBounds, 10.0);
			ElementBounds toggleButtonBounds = ElementBounds.Fixed((double)(int)insetSlotBounds.fixedX + insetSlotBounds.fixedWidth / 2.0 - textExt.Width / (double)RuntimeEnv.GUIScale / 2.0 - 12.0, 0.0, textExt.Width / (double)RuntimeEnv.GUIScale + 1.0, textExt.Height / (double)RuntimeEnv.GUIScale).FixedUnder(insetSlotBounds, 4.0).WithAlignment(EnumDialogArea.LeftFixed)
				.WithFixedPadding(12.0, 6.0);
			ElementBounds bounds = null;
			ElementBounds prevbounds = null;
			double leftX = 0.0;
			IReadOnlyList<AppliedSkinnablePartVariant> appliedSkinParts = behavior.AppliedSkinParts;
			SkinnablePart[] availableSkinParts = behavior.AvailableSkinParts;
			foreach (SkinnablePart skinpart in availableSkinParts)
			{
				if (skinpart.Hidden)
				{
					continue;
				}
				bounds = ElementBounds.Fixed(leftX, (prevbounds == null || prevbounds.fixedY == 0.0) ? (-10.0) : (prevbounds.fixedY + 8.0), colorIconSize, colorIconSize);
				if (!AllowedSkinPartSelection(skinpart.Code))
				{
					continue;
				}
				string code = skinpart.Code;
				AppliedSkinnablePartVariant appliedVar = appliedSkinParts.FirstOrDefault((AppliedSkinnablePartVariant sp) => sp.PartCode == code);
				SkinnablePartVariant[] variants = skinpart.Variants.Where((SkinnablePartVariant p) => variantCategories.Contains(p.Category) || (AllowKeepCurrent && p.Code == appliedVar.Code)).ToArray();
				if (skinpart.Type == EnumSkinnableType.Texture && !skinpart.UseDropDown)
				{
					int[] colors = variants.Select((SkinnablePartVariant p) => p.Color).ToArray();
					int selectedIndex = 0;
					createCharacterComposer.AddRichtext(Lang.Get("skinpart-" + code), CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0.0, 10.0).WithFixedSize(210.0, 22.0));
					createCharacterComposer.AddColorListPicker(colors, delegate(int index)
					{
						onToggleSkinPart(code, index);
					}, bounds = bounds.BelowCopy().WithFixedSize(colorIconSize, colorIconSize), 180, "picker-" + code);
					for (int i2 = 0; i2 < variants.Length; i2++)
					{
						if (variants[i2].Code == appliedVar?.Code)
						{
							selectedIndex = i2;
						}
						GuiElementColorListPicker colorListPicker = createCharacterComposer.GetColorListPicker("picker-" + code + "-" + i2);
						colorListPicker.ShowToolTip = true;
						colorListPicker.TooltipText = Lang.Get("color-" + variants[i2].Code);
					}
					createCharacterComposer.ColorListPickerSetValue("picker-" + code, selectedIndex);
				}
				else
				{
					int selectedIndex2 = Math.Max(0, variants.IndexOf((SkinnablePartVariant v) => v.Code == appliedVar?.Code));
					string[] names = variants.Select((SkinnablePartVariant v) => Lang.Get("skinpart-" + code + "-" + v.Code)).ToArray();
					string[] values = variants.Select((SkinnablePartVariant v) => v.Code).ToArray();
					createCharacterComposer.AddRichtext(Lang.Get("skinpart-" + code), CairoFont.WhiteSmallText(), bounds = bounds.BelowCopy(0.0, 10.0).WithFixedSize(210.0, 22.0));
					string tooltip = Lang.GetIfExists("skinpartdesc-" + code);
					if (tooltip != null)
					{
						createCharacterComposer.AddHoverText(tooltip, CairoFont.WhiteSmallText(), 300, bounds = bounds.FlatCopy());
					}
					createCharacterComposer.AddDropDown(values, names, selectedIndex2, delegate(string variantcode, bool selected)
					{
						onToggleSkinPart(code, variantcode);
					}, bounds = bounds.BelowCopy().WithFixedSize(200.0, 25.0), "dropdown-" + code);
				}
				prevbounds = bounds.FlatCopy();
				if (skinpart.Colbreak)
				{
					leftX = insetSlotBounds.fixedX + insetSlotBounds.fixedWidth + 22.0;
					prevbounds.fixedY = 0.0;
				}
			}
			createCharacterComposer.AddInset(insetSlotBounds, 2).AddToggleButton(Lang.Get("Show dressed"), smallfont, OnToggleDressOnOff, toggleButtonBounds, "showdressedtoggle").AddIf(modSys != null)
				.AddButton(Lang.Get("Randomize"), () => OnRandomizeSkin(new Dictionary<string, string>()), ElementBounds.Fixed(0, dlgHeight - 25).WithAlignment(EnumDialogArea.LeftFixed).WithFixedPadding(8.0, 6.0), CairoFont.WhiteSmallText(), EnumButtonStyle.Small)
				.EndIf()
				.AddIf(modSys != null && capi.Settings.String.Exists("lastSkinSelection"))
				.AddButton(Lang.Get("Last selection"), () => OnRandomizeSkin(modSys.getPreviousSelection()), ElementBounds.Fixed(130, dlgHeight - 25).WithAlignment(EnumDialogArea.LeftFixed).WithFixedPadding(8.0, 6.0), CairoFont.WhiteSmallText(), EnumButtonStyle.Small)
				.EndIf()
				.AddSmallButton(Lang.Get("Confirm Skin"), OnNext, ElementBounds.Fixed(0, dlgHeight - 25).WithAlignment(EnumDialogArea.RightFixed).WithFixedPadding(12.0, 6.0));
			createCharacterComposer.GetToggleButton("showdressedtoggle").SetValue(!charNaked);
			onBeforeCompose?.Invoke(createCharacterComposer);
		}
		if (curTab == 1)
		{
			(capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer).TesselateShape();
			ypos -= 10.0;
			ElementBounds leftColBounds2 = ElementBounds.Fixed(0.0, ypos, 0.0, dlgHeight - 47).FixedGrow(2.0 * pad, 2.0 * pad);
			insetSlotBounds = ElementBounds.Fixed(0.0, ypos + 25.0, 190.0, leftColBounds2.fixedHeight - 2.0 * pad + 10.0).FixedRightOf(leftColBounds2, 10.0);
			ElementStdBounds.SlotGrid(EnumDialogArea.None, 0.0, ypos, 1, rows).FixedGrow(2.0 * pad, 2.0 * pad).FixedRightOf(insetSlotBounds, 10.0);
			ElementBounds prevButtonBounds = ElementBounds.Fixed(0.0, ypos + 25.0, 35.0, slotsize - 4.0).WithFixedPadding(2.0).FixedRightOf(insetSlotBounds, 20.0);
			ElementBounds centerTextBounds = ElementBounds.Fixed(0.0, ypos + 25.0, 200.0, slotsize - 4.0 - 8.0).FixedRightOf(prevButtonBounds, 20.0);
			ElementBounds charclasssInset = centerTextBounds.ForkBoundingParent(4.0, 4.0, 4.0, 4.0);
			ElementBounds nextButtonBounds = ElementBounds.Fixed(0.0, ypos + 25.0, 35.0, slotsize - 4.0).WithFixedPadding(2.0).FixedRightOf(charclasssInset, 20.0);
			CairoFont font = CairoFont.WhiteMediumText();
			centerTextBounds.fixedY += (centerTextBounds.fixedHeight - font.GetFontExtents().Height / (double)RuntimeEnv.GUIScale) / 2.0;
			ElementBounds charTextBounds = ElementBounds.Fixed(0.0, 0.0, 480.0, 100.0).FixedUnder(prevButtonBounds, 20.0).FixedRightOf(insetSlotBounds, 20.0);
			createCharacterComposer.AddInset(insetSlotBounds, 2).AddIconButton("left", delegate
			{
				changeClass(-1);
			}, prevButtonBounds.FlatCopy()).AddInset(charclasssInset, 2)
				.AddDynamicText("Commoner", font.Clone().WithOrientation(EnumTextOrientation.Center), centerTextBounds, "className")
				.AddIconButton("right", delegate
				{
					changeClass(1);
				}, nextButtonBounds.FlatCopy())
				.AddRichtext("", CairoFont.WhiteDetailText(), charTextBounds, "characterDesc")
				.AddSmallButton(Lang.Get("Confirm Class"), OnConfirm, ElementBounds.Fixed(0, dlgHeight - 25).WithAlignment(EnumDialogArea.RightFixed).WithFixedPadding(12.0, 6.0));
			changeClass(0);
		}
		GuiElementHorizontalTabs tabElem = createCharacterComposer.GetHorizontalTabs("tabs");
		if (tabElem != null)
		{
			tabElem.unscaledTabSpacing = 20.0;
			tabElem.unscaledTabPadding = 10.0;
			tabElem.activeElement = curTab;
		}
		createCharacterComposer.Compose();
	}

	private bool OnRandomizeSkin(Dictionary<string, string> preselection)
	{
		EntityPlayer entity = capi.World.Player.Entity;
		EntityBehaviorPlayerInventory bh = capi.World.Player.Entity.GetBehavior<EntityBehaviorPlayerInventory>();
		bh.doReloadShapeAndSkin = false;
		modSys.randomizeSkin(entity, preselection);
		EntityBehaviorExtraSkinnable skinMod = entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		foreach (AppliedSkinnablePartVariant appliedPart in skinMod.AppliedSkinParts)
		{
			string partcode = appliedPart.PartCode;
			SkinnablePart skinPart = skinMod.AvailableSkinParts.FirstOrDefault((SkinnablePart part) => part.Code == partcode);
			int index = skinPart.Variants.IndexOf((SkinnablePartVariant part) => part.Code == appliedPart.Code);
			if (skinPart.Type == EnumSkinnableType.Texture && !skinPart.UseDropDown)
			{
				Composers["createcharacter"]?.ColorListPickerSetValue("picker-" + partcode, index);
			}
			else
			{
				Composers["createcharacter"]?.GetDropDown("dropdown-" + partcode)?.SetSelectedIndex(index);
			}
		}
		bh.doReloadShapeAndSkin = true;
		reTesselate();
		return true;
	}

	private void OnToggleDressOnOff(bool on)
	{
		charNaked = !on;
		capi.World.Player.Entity.GetBehavior<EntityBehaviorPlayerInventory>().hideClothing = charNaked;
		if (modSys != null)
		{
			string charclass = capi.World.Player.Entity.WatchedAttributes.GetString("characterClass", modSys.characterClasses[0].Code);
			modSys.setCharacterClass(capi.World.Player.Entity, charclass);
		}
		reTesselate();
	}

	protected virtual void onToggleSkinPart(string partCode, string variantCode)
	{
		capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>().selectSkinPart(partCode, variantCode);
	}

	protected virtual void onToggleSkinPart(string partCode, int index)
	{
		EntityBehaviorExtraSkinnable? behavior = capi.World.Player.Entity.GetBehavior<EntityBehaviorExtraSkinnable>();
		string variantCode = behavior.AvailableSkinPartsByCode[partCode].Variants[index].Code;
		behavior.selectSkinPart(partCode, variantCode);
	}

	protected virtual bool OnNext()
	{
		if (AllowClassSelection)
		{
			curTab = 1;
			ComposeGuis();
		}
		else
		{
			didSelect = true;
			TryClose();
		}
		return true;
	}

	private void onTabClicked(int tabid)
	{
		curTab = tabid;
		ComposeGuis();
	}

	public override void OnGuiOpened()
	{
		string charclass = capi.World.Player.Entity.WatchedAttributes.GetString("characterClass");
		if (AllowClassSelection)
		{
			if (charclass != null)
			{
				modSys.setCharacterClass(capi.World.Player.Entity, charclass);
			}
			else
			{
				modSys.setCharacterClass(capi.World.Player.Entity, modSys.characterClasses[0].Code);
			}
		}
		ComposeGuis();
		(capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer).TesselateShape();
		if ((capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Guest || capi.World.Player.WorldData.CurrentGameMode == EnumGameMode.Survival) && characterInv != null)
		{
			characterInv.Open(capi.World.Player);
		}
	}

	public override void OnGuiClosed()
	{
		if (characterInv != null)
		{
			characterInv.Close(capi.World.Player);
			Composers["createcharacter"].GetSlotGrid("leftSlots")?.OnGuiClosed(capi);
			Composers["createcharacter"].GetSlotGrid("rightSlots")?.OnGuiClosed(capi);
		}
		if (modSys != null)
		{
			CharacterClass chclass = modSys.characterClasses[currentClassIndex];
			modSys.ClientSelectionDone(characterInv, chclass.Code, didSelect);
		}
		capi.World.Player.Entity.GetBehavior<EntityBehaviorPlayerInventory>().hideClothing = false;
		reTesselate();
	}

	private bool OnConfirm()
	{
		didSelect = true;
		TryClose();
		return true;
	}

	protected virtual void OnTitleBarClose()
	{
		TryClose();
	}

	protected void SendInvPacket(object packet)
	{
		capi.Network.SendPacketClient(packet);
	}

	private void changeClass(int dir)
	{
		currentClassIndex = GameMath.Mod(currentClassIndex + dir, modSys.characterClasses.Count);
		CharacterClass chclass = modSys.characterClasses[currentClassIndex];
		Composers["createcharacter"].GetDynamicText("className").SetNewText(Lang.Get("characterclass-" + chclass.Code));
		StringBuilder fulldesc = new StringBuilder();
		StringBuilder attributes = new StringBuilder();
		fulldesc.AppendLine(Lang.Get("characterdesc-" + chclass.Code));
		fulldesc.AppendLine();
		fulldesc.AppendLine(Lang.Get("traits-title"));
		foreach (Trait trait in from code in chclass.Traits
			select modSys.TraitsByCode[code] into trait2
			orderby (int)trait2.Type
			select trait2)
		{
			attributes.Clear();
			foreach (KeyValuePair<string, double> val in trait.Attributes)
			{
				if (attributes.Length > 0)
				{
					attributes.Append(", ");
				}
				attributes.Append(Lang.Get(string.Format(GlobalConstants.DefaultCultureInfo, "charattribute-{0}-{1}", val.Key, val.Value)));
			}
			if (attributes.Length > 0)
			{
				fulldesc.AppendLine(Lang.Get("traitwithattributes", Lang.Get("trait-" + trait.Code), attributes));
				continue;
			}
			string desc = Lang.GetIfExists("traitdesc-" + trait.Code);
			if (desc != null)
			{
				fulldesc.AppendLine(Lang.Get("traitwithattributes", Lang.Get("trait-" + trait.Code), desc));
			}
			else
			{
				fulldesc.AppendLine(Lang.Get("trait-" + trait.Code));
			}
		}
		if (chclass.Traits.Length == 0)
		{
			fulldesc.AppendLine(Lang.Get("No positive or negative traits"));
		}
		Composers["createcharacter"].GetRichtext("characterDesc").SetNewText(fulldesc.ToString(), CairoFont.WhiteDetailText());
		modSys.setCharacterClass(capi.World.Player.Entity, chclass.Code);
		reTesselate();
	}

	protected void reTesselate()
	{
		(capi.World.Player.Entity.Properties.Client.Renderer as EntityShapeRenderer).TesselateShape();
	}

	public void PrepAndOpen()
	{
		TryOpen();
	}

	public override bool CaptureAllInputs()
	{
		return IsOpened();
	}

	public override void OnMouseWheel(MouseWheelEventArgs args)
	{
		base.OnMouseWheel(args);
		if (insetSlotBounds.PointInside(capi.Input.MouseX, capi.Input.MouseY) && curTab == 0)
		{
			charZoom = GameMath.Clamp(charZoom + args.deltaPrecise / 5f, 0.5f, 1f);
		}
	}

	public override void OnMouseDown(MouseEvent args)
	{
		base.OnMouseDown(args);
		rotateCharacter = insetSlotBounds.PointInside(args.X, args.Y);
	}

	public override void OnMouseUp(MouseEvent args)
	{
		base.OnMouseUp(args);
		rotateCharacter = false;
	}

	public override void OnMouseMove(MouseEvent args)
	{
		base.OnMouseMove(args);
		if (rotateCharacter)
		{
			yaw -= (float)args.DeltaX / 100f;
		}
	}

	public override void OnRenderGUI(float deltaTime)
	{
		base.OnRenderGUI(deltaTime);
		if (capi.IsGamePaused)
		{
			capi.World.Player.Entity.talkUtil.OnGameTick(deltaTime);
		}
		capi.Render.GlPushMatrix();
		if (focused)
		{
			capi.Render.GlTranslate(0f, 0f, 150f);
		}
		capi.Render.GlRotate(-14f, 1f, 0f, 0f);
		mat.Identity();
		mat.RotateXDeg(-14f);
		Vec4f lightRot = mat.TransformVector(lighPos);
		double pad = GuiElement.scaled(GuiElementItemSlotGridBase.unscaledSlotPadding);
		capi.Render.CurrentActiveShader.Uniform("lightPosition", lightRot.X, lightRot.Y, lightRot.Z);
		capi.Render.PushScissor(insetSlotBounds);
		double posX = insetSlotBounds.renderX + pad - GuiElement.scaled(195.0) * (double)charZoom + GuiElement.scaled(115f * (1f - charZoom));
		double posY = insetSlotBounds.renderY + pad + GuiElement.scaled(10f * (1f - charZoom));
		double posZ = (float)GuiElement.scaled(230.0);
		float size = (float)GuiElement.scaled(330f * charZoom);
		if (curTab == 1)
		{
			posX = insetSlotBounds.renderX + pad - GuiElement.scaled(110.0);
			posY = insetSlotBounds.renderY + pad - GuiElement.scaled(15.0);
			size = (float)GuiElement.scaled(205.0);
		}
		capi.Render.RenderEntityToGui(deltaTime, capi.World.Player.Entity, posX, posY, posZ, yaw, size, -1);
		capi.Render.PopScissor();
		capi.Render.CurrentActiveShader.Uniform("lightPosition", 0.7071068f, -0.7071068f, 0f);
		capi.Render.GlPopMatrix();
	}
}
