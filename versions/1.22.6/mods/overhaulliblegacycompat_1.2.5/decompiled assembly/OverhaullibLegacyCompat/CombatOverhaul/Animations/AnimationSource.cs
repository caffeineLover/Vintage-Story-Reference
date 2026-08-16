using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019C RID: 412
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class AnimationSource : IEquatable<AnimationSource>
	{
		// Token: 0x060013F0 RID: 5104 RVA: 0x00063643 File Offset: 0x00061843
		public AnimationSource(string Domain, string SourceKey, AnimationSourceKind Kind = AnimationSourceKind.ConfigAnimation, [Nullable(2)] string AssetPath = null, [Nullable(2)] string ShapeAnimationCode = null)
		{
			this.Domain = Domain;
			this.SourceKey = SourceKey;
			this.Kind = Kind;
			this.AssetPath = AssetPath;
			this.ShapeAnimationCode = ShapeAnimationCode;
			base..ctor();
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00063670 File Offset: 0x00061870
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(AnimationSource);
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0006367C File Offset: 0x0006187C
		// (set) Token: 0x060013F3 RID: 5107 RVA: 0x00063684 File Offset: 0x00061884
		public string Domain { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0006368D File Offset: 0x0006188D
		// (set) Token: 0x060013F5 RID: 5109 RVA: 0x00063695 File Offset: 0x00061895
		public string SourceKey { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0006369E File Offset: 0x0006189E
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x000636A6 File Offset: 0x000618A6
		public AnimationSourceKind Kind { get; set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x000636AF File Offset: 0x000618AF
		// (set) Token: 0x060013F9 RID: 5113 RVA: 0x000636B7 File Offset: 0x000618B7
		[Nullable(2)]
		public string AssetPath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x000636C0 File Offset: 0x000618C0
		// (set) Token: 0x060013FB RID: 5115 RVA: 0x000636C8 File Offset: 0x000618C8
		[Nullable(2)]
		public string ShapeAnimationCode { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x000636D4 File Offset: 0x000618D4
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

		// Token: 0x060013FD RID: 5117 RVA: 0x00063758 File Offset: 0x00061958
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

		// Token: 0x060013FE RID: 5118 RVA: 0x000637A4 File Offset: 0x000619A4
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

		// Token: 0x060013FF RID: 5119 RVA: 0x0006385B File Offset: 0x00061A5B
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(AnimationSource left, AnimationSource right)
		{
			return !(left == right);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x00063867 File Offset: 0x00061A67
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(AnimationSource left, AnimationSource right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0006387C File Offset: 0x00061A7C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Domain>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourceKey>k__BackingField)) * -1521134295 + EqualityComparer<AnimationSourceKind>.Default.GetHashCode(this.<Kind>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<AssetPath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ShapeAnimationCode>k__BackingField);
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0006390C File Offset: 0x00061B0C
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as AnimationSource);
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0006391C File Offset: 0x00061B1C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(AnimationSource other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Domain>k__BackingField, other.<Domain>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SourceKey>k__BackingField, other.<SourceKey>k__BackingField) && EqualityComparer<AnimationSourceKind>.Default.Equals(this.<Kind>k__BackingField, other.<Kind>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<AssetPath>k__BackingField, other.<AssetPath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ShapeAnimationCode>k__BackingField, other.<ShapeAnimationCode>k__BackingField));
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000639CC File Offset: 0x00061BCC
		[CompilerGenerated]
		private AnimationSource(AnimationSource original)
		{
			this.Domain = original.<Domain>k__BackingField;
			this.SourceKey = original.<SourceKey>k__BackingField;
			this.Kind = original.<Kind>k__BackingField;
			this.AssetPath = original.<AssetPath>k__BackingField;
			this.ShapeAnimationCode = original.<ShapeAnimationCode>k__BackingField;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00063A1B File Offset: 0x00061C1B
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
