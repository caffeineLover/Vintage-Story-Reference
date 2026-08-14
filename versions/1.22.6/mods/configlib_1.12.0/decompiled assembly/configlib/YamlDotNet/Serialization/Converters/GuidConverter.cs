using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x0200016D RID: 365
	[NullableContext(1)]
	[Nullable(0)]
	public class GuidConverter : IYamlTypeConverter
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00019C3B File Offset: 0x00017E3B
		public GuidConverter(bool jsonCompatible)
		{
			this.jsonCompatible = jsonCompatible;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00019C4A File Offset: 0x00017E4A
		public bool Accepts(Type type)
		{
			return type == typeof(Guid);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00019C5C File Offset: 0x00017E5C
		public object ReadYaml(IParser parser, Type type)
		{
			return new Guid(parser.Consume<Scalar>().Value);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00019C74 File Offset: 0x00017E74
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			Guid guid = (Guid)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, guid.ToString("D"), this.jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x0400034E RID: 846
		private readonly bool jsonCompatible;
	}
}
