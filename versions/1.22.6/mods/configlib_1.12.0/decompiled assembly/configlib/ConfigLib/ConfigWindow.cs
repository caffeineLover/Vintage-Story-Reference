using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using ConfigLib.Formatting;
using ImGuiNET;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using VSImGui;

namespace ConfigLib
{
	// Token: 0x02000048 RID: 72
	[NullableContext(1)]
	[Nullable(0)]
	internal class ConfigWindow
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x0000AF90 File Offset: 0x00009190
		public ConfigWindow(ICoreClientAPI api)
		{
			this._api = api;
			this._configsSystem = api.ModLoader.GetModSystem<ConfigLibModSystem>(true);
			this._domains = this._configsSystem.GetDomains();
			this._custom = (this._configsSystem.GetCustomConfigs() ?? new Dictionary<string, Func<string, ControlButtons, ControlButtons>>());
			foreach (string mod in this._domains)
			{
				this._mods.Add(mod);
			}
			foreach (string mod2 in this._custom.Keys)
			{
				if (!this._mods.Contains(mod2))
				{
					this._mods.Add(mod2);
				}
			}
			this._style = new Style
			{
				ColorBackgroundMenuBar = new ValueTuple<float, float, float, float>(0f, 0f, 0f, 0f),
				BorderFrame = 0f
			};
			this._redButton = new Style
			{
				ColorButton = new Value4(0.6f, 0.4f, 0.4f, 1f),
				ColorButtonHovered = new Value4(1f, 0.5f, 0.5f, 1f)
			};
			this._greenButton = new Style
			{
				ColorButton = new Value4(0.4f, 0.6f, 0.4f, 1f),
				ColorButtonHovered = new Value4(0.6f, 1f, 0.5f, 1f)
			};
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public unsafe bool Draw()
		{
			this._nextId = 0L;
			bool opened = true;
			this._controlButtons.Reset();
			this.UpdateCustomMods();
			using (new StyleApplier(this._style))
			{
				ImGui.SetNextWindowSizeConstraints(new Vector2(600f, 600f), new Vector2(1000f, 2000f));
				ImGuiWindowFlags flags = 1024;
				if (this._unsavedChanges)
				{
					flags |= 1048576;
				}
				ImGui.Begin(MemoryExtensions.AsSpan("Configs##configlib"), ref opened, flags);
				if (*ImGui.GetIO().KeysDown[526] && ImGui.IsWindowFocused())
				{
					ImGui.End();
					this.SaveAll();
					return false;
				}
				this.DrawMenuBar();
				this.DrawConfigList();
				ImGui.End();
			}
			if (!opened)
			{
				this.SaveAll();
			}
			return opened;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B294 File Offset: 0x00009494
		private void DrawMenuBar()
		{
			if (!ImGui.BeginMenuBar())
			{
				return;
			}
			if (!this._unsavedChanges)
			{
				ImGui.BeginDisabled();
			}
			if (ImGui.MenuItem(MemoryExtensions.AsSpan("Save All")))
			{
				this._controlButtons.Save = true;
				this.SaveAll();
			}
			if (!this._unsavedChanges)
			{
				ImGui.EndDisabled();
			}
			ConfigWindow.DrawItemHint("Saves all configs to files.");
			if (ImGui.MenuItem(MemoryExtensions.AsSpan("Restore All")))
			{
				this._controlButtons.Restore = true;
				this.RestoreAll();
			}
			ConfigWindow.DrawItemHint("Retrieves values from config files.");
			ImGui.EndMenuBar();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B324 File Offset: 0x00009524
		private void SaveAll()
		{
			foreach (string domain in this._domains)
			{
				Config config = this._configsSystem.GetConfigImpl(domain);
				if (this._api.IsSinglePlayer && config != null)
				{
					config.WriteToFile();
				}
			}
			this._unsavedDomains.Clear();
			this._unsavedChanges = false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B3A0 File Offset: 0x000095A0
		private void RestoreAll()
		{
			if (!this._api.IsSinglePlayer)
			{
				return;
			}
			foreach (string domain in this._domains)
			{
				Config configImpl = this._configsSystem.GetConfigImpl(domain);
				if (configImpl != null)
				{
					configImpl.ReadFromFile();
				}
			}
			this.SetUnsavedChanges();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B414 File Offset: 0x00009614
		private void UpdateCustomMods()
		{
			foreach (string mod in this._custom.Keys)
			{
				if (!this._mods.Contains(mod))
				{
					this._mods.Add(mod);
				}
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B480 File Offset: 0x00009680
		private void DrawConfigList()
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Mods configs##configlib"), MemoryExtensions.AsSpan("filter (supports wildcards)"), ref this._filter, 100U);
			string[] domains;
			string[] names;
			this.FilterMods(StyleEditor.WildCardToRegular(this._filter), out domains, out names);
			ImGui.ListBox(MemoryExtensions.AsSpan("##modslist.configlib"), ref this._currentIndex, names, domains.Length, 5);
			ImGui.NewLine();
			float height = ImGui.GetWindowHeight() - ImGui.GetCursorPosY() - 20f;
			ImGui.BeginChild(MemoryExtensions.AsSpan("##configlibdomainconfig"), new Vector2(0f, height), true, 1024);
			if (domains.Length > this._currentIndex)
			{
				this.DrawDomainTab(domains[this._currentIndex]);
			}
			ImGui.EndChild();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B538 File Offset: 0x00009738
		private void SaveSettings(string domain)
		{
			if (!Enumerable.Contains<string>(this._domains, domain))
			{
				return;
			}
			Config configImpl = this._configsSystem.GetConfigImpl(domain);
			if (configImpl != null)
			{
				configImpl.WriteToFile();
			}
			if (this._unsavedDomains.Contains(this._currentIndex))
			{
				this._unsavedDomains.Remove(this._currentIndex);
			}
			if (!Enumerable.Any<int>(this._unsavedDomains))
			{
				this._unsavedChanges = false;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B5A4 File Offset: 0x000097A4
		private void RestoreSettings(string domain)
		{
			if (!Enumerable.Contains<string>(this._domains, domain))
			{
				return;
			}
			Config configImpl = this._configsSystem.GetConfigImpl(domain);
			if (configImpl != null)
			{
				configImpl.ReadFromFile();
			}
			this.SetUnsavedChanges();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B5D3 File Offset: 0x000097D3
		private void DefaultSettings(string domain)
		{
			if (!Enumerable.Contains<string>(this._domains, domain))
			{
				return;
			}
			Config configImpl = this._configsSystem.GetConfigImpl(domain);
			if (configImpl != null)
			{
				configImpl.RestoreToDefaults();
			}
			this.SetUnsavedChanges();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B604 File Offset: 0x00009804
		private string Title(string name)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(12, 2);
			defaultInterpolatedStringHandler.AppendFormatted(name);
			defaultInterpolatedStringHandler.AppendLiteral("##configlib:");
			long nextId = this._nextId;
			this._nextId = nextId + 1L;
			defaultInterpolatedStringHandler.AppendFormatted<long>(nextId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B650 File Offset: 0x00009850
		private void FilterMods(string filter, out string[] domains, out string[] names)
		{
			domains = Enumerable.ToArray<string>(this._mods);
			names = Enumerable.ToArray<string>(Enumerable.Select<string, string>(this._mods, new Func<string, string>(this.GetModName)));
			if (filter == "")
			{
				return;
			}
			List<string> newDomains = new List<string>();
			List<string> newNames = new List<string>();
			for (int index = 0; index < domains.Length; index++)
			{
				if (StyleEditor.Match(filter, names[index]))
				{
					newDomains.Add(domains[index]);
					newNames.Add(names[index]);
				}
			}
			domains = newDomains.ToArray();
			names = newNames.ToArray();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000B6E4 File Offset: 0x000098E4
		private string GetModName(string domain)
		{
			Config configImpl = this._configsSystem.GetConfigImpl(domain);
			string modName = (configImpl != null) ? configImpl.ModName : null;
			if (modName == null)
			{
				try
				{
					Mod mod = this._api.ModLoader.GetMod(domain);
					string text;
					if (mod == null)
					{
						text = null;
					}
					else
					{
						ModInfo info = mod.Info;
						text = ((info != null) ? info.Name : null);
					}
					modName = (text ?? Lang.Get(domain, Array.Empty<object>()));
				}
				catch
				{
				}
			}
			return modName ?? domain;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000B764 File Offset: 0x00009964
		private void DrawDomainTab(string domain)
		{
			if (ImGui.BeginMenuBar())
			{
				if (this._confirmDefaults)
				{
					ImGui.Text(MemoryExtensions.AsSpan("Defaults:"));
					ImGui.SameLine();
					using (new StyleApplier(this._greenButton))
					{
						if (ImGui.Button(MemoryExtensions.AsSpan("Confirm")))
						{
							this._controlButtons.Defaults = true;
							this._confirmDefaults = false;
							if (this._api.IsSinglePlayer)
							{
								this.DefaultSettings(domain);
							}
						}
					}
					using (new StyleApplier(this._redButton))
					{
						if (ImGui.Button(MemoryExtensions.AsSpan("Cancel")))
						{
							this._confirmDefaults = false;
						}
						goto IL_200;
					}
				}
				if (!this._customConfig && !this._unsavedDomains.Contains(this._currentIndex))
				{
					ImGui.BeginDisabled();
				}
				if (this._visibleControlButtons.Save && ImGui.MenuItem(MemoryExtensions.AsSpan("Save")))
				{
					this._controlButtons.Save = true;
					this.SaveSettings(domain);
					this._confirmDefaults = false;
				}
				if (!this._customConfig && !this._unsavedDomains.Contains(this._currentIndex))
				{
					ImGui.EndDisabled();
				}
				ConfigWindow.DrawItemHint("Saves changes to config file.");
				if (this._visibleControlButtons.Restore && ImGui.MenuItem(MemoryExtensions.AsSpan("Restore")))
				{
					this._controlButtons.Restore = true;
					this.RestoreSettings(domain);
					this._confirmDefaults = false;
				}
				ConfigWindow.DrawItemHint("Retrieves values from config file.");
				if (!this._custom.ContainsKey(domain))
				{
					ImGui.BeginDisabled();
				}
				if (this._visibleControlButtons.Reload && ImGui.MenuItem(MemoryExtensions.AsSpan("Reload")))
				{
					this._controlButtons.Reload = true;
					this._confirmDefaults = false;
				}
				ConfigWindow.DrawItemHint("Applies settings changes (if the mod supports this feature)");
				if (!this._custom.ContainsKey(domain))
				{
					ImGui.EndDisabled();
				}
				if (this._visibleControlButtons.Defaults && ImGui.MenuItem(MemoryExtensions.AsSpan("Defaults")))
				{
					this._confirmDefaults = true;
				}
				ConfigWindow.DrawItemHint("Sets settings to default values");
				IL_200:
				ImGui.EndMenuBar();
			}
			if (Enumerable.Contains<string>(this._domains, domain))
			{
				Config config = this._configsSystem.GetConfigImpl(domain);
				if (config != null)
				{
					this._visibleControlButtons = new ControlButtons(true);
					ImGui.Text(MemoryExtensions.AsSpan("To apply changes press 'Save' and re-enter the world."));
					if (!this._api.IsSinglePlayer && !this._api.World.Player.HasPrivilege(Privilege.controlserver))
					{
						ImGui.Text(MemoryExtensions.AsSpan("Only client side settings are available for edit."));
					}
					ImGui.Separator();
					this.DrawModConfig(config);
				}
				else
				{
					ImGui.Text(MemoryExtensions.AsSpan("\nConfig is unavailable\n"));
				}
			}
			this._customConfig = false;
			if (this._custom.ContainsKey(domain))
			{
				ImGui.Separator();
				Func<string, ControlButtons, ControlButtons> func = this._custom[domain];
				ControlButtons visibleControlButtons;
				if (func == null)
				{
					visibleControlButtons = new ControlButtons(true);
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(10, 1);
					defaultInterpolatedStringHandler.AppendLiteral("configlib:");
					long nextId = this._nextId;
					this._nextId = nextId + 1L;
					defaultInterpolatedStringHandler.AppendFormatted<long>(nextId);
					visibleControlButtons = func.Invoke(defaultInterpolatedStringHandler.ToStringAndClear(), this._controlButtons);
				}
				this._visibleControlButtons = visibleControlButtons;
				this._customConfig = true;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		private void DrawModConfig(Config config)
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Search##settings"), MemoryExtensions.AsSpan("filter (supports wildcards)"), ref this._settingsFilter, 100U);
			ImGui.Separator();
			string filter = (this._settingsFilter == "") ? ".*" : StyleEditor.WildCardToRegular(this._settingsFilter);
			this._disabledHeaders.Clear();
			float currentHeader = 0f;
			bool hasSettings = false;
			float num;
			IConfigBlock configBlock;
			foreach (KeyValuePair<float, IConfigBlock> keyValuePair in config.ConfigBlocks)
			{
				keyValuePair.Deconstruct(ref num, ref configBlock);
				float weight = num;
				IConfigBlock configBlock2 = configBlock;
				ConfigSetting setting = configBlock2 as ConfigSetting;
				if (setting != null && !setting.Hide && (StyleEditor.Match(filter, setting.YamlCode) || StyleEditor.Match(filter, setting.InGui ?? setting.YamlCode)))
				{
					hasSettings = true;
				}
				IFormattingBlock formatting = configBlock2 as IFormattingBlock;
				if (formatting != null && (formatting.Collapsible || formatting.StopCollapsible))
				{
					if (!hasSettings && currentHeader != 0f)
					{
						this._disabledHeaders.Add(currentHeader);
					}
					currentHeader = weight;
					hasSettings = false;
				}
			}
			if (!hasSettings && currentHeader != 0f)
			{
				this._disabledHeaders.Add(currentHeader);
			}
			bool collapsed = false;
			foreach (KeyValuePair<float, IConfigBlock> keyValuePair in config.ConfigBlocks)
			{
				keyValuePair.Deconstruct(ref num, ref configBlock);
				float weight2 = num;
				IConfigBlock configBlock3 = configBlock;
				ConfigSetting setting2 = configBlock3 as ConfigSetting;
				if (setting2 != null && !collapsed && !setting2.Hide && (StyleEditor.Match(filter, setting2.YamlCode) || StyleEditor.Match(filter, setting2.InGui ?? setting2.YamlCode)))
				{
					this.DrawSetting(setting2, config.Domain);
				}
				IFormattingBlock formatting2 = configBlock3 as IFormattingBlock;
				if (formatting2 != null)
				{
					if (formatting2.Collapsible)
					{
						if (this._disabledHeaders.Contains(weight2))
						{
							ImGui.BeginDisabled();
						}
						collapsed = !formatting2.Draw(weight2.ToString());
						if (this._disabledHeaders.Contains(weight2))
						{
							ImGui.EndDisabled();
						}
					}
					else if (formatting2.StopCollapsible)
					{
						collapsed = false;
						formatting2.Draw(weight2.ToString());
					}
					else if (!collapsed)
					{
						formatting2.Draw(weight2.ToString());
					}
				}
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000BD20 File Offset: 0x00009F20
		private void DrawSetting(ConfigSetting setting, string domain)
		{
			string name = setting.InGui ?? setting.YamlCode;
			if (!this._api.IsSinglePlayer && !setting.ClientSide && !this._api.World.Player.HasPrivilege(Privilege.controlserver))
			{
				ImGui.BeginDisabled();
			}
			if (ImGui.Button(MemoryExtensions.AsSpan("~##" + name)))
			{
				if (setting.Validation != null && setting.Validation.Mapping != null)
				{
					setting.MappingKey = setting.DefaultValue.AsString(null);
					setting.Value = setting.Validation.Mapping[setting.MappingKey].Clone();
				}
				else
				{
					setting.Value = setting.DefaultValue.Clone();
				}
				this.SetUnsavedChanges();
			}
			string hint = "";
			if (setting.ClientSide)
			{
				hint += "(client side) ";
			}
			string text = hint;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Reset to default value: ");
			defaultInterpolatedStringHandler.AppendFormatted<JsonObject>(setting.DefaultValue);
			hint = text + defaultInterpolatedStringHandler.ToStringAndClear();
			ConfigWindow.DrawItemHint(hint);
			ImGui.SameLine();
			if (setting.Link != "")
			{
				this.DrawLink(setting, setting.YamlCode);
				ImGui.SameLine();
			}
			ImGui.PushItemWidth(300f);
			if (setting.Validation != null)
			{
				this.DrawValidatedSetting(name, setting, domain);
			}
			else
			{
				switch (setting.SettingType)
				{
				case ConfigSettingType.Boolean:
					this.DrawBooleanSetting(name, setting);
					break;
				case ConfigSettingType.Float:
					this.DrawFloatSetting(name, setting);
					break;
				case ConfigSettingType.Integer:
					this.DrawIntegerSetting(name, setting);
					break;
				case ConfigSettingType.String:
					this.DrawStringSetting(name, setting);
					break;
				case ConfigSettingType.Color:
					this.DrawColorSetting(name, setting);
					break;
				default:
					ImGui.TextDisabled(MemoryExtensions.AsSpan(setting.YamlCode + ": unavailable"));
					break;
				}
			}
			ConfigWindow.DrawHint(setting);
			ImGui.PopItemWidth();
			if (!this._api.IsSinglePlayer && !setting.ClientSide && !this._api.World.Player.HasPrivilege(Privilege.controlserver))
			{
				ImGui.EndDisabled();
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000BF38 File Offset: 0x0000A138
		private static void DrawHint(ConfigSetting setting)
		{
			if (setting.Comment == null)
			{
				return;
			}
			ImGui.SameLine();
			ImGui.TextDisabled(MemoryExtensions.AsSpan("(?)"));
			if (ImGui.BeginItemTooltip())
			{
				ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
				ImGui.TextUnformatted(MemoryExtensions.AsSpan(setting.Comment));
				ImGui.PopTextWrapPos();
				ImGui.EndTooltip();
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000BF94 File Offset: 0x0000A194
		private void DrawLink(ConfigSetting setting, string name)
		{
			ImGui.SameLine();
			if (ImGui.Button(MemoryExtensions.AsSpan("link##" + name)))
			{
				this._api.Gui.OpenLink(setting.Link);
			}
			ConfigWindow.DrawItemHint("open in browser: " + setting.Link);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		private void DrawValidatedSetting(string name, ConfigSetting setting, string domain)
		{
			Validation validation = setting.Validation;
			bool flag = ((validation != null) ? validation.Mapping : null) != null;
			Validation validation2 = setting.Validation;
			bool values = ((validation2 != null) ? validation2.Values : null) != null;
			Validation validation3 = setting.Validation;
			bool flag2;
			if (((validation3 != null) ? validation3.Minimum : null) == null)
			{
				Validation validation4 = setting.Validation;
				flag2 = (((validation4 != null) ? validation4.Maximum : null) != null);
			}
			else
			{
				flag2 = true;
			}
			bool minmax = flag2;
			if (flag)
			{
				this.DrawMappingSetting(name, setting, domain);
				return;
			}
			if (values)
			{
				this.DrawValuesSetting(name, setting);
				return;
			}
			if (minmax)
			{
				this.DrawMinMaxSetting(name, setting);
				return;
			}
			ImGui.Text(MemoryExtensions.AsSpan(name + ": unavailable"));
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C088 File Offset: 0x0000A288
		private void DrawMappingSetting(string name, ConfigSetting setting, string domain)
		{
			Validation validation = setting.Validation;
			if (((validation != null) ? validation.Mapping : null) == null || setting.MappingKey == null)
			{
				return;
			}
			string[] values = Enumerable.ToArray<string>(setting.Validation.Mapping.Keys);
			string[] translatedValues = Enumerable.ToArray<string>(Enumerable.Select<string, string>(values, (string key) => Lang.GetIfExists(domain + ":mappingkey-" + key, Array.Empty<object>()) ?? key));
			setting.MappingKey = values[this.DrawComboBox(this.Title(name), Lang.GetIfExists(domain + ":mappingkey-" + setting.MappingKey, Array.Empty<object>()) ?? setting.MappingKey, translatedValues, setting)];
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000C130 File Offset: 0x0000A330
		private void DrawValuesSetting(string name, ConfigSetting setting)
		{
			Validation validation = setting.Validation;
			if (((validation != null) ? validation.Values : null) == null || setting.Validation.Values.Count == 0)
			{
				return;
			}
			string[] values = Enumerable.ToArray<string>(Enumerable.Select<JsonObject, string>(setting.Validation.Values, (JsonObject value) => value.Token.ToString()));
			string value2 = setting.Value.ToString();
			int index = this.DrawComboBox(this.Title(name), value2, values, setting);
			if (index < setting.Validation.Values.Count)
			{
				setting.Value = setting.Validation.Values[index];
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		private void DrawMinMaxSetting(string name, ConfigSetting setting)
		{
			JTokenType type = setting.Value.Token.Type;
			if (type == 6)
			{
				this.DrawIntegerMinMaxSetting(name, setting);
				return;
			}
			if (type != 7)
			{
				ImGui.Text(MemoryExtensions.AsSpan(name + ": unavailable"));
				return;
			}
			this.DrawFloatMinMaxSetting(name, setting);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C230 File Offset: 0x0000A430
		private void DrawIntegerMinMaxSetting(string name, ConfigSetting setting)
		{
			if (((setting != null) ? setting.Validation : null) == null)
			{
				return;
			}
			int value = setting.Value.AsInt(0);
			JsonObject minimum = setting.Validation.Minimum;
			int? min = (minimum != null) ? new int?(minimum.AsInt(0)) : default(int?);
			JsonObject maximum = setting.Validation.Maximum;
			int? max = (maximum != null) ? new int?(maximum.AsInt(0)) : default(int?);
			JsonObject step2 = setting.Validation.Step;
			int? step = (step2 != null) ? new int?(step2.AsInt(0)) : default(int?);
			int num = value;
			if (min != null && max != null)
			{
				ImGuiSliderFlags flags = 0;
				if (setting.Logarithmic)
				{
					flags |= 32;
				}
				ImGui.PushItemWidth(70f);
				ImGui.DragInt(MemoryExtensions.AsSpan(this.Title("##" + name + "edit")), ref value, (float)step.GetValueOrDefault(1), min.Value, max.Value);
				ImGui.SameLine();
				ImGui.PopItemWidth();
				if (value < min.Value)
				{
					value = min.Value;
				}
				if (value > max.Value)
				{
					value = max.Value;
				}
				ImGui.PushItemWidth(223f);
				ImGui.SliderInt(MemoryExtensions.AsSpan(this.Title(name)), ref value, min.Value, max.Value, MemoryExtensions.AsSpan(""), flags);
				ImGui.PopItemWidth();
				ConfigWindow.StepInt(ref value, min, max, step);
			}
			else
			{
				ImGui.DragInt(MemoryExtensions.AsSpan(this.Title(name)), ref value, 1f, min.GetValueOrDefault(int.MinValue), max.GetValueOrDefault(int.MaxValue));
				if (min != null && value < min.Value)
				{
					value = min.Value;
				}
				if (max != null && value > max.Value)
				{
					value = max.Value;
				}
				ConfigWindow.StepInt(ref value, min, max, step);
			}
			if (num != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue((long)value));
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000C448 File Offset: 0x0000A648
		private static void StepInt(ref int value, int? min, int? max, int? step)
		{
			if (step != null)
			{
				int? num = step;
				int num2 = 0;
				if (!(num.GetValueOrDefault() == num2 & num != null))
				{
					int stepValue = step.Value;
					int stepPoint = min ?? max.GetValueOrDefault();
					value = (value - stepPoint) / stepValue * stepValue + stepPoint;
					return;
				}
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		private void DrawFloatMinMaxSetting(string name, ConfigSetting setting)
		{
			if (((setting != null) ? setting.Validation : null) == null)
			{
				return;
			}
			float value = setting.Value.AsFloat(0f);
			JsonObject minimum = setting.Validation.Minimum;
			float? min = (minimum != null) ? new float?(minimum.AsFloat(0f)) : default(float?);
			JsonObject maximum = setting.Validation.Maximum;
			float? max = (maximum != null) ? new float?(maximum.AsFloat(0f)) : default(float?);
			JsonObject step2 = setting.Validation.Step;
			float? step = (step2 != null) ? new float?(step2.AsFloat(0f)) : default(float?);
			if (step != null)
			{
				float? num = step % (float)1;
				float num2 = 0f;
				if (num.GetValueOrDefault() == num2 & num != null)
				{
					if (min != null)
					{
						num = min % (float)1;
						num2 = 0f;
						if (num.GetValueOrDefault() == num2 & num != null)
						{
							goto IL_19D;
						}
					}
					if (max == null)
					{
						goto IL_1A6;
					}
					num = max % (float)1;
					num2 = 0f;
					if (!(num.GetValueOrDefault() == num2 & num != null))
					{
						goto IL_1A6;
					}
					IL_19D:
					this.DrawIntegerSetting(name, setting);
					return;
				}
			}
			IL_1A6:
			float num3 = value;
			if (min != null && max != null)
			{
				ImGuiSliderFlags flags = 0;
				if (setting.Logarithmic)
				{
					flags |= 32;
				}
				ImGui.PushItemWidth(70f);
				ImGui.DragFloat(MemoryExtensions.AsSpan(this.Title("##" + name + "edit")), ref value, 1f, min.Value, max.Value);
				ImGui.SameLine();
				ImGui.PopItemWidth();
				if (value < min.Value)
				{
					value = min.Value;
				}
				if (value > max.Value)
				{
					value = max.Value;
				}
				ImGui.PushItemWidth(223f);
				ImGui.SliderFloat(MemoryExtensions.AsSpan(this.Title(name)), ref value, min.Value, max.Value, MemoryExtensions.AsSpan(""), flags);
				ImGui.PopItemWidth();
				ConfigWindow.StepFloat(ref value, min, max, step);
			}
			else
			{
				ImGui.DragFloat(MemoryExtensions.AsSpan(this.Title(name)), ref value, 1f, min.GetValueOrDefault(float.MinValue), max.GetValueOrDefault(float.MaxValue));
				if (min != null && value < min.Value)
				{
					value = min.Value;
				}
				if (max != null && value > max.Value)
				{
					value = max.Value;
				}
				ConfigWindow.StepFloat(ref value, min, max, step);
			}
			if (num3 != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue(value));
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		private static void StepFloat(ref float value, float? min, float? max, float? step)
		{
			if (step != null)
			{
				float? num = step;
				float num2 = 0f;
				if (!(num.GetValueOrDefault() == num2 & num != null))
				{
					float prev = value;
					float stepValue = step.Value;
					float stepPoint = min ?? max.GetValueOrDefault();
					value = MathF.Round((value - stepPoint) / stepValue) * stepValue + stepPoint;
					float num3 = value;
					return;
				}
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000C844 File Offset: 0x0000AA44
		private void DrawIntegerSetting(string name, ConfigSetting setting)
		{
			int value = setting.Value.AsInt(0);
			int num = value;
			ImGui.DragInt(MemoryExtensions.AsSpan(this.Title(name)), ref value);
			if (num != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue((long)value));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000C894 File Offset: 0x0000AA94
		private void DrawFloatSetting(string name, ConfigSetting setting)
		{
			float value = setting.Value.AsFloat(0f);
			float num = value;
			ImGui.DragFloat(MemoryExtensions.AsSpan(this.Title(name)), ref value);
			if (num != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue(value));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
		private void DrawStringSetting(string name, ConfigSetting setting)
		{
			string value = setting.Value.AsString(null);
			string text = value;
			ImGui.InputText(MemoryExtensions.AsSpan(this.Title(name)), ref value, 256U);
			if (text != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue(value));
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000C944 File Offset: 0x0000AB44
		private void DrawColorSetting(string name, ConfigSetting setting)
		{
			string value = setting.Value.AsString(null);
			string text = value;
			int red = int.Parse(value.Substring(1, 2), 515);
			int green = int.Parse(value.Substring(3, 2), 515);
			int blue = int.Parse(value.Substring(5, 2), 515);
			Vector3 color;
			color..ctor((float)red / 255f, (float)green / 255f, (float)blue / 255f);
			ImGui.ColorEdit3(MemoryExtensions.AsSpan(this.Title(name)), ref color, 4194304);
			value = ColorUtil.Int2Hex(ColorUtil.ToRgba(255, (int)(color.X * 255f), (int)(color.Y * 255f), (int)(color.Z * 255f)));
			if (text != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue(value));
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000CA30 File Offset: 0x0000AC30
		private void DrawBooleanSetting(string name, ConfigSetting setting)
		{
			bool value = setting.Value.AsBool(false);
			bool flag = value;
			ImGui.Checkbox(MemoryExtensions.AsSpan(this.Title(name)), ref value);
			if (flag != value)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			setting.Value = new JsonObject(new JValue(value));
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000CA80 File Offset: 0x0000AC80
		private int DrawComboBox(string name, string value, string[] values, ConfigSetting setting)
		{
			int index = 0;
			while (index < values.Length && !(values[index] == value))
			{
				index++;
			}
			int num = index;
			ImGui.Combo(MemoryExtensions.AsSpan(name), ref index, values, values.Length);
			if (num != index)
			{
				this.SetUnsavedChanges();
				setting.Changed();
			}
			return index;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000CACA File Offset: 0x0000ACCA
		public static void DrawItemHint(string hint)
		{
			if (ImGui.BeginItemTooltip())
			{
				ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
				ImGui.TextUnformatted(MemoryExtensions.AsSpan(hint));
				ImGui.PopTextWrapPos();
				ImGui.EndTooltip();
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		private void SetUnsavedChanges()
		{
			if (!this._unsavedDomains.Contains(this._currentIndex))
			{
				this._unsavedDomains.Add(this._currentIndex);
			}
			this._unsavedChanges = true;
		}

		// Token: 0x040000F6 RID: 246
		private readonly ICoreClientAPI _api;

		// Token: 0x040000F7 RID: 247
		private readonly IEnumerable<string> _domains;

		// Token: 0x040000F8 RID: 248
		private readonly Dictionary<string, Func<string, ControlButtons, ControlButtons>> _custom;

		// Token: 0x040000F9 RID: 249
		private readonly HashSet<string> _mods = new HashSet<string>();

		// Token: 0x040000FA RID: 250
		private readonly HashSet<int> _unsavedDomains = new HashSet<int>();

		// Token: 0x040000FB RID: 251
		private readonly ConfigLibModSystem _configsSystem;

		// Token: 0x040000FC RID: 252
		private readonly Style _redButton;

		// Token: 0x040000FD RID: 253
		private readonly Style _greenButton;

		// Token: 0x040000FE RID: 254
		private readonly Style _style;

		// Token: 0x040000FF RID: 255
		private readonly HashSet<float> _disabledHeaders = new HashSet<float>();

		// Token: 0x04000100 RID: 256
		private int _currentIndex;

		// Token: 0x04000101 RID: 257
		private long _nextId;

		// Token: 0x04000102 RID: 258
		private ControlButtons _controlButtons = new ControlButtons();

		// Token: 0x04000103 RID: 259
		private ControlButtons _visibleControlButtons = new ControlButtons(true);

		// Token: 0x04000104 RID: 260
		private bool _unsavedChanges;

		// Token: 0x04000105 RID: 261
		private bool _customConfig;

		// Token: 0x04000106 RID: 262
		private string _filter = "";

		// Token: 0x04000107 RID: 263
		private string _settingsFilter = "";

		// Token: 0x04000108 RID: 264
		private bool _confirmDefaults;
	}
}
