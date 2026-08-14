using System;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006B RID: 107
	public interface IExtendedGuiElement
	{
		// Token: 0x060002F4 RID: 756
		void Init(string elementKey, ElementBounds bounds, GuiComposer composer);

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FF76 File Offset: 0x0000E176
		void OnAdd(GuiComposer composer)
		{
		}
	}
}
