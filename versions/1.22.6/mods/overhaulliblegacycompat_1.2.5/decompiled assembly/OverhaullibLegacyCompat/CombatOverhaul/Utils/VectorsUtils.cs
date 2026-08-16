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
		// Token: 0x0600034B RID: 843 RVA: 0x0000F648 File Offset: 0x0000D848
		public static Vector2 ToSystem(this Vector2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F65B File Offset: 0x0000D85B
		public static Vector2 ToOpenTK(this Vector2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000F66E File Offset: 0x0000D86E
		public static Vector3 ToSystem(this Vector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000F687 File Offset: 0x0000D887
		public static Vector3 ToOpenTK(this Vector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000F6A0 File Offset: 0x0000D8A0
		public static Vector4 ToSystem(this Vector4 value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000F6BF File Offset: 0x0000D8BF
		public static Vector4 ToOpenTK(this Vector4 value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		public static Vector2 ToSystem(this Vector2d value)
		{
			return new Vector2((float)value.X, (float)value.Y);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000F6F3 File Offset: 0x0000D8F3
		public static Vector3 ToSystem(this Vector3d value)
		{
			return new Vector3((float)value.X, (float)value.Y, (float)value.Z);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000F70F File Offset: 0x0000D90F
		public static Vector4 ToSystem(this Vector4d value)
		{
			return new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F732 File Offset: 0x0000D932
		public static Vector3d ToOpenTK(this Vec3d value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000F74B File Offset: 0x0000D94B
		public static Vector3d ToOpenTK(this FastVec3d value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000F764 File Offset: 0x0000D964
		public static Vector3d ToOpenTK(this EntityPos value)
		{
			return new Vector3d(value.X, value.Y, value.Z);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F77D File Offset: 0x0000D97D
		public static Vector3d ToOpenTKWithDimension(this EntityPos value)
		{
			return new Vector3d(value.X, value.InternalY, value.Z);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F796 File Offset: 0x0000D996
		public static Vector3 ToOpenTK(this Vec3f value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000F7AF File Offset: 0x0000D9AF
		public static Vector3 ToOpenTK(this FastVec3f value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
		public static FastVec3d ToVanilla(this Vector3d value)
		{
			return new FastVec3d(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F7E1 File Offset: 0x0000D9E1
		public static FastVec3f ToVanilla(this Vector3 value)
		{
			return new FastVec3f(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000F7FA File Offset: 0x0000D9FA
		public static Vec3d ToVanillaRef(this Vector3d value)
		{
			return new Vec3d(value.X, value.Y, value.Z);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000F813 File Offset: 0x0000DA13
		public static Vec3f ToVanillaRef(this Vector3 value)
		{
			return new Vec3f(value.X, value.Y, value.Z);
		}
	}
}
