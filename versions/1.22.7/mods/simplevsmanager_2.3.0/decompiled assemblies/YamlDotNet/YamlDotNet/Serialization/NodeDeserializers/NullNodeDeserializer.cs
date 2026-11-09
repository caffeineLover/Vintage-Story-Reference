using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200007F RID: 127
	public sealed class NullNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00009FF4 File Offset: 0x000081F4
		[NullableContext(1)]
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			value = null;
			NodeEvent nodeEvent;
			if (parser.Accept(out nodeEvent) && NullNodeDeserializer.NodeIsNull(nodeEvent))
			{
				parser.SkipThisAndNestedEvents();
				return true;
			}
			return false;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A020 File Offset: 0x00008220
		[NullableContext(1)]
		private static bool NodeIsNull(NodeEvent nodeEvent)
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
