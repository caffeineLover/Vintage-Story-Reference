using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200009D RID: 157
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModeSelector
	{
		// Token: 0x06000855 RID: 2133 RVA: 0x00030621 File Offset: 0x0002E821
		public ModeSelector(ICoreClientAPI api, IEnumerable<ModeConfig> modes)
		{
			this._api = api;
			this.SelectedMode = Enumerable.FirstOrDefault<ModeConfig>(modes).Code;
			this._modes = Enumerable.ToArray<ModeConfig>(modes);
			this._modesSkillItems = this.GenerateToolModes();
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00030659 File Offset: 0x0002E859
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x00030661 File Offset: 0x0002E861
		public string SelectedMode { get; private set; }

		// Token: 0x06000858 RID: 2136 RVA: 0x0003066A File Offset: 0x0002E86A
		public int GetToolMode()
		{
			return this.GetSelectedModeIndex();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00030672 File Offset: 0x0002E872
		public SkillItem[] GetToolModes()
		{
			return this._modesSkillItems;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0003067A File Offset: 0x0002E87A
		public void SetToolMode(int toolMode)
		{
			if (this._modes.Length <= toolMode || toolMode < 0)
			{
				return;
			}
			this.SelectedMode = this._modes[toolMode].Code;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000306A4 File Offset: 0x0002E8A4
		private int GetSelectedModeIndex()
		{
			for (int index = 0; index < this._modes.Length; index++)
			{
				if (this._modes[index].Code == this.SelectedMode)
				{
					return index;
				}
			}
			return 0;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000306E5 File Offset: 0x0002E8E5
		private SkillItem[] GenerateToolModes()
		{
			return Enumerable.ToArray<SkillItem>(Enumerable.Select<ModeConfig, SkillItem>(this._modes, delegate(ModeConfig mode)
			{
				SkillItem skillItemWithIcon = ToolModesUtils.GetSkillItemWithIcon(this._api, mode.Icon, "#FFFFFF");
				skillItemWithIcon.Code = mode.Code;
				skillItemWithIcon.Name = Lang.Get(mode.Name, Array.Empty<object>());
				return skillItemWithIcon;
			}));
		}

		// Token: 0x040003D1 RID: 977
		private readonly ICoreClientAPI _api;

		// Token: 0x040003D2 RID: 978
		private readonly ModeConfig[] _modes;

		// Token: 0x040003D3 RID: 979
		private readonly SkillItem[] _modesSkillItems;
	}
}
