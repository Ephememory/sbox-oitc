using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class HudGameState : Panel
{
	private BBGame.GameState CurrentState => (GameManager.Current as BBGame).CurrentGameState;

	public static Action OnStateChanged;
	public static Action OnNumPlayersFulfilled;

	public HudGameState()
	{
		OnStateChanged += () =>
		{
			SetClass("toggled", CurrentState.Tier == BBGame.GameStateTier.MidGame);
		};

		OnNumPlayersFulfilled += () =>
		{
			SetClass("toggled", true);
		};
	}
}
