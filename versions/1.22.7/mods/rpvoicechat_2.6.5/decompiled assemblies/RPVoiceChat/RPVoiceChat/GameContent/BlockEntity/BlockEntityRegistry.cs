using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000F4 RID: 244
	public class BlockEntityRegistry
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x00030F78 File Offset: 0x0002F178
		public static void RegisterBlockEntities(ICoreAPI api)
		{
			api.RegisterBlockEntityClass("ChurchBellPart", typeof(BlockEntityChurchBellPart));
			api.RegisterBlockEntityClass("ChurchBellLayer", typeof(BlockEntityChurchBellLayer));
			api.RegisterBlockEntityClass("BlockEntityTelegraph", typeof(BlockEntityTelegraph));
			api.RegisterBlockEntityClass("BlockEntityConnector", typeof(BlockEntityConnector));
			api.RegisterBlockEntityClass("BlockEntityPrinter", typeof(BlockEntityPrinter));
			api.RegisterBlockEntityClass("BlockEntitySignalLamp", typeof(BlockEntitySignalLamp));
			api.RegisterBlockEntityClass("BlockEntityCarillonBell", typeof(BlockEntityCarillonBell));
			api.RegisterBlockEntityClass("BlockEntityBellHammer", typeof(BlockEntityBellHammer));
			api.RegisterBlockEntityClass("BESoundEmitting", typeof(BESoundEmitting));
			api.RegisterBlockEntityClass("BlockEntityLucerne", typeof(BlockEntityLucerne));
			api.RegisterBlockEntityClass("BlockEntitySwitchboard", typeof(BlockEntitySwitchboard));
			api.RegisterBlockEntityClass("BlockEntityTelephone", typeof(BlockEntityTelephone));
			api.RegisterBlockEntityClass("BlockEntitySpeaker", typeof(BlockEntitySpeaker));
			api.RegisterBlockEntityClass("BlockEntityRadioSupervisionConsole", typeof(BlockEntityRadioSupervisionConsole));
			api.RegisterBlockEntityClass("BlockEntityRadioMicrophone", typeof(BlockEntityRadioMicrophone));
			api.RegisterBlockEntityClass("BlockEntityRadioEmitter", typeof(BlockEntityRadioEmitter));
			api.RegisterBlockEntityClass("BlockEntityRadioAntennaPart", typeof(BlockEntityRadioAntennaPart));
			api.RegisterBlockEntityClass("BlockEntityRadioMixingConsole", typeof(BlockEntityRadioMixingConsole));
			api.RegisterBlockEntityClass("BlockEntityRadioReceiver", typeof(BlockEntityRadioReceiver));
		}
	}
}
