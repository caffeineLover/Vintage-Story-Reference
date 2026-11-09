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
	internal sealed class NumberSettingsContext : IContext<float, float>
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00008984 File Offset: 0x00006B84
		public NumberSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, float>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => NumberSettingsContext.Supported(entry.Value)), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => NumberSettingsContext.ConvertSetting(entry.Value));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008A05 File Offset: 0x00006C05
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008A13 File Offset: 0x00006C13
		public float Resolve(string name, params float[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008A24 File Offset: 0x00006C24
		private static bool Supported(ConfigSetting setting)
		{
			bool result;
			switch (setting.SettingType)
			{
			case ConfigSettingType.Boolean:
				result = true;
				break;
			case ConfigSettingType.Float:
				result = true;
				break;
			case ConfigSettingType.Integer:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008A60 File Offset: 0x00006C60
		private static float ConvertSetting(ConfigSetting setting)
		{
			float result;
			switch (setting.SettingType)
			{
			case ConfigSettingType.Boolean:
				result = (setting.Value.AsBool(false) > false);
				break;
			case ConfigSettingType.Float:
				result = setting.Value.AsFloat(0f);
				break;
			case ConfigSettingType.Integer:
				result = setting.Value.AsFloat(0f);
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x04000096 RID: 150
		private readonly Dictionary<string, float> _settings;
	}
}
