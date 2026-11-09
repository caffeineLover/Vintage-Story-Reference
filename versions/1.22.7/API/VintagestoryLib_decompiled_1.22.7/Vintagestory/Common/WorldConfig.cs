using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.Common;

public class WorldConfig
{
	public List<ModContainer> mods;

	protected List<PlayStyle> playstyles;

	protected string playstylecode;

	protected JsonObject jworldconfig;

	protected Dictionary<string, WorldConfigurationValue> worldConfigsPlaystyle = new Dictionary<string, WorldConfigurationValue>();

	protected Dictionary<string, WorldConfigurationValue> worldConfigsCustom = new Dictionary<string, WorldConfigurationValue>();

	public int MapsizeY = 256;

	public string Seed;

	public bool IsNewWorld;

	public List<PlayStyle> PlayStyles => playstyles;

	public PlayStyle CurrentPlayStyle => playstyles.FirstOrDefault((PlayStyle p) => p.Code == playstylecode);

	public int CurrentPlayStyleIndex => playstyles.IndexOf(CurrentPlayStyle);

	public Dictionary<string, WorldConfigurationValue> WorldConfigsPlaystyle => worldConfigsPlaystyle;

	public Dictionary<string, WorldConfigurationValue> WorldConfigsCustom => worldConfigsCustom;

	public JsonObject Jworldconfig => jworldconfig;

	public WorldConfigurationValue this[string code]
	{
		get
		{
			if (worldConfigsCustom.TryGetValue(code, out var val))
			{
				return val;
			}
			worldConfigsPlaystyle.TryGetValue(code, out val);
			return val;
		}
	}

	internal void loadFromSavegame(SaveGame savegame)
	{
		if (savegame != null)
		{
			Seed = savegame.Seed.ToString();
			MapsizeY = savegame.MapSizeY;
			selectPlayStyle(savegame.PlayStyle);
			loadWorldConfigValues(new JsonObject(JToken.Parse(savegame.WorldConfiguration.ToJsonToken())), WorldConfigsCustom);
			updateJWorldConfig();
		}
	}

	public WorldConfig(List<ModContainer> mods)
	{
		this.mods = mods;
		LoadPlayStyles();
	}

	public void LoadPlayStyles()
	{
		playstyles = new List<PlayStyle>();
		foreach (ModContainer mod in mods)
		{
			if (mod.Error.HasValue || !mod.Enabled || mod.WorldConfig?.PlayStyles == null)
			{
				continue;
			}
			PlayStyle[] playStyles = mod.WorldConfig.PlayStyles;
			foreach (PlayStyle playstyle in playStyles)
			{
				if (playstyles.Find((PlayStyle sAttr) => sAttr.Code == playstyle.Code) == null)
				{
					playstyles.Add(playstyle);
				}
			}
		}
		playstyles = playstyles.OrderBy((PlayStyle p) => p.ListOrder).ToList();
		if (playstyles.Count == 0)
		{
			playstyles.Add(new PlayStyle
			{
				Code = "default",
				LangCode = "default",
				WorldConfig = new JsonObject(JObject.Parse("{}"))
			});
		}
	}

	public void selectPlayStyle(int index)
	{
		playstylecode = playstyles[index].Code;
		loadWorldConfigValuesFromPlaystyle();
	}

	public void selectPlayStyle(string playstylecode)
	{
		this.playstylecode = playstylecode;
		loadWorldConfigValuesFromPlaystyle();
	}

	private void loadWorldConfigValuesFromPlaystyle()
	{
		if (playstylecode != null)
		{
			PlayStyle playstyle = CurrentPlayStyle;
			jworldconfig = playstyle.WorldConfig.Clone();
			loadWorldConfigValues(jworldconfig, worldConfigsPlaystyle);
			updateJWorldConfig();
		}
	}

	public void loadWorldConfigValues(JsonObject jworldconfig, Dictionary<string, WorldConfigurationValue> intoDict)
	{
		intoDict.Clear();
		foreach (ModContainer mod in mods)
		{
			ModWorldConfiguration config = mod.WorldConfig;
			if (config == null)
			{
				continue;
			}
			WorldConfigurationAttribute[] worldConfigAttributes = config.WorldConfigAttributes;
			foreach (WorldConfigurationAttribute attribute in worldConfigAttributes)
			{
				WorldConfigurationValue value = new WorldConfigurationValue();
				value.Attribute = attribute;
				value.Code = attribute.Code;
				JsonObject valueObject = jworldconfig[value.Code];
				if (valueObject.Exists)
				{
					switch (value.Attribute.DataType)
					{
					case EnumDataType.Bool:
						value.Value = valueObject.AsBool((bool)value.Attribute.TypedDefault);
						break;
					case EnumDataType.DoubleInput:
					case EnumDataType.DoubleRange:
						value.Value = valueObject.AsDouble((double)value.Attribute.TypedDefault);
						break;
					case EnumDataType.String:
					case EnumDataType.DropDown:
					case EnumDataType.StringRange:
						value.Value = valueObject.AsString((string)value.Attribute.TypedDefault);
						break;
					case EnumDataType.IntInput:
					case EnumDataType.IntRange:
						value.Value = valueObject.AsInt((int)value.Attribute.TypedDefault);
						break;
					}
					intoDict[value.Code] = value;
				}
			}
		}
	}

	public void updateJWorldConfig()
	{
		if (CurrentPlayStyle != null)
		{
			jworldconfig = allDefaultValues(mods);
			updateJWorldConfigFrom(worldConfigsPlaystyle);
			updateJWorldConfigFrom(worldConfigsCustom);
		}
	}

	public static JsonObject allDefaultValues(List<ModContainer> mods)
	{
		JToken token = JToken.Parse("{}");
		JObject obj = token as JObject;
		foreach (ModContainer mod in mods)
		{
			ModWorldConfiguration config = mod.WorldConfig;
			if (config == null)
			{
				continue;
			}
			WorldConfigurationAttribute[] worldConfigAttributes = config.WorldConfigAttributes;
			foreach (WorldConfigurationAttribute attribute in worldConfigAttributes)
			{
				switch (attribute.DataType)
				{
				case EnumDataType.Bool:
					obj[attribute.Code] = (bool)attribute.TypedDefault;
					break;
				case EnumDataType.DoubleInput:
				case EnumDataType.DoubleRange:
					obj[attribute.Code] = (double)attribute.TypedDefault;
					break;
				case EnumDataType.IntInput:
				case EnumDataType.IntRange:
					obj[attribute.Code] = (int)attribute.TypedDefault;
					break;
				case EnumDataType.String:
				case EnumDataType.DropDown:
				case EnumDataType.StringRange:
					obj[attribute.Code] = (string)attribute.TypedDefault;
					break;
				}
			}
		}
		return new JsonObject(token);
	}

	public void updateJWorldConfigFrom(Dictionary<string, WorldConfigurationValue> dict)
	{
		JObject obj = jworldconfig.Token as JObject;
		foreach (KeyValuePair<string, WorldConfigurationValue> pair in dict)
		{
			object value = pair.Value.Value;
			switch (pair.Value.Attribute.DataType)
			{
			case EnumDataType.Bool:
				obj[pair.Key] = (bool)value;
				break;
			case EnumDataType.DoubleInput:
			case EnumDataType.DoubleRange:
				obj[pair.Key] = (double)value;
				break;
			case EnumDataType.IntInput:
			case EnumDataType.IntRange:
				obj[pair.Key] = (int)value;
				break;
			case EnumDataType.String:
			case EnumDataType.DropDown:
			case EnumDataType.StringRange:
				obj[pair.Key] = (string)value;
				break;
			}
		}
	}

	public void ApplyConfigs(List<GuiElement> inputElements)
	{
		int i = 0;
		worldConfigsCustom = new Dictionary<string, WorldConfigurationValue>();
		foreach (ModContainer mod in mods)
		{
			ModWorldConfiguration config = mod.WorldConfig;
			if (config == null)
			{
				continue;
			}
			WorldConfigurationAttribute[] worldConfigAttributes = config.WorldConfigAttributes;
			foreach (WorldConfigurationAttribute attribute in worldConfigAttributes)
			{
				if (attribute.OnCustomizeScreen)
				{
					GuiElement elem = inputElements[i];
					WorldConfigurationValue value = new WorldConfigurationValue();
					value.Attribute = attribute;
					value.Code = attribute.Code;
					switch (attribute.DataType)
					{
					case EnumDataType.Bool:
					{
						GuiElementSwitch switchElem = elem as GuiElementSwitch;
						value.Value = switchElem.On;
						break;
					}
					case EnumDataType.DoubleInput:
					{
						GuiElementNumberInput numInput2 = elem as GuiElementNumberInput;
						value.Value = numInput2.GetText().ToDouble();
						break;
					}
					case EnumDataType.DoubleRange:
					{
						GuiElementSlider slider3 = elem as GuiElementSlider;
						value.Value = (double)((decimal)slider3.GetValue() / attribute.Multiplier);
						break;
					}
					case EnumDataType.IntInput:
					{
						GuiElementNumberInput numInput = elem as GuiElementNumberInput;
						value.Value = numInput.GetText().ToInt();
						break;
					}
					case EnumDataType.IntRange:
					{
						GuiElementSlider slider2 = elem as GuiElementSlider;
						value.Value = slider2.GetValue();
						break;
					}
					case EnumDataType.DropDown:
					{
						GuiElementDropDown dropDown = elem as GuiElementDropDown;
						value.Value = dropDown.SelectedValue;
						break;
					}
					case EnumDataType.String:
					{
						GuiElementTextInput textInput = elem as GuiElementTextInput;
						value.Value = textInput.GetText();
						break;
					}
					case EnumDataType.StringRange:
					{
						GuiElementSlider slider = elem as GuiElementSlider;
						value.Value = attribute.Values[slider.GetValue()];
						break;
					}
					}
					worldConfigsCustom.Add(value.Code, value);
					i++;
				}
			}
		}
	}

	public string ToRichText(bool withCustomConfigs)
	{
		return ToRichText(CurrentPlayStyle, withCustomConfigs);
	}

	public string ToRichText(PlayStyle playstyle, bool withCustomConfigs)
	{
		if (CurrentPlayStyle == null)
		{
			return "";
		}
		JsonObject pworldconfig = playstyle.WorldConfig.Clone();
		if (withCustomConfigs)
		{
			JObject obj = pworldconfig.Token as JObject;
			foreach (KeyValuePair<string, WorldConfigurationValue> pair in worldConfigsCustom)
			{
				object value = pair.Value.Value;
				switch (pair.Value.Attribute.DataType)
				{
				case EnumDataType.Bool:
					obj[pair.Key] = (bool)value;
					break;
				case EnumDataType.DoubleInput:
				case EnumDataType.DoubleRange:
					obj[pair.Key] = (double)value;
					break;
				case EnumDataType.IntInput:
				case EnumDataType.IntRange:
					obj[pair.Key] = (int)value;
					break;
				case EnumDataType.String:
				case EnumDataType.DropDown:
				case EnumDataType.StringRange:
					obj[pair.Key] = (string)value;
					break;
				}
			}
		}
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("<font opacity=\"0.6\">" + Lang.Get("World height:") + "</font> " + MapsizeY);
		if (Seed == null || Seed.Length == 0)
		{
			sb.AppendLine("<font opacity=\"0.6\">" + Lang.Get("Random seed") + "</font> ");
		}
		else
		{
			sb.AppendLine("<font opacity=\"0.6\">" + Lang.Get("Seed: ", Seed) + "</font> " + Seed);
		}
		foreach (ModContainer mod in mods)
		{
			ModWorldConfiguration config = mod.WorldConfig;
			if (config == null)
			{
				continue;
			}
			WorldConfigurationAttribute[] worldConfigAttributes = config.WorldConfigAttributes;
			foreach (WorldConfigurationAttribute attribute in worldConfigAttributes)
			{
				WorldConfigurationValue value2 = new WorldConfigurationValue();
				value2.Attribute = attribute;
				value2.Code = attribute.Code;
				JsonObject valueObject = pworldconfig[value2.Code];
				if (valueObject.Exists && valueObject.Token.ToString() != attribute.Default)
				{
					sb.AppendLine("<font opacity=\"0.6\">" + Lang.Get("worldattribute-" + attribute.Code) + ":</font> " + attribute.valueToHumanReadable(valueObject.Token.ToString()));
				}
			}
		}
		return sb.ToString();
	}

	public string ToJson()
	{
		jworldconfig.Token["playstyle"] = playstylecode;
		jworldconfig.Token["worldHeight"] = MapsizeY;
		return jworldconfig.ToString();
	}

	public void FromJson(string json)
	{
		JsonObject jworldconfig = new JsonObject(JToken.Parse(json));
		try
		{
			playstylecode = jworldconfig?.Token["playstyle"]?.ToString();
			MapsizeY = jworldconfig?.Token["worldHeight"]?.ToString()?.ToInt() ?? MapsizeY;
		}
		catch (Exception)
		{
			return;
		}
		selectPlayStyle(playstylecode);
		loadWorldConfigValues(jworldconfig, WorldConfigsCustom);
		updateJWorldConfig();
	}

	public WorldConfig Clone()
	{
		return new WorldConfig(mods)
		{
			playstylecode = playstylecode,
			jworldconfig = jworldconfig.Clone(),
			worldConfigsPlaystyle = new Dictionary<string, WorldConfigurationValue>(worldConfigsPlaystyle),
			worldConfigsCustom = new Dictionary<string, WorldConfigurationValue>(worldConfigsCustom),
			MapsizeY = MapsizeY,
			Seed = Seed,
			IsNewWorld = IsNewWorld
		};
	}
}
