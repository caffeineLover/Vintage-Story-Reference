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
	internal sealed class BooleanSettingsContext : IContext<bool, bool>
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00008AFC File Offset: 0x00006CFC
		public BooleanSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, bool>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.Boolean), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value.AsBool(false));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008B7D File Offset: 0x00006D7D
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008B8B File Offset: 0x00006D8B
		public bool Resolve(string name, params bool[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x0400009B RID: 155
		private readonly Dictionary<string, bool> _settings;
	}
}
