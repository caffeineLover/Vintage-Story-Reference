using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200014F RID: 335
	public sealed class NullNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x00017D98 File Offset: 0x00015F98
		[NullableContext(1)]
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			value = null;
			NodeEvent nodeEvent;
			if (parser.Accept(out nodeEvent) && this.NodeIsNull(nodeEvent))
			{
				parser.SkipThisAndNestedEvents();
				return true;
			}
			return false;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00017DC8 File Offset: 0x00015FC8
		[NullableContext(1)]
		private bool NodeIsNull(NodeEvent nodeEvent)
		{
			if (nodeEvent.Tag == "tag:yaml.org,2002:null")
			{
				return true;
			}
			Scalar scalar = nodeEvent as Scalar;
			if (scalar != null && scalar.Style == ScalarStyle.Plain && !scalar.IsKey)
			{
				string value = scalar.Value;
				return value == "" || value == "~" || value == "null" || value == "Null" || value == "NULL";
			}
			return false;
		}
	}
}
