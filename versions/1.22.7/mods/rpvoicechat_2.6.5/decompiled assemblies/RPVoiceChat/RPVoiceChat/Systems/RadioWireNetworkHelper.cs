using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000049 RID: 73
	public static class RadioWireNetworkHelper
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x0000BC18 File Offset: 0x00009E18
		public static IEnumerable<BEWireNode> GetReachableWiredNodes(BEWireNode start)
		{
			RadioWireNetworkHelper.<GetReachableWiredNodes>d__0 <GetReachableWiredNodes>d__ = new RadioWireNetworkHelper.<GetReachableWiredNodes>d__0(-2);
			<GetReachableWiredNodes>d__.<>3__start = start;
			return <GetReachableWiredNodes>d__;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BC28 File Offset: 0x00009E28
		public static BlockEntityRadioSupervisionConsole FindSupervisionConsole(BEWireNode start)
		{
			return Enumerable.FirstOrDefault<BlockEntityRadioSupervisionConsole>(Enumerable.OfType<BlockEntityRadioSupervisionConsole>(RadioWireNetworkHelper.GetReachableWiredNodes(start)));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000BC3A File Offset: 0x00009E3A
		public static IEnumerable<BlockEntityRadioEmitter> FindEmitters(BEWireNode start)
		{
			return Enumerable.OfType<BlockEntityRadioEmitter>(RadioWireNetworkHelper.GetReachableWiredNodes(start));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BC47 File Offset: 0x00009E47
		public static IEnumerable<BlockEntityRadioMicrophone> FindMicrophones(BEWireNode start)
		{
			return Enumerable.OfType<BlockEntityRadioMicrophone>(RadioWireNetworkHelper.GetReachableWiredNodes(start));
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BC54 File Offset: 0x00009E54
		public static IEnumerable<BlockEntitySpeaker> FindSpeakers(BEWireNode start)
		{
			return Enumerable.OfType<BlockEntitySpeaker>(RadioWireNetworkHelper.GetReachableWiredNodes(start));
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BC61 File Offset: 0x00009E61
		public static IEnumerable<BlockEntityRadioReceiver> FindReceivers(BEWireNode start)
		{
			return Enumerable.OfType<BlockEntityRadioReceiver>(RadioWireNetworkHelper.GetReachableWiredNodes(start));
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000BC6E File Offset: 0x00009E6E
		public static IEnumerable<BlockEntityRadioMixingConsole> FindMixingConsoles(BEWireNode start)
		{
			return Enumerable.OfType<BlockEntityRadioMixingConsole>(RadioWireNetworkHelper.GetReachableWiredNodes(start));
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public static bool HasOnAirMixingConsole(BEWireNode start)
		{
			if (start == null)
			{
				return false;
			}
			if (Enumerable.Any<BlockEntityRadioMixingConsole>(RadioWireNetworkHelper.FindMixingConsoles(start), (BlockEntityRadioMixingConsole console) => console.IsOnAir))
			{
				return true;
			}
			long networkId = start.NetworkUID;
			return networkId != 0L && Enumerable.Any<RadioRfProgramPresence>(RadioRfPresenceRegistry.GetPrograms(), (RadioRfProgramPresence program) => program.IsOnAir && program.NetworkId == networkId);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000BCEE File Offset: 0x00009EEE
		public static bool IsRadioWiredNetwork(BEWireNode start)
		{
			if (start == null)
			{
				return false;
			}
			return Enumerable.Any<BEWireNode>(RadioWireNetworkHelper.GetReachableWiredNodes(start), delegate(BEWireNode node)
			{
				IWireTypedNode typed = node as IWireTypedNode;
				return typed != null && WireNodeKindRules.IsRadioFamilyEndpoint(typed.WireNodeKind);
			});
		}
	}
}
