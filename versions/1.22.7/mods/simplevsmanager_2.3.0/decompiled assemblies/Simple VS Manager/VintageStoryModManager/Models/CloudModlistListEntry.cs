using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B4 RID: 180
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloudModlistListEntry
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x00053418 File Offset: 0x00051618
		public CloudModlistListEntry(string ownerId, string slotKey, string slotLabel, [Nullable(2)] string name, [Nullable(2)] string description, [Nullable(2)] string version, [Nullable(2)] string uploader, IReadOnlyList<string> mods, string contentJson, DateTimeOffset? dateAdded, [Nullable(2)] string gameVersion, bool isContentComplete)
		{
			if (ownerId == null)
			{
				throw new ArgumentNullException("ownerId");
			}
			this.OwnerId = ownerId;
			if (slotKey == null)
			{
				throw new ArgumentNullException("slotKey");
			}
			this.SlotKey = slotKey;
			if (slotLabel == null)
			{
				throw new ArgumentNullException("slotLabel");
			}
			this.SlotLabel = slotLabel;
			this.Name = (string.IsNullOrWhiteSpace(name) ? null : name.Trim());
			this.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
			this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			this.Uploader = (string.IsNullOrWhiteSpace(uploader) ? this.OwnerId : uploader.Trim());
			List<string> list = (mods != null) ? Enumerable.ToList<string>(mods) : null;
			if (list == null)
			{
				throw new ArgumentNullException("mods");
			}
			this.Mods = list;
			if (contentJson == null)
			{
				throw new ArgumentNullException("contentJson");
			}
			this.ContentJson = contentJson;
			this.DateAdded = dateAdded;
			this.GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? null : gameVersion.Trim());
			this.IsContentComplete = isContentComplete;
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x00053539 File Offset: 0x00051739
		public string OwnerId { get; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00053541 File Offset: 0x00051741
		public string SlotKey { get; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00053549 File Offset: 0x00051749
		public string SlotLabel { get; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x00053551 File Offset: 0x00051751
		[Nullable(2)]
		public string Name { [NullableContext(2)] get; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x00053559 File Offset: 0x00051759
		[Nullable(2)]
		public string Description { [NullableContext(2)] get; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x00053561 File Offset: 0x00051761
		[Nullable(2)]
		public string Version { [NullableContext(2)] get; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00053569 File Offset: 0x00051769
		public string Uploader { get; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x00053571 File Offset: 0x00051771
		public IReadOnlyList<string> Mods { get; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00053579 File Offset: 0x00051779
		public string ContentJson { get; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00053581 File Offset: 0x00051781
		public DateTimeOffset? DateAdded { get; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00053589 File Offset: 0x00051789
		[Nullable(2)]
		public string GameVersion { [NullableContext(2)] get; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x00053591 File Offset: 0x00051791
		public bool IsContentComplete { get; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x00053599 File Offset: 0x00051799
		public string DisplayName
		{
			get
			{
				return this.Name ?? "Unnamed Modlist";
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x000535AC File Offset: 0x000517AC
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

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00053610 File Offset: 0x00051810
		public string DateAddedDisplay
		{
			get
			{
				DateTimeOffset? dateTimeOffset;
				return ((this.DateAdded != null) ? dateTimeOffset.GetValueOrDefault().ToLocalTime().ToString("g", CultureInfo.CurrentCulture) : null) ?? "—";
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0005365A File Offset: 0x0005185A
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
