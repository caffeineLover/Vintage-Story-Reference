using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006C RID: 108
	public class NamedSlider : GuiElementSlider
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000FF78 File Offset: 0x0000E178
		public string name { get; }

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FF80 File Offset: 0x0000E180
		public NamedSlider(ICoreClientAPI capi, string name, ActionConsumable<int, string> extendedCallback, ElementBounds bounds) : base(capi, NamedSlider.WrapCallback(name, extendedCallback), bounds)
		{
			this.name = name;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FF99 File Offset: 0x0000E199
		private static ActionConsumable<int> WrapCallback(string name, ActionConsumable<int, string> extendedCallback)
		{
			return (int val) => extendedCallback.Invoke(val, name);
		}
	}
}
