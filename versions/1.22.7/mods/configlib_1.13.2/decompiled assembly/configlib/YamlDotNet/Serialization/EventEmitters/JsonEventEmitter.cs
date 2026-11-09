using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000163 RID: 355
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class JsonEventEmitter : ChainedEventEmitter
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x0001883A File Offset: 0x00016A3A
		public JsonEventEmitter(IEventEmitter nextEmitter, YamlFormatter formatter) : base(nextEmitter)
		{
			this.formatter = formatter;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001884A File Offset: 0x00016A4A
		public JsonEventEmitter(IEventEmitter nextEmitter) : this(nextEmitter, YamlFormatter.Default)
		{
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00018858 File Offset: 0x00016A58
		public override void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.NeedsExpansion = true;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00018864 File Offset: 0x00016A64
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

		// Token: 0x06000728 RID: 1832 RVA: 0x00018A1D File Offset: 0x00016C1D
		public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = MappingStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018A2E File Offset: 0x00016C2E
		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			eventInfo.Style = SequenceStyle.Flow;
			base.Emit(eventInfo, emitter);
		}

		// Token: 0x0400030C RID: 780
		private readonly YamlFormatter formatter;
	}
}
