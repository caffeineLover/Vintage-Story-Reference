using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityDrifter : EntityHumanoid
{
	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		AnimationTrigger animationTrigger = new AnimationTrigger();
		animationTrigger.OnControls = new EnumEntityActivity[1] { EnumEntityActivity.Dead };
		AnimationTrigger trigger = animationTrigger;
		int oddsToAlter = properties.Attributes["oddsToAlter"].AsInt(5);
		string dieAnimationCode = properties.Attributes[" "].AsString("die");
		string alternativeDieAnimationCode = properties.Attributes["alternativeDieAnimationCode"].AsString("crawldie");
		if (EntityId % oddsToAlter == 0L)
		{
			float[] collisionBox = properties.Attributes["alternativeCollisionBox"].AsArray(new float[2] { 0.9f, 0.6f });
			ArgumentNullException.ThrowIfNull(collisionBox, "collisionBox");
			Dictionary<string, string> animationsRemapping = new Dictionary<string, string>
			{
				{ "idle", "crawlidle" },
				{ "standwalk", "crawlwalk" },
				{ "standlowwalk", "crawlwalk" },
				{ "standrun", "crawlrun" },
				{ "standidle", "crawlidle" },
				{ "standdespair", "crawlemote" },
				{ "standcry", "crawlemote" },
				{ "standhurt", "crawlhurt" },
				{ "standdie", "crawldie" }
			};
			if (properties.Attributes.KeyExists("animationsRemapping"))
			{
				animationsRemapping = properties.Attributes["animationsRemapping"].AsObject<Dictionary<string, string>>();
				ArgumentNullException.ThrowIfNull(animationsRemapping, "animationsRemapping");
			}
			AnimationMetaData dieAnimationMetaData = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == dieAnimationCode);
			AnimationMetaData alternativeDieAnimationMetaData = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == alternativeDieAnimationCode);
			if (dieAnimationMetaData != null)
			{
				dieAnimationMetaData.TriggeredBy = null;
			}
			if (alternativeDieAnimationMetaData != null)
			{
				alternativeDieAnimationMetaData.TriggeredBy = trigger;
			}
			properties.CollisionBoxSize = new Vec2f(collisionBox[0], collisionBox[1]);
			properties.SelectionBoxSize = new Vec2f(collisionBox[0], collisionBox[1]);
			string idleAnimationCode = properties.Attributes["idleAnimationCode"].AsString("idle");
			string alternativeIdleAnimationCode = properties.Attributes["alternativeIdleAnimationCode"].AsString("crawlidle");
			AnimationMetaData idleAnimationMetaData = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == idleAnimationCode);
			AnimationMetaData alternativeIdleAnimationMetaData = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == alternativeIdleAnimationCode);
			if (idleAnimationMetaData != null)
			{
				idleAnimationMetaData.TriggeredBy = null;
			}
			if (alternativeIdleAnimationMetaData != null)
			{
				alternativeIdleAnimationMetaData.TriggeredBy = new AnimationTrigger
				{
					DefaultAnim = true
				};
			}
			bool alternativeCanClimb = properties.Attributes["alternativeCanClimb"].AsBool();
			properties.CanClimb = alternativeCanClimb;
			AnimManager = new RemapAnimationManager(animationsRemapping)
			{
				IdleAnimation = alternativeIdleAnimationCode
			};
		}
		else
		{
			AnimationMetaData dieAnimationMetaData2 = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == dieAnimationCode);
			AnimationMetaData alternativeDieAnimationMetaData2 = properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == alternativeDieAnimationCode);
			if (dieAnimationMetaData2 != null)
			{
				dieAnimationMetaData2.TriggeredBy = trigger;
			}
			if (alternativeDieAnimationMetaData2 != null)
			{
				alternativeDieAnimationMetaData2.TriggeredBy = null;
			}
		}
		base.Initialize(properties, api, InChunkIndex3d);
	}
}
