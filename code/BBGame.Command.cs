
namespace OITC;

partial class BBGame
{
	[ConVar.Replicated( "oitc_debug" )]
	public static bool oitc_debug { get; set; } = false;

	[ConVar.Replicated]
	public static int oitc_score_limit { get; set; } = 10;

	[ConVar.Replicated]
	public static float oitc_time_limit { get; set; }

	[ConCmd.Server( "oitc_restart" )]
	public static void RestartGame()
	{
		Game.AssertServer();
		Log.Info( ConsoleSystem.Caller );
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

	[ConCmd.Server( "give_ammo" )]
	public static void GiveAmmo()
	{
		Game.AssertServer();
		if ( Sandbox.ConsoleSystem.Caller.SteamId != 76561197998255119 ) return;
		(ConsoleSystem.Caller.Pawn as BBPlayer).AwardAmmo( 4 );
		//ConsoleSystem.Caller.Pawn.PlaySound( "squish" );
	}

	[ConCmd.Server( "give_ammo_all" )]
	public static void GiveAmmoAll()
	{
		Game.AssertServer();
		foreach ( var c in Game.Clients )
		{
			var player = (c.Pawn as BBPlayer);
			player.AwardAmmo( 4 );

		}
	}

	[ConCmd.Server( "oitc_cookie" )]
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
