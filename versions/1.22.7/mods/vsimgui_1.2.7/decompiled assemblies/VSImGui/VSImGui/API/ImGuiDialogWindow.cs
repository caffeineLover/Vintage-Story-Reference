using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Client;

namespace VSImGui.API
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ImGuiDialogWindow : ImGuiDialogBase
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D544 File Offset: 0x0000B744
		protected string WindowTitle { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000D54C File Offset: 0x0000B74C
		protected string WindowId { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000D554 File Offset: 0x0000B754
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000D55C File Offset: 0x0000B75C
		protected string TitleInfo { get; set; } = "";

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000D565 File Offset: 0x0000B765
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000D56D File Offset: 0x0000B76D
		protected ImGuiWindowFlags Flags { get; set; }

		// Token: 0x0600028F RID: 655 RVA: 0x0000D578 File Offset: 0x0000B778
		protected ImGuiDialogWindow(ICoreClientAPI api, string windowTitle, string windowId = "", bool includeTitleIntoId = true, ImGuiWindowFlags flags = 0) : base(api)
		{
			this.WindowTitle = windowTitle;
			if (includeTitleIntoId)
			{
				this.WindowId = windowTitle + "." + windowId;
			}
			else
			{
				this.WindowId = windowId;
			}
			this._title = "##" + this.WindowId + "###" + this.WindowTitle;
			this.Flags = flags;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D5E6 File Offset: 0x0000B7E6
		protected ImGuiDialogWindow(ICoreClientAPI api, ImGuiWindowFlags flags, string windowTitle, string windowId = "", bool includeTitleIntoId = true) : this(api, windowTitle, windowId, includeTitleIntoId, flags)
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D5F8 File Offset: 0x0000B7F8
		protected override CallbackGUIStatus Draw(float deltaSeconds)
		{
			bool opened = base.Opened;
			if (!opened)
			{
				return base.DrawStatus();
			}
			if (ImGui.Begin(MemoryExtensions.AsSpan(this._title + this.TitleInfo), ref opened, this.Flags))
			{
				if (!this.OnDraw() && opened)
				{
					base.Close();
				}
				ImGui.End();
			}
			if (!opened)
			{
				base.Close();
			}
			return base.DrawStatus();
		}

		// Token: 0x04000052 RID: 82
		private readonly string _title;
	}
}
