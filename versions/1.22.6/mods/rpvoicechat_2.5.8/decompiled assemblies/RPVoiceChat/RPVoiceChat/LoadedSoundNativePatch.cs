using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.Client;

namespace RPVoiceChat
{
	// Token: 0x0200001A RID: 26
	internal static class LoadedSoundNativePatch
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000085 RID: 133 RVA: 0x00004A84 File Offset: 0x00002C84
		// (remove) Token: 0x06000086 RID: 134 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public static event Action OnOutputDeviceChange;

		// Token: 0x06000087 RID: 135 RVA: 0x00004AEC File Offset: 0x00002CEC
		public static void Patch(Harmony harmony)
		{
			MethodInfo originalChangeDevice = AccessTools.Method(typeof(LoadedSoundNative), "ChangeOutputDevice", null, null);
			MethodInfo postfixChangeDevice = AccessTools.Method(typeof(LoadedSoundNativePatch), "ChangeOutputDevicePostfix", null, null);
			harmony.Patch(originalChangeDevice, null, new HarmonyMethod(postfixChangeDevice), null, null);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004B38 File Offset: 0x00002D38
		public static void ChangeOutputDevicePostfix()
		{
			Action onOutputDeviceChange = LoadedSoundNativePatch.OnOutputDeviceChange;
			if (onOutputDeviceChange == null)
			{
				return;
			}
			onOutputDeviceChange.Invoke();
		}
	}
}
