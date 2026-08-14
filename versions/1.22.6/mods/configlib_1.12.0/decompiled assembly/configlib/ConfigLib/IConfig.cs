using System;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	public interface IConfig
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000022 RID: 34
		string ConfigFilePath { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000023 RID: 35
		int Version { get; }

		// Token: 0x06000024 RID: 36
		void WriteToFile();

		// Token: 0x06000025 RID: 37
		bool ReadFromFile();

		// Token: 0x06000026 RID: 38
		void RestoreToDefaults();

		// Token: 0x06000027 RID: 39
		[return: Nullable(2)]
		ISetting GetSetting(string code);

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000028 RID: 40
		// (remove) Token: 0x06000029 RID: 41
		[Nullable(new byte[]
		{
			2,
			1
		})]
		event Action<ISetting> SettingChanged;

		// Token: 0x0600002A RID: 42
		void AssignSettingsValues(object target);
	}
}
