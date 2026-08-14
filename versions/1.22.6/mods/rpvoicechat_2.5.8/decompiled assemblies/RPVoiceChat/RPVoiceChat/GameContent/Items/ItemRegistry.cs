using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x0200009C RID: 156
	public class ItemRegistry
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00019F20 File Offset: 0x00018120
		public static void RegisterItems(ICoreAPI api)
		{
			api.RegisterItemClass("soundemitting", typeof(ItemSoundEmitting));
			api.RegisterItemClass("voiceamplifier", typeof(ItemVoiceAmplifier));
			api.RegisterItemClass("handheldradio", typeof(ItemRadio));
			api.RegisterItemClass("telegraphwire", typeof(ItemTelegraphWire));
			api.RegisterItemClass("telegram", typeof(ItemTelegram));
			api.RegisterItemClass("wirecutter", typeof(ItemWireCutter));
		}
	}
}
