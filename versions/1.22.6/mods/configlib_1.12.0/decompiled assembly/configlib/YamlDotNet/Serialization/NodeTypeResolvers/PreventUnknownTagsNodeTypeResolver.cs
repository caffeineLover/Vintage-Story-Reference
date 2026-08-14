using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013B RID: 315
	public class PreventUnknownTagsNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x00017358 File Offset: 0x00015558
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
