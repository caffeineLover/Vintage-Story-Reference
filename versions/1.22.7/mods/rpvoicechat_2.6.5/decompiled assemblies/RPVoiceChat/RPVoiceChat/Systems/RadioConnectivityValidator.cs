using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000033 RID: 51
	public static class RadioConnectivityValidator
	{
		// Token: 0x0600013C RID: 316 RVA: 0x0000911C File Offset: 0x0000731C
		public static void ValidateNode(BEWireNode node)
		{
			bool flag;
			if (node == null)
			{
				flag = true;
			}
			else
			{
				ICoreAPI api = node.Api;
				flag = (((api != null) ? new EnumAppSide?(api.Side) : default(EnumAppSide?)).GetValueOrDefault() != 1);
			}
			if (flag)
			{
				return;
			}
			BlockEntityRadioMixingConsole mixingConsole = node as BlockEntityRadioMixingConsole;
			if (mixingConsole != null)
			{
				RadioConnectivityValidator.ValidateMixingConsole(mixingConsole);
				return;
			}
			BlockEntityRadioMicrophone microphone = node as BlockEntityRadioMicrophone;
			if (microphone == null)
			{
				return;
			}
			RadioConnectivityValidator.ValidateMicrophone(microphone, null);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009182 File Offset: 0x00007382
		public static void ValidateMixingConsole(BlockEntityRadioMixingConsole mixingConsole)
		{
			if (mixingConsole == null || !mixingConsole.IsOnAir)
			{
				return;
			}
			if (mixingConsole.NetworkUID == 0L || !mixingConsole.HasWiredBroadcastPath())
			{
				mixingConsole.ClearOnAir();
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000091A8 File Offset: 0x000073A8
		public static void ValidateMicrophone(BlockEntityRadioMicrophone microphone, ICoreServerAPI sapi = null)
		{
			if (microphone == null || !microphone.IsTransmitting)
			{
				return;
			}
			if (microphone.NetworkUID == 0L || !RadioWireNetworkHelper.IsRadioWiredNetwork(microphone))
			{
				microphone.ClearTransmission();
				return;
			}
			if (RadioWireNetworkHelper.HasOnAirMixingConsole(microphone))
			{
				return;
			}
			if (sapi == null)
			{
				return;
			}
			if (RadioWiredRouteBuilder.BuildRoutesForWiredNode(sapi, microphone).Count == 0)
			{
				microphone.ClearTransmission();
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000091FC File Offset: 0x000073FC
		public static void ValidateNodesInComponents(IEnumerable<BEWireNode> nodes)
		{
			if (nodes == null)
			{
				return;
			}
			foreach (BEWireNode node in nodes)
			{
				RadioConnectivityValidator.ValidateNode(node);
			}
		}
	}
}
