using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000073 RID: 115
	public class PreventUnknownTagsNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002CD RID: 717 RVA: 0x000096B8 File Offset: 0x000078B8
		[NullableContext(1)]
		bool INodeTypeResolver.Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			if (nodeEvent != null && !nodeEvent.Tag.IsEmpty)
			{
				Mark start = nodeEvent.Start;
				Mark end = nodeEvent.End;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Encountered an unresolved tag '");
				defaultInterpolatedStringHandler.AppendFormatted<TagName>(nodeEvent.Tag);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new YamlException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return false;
		}
	}
}
