using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	[Nullable(0)]
	public static class VintageStoryGameVersionService
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x00050470 File Offset: 0x0004E670
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public static Task<string> GetLatestReleaseVersionAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			VintageStoryGameVersionService.<GetLatestReleaseVersionAsync>d__2 <GetLatestReleaseVersionAsync>d__;
			<GetLatestReleaseVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetLatestReleaseVersionAsync>d__.cancellationToken = cancellationToken;
			<GetLatestReleaseVersionAsync>d__.<>1__state = -1;
			<GetLatestReleaseVersionAsync>d__.<>t__builder.Start<VintageStoryGameVersionService.<GetLatestReleaseVersionAsync>d__2>(ref <GetLatestReleaseVersionAsync>d__);
			return <GetLatestReleaseVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000504B4 File Offset: 0x0004E6B4
		public static Task<IReadOnlyList<string>> GetRecentReleaseVersionsAsync(int count, CancellationToken cancellationToken = default(CancellationToken))
		{
			VintageStoryGameVersionService.<GetRecentReleaseVersionsAsync>d__3 <GetRecentReleaseVersionsAsync>d__;
			<GetRecentReleaseVersionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<string>>.Create();
			<GetRecentReleaseVersionsAsync>d__.count = count;
			<GetRecentReleaseVersionsAsync>d__.cancellationToken = cancellationToken;
			<GetRecentReleaseVersionsAsync>d__.<>1__state = -1;
			<GetRecentReleaseVersionsAsync>d__.<>t__builder.Start<VintageStoryGameVersionService.<GetRecentReleaseVersionsAsync>d__3>(ref <GetRecentReleaseVersionsAsync>d__);
			return <GetRecentReleaseVersionsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00050500 File Offset: 0x0004E700
		private static bool IsSuccessStatus(JsonElement statusCodeElement)
		{
			JsonValueKind valueKind = statusCodeElement.ValueKind;
			bool result;
			if (valueKind != 3)
			{
				int num;
				result = (valueKind == 4 && (statusCodeElement.TryGetInt32(ref num) && num >= 200) && num < 300);
			}
			else
			{
				int num2;
				result = (int.TryParse(statusCodeElement.GetString(), ref num2) && num2 >= 200 && num2 < 300);
			}
			return result;
		}

		// Token: 0x040005CE RID: 1486
		private static readonly string GameVersionsEndpoint = DevConfig.GameVersionsEndpoint;

		// Token: 0x040005CF RID: 1487
		private static readonly HttpClient HttpClient = new HttpClient();

		// Token: 0x0200030F RID: 783
		[Nullable(0)]
		private sealed class VersionEntry : IEquatable<VintageStoryGameVersionService.VersionEntry>
		{
			// Token: 0x0600196E RID: 6510 RVA: 0x000925D3 File Offset: 0x000907D3
			public VersionEntry(string Name, [Nullable(2)] Version ParsedVersion, bool IsPrerelease)
			{
				this.Name = Name;
				this.ParsedVersion = ParsedVersion;
				this.IsPrerelease = IsPrerelease;
				base..ctor();
			}

			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x0600196F RID: 6511 RVA: 0x000925F0 File Offset: 0x000907F0
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(VintageStoryGameVersionService.VersionEntry);
				}
			}

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x06001970 RID: 6512 RVA: 0x000925FC File Offset: 0x000907FC
			// (set) Token: 0x06001971 RID: 6513 RVA: 0x00092604 File Offset: 0x00090804
			public string Name { get; set; }

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x06001972 RID: 6514 RVA: 0x0009260D File Offset: 0x0009080D
			// (set) Token: 0x06001973 RID: 6515 RVA: 0x00092615 File Offset: 0x00090815
			[Nullable(2)]
			public Version ParsedVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x06001974 RID: 6516 RVA: 0x0009261E File Offset: 0x0009081E
			// (set) Token: 0x06001975 RID: 6517 RVA: 0x00092626 File Offset: 0x00090826
			public bool IsPrerelease { get; set; }

			// Token: 0x06001976 RID: 6518 RVA: 0x00092630 File Offset: 0x00090830
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VersionEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001977 RID: 6519 RVA: 0x0009267C File Offset: 0x0009087C
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Name = ");
				builder.Append(this.Name);
				builder.Append(", ParsedVersion = ");
				builder.Append(this.ParsedVersion);
				builder.Append(", IsPrerelease = ");
				builder.Append(this.IsPrerelease.ToString());
				return true;
			}

			// Token: 0x06001978 RID: 6520 RVA: 0x000926E8 File Offset: 0x000908E8
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(VintageStoryGameVersionService.VersionEntry left, VintageStoryGameVersionService.VersionEntry right)
			{
				return !(left == right);
			}

			// Token: 0x06001979 RID: 6521 RVA: 0x000926F4 File Offset: 0x000908F4
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(VintageStoryGameVersionService.VersionEntry left, VintageStoryGameVersionService.VersionEntry right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600197A RID: 6522 RVA: 0x00092708 File Offset: 0x00090908
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Name>k__BackingField)) * -1521134295 + EqualityComparer<Version>.Default.GetHashCode(this.<ParsedVersion>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsPrerelease>k__BackingField);
			}

			// Token: 0x0600197B RID: 6523 RVA: 0x0009276A File Offset: 0x0009096A
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as VintageStoryGameVersionService.VersionEntry);
			}

			// Token: 0x0600197C RID: 6524 RVA: 0x00092778 File Offset: 0x00090978
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(VintageStoryGameVersionService.VersionEntry other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Name>k__BackingField, other.<Name>k__BackingField) && EqualityComparer<Version>.Default.Equals(this.<ParsedVersion>k__BackingField, other.<ParsedVersion>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsPrerelease>k__BackingField, other.<IsPrerelease>k__BackingField));
			}

			// Token: 0x0600197E RID: 6526 RVA: 0x000927F1 File Offset: 0x000909F1
			[CompilerGenerated]
			private VersionEntry(VintageStoryGameVersionService.VersionEntry original)
			{
				this.Name = original.<Name>k__BackingField;
				this.ParsedVersion = original.<ParsedVersion>k__BackingField;
				this.IsPrerelease = original.<IsPrerelease>k__BackingField;
			}

			// Token: 0x0600197F RID: 6527 RVA: 0x0009281D File Offset: 0x00090A1D
			[CompilerGenerated]
			public void Deconstruct(out string Name, [Nullable(2)] out Version ParsedVersion, out bool IsPrerelease)
			{
				Name = this.Name;
				ParsedVersion = this.ParsedVersion;
				IsPrerelease = this.IsPrerelease;
			}
		}

		// Token: 0x02000310 RID: 784
		[Nullable(0)]
		private sealed class VersionEntryComparer : IComparer<VintageStoryGameVersionService.VersionEntry>
		{
			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x06001980 RID: 6528 RVA: 0x00092837 File Offset: 0x00090A37
			public static VintageStoryGameVersionService.VersionEntryComparer Instance { get; } = new VintageStoryGameVersionService.VersionEntryComparer();

			// Token: 0x06001981 RID: 6529 RVA: 0x00092840 File Offset: 0x00090A40
			[NullableContext(2)]
			public int Compare(VintageStoryGameVersionService.VersionEntry x, VintageStoryGameVersionService.VersionEntry y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				int num = VintageStoryGameVersionService.VersionEntryComparer.CompareParsedVersions(x.ParsedVersion, y.ParsedVersion);
				if (num != 0)
				{
					return num;
				}
				if (x.IsPrerelease == y.IsPrerelease)
				{
					return string.Compare(x.Name, y.Name, 5);
				}
				if (!x.IsPrerelease)
				{
					return -1;
				}
				return 1;
			}

			// Token: 0x06001982 RID: 6530 RVA: 0x000928A2 File Offset: 0x00090AA2
			[NullableContext(2)]
			private static int CompareParsedVersions(Version x, Version y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				return y.CompareTo(x);
			}
		}
	}
}
