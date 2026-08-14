using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LocalModlistListEntry
	{
		// Token: 0x06000F50 RID: 3920 RVA: 0x0005393C File Offset: 0x00051B3C
		[NullableContext(2)]
		public LocalModlistListEntry([Nullable(1)] string filePath, string name, string description, string version, string uploader, [Nullable(1)] IReadOnlyList<string> mods, DateTimeOffset? lastModified, string gameVersion, SerializablePreset sourcePreset = null, ModlistGroupDefinition groupDefinition = null, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModlistGroupConflict> groupConflicts = null, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> missingGroupMembers = null, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModlistGroupMemberListEntry> groupMembers = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(filePath, "filePath");
			this.FilePath = filePath;
			this.FileName = Path.GetFileName(filePath);
			this.Name = (string.IsNullOrWhiteSpace(name) ? null : name.Trim());
			this.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
			this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			this.Uploader = (string.IsNullOrWhiteSpace(uploader) ? null : uploader.Trim());
			List<string> list = (mods != null) ? Enumerable.ToList<string>(mods) : null;
			if (list == null)
			{
				throw new ArgumentNullException("mods");
			}
			this.Mods = list;
			this.LastModified = lastModified;
			this.GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? null : gameVersion.Trim());
			this.SourcePreset = sourcePreset;
			this.GroupDefinition = groupDefinition;
			IReadOnlyList<ModlistGroupConflict> readOnlyList2;
			if (groupConflicts != null)
			{
				IReadOnlyList<ModlistGroupConflict> readOnlyList = Enumerable.ToList<ModlistGroupConflict>(groupConflicts);
				readOnlyList2 = readOnlyList;
			}
			else
			{
				IReadOnlyList<ModlistGroupConflict> readOnlyList = Array.Empty<ModlistGroupConflict>();
				readOnlyList2 = readOnlyList;
			}
			this.GroupConflicts = readOnlyList2;
			IReadOnlyList<string> readOnlyList4;
			if (missingGroupMembers != null)
			{
				IReadOnlyList<string> readOnlyList3 = Enumerable.ToList<string>(missingGroupMembers);
				readOnlyList4 = readOnlyList3;
			}
			else
			{
				IReadOnlyList<string> readOnlyList3 = Array.Empty<string>();
				readOnlyList4 = readOnlyList3;
			}
			this.MissingGroupMembers = readOnlyList4;
			IReadOnlyList<ModlistGroupMemberListEntry> readOnlyList6;
			if (groupMembers != null)
			{
				IReadOnlyList<ModlistGroupMemberListEntry> readOnlyList5 = Enumerable.ToList<ModlistGroupMemberListEntry>(groupMembers);
				readOnlyList6 = readOnlyList5;
			}
			else
			{
				IReadOnlyList<ModlistGroupMemberListEntry> readOnlyList5 = Array.Empty<ModlistGroupMemberListEntry>();
				readOnlyList6 = readOnlyList5;
			}
			this.GroupMembers = readOnlyList6;
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00053A7C File Offset: 0x00051C7C
		public string FilePath { get; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00053A84 File Offset: 0x00051C84
		public string FileName { get; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x00053A8C File Offset: 0x00051C8C
		[Nullable(2)]
		public string Name { [NullableContext(2)] get; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x00053A94 File Offset: 0x00051C94
		[Nullable(2)]
		public string Description { [NullableContext(2)] get; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00053A9C File Offset: 0x00051C9C
		[Nullable(2)]
		public string Version { [NullableContext(2)] get; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x00053AA4 File Offset: 0x00051CA4
		[Nullable(2)]
		public string Uploader { [NullableContext(2)] get; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x00053AAC File Offset: 0x00051CAC
		public IReadOnlyList<string> Mods { get; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x00053AB4 File Offset: 0x00051CB4
		public DateTimeOffset? LastModified { get; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00053ABC File Offset: 0x00051CBC
		[Nullable(2)]
		public string GameVersion { [NullableContext(2)] get; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x00053AC4 File Offset: 0x00051CC4
		[Nullable(2)]
		public SerializablePreset SourcePreset { [NullableContext(2)] get; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x00053ACC File Offset: 0x00051CCC
		[Nullable(2)]
		public ModlistGroupDefinition GroupDefinition { [NullableContext(2)] get; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x00053AD4 File Offset: 0x00051CD4
		public IReadOnlyList<ModlistGroupConflict> GroupConflicts { get; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x00053ADC File Offset: 0x00051CDC
		public IReadOnlyList<string> MissingGroupMembers { get; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00053AE4 File Offset: 0x00051CE4
		public IReadOnlyList<ModlistGroupMemberListEntry> GroupMembers { get; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x00053AEC File Offset: 0x00051CEC
		public bool IsGroup
		{
			get
			{
				return this.GroupDefinition != null;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x00053AFA File Offset: 0x00051CFA
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x00053B02 File Offset: 0x00051D02
		public bool IsGroupExpanded { get; set; } = true;

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00053B0B File Offset: 0x00051D0B
		public string BaseDisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Name))
				{
					return this.Name;
				}
				return Path.GetFileNameWithoutExtension(this.FileName);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x00053B2C File Offset: 0x00051D2C
		public string DisplayName
		{
			get
			{
				return this.BaseDisplayName;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x00053B34 File Offset: 0x00051D34
		public string UploaderDisplay
		{
			get
			{
				if (this.IsGroup)
				{
					return "Modlist Group";
				}
				if (!string.IsNullOrWhiteSpace(this.Uploader))
				{
					return this.Uploader;
				}
				return "—";
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x00053B60 File Offset: 0x00051D60
		public string ModsSummary
		{
			get
			{
				if (this.Mods.Count == 0)
				{
					return "No mods";
				}
				if (this.Mods.Count != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(5, 1);
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Mods.Count);
					defaultInterpolatedStringHandler.AppendLiteral(" mods");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "1 mod";
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x00053BC4 File Offset: 0x00051DC4
		public string LastModifiedDisplay
		{
			get
			{
				DateTimeOffset? dateTimeOffset;
				return ((this.LastModified != null) ? dateTimeOffset.GetValueOrDefault().ToLocalTime().ToString("g", CultureInfo.CurrentCulture) : null) ?? "—";
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x00053C0E File Offset: 0x00051E0E
		public string GameVersionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.GameVersion))
				{
					return this.GameVersion;
				}
				return "—";
			}
		}
	}
}
