using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000024 RID: 36
	public interface ITrackingRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060000E1 RID: 225
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;
	}
}
