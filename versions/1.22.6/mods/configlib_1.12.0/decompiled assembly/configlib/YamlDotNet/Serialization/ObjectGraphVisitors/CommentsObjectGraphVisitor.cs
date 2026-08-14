using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CommentsObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x00016081 File Offset: 0x00014281
		public CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001608C File Offset: 0x0001428C
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			YamlMemberAttribute customAttribute = key.GetCustomAttribute<YamlMemberAttribute>();
			if (customAttribute != null && customAttribute.Description != null)
			{
				context.Emit(new Comment(customAttribute.Description, false));
			}
			return base.EnterMapping(key, value, context);
		}
	}
}
