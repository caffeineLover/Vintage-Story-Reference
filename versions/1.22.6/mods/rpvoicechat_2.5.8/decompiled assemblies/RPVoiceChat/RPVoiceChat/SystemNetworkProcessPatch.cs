using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace RPVoiceChat
{
	// Token: 0x0200001D RID: 29
	internal static class SystemNetworkProcessPatch
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000094 RID: 148 RVA: 0x00004D58 File Offset: 0x00002F58
		// (remove) Token: 0x06000095 RID: 149 RVA: 0x00004D8C File Offset: 0x00002F8C
		public static event Func<int, Packet_CustomPacket, bool> OnProcessInBackground;

		// Token: 0x06000096 RID: 150 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(SystemNetworkProcess), "ProcessInBackground", null, null);
			MethodInfo PostfixMethod = AccessTools.Method(typeof(SystemNetworkProcessPatch), "ProcessInBackground", null, null);
			harmony.Patch(OriginalMethod, null, new HarmonyMethod(PostfixMethod), null, null);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004E0C File Offset: 0x0000300C
		public static void ProcessInBackground(ref bool __result, Packet_Server packet)
		{
			if ((int)SystemNetworkProcessPatch.packetIdField.GetValue(packet) != 55)
			{
				return;
			}
			Packet_CustomPacket customPacket = (Packet_CustomPacket)SystemNetworkProcessPatch.customPacketField.GetValue(packet);
			int channelId = (int)SystemNetworkProcessPatch.channelIdField.GetValue(customPacket);
			Func<int, Packet_CustomPacket, bool> onProcessInBackground = SystemNetworkProcessPatch.OnProcessInBackground;
			if (onProcessInBackground != null && onProcessInBackground.Invoke(channelId, customPacket))
			{
				__result = true;
			}
		}

		// Token: 0x0400002F RID: 47
		private const int _customPacketId = 55;

		// Token: 0x04000030 RID: 48
		private static FieldInfo packetIdField = AccessTools.Field(typeof(Packet_Server), "Id");

		// Token: 0x04000031 RID: 49
		private static FieldInfo customPacketField = AccessTools.Field(typeof(Packet_Server), "CustomPacket");

		// Token: 0x04000032 RID: 50
		private static FieldInfo channelIdField = AccessTools.Field(typeof(Packet_CustomPacket), "ChannelId");
	}
}
