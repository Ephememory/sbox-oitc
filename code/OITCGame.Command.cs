
namespace OITC;

partial class OITCGame
{
	[ConVar.Replicated( "oitc_debug" )]
	public static bool DebugMode { get; set; } = false;

	[ConVar.Replicated( "oitc_score_limit" )]
	public static int ScoreLimit { get; set; } = 15;

	[ConVar.Replicated( "oitc_time_limit" )]
	public static float TimeLimit { get; set; }

	[ConVar.Server( "oitc_dev_only", Help = "Locks the server and prevents anyone else from connecting." )]
	public static bool DevOnly { get; set; }

	[ConCmd.Server( "oitc_restart" )]
	public static void RestartGame()
	{
		Game.AssertServer();

		foreach ( var c in Game.Clients )
		{
			if ( c.Pawn is not Player player )
				continue;

			player.SetAmmo( 1 );
			player.Respawn();
			c.SetValue( "kills", 0 );
			c.SetValue( "deaths", 0 );
		}

		Current.State.Text = "FIGHT!";
		Current.State.Tier = GameState.MidGame;
	}

	[ConCmd.Admin( "set_ammo" )]
	public static void SetAmmo()
	{
		Game.AssertServer();
		(ConsoleSystem.Caller.Pawn as Player).SetAmmo( Player.MaxAmmo );
	}

	[ConCmd.Admin( "set_ammo_ammo_all" )]
	public static void SetAmmoAll()
	{
		Game.AssertServer();
		foreach ( var c in Game.Clients )
		{
			if ( c.Pawn is not Player player )
				continue;

			player.AwardAmmo( Player.MaxAmmo );
		}
	}

	[ConCmd.Admin( "kill" )]
	public static void Kill()
	{
		if ( ConsoleSystem.Caller.Pawn is not Player ply )
			return;

		ply.TakeDamage( DamageInfo.Generic( 10000 ) );
	}

}
