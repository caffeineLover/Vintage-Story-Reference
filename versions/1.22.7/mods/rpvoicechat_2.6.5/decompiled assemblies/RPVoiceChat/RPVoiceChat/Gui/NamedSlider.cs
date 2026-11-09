using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200008F RID: 143
	public class NamedSlider : GuiElementSlider
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00014020 File Offset: 0x00012220
		public string name { get; }

		// Token: 0x060003B9 RID: 953 RVA: 0x00014028 File Offset: 0x00012228
		public NamedSlider(ICoreClientAPI capi, string name, ActionConsumable<int, string> extendedCallback, ElementBounds bounds) : base(capi, NamedSlider.WrapCallback(name, extendedCallback), bounds)
		{
			this.name = name;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014041 File Offset: 0x00012241
		private static ActionConsumable<int> WrapCallback(string name, ActionConsumable<int, string> extendedCallback)
		{
			return (int val) => extendedCallback.Invoke(val, name);
		}
	}
}
