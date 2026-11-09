using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000032 RID: 50
	public static class RadioBlockIndex
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00008FE6 File Offset: 0x000071E6
		public static void RegisterEmitter(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.emitterPositions.Add(pos.Copy());
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009002 File Offset: 0x00007202
		public static void UnregisterEmitter(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.emitterPositions.Remove(pos);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009019 File Offset: 0x00007219
		public static void RegisterMicrophone(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.microphonePositions.Add(pos.Copy());
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009035 File Offset: 0x00007235
		public static void UnregisterMicrophone(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.microphonePositions.Remove(pos);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000904C File Offset: 0x0000724C
		public static IEnumerable<BlockEntityRadioEmitter> GetLoadedEmitters(IWorldAccessor world)
		{
			RadioBlockIndex.<GetLoadedEmitters>d__8 <GetLoadedEmitters>d__ = new RadioBlockIndex.<GetLoadedEmitters>d__8(-2);
			<GetLoadedEmitters>d__.<>3__world = world;
			return <GetLoadedEmitters>d__;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000905C File Offset: 0x0000725C
		public static IEnumerable<BlockEntityRadioMicrophone> GetLoadedMicrophones(IWorldAccessor world)
		{
			RadioBlockIndex.<GetLoadedMicrophones>d__9 <GetLoadedMicrophones>d__ = new RadioBlockIndex.<GetLoadedMicrophones>d__9(-2);
			<GetLoadedMicrophones>d__.<>3__world = world;
			return <GetLoadedMicrophones>d__;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000906C File Offset: 0x0000726C
		public static void RegisterMixingConsole(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.mixingConsolePositions.Add(pos.Copy());
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009088 File Offset: 0x00007288
		public static void UnregisterMixingConsole(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.mixingConsolePositions.Remove(pos);
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000909F File Offset: 0x0000729F
		public static void RegisterSupervisionConsole(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.supervisionConsolePositions.Add(pos.Copy());
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000090BB File Offset: 0x000072BB
		public static void UnregisterSupervisionConsole(BlockPos pos)
		{
			if (pos != null)
			{
				RadioBlockIndex.supervisionConsolePositions.Remove(pos);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000090D2 File Offset: 0x000072D2
		public static IEnumerable<BlockEntityRadioMixingConsole> GetLoadedMixingConsoles(IWorldAccessor world)
		{
			RadioBlockIndex.<GetLoadedMixingConsoles>d__14 <GetLoadedMixingConsoles>d__ = new RadioBlockIndex.<GetLoadedMixingConsoles>d__14(-2);
			<GetLoadedMixingConsoles>d__.<>3__world = world;
			return <GetLoadedMixingConsoles>d__;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000090E2 File Offset: 0x000072E2
		public static IEnumerable<BlockEntityRadioSupervisionConsole> GetLoadedSupervisionConsoles(IWorldAccessor world)
		{
			RadioBlockIndex.<GetLoadedSupervisionConsoles>d__15 <GetLoadedSupervisionConsoles>d__ = new RadioBlockIndex.<GetLoadedSupervisionConsoles>d__15(-2);
			<GetLoadedSupervisionConsoles>d__.<>3__world = world;
			return <GetLoadedSupervisionConsoles>d__;
		}

		// Token: 0x0400007D RID: 125
		private static readonly HashSet<BlockPos> emitterPositions = new HashSet<BlockPos>();

		// Token: 0x0400007E RID: 126
		private static readonly HashSet<BlockPos> microphonePositions = new HashSet<BlockPos>();

		// Token: 0x0400007F RID: 127
		private static readonly HashSet<BlockPos> mixingConsolePositions = new HashSet<BlockPos>();

		// Token: 0x04000080 RID: 128
		private static readonly HashSet<BlockPos> supervisionConsolePositions = new HashSet<BlockPos>();
	}
}
