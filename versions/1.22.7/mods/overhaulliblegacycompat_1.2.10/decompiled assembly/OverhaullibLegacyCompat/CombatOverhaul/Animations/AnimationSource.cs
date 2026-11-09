using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AA RID: 426
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class AnimationSource : IEquatable<AnimationSource>
	{
		// Token: 0x06001468 RID: 5224 RVA: 0x000650C3 File Offset: 0x000632C3
		public AnimationSource(string Domain, string SourceKey, AnimationSourceKind Kind = AnimationSourceKind.ConfigAnimation, [Nullable(2)] string AssetPath = null, [Nullable(2)] string ShapeAnimationCode = null)
		{
			this.Domain = Domain;
			this.SourceKey = SourceKey;
			this.Kind = Kind;
			this.AssetPath = AssetPath;
			this.ShapeAnimationCode = ShapeAnimationCode;
			base..ctor();
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x000650F0 File Offset: 0x000632F0
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(AnimationSource);
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x000650FC File Offset: 0x000632FC
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x00065104 File Offset: 0x00063304
		public string Domain { get; set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0006510D File Offset: 0x0006330D
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x00065115 File Offset: 0x00063315
		public string SourceKey { get; set; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x0006511E File Offset: 0x0006331E
		// (set) Token: 0x0600146F RID: 5231 RVA: 0x00065126 File Offset: 0x00063326
		public AnimationSourceKind Kind { get; set; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0006512F File Offset: 0x0006332F
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00065137 File Offset: 0x00063337
		[Nullable(2)]
		public string AssetPath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00065140 File Offset: 0x00063340
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00065148 File Offset: 0x00063348
		[Nullable(2)]
		public string ShapeAnimationCode { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00065154 File Offset: 0x00063354
		public string DisplayPath
		{
			get
			{
				if (this.Kind != AnimationSourceKind.ShapeAnimation || this.AssetPath == null)
				{
					return this.Domain + ":" + this.SourceKey;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.Domain);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(this.AssetPath);
				defaultInterpolatedStringHandler.AppendLiteral("#");
				defaultInterpolatedStringHandler.AppendFormatted(this.ShapeAnimationCode);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000651D8 File Offset: 0x000633D8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("AnimationSource");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00065224 File Offset: 0x00063424
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Domain = ");
			builder.Append(this.Domain);
			builder.Append(", SourceKey = ");
			builder.Append(this.SourceKey);
			builder.Append(", Kind = ");
			builder.Append(this.Kind.ToString());
			builder.Append(", AssetPath = ");
			builder.Append(this.AssetPath);
			builder.Append(", ShapeAnimationCode = ");
			builder.Append(this.ShapeAnimationCode);
			builder.Append(", DisplayPath = ");
			builder.Append(this.DisplayPath);
			return true;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000652DB File Offset: 0x000634DB
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(AnimationSource left, AnimationSource right)
		{
			return !(left == right);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000652E7 File Offset: 0x000634E7
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(AnimationSource left, AnimationSource right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000652FC File Offset: 0x000634FC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Domain>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourceKey>k__BackingField)) * -1521134295 + EqualityComparer<AnimationSourceKind>.Default.GetHashCode(this.<Kind>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<AssetPath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ShapeAnimationCode>k__BackingField);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0006538C File Offset: 0x0006358C
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as AnimationSource);
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0006539C File Offset: 0x0006359C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(AnimationSource other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Domain>k__BackingField, other.<Domain>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SourceKey>k__BackingField, other.<SourceKey>k__BackingField) && EqualityComparer<AnimationSourceKind>.Default.Equals(this.<Kind>k__BackingField, other.<Kind>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<AssetPath>k__BackingField, other.<AssetPath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ShapeAnimationCode>k__BackingField, other.<ShapeAnimationCode>k__BackingField));
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0006544C File Offset: 0x0006364C
		[CompilerGenerated]
		private AnimationSource(AnimationSource original)
		{
			this.Domain = original.<Domain>k__BackingField;
			this.SourceKey = original.<SourceKey>k__BackingField;
			this.Kind = original.<Kind>k__BackingField;
			this.AssetPath = original.<AssetPath>k__BackingField;
			this.ShapeAnimationCode = original.<ShapeAnimationCode>k__BackingField;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0006549B File Offset: 0x0006369B
		[CompilerGenerated]
		public void Deconstruct(out string Domain, out string SourceKey, out AnimationSourceKind Kind, [Nullable(2)] out string AssetPath, [Nullable(2)] out string ShapeAnimationCode)
		{
			Domain = this.Domain;
			SourceKey = this.SourceKey;
			Kind = this.Kind;
			AssetPath = this.AssetPath;
			ShapeAnimationCode = this.ShapeAnimationCode;
		}
	}
}
