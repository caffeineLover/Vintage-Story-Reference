using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RPVoiceChat.Util;
using Vintagestory.Common;
using Vintagestory.Server;

namespace RPVoiceChat
{
	// Token: 0x0200001E RID: 30
	internal static class TcpNetServerPatch
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004EC0 File Offset: 0x000030C0
		public static void Patch(Harmony harmony)
		{
			MethodInfo OriginalMethod = AccessTools.Method(typeof(TcpNetServer), "ServerReceivedMessage", null, null);
			MethodInfo TranspilerMethod = AccessTools.Method(typeof(TcpNetServerPatch), "server_ReceivedMessage_Transpiler", null, null);
			harmony.Patch(OriginalMethod, null, null, new HarmonyMethod(TranspilerMethod), null);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004F0C File Offset: 0x0000310C
		public static IEnumerable<CodeInstruction> server_ReceivedMessage_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			int patchIndex = -1;
			List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
			for (int i = 0; i < codes.Count; i++)
			{
				CodeInstruction codeInstr = codes[i];
				if (!(codeInstr.opcode != TcpNetServerPatch.anchor.opcode) && codeInstr.operand == TcpNetServerPatch.anchor.operand)
				{
					patchIndex = i;
					break;
				}
			}
			if (patchIndex == -1)
			{
				Logger.server.Error("Couldn't find transpiler anchor for TcpNetServer patch");
				return instructions;
			}
			codes.InsertRange(patchIndex, TcpNetServerPatch.patch);
			Logger.server.Notification("TcpNetServer was successfully patched");
			return Enumerable.AsEnumerable<CodeInstruction>(codes);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004FA0 File Offset: 0x000031A0
		private static void OnReceivedMessage(NetIncomingMessage msg)
		{
			ConcurrentQueue<NetIncomingMessage> concurrentQueue = TcpNetServerPatch.messages;
			lock (concurrentQueue)
			{
				TcpNetServerPatch.messages.Enqueue(msg);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004FE4 File Offset: 0x000031E4
		public static NetIncomingMessage ReadMessage()
		{
			ConcurrentQueue<NetIncomingMessage> concurrentQueue = TcpNetServerPatch.messages;
			NetIncomingMessage result2;
			lock (concurrentQueue)
			{
				if (TcpNetServerPatch.messages.Count > 0)
				{
					NetIncomingMessage result;
					TcpNetServerPatch.messages.TryDequeue(ref result);
					result2 = result;
				}
				else
				{
					result2 = null;
				}
			}
			return result2;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005040 File Offset: 0x00003240
		// Note: this type is marked as 'beforefieldinit'.
		static TcpNetServerPatch()
		{
			List<CodeInstruction> list = new List<CodeInstruction>();
			list.Add(new CodeInstruction(OpCodes.Ldloc_0, null));
			list.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(TcpNetServerPatch), "OnReceivedMessage", null, null)));
			TcpNetServerPatch.patch = list;
			TcpNetServerPatch.messages = new ConcurrentQueue<NetIncomingMessage>();
		}

		// Token: 0x04000033 RID: 51
		private static readonly CodeInstruction anchor = new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(ConcurrentQueue<NetIncomingMessage>), "Enqueue", null, null));

		// Token: 0x04000034 RID: 52
		private static readonly List<CodeInstruction> patch;

		// Token: 0x04000035 RID: 53
		private static ConcurrentQueue<NetIncomingMessage> messages;
	}
}
