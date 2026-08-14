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
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public class ConfigSetting : ISetting, IConfigBlock
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00007360 File Offset: 0x00005560
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00007368 File Offset: 0x00005568
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000073A1 File Offset: 0x000055A1
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000073A9 File Offset: 0x000055A9
		public JsonObject DefaultValue { get; internal set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000073B2 File Offset: 0x000055B2
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000073BA File Offset: 0x000055BA
		public ConfigSettingType SettingType { get; internal set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000073C3 File Offset: 0x000055C3
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000073CB File Offset: 0x000055CB
		public string YamlCode { get; internal set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000073D4 File Offset: 0x000055D4
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000073DC File Offset: 0x000055DC
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00007429 File Offset: 0x00005629
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00007431 File Offset: 0x00005631
		[Nullable(2)]
		public string Comment { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000743A File Offset: 0x0000563A
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00007442 File Offset: 0x00005642
		[Nullable(2)]
		public Validation Validation { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000744B File Offset: 0x0000564B
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00007453 File Offset: 0x00005653
		public float SortingWeight { get; internal set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000745C File Offset: 0x0000565C
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00007464 File Offset: 0x00005664
		[Nullable(2)]
		public string InGui { [NullableContext(2)] get; [NullableContext(2)] internal set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000746D File Offset: 0x0000566D
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00007475 File Offset: 0x00005675
		public bool Logarithmic { get; internal set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000747E File Offset: 0x0000567E
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00007486 File Offset: 0x00005686
		public bool ClientSide { get; internal set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000748F File Offset: 0x0000568F
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00007497 File Offset: 0x00005697
		public bool Hide { get; internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000074A0 File Offset: 0x000056A0
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000074A8 File Offset: 0x000056A8
		public string Link { get; internal set; } = "";

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000074B1 File Offset: 0x000056B1
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000074B9 File Offset: 0x000056B9
		public bool ChangedSinceLastSave { get; internal set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060000FE RID: 254 RVA: 0x000074C4 File Offset: 0x000056C4
		// (remove) Token: 0x060000FF RID: 255 RVA: 0x000074FC File Offset: 0x000056FC
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

		// Token: 0x06000100 RID: 256 RVA: 0x00007531 File Offset: 0x00005731
		public ConfigSetting(string yamlCode, JsonObject defaultValue, ConfigSettingType settingType)
		{
			this._value = defaultValue;
			this.DefaultValue = defaultValue;
			this.SettingType = settingType;
			this.YamlCode = yamlCode;
			this.InGui = yamlCode;
			this.Hide = false;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007570 File Offset: 0x00005770
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

		// Token: 0x06000102 RID: 258 RVA: 0x000075E8 File Offset: 0x000057E8
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

		// Token: 0x06000103 RID: 259 RVA: 0x00007270 File Offset: 0x00005470
		internal void Changed()
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000076B8 File Offset: 0x000058B8
		internal void SetValueFrom(ConfigSetting value)
		{
			this.Value = value._value.Clone();
			this.MappingKey = value.MappingKey;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000076D7 File Offset: 0x000058D7
		public static implicit operator ConfigSettingPacket(ConfigSetting setting)
		{
			return new ConfigSettingPacket(setting);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000076DF File Offset: 0x000058DF
		public ConfigSetting Clone()
		{
			return new ConfigSetting(this);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000076E8 File Offset: 0x000058E8
		public bool AssignSettingValue(object target)
		{
			Type type = target.GetType();
			IEnumerable<ValueTuple<string, FieldInfo>> fields = Enumerable.Select<FieldInfo, ValueTuple<string, FieldInfo>>(type.GetFields(52), (FieldInfo field) => new ValueTuple<string, FieldInfo>(ConfigSetting.NormalizeName(field.Name), field));
			IEnumerable<ValueTuple<string, PropertyInfo>> properties = Enumerable.Select<PropertyInfo, ValueTuple<string, PropertyInfo>>(Enumerable.Where<PropertyInfo>(type.GetProperties(52), (PropertyInfo property) => property.CanWrite), (PropertyInfo property) => new ValueTuple<string, PropertyInfo>(ConfigSetting.NormalizeName(property.Name), property));
			return this.AssignSettingValue(target, fields, properties);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007784 File Offset: 0x00005984
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

		// Token: 0x06000109 RID: 265 RVA: 0x00007884 File Offset: 0x00005A84
		internal static string NormalizeName(string name)
		{
			return name.ToLowerInvariant().Replace("_", "");
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000789C File Offset: 0x00005A9C
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

		// Token: 0x0600010B RID: 267 RVA: 0x00007980 File Offset: 0x00005B80
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

		// Token: 0x0600010C RID: 268 RVA: 0x00007A58 File Offset: 0x00005C58
		internal string ToYaml()
		{
			string yamlToken = this.SerializeToken();
			return this.AddComments(yamlToken);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007A78 File Offset: 0x00005C78
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

		// Token: 0x0600010E RID: 270 RVA: 0x00007B0C File Offset: 0x00005D0C
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

		// Token: 0x0600010F RID: 271 RVA: 0x00007BDC File Offset: 0x00005DDC
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

		// Token: 0x06000110 RID: 272 RVA: 0x00007CD0 File Offset: 0x00005ED0
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

		// Token: 0x06000111 RID: 273 RVA: 0x00007D0D File Offset: 0x00005F0D
		private string GetPreComment()
		{
			if (this.Comment != null)
			{
				return "\n# " + this.Comment.Replace("\n", "\n# ");
			}
			return "";
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007D3C File Offset: 0x00005F3C
		private string GetDefaultValueComment()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(13, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" (default: ");
			defaultInterpolatedStringHandler.AppendFormatted<JsonObject>(this.DefaultValue);
			defaultInterpolatedStringHandler.AppendLiteral(") ");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007D7F File Offset: 0x00005F7F
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

		// Token: 0x06000114 RID: 276 RVA: 0x00007DC0 File Offset: 0x00005FC0
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

		// Token: 0x06000115 RID: 277 RVA: 0x00007E74 File Offset: 0x00006074
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

		// Token: 0x06000116 RID: 278 RVA: 0x00007FF4 File Offset: 0x000061F4
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

		// Token: 0x06000117 RID: 279 RVA: 0x000080B4 File Offset: 0x000062B4
		private static JToken Unwrap(JObject token)
		{
			JToken value = token["value"];
			if (value == null)
			{
				return new JValue("<invalid>");
			}
			return value;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000080DC File Offset: 0x000062DC
		private static string Localize(string value, string domain)
		{
			string langCode = value.Contains(':') ? value : (domain + ":" + value);
			if (!Lang.HasTranslation(langCode, true, true))
			{
				return Lang.Get(value, Array.Empty<object>());
			}
			return Lang.Get(langCode, Array.Empty<object>());
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008124 File Offset: 0x00006324
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

		// Token: 0x0400007E RID: 126
		private JsonObject _value;

		// Token: 0x0400007F RID: 127
		[Nullable(2)]
		private string _mappingKey;

		// Token: 0x02000022 RID: 34
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000080 RID: 128
			[Nullable(0)]
			public static Func<JToken, object> <0>__ConvertJTokenToObject;
		}
	}
}
