using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016A RID: 362
	[NullableContext(1)]
	[Nullable(0)]
	public class SystemTypeConverter : IYamlTypeConverter
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0001936F File Offset: 0x0001756F
		public bool Accepts(Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00019381 File Offset: 0x00017581
		public object ReadYaml(IParser parser, Type type)
		{
			return Type.GetType(parser.Consume<Scalar>().Value, true);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00019394 File Offset: 0x00017594
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			Type type2 = (Type)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, type2.AssemblyQualifiedName, ScalarStyle.Any, true, false));
		}
	}
}
