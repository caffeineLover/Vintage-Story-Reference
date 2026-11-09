using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000165 RID: 357
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class PlayerModelSkinSubscription : IDisposable
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x000551BB File Offset: 0x000533BB
		private PlayerModelSkinSubscription(object skinBehavior, EventInfo onModelChangedEvent, Action<string> handler)
		{
			this._skinBehavior = skinBehavior;
			this._onModelChangedEvent = onModelChangedEvent;
			this._handler = handler;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000551D8 File Offset: 0x000533D8
		[return: Nullable(2)]
		public static PlayerModelSkinSubscription Subscribe(Entity entity, Action<string> onModelChanged, bool invokeCurrentModel)
		{
			EntityBehavior skinBehavior = entity.GetBehavior("skinnableplayercustommodel");
			if (skinBehavior == null)
			{
				return null;
			}
			Type skinBehaviorType = skinBehavior.GetType();
			EventInfo onModelChangedEvent = skinBehaviorType.GetEvent("OnModelChanged", 20);
			if (onModelChangedEvent == null)
			{
				return null;
			}
			onModelChangedEvent.AddEventHandler(skinBehavior, onModelChanged);
			if (invokeCurrentModel)
			{
				PropertyInfo property = skinBehaviorType.GetProperty("CurrentModelCode", 20);
				string modelCode = ((property != null) ? property.GetValue(skinBehavior) : null) as string;
				if (modelCode != null)
				{
					onModelChanged.Invoke(modelCode);
				}
			}
			return new PlayerModelSkinSubscription(skinBehavior, onModelChangedEvent, onModelChanged);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00055253 File Offset: 0x00053453
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this._onModelChangedEvent.RemoveEventHandler(this._skinBehavior, this._handler);
		}

		// Token: 0x04000844 RID: 2116
		private const string BehaviorName = "skinnableplayercustommodel";

		// Token: 0x04000845 RID: 2117
		private const string OnModelChangedEventName = "OnModelChanged";

		// Token: 0x04000846 RID: 2118
		private const string CurrentModelCodePropertyName = "CurrentModelCode";

		// Token: 0x04000847 RID: 2119
		private readonly object _skinBehavior;

		// Token: 0x04000848 RID: 2120
		private readonly EventInfo _onModelChangedEvent;

		// Token: 0x04000849 RID: 2121
		private readonly Action<string> _handler;

		// Token: 0x0400084A RID: 2122
		private bool _disposed;
	}
}
