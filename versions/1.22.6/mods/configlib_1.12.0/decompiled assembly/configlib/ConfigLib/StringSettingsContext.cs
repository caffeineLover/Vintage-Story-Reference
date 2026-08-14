using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StringSettingsContext : IContext<string, string>
	{
		// Token: 0x06000153 RID: 339 RVA: 0x000088B0 File Offset: 0x00006AB0
		public StringSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, string>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.String), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value.AsString(null));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008931 File Offset: 0x00006B31
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000893F File Offset: 0x00006B3F
		public string Resolve(string name, params string[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x0400009D RID: 157
		private readonly Dictionary<string, string> _settings;
	}
}
