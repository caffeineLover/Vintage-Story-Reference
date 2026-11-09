using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Schemas;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000164 RID: 356
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TypeAssigningEventEmitter : ChainedEventEmitter
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00018A40 File Offset: 0x00016C40
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings, bool quoteYaml1_1Strings, ScalarStyle defaultScalarStyle, YamlFormatter formatter) : base(nextEmitter)
		{
			this.defaultScalarStyle = defaultScalarStyle;
			this.formatter = formatter;
			this.tagMappings = tagMappings;
			this.quoteNecessaryStrings = quoteNecessaryStrings;
			string text = quoteYaml1_1Strings ? TypeAssigningEventEmitter.CombinedYaml1_1SpecialStrings_Pattern : TypeAssigningEventEmitter.SpecialStrings_Pattern;
			this.isSpecialStringValue_Regex = new Regex(text, 8);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00018A91 File Offset: 0x00016C91
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings, bool quoteYaml1_1Strings, ScalarStyle defaultScalarStyle) : this(nextEmitter, requireTagWhenStaticAndActualTypesAreDifferent, tagMappings, quoteNecessaryStrings, quoteYaml1_1Strings, defaultScalarStyle, YamlFormatter.Default)
		{
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00018AA7 File Offset: 0x00016CA7
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings, ScalarStyle defaultScalarStyle) : this(nextEmitter, requireTagWhenStaticAndActualTypesAreDifferent, tagMappings, quoteNecessaryStrings, false, defaultScalarStyle, YamlFormatter.Default)
		{
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00018ABC File Offset: 0x00016CBC
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings, bool quoteYaml1_1Strings) : this(nextEmitter, requireTagWhenStaticAndActualTypesAreDifferent, tagMappings, quoteNecessaryStrings, quoteYaml1_1Strings, ScalarStyle.Any)
		{
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00018ACC File Offset: 0x00016CCC
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings) : this(nextEmitter, requireTagWhenStaticAndActualTypesAreDifferent, tagMappings, quoteNecessaryStrings, false)
		{
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00018ADA File Offset: 0x00016CDA
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool requireTagWhenStaticAndActualTypesAreDifferent, IDictionary<Type, TagName> tagMappings) : base(nextEmitter)
		{
			this.requireTagWhenStaticAndActualTypesAreDifferent = requireTagWhenStaticAndActualTypesAreDifferent;
			if (tagMappings == null)
			{
				throw new ArgumentNullException("tagMappings");
			}
			this.tagMappings = tagMappings;
			this.formatter = YamlFormatter.Default;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00018B0C File Offset: 0x00016D0C
		public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			ScalarStyle style = ScalarStyle.Plain;
			object value = eventInfo.Source.Value;
			if (value == null)
			{
				eventInfo.Tag = JsonSchema.Tags.Null;
				eventInfo.RenderedValue = "";
			}
			else
			{
				TypeCode typeCode = eventInfo.Source.Type.GetTypeCode();
				switch (typeCode)
				{
				case 0:
					eventInfo.Tag = JsonSchema.Tags.Null;
					eventInfo.RenderedValue = "";
					goto IL_278;
				case 3:
					eventInfo.Tag = JsonSchema.Tags.Bool;
					eventInfo.RenderedValue = this.formatter.FormatBoolean(value);
					goto IL_278;
				case 4:
				case 18:
					eventInfo.Tag = FailsafeSchema.Tags.Str;
					eventInfo.RenderedValue = value.ToString();
					if (this.quoteNecessaryStrings && this.IsSpecialStringValue(eventInfo.RenderedValue))
					{
						style = ScalarStyle.DoubleQuoted;
						goto IL_278;
					}
					style = this.defaultScalarStyle;
					goto IL_278;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
					if (!eventInfo.Source.Type.IsEnum)
					{
						eventInfo.Tag = JsonSchema.Tags.Int;
						eventInfo.RenderedValue = this.formatter.FormatNumber(value);
						goto IL_278;
					}
					eventInfo.Tag = FailsafeSchema.Tags.Str;
					eventInfo.RenderedValue = value.ToString();
					if (this.quoteNecessaryStrings && this.IsSpecialStringValue(eventInfo.RenderedValue))
					{
						style = ScalarStyle.DoubleQuoted;
						goto IL_278;
					}
					style = this.defaultScalarStyle;
					goto IL_278;
				case 13:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber((float)value);
					goto IL_278;
				case 14:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber((double)value);
					goto IL_278;
				case 15:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber(value);
					goto IL_278;
				case 16:
					eventInfo.Tag = DefaultSchema.Tags.Timestamp;
					eventInfo.RenderedValue = this.formatter.FormatDateTime(value);
					goto IL_278;
				}
				if (!(eventInfo.Source.Type == typeof(TimeSpan)))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("TypeCode.");
					defaultInterpolatedStringHandler.AppendFormatted<TypeCode>(typeCode);
					defaultInterpolatedStringHandler.AppendLiteral(" is not supported.");
					throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				eventInfo.RenderedValue = this.formatter.FormatTimeSpan(value);
			}
			IL_278:
			eventInfo.IsPlainImplicit = true;
			if (eventInfo.Style == ScalarStyle.Any)
			{
				eventInfo.Style = style;
			}
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00018DAF File Offset: 0x00016FAF
		public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			this.AssignTypeIfNeeded(eventInfo);
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00018DC0 File Offset: 0x00016FC0
		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			this.AssignTypeIfNeeded(eventInfo);
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00018DD4 File Offset: 0x00016FD4
		private void AssignTypeIfNeeded(ObjectEventInfo eventInfo)
		{
			TagName tag;
			if (this.tagMappings.TryGetValue(eventInfo.Source.Type, ref tag))
			{
				eventInfo.Tag = tag;
				return;
			}
			if (this.requireTagWhenStaticAndActualTypesAreDifferent && eventInfo.Source.Value != null && eventInfo.Source.Type != eventInfo.Source.StaticType)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(285, 5);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot serialize type '");
				defaultInterpolatedStringHandler.AppendFormatted(eventInfo.Source.Type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("' where a '");
				defaultInterpolatedStringHandler.AppendFormatted(eventInfo.Source.StaticType.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("' was expected ");
				defaultInterpolatedStringHandler.AppendLiteral("because no tag mapping has been registered for '");
				defaultInterpolatedStringHandler.AppendFormatted(eventInfo.Source.Type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("', ");
				defaultInterpolatedStringHandler.AppendLiteral("which means that it won't be possible to deserialize the document.\n");
				defaultInterpolatedStringHandler.AppendLiteral("Register a tag mapping using the SerializerBuilder.WithTagMapping method.\n\n");
				defaultInterpolatedStringHandler.AppendLiteral("E.g: builder.WithTagMapping(\"!");
				defaultInterpolatedStringHandler.AppendFormatted(eventInfo.Source.Type.Name);
				defaultInterpolatedStringHandler.AppendLiteral("\", typeof(");
				defaultInterpolatedStringHandler.AppendFormatted(eventInfo.Source.Type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("));");
				throw new YamlException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00018F43 File Offset: 0x00017143
		private bool IsSpecialStringValue(string value)
		{
			if (value.Trim() == string.Empty)
			{
				return true;
			}
			Regex regex = this.isSpecialStringValue_Regex;
			return regex != null && regex.IsMatch(value);
		}

		// Token: 0x0400030D RID: 781
		private readonly bool requireTagWhenStaticAndActualTypesAreDifferent;

		// Token: 0x0400030E RID: 782
		private readonly IDictionary<Type, TagName> tagMappings;

		// Token: 0x0400030F RID: 783
		private readonly bool quoteNecessaryStrings;

		// Token: 0x04000310 RID: 784
		[Nullable(2)]
		private readonly Regex isSpecialStringValue_Regex;

		// Token: 0x04000311 RID: 785
		private static readonly string SpecialStrings_Pattern = "^(null|Null|NULL|\\~|true|True|TRUE|false|False|FALSE|[-+]?[0-9]+|0o[0-7]+|0x[0-9a-fA-F]+|[-+]?(\\.[0-9]+|[0-9]+(\\.[0-9]*)?)([eE][-+]?[0-9]+)?|[-+]?(\\.inf|\\.Inf|\\.INF)|\\.nan|\\.NaN|\\.NAN)$";

		// Token: 0x04000312 RID: 786
		private static readonly string CombinedYaml1_1SpecialStrings_Pattern = "^(null|Null|NULL|\\~|true|True|TRUE|false|False|FALSE|y|Y|yes|Yes|YES|n|N|no|No|NO|on|On|ON|off|Off|OFF|[-+]?0b[0-1_]+|[-+]?0o?[0-7_]+|[-+]?(0|[1-9][0-9_]*)|[-+]?0x[0-9a-fA-F_]+|[-+]?[1-9][0-9_]*(:[0-5]?[0-9])+|[-+]?([0-9][0-9_]*)?\\.[0-9_]*([eE][-+][0-9]+)?|[-+]?[0-9][0-9_]*(:[0-5]?[0-9])+\\.[0-9_]*|[-+]?\\.(inf|Inf|INF)|\\.(nan|NaN|NAN))$";

		// Token: 0x04000313 RID: 787
		private readonly ScalarStyle defaultScalarStyle;

		// Token: 0x04000314 RID: 788
		private readonly YamlFormatter formatter;
	}
}
