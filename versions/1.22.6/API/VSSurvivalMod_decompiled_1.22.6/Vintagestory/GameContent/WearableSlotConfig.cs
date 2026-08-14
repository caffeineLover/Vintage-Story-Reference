using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class WearableSlotConfig
{
	[DocumentAsJson("Required", "", true)]
	public SeatConfig SeatConfig;

	[DocumentAsJson("Required", "", false)]
	public string Code;

	[DocumentAsJson("Recommended", "", false)]
	public string[] ForCategoryCodes;

	[DocumentAsJson("Optional", "None", false)]
	public string[] BehindSlots;

	[DocumentAsJson("Required", "", false)]
	public string AttachmentPointCode;

	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, StepParentElementTo> StepParentTo;

	public string ProvidesSeatId;

	[DocumentAsJson("Optional", "False", false)]
	public bool EmptyInteractPassThrough { get; set; }

	public bool CanHold(string code)
	{
		for (int i = 0; i < ForCategoryCodes.Length; i++)
		{
			if (ForCategoryCodes[i] == code)
			{
				return true;
			}
		}
		return false;
	}
}
