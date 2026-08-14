using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class JsonSettingsContext : IContext<JsonObject, JsonObject>
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00008980 File Offset: 0x00006B80
		public JsonSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, JsonObject>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.SettingType == ConfigSettingType.Other), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => entry.Value.Value);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008A01 File Offset: 0x00006C01
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008A0F File Offset: 0x00006C0F
		public JsonObject Resolve(string name, params JsonObject[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x040000A2 RID: 162
		private readonly Dictionary<string, JsonObject> _settings;
	}
}
