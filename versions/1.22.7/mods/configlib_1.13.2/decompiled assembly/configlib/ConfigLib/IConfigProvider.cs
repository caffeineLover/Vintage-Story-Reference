using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x0200000B RID: 11
	[NullableContext(2)]
	public interface IConfigProvider
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003A RID: 58
		// (remove) Token: 0x0600003B RID: 59
		event Action ConfigWindowClosed;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600003C RID: 60
		// (remove) Token: 0x0600003D RID: 61
		event Action ConfigWindowOpened;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600003E RID: 62
		// (remove) Token: 0x0600003F RID: 63
		event Action ConfigsLoaded;

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64
		[Nullable(1)]
		IEnumerable<string> Domains { [NullableContext(1)] get; }

		// Token: 0x06000041 RID: 65
		[NullableContext(1)]
		[return: Nullable(2)]
		IConfig GetConfig(string domain);

		// Token: 0x06000042 RID: 66
		[NullableContext(1)]
		[return: Nullable(2)]
		ISetting GetSetting(string domain, string code);

		// Token: 0x06000043 RID: 67
		[NullableContext(1)]
		void RegisterCustomConfig(string domain, Action<string, ControlButtons> drawDelegate);

		// Token: 0x06000044 RID: 68
		[NullableContext(1)]
		void RegisterCustomConfig(string domain, Func<string, ControlButtons, ControlButtons> drawDelegate);

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000045 RID: 69
		// (remove) Token: 0x06000046 RID: 70
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		event Action<string, IConfig, ISetting> SettingChanged;
	}
}
