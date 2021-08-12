using Sandbox;
using System.Collections.Generic;

public partial class BBGame : Game
{

	public int NumPlayers => Client.All.Count;
	public enum GameStateTier : byte
	{
		WaitingForPlayers,
		Warmup,
		MidGame,
		RoundOver
	}

	public class GameState
	{
		public ulong TopFragSteamId;
		public string TopFragName;
		public GameStateTier Tier;

		public string TierText
		{
			get
			{
				string returnText = "you shouldnt be seeing this";
				switch ( Tier )
				{
					case GameStateTier.WaitingForPlayers:
						returnText = "WAITING FOR PLAYERS...";
						break;

					case GameStateTier.RoundOver:
						returnText = "GAME OVER!";
						break;


					case GameStateTier.MidGame:
						returnText = "FIGHT!";
						break;

					default:
						returnText = "";
						break;
				}

				return returnText;
			}
		}

	}

	private void SetGameState( GameState newState )
	{
		Host.AssertServer();
		CurrentGameState = new GameState
		{
			TopFragSteamId = newState.TopFragSteamId,
			Tier = newState.Tier
		};
		SetGameStateClient( newState.TopFragSteamId, newState.Tier );
	}

	[ClientRpc]
	private void SetGameStateClient( ulong topFragSteamId, GameStateTier newTier )
	{
		Host.AssertClient();
		CurrentGameState = new GameState
		{
			TopFragSteamId = topFragSteamId,
			Tier = newTier
		};

		HudGameState.OnStateChanged.Invoke();
	}


	private void ReCalculateGameState()
	{
		Host.AssertServer();
		if ( CurrentGameState == null )
		{
			CurrentGameState = new GameState
			{

			};
		}

		if ( CurrentGameState.Tier == GameStateTier.RoundOver ) return;
		if ( NumPlayers < 2 )
		{
			SetGameState( new GameState
			{
				Tier = GameStateTier.WaitingForPlayers
			} );
		}

	}
}
