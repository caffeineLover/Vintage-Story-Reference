using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
	// Token: 0x02000169 RID: 361
	[NullableContext(1)]
	[Nullable(0)]
	public class GuidConverter : IYamlTypeConverter
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x000192F3 File Offset: 0x000174F3
		public GuidConverter(bool jsonCompatible)
		{
			this.jsonCompatible = jsonCompatible;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00019302 File Offset: 0x00017502
		public bool Accepts(Type type)
		{
			return type == typeof(Guid);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00019314 File Offset: 0x00017514
		public object ReadYaml(IParser parser, Type type)
		{
			return new Guid(parser.Consume<Scalar>().Value);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001932C File Offset: 0x0001752C
		public void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type)
		{
			Guid guid = (Guid)value;
			emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, guid.ToString("D"), this.jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
		}

		// Token: 0x04000320 RID: 800
		private readonly bool jsonCompatible;
	}
}
