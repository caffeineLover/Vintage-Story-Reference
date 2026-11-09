using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorDrunkTyping : EntityBehavior
{
	private ICoreAPI api;

	private bool isCommand;

	public EntityBehaviorDrunkTyping(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject typeAttributes)
	{
		api = entity.World.Api;
	}

	public override void OnEntityLoaded()
	{
		if (entity.Api is ICoreClientAPI capi && (entity as EntityPlayer)?.PlayerUID == capi.Settings.String["playeruid"])
		{
			capi.Event.RegisterEventBusListener(onChatKeyDownPre, 1.0, "chatkeydownpre");
			capi.Event.RegisterEventBusListener(onChatKeyDownPost, 1.0, "chatkeydownpost");
		}
	}

	public override string PropertyName()
	{
		return "drunktyping";
	}

	private void onChatKeyDownPre(string eventName, ref EnumHandling handling, IAttribute data)
	{
		string text = ((data as TreeAttribute)["text"] as StringAttribute).value;
		isCommand = text.Length > 0 && (text[0] == '.' || text[0] == '/');
	}

	private void onChatKeyDownPost(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute treeAttr = data as TreeAttribute;
		int keyCode = (treeAttr["key"] as IntAttribute).value;
		string text = (treeAttr["text"] as StringAttribute).value;
		int deltacaretPos = 0;
		if (isCommand && text.Length > 0 && text[0] != '.' && text[0] != '/')
		{
			string newtext = text[0].ToString() ?? "";
			for (int i = 1; i < text.Length; i++)
			{
				newtext = slurText(newtext, ref deltacaretPos);
				newtext += text[i];
			}
			text = newtext;
			(treeAttr["text"] as StringAttribute).value = text;
		}
		else if (keyCode != 53 && keyCode != 47 && keyCode != 48 && keyCode != 55 && keyCode != 5 && keyCode != 3 && text.Length > 0 && text[0] != '.' && text[0] != '/')
		{
			text = slurText(text, ref deltacaretPos);
			(treeAttr["text"] as StringAttribute).value = text;
		}
		treeAttr.SetInt("deltacaretpos", deltacaretPos);
	}

	private string slurText(string text, ref int caretPos)
	{
		Random rnd = api.World.Rand;
		float intox = entity.WatchedAttributes.GetFloat("intoxication");
		if (rnd.NextDouble() < (double)intox)
		{
			switch (rnd.Next(9))
			{
			case 0:
			case 1:
				if (text.Length > 1)
				{
					text = text.Substring(0, text.Length - 2) + text[text.Length - 1] + text[text.Length - 2];
				}
				break;
			case 2:
			case 3:
			case 4:
				if (text.Length > 0)
				{
					text += text[text.Length - 1];
					caretPos++;
				}
				break;
			case 5:
			case 6:
			{
				if (text.Length <= 0)
				{
					break;
				}
				string[] keybLayout = new string[4] { "1234567890-", "qwertyuiop[", "asdfghjkl;", "zxcvbnm,." };
				char lastchar = text[text.Length - 1];
				for (int i = 0; i < 3; i++)
				{
					int index = keybLayout[i].IndexOf(lastchar);
					if (index >= 0)
					{
						int rndoffset = rnd.Next(2) * 2 - 1;
						text += keybLayout[i][GameMath.Clamp(index + rndoffset, 0, keybLayout[i].Length - 1)];
						caretPos++;
					}
				}
				break;
			}
			}
		}
		return text;
	}
}
