using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	internal static class VectorsUtils
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000F688 File Offset: 0x0000D888
		public static Vector2 ToSystem(this Vector2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F69B File Offset: 0x0000D89B
		public static Vector2 ToOpenTK(this Vector2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000F6AE File Offset: 0x0000D8AE
		public static Vector3 ToSystem(this Vector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000F6C7 File Offset: 0x0000D8C7
		public static Vector3 ToOpenTK(this Vector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		public static Vector4 ToSystem(this Vector4 value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000F6FF File Offset: 0x0000D8FF
		public static Vector4 ToOpenTK(this Vector4 value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000F71E File Offset: 0x0000D91E
		public static Vector2 ToSystem(this Vector2d value)
		{
			return new Vector2((float)value.X, (float)value.Y);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000F733 File Offset: 0x0000D933
		public static Vector3 ToSystem(this Vector3d value)
		{
			return new Vector3((float)value.X, (float)value.Y, (float)value.Z);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000F74F File Offset: 0x0000D94F
		public static Vector4 ToSystem(this Vector4d value)
		{
			return new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F772 File Offset: 0x0000D972
		public static Vector3d ToOpenTK(this Vec3d value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000F78B File Offset: 0x0000D98B
		public static Vector3d ToOpenTK(this FastVec3d value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		public static Vector3d ToOpenTK(this EntityPos value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F7BD File Offset: 0x0000D9BD
		public static Vector3d ToOpenTKWithDimension(this EntityPos value)
		{
			return new Vector3d(value.X, value.InternalY, value.Z);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F7D6 File Offset: 0x0000D9D6
		public static Vector3 ToOpenTK(this Vec3f value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000F7EF File Offset: 0x0000D9EF
		public static Vector3 ToOpenTK(this FastVec3f value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000F808 File Offset: 0x0000DA08
		public static FastVec3d ToVanilla(this Vector3d value)
		{
			return new FastVec3d(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F821 File Offset: 0x0000DA21
		public static FastVec3f ToVanilla(this Vector3 value)
		{
			return new FastVec3f(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000F83A File Offset: 0x0000DA3A
		public static Vec3d ToVanillaRef(this Vector3d value)
		{
			return new Vec3d(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000F853 File Offset: 0x0000DA53
		public static Vec3f ToVanillaRef(this Vector3 value)
		{
			return new Vec3f(value.X, value.Y, value.Z);
		}
	}
}
