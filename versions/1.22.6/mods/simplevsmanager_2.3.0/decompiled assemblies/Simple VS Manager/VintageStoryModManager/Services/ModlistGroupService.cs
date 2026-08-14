using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	public static class ModlistGroupService
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x00042980 File Offset: 0x00040B80
		public static ModlistGroupDefinition Load(string filePath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(filePath, "filePath");
			ModlistGroupDefinition modlistGroupDefinition = JsonSerializer.Deserialize<ModlistGroupDefinition>(File.ReadAllText(filePath), ModlistGroupService.JsonOptions);
			if (modlistGroupDefinition == null)
			{
				throw new InvalidDataException("The modlist group file was empty.");
			}
			ModlistGroupDefinition modlistGroupDefinition2 = modlistGroupDefinition;
			if (modlistGroupDefinition2.Members == null)
			{
				modlistGroupDefinition2.Members = new List<string>();
			}
			modlistGroupDefinition.ConflictResolutions = new Dictionary<string, string>(modlistGroupDefinition.ConflictResolutions ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
			return modlistGroupDefinition;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000429F0 File Offset: 0x00040BF0
		public static void Save(string filePath, ModlistGroupDefinition definition)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(filePath, "filePath");
			ArgumentNullException.ThrowIfNull(definition, "definition");
			definition.SchemaVersion = 1;
			if (definition.Members == null)
			{
				definition.Members = new List<string>();
			}
			definition.ConflictResolutions = new Dictionary<string, string>(definition.ConflictResolutions ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
			File.WriteAllText(filePath, JsonSerializer.Serialize<ModlistGroupDefinition>(definition, ModlistGroupService.JsonOptions));
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00042A64 File Offset: 0x00040C64
		public static string CreateReference(string groupFilePath, string memberFilePath)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(groupFilePath, "groupFilePath");
			ArgumentException.ThrowIfNullOrWhiteSpace(memberFilePath, "memberFilePath");
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(groupFilePath));
			if (directoryName == null)
			{
				throw new InvalidOperationException("The group file does not have a parent directory.");
			}
			string fullPath = Path.GetFullPath(memberFilePath);
			string relativePath = Path.GetRelativePath(directoryName, fullPath);
			string text = relativePath;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 1);
			defaultInterpolatedStringHandler.AppendLiteral("..");
			defaultInterpolatedStringHandler.AppendFormatted<char>(Path.DirectorySeparatorChar);
			if (!text.StartsWith(defaultInterpolatedStringHandler.ToStringAndClear(), 4) && !string.Equals(relativePath, "..", 4))
			{
				return relativePath;
			}
			return fullPath;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00042AF4 File Offset: 0x00040CF4
		public static string ResolveReference(string groupFilePath, string memberReference)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(groupFilePath, "groupFilePath");
			ArgumentException.ThrowIfNullOrWhiteSpace(memberReference, "memberReference");
			if (Path.IsPathRooted(memberReference))
			{
				return Path.GetFullPath(memberReference);
			}
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(groupFilePath));
			if (directoryName == null)
			{
				throw new InvalidOperationException("The group file does not have a parent directory.");
			}
			return Path.GetFullPath(Path.Combine(directoryName, memberReference));
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00042B4C File Offset: 0x00040D4C
		public static ModlistGroupBuildResult BuildMergedPreset(ModlistGroupDefinition definition, IReadOnlyList<ModlistGroupSource> sources, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> missingMembers = null)
		{
			ArgumentNullException.ThrowIfNull(definition, "definition");
			ArgumentNullException.ThrowIfNull(sources, "sources");
			Dictionary<string, List<ModlistGroupService.ModCandidate>> dictionary = new Dictionary<string, List<ModlistGroupService.ModCandidate>>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			using (IEnumerator<ModlistGroupSource> enumerator = sources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModlistGroupSource source = enumerator.Current;
					if (source.Preset.DisabledEntries != null)
					{
						foreach (string text in source.Preset.DisabledEntries)
						{
							if (!string.IsNullOrWhiteSpace(text) && hashSet.Add(text.Trim()))
							{
								list2.Add(text.Trim());
							}
						}
					}
					if (source.Preset.Mods != null)
					{
						Func<ModlistGroupService.ModCandidate, bool> <>9__1;
						foreach (SerializablePresetModState serializablePresetModState in source.Preset.Mods)
						{
							if (serializablePresetModState != null && !string.IsNullOrWhiteSpace(serializablePresetModState.ModId))
							{
								string text2 = serializablePresetModState.ModId.Trim();
								List<ModlistGroupService.ModCandidate> list3;
								if (!dictionary.TryGetValue(text2, ref list3))
								{
									list3 = new List<ModlistGroupService.ModCandidate>();
									dictionary[text2] = list3;
									list.Add(text2);
								}
								IEnumerable<ModlistGroupService.ModCandidate> enumerable = list3;
								Func<ModlistGroupService.ModCandidate, bool> func;
								if ((func = <>9__1) == null)
								{
									func = (<>9__1 = ((ModlistGroupService.ModCandidate candidate) => string.Equals(candidate.Source.Reference, source.Reference, 5)));
								}
								if (!Enumerable.Any<ModlistGroupService.ModCandidate>(enumerable, func))
								{
									list3.Add(new ModlistGroupService.ModCandidate(source, serializablePresetModState));
								}
							}
						}
					}
				}
			}
			List<ModlistGroupConflict> list4 = new List<ModlistGroupConflict>();
			Dictionary<string, ModlistGroupService.ModCandidate> selectedCandidates = new Dictionary<string, ModlistGroupService.ModCandidate>(StringComparer.OrdinalIgnoreCase);
			foreach (string text3 in list)
			{
				List<ModlistGroupService.ModCandidate> list5 = dictionary[text3];
				IEnumerable<ModlistGroupService.ModCandidate> enumerable2 = list5;
				Func<ModlistGroupService.ModCandidate, string> func2;
				if ((func2 = ModlistGroupService.<>O.<0>__BuildChoiceComparisonKey) == null)
				{
					func2 = (ModlistGroupService.<>O.<0>__BuildChoiceComparisonKey = new Func<ModlistGroupService.ModCandidate, string>(ModlistGroupService.BuildChoiceComparisonKey));
				}
				List<ModlistGroupService.ModCandidate> list6 = Enumerable.ToList<ModlistGroupService.ModCandidate>(Enumerable.Select<IGrouping<string, ModlistGroupService.ModCandidate>, ModlistGroupService.ModCandidate>(Enumerable.GroupBy<ModlistGroupService.ModCandidate, string>(enumerable2, func2, StringComparer.Ordinal), (IGrouping<string, ModlistGroupService.ModCandidate> group) => Enumerable.First<ModlistGroupService.ModCandidate>(group)));
				if (list6.Count == 1)
				{
					selectedCandidates[text3] = list6[0];
				}
				else
				{
					string text4;
					string selectedReference = definition.ConflictResolutions.TryGetValue(text3, ref text4) ? text4 : null;
					ModlistGroupService.ModCandidate resolvedCandidate = Enumerable.FirstOrDefault<ModlistGroupService.ModCandidate>(list5, (ModlistGroupService.ModCandidate candidate) => ModlistGroupService.ReferencesEqual(candidate.Source.Reference, selectedReference));
					ModlistGroupService.ModCandidate modCandidate = (resolvedCandidate == null) ? ModlistGroupService.SelectDefaultCandidate(list6) : Enumerable.First<ModlistGroupService.ModCandidate>(list6, (ModlistGroupService.ModCandidate candidate) => string.Equals(ModlistGroupService.BuildChoiceComparisonKey(candidate), ModlistGroupService.BuildChoiceComparisonKey(resolvedCandidate), 4));
					selectedCandidates[text3] = modCandidate;
					List<ModlistGroupConflictChoice> choices = Enumerable.ToList<ModlistGroupConflictChoice>(Enumerable.Select<ModlistGroupService.ModCandidate, ModlistGroupConflictChoice>(list6, (ModlistGroupService.ModCandidate candidate) => new ModlistGroupConflictChoice(candidate.Source.Reference, candidate.Source.FilePath, candidate.Source.DisplayName, candidate.Mod.Version, ModlistGroupService.HasCustomConfiguration(candidate.Source.Preset, candidate.Mod))));
					list4.Add(new ModlistGroupConflict(text3, choices, modCandidate.Source.Reference));
				}
			}
			List<SerializablePresetModState> mods = Enumerable.ToList<SerializablePresetModState>(Enumerable.Select<string, SerializablePresetModState>(list, (string modId) => ModlistGroupService.CloneModState(selectedCandidates[modId].Mod)));
			List<SerializableModConfiguration> list7 = new List<SerializableModConfiguration>();
			foreach (string text5 in list)
			{
				ModlistGroupService.ModCandidate modCandidate2 = selectedCandidates[text5];
				if (modCandidate2.Source.Preset.Configurations != null)
				{
					foreach (SerializableModConfiguration serializableModConfiguration in modCandidate2.Source.Preset.Configurations)
					{
						if (serializableModConfiguration != null)
						{
							string modId2 = serializableModConfiguration.ModId;
							if (string.Equals((modId2 != null) ? modId2.Trim() : null, text5, 5))
							{
								list7.Add(ModlistGroupService.CloneConfiguration(serializableModConfiguration));
							}
						}
					}
				}
			}
			SerializablePreset serializablePreset = new SerializablePreset();
			serializablePreset.Name = (string.IsNullOrWhiteSpace(definition.Name) ? "Unnamed Modlist Group" : definition.Name.Trim());
			serializablePreset.Description = (string.IsNullOrWhiteSpace(definition.Description) ? null : definition.Description.Trim());
			serializablePreset.Version = (string.IsNullOrWhiteSpace(definition.Version) ? null : definition.Version.Trim());
			serializablePreset.GameVersion = (string.IsNullOrWhiteSpace(definition.GameVersion) ? null : definition.GameVersion.Trim());
			serializablePreset.DisabledEntries = list2;
			serializablePreset.Mods = mods;
			serializablePreset.Configurations = ((list7.Count > 0) ? list7 : null);
			serializablePreset.IncludeModStatus = new bool?(Enumerable.Any<ModlistGroupSource>(sources, delegate(ModlistGroupSource source)
			{
				if (source.Preset.IncludeModStatus.GetValueOrDefault())
				{
					return true;
				}
				List<SerializablePresetModState> mods2 = source.Preset.Mods;
				if (mods2 == null)
				{
					return false;
				}
				return Enumerable.Any<SerializablePresetModState>(mods2, delegate(SerializablePresetModState mod)
				{
					bool? flag = (mod != null) ? mod.IsActive : default(bool?);
					return flag != null;
				});
			}));
			serializablePreset.IncludeModVersions = new bool?(Enumerable.Any<ModlistGroupSource>(sources, delegate(ModlistGroupSource source)
			{
				if (source.Preset.IncludeModVersions.GetValueOrDefault())
				{
					return true;
				}
				List<SerializablePresetModState> mods2 = source.Preset.Mods;
				if (mods2 == null)
				{
					return false;
				}
				return Enumerable.Any<SerializablePresetModState>(mods2, (SerializablePresetModState mod) => !string.IsNullOrWhiteSpace((mod != null) ? mod.Version : null));
			}));
			serializablePreset.Exclusive = new bool?(true);
			IReadOnlyList<ModlistGroupConflict> conflicts = list4;
			IReadOnlyList<string> missingMembers2;
			if (missingMembers != null)
			{
				IReadOnlyList<string> readOnlyList = Enumerable.ToList<string>(missingMembers);
				missingMembers2 = readOnlyList;
			}
			else
			{
				IReadOnlyList<string> readOnlyList = Array.Empty<string>();
				missingMembers2 = readOnlyList;
			}
			return new ModlistGroupBuildResult(serializablePreset, conflicts, missingMembers2);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00043164 File Offset: 0x00041364
		private static bool ReferencesEqual(string left, [Nullable(2)] string right)
		{
			return !string.IsNullOrWhiteSpace(right) && string.Equals(left, right, 5);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00043178 File Offset: 0x00041378
		private static ModlistGroupService.ModCandidate SelectDefaultCandidate(IReadOnlyList<ModlistGroupService.ModCandidate> candidates)
		{
			ModlistGroupService.ModCandidate modCandidate = candidates[0];
			bool flag = ModlistGroupService.HasCustomConfiguration(modCandidate.Source.Preset, modCandidate.Mod);
			for (int i = 1; i < candidates.Count; i++)
			{
				ModlistGroupService.ModCandidate modCandidate2 = candidates[i];
				int num = ModlistGroupService.CompareVersions(modCandidate2.Mod.Version, modCandidate.Mod.Version);
				bool flag2 = ModlistGroupService.HasCustomConfiguration(modCandidate2.Source.Preset, modCandidate2.Mod);
				if (num > 0 || (num == 0 && flag2 && !flag))
				{
					modCandidate = modCandidate2;
					flag = flag2;
				}
			}
			return modCandidate;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0004320C File Offset: 0x0004140C
		[NullableContext(2)]
		private static int CompareVersions(string left, string right)
		{
			string text = VersionStringUtility.Normalize(left);
			string text2 = VersionStringUtility.Normalize(right);
			if (text != null && text2 != null)
			{
				IEnumerable<string> enumerable = text.Split('.', 0);
				Func<string, int> func;
				if ((func = ModlistGroupService.<>O.<1>__Parse) == null)
				{
					func = (ModlistGroupService.<>O.<1>__Parse = new Func<string, int>(int.Parse));
				}
				int[] array = Enumerable.ToArray<int>(Enumerable.Select<string, int>(enumerable, func));
				IEnumerable<string> enumerable2 = text2.Split('.', 0);
				Func<string, int> func2;
				if ((func2 = ModlistGroupService.<>O.<1>__Parse) == null)
				{
					func2 = (ModlistGroupService.<>O.<1>__Parse = new Func<string, int>(int.Parse));
				}
				int[] array2 = Enumerable.ToArray<int>(Enumerable.Select<string, int>(enumerable2, func2));
				int num = Math.Max(array.Length, array2.Length);
				for (int i = 0; i < num; i++)
				{
					int num2 = (i < array.Length) ? array[i] : 0;
					int num3 = (i < array2.Length) ? array2[i] : 0;
					int num4 = num2.CompareTo(num3);
					if (num4 != 0)
					{
						return num4;
					}
				}
				return 0;
			}
			if (text != null)
			{
				return 1;
			}
			if (text2 != null)
			{
				return -1;
			}
			return string.Compare(ModlistGroupService.NormalizeOptional(left), ModlistGroupService.NormalizeOptional(right), 5);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00043300 File Offset: 0x00041500
		private static bool HasCustomConfiguration(SerializablePreset preset, SerializablePresetModState mod)
		{
			List<SerializableModConfiguration> configurations = preset.Configurations;
			List<SerializableModConfiguration> list = ((configurations != null) ? Enumerable.ToList<SerializableModConfiguration>(Enumerable.Where<SerializableModConfiguration>(configurations, delegate(SerializableModConfiguration configuration)
			{
				if (configuration != null)
				{
					string modId = configuration.ModId;
					string text = (modId != null) ? modId.Trim() : null;
					string modId2 = mod.ModId;
					return string.Equals(text, (modId2 != null) ? modId2.Trim() : null, 5);
				}
				return false;
			})) : null) ?? new List<SerializableModConfiguration>();
			if (list.Count <= 0)
			{
				return !string.IsNullOrEmpty(ModConfigurationEncoding.Decode(mod.ConfigurationContent));
			}
			return Enumerable.Any<SerializableModConfiguration>(list, (SerializableModConfiguration configuration) => !string.IsNullOrEmpty(configuration.Content));
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00043390 File Offset: 0x00041590
		private static string BuildChoiceComparisonKey(ModlistGroupService.ModCandidate candidate)
		{
			ModlistGroupService.<>c__DisplayClass11_0 CS$<>8__locals1 = new ModlistGroupService.<>c__DisplayClass11_0();
			SerializablePresetModState mod = candidate.Mod;
			ModlistGroupService.<>c__DisplayClass11_0 CS$<>8__locals2 = CS$<>8__locals1;
			string modId = mod.ModId;
			CS$<>8__locals2.modId = ((modId != null) ? modId.Trim() : null);
			List<SerializableModConfiguration> configurations = candidate.Source.Preset.Configurations;
			List<SerializableModConfiguration> list = ((configurations != null) ? Enumerable.ToList<SerializableModConfiguration>(Enumerable.Where<SerializableModConfiguration>(configurations, delegate(SerializableModConfiguration configuration)
			{
				if (configuration != null)
				{
					string modId2 = configuration.ModId;
					return string.Equals((modId2 != null) ? modId2.Trim() : null, CS$<>8__locals1.modId, 5);
				}
				return false;
			})) : null) ?? new List<SerializableModConfiguration>();
			var list2 = Enumerable.ToList(Enumerable.ThenBy(Enumerable.ThenBy(Enumerable.OrderBy(Enumerable.Select(Enumerable.Where<SerializableModConfiguration>(list, (SerializableModConfiguration configuration) => !string.IsNullOrEmpty(configuration.Content)), delegate(SerializableModConfiguration configuration)
			{
				string text3 = ModlistGroupService.NormalizeOptional(configuration.FileName);
				string text4 = (text3 != null) ? text3.ToUpperInvariant() : null;
				string text5 = ModlistGroupService.NormalizeOptional(configuration.RelativePath);
				return new <>f__AnonymousType6<string, string, string>(text4, (text5 != null) ? text5.ToUpperInvariant() : null, configuration.Content);
			}), configuration => configuration.fileName, StringComparer.Ordinal), configuration => configuration.relativePath, StringComparer.Ordinal), configuration => configuration.Content, StringComparer.Ordinal)) ?? new List<<>f__AnonymousType6<string, string, string>>();
			if (list.Count == 0)
			{
				string text = ModConfigurationEncoding.Decode(mod.ConfigurationContent);
				if (!string.IsNullOrEmpty(text))
				{
					var list3 = list2;
					string text2 = ModlistGroupService.NormalizeOptional(mod.ConfigurationFileName);
					list3.Add(new <>f__AnonymousType6<string, string, string>((text2 != null) ? text2.ToUpperInvariant() : null, null, text));
				}
			}
			return JsonSerializer.Serialize(new <>f__AnonymousType7<string, List<<>f__AnonymousType6<string, string, string>>>(ModlistGroupService.NormalizeOptional(mod.Version), list2), null);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00043521 File Offset: 0x00041721
		[NullableContext(2)]
		private static string NormalizeOptional(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return null;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00043534 File Offset: 0x00041734
		private static SerializablePresetModState CloneModState(SerializablePresetModState source)
		{
			return new SerializablePresetModState
			{
				ModId = source.ModId,
				Version = source.Version,
				IsActive = source.IsActive,
				ConfigurationFileName = source.ConfigurationFileName,
				ConfigurationContent = source.ConfigurationContent
			};
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00043582 File Offset: 0x00041782
		private static SerializableModConfiguration CloneConfiguration(SerializableModConfiguration source)
		{
			return new SerializableModConfiguration
			{
				ModId = source.ModId,
				FileName = source.FileName,
				RelativePath = source.RelativePath,
				Content = source.Content
			};
		}

		// Token: 0x040004D8 RID: 1240
		public const string FileExtension = ".svsmgroup";

		// Token: 0x040004D9 RID: 1241
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3,
			WriteIndented = true
		};

		// Token: 0x020002D5 RID: 725
		[Nullable(0)]
		private sealed class ModCandidate : IEquatable<ModlistGroupService.ModCandidate>
		{
			// Token: 0x0600181B RID: 6171 RVA: 0x0008C40A File Offset: 0x0008A60A
			public ModCandidate(ModlistGroupSource Source, SerializablePresetModState Mod)
			{
				this.Source = Source;
				this.Mod = Mod;
				base..ctor();
			}

			// Token: 0x17000507 RID: 1287
			// (get) Token: 0x0600181C RID: 6172 RVA: 0x0008C420 File Offset: 0x0008A620
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModlistGroupService.ModCandidate);
				}
			}

			// Token: 0x17000508 RID: 1288
			// (get) Token: 0x0600181D RID: 6173 RVA: 0x0008C42C File Offset: 0x0008A62C
			// (set) Token: 0x0600181E RID: 6174 RVA: 0x0008C434 File Offset: 0x0008A634
			public ModlistGroupSource Source { get; set; }

			// Token: 0x17000509 RID: 1289
			// (get) Token: 0x0600181F RID: 6175 RVA: 0x0008C43D File Offset: 0x0008A63D
			// (set) Token: 0x06001820 RID: 6176 RVA: 0x0008C445 File Offset: 0x0008A645
			public SerializablePresetModState Mod { get; set; }

			// Token: 0x06001821 RID: 6177 RVA: 0x0008C450 File Offset: 0x0008A650
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ModCandidate");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001822 RID: 6178 RVA: 0x0008C49C File Offset: 0x0008A69C
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Source = ");
				builder.Append(this.Source);
				builder.Append(", Mod = ");
				builder.Append(this.Mod);
				return true;
			}

			// Token: 0x06001823 RID: 6179 RVA: 0x0008C4D6 File Offset: 0x0008A6D6
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModlistGroupService.ModCandidate left, ModlistGroupService.ModCandidate right)
			{
				return !(left == right);
			}

			// Token: 0x06001824 RID: 6180 RVA: 0x0008C4E2 File Offset: 0x0008A6E2
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModlistGroupService.ModCandidate left, ModlistGroupService.ModCandidate right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06001825 RID: 6181 RVA: 0x0008C4F6 File Offset: 0x0008A6F6
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ModlistGroupSource>.Default.GetHashCode(this.<Source>k__BackingField)) * -1521134295 + EqualityComparer<SerializablePresetModState>.Default.GetHashCode(this.<Mod>k__BackingField);
			}

			// Token: 0x06001826 RID: 6182 RVA: 0x0008C536 File Offset: 0x0008A736
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModlistGroupService.ModCandidate);
			}

			// Token: 0x06001827 RID: 6183 RVA: 0x0008C544 File Offset: 0x0008A744
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModlistGroupService.ModCandidate other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ModlistGroupSource>.Default.Equals(this.<Source>k__BackingField, other.<Source>k__BackingField) && EqualityComparer<SerializablePresetModState>.Default.Equals(this.<Mod>k__BackingField, other.<Mod>k__BackingField));
			}

			// Token: 0x06001829 RID: 6185 RVA: 0x0008C5A5 File Offset: 0x0008A7A5
			[CompilerGenerated]
			private ModCandidate(ModlistGroupService.ModCandidate original)
			{
				this.Source = original.<Source>k__BackingField;
				this.Mod = original.<Mod>k__BackingField;
			}

			// Token: 0x0600182A RID: 6186 RVA: 0x0008C5C5 File Offset: 0x0008A7C5
			[CompilerGenerated]
			public void Deconstruct(out ModlistGroupSource Source, out SerializablePresetModState Mod)
			{
				Source = this.Source;
				Mod = this.Mod;
			}
		}

		// Token: 0x020002D6 RID: 726
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04001227 RID: 4647
			[Nullable(0)]
			public static Func<ModlistGroupService.ModCandidate, string> <0>__BuildChoiceComparisonKey;

			// Token: 0x04001228 RID: 4648
			[Nullable(0)]
			public static Func<string, int> <1>__Parse;
		}
	}
}
