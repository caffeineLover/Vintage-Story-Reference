using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CommentsObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00008383 File Offset: 0x00006583
		public CommentsObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor) : base(nextVisitor)
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000838C File Offset: 0x0000658C
		public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			YamlMemberAttribute customAttribute = key.GetCustomAttribute<YamlMemberAttribute>();
			if (customAttribute != null && customAttribute.Description != null)
			{
				context.Emit(new Comment(customAttribute.Description, false));
			}
			return base.EnterMapping(key, value, context, serializer);
		}
	}
}
