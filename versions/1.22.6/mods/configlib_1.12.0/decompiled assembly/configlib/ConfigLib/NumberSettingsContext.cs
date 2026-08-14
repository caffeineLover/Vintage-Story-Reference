using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class NumberSettingsContext : IContext<float, float>
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00008668 File Offset: 0x00006868
		public NumberSettingsContext(Dictionary<string, ConfigSetting> settings)
		{
			this._settings = Enumerable.ToDictionary<KeyValuePair<string, ConfigSetting>, string, float>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(settings, (KeyValuePair<string, ConfigSetting> entry) => NumberSettingsContext.Supported(entry.Value)), (KeyValuePair<string, ConfigSetting> entry) => entry.Key, (KeyValuePair<string, ConfigSetting> entry) => NumberSettingsContext.ConvertSetting(entry.Value));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000086E9 File Offset: 0x000068E9
		public bool Resolvable(string name)
		{
			return this._settings.ContainsKey(name);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000086F7 File Offset: 0x000068F7
		public float Resolve(string name, params float[] arguments)
		{
			return this._settings[name];
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00008708 File Offset: 0x00006908
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

		// Token: 0x06000145 RID: 325 RVA: 0x00008744 File Offset: 0x00006944
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

		// Token: 0x04000093 RID: 147
		private readonly Dictionary<string, float> _settings;
	}
}
