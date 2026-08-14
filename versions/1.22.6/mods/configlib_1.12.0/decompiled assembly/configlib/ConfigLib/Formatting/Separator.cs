using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ImGuiNET;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace ConfigLib.Formatting
{
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Separator : IFormattingBlock, IConfigBlock
	{
		// Token: 0x06000245 RID: 581 RVA: 0x0000D458 File Offset: 0x0000B658
		public Separator(JsonObject definition, string domain, ICoreAPI api)
		{
			this._weight = definition["weight"].AsFloat(0f);
			this._collapsible = definition["collapsible"].AsBool(false);
			this._stopCollapsible = this._collapsible;
			this._weight = ((this._weight < 0f) ? 0f : this._weight);
			StringBuilder yaml = new StringBuilder();
			yaml.Append("\n\n");
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (definition.KeyExists("title"))
			{
				this._stopCollapsible = true;
				string title = Separator.Localize(definition["title"].AsString(null), domain);
				this._title = title;
				int width = title.Length + 6;
				string line2 = new string('#', width);
				StringBuilder stringBuilder = yaml;
				StringBuilder stringBuilder2 = stringBuilder;
				appendInterpolatedStringHandler..ctor(9, 3, stringBuilder);
				appendInterpolatedStringHandler.AppendFormatted(line2);
				appendInterpolatedStringHandler.AppendLiteral("\n## ");
				appendInterpolatedStringHandler.AppendFormatted(title);
				appendInterpolatedStringHandler.AppendLiteral(" ##\n");
				appendInterpolatedStringHandler.AppendFormatted(line2);
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder2.Append(ref appendInterpolatedStringHandler);
			}
			if (definition.KeyExists("text"))
			{
				string text = Separator.Localize(definition["text"].AsString(null), domain);
				this._text = text;
				string composed = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(text.Split('\n', 0), (string line) => "# " + line), (string first, string second) => first + "\n" + second);
				StringBuilder stringBuilder = yaml;
				StringBuilder stringBuilder3 = stringBuilder;
				appendInterpolatedStringHandler..ctor(1, 1, stringBuilder);
				appendInterpolatedStringHandler.AppendFormatted(composed);
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder3.Append(ref appendInterpolatedStringHandler);
			}
			if (definition.KeyExists("link"))
			{
				this._link = definition["link"].AsString(null);
				this._linkText = definition["linkText"].AsString(null);
				if (this._linkText != null)
				{
					this._linkText = Separator.Localize(this._linkText, domain);
				}
				StringBuilder stringBuilder = yaml;
				StringBuilder stringBuilder4 = stringBuilder;
				appendInterpolatedStringHandler..ctor(3, 1, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("# ");
				appendInterpolatedStringHandler.AppendFormatted(this._link);
				appendInterpolatedStringHandler.AppendLiteral("\n");
				stringBuilder4.Append(ref appendInterpolatedStringHandler);
			}
			this._yaml = yaml.ToString();
			this._api = api;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000D6D2 File Offset: 0x0000B8D2
		public string Yaml
		{
			get
			{
				return this._yaml;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D6DA File Offset: 0x0000B8DA
		public float SortingWeight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D6E2 File Offset: 0x0000B8E2
		public bool Collapsible
		{
			get
			{
				return this._collapsible;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000D6EA File Offset: 0x0000B8EA
		public bool StopCollapsible
		{
			get
			{
				return this._stopCollapsible;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D6F4 File Offset: 0x0000B8F4
		public bool Draw(string id)
		{
			bool collapsed = false;
			if (this._title != null)
			{
				if (this._collapsible)
				{
					collapsed = !ImGui.CollapsingHeader(MemoryExtensions.AsSpan(this._title + "##" + id));
				}
				else
				{
					ImGui.SeparatorText(MemoryExtensions.AsSpan(this._title));
				}
			}
			else
			{
				ImGui.Separator();
			}
			if (this._text != null)
			{
				ImGui.TextWrapped(MemoryExtensions.AsSpan(this._text));
			}
			if (this._link != null)
			{
				if (ImGui.Button(MemoryExtensions.AsSpan((this._linkText ?? this._link) + "##" + id)))
				{
					ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
					if (coreClientAPI != null)
					{
						coreClientAPI.Gui.OpenLink(this._link);
					}
				}
				if (ImGui.BeginItemTooltip())
				{
					ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
					ImGui.TextUnformatted(MemoryExtensions.AsSpan("open in browser: " + this._link));
					ImGui.PopTextWrapPos();
					ImGui.EndTooltip();
				}
				ImGui.Separator();
			}
			return !collapsed;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D7FC File Offset: 0x0000B9FC
		private static string Localize(string value, string domain)
		{
			string langCode = value.Contains(':') ? value : (domain + ":" + value);
			if (!Lang.HasTranslation(langCode, true, true))
			{
				return value;
			}
			return Lang.Get(langCode, Array.Empty<object>());
		}

		// Token: 0x0400011A RID: 282
		private readonly string _yaml;

		// Token: 0x0400011B RID: 283
		private readonly float _weight;

		// Token: 0x0400011C RID: 284
		private readonly bool _collapsible;

		// Token: 0x0400011D RID: 285
		private readonly bool _stopCollapsible;

		// Token: 0x0400011E RID: 286
		[Nullable(2)]
		private readonly string _title;

		// Token: 0x0400011F RID: 287
		[Nullable(2)]
		private readonly string _text;

		// Token: 0x04000120 RID: 288
		[Nullable(2)]
		private readonly string _link;

		// Token: 0x04000121 RID: 289
		[Nullable(2)]
		private readonly string _linkText;

		// Token: 0x04000122 RID: 290
		private readonly ICoreAPI _api;
	}
}
