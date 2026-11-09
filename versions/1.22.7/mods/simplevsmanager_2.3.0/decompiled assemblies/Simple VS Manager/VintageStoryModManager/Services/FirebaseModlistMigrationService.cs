using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SimpleVsManager.Cloud;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000061 RID: 97
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class FirebaseModlistMigrationService
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x00038E20 File Offset: 0x00037020
		[NullableContext(2)]
		[return: Nullable(1)]
		public Task<bool> TryMigrateAsync(string playerUid, string playerName, UserConfigurationService userConfiguration, CancellationToken ct)
		{
			FirebaseModlistMigrationService.<TryMigrateAsync>d__2 <TryMigrateAsync>d__;
			<TryMigrateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<TryMigrateAsync>d__.<>4__this = this;
			<TryMigrateAsync>d__.playerUid = playerUid;
			<TryMigrateAsync>d__.playerName = playerName;
			<TryMigrateAsync>d__.userConfiguration = userConfiguration;
			<TryMigrateAsync>d__.ct = ct;
			<TryMigrateAsync>d__.<>1__state = -1;
			<TryMigrateAsync>d__.<>t__builder.Start<FirebaseModlistMigrationService.<TryMigrateAsync>d__2>(ref <TryMigrateAsync>d__);
			return <TryMigrateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00038E84 File Offset: 0x00037084
		private string BuildBackupMessageSuffix([Nullable(2)] string backupPath)
		{
			if (string.IsNullOrWhiteSpace(backupPath))
			{
				return string.Empty;
			}
			return " Legacy auth saved to " + backupPath + " for reference.";
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00038EA4 File Offset: 0x000370A4
		[NullableContext(2)]
		private static bool IsNewProject(string projectId)
		{
			return string.Equals(projectId, DevConfig.FirebaseNewProjectId, 5);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00038EB2 File Offset: 0x000370B2
		[NullableContext(2)]
		private static bool IsLegacyProject(string projectId)
		{
			return string.Equals(projectId, DevConfig.FirebaseLegacyProjectId, 5);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00038EC0 File Offset: 0x000370C0
		private Task<IReadOnlyList<FirebaseModlistMigrationService.LegacyModlist>> DownloadLegacyModlistsAsync(FirebaseModlistStore legacyStore, CancellationToken ct)
		{
			FirebaseModlistMigrationService.<DownloadLegacyModlistsAsync>d__6 <DownloadLegacyModlistsAsync>d__;
			<DownloadLegacyModlistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<FirebaseModlistMigrationService.LegacyModlist>>.Create();
			<DownloadLegacyModlistsAsync>d__.legacyStore = legacyStore;
			<DownloadLegacyModlistsAsync>d__.ct = ct;
			<DownloadLegacyModlistsAsync>d__.<>1__state = -1;
			<DownloadLegacyModlistsAsync>d__.<>t__builder.Start<FirebaseModlistMigrationService.<DownloadLegacyModlistsAsync>d__6>(ref <DownloadLegacyModlistsAsync>d__);
			return <DownloadLegacyModlistsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00038F0C File Offset: 0x0003710C
		[return: Nullable(2)]
		private string TryBackupLegacyAuthFile(string stateFilePath)
		{
			string result;
			try
			{
				string directoryName = Path.GetDirectoryName(stateFilePath);
				string fileName = Path.GetFileName(stateFilePath);
				if (string.IsNullOrWhiteSpace(directoryName) || string.IsNullOrWhiteSpace(fileName))
				{
					result = null;
				}
				else
				{
					string text = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
					string text2 = fileName + ".legacy." + text;
					string text3 = Path.Combine(directoryName, text2);
					File.Move(stateFilePath, text3, true);
					result = text3;
				}
			}
			catch (IOException)
			{
				result = null;
			}
			catch (UnauthorizedAccessException)
			{
				result = null;
			}
			catch (SecurityException)
			{
				result = null;
			}
			catch (NotSupportedException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00038FC8 File Offset: 0x000371C8
		[NullableContext(2)]
		private string TryRotateExistingBackup(UserConfigurationService userConfiguration)
		{
			string result;
			try
			{
				string backupFilePath = FirebaseAnonymousAuthenticator.GetBackupFilePath();
				if (string.IsNullOrWhiteSpace(backupFilePath))
				{
					result = null;
				}
				else if (!File.Exists(backupFilePath))
				{
					FirebaseModlistMigrationService.ResetBackupFlag(userConfiguration);
					result = null;
				}
				else
				{
					string directoryName = Path.GetDirectoryName(backupFilePath);
					string fileName = Path.GetFileName(backupFilePath);
					if (string.IsNullOrWhiteSpace(directoryName) || string.IsNullOrWhiteSpace(fileName))
					{
						result = null;
					}
					else
					{
						string text = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
						string text2 = Path.Combine(directoryName, fileName + ".legacy." + text);
						File.Move(backupFilePath, text2, true);
						FirebaseModlistMigrationService.ResetBackupFlag(userConfiguration);
						result = text2;
					}
				}
			}
			catch (IOException)
			{
				result = null;
			}
			catch (UnauthorizedAccessException)
			{
				result = null;
			}
			catch (SecurityException)
			{
				result = null;
			}
			catch (NotSupportedException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000390B4 File Offset: 0x000372B4
		[NullableContext(2)]
		private static void ResetBackupFlag(UserConfigurationService userConfiguration)
		{
			try
			{
				if (userConfiguration != null)
				{
					userConfiguration.ResetFirebaseAuthBackupFlag();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000390E0 File Offset: 0x000372E0
		[return: Nullable(2)]
		private string TryReadProjectId(string stateFilePath)
		{
			string result;
			try
			{
				string text = File.ReadAllText(stateFilePath);
				if (string.IsNullOrWhiteSpace(text))
				{
					result = null;
				}
				else
				{
					FirebaseModlistMigrationService.AuthStateModel authStateModel = JsonSerializer.Deserialize<FirebaseModlistMigrationService.AuthStateModel>(text, this._jsonOptions);
					result = FirebaseModlistMigrationService.TryExtractProjectId((authStateModel != null) ? authStateModel.IdToken : null);
				}
			}
			catch (IOException)
			{
				result = null;
			}
			catch (UnauthorizedAccessException)
			{
				result = null;
			}
			catch (JsonException)
			{
				result = null;
			}
			catch (SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003916C File Offset: 0x0003736C
		[NullableContext(2)]
		private static string TryExtractProjectId(string idToken)
		{
			if (string.IsNullOrWhiteSpace(idToken))
			{
				return null;
			}
			string[] array = idToken.Split('.', 0);
			if (array.Length < 2)
			{
				return null;
			}
			try
			{
				string text = array[1];
				text = text.PadRight(text.Length + (4 - text.Length % 4) % 4, '=');
				text = text.Replace('-', '+').Replace('_', '/');
				using (JsonDocument jsonDocument = JsonDocument.Parse(Convert.FromBase64String(text), default(JsonDocumentOptions)))
				{
					JsonElement jsonElement;
					if (jsonDocument.RootElement.TryGetProperty("iss", ref jsonElement) && jsonElement.ValueKind == 3)
					{
						string @string = jsonElement.GetString();
						if (!string.IsNullOrWhiteSpace(@string))
						{
							int num = @string.LastIndexOf("/", 4);
							return (num >= 0 && num < @string.Length - 1) ? @string.Substring(num + 1) : @string;
						}
					}
					JsonElement jsonElement2;
					if (jsonDocument.RootElement.TryGetProperty("aud", ref jsonElement2) && jsonElement2.ValueKind == 3)
					{
						return jsonElement2.GetString();
					}
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000392B0 File Offset: 0x000374B0
		private FirebaseModlistMigrationService.LegacyModlist EncodeLegacyModlist(FirebaseModlistMigrationService.LegacyModlist legacyModlist)
		{
			string text = this.TryEncodeModlistContent(legacyModlist.ContentJson);
			return new FirebaseModlistMigrationService.LegacyModlist(legacyModlist.SlotKey, text ?? legacyModlist.ContentJson);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000392E0 File Offset: 0x000374E0
		[return: Nullable(2)]
		private string TryEncodeModlistContent(string contentJson)
		{
			string result;
			try
			{
				SerializablePreset serializablePreset = JsonSerializer.Deserialize<SerializablePreset>(contentJson, this._modlistJsonOptions);
				if (serializablePreset == null)
				{
					result = null;
				}
				else
				{
					FirebaseModlistMigrationService.EncodePresetConfigurationContent(serializablePreset);
					result = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, this._modlistJsonOptions);
				}
			}
			catch (JsonException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0003932C File Offset: 0x0003752C
		private static void EncodePresetConfigurationContent(SerializablePreset preset)
		{
			if (preset.Mods == null)
			{
				return;
			}
			foreach (SerializablePresetModState serializablePresetModState in preset.Mods)
			{
				if (((serializablePresetModState != null) ? serializablePresetModState.ConfigurationContent : null) != null)
				{
					serializablePresetModState.ConfigurationContent = ModConfigurationEncoding.Encode(serializablePresetModState.ConfigurationContent);
				}
			}
		}

		// Token: 0x0400047C RID: 1148
		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x0400047D RID: 1149
		private readonly JsonSerializerOptions _modlistJsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3,
			WriteIndented = true
		};

		// Token: 0x02000279 RID: 633
		[Nullable(0)]
		private sealed class LegacyModlist : IEquatable<FirebaseModlistMigrationService.LegacyModlist>
		{
			// Token: 0x060015F5 RID: 5621 RVA: 0x0007F5B4 File Offset: 0x0007D7B4
			public LegacyModlist(string SlotKey, string ContentJson)
			{
				this.SlotKey = SlotKey;
				this.ContentJson = ContentJson;
				base..ctor();
			}

			// Token: 0x17000492 RID: 1170
			// (get) Token: 0x060015F6 RID: 5622 RVA: 0x0007F5CA File Offset: 0x0007D7CA
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(FirebaseModlistMigrationService.LegacyModlist);
				}
			}

			// Token: 0x17000493 RID: 1171
			// (get) Token: 0x060015F7 RID: 5623 RVA: 0x0007F5D6 File Offset: 0x0007D7D6
			// (set) Token: 0x060015F8 RID: 5624 RVA: 0x0007F5DE File Offset: 0x0007D7DE
			public string SlotKey { get; set; }

			// Token: 0x17000494 RID: 1172
			// (get) Token: 0x060015F9 RID: 5625 RVA: 0x0007F5E7 File Offset: 0x0007D7E7
			// (set) Token: 0x060015FA RID: 5626 RVA: 0x0007F5EF File Offset: 0x0007D7EF
			public string ContentJson { get; set; }

			// Token: 0x060015FB RID: 5627 RVA: 0x0007F5F8 File Offset: 0x0007D7F8
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LegacyModlist");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060015FC RID: 5628 RVA: 0x0007F644 File Offset: 0x0007D844
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("SlotKey = ");
				builder.Append(this.SlotKey);
				builder.Append(", ContentJson = ");
				builder.Append(this.ContentJson);
				return true;
			}

			// Token: 0x060015FD RID: 5629 RVA: 0x0007F67E File Offset: 0x0007D87E
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(FirebaseModlistMigrationService.LegacyModlist left, FirebaseModlistMigrationService.LegacyModlist right)
			{
				return !(left == right);
			}

			// Token: 0x060015FE RID: 5630 RVA: 0x0007F68A File Offset: 0x0007D88A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(FirebaseModlistMigrationService.LegacyModlist left, FirebaseModlistMigrationService.LegacyModlist right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060015FF RID: 5631 RVA: 0x0007F69E File Offset: 0x0007D89E
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SlotKey>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ContentJson>k__BackingField);
			}

			// Token: 0x06001600 RID: 5632 RVA: 0x0007F6DE File Offset: 0x0007D8DE
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as FirebaseModlistMigrationService.LegacyModlist);
			}

			// Token: 0x06001601 RID: 5633 RVA: 0x0007F6EC File Offset: 0x0007D8EC
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(FirebaseModlistMigrationService.LegacyModlist other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<SlotKey>k__BackingField, other.<SlotKey>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ContentJson>k__BackingField, other.<ContentJson>k__BackingField));
			}

			// Token: 0x06001603 RID: 5635 RVA: 0x0007F74D File Offset: 0x0007D94D
			[CompilerGenerated]
			private LegacyModlist(FirebaseModlistMigrationService.LegacyModlist original)
			{
				this.SlotKey = original.<SlotKey>k__BackingField;
				this.ContentJson = original.<ContentJson>k__BackingField;
			}

			// Token: 0x06001604 RID: 5636 RVA: 0x0007F76D File Offset: 0x0007D96D
			[CompilerGenerated]
			public void Deconstruct(out string SlotKey, out string ContentJson)
			{
				SlotKey = this.SlotKey;
				ContentJson = this.ContentJson;
			}
		}

		// Token: 0x0200027A RID: 634
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class AuthStateModel
		{
			// Token: 0x17000495 RID: 1173
			// (get) Token: 0x06001605 RID: 5637 RVA: 0x0007F77F File Offset: 0x0007D97F
			// (set) Token: 0x06001606 RID: 5638 RVA: 0x0007F787 File Offset: 0x0007D987
			public string IdToken { get; set; }
		}
	}
}
