using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	internal static class FeatureSwitches
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002A7B File Offset: 0x00000C7B
		public static bool EnableINotifyPropertyChangingSupport
		{
			[MethodImpl(256)]
			get
			{
				return FeatureSwitches.GetConfigurationValue("MVVMTOOLKIT_ENABLE_INOTIFYPROPERTYCHANGING_SUPPORT", ref FeatureSwitches.enableINotifyPropertyChangingSupport, true);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A90 File Offset: 0x00000C90
		private static bool GetConfigurationValue(string propertyName, ref int cachedResult, bool defaultValue)
		{
			if (cachedResult < 0)
			{
				return false;
			}
			if (cachedResult > 0)
			{
				return true;
			}
			bool flag;
			if (!AppContext.TryGetSwitch(propertyName, ref flag))
			{
				flag = defaultValue;
			}
			cachedResult = (flag ? 1 : -1);
			return flag;
		}

		// Token: 0x04000021 RID: 33
		private const string EnableINotifyPropertyChangingSupportPropertyName = "MVVMTOOLKIT_ENABLE_INOTIFYPROPERTYCHANGING_SUPPORT";

		// Token: 0x04000022 RID: 34
		private static int enableINotifyPropertyChangingSupport;
	}
}
