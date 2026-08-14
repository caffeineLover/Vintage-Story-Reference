using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000010 RID: 16
	public struct ALDevice : IEquatable<ALDevice>
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002704 File Offset: 0x00000904
		public ALDevice(IntPtr handle)
		{
			this.Handle = handle;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002710 File Offset: 0x00000910
		public override bool Equals(object obj)
		{
			if (obj is ALDevice)
			{
				ALDevice other = (ALDevice)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002735 File Offset: 0x00000935
		public bool Equals([AllowNull] ALDevice other)
		{
			return this.Handle.Equals(other.Handle);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000274D File Offset: 0x0000094D
		public override int GetHashCode()
		{
			return HashCode.Combine<IntPtr>(this.Handle);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000275A File Offset: 0x0000095A
		public static bool operator ==(ALDevice left, ALDevice right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002764 File Offset: 0x00000964
		public static bool operator !=(ALDevice left, ALDevice right)
		{
			return !(left == right);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002770 File Offset: 0x00000970
		public static implicit operator IntPtr(ALDevice device)
		{
			return device.Handle;
		}

		// Token: 0x0400003D RID: 61
		public static readonly ALDevice Null = new ALDevice(IntPtr.Zero);

		// Token: 0x0400003E RID: 62
		public IntPtr Handle;
	}
}
