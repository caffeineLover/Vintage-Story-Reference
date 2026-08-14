using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000A7 RID: 167
	public class BlockRegistry
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x0001B6B0 File Offset: 0x000198B0
		public static void RegisterBlocks(ICoreAPI api)
		{
			api.RegisterBlockClass("soundemittingblock", typeof(SoundEmittingBlock));
			api.RegisterBlockClass("carillonbellblock", typeof(CarillonBellBlock));
			api.RegisterBlockClass("churchbellpart", typeof(ChurchBellPartBlock));
			api.RegisterBlockClass("churchbelllayer", typeof(ChurchBellLayerBlock));
			api.RegisterBlockClass("telegraphblock", typeof(TelegraphBlock));
			api.RegisterBlockClass("connectorblock", typeof(ConnectorBlock));
			api.RegisterBlockClass("printerblock", typeof(PrinterBlock));
			api.RegisterBlockClass("signallampblock", typeof(SignalLampBlock));
			api.RegisterBlockClass("bellhammerblock", typeof(BellHammerBlock));
			api.RegisterBlockClass("lucerneblock", typeof(LucerneBlock));
			api.RegisterBlockClass("switchboardblock", typeof(SwitchboardBlock));
			api.RegisterBlockClass("telephoneblock", typeof(TelephoneBlock));
			api.RegisterBlockClass("speakerblock", typeof(SpeakerBlock));
		}
	}
}
