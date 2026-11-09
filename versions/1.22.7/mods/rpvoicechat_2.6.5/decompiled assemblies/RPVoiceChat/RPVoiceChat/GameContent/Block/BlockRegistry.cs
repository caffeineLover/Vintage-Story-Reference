using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Block
{
	// Token: 0x020000D3 RID: 211
	public class BlockRegistry
	{
		// Token: 0x06000654 RID: 1620 RVA: 0x000224E4 File Offset: 0x000206E4
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
			api.RegisterBlockClass("radioconsoleblock", typeof(RadioSupervisionConsoleBlock));
			api.RegisterBlockClass("radiomicrophoneblock", typeof(RadioMicrophoneBlock));
			api.RegisterBlockClass("radioemitterblock", typeof(RadioEmitterBlock));
			api.RegisterBlockClass("radioantenna_partblock", typeof(RadioAntennaPartBlock));
			api.RegisterBlockClass("radioantennapartblock", typeof(RadioAntennaPartBlock));
			api.RegisterBlockClass("radiomixingconsoleblock", typeof(RadioMixingConsoleBlock));
			api.RegisterBlockClass("radioreceiverblock", typeof(RadioReceiverBlock));
		}
	}
}
