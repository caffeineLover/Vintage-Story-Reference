using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000167 RID: 359
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JsonEventEmitter : ChainedEventEmitter
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x00019182 File Offset: 0x00017382
		public JsonEventEmitter(IEventEmitter nextEmitter, YamlFormatter formatter) : base(nextEmitter)
		{
			this.formatter = formatter;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00019192 File Offset: 0x00017392
		public JsonEventEmitter(IEventEmitter nextEmitter) : this(nextEmitter, YamlFormatter.Default)
		{
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000191A0 File Offset: 0x000173A0
		public override void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.NeedsExpansion = true;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000191AC File Offset: 0x000173AC
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
					goto IL_1A4;
				case 3:
					eventInfo.RenderedValue = this.formatter.FormatBoolean(value);
					goto IL_1A4;
				case 4:
				case 18:
					eventInfo.RenderedValue = value.ToString();
					eventInfo.Style = ScalarStyle.DoubleQuoted;
					goto IL_1A4;
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
					if (eventInfo.Source.Type.IsEnum())
					{
						eventInfo.RenderedValue = value.ToString();
						eventInfo.Style = ScalarStyle.DoubleQuoted;
						goto IL_1A4;
					}
					eventInfo.RenderedValue = this.formatter.FormatNumber(value);
					goto IL_1A4;
				case 13:
				case 14:
				case 15:
					eventInfo.RenderedValue = this.formatter.FormatNumber(value);
					goto IL_1A4;
				case 16:
					eventInfo.RenderedValue = this.formatter.FormatDateTime(value);
					goto IL_1A4;
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
			IL_1A4:
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00019365 File Offset: 0x00017565
		public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = MappingStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00019376 File Offset: 0x00017576
		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = SequenceStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x0400033A RID: 826
		private readonly YamlFormatter formatter;
	}
}
