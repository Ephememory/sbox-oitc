global using Sandbox;

namespace OITC;

partial class BBGame : GameManager
{
	[Net]
	public GameStateInfo State { get; private set; }

	public static new BBGame Current => GameManager.Current as BBGame;

	public BBGame()
	{
		if ( Game.IsClient )
		{
			_ = new Hud();
		}
	}

	public override void ClientJoined( IClient cl )
	{
		NumPlayers++;
		var player = new BBPlayer( cl );
		cl.Pawn = player;
		player.Respawn();

		base.ClientJoined( cl );

		// Randomly gives you the cookie cookie.
		if ( Game.Random.Int( 1, 420 ) == 69 )
		{
			player.SetCookieFlashlightCookie();
			Chat.AddChatEntry( To.Single( cl ), "OITC", "Enjoy your cookie.", 0 );
		}

		Chat.AddChatEntry(To.Everyone, "", $"{cl.Name} joined the game.", cl.SteamId.ToString() );

		if ( Game.IsClient )
			return;

		ReCalculateGameState();

		// Only do this on join to avoid 3 - 1 causing a round restart.
		if ( NumPlayers == 2 )
		{
			EndRound();
			NumPlayerFulfilled();
		}
	}

	public override void ClientDisconnect( IClient cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
		NumPlayers--;
		if ( Game.IsClient )
			return;

		ReCalculateGameState();
	}

	public override void OnKilled( IClient client, Entity pawn )
	{
		// Override the base OnKilled but do not invoke it.
		Game.AssertServer();

		Log.Info( $"{client.Name} was killed" );

		if ( pawn.LastAttacker != null )
		{
			if ( pawn.LastAttacker.Client != null )
			{
				var killedByText = (pawn.LastAttackerWeapon as Weapon).GetKilledByText();

				if ( string.IsNullOrEmpty( killedByText ) )
				{
					killedByText = pawn.LastAttackerWeapon?.ClassName;
				}

				OnKilledClient( pawn.LastAttacker.Client, client, killedByText );
			}
			else
			{
				OnKilledClient( client, client, "killed" );
			}
		}
		else
		{
			OnKilledClient( client, null, "died" );
		}

		if ( Game.IsClient ) return;
		var killer = pawn.LastAttacker;
		var weapon = pawn.LastAttackerWeapon;

		if ( killer == null )
			return; //Watch out for suicides!

		if ( pawn is not BBPlayer killed )
			return;

		if ( killer is BBPlayer ply )
		{
			var amountToAward = weapon.GetType() == typeof( Fists ) ? 2 : 1;
			ply.AwardAmmo( amountToAward );
		}

		if ( State.Tier != GameState.MidGame )
			return;

		var killerClient = killer.Client;
		var killerKills = killerClient.GetValue<int>( "kills" );

		if ( killerKills >= oitc_score_limit - 1 )
		{
			State.Tier = GameState.RoundOver;
			State.Text = "Game over!";
			EndRound();
		}

		Log.Info( $"{client.Name} was killed by {killer.Client.NetworkIdent} with {weapon}" );
	}

	[ClientRpc]
	public void OnKilledClient( IClient killer, IClient victim, string method )
	{
		Killfeed.Current.AddEntry( killer, victim, method );
	}

	private async void EndRound()
	{
		EndRoundClient();
		await GameTask.DelayRealtimeSeconds( 5f );
		RestartGame();
	}

	[ClientRpc]
	private void EndRoundClient()
	{
		Game.AssertClient();

		RestartTimer.UntilRestart = 5;
	}

	[ClientRpc]
	private void NumPlayerFulfilled()
	{
		Game.AssertClient();
		HudGameState.OnNumPlayersFulfilled?.Invoke();
	}

}
