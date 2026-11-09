using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	[Nullable(0)]
	public class DateTime8601Converter : IYamlTypeConverter
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0000BC79 File Offset: 0x00009E79
		public DateTime8601Converter() : this(ScalarStyle.Any)
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000BC82 File Offset: 0x00009E82
		public DateTime8601Converter(ScalarStyle scalarStyle)
		{
			this.scalarStyle = scalarStyle;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000BC91 File Offset: 0x00009E91
		public bool Accepts(Type type)
		{
			return type == typeof(DateTime);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000BCA4 File Offset: 0x00009EA4
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			DateTime dateTime = DateTime.ParseExact(value, "O", CultureInfo.InvariantCulture, 128);
			return dateTime;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000BCDC File Offset: 0x00009EDC
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			string value2 = ((DateTime)value).ToString("O", CultureInfo.InvariantCulture);
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, value2, this.scalarStyle, true, false));
		}

		// Token: 0x040000E6 RID: 230
		private readonly ScalarStyle scalarStyle;
	}
}
