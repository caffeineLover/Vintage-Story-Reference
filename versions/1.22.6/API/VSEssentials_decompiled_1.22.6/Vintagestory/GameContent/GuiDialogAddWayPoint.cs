using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class GuiDialogAddWayPoint : GuiDialogGeneric
{
	protected const string composerId = "worldmap-addwp";

	protected const string colorPickerId = "colorPicker";

	protected const string iconPickerId = "iconPicker";

	protected const string nameInputId = "nameInput";

	protected const string saveButtonId = "saveButton";

	protected const string autoSuggestSwitchId = "autoSuggestSwitch";

	protected const string pinnedSwitchId = "pinnedSwitch";

	protected const string savedNamesFileName = "waypoints-names.json";

	protected const string nameSuggestionPrefix = "wpSuggestion";

	protected const int colorIconSize = 22;

	protected int[] colors = Array.Empty<int>();

	protected string[] icons = Array.Empty<string>();

	protected string? selectedIcon;

	protected string? selectedColor;

	protected bool autoSuggest = true;

	protected bool ignoreNextAutosuggestDisable;

	protected EnumDialogType dialogType;

	protected bool prefersUngrabbedMouse = true;

	protected bool currentNameWasSuggested;

	protected int lastSuggestedNameLength;

	protected bool ignoreNextOnNameChanged;

	protected bool autoSuggestSavedNames = true;

	public override bool PrefersUngrabbedMouse => prefersUngrabbedMouse = true;

	public override EnumDialogType DialogType => dialogType;

	public override bool DisableMouseGrab => true;

	public Vec3d? WorldPos { get; set; }

	public override double DrawOrder => 0.2;

	public static Dictionary<string, string> SavedNames { get; set; } = new Dictionary<string, string>();

	public GuiDialogAddWayPoint(ICoreClientAPI clientApi, WaypointMapLayer mapLayer)
		: base("", clientApi)
	{
		GetIconsAndColors(mapLayer);
	}

	public override bool CaptureAllInputs()
	{
		return IsOpened();
	}

	public override void OnMouseDown(MouseEvent args)
	{
		base.OnMouseDown(args);
		args.Handled = true;
	}

	public override void OnMouseUp(MouseEvent args)
	{
		base.OnMouseUp(args);
		args.Handled = true;
	}

	public override void OnMouseMove(MouseEvent args)
	{
		base.OnMouseMove(args);
		args.Handled = true;
	}

	public override void OnMouseWheel(MouseWheelEventArgs args)
	{
		base.OnMouseWheel(args);
		args.SetHandled();
	}

	public override bool TryOpen()
	{
		ComposeDialog();
		SetToolTipText();
		return base.TryOpen();
	}

	public static void LoadSavedNames(ILogger logger)
	{
		string path = Path.Combine(GamePaths.Config, "waypoints-names.json");
		if (File.Exists(path))
		{
			Dictionary<string, string> loadedNames = null;
			try
			{
				loadedNames = new JsonObject(JObject.Parse(File.ReadAllText(path))).AsObject<Dictionary<string, string>>();
			}
			catch (Exception value)
			{
				logger.Error($"[GuiDialogAddWayPoint] Error while loading saved file names from file '{"waypoints-names.json"}':\n{value}");
			}
			if (loadedNames == null)
			{
				logger.Error("[GuiDialogAddWayPoint] Error while loading saved file names from file 'waypoints-names.json'");
			}
			else
			{
				SavedNames.AddRange<string, string>(loadedNames);
			}
		}
	}

	public static void StoreSavedNames(ILogger logger)
	{
		FileInfo file = new FileInfo(Path.Combine(GamePaths.Config, "waypoints-names.json"));
		if (file.Directory == null)
		{
			logger.Error("[GuiDialogAddWayPoint] Error while storing saved file names to file 'waypoints-names.json': ");
			return;
		}
		GamePaths.EnsurePathExists(file.Directory.FullName);
		string json = JsonConvert.SerializeObject(SavedNames, Formatting.Indented);
		File.WriteAllText(file.FullName, json);
	}

	protected void GetIconsAndColors(WaypointMapLayer mapLayer)
	{
		icons = mapLayer.WaypointIcons.Keys.ToArray();
		colors = mapLayer.WaypointColors.ToArray();
	}

	protected virtual void ComposeDialog()
	{
		ElementBounds leftColumn = ElementBounds.Fixed(0.0, 28.0, 90.0, 25.0);
		ElementBounds rightColumn = leftColumn.RightCopy();
		ElementBounds buttonRow = ElementBounds.Fixed(0.0, 28.0, 360.0, 25.0);
		ElementBounds backgroundBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
		backgroundBounds.BothSizing = ElementSizing.FitToChildren;
		backgroundBounds.WithChildren(leftColumn, rightColumn);
		ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle).WithFixedAlignmentOffset(0.0 - GuiStyle.DialogToScreenPadding, 0.0);
		base.SingleComposer?.Dispose();
		selectedIcon = icons[0];
		selectedColor = ColorUtil.Int2Hex(colors[0]);
		base.SingleComposer = capi.Gui.CreateCompo("worldmap-addwp", dialogBounds);
		base.SingleComposer.AddShadedDialogBG(backgroundBounds, withTitleBar: false).AddDialogTitleBar(Lang.Get("Add waypoint"), delegate
		{
			TryClose();
		}).BeginChildElements(backgroundBounds)
			.AddStaticText(Lang.Get("Name"), CairoFont.WhiteSmallText(), leftColumn = leftColumn.FlatCopy())
			.AddTextInput(rightColumn = rightColumn.FlatCopy().WithFixedWidth(200.0), OnNameChanged, CairoFont.TextInput(), "nameInput")
			.AddStaticText(Lang.Get("Pinned"), CairoFont.WhiteSmallText(), leftColumn = leftColumn.BelowCopy(0.0, 9.0))
			.AddSwitch(OnPinnedToggled, rightColumn.BelowCopy(0.0, 5.0).WithFixedWidth(200.0), "pinnedSwitch")
			.AddStaticText(Lang.Get("Suggest saved"), CairoFont.WhiteSmallText(), leftColumn.RightCopy(50.0).WithFixedWidth(140.0))
			.AddSwitch(OnAutoSuggestToggled, leftColumn.RightCopy(170.0, -4.0).WithFixedWidth(40.0), "autoSuggestSwitch")
			.AddRichtext(Lang.Get("waypoint-color"), CairoFont.WhiteSmallText(), leftColumn = leftColumn.BelowCopy(0.0, 5.0))
			.AddColorListPicker(colors, OnColorSelected, leftColumn = leftColumn.BelowCopy(0.0, 5.0).WithFixedSize(22.0, 22.0), 270, "colorPicker")
			.AddStaticText(Lang.Get("Icon"), CairoFont.WhiteSmallText(), leftColumn = leftColumn.WithFixedPosition(0.0, leftColumn.fixedY + leftColumn.fixedHeight).WithFixedWidth(200.0).BelowCopy())
			.AddIconListPicker(icons, OnIconSelected, leftColumn = leftColumn.BelowCopy(0.0, 5.0).WithFixedSize(27.0, 27.0), 270, "iconPicker")
			.AddSmallButton(Lang.Get("Cancel"), OnCancel, buttonRow.FlatCopy().FixedUnder(leftColumn).WithFixedWidth(100.0))
			.AddSmallButton(Lang.Get("Save"), OnSave, buttonRow.FlatCopy().FixedUnder(leftColumn).WithFixedWidth(100.0)
				.WithAlignment(EnumDialogArea.RightFixed), EnumButtonStyle.Normal, "saveButton")
			.EndChildElements();
		base.SingleComposer.Compose();
		base.SingleComposer.GetButton("saveButton").Enabled = false;
		base.SingleComposer.GetSwitch("autoSuggestSwitch").SetValue(autoSuggestSavedNames);
		base.SingleComposer.ColorListPickerSetValue("colorPicker", 0);
		base.SingleComposer.IconListPickerSetValue("iconPicker", 0);
	}

	protected virtual void OnIconSelected(int index)
	{
		if (index < icons.Length && index >= 0)
		{
			selectedIcon = icons[index];
		}
		else
		{
			selectedIcon = icons[0];
		}
		AutoSuggestName();
		SetToolTipText();
	}

	protected virtual void OnColorSelected(int index)
	{
		if (index < colors.Length && index >= 0)
		{
			selectedColor = ColorUtil.Int2Hex(colors[index]);
		}
		else
		{
			selectedColor = ColorUtil.Int2Hex(colors[0]);
		}
		AutoSuggestName();
	}

	protected virtual void OnPinnedToggled(bool on)
	{
	}

	protected virtual void OnAutoSuggestToggled(bool on)
	{
		autoSuggestSavedNames = on;
		SetToolTipText();
	}

	protected virtual void AutoSuggestName()
	{
		if (!autoSuggest)
		{
			return;
		}
		GuiElementTextInput nameInputElement = base.SingleComposer.GetTextInput("nameInput");
		ignoreNextAutosuggestDisable = true;
		currentNameWasSuggested = true;
		if (autoSuggestSavedNames)
		{
			string savedName = TryGetSelectedSavedName();
			if (savedName != null)
			{
				lastSuggestedNameLength = savedName.Length;
				nameInputElement.SetValue(savedName);
				return;
			}
		}
		if (Lang.HasTranslation("wpSuggestion-" + selectedIcon + "-" + selectedColor))
		{
			string newName = Lang.Get("wpSuggestion-" + selectedIcon + "-" + selectedColor);
			lastSuggestedNameLength = newName.Length;
			nameInputElement.SetValue(newName);
		}
		else if (Lang.HasTranslation("wpSuggestion-" + selectedIcon))
		{
			string newName2 = Lang.Get("wpSuggestion-" + selectedIcon);
			lastSuggestedNameLength = newName2.Length;
			nameInputElement.SetValue(newName2);
		}
		else
		{
			lastSuggestedNameLength = 0;
			nameInputElement.SetValue("");
		}
		lastSuggestedNameLength = nameInputElement.GetText().Length;
	}

	protected virtual string? TryGetSelectedSavedName()
	{
		if (selectedIcon == null || selectedColor == null)
		{
			return null;
		}
		return TryGetSavedName(selectedIcon, selectedColor);
	}

	protected virtual string? TryGetSavedName(string icon, string color)
	{
		string key = GetSavedWaypointKey(icon, color);
		if (SavedNames.TryGetValue(key, out string name))
		{
			return name;
		}
		return null;
	}

	protected virtual void SaveWaypointName(string name)
	{
		if (selectedIcon != null && selectedColor != null)
		{
			string key = GetSavedWaypointKey(selectedIcon, selectedColor);
			SavedNames[key] = name;
		}
	}

	protected virtual void SetToolTipText()
	{
		if (selectedIcon == null)
		{
			return;
		}
		for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
		{
			GuiElementColorListPicker picker = null;
			try
			{
				picker = base.SingleComposer.GetColorListPicker($"{"colorPicker"}-{colorIndex}");
			}
			catch (Exception value)
			{
				capi.Logger.Error($"[GuiDialogAddWayPoint] Error while setting tooltip text for color picker: {value}");
			}
			if (picker != null)
			{
				string waypointName = TryGetSavedName(selectedIcon, ColorUtil.Int2Hex(colors[colorIndex]));
				if (waypointName != null && autoSuggestSavedNames)
				{
					picker.ShowToolTip = true;
					picker.TooltipText = waypointName;
				}
				else
				{
					picker.ShowToolTip = false;
					picker.TooltipText = "";
				}
			}
		}
	}

	protected virtual string GetSavedWaypointKey(string icon, string color)
	{
		return icon + "-" + color;
	}

	protected virtual bool OnSave()
	{
		if (WorldPos == null)
		{
			return false;
		}
		string name = base.SingleComposer.GetTextInput("nameInput").GetText();
		bool pinned = base.SingleComposer.GetSwitch("pinnedSwitch").On;
		global::_003C_003Ey__InlineArray7<object> buffer = default(global::_003C_003Ey__InlineArray7<object>);
		buffer[0] = selectedIcon;
		buffer[1] = WorldPos.X.ToString(GlobalConstants.DefaultCultureInfo);
		buffer[2] = WorldPos.Y.ToString(GlobalConstants.DefaultCultureInfo);
		buffer[3] = WorldPos.Z.ToString(GlobalConstants.DefaultCultureInfo);
		buffer[4] = pinned;
		buffer[5] = selectedColor;
		buffer[6] = name;
		string command = string.Format("/waypoint addati {0} ={1} ={2} ={3} {4} {5} {6}", (ReadOnlySpan<object?>)buffer);
		capi.SendChatMessage(command);
		TryClose();
		SaveWaypointName(name);
		StoreSavedNames(capi.Logger);
		return true;
	}

	protected virtual bool OnCancel()
	{
		TryClose();
		return true;
	}

	protected virtual void OnNameChanged(string newName)
	{
		if (ignoreNextOnNameChanged)
		{
			ignoreNextOnNameChanged = false;
			return;
		}
		base.SingleComposer.GetButton("saveButton").Enabled = newName.Trim() != "";
		if (!ignoreNextAutosuggestDisable)
		{
			autoSuggest = newName.Length == 0;
			currentNameWasSuggested = false;
		}
		if (lastSuggestedNameLength != 0 && newName.Length != lastSuggestedNameLength)
		{
			GuiElementTextInput textInput = base.SingleComposer.GetTextInput("nameInput");
			lastSuggestedNameLength = 0;
			ignoreNextOnNameChanged = true;
			textInput.SetValue("");
		}
		ignoreNextAutosuggestDisable = false;
	}
}
