using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016E RID: 366
	[NullableContext(1)]
	[Nullable(0)]
	public class SystemTypeConverter : IYamlTypeConverter
	{
		// Token: 0x0600078A RID: 1930 RVA: 0x00019CB7 File Offset: 0x00017EB7
		public bool Accepts(Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00019CC9 File Offset: 0x00017EC9
		public object ReadYaml(IParser parser, Type type)
		{
			return Type.GetType(parser.Consume<Scalar>().Value, true);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00019CDC File Offset: 0x00017EDC
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			Type type2 = (Type)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, type2.AssemblyQualifiedName, ScalarStyle.Any, true, false));
		}
	}
}
