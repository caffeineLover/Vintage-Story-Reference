using System;
using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

public class RenderOrderReservation
{
	public double rangeStart;

	public double rangeEnd;

	public Type allowedType1;

	public Type allowedType2;

	public bool Conflicts(IRenderer handler, double newRenderOrder)
	{
		if (newRenderOrder < rangeStart)
		{
			return false;
		}
		if (newRenderOrder > rangeEnd)
		{
			return false;
		}
		Type testedType = handler.GetType();
		if (allowedType1.IsAssignableFrom(testedType))
		{
			return false;
		}
		if (allowedType2.IsAssignableFrom(testedType))
		{
			return false;
		}
		return true;
	}
}
