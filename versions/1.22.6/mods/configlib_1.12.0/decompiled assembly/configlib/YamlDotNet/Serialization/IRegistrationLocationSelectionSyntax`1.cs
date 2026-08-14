using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A1 RID: 161
	public interface IRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060003EC RID: 1004
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003ED RID: 1005
		void Before<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003EE RID: 1006
		void After<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003EF RID: 1007
		void OnTop();

		// Token: 0x060003F0 RID: 1008
		void OnBottom();
	}
}
