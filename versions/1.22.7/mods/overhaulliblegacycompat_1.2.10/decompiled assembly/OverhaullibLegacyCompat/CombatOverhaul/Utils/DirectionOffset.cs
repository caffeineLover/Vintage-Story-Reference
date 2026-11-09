using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct DirectionOffset
	{
		// Token: 0x06000262 RID: 610 RVA: 0x00009D98 File Offset: 0x00007F98
		public DirectionOffset(Vector3d direction, Vector3d reference)
		{
			double yawSin = (reference.Z * direction.X - reference.X * direction.Z) / Math.Sqrt((reference.X * reference.X + reference.Z * reference.Z) * (direction.X * direction.X + direction.Z * direction.Z));
			double pitchSin = (reference.Z * direction.Y - reference.Y * direction.Z) / Math.Sqrt((reference.Y * reference.Y + reference.Z * reference.Z) * (direction.Y * direction.Y + direction.Z * direction.Z));
			this.Yaw = Angle.FromRadians((float)GameMath.Clamp(Math.Asin(yawSin), -6.283185307179586, 6.283185307179586));
			this.Pitch = Angle.FromRadians((float)GameMath.Clamp(Math.Asin(pitchSin), -6.283185307179586, 6.283185307179586));
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009EAC File Offset: 0x000080AC
		public DirectionOffset(Vec3d direction, Vec3d reference)
		{
			double yawSin = (reference.Z * direction.X - reference.X * direction.Z) / Math.Sqrt((reference.X * reference.X + reference.Z * reference.Z) * (direction.X * direction.X + direction.Z * direction.Z));
			double pitchSin = (reference.Z * direction.Y - reference.Y * direction.Z) / Math.Sqrt((reference.Y * reference.Y + reference.Z * reference.Z) * (direction.Y * direction.Y + direction.Z * direction.Z));
			this.Yaw = Angle.FromRadians((float)GameMath.Clamp(Math.Asin(yawSin), -6.283185307179586, 6.283185307179586));
			this.Pitch = Angle.FromRadians((float)GameMath.Clamp(Math.Asin(pitchSin), -6.283185307179586, 6.283185307179586));
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009FC0 File Offset: 0x000081C0
		public DirectionOffset(Vec3f direction, Vec3f reference)
		{
			float yawSin = (reference.Z * direction.X - reference.X * direction.Z) / MathF.Sqrt((reference.X * reference.X + reference.Z * reference.Z) * (direction.X * direction.X + direction.Z * direction.Z));
			float pitchSin = (reference.Z * direction.Y - reference.Y * direction.Z) / MathF.Sqrt((reference.Y * reference.Y + reference.Z * reference.Z) * (direction.Y * direction.Y + direction.Z * direction.Z));
			this.Yaw = Angle.FromRadians(GameMath.Clamp(MathF.Asin(yawSin), -6.2831855f, 6.2831855f));
			this.Pitch = Angle.FromRadians(GameMath.Clamp(MathF.Asin(pitchSin), -6.2831855f, 6.2831855f));
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public DirectionOffset(Vector3d direction)
		{
			ValueTuple<Angle, Angle> valueTuple = DirectionOffset.YawPitchFromDirection(direction);
			Angle yaw = valueTuple.Item1;
			Angle pitch = valueTuple.Item2;
			this.Yaw = yaw;
			this.Pitch = pitch;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000A0F0 File Offset: 0x000082F0
		public DirectionOffset(Vec3d directionVanilla)
		{
			ValueTuple<Angle, Angle> valueTuple = DirectionOffset.YawPitchFromDirection(directionVanilla.ToOpenTK());
			Angle yaw = valueTuple.Item1;
			Angle pitch = valueTuple.Item2;
			this.Yaw = yaw;
			this.Pitch = pitch;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000A124 File Offset: 0x00008324
		public DirectionOffset(Vec3f directionVanilla)
		{
			ValueTuple<Angle, Angle> valueTuple = DirectionOffset.YawPitchFromDirection(directionVanilla.ToOpenTK());
			Angle yaw = valueTuple.Item1;
			Angle pitch = valueTuple.Item2;
			this.Yaw = yaw;
			this.Pitch = pitch;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000A15C File Offset: 0x0000835C
		public DirectionOffset(Angle pitch, Angle yaw)
		{
			this.Yaw = yaw;
			this.Pitch = pitch;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A16C File Offset: 0x0000836C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Pitch: ");
			defaultInterpolatedStringHandler.AppendFormatted<Angle>(this.Pitch);
			defaultInterpolatedStringHandler.AppendLiteral(", Yaw: ");
			defaultInterpolatedStringHandler.AppendFormatted<Angle>(this.Yaw);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000A1BC File Offset: 0x000083BC
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			DirectionOffset? directionOffset;
			return (((DirectionOffset?)obj != null) ? new Angle?(directionOffset.GetValueOrDefault().Pitch) : default(Angle?)) == this.Pitch && ((DirectionOffset)obj).Yaw == this.Yaw;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A230 File Offset: 0x00008430
		public override int GetHashCode()
		{
			return new ValueTuple<Angle, Angle>(this.Pitch, this.Yaw).GetHashCode();
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000A25C File Offset: 0x0000845C
		public static DirectionOffset Zero
		{
			get
			{
				return new DirectionOffset(Angle.Zero, Angle.Zero);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000A26D File Offset: 0x0000846D
		public static DirectionOffset FromRadians(float pitch, float yaw)
		{
			return new DirectionOffset(Angle.FromRadians(pitch), Angle.FromRadians(yaw));
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A280 File Offset: 0x00008480
		public static DirectionOffset FromDegrees(float pitch, float yaw)
		{
			return new DirectionOffset(Angle.FromDegrees(pitch), Angle.FromDegrees(yaw));
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A293 File Offset: 0x00008493
		public static DirectionOffset FromMinutes(float pitch, float yaw)
		{
			return new DirectionOffset(Angle.FromMinutes(pitch), Angle.FromMinutes(yaw));
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A2A6 File Offset: 0x000084A6
		public static DirectionOffset FromSeconds(float pitch, float yaw)
		{
			return new DirectionOffset(Angle.FromSeconds(pitch), Angle.FromSeconds(yaw));
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A2B9 File Offset: 0x000084B9
		public static DirectionOffset operator +(DirectionOffset a, DirectionOffset b)
		{
			return new DirectionOffset(a.Pitch + b.Pitch, a.Yaw + b.Yaw);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A2E2 File Offset: 0x000084E2
		public static DirectionOffset operator -(DirectionOffset a, DirectionOffset b)
		{
			return new DirectionOffset(a.Pitch - b.Pitch, a.Yaw - b.Yaw);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A30B File Offset: 0x0000850B
		public static DirectionOffset operator *(DirectionOffset a, float b)
		{
			return new DirectionOffset(a.Pitch * b, a.Yaw * b);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A32A File Offset: 0x0000852A
		public static DirectionOffset operator *(float a, DirectionOffset b)
		{
			return new DirectionOffset(a * b.Pitch, a * b.Yaw);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A349 File Offset: 0x00008549
		public static DirectionOffset operator /(DirectionOffset a, float b)
		{
			return new DirectionOffset(a.Pitch / b, a.Yaw / b);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A368 File Offset: 0x00008568
		public static bool operator ==(DirectionOffset a, DirectionOffset b)
		{
			return a.Pitch == b.Pitch && a.Yaw == b.Yaw;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A390 File Offset: 0x00008590
		public static bool operator !=(DirectionOffset a, DirectionOffset b)
		{
			return !(a == b);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A39C File Offset: 0x0000859C
		public static bool operator <(DirectionOffset a, DirectionOffset b)
		{
			return a.Pitch < b.Pitch && a.Yaw < b.Yaw;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A3C4 File Offset: 0x000085C4
		public static bool operator >(DirectionOffset a, DirectionOffset b)
		{
			return a.Pitch > b.Pitch && a.Yaw > b.Yaw;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A3EC File Offset: 0x000085EC
		public static bool operator <=(DirectionOffset a, DirectionOffset b)
		{
			return !(a > b);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public static bool operator >=(DirectionOffset a, DirectionOffset b)
		{
			return !(a < b);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A404 File Offset: 0x00008604
		public static Vec3f FromCameraReferenceFrame(EntityAgent player, Vec3f position)
		{
			Vec3f vec3f = player.SidedPos.GetViewVector().Normalize();
			Vec3f vertical = new Vec3f(0f, 1f, 0f);
			Vec3f localZ = vec3f;
			Vec3f vec3f2 = vec3f.Cross(vertical).Normalize();
			Vec3f localY = vec3f2.Cross(localZ);
			return vec3f2 * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A478 File Offset: 0x00008678
		public static Vec3d FromCameraReferenceFrame(EntityAgent player, Vec3d position)
		{
			Vec3f viewVectorF = player.SidedPos.GetViewVector();
			Vec3d vec3d = new Vec3d((double)viewVectorF.X, (double)viewVectorF.Y, (double)viewVectorF.Z);
			Vec3d vertical = new Vec3d(0.0, 1.0, 0.0);
			Vec3d localZ = vec3d.Normalize();
			Vec3d vec3d2 = vec3d.Cross(vertical).Normalize();
			Vec3d localY = vec3d2.Cross(localZ);
			return vec3d2 * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A514 File Offset: 0x00008714
		public static Vec3d ToCameraReferenceFrame(EntityAgent player, Vec3d position)
		{
			Vec3f viewVectorF = player.SidedPos.GetViewVector();
			Vec3d vec3d = new Vec3d((double)viewVectorF.X, (double)viewVectorF.Y, (double)viewVectorF.Z);
			Vec3d vertical = new Vec3d(0.0, 1.0, 0.0);
			Vec3d localZ = vec3d.Normalize();
			Vec3d vec3d2 = vec3d.Cross(vertical).Normalize();
			Vec3d localY = vec3d2.Cross(localZ);
			DirectionOffset.InverseMatrix(vec3d2, localY, localZ);
			return vec3d2 * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A5B8 File Offset: 0x000087B8
		public static Vec3f ToCameraReferenceFrame(EntityAgent player, Vec3f position)
		{
			Vec3f viewVectorF = player.SidedPos.GetViewVector();
			Vec3f vec3f = new Vec3f(viewVectorF.X, viewVectorF.Y, viewVectorF.Z);
			Vec3f vertical = new Vec3f(0f, 1f, 0f);
			Vec3f localZ = vec3f.Normalize();
			Vec3f vec3f2 = vec3f.Cross(vertical).Normalize();
			Vec3f localY = vec3f2.Cross(localZ);
			DirectionOffset.InverseMatrix(vec3f2, localY, localZ);
			return vec3f2 * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A64C File Offset: 0x0000884C
		public static Vec3d ToReferenceFrame(Vec3d reference, Vec3d position)
		{
			Vec3d vertical = new Vec3d(0.0, 1.0, 0.0);
			Vec3d localZ = reference.Normalize();
			Vec3d vec3d = reference.Cross(vertical).Normalize();
			Vec3d localY = vec3d.Cross(localZ);
			DirectionOffset.InverseMatrix(vec3d, localY, localZ);
			return vec3d * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A6CC File Offset: 0x000088CC
		public static Vec3f ToReferenceFrame(Vec3f reference, Vec3f position)
		{
			Vec3f vertical = new Vec3f(0f, 1f, 0f);
			Vec3f localZ = reference.Normalize();
			Vec3f vec3f = reference.Cross(vertical).Normalize();
			Vec3f localY = vec3f.Cross(localZ);
			DirectionOffset.InverseMatrix(vec3f, localY, localZ);
			return vec3f * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A740 File Offset: 0x00008940
		public static Vec3d FromReferenceFrame(Vec3d reference, Vec3d position)
		{
			Vec3d vertical = new Vec3d(0.0, 1.0, 0.0);
			Vec3d localZ = reference.Normalize();
			Vec3d vec3d = reference.Cross(vertical).Normalize();
			Vec3d localY = vec3d.Cross(localZ);
			return vec3d * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A7B8 File Offset: 0x000089B8
		public static void InverseMatrix(Vec3d X, Vec3d Y, Vec3d Z)
		{
			double[] matrix = new double[]
			{
				X.X,
				X.Y,
				X.Z,
				Y.X,
				Y.Y,
				Y.Z,
				Z.X,
				Z.Y,
				Z.Z
			};
			Mat3d.Invert(matrix, matrix);
			X.X = matrix[0];
			X.Y = matrix[1];
			X.Z = matrix[2];
			Y.X = matrix[3];
			Y.Y = matrix[4];
			Y.Z = matrix[5];
			Z.X = matrix[6];
			Z.Y = matrix[7];
			Z.Z = matrix[8];
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A878 File Offset: 0x00008A78
		public static void InverseMatrix(Vec3f X, Vec3f Y, Vec3f Z)
		{
			float[] matrix = new float[]
			{
				X.X,
				X.Y,
				X.Z,
				Y.X,
				Y.Y,
				Y.Z,
				Z.X,
				Z.Y,
				Z.Z
			};
			Mat3f.Invert(matrix, matrix);
			X.X = matrix[0];
			X.Y = matrix[1];
			X.Z = matrix[2];
			Y.X = matrix[3];
			Y.Y = matrix[4];
			Y.Z = matrix[5];
			Z.X = matrix[6];
			Z.Y = matrix[7];
			Z.Z = matrix[8];
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A938 File Offset: 0x00008B38
		public static DirectionOffset GetDirection(Entity receiver, Entity source)
		{
			Vec3d vec3d = source.ServerPos.XYZ.Add(0.0, source.LocalEyePos.Y, 0.0).Sub(source.ServerPos.Motion);
			Vec3d receiverEyesPosition = receiver.SidedPos.XYZ.Add(0.0, (double)((float)receiver.LocalEyePos.Y), 0.0);
			Vec3d attackDirection = vec3d - receiverEyesPosition;
			Vec3d vec3d2 = EntityPos.GetViewVector(receiver.SidedPos.Pitch, receiver.SidedPos.Yaw).ToVec3d();
			vec3d2.Y = 0.0;
			return new DirectionOffset(DirectionOffset.ToReferenceFrame(vec3d2, attackDirection));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		public static DirectionOffset GetDirectionWithRespectToCamera(Entity receiver, Entity source)
		{
			Vec3d vec3d = source.ServerPos.XYZ.Add(0.0, source.LocalEyePos.Y, 0.0).Sub(source.ServerPos.Motion);
			Vec3d receiverEyesPosition = receiver.SidedPos.XYZ.Add(0.0, receiver.LocalEyePos.Y, 0.0);
			Vec3d attackDirection = vec3d - receiverEyesPosition;
			return new DirectionOffset(DirectionOffset.ToReferenceFrame(EntityPos.GetViewVector(receiver.SidedPos.Pitch, receiver.SidedPos.Yaw).ToVec3d(), attackDirection));
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		public static DirectionOffset GetDirectionWithRespectToCamera(Vector3d view, Vector3d direction)
		{
			Vec3d attackDirection = direction.ToVanillaRef();
			return new DirectionOffset(DirectionOffset.ToReferenceFrame(view.ToVanillaRef(), attackDirection).ToOpenTK());
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000AACC File Offset: 0x00008CCC
		public static Vector3d GetDirectionVectorWithRespectToCamera(Vector3d view, Vector3d direction)
		{
			Vec3d attackDirection = direction.ToVanillaRef();
			return DirectionOffset.ToReferenceFrame(view.ToVanillaRef(), attackDirection).ToOpenTK();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public static Vector3d GetDirectionVectorWithRespectToWorld(Vector3d view, Vector3d direction)
		{
			Vec3d attackDirection = direction.ToVanillaRef();
			return DirectionOffset.FromReferenceFrame(view.ToVanillaRef(), attackDirection).ToOpenTK();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000AB19 File Offset: 0x00008D19
		[NullableContext(0)]
		public static Vector3d DirectionFromYawPitch([TupleElementNames(new string[]
		{
			"yaw",
			"pitch"
		})] ValueTuple<Angle, Angle> angles)
		{
			return DirectionOffset.DirectionFromYawPitch(angles.Item1, angles.Item2);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public static Vector3d DirectionFromYawPitch(DirectionOffset angles)
		{
			return DirectionOffset.DirectionFromYawPitch(angles.Yaw, angles.Pitch);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000AB40 File Offset: 0x00008D40
		public static Vector3d DirectionFromYawPitch(Angle yaw, Angle pitch)
		{
			double cosPitch = Math.Cos((double)pitch.Radians);
			return new Vector3d(Math.Sin((double)yaw.Radians) * cosPitch, Math.Sin((double)pitch.Radians), Math.Cos((double)yaw.Radians) * cosPitch);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000AB8C File Offset: 0x00008D8C
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"yaw",
			"pitch"
		})]
		public static ValueTuple<Angle, Angle> YawPitchFromDirection(Vector3d direction)
		{
			direction = direction.Normalized();
			Angle angle = Angle.FromRadians(Math.Atan2(direction.X, direction.Z));
			Angle pitch = Angle.FromRadians(Math.Asin(direction.Y));
			return new ValueTuple<Angle, Angle>(angle, pitch);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public static DirectionOffset OffsetFromDirection(Vector3d direction)
		{
			direction = direction.Normalized();
			Angle yaw = Angle.FromRadians(Math.Atan2(direction.X, direction.Z));
			return new DirectionOffset(Angle.FromRadians(Math.Asin(direction.Y)), yaw);
		}

		// Token: 0x040000FD RID: 253
		public readonly Angle Pitch;

		// Token: 0x040000FE RID: 254
		public readonly Angle Yaw;
	}
}
