using System;

namespace Concentus.Native
{
	// Token: 0x02000075 RID: 117
	internal readonly struct OSAndArchitecture : IEquatable<OSAndArchitecture>
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00023866 File Offset: 0x00021A66
		public PlatformOperatingSystem OS { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0002386E File Offset: 0x00021A6E
		public PlatformArchitecture Architecture { get; }

		// Token: 0x06000313 RID: 787 RVA: 0x00023876 File Offset: 0x00021A76
		public OSAndArchitecture(PlatformOperatingSystem OS, PlatformArchitecture architecture)
		{
			this.OS = OS;
			this.Architecture = architecture;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00023886 File Offset: 0x00021A86
		public override string ToString()
		{
			return this.OS.GetRuntimeIdString() + "-" + this.Architecture.GetRuntimeIdString();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000238A8 File Offset: 0x00021AA8
		public override int GetHashCode()
		{
			return this.OS.GetHashCode() * 17 + this.Architecture.GetHashCode() * 37119;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x000238E7 File Offset: 0x00021AE7
		public override bool Equals(object other)
		{
			return other != null && !(base.GetType() != other.GetType()) && this.Equals((OSAndArchitecture)other);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00023917 File Offset: 0x00021B17
		public bool Equals(OSAndArchitecture other)
		{
			return this.OS == other.OS && this.Architecture == other.Architecture;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00023939 File Offset: 0x00021B39
		public static bool operator ==(OSAndArchitecture left, OSAndArchitecture right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00023943 File Offset: 0x00021B43
		public static bool operator !=(OSAndArchitecture left, OSAndArchitecture right)
		{
			return !(left == right);
		}
	}
}
