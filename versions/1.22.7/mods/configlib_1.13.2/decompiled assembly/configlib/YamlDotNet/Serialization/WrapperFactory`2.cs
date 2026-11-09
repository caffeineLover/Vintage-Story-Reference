using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200007C RID: 124
	// (Invoke) Token: 0x060002FF RID: 767
	public delegate TComponent WrapperFactory<[Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped) where TComponent : TComponentBase;
}
