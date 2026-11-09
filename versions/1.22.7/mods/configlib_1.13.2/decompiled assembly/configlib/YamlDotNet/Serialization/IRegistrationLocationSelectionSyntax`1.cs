using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009D RID: 157
	public interface IRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060003B0 RID: 944
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003B1 RID: 945
		void Before<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003B2 RID: 946
		void After<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060003B3 RID: 947
		void OnTop();

		// Token: 0x060003B4 RID: 948
		void OnBottom();
	}
}
