namespace OITC;
using Sandbox.UI;
using System;

public partial class GameStateDisplay : Panel
{
	public static Action OnStateChanged;
	public static Action OnNumPlayersFulfilled;

	public GameStateDisplay()
	{
		OnNumPlayersFulfilled += () =>
		{
			SetClass( "toggled", true );
		};
	}
}
