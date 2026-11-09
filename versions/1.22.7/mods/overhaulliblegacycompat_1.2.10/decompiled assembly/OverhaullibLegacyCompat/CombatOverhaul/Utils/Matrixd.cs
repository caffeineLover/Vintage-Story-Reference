using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	public class Matrixd
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000F86C File Offset: 0x0000DA6C
		public double[] ValuesAsDouble
		{
			get
			{
				double[] values = new double[16];
				for (int i = 0; i < 16; i++)
				{
					values[i] = this.Values[i];
				}
				return values;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000F89A File Offset: 0x0000DA9A
		public Matrixd()
		{
			this.Values = Mat4d.Create();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000F8AD File Offset: 0x0000DAAD
		public Matrixd(double[] values)
		{
			this.Values = Mat4d.Create();
			this.Set(values);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
		public static Matrixd Create()
		{
			return new Matrixd();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000F8CF File Offset: 0x0000DACF
		public Matrixd Identity()
		{
			Mat4d.Identity(this.Values);
			return this;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		public Matrixd Set(double[] values)
		{
			this.Values[0] = values[0];
			this.Values[1] = values[1];
			this.Values[2] = values[2];
			this.Values[3] = values[3];
			this.Values[4] = values[4];
			this.Values[5] = values[5];
			this.Values[6] = values[6];
			this.Values[7] = values[7];
			this.Values[8] = values[8];
			this.Values[9] = values[9];
			this.Values[10] = values[10];
			this.Values[11] = values[11];
			this.Values[12] = values[12];
			this.Values[13] = values[13];
			this.Values[14] = values[14];
			this.Values[15] = values[15];
			return this;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F9AC File Offset: 0x0000DBAC
		public Matrixd Set(float[] values)
		{
			this.Values[0] = (double)values[0];
			this.Values[1] = (double)values[1];
			this.Values[2] = (double)values[2];
			this.Values[3] = (double)values[3];
			this.Values[4] = (double)values[4];
			this.Values[5] = (double)values[5];
			this.Values[6] = (double)values[6];
			this.Values[7] = (double)values[7];
			this.Values[8] = (double)values[8];
			this.Values[9] = (double)values[9];
			this.Values[10] = (double)values[10];
			this.Values[11] = (double)values[11];
			this.Values[12] = (double)values[12];
			this.Values[13] = (double)values[13];
			this.Values[14] = (double)values[14];
			this.Values[15] = (double)values[15];
			return this;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public Matrixd Translate(double x, double y, double z)
		{
			Mat4d.Translate(this.Values, this.Values, x, y, z);
			return this;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000FAA3 File Offset: 0x0000DCA3
		public Matrixd Translate(Vec3f vec)
		{
			Mat4d.Translate(this.Values, this.Values, (double)vec.X, (double)vec.Y, (double)vec.Z);
			return this;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000FACD File Offset: 0x0000DCCD
		public Matrixd Translate(float x, float y, float z)
		{
			Mat4d.Translate(this.Values, this.Values, (double)x, (double)y, (double)z);
			return this;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public Matrixd Scale(double x, double y, double z)
		{
			Mat4d.Scale(this.Values, this.Values, new double[]
			{
				x,
				y,
				z
			});
			return this;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000FB10 File Offset: 0x0000DD10
		public Matrixd RotateDeg(Vec3f degrees)
		{
			Mat4d.RotateX(this.Values, this.Values, (double)(degrees.X * 0.017453292f));
			Mat4d.RotateY(this.Values, this.Values, (double)(degrees.Y * 0.017453292f));
			Mat4d.RotateZ(this.Values, this.Values, (double)(degrees.Z * 0.017453292f));
			return this;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		public Matrixd Rotate(Vec3f radians)
		{
			Mat4d.RotateX(this.Values, this.Values, (double)radians.X);
			Mat4d.RotateY(this.Values, this.Values, (double)radians.Y);
			Mat4d.RotateZ(this.Values, this.Values, (double)radians.Z);
			return this;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
		public Matrixd Rotate(double radX, double radY, double radZ)
		{
			Mat4d.RotateX(this.Values, this.Values, radX);
			Mat4d.RotateY(this.Values, this.Values, radY);
			Mat4d.RotateZ(this.Values, this.Values, radZ);
			return this;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FC11 File Offset: 0x0000DE11
		public Matrixd RotateX(double radX)
		{
			Mat4d.RotateX(this.Values, this.Values, radX);
			return this;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000FC27 File Offset: 0x0000DE27
		public Matrixd RotateY(double radY)
		{
			Mat4d.RotateY(this.Values, this.Values, radY);
			return this;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000FC3D File Offset: 0x0000DE3D
		public Matrixd RotateZ(double radZ)
		{
			Mat4d.RotateZ(this.Values, this.Values, radZ);
			return this;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000FC53 File Offset: 0x0000DE53
		public Matrixd RotateXDeg(double degX)
		{
			Mat4d.RotateX(this.Values, this.Values, degX * 0.01745329238474369);
			return this;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000FC73 File Offset: 0x0000DE73
		public Matrixd RotateYDeg(double degY)
		{
			Mat4d.RotateY(this.Values, this.Values, degY * 0.01745329238474369);
			return this;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000FC93 File Offset: 0x0000DE93
		public Matrixd RotateZDeg(double degZ)
		{
			Mat4d.RotateZ(this.Values, this.Values, degZ * 0.01745329238474369);
			return this;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		public Vec4d TransformVector(Vec4d vec)
		{
			Vec4d outval = new Vec4d();
			Mat4d.MulWithVec4(this.Values, vec, outval);
			return outval;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000FCD5 File Offset: 0x0000DED5
		public Matrixd Mul(double[] matrix)
		{
			Mat4d.Mul(this.Values, this.Values, matrix);
			return this;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		public Matrixd Mul(Matrixd matrix)
		{
			Mat4d.Mul(this.Values, this.Values, matrix.Values);
			return this;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000FD06 File Offset: 0x0000DF06
		public Matrixd ReverseMul(double[] matrix)
		{
			Mat4d.Mul(this.Values, matrix, this.Values);
			return this;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		public Matrixd FollowPlayer()
		{
			this.Values[12] = 0.0;
			this.Values[13] = 0.0;
			this.Values[14] = 0.0;
			return this;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000FD55 File Offset: 0x0000DF55
		public Matrixd FollowPlayerXZ()
		{
			this.Values[12] = 0.0;
			this.Values[14] = 0.0;
			return this;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000FD7C File Offset: 0x0000DF7C
		public Matrixd Invert()
		{
			Mat4d.Invert(this.Values, this.Values);
			return this;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000FD91 File Offset: 0x0000DF91
		public Matrixd Clone()
		{
			return new Matrixd
			{
				Values = (double[])this.Values.Clone()
			};
		}

		// Token: 0x04000132 RID: 306
		public double[] Values;
	}
}
