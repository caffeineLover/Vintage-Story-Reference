using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000BB RID: 187
	public class BlockEntityRegistry
	{
		// Token: 0x060006B0 RID: 1712 RVA: 0x00025B60 File Offset: 0x00023D60
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
		}
	}
}
