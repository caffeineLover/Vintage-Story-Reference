using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StringSettingsContext : IContext<string, string>
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00008BCC File Offset: 0x00006DCC
		public StringSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, string>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.String), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value.AsString(null));
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008C4D File Offset: 0x00006E4D
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008C5B File Offset: 0x00006E5B
		public string Resolve(string name, params string[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x040000A0 RID: 160
		private readonly Dictionary<string, string> _settings;
	}
}
