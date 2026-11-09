using System;
using System.Reflection;
using Vintagestory.API.Common;

namespace Achievements
{
	// Token: 0x02000002 RID: 2
	internal class AchievementsManager
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void RegisterAchievement(string modId, string achievementId, string achievementGraphicCode)
		{
			if (AchievementsManager.registerAchievementMethod == null)
			{
				Assembly a = AchievementsManager.GetAchievementsAssembly();
				if (a == null)
				{
					return;
				}
				AchievementsManager.registerAchievementMethod = a.GetType("Achievements.AchievementsApi").GetMethod("RegisterAchievement");
			}
			AchievementsManager.registerAchievementMethod.Invoke(null, new object[]
			{
				modId,
				achievementId,
				achievementGraphicCode
			});
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B4 File Offset: 0x000002B4
		public static void UnlockAchievementForPlayer(string achievementId, EntityPlayer player)
		{
			if (AchievementsManager.unlockAchievementMethod == null)
			{
				Assembly a = AchievementsManager.GetAchievementsAssembly();
				if (a == null)
				{
					return;
				}
				AchievementsManager.unlockAchievementMethod = a.GetType("Achievements.AchievementsApi").GetMethod("UnlockAchievementForPlayer");
			}
			AchievementsManager.unlockAchievementMethod.Invoke(null, new object[]
			{
				achievementId,
				player
			});
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002114 File Offset: 0x00000314
		public static bool IsAchievementUnlockedForPlayer(string achievementId, EntityPlayer player, bool defaultIfNotInstalled = false)
		{
			if (AchievementsManager.isAchievementUnlockedMethod == null)
			{
				Assembly a = AchievementsManager.GetAchievementsAssembly();
				if (a == null)
				{
					return defaultIfNotInstalled;
				}
				AchievementsManager.isAchievementUnlockedMethod = a.GetType("Achievements.AchievementsApi").GetMethod("IsAchievementUnlockedForPlayer");
			}
			return (bool)AchievementsManager.isAchievementUnlockedMethod.Invoke(null, new object[]
			{
				achievementId,
				player
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002178 File Offset: 0x00000378
		private static Assembly GetAchievementsAssembly()
		{
			if (AchievementsManager.cachedAchievementsAssembly != null)
			{
				return AchievementsManager.cachedAchievementsAssembly;
			}
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (a.GetName().Name == "natsachievements")
				{
					AchievementsManager.cachedAchievementsAssembly = a;
					return AchievementsManager.cachedAchievementsAssembly;
				}
			}
			return null;
		}

		// Token: 0x04000001 RID: 1
		private static Assembly cachedAchievementsAssembly;

		// Token: 0x04000002 RID: 2
		private static MethodInfo registerAchievementMethod;

		// Token: 0x04000003 RID: 3
		private static MethodInfo unlockAchievementMethod;

		// Token: 0x04000004 RID: 4
		private static MethodInfo isAchievementUnlockedMethod;
	}
}
