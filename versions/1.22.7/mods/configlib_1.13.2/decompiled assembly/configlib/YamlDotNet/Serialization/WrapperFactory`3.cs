using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200007D RID: 125
	// (Invoke) Token: 0x06000303 RID: 771
	public delegate TComponent WrapperFactory<[Nullable(2)] TArgument, [Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped, TArgument argument) where TComponent : TComponentBase;
}
