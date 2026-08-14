using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x0600033B RID: 827
	public delegate TComponent WrapperFactory<[Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped) where TComponent : TComponentBase;
}
