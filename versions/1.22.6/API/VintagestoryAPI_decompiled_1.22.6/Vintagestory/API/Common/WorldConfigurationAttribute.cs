using System.Globalization;
using Newtonsoft.Json;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

public class WorldConfigurationAttribute
{
	public EnumDataType DataType;

	public string Category;

	public string Code;

	public double Min;

	public double Max;

	public double Step;

	public double Alarm = 2147483647.0;

	public decimal Multiplier = 1m;

	public decimal DisplayUnit = 1m;

	public bool OnCustomizeScreen = true;

	public string Default;

	public string[] Values;

	public string[] Names;

	public string[] SkipValues;

	public bool OnlyDuringWorldCreate;

	[JsonIgnore]
	public ModInfo ModInfo { get; set; }

	public object TypedDefault => stringToValue(Default);

	public object stringToValue(string text)
	{
		switch (DataType)
		{
		case EnumDataType.Bool:
		{
			bool.TryParse(text, out var on);
			return on;
		}
		case EnumDataType.DoubleInput:
		case EnumDataType.DoubleRange:
		{
			double.TryParse(text, NumberStyles.Float, GlobalConstants.DefaultCultureInfo, out var dval);
			return dval;
		}
		case EnumDataType.IntInput:
		case EnumDataType.IntRange:
		{
			int.TryParse(text, NumberStyles.Integer, GlobalConstants.DefaultCultureInfo, out var val);
			return val;
		}
		case EnumDataType.String:
		case EnumDataType.DropDown:
		case EnumDataType.StringRange:
			return text;
		default:
			return null;
		}
	}

	public string valueToHumanReadable(string value)
	{
		switch (DataType)
		{
		case EnumDataType.Bool:
			if (!(value.ToLowerInvariant() == "true"))
			{
				return Lang.Get("Off");
			}
			return Lang.Get("On");
		case EnumDataType.DropDown:
		case EnumDataType.StringRange:
		{
			int index = Values.IndexOf(value);
			string text;
			if (index < 0)
			{
				text = value;
				if (text == null)
				{
					return "";
				}
			}
			else
			{
				text = Lang.Get("worldconfig-" + Code + "-" + Names[index]);
			}
			return text;
		}
		case EnumDataType.IntInput:
		case EnumDataType.DoubleInput:
		case EnumDataType.IntRange:
		case EnumDataType.String:
		case EnumDataType.DoubleRange:
			return value.ToString();
		default:
			return null;
		}
	}
}
