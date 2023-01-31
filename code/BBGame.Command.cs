
namespace OITC;

partial class BBGame
{
	[ConVar.Replicated( "oitc_debug" )]
	public static bool DebugMode { get; set; } = false;

	[ConVar.Replicated( "oitc_score_limit" )]
	public static int ScoreLimit { get; set; } = 10;

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
			var player = (c.Pawn as BBPlayer);
			//this is chtupid tbh
			player.RemoveAmmo( player.PistolAmmo );
			player.AwardAmmo( 1 );
			player.Respawn();
			c.SetValue( "kills", 0 );
			c.SetValue( "deaths", 0 );
		}

		Current.State.Text = "Fight!";
		Current.State.Tier = GameState.MidGame;
	}

	[ConCmd.Admin( "give_ammo" )]
	public static void GiveAmmo()
	{
		Game.AssertServer();
		(ConsoleSystem.Caller.Pawn as BBPlayer).AwardAmmo( BBPlayer.MaxAmmo );
	}

	[ConCmd.Admin( "give_ammo_all" )]
	public static void GiveAmmoAll()
	{
		Game.AssertServer();
		foreach ( var c in Game.Clients )
		{
			var player = (c.Pawn as BBPlayer);
			player.AwardAmmo( 4 );
		}
	}

	[ConCmd.Admin( "oitc_cookie" )]
	public static void CookieFlashlight()
	{
		Game.AssertServer();
		(Sandbox.ConsoleSystem.Caller.Pawn as BBPlayer).SetCookieFlashlightCookie();
	}

	[ConCmd.Admin( "kill" )]
	public static void Kill()
	{
		if ( ConsoleSystem.Caller.Pawn is not BBPlayer ply )
			return;

		ply.TakeDamage( DamageInfo.Generic( 10000 ) );
	}

}
