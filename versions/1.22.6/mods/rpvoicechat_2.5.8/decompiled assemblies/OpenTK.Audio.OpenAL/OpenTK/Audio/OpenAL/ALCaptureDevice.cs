using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000004 RID: 4
	public struct ALCaptureDevice : IEquatable<ALCaptureDevice>
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public ALCaptureDevice(IntPtr handle)
		{
			this.Handle = handle;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000206C File Offset: 0x0000026C
		public override bool Equals(object obj)
		{
			if (obj is ALCaptureDevice)
			{
				ALCaptureDevice other = (ALCaptureDevice)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002091 File Offset: 0x00000291
		public bool Equals([AllowNull] ALCaptureDevice other)
		{
			return this.Handle.Equals(other.Handle);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020A9 File Offset: 0x000002A9
		public override int GetHashCode()
		{
			return HashCode.Combine<IntPtr>(this.Handle);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020B6 File Offset: 0x000002B6
		public static bool operator ==(ALCaptureDevice left, ALCaptureDevice right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020C0 File Offset: 0x000002C0
		public static bool operator !=(ALCaptureDevice left, ALCaptureDevice right)
		{
			return !(left == right);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020CC File Offset: 0x000002CC
		public static implicit operator IntPtr(ALCaptureDevice device)
		{
			return device.Handle;
		}

		// Token: 0x04000001 RID: 1
		public static readonly ALCaptureDevice Null = new ALCaptureDevice(IntPtr.Zero);

		// Token: 0x04000002 RID: 2
		public IntPtr Handle;
	}
}
