using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ConfigLib.Formatting;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using YamlDotNet.Serialization;

namespace ConfigLib
{
	// Token: 0x02000023 RID: 35
	[NullableContext(1)]
	[Nullable(0)]
	public class ConfigSetting : ISetting, IConfigBlock
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000767C File Offset: 0x0000587C
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007684 File Offset: 0x00005884
		public JsonObject Value
		{
			get
			{
				return this._value;
			}
			set
			{
				bool flag = this._value.ToString() != value.ToString();
				this._value = value;
				if (flag)
				{
					this.ChangedSinceLastSave = true;
					Action<ConfigSetting> settingChanged = this.SettingChanged;
					if (settingChanged == null)
					{
						return;
					}
					settingChanged.Invoke(this);
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000076BD File Offset: 0x000058BD
		// (set) Token: 0x0600010A RID: 266 RVA: 0x000076C5 File Offset: 0x000058C5
		public JsonObject DefaultValue { get; internal set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000076CE File Offset: 0x000058CE
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000076D6 File Offset: 0x000058D6
		public ConfigSettingType SettingType { get; internal set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000076DF File Offset: 0x000058DF
		// (set) Token: 0x0600010E RID: 270 RVA: 0x000076E7 File Offset: 0x000058E7
		public string YamlCode { get; internal set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000076F0 File Offset: 0x000058F0
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000076F8 File Offset: 0x000058F8
		[Nullable(2)]
		public string MappingKey
		{
			[NullableContext(2)]
			get
			{
				return this._mappingKey;
			}
			[NullableContext(2)]
			set
			{
				this._mappingKey = value;
				if (this._mappingKey != null && this.Validation != null && this.Validation.Mapping != null)
				{
					this.Value = this.Validation.Mapping[this._mappingKey];
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00007745 File Offset: 0x00005945
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000774D File Offset: 0x0000594D
		[Nullable(2)]
		public string Comment { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007756 File Offset: 0x00005956
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000775E File Offset: 0x0000595E
		[Nullable(2)]
		public Validation Validation { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007767 File Offset: 0x00005967
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000776F File Offset: 0x0000596F
		public float SortingWeight { get; internal set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007778 File Offset: 0x00005978
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00007780 File Offset: 0x00005980
		[Nullable(2)]
		public string InGui { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007789 File Offset: 0x00005989
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00007791 File Offset: 0x00005991
		public bool Logarithmic { get; internal set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000779A File Offset: 0x0000599A
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000077A2 File Offset: 0x000059A2
		public bool ClientSide { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011D RID: 285 RVA: 0x000077AB File Offset: 0x000059AB
		// (set) Token: 0x0600011E RID: 286 RVA: 0x000077B3 File Offset: 0x000059B3
		public bool Hide { get; internal set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011F RID: 287 RVA: 0x000077BC File Offset: 0x000059BC
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000077C4 File Offset: 0x000059C4
		public string Link { get; internal set; } = "";

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000077CD File Offset: 0x000059CD
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000077D5 File Offset: 0x000059D5
		public bool ChangedSinceLastSave { get; internal set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000123 RID: 291 RVA: 0x000077E0 File Offset: 0x000059E0
		// (remove) Token: 0x06000124 RID: 292 RVA: 0x00007818 File Offset: 0x00005A18
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<ConfigSetting> SettingChanged;

		// Token: 0x06000125 RID: 293 RVA: 0x0000784D File Offset: 0x00005A4D
		public ConfigSetting(string yamlCode, JsonObject defaultValue, ConfigSettingType settingType)
		{
			this._value = defaultValue;
			this.DefaultValue = defaultValue;
			this.SettingType = settingType;
			this.YamlCode = yamlCode;
			this.InGui = yamlCode;
			this.Hide = false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000788C File Offset: 0x00005A8C
		public ConfigSetting(ConfigSettingPacket settings)
		{
			this._value = new JsonObject(ConfigSetting.Unwrap(JObject.Parse(settings.Value)));
			this.DefaultValue = this._value;
			this.SettingType = ConfigSettingType.None;
			this.MappingKey = settings.MappingKey;
			this.ClientSide = settings.ClientSide;
			this.YamlCode = string.Empty;
			this.Hide = false;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007904 File Offset: 0x00005B04
		private ConfigSetting(ConfigSetting previous)
		{
			this._value = previous.Value.Clone();
			this.DefaultValue = previous.DefaultValue.Clone();
			this.SettingType = previous.SettingType;
			this.YamlCode = previous.YamlCode;
			this.MappingKey = previous.MappingKey;
			this.Comment = previous.Comment;
			this.Validation = previous.Validation;
			this.SortingWeight = previous.SortingWeight;
			this.InGui = previous.InGui;
			this.Logarithmic = previous.Logarithmic;
			this.ClientSide = previous.ClientSide;
			this.SettingChanged = previous.SettingChanged;
			this.Hide = previous.Hide;
			this.Link = previous.Link;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00002176 File Offset: 0x00000376
		internal void Changed()
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000079D4 File Offset: 0x00005BD4
		internal void SetValueFrom(ConfigSetting value)
		{
			this.Value = value._value.Clone();
			this.MappingKey = value.MappingKey;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000079F3 File Offset: 0x00005BF3
		public static implicit operator ConfigSettingPacket(ConfigSetting setting)
		{
			return new ConfigSettingPacket(setting);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000079FB File Offset: 0x00005BFB
		public ConfigSetting Clone()
		{
			return new ConfigSetting(this);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007A04 File Offset: 0x00005C04
		public bool AssignSettingValue(object target)
		{
			Type type = target.GetType();
			IEnumerable<ValueTuple<string, FieldInfo>> fields = Enumerable.Select<FieldInfo, ValueTuple<string, FieldInfo>>(type.GetFields(52), (FieldInfo field) => new ValueTuple<string, FieldInfo>(ConfigSetting.NormalizeName(field.Name), field));
			IEnumerable<ValueTuple<string, PropertyInfo>> properties = Enumerable.Select<PropertyInfo, ValueTuple<string, PropertyInfo>>(Enumerable.Where<PropertyInfo>(type.GetProperties(52), (PropertyInfo property) => property.CanWrite), (PropertyInfo property) => new ValueTuple<string, PropertyInfo>(ConfigSetting.NormalizeName(property.Name), property));
			return this.AssignSettingValue(target, fields, properties);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007AA0 File Offset: 0x00005CA0
		internal bool AssignSettingValue(object target, [TupleElementNames(new string[]
		{
			"code",
			"field"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IEnumerable<ValueTuple<string, FieldInfo>> fields, [TupleElementNames(new string[]
		{
			"code",
			"field"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IEnumerable<ValueTuple<string, PropertyInfo>> properties)
		{
			string code = ConfigSetting.NormalizeName(this.YamlCode);
			bool anyAssigned = false;
			Func<ValueTuple<string, FieldInfo>, bool> <>9__0;
			Func<ValueTuple<string, FieldInfo>, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (([TupleElementNames(new string[]
				{
					"code",
					"field"
				})] ValueTuple<string, FieldInfo> entry) => entry.Item1 == code));
			}
			foreach (ValueTuple<string, FieldInfo> valueTuple in Enumerable.Where<ValueTuple<string, FieldInfo>>(fields, func))
			{
				FieldInfo field = valueTuple.Item2;
				if (this.AssignSettingValue(target, field))
				{
					anyAssigned = true;
				}
			}
			Func<ValueTuple<string, PropertyInfo>, bool> <>9__1;
			Func<ValueTuple<string, PropertyInfo>, bool> func2;
			if ((func2 = <>9__1) == null)
			{
				func2 = (<>9__1 = (([TupleElementNames(new string[]
				{
					"code",
					"field"
				})] ValueTuple<string, PropertyInfo> entry) => entry.Item1 == code));
			}
			foreach (ValueTuple<string, PropertyInfo> valueTuple2 in Enumerable.Where<ValueTuple<string, PropertyInfo>>(properties, func2))
			{
				PropertyInfo field2 = valueTuple2.Item2;
				if (this.AssignSettingValue(target, field2))
				{
					anyAssigned = true;
				}
			}
			return anyAssigned;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007BA0 File Offset: 0x00005DA0
		internal static string NormalizeName(string name)
		{
			return name.ToLowerInvariant().Replace("_", "");
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007BB8 File Offset: 0x00005DB8
		private bool AssignSettingValue(object target, [Nullable(2)] PropertyInfo property)
		{
			if (property != null && property.CanWrite)
			{
				switch (this.SettingType)
				{
				case ConfigSettingType.Boolean:
					property.SetValue(target, this.Value.AsBool(false));
					break;
				case ConfigSettingType.Float:
					property.SetValue(target, this.Value.AsFloat(0f));
					break;
				case ConfigSettingType.Integer:
					property.SetValue(target, this.Value.AsInt(0));
					break;
				case ConfigSettingType.String:
					property.SetValue(target, this.Value.AsString(null));
					break;
				case ConfigSettingType.Color:
					property.SetValue(target, this.Value.AsString(null));
					break;
				case ConfigSettingType.Other:
					property.SetValue(target, this.Value.ToAttribute());
					break;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007C9C File Offset: 0x00005E9C
		private bool AssignSettingValue(object target, [Nullable(2)] FieldInfo field)
		{
			if (field != null)
			{
				switch (this.SettingType)
				{
				case ConfigSettingType.Boolean:
					field.SetValue(target, this.Value.AsBool(false));
					break;
				case ConfigSettingType.Float:
					field.SetValue(target, this.Value.AsFloat(0f));
					break;
				case ConfigSettingType.Integer:
					field.SetValue(target, this.Value.AsInt(0));
					break;
				case ConfigSettingType.String:
					field.SetValue(target, this.Value.AsString(null));
					break;
				case ConfigSettingType.Color:
					field.SetValue(target, this.Value.AsString(null));
					break;
				case ConfigSettingType.Other:
					field.SetValue(target, this.Value.ToAttribute());
					break;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007D74 File Offset: 0x00005F74
		internal string ToYaml()
		{
			string yamlToken = this.SerializeToken();
			return this.AddComments(yamlToken);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007D94 File Offset: 0x00005F94
		private string SerializeToken()
		{
			JProperty token;
			if (this.MappingKey != null)
			{
				token = new JProperty(this.YamlCode, new JValue(this.MappingKey));
			}
			else
			{
				token = new JProperty(this.YamlCode, this.Value.Token);
			}
			JObject jobject = new JObject();
			jobject.Add(token);
			object simplifiedToken = ConfigSetting.ConvertJTokenToObject(jobject);
			Serializer serializer = new Serializer();
			string result;
			using (StringWriter writer = new StringWriter())
			{
				serializer.Serialize(writer, simplifiedToken);
				result = writer.ToString();
			}
			return result;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00007E28 File Offset: 0x00006028
		private static object ConvertJTokenToObject(JToken token)
		{
			JValue value = token as JValue;
			if (value != null && value.Value != null)
			{
				return value.Value;
			}
			if (token is JArray)
			{
				IEnumerable<JToken> enumerable = Enumerable.AsEnumerable<JToken>(token);
				Func<JToken, object> func;
				if ((func = ConfigSetting.<>O.<0>__ConvertJTokenToObject) == null)
				{
					func = (ConfigSetting.<>O.<0>__ConvertJTokenToObject = new Func<JToken, object>(ConfigSetting.ConvertJTokenToObject));
				}
				return Enumerable.ToList<object>(Enumerable.Select<JToken, object>(enumerable, func));
			}
			if (token is JObject)
			{
				return Enumerable.ToDictionary<JProperty, string, object>(Enumerable.Cast<JProperty>(Enumerable.AsEnumerable<JToken>(token)), (JProperty x) => x.Name, (JProperty x) => ConfigSetting.ConvertJTokenToObject(x.Value));
			}
			throw new InvalidOperationException("Unexpected token: " + ((token != null) ? token.ToString() : null));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007EF8 File Offset: 0x000060F8
		private string AddComments(string yamlToken)
		{
			ValueTuple<string, string> valueTuple = ConfigSetting.SplitToken(yamlToken);
			string first = valueTuple.Item1;
			string other = valueTuple.Item2;
			string comment = this.GetPreComment();
			if (comment != "")
			{
				comment += "\n";
			}
			if (this.Link != "")
			{
				comment = comment + "# " + this.Link + "\n";
			}
			string inline = this.GetInlineComment();
			if (inline != "")
			{
				inline = " # " + inline;
			}
			string defaultValue = this.GetDefaultValueComment();
			if (defaultValue != "" && inline == "")
			{
				defaultValue = " # " + defaultValue;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 5);
			defaultInterpolatedStringHandler.AppendFormatted(comment);
			defaultInterpolatedStringHandler.AppendFormatted(first);
			defaultInterpolatedStringHandler.AppendFormatted(inline);
			defaultInterpolatedStringHandler.AppendFormatted(defaultValue);
			defaultInterpolatedStringHandler.AppendFormatted(other);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007FEC File Offset: 0x000061EC
		[return: TupleElementNames(new string[]
		{
			"firstLine",
			"remainder"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1,
			1
		})]
		private static ValueTuple<string, string> SplitToken(string token)
		{
			string[] split = token.Split("\r\n", 2, 1);
			return new ValueTuple<string, string>(split[0], (split.Length > 1) ? ("\r\n" + split[1]) : "");
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008029 File Offset: 0x00006229
		private string GetPreComment()
		{
			if (this.Comment != null)
			{
				return "\n# " + this.Comment.Replace("\n", "\n# ");
			}
			return "";
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008058 File Offset: 0x00006258
		private string GetDefaultValueComment()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" (default: ");
			defaultInterpolatedStringHandler.AppendFormatted<JsonObject>(this.DefaultValue);
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000809B File Offset: 0x0000629B
		private string GetInlineComment()
		{
			Validation validation = this.Validation;
			if (((validation != null) ? validation.Mapping : null) != null)
			{
				return this.ParseMappingComment();
			}
			Validation validation2 = this.Validation;
			if (((validation2 != null) ? validation2.Values : null) != null)
			{
				return this.ParseValuesComment();
			}
			return this.ParseRangeComment();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000080DC File Offset: 0x000062DC
		private string ParseMappingComment()
		{
			if (this.Validation == null || this.Validation.Mapping == null)
			{
				return "";
			}
			StringBuilder result = new StringBuilder();
			result.Append("value from: ");
			bool first = true;
			foreach (KeyValuePair<string, JsonObject> keyValuePair in this.Validation.Mapping)
			{
				string text;
				JsonObject jsonObject;
				keyValuePair.Deconstruct(ref text, ref jsonObject);
				string name = text;
				if (!first)
				{
					result.Append(", ");
				}
				if (first)
				{
					first = false;
				}
				result.Append(name);
			}
			return result.ToString();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008190 File Offset: 0x00006390
		private string ParseRangeComment()
		{
			if (this.Validation == null || (this.Validation.Minimum == null && this.Validation.Maximum == null && this.Validation.Step == null))
			{
				return "";
			}
			bool flag = this.Validation.Minimum != null;
			bool flag2 = this.Validation.Maximum != null;
			string text;
			if (flag)
			{
				if (!flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(13, 1);
					defaultInterpolatedStringHandler.AppendLiteral("greater than ");
					defaultInterpolatedStringHandler.AppendFormatted<JsonObject>(this.Validation.Minimum);
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(9, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("from ");
					defaultInterpolatedStringHandler2.AppendFormatted<JsonObject>(this.Validation.Minimum);
					defaultInterpolatedStringHandler2.AppendLiteral(" to ");
					defaultInterpolatedStringHandler2.AppendFormatted<JsonObject>(this.Validation.Maximum);
					text = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			else if (!flag2)
			{
				text = "";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(10, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("less than ");
				defaultInterpolatedStringHandler3.AppendFormatted<JsonObject>(this.Validation.Maximum);
				text = defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			string minMax = text;
			if (this.Validation.Step != null && minMax != "")
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(14, 2);
				defaultInterpolatedStringHandler4.AppendFormatted(minMax);
				defaultInterpolatedStringHandler4.AppendLiteral(" with step of ");
				defaultInterpolatedStringHandler4.AppendFormatted<JsonObject>(this.Validation.Step);
				return defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			return minMax;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008310 File Offset: 0x00006510
		private string ParseValuesComment()
		{
			if (this.Validation == null || this.Validation.Values == null)
			{
				return "";
			}
			StringBuilder result = new StringBuilder();
			result.Append("value from: ");
			bool first = true;
			foreach (string value in Enumerable.Select<JsonObject, string>(this.Validation.Values, (JsonObject element) => element.ToString()))
			{
				if (!first)
				{
					result.Append(", ");
				}
				if (first)
				{
					first = false;
				}
				result.Append(value);
			}
			return result.ToString();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000083D0 File Offset: 0x000065D0
		private static JToken Unwrap(JObject token)
		{
			JToken value = token["value"];
			if (value == null)
			{
				return new JValue("<invalid>");
			}
			return value;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000083F8 File Offset: 0x000065F8
		private static string Localize(string value, string domain)
		{
			string langCode = value.Contains(':') ? value : (domain + ":" + value);
			if (!Lang.HasTranslation(langCode, true, true))
			{
				return Lang.Get(value, Array.Empty<object>());
			}
			return Lang.Get(langCode, Array.Empty<object>());
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008440 File Offset: 0x00006640
		internal static ConfigSetting FromJson(JsonObject json, ConfigSettingType settingType, string domain, string code, ICoreAPI api)
		{
			if (!json.KeyExists("default"))
			{
				Type typeFromHandle = typeof(ConfigSetting);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Setting '");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral("' of type '");
				defaultInterpolatedStringHandler.AppendFormatted<ConfigSettingType>(settingType);
				defaultInterpolatedStringHandler.AppendLiteral("' does not have default value");
				LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ConfigSetting setting = new ConfigSetting(json["name"].AsString(code), json["default"], settingType)
			{
				Value = json["default"],
				Comment = json["comment"].AsString(null),
				InGui = json["ingui"].AsString(json["nameInGui"].AsString(json["name"].AsString(code))),
				ClientSide = json["clientSide"].AsBool(false),
				SortingWeight = json["weight"].AsFloat(0f),
				Logarithmic = json["logarithmic"].AsBool(false),
				Hide = json["hide"].AsBool(false),
				Link = json["link"].AsString("")
			};
			if (setting.InGui != null)
			{
				setting.InGui = ConfigSetting.Localize(setting.InGui, domain);
			}
			if (setting.Comment != null)
			{
				setting.Comment = ConfigSetting.Localize(setting.Comment, domain);
			}
			ValueTuple<string, JsonObject, Validation> valueTuple = Validation.FromJson(json, setting.Value.AsString(null));
			string mappingKey = valueTuple.Item1;
			JsonObject value = valueTuple.Item2;
			Validation validation = valueTuple.Item3;
			if (value != null)
			{
				setting.Value = value;
			}
			setting.MappingKey = mappingKey;
			setting.Validation = validation;
			return setting;
		}

		// Token: 0x04000081 RID: 129
		private JsonObject _value;

		// Token: 0x04000082 RID: 130
		[Nullable(2)]
		private string _mappingKey;

		// Token: 0x02000024 RID: 36
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000083 RID: 131
			[Nullable(0)]
			public static Func<JToken, object> <0>__ConvertJTokenToObject;
		}
	}
}
