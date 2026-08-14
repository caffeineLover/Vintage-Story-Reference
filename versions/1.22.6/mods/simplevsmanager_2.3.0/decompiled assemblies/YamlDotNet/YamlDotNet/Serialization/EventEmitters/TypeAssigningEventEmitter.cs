using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Schemas;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000090 RID: 144
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TypeAssigningEventEmitter : ChainedEventEmitter
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0000B740 File Offset: 0x00009940
		public TypeAssigningEventEmitter(IEventEmitter nextEmitter, IDictionary<Type, TagName> tagMappings, bool quoteNecessaryStrings, bool quoteYaml1_1Strings, ScalarStyle defaultScalarStyle, YamlFormatter formatter, INamingConvention enumNamingConvention, ITypeInspector typeInspector) : base(nextEmitter)
		{
			this.defaultScalarStyle = defaultScalarStyle;
			this.formatter = formatter;
			this.tagMappings = tagMappings;
			this.quoteNecessaryStrings = quoteNecessaryStrings;
			string text = quoteYaml1_1Strings ? TypeAssigningEventEmitter.CombinedYaml1_1SpecialStrings_Pattern : TypeAssigningEventEmitter.SpecialStrings_Pattern;
			this.isSpecialStringValue_Regex = new Regex(text, 8);
			this.enumNamingConvention = enumNamingConvention;
			this.typeInspector = typeInspector;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000B7A0 File Offset: 0x000099A0
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
					goto IL_2A2;
				case 3:
					eventInfo.Tag = JsonSchema.Tags.Bool;
					eventInfo.RenderedValue = this.formatter.FormatBoolean(value);
					goto IL_2A2;
				case 4:
				case 18:
					eventInfo.Tag = FailsafeSchema.Tags.Str;
					eventInfo.RenderedValue = value.ToString();
					if (this.quoteNecessaryStrings && this.IsSpecialStringValue(eventInfo.RenderedValue))
					{
						style = ScalarStyle.DoubleQuoted;
						goto IL_2A2;
					}
					style = this.defaultScalarStyle;
					goto IL_2A2;
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
						goto IL_2A2;
					}
					eventInfo.Tag = FailsafeSchema.Tags.Str;
					eventInfo.RenderedValue = this.formatter.FormatEnum.Invoke(value, this.typeInspector, this.enumNamingConvention);
					if (this.quoteNecessaryStrings && this.IsSpecialStringValue(eventInfo.RenderedValue) && this.formatter.PotentiallyQuoteEnums.Invoke(value))
					{
						style = ScalarStyle.DoubleQuoted;
						goto IL_2A2;
					}
					style = this.defaultScalarStyle;
					goto IL_2A2;
				case 13:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber((float)value);
					goto IL_2A2;
				case 14:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber((double)value);
					goto IL_2A2;
				case 15:
					eventInfo.Tag = JsonSchema.Tags.Float;
					eventInfo.RenderedValue = this.formatter.FormatNumber(value);
					goto IL_2A2;
				case 16:
					eventInfo.Tag = DefaultSchema.Tags.Timestamp;
					eventInfo.RenderedValue = this.formatter.FormatDateTime(value);
					goto IL_2A2;
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
			IL_2A2:
			eventInfo.IsPlainImplicit = true;
			if (eventInfo.Style == ScalarStyle.Any)
			{
				eventInfo.Style = style;
			}
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000BA6D File Offset: 0x00009C6D
		public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			this.AssignTypeIfNeeded(eventInfo);
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000BA7E File Offset: 0x00009C7E
		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			this.AssignTypeIfNeeded(eventInfo);
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000BA90 File Offset: 0x00009C90
		private void AssignTypeIfNeeded(ObjectEventInfo eventInfo)
		{
			TagName tag;
			if (this.tagMappings.TryGetValue(eventInfo.Source.Type, ref tag))
			{
				eventInfo.Tag = tag;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BABE File Offset: 0x00009CBE
		private bool IsSpecialStringValue(string value)
		{
			if (value.Trim() == string.Empty)
			{
				return true;
			}
			Regex regex = this.isSpecialStringValue_Regex;
			return regex != null && regex.IsMatch(value);
		}

		// Token: 0x040000DA RID: 218
		private readonly IDictionary<Type, TagName> tagMappings;

		// Token: 0x040000DB RID: 219
		private readonly bool quoteNecessaryStrings;

		// Token: 0x040000DC RID: 220
		[Nullable(2)]
		private readonly Regex isSpecialStringValue_Regex;

		// Token: 0x040000DD RID: 221
		private static readonly string SpecialStrings_Pattern = "^(null|Null|NULL|\\~|true|True|TRUE|false|False|FALSE|[-+]?[0-9]+|0o[0-7]+|0x[0-9a-fA-F]+|[-+]?(\\.[0-9]+|[0-9]+(\\.[0-9]*)?)([eE][-+]?[0-9]+)?|[-+]?(\\.inf|\\.Inf|\\.INF)|\\.nan|\\.NaN|\\.NAN|\\s.*)$";

		// Token: 0x040000DE RID: 222
		private static readonly string CombinedYaml1_1SpecialStrings_Pattern = "^(null|Null|NULL|\\~|true|True|TRUE|false|False|FALSE|y|Y|yes|Yes|YES|n|N|no|No|NO|on|On|ON|off|Off|OFF|[-+]?0b[0-1_]+|[-+]?0o?[0-7_]+|[-+]?(0|[1-9][0-9_]*)|[-+]?0x[0-9a-fA-F_]+|[-+]?[1-9][0-9_]*(:[0-5]?[0-9])+|[-+]?([0-9][0-9_]*)?\\.[0-9_]*([eE][-+][0-9]+)?|[-+]?[0-9][0-9_]*(:[0-5]?[0-9])+\\.[0-9_]*|[-+]?\\.(inf|Inf|INF)|\\.(nan|NaN|NAN))$";

		// Token: 0x040000DF RID: 223
		private readonly ScalarStyle defaultScalarStyle;

		// Token: 0x040000E0 RID: 224
		private readonly YamlFormatter formatter;

		// Token: 0x040000E1 RID: 225
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x040000E2 RID: 226
		private readonly ITypeInspector typeInspector;
	}
}
