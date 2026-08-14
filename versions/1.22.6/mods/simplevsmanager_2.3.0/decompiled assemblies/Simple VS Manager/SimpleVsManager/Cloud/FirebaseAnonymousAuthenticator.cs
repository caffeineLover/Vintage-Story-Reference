using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using VintageStoryModManager;
using VintageStoryModManager.Services;

namespace SimpleVsManager.Cloud
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FirebaseAnonymousAuthenticator : IDisposable
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000033B2 File Offset: 0x000015B2
		public FirebaseAnonymousAuthenticator() : this(FirebaseAnonymousAuthenticator.DefaultApiKey)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000033C0 File Offset: 0x000015C0
		public FirebaseAnonymousAuthenticator(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				throw new ArgumentException("A Firebase API key is required.", "apiKey");
			}
			this.ApiKey = apiKey.Trim();
			this._stateFilePath = FirebaseAnonymousAuthenticator.DetermineStateFilePath();
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000341A File Offset: 0x0000161A
		public string ApiKey { get; }

		// Token: 0x0600006C RID: 108 RVA: 0x00003424 File Offset: 0x00001624
		public Task<string> GetIdTokenAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<GetIdTokenAsync>d__18 <GetIdTokenAsync>d__;
			<GetIdTokenAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetIdTokenAsync>d__.<>4__this = this;
			<GetIdTokenAsync>d__.ct = ct;
			<GetIdTokenAsync>d__.<>1__state = -1;
			<GetIdTokenAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<GetIdTokenAsync>d__18>(ref <GetIdTokenAsync>d__);
			return <GetIdTokenAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003470 File Offset: 0x00001670
		public Task<FirebaseAnonymousAuthenticator.FirebaseAuthSession> GetSessionAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<GetSessionAsync>d__19 <GetSessionAsync>d__;
			<GetSessionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseAnonymousAuthenticator.FirebaseAuthSession>.Create();
			<GetSessionAsync>d__.<>4__this = this;
			<GetSessionAsync>d__.ct = ct;
			<GetSessionAsync>d__.<>1__state = -1;
			<GetSessionAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<GetSessionAsync>d__19>(ref <GetSessionAsync>d__);
			return <GetSessionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000034BC File Offset: 0x000016BC
		public Task<FirebaseAnonymousAuthenticator.FirebaseAuthSession?> TryGetExistingSessionAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<TryGetExistingSessionAsync>d__20 <TryGetExistingSessionAsync>d__;
			<TryGetExistingSessionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseAnonymousAuthenticator.FirebaseAuthSession?>.Create();
			<TryGetExistingSessionAsync>d__.<>4__this = this;
			<TryGetExistingSessionAsync>d__.ct = ct;
			<TryGetExistingSessionAsync>d__.<>1__state = -1;
			<TryGetExistingSessionAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<TryGetExistingSessionAsync>d__20>(ref <TryGetExistingSessionAsync>d__);
			return <TryGetExistingSessionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003508 File Offset: 0x00001708
		public Task MarkTokenAsExpiredAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<MarkTokenAsExpiredAsync>d__21 <MarkTokenAsExpiredAsync>d__;
			<MarkTokenAsExpiredAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<MarkTokenAsExpiredAsync>d__.<>4__this = this;
			<MarkTokenAsExpiredAsync>d__.ct = ct;
			<MarkTokenAsExpiredAsync>d__.<>1__state = -1;
			<MarkTokenAsExpiredAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<MarkTokenAsExpiredAsync>d__21>(ref <MarkTokenAsExpiredAsync>d__);
			return <MarkTokenAsExpiredAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003554 File Offset: 0x00001754
		public Task DeleteAccountAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<DeleteAccountAsync>d__22 <DeleteAccountAsync>d__;
			<DeleteAccountAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteAccountAsync>d__.<>4__this = this;
			<DeleteAccountAsync>d__.ct = ct;
			<DeleteAccountAsync>d__.<>1__state = -1;
			<DeleteAccountAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<DeleteAccountAsync>d__22>(ref <DeleteAccountAsync>d__);
			return <DeleteAccountAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000035A0 File Offset: 0x000017A0
		internal static bool HasPersistedState()
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			if (string.IsNullOrWhiteSpace(stateFilePath) || !File.Exists(stateFilePath))
			{
				return false;
			}
			bool result;
			try
			{
				string text = File.ReadAllText(stateFilePath);
				if (string.IsNullOrWhiteSpace(text))
				{
					result = false;
				}
				else
				{
					FirebaseAnonymousAuthenticator.AuthStateModel authStateModel = JsonSerializer.Deserialize<FirebaseAnonymousAuthenticator.AuthStateModel>(text, FirebaseAnonymousAuthenticator.JsonOptions);
					if (authStateModel == null || string.IsNullOrWhiteSpace(authStateModel.IdToken) || string.IsNullOrWhiteSpace(authStateModel.RefreshToken) || string.IsNullOrWhiteSpace(authStateModel.UserId))
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (IOException)
			{
				result = false;
			}
			catch (UnauthorizedAccessException)
			{
				result = false;
			}
			catch (JsonException)
			{
				result = false;
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003660 File Offset: 0x00001860
		internal static string GetStateFilePath()
		{
			string firebaseStateFilePathOverride = DeveloperProfileManager.GetFirebaseStateFilePathOverride();
			if (!string.IsNullOrWhiteSpace(firebaseStateFilePathOverride))
			{
				return firebaseStateFilePathOverride;
			}
			string text = FirebaseAnonymousAuthenticator.TryGetSecureBaseDirectory();
			if (!string.IsNullOrWhiteSpace(text))
			{
				return Path.Combine(text, FirebaseAnonymousAuthenticator.StateFileName);
			}
			return FirebaseAnonymousAuthenticator.GetDefaultStateFilePath();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000369C File Offset: 0x0000189C
		private static string DetermineStateFilePath()
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			string directoryName = Path.GetDirectoryName(stateFilePath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				try
				{
					Directory.CreateDirectory(directoryName);
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (NotSupportedException)
				{
				}
				catch (SecurityException)
				{
				}
			}
			return stateFilePath;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003708 File Offset: 0x00001908
		[NullableContext(2)]
		private static string TryGetSecureBaseDirectory()
		{
			string customConfigFolder = CustomConfigFolderManager.GetCustomConfigFolder();
			if (!string.IsNullOrWhiteSpace(customConfigFolder) && FirebaseAnonymousAuthenticator.TryEnsureDirectory(customConfigFolder))
			{
				return customConfigFolder;
			}
			Environment.SpecialFolder[] array = new Environment.SpecialFolder[]
			{
				28,
				26
			};
			for (int i = 0; i < array.Length; i++)
			{
				string text = FirebaseAnonymousAuthenticator.TryGetFolderPath(array[i]);
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = Path.Combine(text, "Simple VS Manager");
					if (FirebaseAnonymousAuthenticator.TryEnsureDirectory(text2))
					{
						return text2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003778 File Offset: 0x00001978
		private static string GetDefaultStateFilePath()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (!string.IsNullOrWhiteSpace(managerDataDirectory))
			{
				return Path.Combine(managerDataDirectory, FirebaseAnonymousAuthenticator.StateFileName);
			}
			string text = FirebaseAnonymousAuthenticator.TryGetFolderPath(5) ?? FirebaseAnonymousAuthenticator.TryGetFolderPath(5);
			return Path.Combine(string.IsNullOrWhiteSpace(text) ? Environment.CurrentDirectory : text, "Simple VS Manager", FirebaseAnonymousAuthenticator.StateFileName);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000037D0 File Offset: 0x000019D0
		[NullableContext(2)]
		private static string TryGetFolderPath(Environment.SpecialFolder folder)
		{
			string result;
			try
			{
				string folderPath = Environment.GetFolderPath(folder);
				result = (string.IsNullOrWhiteSpace(folderPath) ? null : folderPath);
			}
			catch (Exception ex) when (ex is PlatformNotSupportedException || ex is InvalidOperationException || ex is SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000383C File Offset: 0x00001A3C
		private static bool TryEnsureDirectory(string path)
		{
			bool result;
			try
			{
				Directory.CreateDirectory(path);
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000038A4 File Offset: 0x00001AA4
		[NullableContext(2)]
		private FirebaseAnonymousAuthenticator.FirebaseAuthState LoadStateFromDisk()
		{
			FirebaseAnonymousAuthenticator.FirebaseAuthState result;
			try
			{
				if (!File.Exists(this._stateFilePath))
				{
					result = null;
				}
				else
				{
					string text = File.ReadAllText(this._stateFilePath);
					if (string.IsNullOrWhiteSpace(text))
					{
						result = null;
					}
					else
					{
						FirebaseAnonymousAuthenticator.FirebaseAuthState firebaseAuthState = FirebaseAnonymousAuthenticator.TryDeserializeState(text);
						if (firebaseAuthState != null)
						{
							result = firebaseAuthState;
						}
						else
						{
							result = FirebaseAnonymousAuthenticator.TryDeserializeLegacyState(text);
						}
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

		// Token: 0x06000079 RID: 121 RVA: 0x0000393C File Offset: 0x00001B3C
		[return: Nullable(2)]
		private static FirebaseAnonymousAuthenticator.FirebaseAuthState TryDeserializeState(string json)
		{
			FirebaseAnonymousAuthenticator.AuthStateModel authStateModel = JsonSerializer.Deserialize<FirebaseAnonymousAuthenticator.AuthStateModel>(json, FirebaseAnonymousAuthenticator.JsonOptions);
			if (authStateModel == null || string.IsNullOrWhiteSpace(authStateModel.IdToken) || string.IsNullOrWhiteSpace(authStateModel.RefreshToken) || string.IsNullOrWhiteSpace(authStateModel.UserId))
			{
				return null;
			}
			DateTimeOffset expiration = (authStateModel.ExpirationUtc == default(DateTimeOffset)) ? DateTimeOffset.MinValue : authStateModel.ExpirationUtc;
			return new FirebaseAnonymousAuthenticator.FirebaseAuthState(authStateModel.IdToken.Trim(), authStateModel.RefreshToken.Trim(), expiration, authStateModel.UserId.Trim());
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000039CC File Offset: 0x00001BCC
		[return: Nullable(2)]
		private static FirebaseAnonymousAuthenticator.FirebaseAuthState TryDeserializeLegacyState(string json)
		{
			FirebaseAnonymousAuthenticator.FirebaseAuthState result;
			using (JsonDocument jsonDocument = JsonDocument.Parse(json, default(JsonDocumentOptions)))
			{
				JsonElement rootElement = jsonDocument.RootElement;
				string text = FirebaseAnonymousAuthenticator.TryGetString(rootElement, "idToken");
				string text2 = FirebaseAnonymousAuthenticator.TryGetString(rootElement, "refreshToken");
				string text3;
				if ((text3 = FirebaseAnonymousAuthenticator.TryGetString(rootElement, "userId")) == null)
				{
					text3 = (FirebaseAnonymousAuthenticator.TryGetString(rootElement, "uid") ?? FirebaseAnonymousAuthenticator.TryGetString(rootElement, "localId"));
				}
				string text4 = text3;
				if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2) || string.IsNullOrWhiteSpace(text4))
				{
					result = null;
				}
				else
				{
					string text5;
					if ((text5 = FirebaseAnonymousAuthenticator.TryGetString(rootElement, "expirationUtc")) == null)
					{
						text5 = (FirebaseAnonymousAuthenticator.TryGetString(rootElement, "expiration") ?? FirebaseAnonymousAuthenticator.TryGetString(rootElement, "expiresAt"));
					}
					DateTimeOffset dateTimeOffset;
					DateTimeOffset expiration = DateTimeOffset.TryParse(text5, ref dateTimeOffset) ? dateTimeOffset : DateTimeOffset.MinValue;
					result = new FirebaseAnonymousAuthenticator.FirebaseAuthState(text.Trim(), text2.Trim(), expiration, text4.Trim());
				}
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003ACC File Offset: 0x00001CCC
		[return: Nullable(2)]
		private static string TryGetString(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty(propertyName, ref jsonElement))
			{
				return null;
			}
			JsonValueKind valueKind = jsonElement.ValueKind;
			string result;
			if (valueKind != 3)
			{
				if (valueKind != 4)
				{
					result = null;
				}
				else
				{
					long num;
					result = (jsonElement.TryGetInt64(ref num) ? num.ToString() : jsonElement.ToString());
				}
			}
			else
			{
				result = jsonElement.GetString();
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003B2C File Offset: 0x00001D2C
		private void SaveStateToDisk(FirebaseAnonymousAuthenticator.FirebaseAuthState state)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(this._stateFilePath);
				if (!string.IsNullOrWhiteSpace(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				string text = JsonSerializer.Serialize<FirebaseAnonymousAuthenticator.AuthStateModel>(new FirebaseAnonymousAuthenticator.AuthStateModel
				{
					IdToken = state.IdToken,
					RefreshToken = state.RefreshToken,
					ExpirationUtc = state.Expiration,
					UserId = state.UserId
				}, FirebaseAnonymousAuthenticator.JsonOptions);
				File.WriteAllText(this._stateFilePath, text);
				this.TryCreateBackup();
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003BDC File Offset: 0x00001DDC
		private void TryCreateBackup()
		{
			try
			{
				if (File.Exists(this._stateFilePath))
				{
					UserConfigurationService userConfigurationService = FirebaseAnonymousAuthenticator.TryGetUserConfiguration();
					if (userConfigurationService == null || !userConfigurationService.FirebaseAuthBackupCreated)
					{
						string backupFilePath = FirebaseAnonymousAuthenticator.GetBackupFilePath();
						if (!string.IsNullOrWhiteSpace(backupFilePath))
						{
							if (File.Exists(backupFilePath))
							{
								FirebaseAnonymousAuthenticator.MarkBackupCreated(userConfigurationService);
							}
							else
							{
								string directoryName = Path.GetDirectoryName(backupFilePath);
								if (!string.IsNullOrWhiteSpace(directoryName))
								{
									Directory.CreateDirectory(directoryName);
								}
								File.Copy(this._stateFilePath, backupFilePath, false);
								FirebaseAnonymousAuthenticator.MarkBackupCreated(userConfigurationService);
							}
						}
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003C98 File Offset: 0x00001E98
		[NullableContext(2)]
		private static void MarkBackupCreated(UserConfigurationService config)
		{
			if (config != null)
			{
				config.EnablePersistence();
				config.SetFirebaseAuthBackupCreated();
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003CAC File Offset: 0x00001EAC
		[NullableContext(2)]
		private static UserConfigurationService TryGetUserConfiguration()
		{
			UserConfigurationService result;
			try
			{
				result = new UserConfigurationService();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003CD8 File Offset: 0x00001ED8
		[NullableContext(2)]
		internal static string GetBackupFilePath()
		{
			string result;
			try
			{
				string folderPath = Environment.GetFolderPath(28);
				if (string.IsNullOrWhiteSpace(folderPath))
				{
					result = null;
				}
				else
				{
					result = Path.Combine(Path.Combine(folderPath, DevConfig.FirebaseAuthBackupDirectoryName), FirebaseAnonymousAuthenticator.StateFileName);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D28 File Offset: 0x00001F28
		private void DeleteStateFile()
		{
			try
			{
				if (File.Exists(this._stateFilePath))
				{
					File.Delete(this._stateFilePath);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003D80 File Offset: 0x00001F80
		private Task<FirebaseAnonymousAuthenticator.FirebaseAuthState> SignInAsync(CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<SignInAsync>d__40 <SignInAsync>d__;
			<SignInAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseAnonymousAuthenticator.FirebaseAuthState>.Create();
			<SignInAsync>d__.<>4__this = this;
			<SignInAsync>d__.ct = ct;
			<SignInAsync>d__.<>1__state = -1;
			<SignInAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<SignInAsync>d__40>(ref <SignInAsync>d__);
			return <SignInAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003DCC File Offset: 0x00001FCC
		private Task DeleteAccountInternalAsync(string idToken, CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<DeleteAccountInternalAsync>d__41 <DeleteAccountInternalAsync>d__;
			<DeleteAccountInternalAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteAccountInternalAsync>d__.<>4__this = this;
			<DeleteAccountInternalAsync>d__.idToken = idToken;
			<DeleteAccountInternalAsync>d__.ct = ct;
			<DeleteAccountInternalAsync>d__.<>1__state = -1;
			<DeleteAccountInternalAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<DeleteAccountInternalAsync>d__41>(ref <DeleteAccountInternalAsync>d__);
			return <DeleteAccountInternalAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003E20 File Offset: 0x00002020
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<FirebaseAnonymousAuthenticator.FirebaseAuthState> TryRefreshAsync(FirebaseAnonymousAuthenticator.FirebaseAuthState existingState, CancellationToken ct)
		{
			FirebaseAnonymousAuthenticator.<TryRefreshAsync>d__42 <TryRefreshAsync>d__;
			<TryRefreshAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseAnonymousAuthenticator.FirebaseAuthState>.Create();
			<TryRefreshAsync>d__.<>4__this = this;
			<TryRefreshAsync>d__.existingState = existingState;
			<TryRefreshAsync>d__.ct = ct;
			<TryRefreshAsync>d__.<>1__state = -1;
			<TryRefreshAsync>d__.<>t__builder.Start<FirebaseAnonymousAuthenticator.<TryRefreshAsync>d__42>(ref <TryRefreshAsync>d__);
			return <TryRefreshAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003E74 File Offset: 0x00002074
		private static double ParseExpirationSeconds(string expiresIn)
		{
			double num;
			if (double.TryParse(expiresIn, ref num) && num > 0.0)
			{
				return num;
			}
			return 3600.0;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003EA2 File Offset: 0x000020A2
		private static string Truncate(string value, int length)
		{
			if (string.IsNullOrEmpty(value) || value.Length <= length)
			{
				return value;
			}
			return value.Substring(0, length) + "...";
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003ECC File Offset: 0x000020CC
		[NullableContext(2)]
		public static void EnsureStartupBackup(UserConfigurationService config)
		{
			try
			{
				if (config != null && !config.FirebaseAuthBackupCreated)
				{
					string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
					if (!string.IsNullOrWhiteSpace(stateFilePath) && File.Exists(stateFilePath))
					{
						string backupFilePath = FirebaseAnonymousAuthenticator.GetBackupFilePath();
						if (!string.IsNullOrWhiteSpace(backupFilePath))
						{
							if (File.Exists(backupFilePath))
							{
								FirebaseAnonymousAuthenticator.MarkBackupCreated(config);
							}
							else
							{
								string directoryName = Path.GetDirectoryName(backupFilePath);
								if (!string.IsNullOrWhiteSpace(directoryName))
								{
									Directory.CreateDirectory(directoryName);
								}
								File.Copy(stateFilePath, backupFilePath, false);
								FirebaseAnonymousAuthenticator.MarkBackupCreated(config);
							}
						}
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003F84 File Offset: 0x00002184
		public void Dispose()
		{
			object disposeLock = this._disposeLock;
			lock (disposeLock)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			this._stateLock.Dispose();
		}

		// Token: 0x04000058 RID: 88
		private static readonly string SignInEndpoint = DevConfig.FirebaseSignInEndpoint;

		// Token: 0x04000059 RID: 89
		private static readonly string RefreshEndpoint = DevConfig.FirebaseRefreshEndpoint;

		// Token: 0x0400005A RID: 90
		private static readonly string DeleteEndpoint = DevConfig.FirebaseDeleteEndpoint;

		// Token: 0x0400005B RID: 91
		private static readonly string StateFileName = DevConfig.FirebaseAuthStateFileName;

		// Token: 0x0400005C RID: 92
		private static readonly HttpClient HttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};

		// Token: 0x0400005D RID: 93
		private static readonly TimeSpan ExpirationSkew = TimeSpan.FromMinutes(2.0);

		// Token: 0x0400005E RID: 94
		private static readonly string DefaultApiKey = DevConfig.FirebaseDefaultApiKey;

		// Token: 0x0400005F RID: 95
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x04000060 RID: 96
		private readonly string _stateFilePath;

		// Token: 0x04000061 RID: 97
		private readonly SemaphoreSlim _stateLock = new SemaphoreSlim(1, 1);

		// Token: 0x04000062 RID: 98
		private readonly object _disposeLock = new object();

		// Token: 0x04000063 RID: 99
		[Nullable(2)]
		private FirebaseAnonymousAuthenticator.FirebaseAuthState _cachedState;

		// Token: 0x04000064 RID: 100
		private bool _disposed;

		// Token: 0x020000E2 RID: 226
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class AuthStateModel
		{
			// Token: 0x170003F5 RID: 1013
			// (get) Token: 0x06001090 RID: 4240 RVA: 0x00057DEE File Offset: 0x00055FEE
			// (set) Token: 0x06001091 RID: 4241 RVA: 0x00057DF6 File Offset: 0x00055FF6
			public string IdToken { get; set; }

			// Token: 0x170003F6 RID: 1014
			// (get) Token: 0x06001092 RID: 4242 RVA: 0x00057DFF File Offset: 0x00055FFF
			// (set) Token: 0x06001093 RID: 4243 RVA: 0x00057E07 File Offset: 0x00056007
			public string RefreshToken { get; set; }

			// Token: 0x170003F7 RID: 1015
			// (get) Token: 0x06001094 RID: 4244 RVA: 0x00057E10 File Offset: 0x00056010
			// (set) Token: 0x06001095 RID: 4245 RVA: 0x00057E18 File Offset: 0x00056018
			public DateTimeOffset ExpirationUtc { get; set; }

			// Token: 0x170003F8 RID: 1016
			// (get) Token: 0x06001096 RID: 4246 RVA: 0x00057E21 File Offset: 0x00056021
			// (set) Token: 0x06001097 RID: 4247 RVA: 0x00057E29 File Offset: 0x00056029
			public string UserId { get; set; }
		}

		// Token: 0x020000E3 RID: 227
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class SignInResponse
		{
			// Token: 0x170003F9 RID: 1017
			// (get) Token: 0x06001099 RID: 4249 RVA: 0x00057E3A File Offset: 0x0005603A
			// (set) Token: 0x0600109A RID: 4250 RVA: 0x00057E42 File Offset: 0x00056042
			[JsonPropertyName("idToken")]
			public string IdToken { get; set; }

			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x0600109B RID: 4251 RVA: 0x00057E4B File Offset: 0x0005604B
			// (set) Token: 0x0600109C RID: 4252 RVA: 0x00057E53 File Offset: 0x00056053
			[JsonPropertyName("refreshToken")]
			public string RefreshToken { get; set; }

			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x0600109D RID: 4253 RVA: 0x00057E5C File Offset: 0x0005605C
			// (set) Token: 0x0600109E RID: 4254 RVA: 0x00057E64 File Offset: 0x00056064
			[JsonPropertyName("expiresIn")]
			public string ExpiresIn { get; set; }

			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x0600109F RID: 4255 RVA: 0x00057E6D File Offset: 0x0005606D
			// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00057E75 File Offset: 0x00056075
			[JsonPropertyName("localId")]
			public string LocalId { get; set; }
		}

		// Token: 0x020000E4 RID: 228
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class RefreshResponse
		{
			// Token: 0x170003FD RID: 1021
			// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00057E86 File Offset: 0x00056086
			// (set) Token: 0x060010A3 RID: 4259 RVA: 0x00057E8E File Offset: 0x0005608E
			[JsonPropertyName("id_token")]
			public string IdToken { get; set; }

			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00057E97 File Offset: 0x00056097
			// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00057E9F File Offset: 0x0005609F
			[JsonPropertyName("refresh_token")]
			public string RefreshToken { get; set; }

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00057EA8 File Offset: 0x000560A8
			// (set) Token: 0x060010A7 RID: 4263 RVA: 0x00057EB0 File Offset: 0x000560B0
			[JsonPropertyName("expires_in")]
			public string ExpiresIn { get; set; }

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00057EB9 File Offset: 0x000560B9
			// (set) Token: 0x060010A9 RID: 4265 RVA: 0x00057EC1 File Offset: 0x000560C1
			[JsonPropertyName("user_id")]
			public string UserId { get; set; }
		}

		// Token: 0x020000E5 RID: 229
		[Nullable(0)]
		private sealed class FirebaseAuthState
		{
			// Token: 0x060010AB RID: 4267 RVA: 0x00057ED2 File Offset: 0x000560D2
			public FirebaseAuthState(string idToken, string refreshToken, DateTimeOffset expiration, string userId)
			{
				this.IdToken = idToken;
				this.RefreshToken = refreshToken;
				this.Expiration = expiration;
				this.UserId = userId;
			}

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x060010AC RID: 4268 RVA: 0x00057EF7 File Offset: 0x000560F7
			public string IdToken { get; }

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x060010AD RID: 4269 RVA: 0x00057EFF File Offset: 0x000560FF
			public string RefreshToken { get; }

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x060010AE RID: 4270 RVA: 0x00057F07 File Offset: 0x00056107
			public DateTimeOffset Expiration { get; }

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x060010AF RID: 4271 RVA: 0x00057F0F File Offset: 0x0005610F
			public string UserId { get; }

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00057F17 File Offset: 0x00056117
			public bool IsExpired
			{
				get
				{
					return DateTimeOffset.UtcNow >= this.Expiration - FirebaseAnonymousAuthenticator.ExpirationSkew;
				}
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x00057F33 File Offset: 0x00056133
			public FirebaseAnonymousAuthenticator.FirebaseAuthState WithExpiration(DateTimeOffset expiration)
			{
				return new FirebaseAnonymousAuthenticator.FirebaseAuthState(this.IdToken, this.RefreshToken, expiration, this.UserId);
			}
		}

		// Token: 0x020000E6 RID: 230
		[Nullable(0)]
		public readonly struct FirebaseAuthSession
		{
			// Token: 0x060010B2 RID: 4274 RVA: 0x00057F4D File Offset: 0x0005614D
			public FirebaseAuthSession(string idToken, string userId)
			{
				this.IdToken = idToken;
				this.UserId = userId;
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00057F5D File Offset: 0x0005615D
			public string IdToken { get; }

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00057F65 File Offset: 0x00056165
			public string UserId { get; }
		}
	}
}
