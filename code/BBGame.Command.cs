using Sandbox;


public partial class BBGame : Sandbox.Game
{
	[ConVar.Replicated( "bb_debug" )]
	public static bool bb_debug { get; set; } = false;


	[ConVar.Replicated]
	public static int bb_score_limit { get; set; } = 30;


	[ConVar.Replicated]
	public static float bb_time_limit { get; set; }

	[ServerCmd( "bb_restart" )]
	public static void RestartGame()
	{
		Host.AssertServer();
		var game = (Game.Current as BBGame);
		foreach ( var c in Client.All )
		{
			var player = (c.Pawn as BBPlayer);
			player.BananaAmmo = 1;
			player.Respawn();
			c.SetScore( "kills", 0 );
			c.SetScore( "deaths", 0 );
		}

		game.SetGameState( new GameState
		{
			TopFragSteamId = game.CurrentGameState.TopFragSteamId,
			TopFragName = game.CurrentGameState.TopFragName,
			Tier = GameStateTier.MidGame
		} );


	}
}
