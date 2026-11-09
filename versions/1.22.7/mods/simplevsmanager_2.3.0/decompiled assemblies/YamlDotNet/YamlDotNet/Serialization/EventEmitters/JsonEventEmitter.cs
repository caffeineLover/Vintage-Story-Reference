using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x0200008F RID: 143
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JsonEventEmitter : ChainedEventEmitter
	{
		// Token: 0x06000321 RID: 801 RVA: 0x0000B485 File Offset: 0x00009685
		public JsonEventEmitter(IEventEmitter nextEmitter, YamlFormatter formatter, INamingConvention enumNamingConvention, ITypeInspector typeInspector) : base(nextEmitter)
		{
			this.formatter = formatter;
			this.enumNamingConvention = enumNamingConvention;
			this.typeInspector = typeInspector;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000B4A4 File Offset: 0x000096A4
		public override void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.NeedsExpansion = true;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.IsPlainImplicit = true;
			eventInfo.Style = ScalarStyle.Plain;
			object value = eventInfo.Source.Value;
			if (value == null)
			{
				eventInfo.RenderedValue = "null";
			}
			else
			{
				TypeCode typeCode = eventInfo.Source.Type.GetTypeCode();
				switch (typeCode)
				{
				case 0:
					eventInfo.RenderedValue = "null";
					goto IL_258;
				case 3:
					eventInfo.RenderedValue = this.formatter.FormatBoolean(value);
					goto IL_258;
				case 4:
				case 18:
					eventInfo.RenderedValue = value.ToString();
					eventInfo.Style = ScalarStyle.DoubleQuoted;
					goto IL_258;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				{
					bool flag = eventInfo.Source.Type.IsEnum();
					if (flag)
					{
						eventInfo.RenderedValue = this.formatter.FormatEnum.Invoke(value, this.typeInspector, this.enumNamingConvention);
						eventInfo.Style = (this.formatter.PotentiallyQuoteEnums.Invoke(value) ? ScalarStyle.DoubleQuoted : ScalarStyle.Plain);
						goto IL_258;
					}
					eventInfo.RenderedValue = this.formatter.FormatNumber(value);
					goto IL_258;
				}
				case 13:
				{
					float num = (float)value;
					eventInfo.RenderedValue = num.ToString("G", CultureInfo.InvariantCulture);
					if (float.IsNaN(num) || float.IsInfinity(num))
					{
						eventInfo.Style = ScalarStyle.DoubleQuoted;
						goto IL_258;
					}
					goto IL_258;
				}
				case 14:
				{
					double num2 = (double)value;
					eventInfo.RenderedValue = num2.ToString("G", CultureInfo.InvariantCulture);
					if (double.IsNaN(num2) || double.IsInfinity(num2))
					{
						eventInfo.Style = ScalarStyle.DoubleQuoted;
						goto IL_258;
					}
					goto IL_258;
				}
				case 15:
					eventInfo.RenderedValue = ((decimal)value).ToString(CultureInfo.InvariantCulture);
					goto IL_258;
				case 16:
					eventInfo.RenderedValue = this.formatter.FormatDateTime(value);
					goto IL_258;
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
			IL_258:
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000B71D File Offset: 0x0000991D
		public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = MappingStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000B72E File Offset: 0x0000992E
		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = SequenceStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x040000D7 RID: 215
		private readonly YamlFormatter formatter;

		// Token: 0x040000D8 RID: 216
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x040000D9 RID: 217
		private readonly ITypeInspector typeInspector;
	}
}
