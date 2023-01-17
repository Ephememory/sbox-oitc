global using Sandbox;

namespace OITC;

[Library( "oitc", Title = "One In The Chamber" )]
partial class BBGame : GameManager
{
	public GameState CurrentGameState { get; private set; }

	private RealTimeUntil timeLimit { get; set; }

	public BBGame()
	{
		if ( Game.IsClient )
		{
			_ = new BBHud();
		}
	}

	public override void ClientJoined( IClient cl )
	{
		NumPlayers++;

		var player = new BBPlayer( cl );
		cl.Pawn = player;
		player.Respawn();

		base.ClientJoined( cl );

		var randomChance = Game.Random.Int( 1, 420 ) == 69;
		if ( randomChance )
		{
			player.SetCookieFlashlightCookie();
			//Sandbox.UI.ChatBox.AddInformation( To.Everyone, $"{cl.Name} rolled a lucky number and got the cookie flashlight!" );
		}

		if ( Game.IsClient )
			return;

		ReCalculateGameState();

		//Only do this on join to avoid 3 - 1 causing a round restart.
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
		//manually overwriting the base.onkilled and tweaking it instead of calling it. its does some dumb shit.
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

				OnKilledMessage( pawn.LastAttacker.Client.SteamId, pawn.LastAttacker.Client.Name,
					client.SteamId,
					client.Name,
					killedByText );
			}
			else
			{
				OnKilledMessage( pawn.LastAttacker.NetworkIdent, pawn.LastAttacker.ToString(), client.SteamId, client.Name, "killed" );
			}
		}
		else
		{
			OnKilledMessage( 0, "", client.SteamId, client.Name, "died" );
		}

		if ( Game.IsClient ) return;
		var killer = pawn.LastAttacker;
		var weapon = pawn.LastAttackerWeapon;

		if ( killer == null ) return; //Watch out for suicides!
		if ( pawn is not BBPlayer killed ) return;

		if ( killer is BBPlayer ply )
		{
			var amountToAward = weapon.GetType() == typeof( WeaponFists ) ? 2 : 1;
			ply.AwardAmmo( amountToAward );

		}

		if ( CurrentGameState.Tier != GameStateTier.MidGame ) return;

		var killedClient = killed.Client;
		var killerClient = killer.Client;
		var killerKills = killerClient.GetValue<int>( "kills" );

		if ( killerKills >= oitc_score_limit - 1 )
		{
			SetGameState( new GameState
			{
				TopFragSteamId = CurrentGameState.TopFragSteamId,
				TopFragName = CurrentGameState.TopFragName,
				Tier = GameStateTier.RoundOver
			} );

			EndRound();
		}

		Log.Info( $"{client.Name} was killed by {killer.Client.NetworkIdent} with {weapon}" );
	}

	private async void EndRound()
	{
		EndRoundClient();
		await GameTask.DelayRealtimeSeconds( 5f );
		ConsoleSystem.Run( "oitc_restart" );
	}

	[ClientRpc]
	private void EndRoundClient()
	{
		Game.AssertClient();

		//HudGameRestartTime.OnRoundOver.Invoke();
	}

	[ClientRpc]
	private void NumPlayerFulfilled()
	{
		Game.AssertClient();
		HudGameState.OnNumPlayersFulfilled?.Invoke();
	}

}
