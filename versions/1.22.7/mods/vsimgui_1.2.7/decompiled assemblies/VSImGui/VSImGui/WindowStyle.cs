using System;
using System.Runtime.CompilerServices;
using System.Text;
using ImGuiNET;
using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	[JsonObject(0)]
	public class WindowStyle : Style
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000B254 File Offset: 0x00009454
		public WindowStyle()
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B2AC File Offset: 0x000094AC
		public WindowStyle(ICoreClientAPI api, Style style) : base(style)
		{
			this.mApi = api;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B30C File Offset: 0x0000950C
		public WindowStyle(ICoreClientAPI api)
		{
			this.mApi = api;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B368 File Offset: 0x00009568
		public WindowStyle(ICoreClientAPI api, string fontName, int fontSize = 14) : base(fontName, fontSize)
		{
			this.mApi = api;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public WindowStyle(ICoreClientAPI api, ImGuiStyle style, string fontName, int fontSize = 14) : base(style, fontName, fontSize)
		{
			this.mApi = api;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000B428 File Offset: 0x00009628
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000B430 File Offset: 0x00009630
		public string BackgroundTexture
		{
			get
			{
				return this.mBackgroundTexturePath;
			}
			set
			{
				this.mBackgroundTexturePath = value;
				try
				{
					ICoreClientAPI coreClientAPI = this.mApi;
					this.mBackgroundTexture = ((coreClientAPI != null) ? coreClientAPI.Render.GetOrLoadTexture(new AssetLocation(this.mBackgroundTexturePath)) : -1);
				}
				catch
				{
					this.mBackgroundTexture = -1;
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000B488 File Offset: 0x00009688
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000B490 File Offset: 0x00009690
		public Value2 BackgroundTextureSize { get; set; } = new ValueTuple<float, float>(-1f, -1f);

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000B49C File Offset: 0x0000969C
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public bool BackgroundTextureApplied
		{
			get
			{
				return this.mBackgroundTexture > 0 && this.BackgroundTextureSize.X > 0f && this.BackgroundTextureSize.Y > 0f && this.mBackgroundTextureApplied;
			}
			set
			{
				if (value)
				{
					this.BackgroundTexture = this.mBackgroundTexturePath;
					this.mBackgroundTextureApplied = true;
					return;
				}
				this.mBackgroundTexture = -1;
				this.mBackgroundTextureApplied = false;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000B50B File Offset: 0x0000970B
		[JsonIgnore]
		public ICoreClientAPI Api
		{
			get
			{
				return this.mApi;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B513 File Offset: 0x00009713
		public bool Begin(string name, ref bool open)
		{
			base.Push();
			bool result = ImGui.Begin(MemoryExtensions.AsSpan(name), ref open, this.Flags.WindowFlagsValue);
			if (this.BackgroundTextureApplied)
			{
				TilingUtils.TileWindowWithTexture(this.mBackgroundTexture, this.BackgroundTextureSize, TilingUtils.TextureRenderLevel.Window);
			}
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000B551 File Offset: 0x00009751
		public bool Begin(string name)
		{
			base.Push();
			bool result = ImGui.Begin(MemoryExtensions.AsSpan(name), this.Flags.WindowFlagsValue);
			if (this.BackgroundTextureApplied)
			{
				TilingUtils.TileWindowWithTexture(this.mBackgroundTexture, this.BackgroundTextureSize, TilingUtils.TextureRenderLevel.Window);
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000B58E File Offset: 0x0000978E
		public void End()
		{
			base.Pop();
			ImGui.End();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000B59B File Offset: 0x0000979B
		public void OnDeserilization(ICoreClientAPI api)
		{
			this.mApi = api;
			if (this.mBackgroundTextureApplied)
			{
				this.BackgroundTexture = this.mBackgroundTexturePath;
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000B5B8 File Offset: 0x000097B8
		public override void SetFrom(Style style)
		{
			base.SetFrom(style);
			WindowStyle windowStyle = style as WindowStyle;
			if (windowStyle == null)
			{
				return;
			}
			this.Flags = windowStyle.Flags;
			this.mBackgroundTexture = windowStyle.mBackgroundTexture;
			this.BackgroundTextureSize = windowStyle.BackgroundTextureSize;
			this.mBackgroundTexturePath = windowStyle.mBackgroundTexturePath;
			this.mApi = windowStyle.mApi;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000B614 File Offset: 0x00009814
		public override string ToCode()
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine("VSImGui.WindowStyle style = new(api);");
			this.TexturePropertiesToCode(result);
			this.WindowPropertiesToCode(result);
			base.OtherPropertiesToCode(result);
			base.FloatPropertiesToCode(result);
			base.Value2PropertiesToCode(result);
			base.Value4PropertiesToCode(result);
			return result.ToString();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000B664 File Offset: 0x00009864
		protected void TexturePropertiesToCode(StringBuilder result)
		{
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			if (this.mBackgroundTextureApplied)
			{
				appendInterpolatedStringHandler..ctor(29, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BackgroundTexture = \"");
				appendInterpolatedStringHandler.AppendFormatted(this.BackgroundTexture);
				appendInterpolatedStringHandler.AppendLiteral("\";");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (this.mBackgroundTextureApplied)
			{
				appendInterpolatedStringHandler..ctor(35, 2, result);
				appendInterpolatedStringHandler.AppendLiteral("style.BackgroundTextureSize = (");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BackgroundTextureSize.X);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<float>(this.BackgroundTextureSize.Y);
				appendInterpolatedStringHandler.AppendLiteral(");");
				result.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000B720 File Offset: 0x00009920
		protected void WindowPropertiesToCode(StringBuilder result)
		{
			if (this.Flags.NoTitleBar)
			{
				result.AppendLine("style.Flags.NoTitleBar = true;");
			}
			if (this.Flags.NoResize)
			{
				result.AppendLine("style.Flags.NoResize = true;");
			}
			if (this.Flags.NoMove)
			{
				result.AppendLine("style.Flags.NoMove = true;");
			}
			if (this.Flags.NoScrollbar)
			{
				result.AppendLine("style.Flags.NoScrollbar = true;");
			}
			if (this.Flags.NoScrollWithMouse)
			{
				result.AppendLine("style.Flags.NoScrollWithMouse = true;");
			}
			if (this.Flags.NoCollapse)
			{
				result.AppendLine("style.Flags.NoCollapse = true;");
			}
			if (this.Flags.AlwaysAutoResize)
			{
				result.AppendLine("style.Flags.AlwaysAutoResize = true;");
			}
			if (this.Flags.NoBackground)
			{
				result.AppendLine("style.Flags.NoBackground = true;");
			}
			if (this.Flags.NoSavedSettings)
			{
				result.AppendLine("style.Flags.NoSavedSettings = true;");
			}
			if (this.Flags.NoMouseInputs)
			{
				result.AppendLine("style.Flags.NoMouseInputs = true;");
			}
			if (this.Flags.MenuBar)
			{
				result.AppendLine("style.Flags.MenuBar = true;");
			}
			if (this.Flags.HorizontalScrollbar)
			{
				result.AppendLine("style.Flags.HorizontalScrollbar = true;");
			}
			if (this.Flags.NoFocusOnAppearing)
			{
				result.AppendLine("style.Flags.NoFocusOnAppearing = true;");
			}
			if (this.Flags.NoBringToFrontOnFocus)
			{
				result.AppendLine("style.Flags.NoBringToFrontOnFocus = true;");
			}
			if (this.Flags.AlwaysVerticalScrollbar)
			{
				result.AppendLine("style.Flags.AlwaysVerticalScrollbar = true;");
			}
			if (this.Flags.AlwaysHorizontalScrollbar)
			{
				result.AppendLine("style.Flags.AlwaysHorizontalScrollbar = true;");
			}
			if (this.Flags.AlwaysUseWindowPadding)
			{
				result.AppendLine("style.Flags.AlwaysUseWindowPadding = true;");
			}
			if (this.Flags.NoNavInputs)
			{
				result.AppendLine("style.Flags.NoNavInputs = true;");
			}
			if (this.Flags.NoNavFocus)
			{
				result.AppendLine("style.Flags.NoNavFocus = true;");
			}
			if (this.Flags.UnsavedDocument)
			{
				result.AppendLine("style.Flags.UnsavedDocument = true;");
			}
			if (this.Flags.NoDocking)
			{
				result.AppendLine("style.Flags.NoDocking = true;");
			}
			if (this.Flags.NoNav)
			{
				result.AppendLine("style.Flags.NoNav = true;");
			}
			if (this.Flags.NoDecoration)
			{
				result.AppendLine("style.Flags.NoDecoration = true;");
			}
			if (this.Flags.NoInputs)
			{
				result.AppendLine("style.Flags.NoInputs = true;");
			}
			if (this.Flags.NavFlattened)
			{
				result.AppendLine("style.Flags.NavFlattened = true;");
			}
			if (this.Flags.ChildWindow)
			{
				result.AppendLine("style.Flags.ChildWindow = true;");
			}
			if (this.Flags.Tooltip)
			{
				result.AppendLine("style.Flags.Tooltip = true;");
			}
			if (this.Flags.Popup)
			{
				result.AppendLine("style.Flags.Popup = true;");
			}
			if (this.Flags.Modal)
			{
				result.AppendLine("style.Flags.Modal = true;");
			}
			if (this.Flags.ChildMenu)
			{
				result.AppendLine("style.Flags.ChildMenu = true;");
			}
		}

		// Token: 0x04000039 RID: 57
		[JsonProperty("WindowFlags")]
		public WindowFlags Flags = new WindowFlags
		{
			WindowFlagsValue = 0
		};

		// Token: 0x0400003A RID: 58
		private ICoreClientAPI mApi;

		// Token: 0x0400003B RID: 59
		private int mBackgroundTexture = -1;

		// Token: 0x0400003C RID: 60
		private string mBackgroundTexturePath = "";

		// Token: 0x0400003D RID: 61
		private bool mBackgroundTextureApplied;
	}
}
