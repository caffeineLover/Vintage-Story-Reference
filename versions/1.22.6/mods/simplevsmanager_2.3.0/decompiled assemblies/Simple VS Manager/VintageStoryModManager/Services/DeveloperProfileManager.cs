using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005E RID: 94
	[NullableContext(1)]
	[Nullable(0)]
	public static class DeveloperProfileManager
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00038680 File Offset: 0x00036880
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00038687 File Offset: 0x00036887
		public static bool DevDebug { get; set; } = false;

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00038690 File Offset: 0x00036890
		[Nullable(2)]
		public static DeveloperProfile CurrentProfile
		{
			[NullableContext(2)]
			get
			{
				if (!DeveloperProfileManager.DevDebug)
				{
					return null;
				}
				object syncRoot = DeveloperProfileManager.SyncRoot;
				DeveloperProfile currentProfile;
				lock (syncRoot)
				{
					currentProfile = DeveloperProfileManager._currentProfile;
				}
				return currentProfile;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000944 RID: 2372 RVA: 0x000386DC File Offset: 0x000368DC
		// (remove) Token: 0x06000945 RID: 2373 RVA: 0x00038710 File Offset: 0x00036910
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static event EventHandler<DeveloperProfileChangedEventArgs> CurrentProfileChanged;

		// Token: 0x06000946 RID: 2374 RVA: 0x00038744 File Offset: 0x00036944
		public static IReadOnlyList<DeveloperProfile> GetProfiles()
		{
			if (!DeveloperProfileManager.DevDebug)
			{
				return Array.Empty<DeveloperProfile>();
			}
			object syncRoot = DeveloperProfileManager.SyncRoot;
			IReadOnlyList<DeveloperProfile> result;
			lock (syncRoot)
			{
				DeveloperProfileManager.EnsureProfilesReadyLocked();
				result = DeveloperProfileManager._profiles.ToArray();
			}
			return result;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0003879C File Offset: 0x0003699C
		public static bool TrySetCurrentProfile(string profileId)
		{
			if (!DeveloperProfileManager.DevDebug)
			{
				return false;
			}
			bool flag = false;
			object syncRoot = DeveloperProfileManager.SyncRoot;
			DeveloperProfile developerProfile;
			lock (syncRoot)
			{
				DeveloperProfileManager.EnsureProfilesReadyLocked();
				developerProfile = Enumerable.FirstOrDefault<DeveloperProfile>(DeveloperProfileManager._profiles, (DeveloperProfile p) => string.Equals(p.Id, profileId, 5));
				if (developerProfile == null)
				{
					return false;
				}
				if (DeveloperProfileManager._currentProfile == null || !string.Equals(DeveloperProfileManager._currentProfile.Id, developerProfile.Id, 5))
				{
					DeveloperProfileManager._currentProfile = developerProfile;
					flag = true;
				}
			}
			if (flag && developerProfile != null)
			{
				DeveloperProfileManager.OnCurrentProfileChanged(developerProfile, false);
			}
			return flag;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0003884C File Offset: 0x00036A4C
		public static void UpdateOriginalProfile(string dataDirectory)
		{
			if (!DeveloperProfileManager.DevDebug || string.IsNullOrWhiteSpace(dataDirectory))
			{
				return;
			}
			DeveloperProfile developerProfile = null;
			bool flag = false;
			object syncRoot = DeveloperProfileManager.SyncRoot;
			lock (syncRoot)
			{
				string fullPath;
				try
				{
					fullPath = Path.GetFullPath(dataDirectory);
				}
				catch (Exception)
				{
					return;
				}
				if (DeveloperProfileManager._originalProfile == null || !string.Equals(DeveloperProfileManager._originalProfile.DataDirectory, fullPath, 5))
				{
					DeveloperProfileManager._originalProfile = new DeveloperProfile("original", "My Profile (Original)", fullPath, null, null, null, true);
					flag = true;
				}
				if (DeveloperProfileManager._profilesRoot == null)
				{
					DeveloperProfileManager._profilesRoot = DeveloperProfileManager.EnsureProfilesRoot();
					flag = true;
				}
				if (flag || DeveloperProfileManager._profiles.Count == 0)
				{
					DeveloperProfileManager.RebuildProfilesLocked();
					flag = true;
				}
				if (DeveloperProfileManager._currentProfile == null || DeveloperProfileManager._currentProfile.IsOriginal)
				{
					DeveloperProfileManager._currentProfile = DeveloperProfileManager._originalProfile;
					developerProfile = DeveloperProfileManager._currentProfile;
				}
				else if (flag)
				{
					developerProfile = DeveloperProfileManager._currentProfile;
				}
				else if (DeveloperProfileManager._originalProfile != null && DeveloperProfileManager._currentProfile == null)
				{
					developerProfile = DeveloperProfileManager._originalProfile;
				}
				if (!flag && developerProfile == null)
				{
					return;
				}
				if (developerProfile == null)
				{
					developerProfile = (DeveloperProfileManager._currentProfile ?? DeveloperProfileManager._originalProfile);
				}
			}
			if (developerProfile != null)
			{
				DeveloperProfileManager.OnCurrentProfileChanged(developerProfile, flag);
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00038980 File Offset: 0x00036B80
		[NullableContext(2)]
		public static string GetFirebaseStateFilePathOverride()
		{
			if (!DeveloperProfileManager.DevDebug)
			{
				return null;
			}
			object syncRoot = DeveloperProfileManager.SyncRoot;
			string result;
			lock (syncRoot)
			{
				DeveloperProfile currentProfile = DeveloperProfileManager._currentProfile;
				string text = (currentProfile != null) ? currentProfile.FirebaseStateFilePath : null;
				if (string.IsNullOrWhiteSpace(text))
				{
					result = null;
				}
				else
				{
					try
					{
						string directoryName = Path.GetDirectoryName(text);
						if (!string.IsNullOrWhiteSpace(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
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
					result = text;
				}
			}
			return result;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00038A2C File Offset: 0x00036C2C
		private static void EnsureProfilesReadyLocked()
		{
			if (DeveloperProfileManager._originalProfile == null)
			{
				DeveloperProfileManager._profiles = new List<DeveloperProfile>();
				return;
			}
			if (DeveloperProfileManager._profilesRoot == null)
			{
				DeveloperProfileManager._profilesRoot = DeveloperProfileManager.EnsureProfilesRoot();
			}
			if (DeveloperProfileManager._profiles.Count == 0)
			{
				DeveloperProfileManager.RebuildProfilesLocked();
			}
			if (DeveloperProfileManager._currentProfile == null)
			{
				DeveloperProfileManager._currentProfile = DeveloperProfileManager._originalProfile;
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00038A80 File Offset: 0x00036C80
		private static void RebuildProfilesLocked()
		{
			List<DeveloperProfile> list = new List<DeveloperProfile>();
			if (DeveloperProfileManager._originalProfile != null)
			{
				list.Add(DeveloperProfileManager._originalProfile);
			}
			if (DeveloperProfileManager._profilesRoot == null)
			{
				DeveloperProfileManager._profiles = list;
				return;
			}
			foreach (DeveloperProfileManager.DeveloperProfileDefinition definition in DeveloperProfileManager.FakeProfiles)
			{
				string text = Path.Combine(DeveloperProfileManager._profilesRoot, definition.Id);
				string dataDirectory = Path.Combine(text, "Data");
				string text2 = Path.Combine(text, "Firebase");
				DeveloperProfileManager.EnsureFakeProfileData(text, dataDirectory, text2, definition);
				string firebaseStateFilePath = Path.Combine(text2, "firebase-auth.json");
				list.Add(new DeveloperProfile(definition.Id, definition.DisplayName, dataDirectory, firebaseStateFilePath, definition.PlayerName, definition.PlayerUid, false));
			}
			DeveloperProfileManager._profiles = list;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00038B48 File Offset: 0x00036D48
		private static string EnsureProfilesRoot()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			string text = string.IsNullOrWhiteSpace(managerDataDirectory) ? Path.Combine(Path.GetTempPath(), "SimpleVSManager", "DeveloperProfiles") : Path.Combine(managerDataDirectory, "DeveloperProfiles");
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00038B8C File Offset: 0x00036D8C
		private static void EnsureFakeProfileData(string profileRoot, string dataDirectory, string firebaseDirectory, DeveloperProfileManager.DeveloperProfileDefinition definition)
		{
			try
			{
				Directory.CreateDirectory(profileRoot);
				Directory.CreateDirectory(dataDirectory);
				Directory.CreateDirectory(firebaseDirectory);
				Directory.CreateDirectory(Path.Combine(dataDirectory, "Mods"));
				Directory.CreateDirectory(Path.Combine(dataDirectory, "ModConfig"));
				JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
				string text = "stringSettings";
				JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
				jsonObject2["playeruid"] = definition.PlayerUid;
				jsonObject2["playername"] = definition.PlayerName;
				jsonObject[text] = jsonObject2;
				jsonObject["stringListSettings"] = new JsonObject(default(JsonNodeOptions?));
				JsonObject jsonObject3 = jsonObject;
				File.WriteAllText(Path.Combine(dataDirectory, "clientsettings.json"), JsonSerializer.Serialize<JsonObject>(jsonObject3, new JsonSerializerOptions
				{
					WriteIndented = true
				}));
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
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00038C94 File Offset: 0x00036E94
		private static void OnCurrentProfileChanged(DeveloperProfile profile, bool definitionsUpdated)
		{
			EventHandler<DeveloperProfileChangedEventArgs> currentProfileChanged = DeveloperProfileManager.CurrentProfileChanged;
			if (currentProfileChanged == null)
			{
				return;
			}
			currentProfileChanged.Invoke(null, new DeveloperProfileChangedEventArgs(profile, definitionsUpdated));
		}

		// Token: 0x0400046B RID: 1131
		private static readonly DeveloperProfileManager.DeveloperProfileDefinition[] FakeProfiles = new DeveloperProfileManager.DeveloperProfileDefinition[]
		{
			new DeveloperProfileManager.DeveloperProfileDefinition("ember-rowan", "Fake: Ember Rowan", "EmberRowan", "c8f8d0a5-1f54-4c0b-93de-314ce521f9db"),
			new DeveloperProfileManager.DeveloperProfileDefinition("juniper-vale", "Fake: Juniper Vale", "JuniperVale", "67f1f0be-18e6-4d1a-86ac-c04dfb75d991"),
			new DeveloperProfileManager.DeveloperProfileDefinition("orion-ashwood", "Fake: Orion Ashwood", "OrionAshwood", "b74cb56b-1af4-4c2c-8a8e-bd13c9bc44b1"),
			new DeveloperProfileManager.DeveloperProfileDefinition("mira-slate", "Fake: Mira Slate", "MiraSlate", "4e902c46-b6c7-4f6a-88a4-425a7980c6b3"),
			new DeveloperProfileManager.DeveloperProfileDefinition("sylas-storm", "Fake: Sylas Storm", "SylasStorm", "df0c1975-3bc4-4f59-9d7f-f2c6c6df2056")
		};

		// Token: 0x0400046C RID: 1132
		private static readonly object SyncRoot = new object();

		// Token: 0x0400046D RID: 1133
		private static List<DeveloperProfile> _profiles = new List<DeveloperProfile>();

		// Token: 0x0400046E RID: 1134
		[Nullable(2)]
		private static DeveloperProfile _originalProfile;

		// Token: 0x0400046F RID: 1135
		[Nullable(2)]
		private static DeveloperProfile _currentProfile;

		// Token: 0x04000470 RID: 1136
		[Nullable(2)]
		private static string _profilesRoot;

		// Token: 0x02000277 RID: 631
		[Nullable(0)]
		private readonly struct DeveloperProfileDefinition : IEquatable<DeveloperProfileManager.DeveloperProfileDefinition>
		{
			// Token: 0x060015E2 RID: 5602 RVA: 0x0007F353 File Offset: 0x0007D553
			public DeveloperProfileDefinition(string Id, string DisplayName, string PlayerName, string PlayerUid)
			{
				this.Id = Id;
				this.DisplayName = DisplayName;
				this.PlayerName = PlayerName;
				this.PlayerUid = PlayerUid;
			}

			// Token: 0x1700048E RID: 1166
			// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0007F372 File Offset: 0x0007D572
			// (set) Token: 0x060015E4 RID: 5604 RVA: 0x0007F37A File Offset: 0x0007D57A
			public string Id { get; set; }

			// Token: 0x1700048F RID: 1167
			// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0007F383 File Offset: 0x0007D583
			// (set) Token: 0x060015E6 RID: 5606 RVA: 0x0007F38B File Offset: 0x0007D58B
			public string DisplayName { get; set; }

			// Token: 0x17000490 RID: 1168
			// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0007F394 File Offset: 0x0007D594
			// (set) Token: 0x060015E8 RID: 5608 RVA: 0x0007F39C File Offset: 0x0007D59C
			public string PlayerName { get; set; }

			// Token: 0x17000491 RID: 1169
			// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0007F3A5 File Offset: 0x0007D5A5
			// (set) Token: 0x060015EA RID: 5610 RVA: 0x0007F3AD File Offset: 0x0007D5AD
			public string PlayerUid { get; set; }

			// Token: 0x060015EB RID: 5611 RVA: 0x0007F3B8 File Offset: 0x0007D5B8
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DeveloperProfileDefinition");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060015EC RID: 5612 RVA: 0x0007F404 File Offset: 0x0007D604
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Id = ");
				builder.Append(this.Id);
				builder.Append(", DisplayName = ");
				builder.Append(this.DisplayName);
				builder.Append(", PlayerName = ");
				builder.Append(this.PlayerName);
				builder.Append(", PlayerUid = ");
				builder.Append(this.PlayerUid);
				return true;
			}

			// Token: 0x060015ED RID: 5613 RVA: 0x0007F476 File Offset: 0x0007D676
			[CompilerGenerated]
			public static bool operator !=(DeveloperProfileManager.DeveloperProfileDefinition left, DeveloperProfileManager.DeveloperProfileDefinition right)
			{
				return !(left == right);
			}

			// Token: 0x060015EE RID: 5614 RVA: 0x0007F482 File Offset: 0x0007D682
			[CompilerGenerated]
			public static bool operator ==(DeveloperProfileManager.DeveloperProfileDefinition left, DeveloperProfileManager.DeveloperProfileDefinition right)
			{
				return left.Equals(right);
			}

			// Token: 0x060015EF RID: 5615 RVA: 0x0007F48C File Offset: 0x0007D68C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<string>.Default.GetHashCode(this.<Id>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<PlayerName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<PlayerUid>k__BackingField);
			}

			// Token: 0x060015F0 RID: 5616 RVA: 0x0007F4EE File Offset: 0x0007D6EE
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is DeveloperProfileManager.DeveloperProfileDefinition && this.Equals((DeveloperProfileManager.DeveloperProfileDefinition)obj);
			}

			// Token: 0x060015F1 RID: 5617 RVA: 0x0007F508 File Offset: 0x0007D708
			[CompilerGenerated]
			public bool Equals(DeveloperProfileManager.DeveloperProfileDefinition other)
			{
				return EqualityComparer<string>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayName>k__BackingField, other.<DisplayName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<PlayerName>k__BackingField, other.<PlayerName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<PlayerUid>k__BackingField, other.<PlayerUid>k__BackingField);
			}

			// Token: 0x060015F2 RID: 5618 RVA: 0x0007F575 File Offset: 0x0007D775
			[CompilerGenerated]
			public void Deconstruct(out string Id, out string DisplayName, out string PlayerName, out string PlayerUid)
			{
				Id = this.Id;
				DisplayName = this.DisplayName;
				PlayerName = this.PlayerName;
				PlayerUid = this.PlayerUid;
			}
		}
	}
}
