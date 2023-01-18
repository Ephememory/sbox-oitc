namespace OITC;
using Sandbox.UI;
using System;

public partial class HudGameState : Panel
{

	public static Action OnStateChanged;
	public static Action OnNumPlayersFulfilled;

	public HudGameState()
	{
		OnStateChanged += () =>
		{
			SetClass( "toggled", BBGame.Current.CurrentGameState.Tier == GameState.MidGame );
		};

		OnNumPlayersFulfilled += () =>
		{
			SetClass( "toggled", true );
		};
	}

	public override void Tick()
	{
		base.Tick();
	}
}
