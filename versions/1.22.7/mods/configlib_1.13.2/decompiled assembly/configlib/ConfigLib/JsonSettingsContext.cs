using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class JsonSettingsContext : IContext<JsonObject, JsonObject>
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00008C9C File Offset: 0x00006E9C
		public JsonSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, JsonObject>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.Other), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008D1D File Offset: 0x00006F1D
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008D2B File Offset: 0x00006F2B
		public JsonObject Resolve(string name, params JsonObject[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x040000A5 RID: 165
		private readonly Dictionary<string, JsonObject> _settings;
	}
}
