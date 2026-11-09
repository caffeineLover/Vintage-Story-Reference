using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009E RID: 158
	public interface ITrackingRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060003B5 RID: 949
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;
	}
}
