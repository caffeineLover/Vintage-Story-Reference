using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ConfigLib
{
	// Token: 0x02000009 RID: 9
	[NullableContext(2)]
	public interface IConfigProvider
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000015 RID: 21
		// (remove) Token: 0x06000016 RID: 22
		event Action ConfigWindowClosed;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000017 RID: 23
		// (remove) Token: 0x06000018 RID: 24
		event Action ConfigWindowOpened;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000019 RID: 25
		// (remove) Token: 0x0600001A RID: 26
		event Action ConfigsLoaded;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001B RID: 27
		[Nullable(1)]
		IEnumerable<string> Domains { [NullableContext(1)] get; }

		// Token: 0x0600001C RID: 28
		[NullableContext(1)]
		[return: Nullable(2)]
		IConfig GetConfig(string domain);

		// Token: 0x0600001D RID: 29
		[NullableContext(1)]
		[return: Nullable(2)]
		ISetting GetSetting(string domain, string code);

		// Token: 0x0600001E RID: 30
		[NullableContext(1)]
		void RegisterCustomConfig(string domain, Action<string, ControlButtons> drawDelegate);

		// Token: 0x0600001F RID: 31
		[NullableContext(1)]
		void RegisterCustomConfig(string domain, Func<string, ControlButtons, ControlButtons> drawDelegate);

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000020 RID: 32
		// (remove) Token: 0x06000021 RID: 33
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
