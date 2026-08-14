using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000023 RID: 35
	public interface IRegistrationLocationSelectionSyntax<[Nullable(2)] TBaseRegistrationType>
	{
		// Token: 0x060000DC RID: 220
		void InsteadOf<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060000DD RID: 221
		void Before<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060000DE RID: 222
		void After<TRegistrationType>() where TRegistrationType : TBaseRegistrationType;

		// Token: 0x060000DF RID: 223
		void OnTop();

		// Token: 0x060000E0 RID: 224
		void OnBottom();
	}
}
