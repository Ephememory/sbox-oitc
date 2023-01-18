namespace OITC;
using Sandbox.UI;
using System;

public partial class HudGameState : Panel
{

	public static Action OnStateChanged;
	public static Action OnNumPlayersFulfilled;

	public HudGameState()
	{
		OnNumPlayersFulfilled += () =>
		{
			SetClass( "toggled", true );
		};
	}
}
