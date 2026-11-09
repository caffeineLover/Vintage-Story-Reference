using System;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200008E RID: 142
	public interface IExtendedGuiElement
	{
		// Token: 0x060003B6 RID: 950
		void Init(string elementKey, ElementBounds bounds, GuiComposer composer);

		// Token: 0x060003B7 RID: 951 RVA: 0x0001401E File Offset: 0x0001221E
		void OnAdd(GuiComposer composer)
		{
		}
	}
}
