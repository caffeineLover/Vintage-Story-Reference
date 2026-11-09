using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	public interface IConfig
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000047 RID: 71
		string ConfigFilePath { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000048 RID: 72
		int Version { get; }

		// Token: 0x06000049 RID: 73
		void WriteToFile();

		// Token: 0x0600004A RID: 74
		bool ReadFromFile();

		// Token: 0x0600004B RID: 75
		void RestoreToDefaults();

		// Token: 0x0600004C RID: 76
		[return: Nullable(2)]
		ISetting GetSetting(string code);

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600004D RID: 77
		// (remove) Token: 0x0600004E RID: 78
		[Nullable(new byte[]
		{
			2,
			1
		})]
		event Action<ISetting> SettingChanged;

		// Token: 0x0600004F RID: 79
		void AssignSettingsValues(object target);
	}
}
