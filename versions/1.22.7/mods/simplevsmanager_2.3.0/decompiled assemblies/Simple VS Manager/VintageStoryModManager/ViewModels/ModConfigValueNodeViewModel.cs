using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004A RID: 74
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigValueNodeViewModel : ModConfigNodeViewModel
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x0002E9B4 File Offset: 0x0002CBB4
		[NullableContext(2)]
		public ModConfigValueNodeViewModel([Nullable(1)] string name, JsonNode node, [Nullable(new byte[]
		{
			1,
			2
		})] Action<JsonNode> valueSetter, string displayName = null) : base(name, ModConfigValueNodeViewModel.DetermineTypeDisplay(node), displayName)
		{
			if (valueSetter == null)
			{
				throw new ArgumentNullException("valueSetter");
			}
			this._valueSetter = valueSetter;
			this._valueKind = ((node != null) ? node.GetValueKind() : 7);
			this._valueText = ModConfigValueNodeViewModel.ConvertNodeToText(node, this._valueKind);
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002EA0A File Offset: 0x0002CC0A
		public override bool IsEditable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0002EA0D File Offset: 0x0002CC0D
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0002EA15 File Offset: 0x0002CC15
		public string ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (base.SetProperty<string>(ref this._valueText, value, "ValueText") && this.IsBoolean)
				{
					base.OnPropertyChanged("BooleanValue");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0002EA40 File Offset: 0x0002CC40
		public bool IsBoolean
		{
			get
			{
				JsonValueKind valueKind = this._valueKind;
				return valueKind - 5 <= 1;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002EA61 File Offset: 0x0002CC61
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002EA74 File Offset: 0x0002CC74
		public bool BooleanValue
		{
			get
			{
				return string.Equals(this._valueText, bool.TrueString, 5);
			}
			set
			{
				string text = value ? bool.TrueString : bool.FalseString;
				if (base.SetProperty<string>(ref this._valueText, text, "BooleanValue"))
				{
					base.OnPropertyChanged("ValueText");
				}
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0002EAB0 File Offset: 0x0002CCB0
		public override void ApplyChanges()
		{
			JsonNode jsonNode = ModConfigValueNodeViewModel.ConvertTextToNode(this.ValueText, this._valueKind, base.DisplayLabel);
			this._valueSetter.Invoke(jsonNode);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002EAE4 File Offset: 0x0002CCE4
		private static string DetermineTypeDisplay([Nullable(2)] JsonNode node)
		{
			JsonValueKind jsonValueKind = (node != null) ? node.GetValueKind() : 7;
			string result;
			switch (jsonValueKind)
			{
			case 1:
				result = "Object";
				break;
			case 2:
				result = "Array";
				break;
			case 3:
				result = "String";
				break;
			case 4:
				result = "Number";
				break;
			case 5:
			case 6:
				result = "Boolean";
				break;
			case 7:
				result = "Null";
				break;
			default:
				result = jsonValueKind.ToString();
				break;
			}
			return result;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002EB64 File Offset: 0x0002CD64
		private static string ConvertNodeToText([Nullable(2)] JsonNode node, JsonValueKind kind)
		{
			if (node == null)
			{
				return string.Empty;
			}
			string result;
			switch (kind)
			{
			case 3:
				result = (node.GetValue<string>() ?? string.Empty);
				break;
			case 4:
				result = node.ToJsonString(null);
				break;
			case 5:
				result = bool.TrueString;
				break;
			case 6:
				result = bool.FalseString;
				break;
			case 7:
				result = string.Empty;
				break;
			default:
				result = node.ToJsonString(null);
				break;
			}
			return result;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002EBD8 File Offset: 0x0002CDD8
		[NullableContext(2)]
		private static JsonNode ConvertTextToNode(string text, JsonValueKind kind, [Nullable(1)] string propertyName)
		{
			switch (kind)
			{
			case 3:
				return JsonValue.Create(text ?? string.Empty, default(JsonNodeOptions?));
			case 4:
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidOperationException("Value for '" + propertyName + "' cannot be empty.");
				}
				long num;
				if (long.TryParse(text, 7, CultureInfo.InvariantCulture, ref num))
				{
					return JsonValue.Create(num, default(JsonNodeOptions?));
				}
				decimal num2;
				if (decimal.TryParse(text, 167, CultureInfo.InvariantCulture, ref num2))
				{
					return JsonValue.Create(num2, default(JsonNodeOptions?));
				}
				throw new InvalidOperationException("Value for '" + propertyName + "' must be a number.");
			}
			case 5:
			case 6:
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidOperationException("Value for '" + propertyName + "' cannot be empty.");
				}
				bool flag;
				if (!bool.TryParse(text, ref flag))
				{
					throw new InvalidOperationException("Value for '" + propertyName + "' must be a boolean (true/false).");
				}
				return JsonValue.Create(flag, default(JsonNodeOptions?));
			}
			case 7:
				if (string.IsNullOrWhiteSpace(text))
				{
					return null;
				}
				if (string.Equals(text.Trim(), "null", 5))
				{
					return null;
				}
				return JsonValue.Create(text, default(JsonNodeOptions?));
			default:
				return JsonValue.Create(text, default(JsonNodeOptions?));
			}
		}

		// Token: 0x040003C3 RID: 963
		private readonly JsonValueKind _valueKind;

		// Token: 0x040003C4 RID: 964
		[Nullable(new byte[]
		{
			1,
			2
		})]
		private readonly Action<JsonNode> _valueSetter;

		// Token: 0x040003C5 RID: 965
		private string _valueText;
	}
}
