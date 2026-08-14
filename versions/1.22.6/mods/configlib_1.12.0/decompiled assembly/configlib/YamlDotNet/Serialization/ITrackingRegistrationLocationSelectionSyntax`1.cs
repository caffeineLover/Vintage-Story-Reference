using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A2 RID: 162
	public interface ITrackingRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060003F1 RID: 1009
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;
	}
}
