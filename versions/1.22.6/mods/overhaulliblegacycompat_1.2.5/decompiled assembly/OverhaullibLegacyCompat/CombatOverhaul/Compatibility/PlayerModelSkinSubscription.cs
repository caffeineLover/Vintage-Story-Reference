using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000159 RID: 345
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class PlayerModelSkinSubscription : IDisposable
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x000538B3 File Offset: 0x00051AB3
		private PlayerModelSkinSubscription(object skinBehavior, EventInfo onModelChangedEvent, Action<string> handler)
		{
			this._skinBehavior = skinBehavior;
			this._onModelChangedEvent = onModelChangedEvent;
			this._handler = handler;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000538D0 File Offset: 0x00051AD0
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

		// Token: 0x06001164 RID: 4452 RVA: 0x0005394B File Offset: 0x00051B4B
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this._onModelChangedEvent.RemoveEventHandler(this._skinBehavior, this._handler);
		}

		// Token: 0x0400081E RID: 2078
		private const string BehaviorName = "skinnableplayercustommodel";

		// Token: 0x0400081F RID: 2079
		private const string OnModelChangedEventName = "OnModelChanged";

		// Token: 0x04000820 RID: 2080
		private const string CurrentModelCodePropertyName = "CurrentModelCode";

		// Token: 0x04000821 RID: 2081
		private readonly object _skinBehavior;

		// Token: 0x04000822 RID: 2082
		private readonly EventInfo _onModelChangedEvent;

		// Token: 0x04000823 RID: 2083
		private readonly Action<string> _handler;

		// Token: 0x04000824 RID: 2084
		private bool _disposed;
	}
}
