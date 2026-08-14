using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000009 RID: 9
	// (Invoke) Token: 0x0600003F RID: 63
	public delegate TComponent WrapperFactory<[Nullable(2)] TArgument, [Nullable(2)] TComponentBase, TComponent>(TComponentBase wrapped, TArgument argument) where TComponent : TComponentBase;
}
