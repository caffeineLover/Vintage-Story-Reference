using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000008 RID: 8
	// (Invoke) Token: 0x0600003B RID: 59
	public delegate TComponent WrapperFactory<[Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped) where TComponent : TComponentBase;
}
