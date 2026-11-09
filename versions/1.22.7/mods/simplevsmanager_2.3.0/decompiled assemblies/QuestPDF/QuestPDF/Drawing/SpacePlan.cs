using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
	// Token: 0x02000112 RID: 274
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct SpacePlan
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x000189A8 File Offset: 0x00016BA8
		[NullableContext(2)]
		internal SpacePlan(SpacePlanType type, float width, float height, string wrapReason = null)
		{
			this.Type = type;
			this.Width = width;
			this.Height = height;
			this.WrapReason = wrapReason;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000189C7 File Offset: 0x00016BC7
		internal static SpacePlan Empty()
		{
			return new SpacePlan(SpacePlanType.Empty, 0f, 0f, null);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000189DA File Offset: 0x00016BDA
		internal static SpacePlan Wrap(string reason)
		{
			return new SpacePlan(SpacePlanType.Wrap, 0f, 0f, reason);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000189ED File Offset: 0x00016BED
		internal static SpacePlan PartialRender(float width, float height)
		{
			return new SpacePlan(SpacePlanType.PartialRender, width, height, null);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000189F8 File Offset: 0x00016BF8
		internal static SpacePlan PartialRender(Size size)
		{
			return SpacePlan.PartialRender(size.Width, size.Height);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00018A0B File Offset: 0x00016C0B
		internal static SpacePlan FullRender(float width, float height)
		{
			return new SpacePlan(SpacePlanType.FullRender, width, height, null);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00018A16 File Offset: 0x00016C16
		internal static SpacePlan FullRender(Size size)
		{
			return SpacePlan.FullRender(size.Width, size.Height);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00018A2C File Offset: 0x00016C2C
		public override string ToString()
		{
			if (this.Type == SpacePlanType.Wrap)
			{
				return this.Type.ToString();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(20, 3);
			defaultInterpolatedStringHandler.AppendFormatted<SpacePlanType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(" (Width: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Width, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(", Height: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Height, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00018ABD File Offset: 0x00016CBD
		public static implicit operator Size(SpacePlan spacePlan)
		{
			return new Size(spacePlan.Width, spacePlan.Height);
		}

		// Token: 0x040003DE RID: 990
		public readonly SpacePlanType Type;

		// Token: 0x040003DF RID: 991
		public readonly float Width;

		// Token: 0x040003E0 RID: 992
		public readonly float Height;

		// Token: 0x040003E1 RID: 993
		[Nullable(2)]
		public readonly string WrapReason;
	}
}
