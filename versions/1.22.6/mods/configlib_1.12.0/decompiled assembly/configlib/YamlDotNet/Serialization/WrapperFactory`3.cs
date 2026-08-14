using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x0600033F RID: 831
	public delegate TComponent WrapperFactory<[Nullable(2)] TArgument, [Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped, TArgument argument) where TComponent : TComponentBase;
}
