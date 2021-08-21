using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class HudGameState : Panel
{

	private BBGame.GameState CurrentState => (Game.Current as BBGame).CurrentGameState;

	public static Action OnStateChanged;
	public static Action OnNumPlayersFulfilled;
	private Label label;

	public HudGameState()
	{
		label = Add.Label( null, null );
		OnStateChanged += () =>
		{
			SetClass( "toggled", CurrentState.Tier == BBGame.GameStateTier.MidGame );
			label.SetText( CurrentState.TierText );
		};

		OnNumPlayersFulfilled += () =>
		{
			SetClass( "toggled", true );
		};
	}


}
