using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.Server;

namespace RPVoiceChat
{
	// Token: 0x0200001B RID: 27
	internal static class NetworkAPIPatch
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000089 RID: 137 RVA: 0x00004B4C File Offset: 0x00002D4C
		// (remove) Token: 0x0600008A RID: 138 RVA: 0x00004B80 File Offset: 0x00002D80
		public static event Func<int, bool> OnHandleCustomPacket;

		// Token: 0x0600008B RID: 139 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(NetworkAPI), "HandleCustomPacket", null, null);
			MethodInfo PrefixMethod = AccessTools.Method(typeof(NetworkAPIPatch), "HandleCustomPacket", null, null);
			harmony.Patch(OriginalMethod, new HarmonyMethod(PrefixMethod), null, null, null);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004C00 File Offset: 0x00002E00
		public static bool HandleCustomPacket(Packet_Client packet)
		{
			Packet_CustomPacket customPacket = (Packet_CustomPacket)NetworkAPIPatch.CustomPacketField.GetValue(packet);
			int channelId = (int)NetworkAPIPatch.ChannelIdField.GetValue(customPacket);
			Func<int, bool> onHandleCustomPacket = NetworkAPIPatch.OnHandleCustomPacket;
			return onHandleCustomPacket == null || !onHandleCustomPacket.Invoke(channelId);
		}

		// Token: 0x0400002B RID: 43
		private static FieldInfo CustomPacketField = AccessTools.Field(typeof(Packet_Client), "CustomPacket");

		// Token: 0x0400002C RID: 44
		private static FieldInfo ChannelIdField = AccessTools.Field(typeof(Packet_CustomPacket), "ChannelId");
	}
}
