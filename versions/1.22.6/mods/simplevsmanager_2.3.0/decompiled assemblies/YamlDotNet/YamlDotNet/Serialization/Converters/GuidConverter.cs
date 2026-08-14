using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	[Nullable(0)]
	public class GuidConverter : IYamlTypeConverter
	{
		// Token: 0x06000346 RID: 838 RVA: 0x0000BF4F File Offset: 0x0000A14F
		public GuidConverter(bool jsonCompatible)
		{
			this.jsonCompatible = jsonCompatible;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000BF5E File Offset: 0x0000A15E
		public bool Accepts(Type type)
		{
			return type == typeof(Guid);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000BF70 File Offset: 0x0000A170
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			return new Guid(value);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000BF94 File Offset: 0x0000A194
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			Guid guid = (Guid)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, guid.ToString("D"), this.jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x040000EF RID: 239
		private readonly bool jsonCompatible;
	}
}
