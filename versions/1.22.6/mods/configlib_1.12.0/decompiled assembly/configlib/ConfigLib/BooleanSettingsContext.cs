using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class BooleanSettingsContext : IContext<bool, bool>
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000087E0 File Offset: 0x000069E0
		public BooleanSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, bool>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.Boolean), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value.AsBool(false));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008861 File Offset: 0x00006A61
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000886F File Offset: 0x00006A6F
		public bool Resolve(string name, params bool[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x04000098 RID: 152
		private readonly Dictionary<string, bool> _settings;
	}
}
