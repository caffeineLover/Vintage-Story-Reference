using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x020000C8 RID: 200
	public class ItemRegistry
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x00020D44 File Offset: 0x0001EF44
		public static void RegisterItems(ICoreAPI api)
		{
			api.RegisterItemClass("soundemitting", typeof(ItemSoundEmitting));
			api.RegisterItemClass("voiceamplifier", typeof(ItemVoiceAmplifier));
			api.RegisterItemClass("radiotalkie", typeof(ItemRadio));
			api.RegisterItemClass("telegraphwire", typeof(ItemTelegraphWire));
			api.RegisterItemClass("telegram", typeof(ItemTelegram));
			api.RegisterItemClass("wirecutter", typeof(ItemWireCutter));
		}
	}
}
