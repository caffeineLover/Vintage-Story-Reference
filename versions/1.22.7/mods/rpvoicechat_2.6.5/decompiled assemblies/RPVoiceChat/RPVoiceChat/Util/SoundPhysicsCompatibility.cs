using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Util
{
	// Token: 0x0200002B RID: 43
	public static class SoundPhysicsCompatibility
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00008690 File Offset: 0x00006890
		public static void Init(ICoreClientAPI api)
		{
			if (SoundPhysicsCompatibility.initialized)
			{
				return;
			}
			SoundPhysicsCompatibility.initialized = true;
			try
			{
				ModSystem modSystem2;
				if (api == null)
				{
					modSystem2 = null;
				}
				else
				{
					IModLoader modLoader = api.ModLoader;
					modSystem2 = ((modLoader != null) ? modLoader.GetModSystem("soundphysicsadapted.SoundPhysicsAdaptedModSystem") : null);
				}
				ModSystem modSystem = modSystem2;
				if (modSystem != null)
				{
					Type apiType = modSystem.GetType().Assembly.GetType("soundphysicsadapted.SoundPhysicsAPI");
					if (!(apiType == null))
					{
						SoundPhysicsCompatibility.isAvailableProperty = apiType.GetProperty("IsAvailable", 24);
						SoundPhysicsCompatibility.getOcclusionGainHFMethod = apiType.GetMethod("GetOcclusionGainHF", 24, null, new Type[]
						{
							typeof(Vec3d),
							typeof(Vec3d)
						}, null);
						SoundPhysicsCompatibility.installed = (SoundPhysicsCompatibility.isAvailableProperty != null && SoundPhysicsCompatibility.getOcclusionGainHFMethod != null);
						if (SoundPhysicsCompatibility.installed)
						{
							Logger client = Logger.client;
							if (client != null)
							{
								client.Notification("Sound Physics Adapted detected. Voices use its occlusion");
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				SoundPhysicsCompatibility.installed = false;
				Logger client2 = Logger.client;
				if (client2 != null)
				{
					client2.Warning("Failed to integrate with Sound Physics Adapted: " + e.Message);
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000087B4 File Offset: 0x000069B4
		public static bool IsAvailable
		{
			get
			{
				if (!SoundPhysicsCompatibility.installed)
				{
					return false;
				}
				bool result;
				try
				{
					result = (bool)SoundPhysicsCompatibility.isAvailableProperty.GetValue(null);
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000087F4 File Offset: 0x000069F4
		public static float GetOcclusionGainHF(Vec3d speakerPos, Vec3d listenerPos)
		{
			if (!SoundPhysicsCompatibility.installed)
			{
				return 1f;
			}
			float result;
			try
			{
				result = (float)SoundPhysicsCompatibility.getOcclusionGainHFMethod.Invoke(null, new object[]
				{
					speakerPos,
					listenerPos
				});
			}
			catch (Exception e)
			{
				Logger client = Logger.client;
				if (client != null)
				{
					client.Debug("Sound Physics Adapted occlusion query failed: " + e.Message);
				}
				result = 1f;
			}
			return result;
		}

		// Token: 0x0400006B RID: 107
		public const string ModId = "soundphysicsadapted";

		// Token: 0x0400006C RID: 108
		private const string ModSystemName = "soundphysicsadapted.SoundPhysicsAdaptedModSystem";

		// Token: 0x0400006D RID: 109
		private const string ApiTypeName = "soundphysicsadapted.SoundPhysicsAPI";

		// Token: 0x0400006E RID: 110
		private static bool initialized;

		// Token: 0x0400006F RID: 111
		private static bool installed;

		// Token: 0x04000070 RID: 112
		private static PropertyInfo isAvailableProperty;

		// Token: 0x04000071 RID: 113
		private static MethodInfo getOcclusionGainHFMethod;
	}
}
