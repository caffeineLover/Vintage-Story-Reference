using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000005 RID: 5
	public struct ALContext : IEquatable<ALContext>
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000020E5 File Offset: 0x000002E5
		public ALContext(IntPtr handle)
		{
			this.Handle = handle;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020F0 File Offset: 0x000002F0
		public override bool Equals(object obj)
		{
			if (obj is ALContext)
			{
				ALContext other = (ALContext)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002115 File Offset: 0x00000315
		public bool Equals([AllowNull] ALContext other)
		{
			return this.Handle.Equals(other.Handle);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000212D File Offset: 0x0000032D
		public override int GetHashCode()
		{
			return HashCode.Combine<IntPtr>(this.Handle);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000213A File Offset: 0x0000033A
		public static bool operator ==(ALContext left, ALContext right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002144 File Offset: 0x00000344
		public static bool operator !=(ALContext left, ALContext right)
		{
			return !(left == right);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002150 File Offset: 0x00000350
		public static implicit operator IntPtr(ALContext context)
		{
			return context.Handle;
		}

		// Token: 0x04000003 RID: 3
		public static readonly ALContext Null = new ALContext(IntPtr.Zero);

		// Token: 0x04000004 RID: 4
		public IntPtr Handle;
	}
}
