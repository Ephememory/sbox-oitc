using Sandbox;

public partial class BBGame : Sandbox.Game
{
	[ConVar.Replicated( "oitc_debug" )]
	public static bool oitc_debug { get; set; } = false;


	[ConVar.Replicated]
	public static int oitc_score_limit { get; set; } = 10;


	[ConVar.Replicated]
	public static float oitc_time_limit { get; set; }

	[ServerCmd( "oitc_restart" )]
	public static void RestartGame()
	{
		Host.AssertServer();
		Log.Info( ConsoleSystem.Caller );
		var game = (Game.Current as BBGame);
		foreach ( var c in Client.All )
		{
			var player = (c.Pawn as BBPlayer);
			//this is chtupid tbh
			player.RemoveAmmo( player.PistolAmmo );
			player.AwardAmmo( 1 );
			player.Respawn();
			c.SetValue( "kills", 0 );
			c.SetValue( "deaths", 0 );
		}

		game.SetGameState( new GameState
		{
			TopFragSteamId = game.CurrentGameState.TopFragSteamId,
			TopFragName = game.CurrentGameState.TopFragName,
			Tier = GameStateTier.MidGame
		} );
	}

	[ServerCmd( "give_fal" )]
	public static void GiveFAL()
	{

		Host.AssertServer();

		//:)
		if ( Sandbox.ConsoleSystem.Caller.PlayerId != 76561197998255119 ) return;

		(ConsoleSystem.Caller.Pawn as BBPlayer).Inventory.Add( new WeaponFAL(), true );
	}

	[ServerCmd( "give_ammo" )]
	public static void GiveAmmo()
	{
		Host.AssertServer();
		if ( Sandbox.ConsoleSystem.Caller.PlayerId != 76561197998255119 ) return;
		(ConsoleSystem.Caller.Pawn as BBPlayer).AwardAmmo( 4 );
		ConsoleSystem.Caller.Pawn.PlaySound( "squish" );
	}

	[ServerCmd( "give_ammo_all" )]
	public static void GiveAmmoAll()
	{
		Host.AssertServer();
		if ( Sandbox.ConsoleSystem.Caller.PlayerId != 76561197998255119 ) return;
		var game = (Game.Current as BBGame);
		foreach ( var c in Client.All )
		{
			var player = (c.Pawn as BBPlayer);
			player.AwardAmmo( 4 );

		}
	}

	[ServerCmd( "gamestate" )]
	public static void GetGameState()
	{
		Host.AssertServer();
		if ( Sandbox.ConsoleSystem.Caller.PlayerId != 76561197998255119 ) return;
		Utils.UtilLog( (Game.Current as BBGame).CurrentGameState.Tier );
	}

	[ClientVar( "fov" )]
	public static float PlayerFov { get; set; } = 90;

	[ServerCmd( "oitc_cookie" )]
	public static void CookieFlashlight()
	{
		Host.AssertServer();
		(Sandbox.ConsoleSystem.Caller.Pawn as BBPlayer).SetCookieFlashlightCookie();
	}

}
