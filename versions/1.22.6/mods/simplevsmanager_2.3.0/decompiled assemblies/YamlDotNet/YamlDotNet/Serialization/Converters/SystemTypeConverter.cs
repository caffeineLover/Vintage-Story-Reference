using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	[Nullable(0)]
	public class SystemTypeConverter : IYamlTypeConverter
	{
		// Token: 0x0600034A RID: 842 RVA: 0x0000BFD7 File Offset: 0x0000A1D7
		public bool Accepts(Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000BFEC File Offset: 0x0000A1EC
		public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
		{
			string value = parser.Consume<Scalar>().Value;
			return Type.GetType(value, true);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C00C File Offset: 0x0000A20C
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer)
		{
			Type type2 = (Type)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, type2.AssemblyQualifiedName, ScalarStyle.Any, true, false));
		}
	}
}
