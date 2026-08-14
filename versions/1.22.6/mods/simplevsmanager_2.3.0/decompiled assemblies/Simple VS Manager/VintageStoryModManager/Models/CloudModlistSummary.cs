using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class CloudModlistSummary
	{
		// Token: 0x06000F45 RID: 3909 RVA: 0x00053678 File Offset: 0x00051878
		public CloudModlistSummary(string name, string description, string version, string uploader, [Nullable(1)] IReadOnlyList<CloudModlistSummary.ModReference> mods, string gameVersion)
		{
			this.Name = (string.IsNullOrWhiteSpace(name) ? null : name.Trim());
			this.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
			this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			this.Uploader = (string.IsNullOrWhiteSpace(uploader) ? null : uploader.Trim());
			this.Mods = (mods ?? Array.Empty<CloudModlistSummary.ModReference>());
			this.GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? null : gameVersion.Trim());
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x00053713 File Offset: 0x00051913
		public string Name { get; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0005371B File Offset: 0x0005191B
		public string Description { get; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x00053723 File Offset: 0x00051923
		public string Version { get; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0005372B File Offset: 0x0005192B
		public string Uploader { get; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x00053733 File Offset: 0x00051933
		[Nullable(1)]
		public IReadOnlyList<CloudModlistSummary.ModReference> Mods { [NullableContext(1)] get; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0005373B File Offset: 0x0005193B
		public string GameVersion { get; }

		// Token: 0x06000F4C RID: 3916 RVA: 0x00053744 File Offset: 0x00051944
		[NullableContext(1)]
		public static CloudModlistSummary FromJsonElement(JsonElement element)
		{
			if (element.ValueKind != 1)
			{
				return CloudModlistSummary.Empty;
			}
			string name = CloudModlistSummary.TryGetTrimmedProperty(element, "name");
			string description = CloudModlistSummary.TryGetTrimmedProperty(element, "description");
			string version = CloudModlistSummary.TryGetTrimmedProperty(element, "version");
			string uploader = CloudModlistSummary.TryGetTrimmedProperty(element, "uploader") ?? CloudModlistSummary.TryGetTrimmedProperty(element, "uploaderName");
			string gameVersion = CloudModlistSummary.TryGetTrimmedProperty(element, "gameVersion") ?? CloudModlistSummary.TryGetTrimmedProperty(element, "vsVersion");
			List<CloudModlistSummary.ModReference> list = new List<CloudModlistSummary.ModReference>();
			JsonElement jsonElement;
			if (element.TryGetProperty("mods", ref jsonElement) && jsonElement.ValueKind == 2)
			{
				foreach (JsonElement element2 in jsonElement.EnumerateArray())
				{
					if (element2.ValueKind == 1)
					{
						string text = CloudModlistSummary.TryGetTrimmedProperty(element2, "modId");
						if (!string.IsNullOrWhiteSpace(text))
						{
							string version2 = CloudModlistSummary.TryGetTrimmedProperty(element2, "version");
							list.Add(new CloudModlistSummary.ModReference(text, version2));
						}
					}
				}
			}
			return new CloudModlistSummary(name, description, version, uploader, list, gameVersion);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00053870 File Offset: 0x00051A70
		[NullableContext(1)]
		public object ToFirebasePayload(string dateAddedIso)
		{
			return new <>f__AnonymousType0<<>f__AnonymousType1<string, string, string, string, string, string, string, IEnumerable<<>f__AnonymousType2<string, string>>>, string>(new <>f__AnonymousType1<string, string, string, string, string, string, string, IEnumerable<<>f__AnonymousType2<string, string>>>(this.Name, this.Description, this.Version, this.Uploader, this.Uploader, this.GameVersion, this.GameVersion, Enumerable.Select(this.Mods, (CloudModlistSummary.ModReference m) => new <>f__AnonymousType2<string, string>(m.ModId, m.Version))), dateAddedIso);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000538DC File Offset: 0x00051ADC
		[NullableContext(1)]
		[return: Nullable(2)]
		private static string TryGetTrimmedProperty(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (element.ValueKind != 1 || !element.TryGetProperty(propertyName, ref jsonElement) || jsonElement.ValueKind != 3)
			{
				return null;
			}
			string @string = jsonElement.GetString();
			if (!string.IsNullOrWhiteSpace(@string))
			{
				return @string.Trim();
			}
			return null;
		}

		// Token: 0x040006AD RID: 1709
		[Nullable(1)]
		public static readonly CloudModlistSummary Empty = new CloudModlistSummary(null, null, null, null, Array.Empty<CloudModlistSummary.ModReference>(), null);

		// Token: 0x02000316 RID: 790
		[NullableContext(1)]
		[Nullable(0)]
		public readonly struct ModReference
		{
			// Token: 0x0600199C RID: 6556 RVA: 0x00093876 File Offset: 0x00091A76
			public ModReference(string modId, [Nullable(2)] string version)
			{
				if (modId == null)
				{
					throw new ArgumentNullException("modId");
				}
				this.ModId = modId;
				this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			}

			// Token: 0x17000555 RID: 1365
			// (get) Token: 0x0600199D RID: 6557 RVA: 0x000938A5 File Offset: 0x00091AA5
			public string ModId { get; }

			// Token: 0x17000556 RID: 1366
			// (get) Token: 0x0600199E RID: 6558 RVA: 0x000938AD File Offset: 0x00091AAD
			[Nullable(2)]
			public string Version { [NullableContext(2)] get; }
		}
	}
}
