using Sandbox;
using System;

[Library( "oitc", Title = "One In The Chamber" )]
partial class BBGame : Game
{

	public GameState CurrentGameState { get; private set; }

	private RealTimeUntil timeLimit { get; set; }

	public BBGame()
	{
		if ( IsServer )
		{
			_ = new BBHud();
		}
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );
		NumPlayers++;

		var player = new BBPlayer( cl );
		cl.Pawn = player;
		player.Respawn();
		if ( IsClient ) return;

		ReCalculateGameState();

		//Only do this on join to avoid 3 - 1 causing a round restart.
		if ( NumPlayers == 2 )
		{
			EndRound();
			NumPlayerFulfilled();
		}

	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
		NumPlayers--;
		if ( IsClient ) return;
		ReCalculateGameState();
	}


	public override void OnKilled( Client client, Entity pawn )
	{
		base.OnKilled( client, pawn );
		if ( Host.IsClient ) return;
		var killer = pawn.LastAttacker;
		var weapon = pawn.LastAttackerWeapon;

		if ( killer == null ) return; //Watch out for suicides!
		if ( pawn is not BBPlayer killed ) return;

		if ( killer is BBPlayer ply )
		{
			var amountToAward = weapon.GetType() == typeof( WeaponFists ) ? 2 : 1;
			ply.AwardAmmo( amountToAward );

		}

		if ( CurrentGameState.Tier == GameStateTier.RoundOver ) return;

		var killedClient = killed.Client;
		killedClient.SetValue( "deaths", killedClient.GetValue<int>( "deaths" ) + 1 );

		var killerClient = killer.Client;
		var killerKills = killerClient.GetValue<int>( "kills" );

		if ( killerKills >= bb_score_limit - 1 )
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
		Sandbox.ConsoleSystem.Run( "bb_restart" );
	}

	[ClientRpc]
	private void EndRoundClient()
	{
		Host.AssertClient();

		HudGameRestartTime.OnRoundOver.Invoke();
	}

	[ClientRpc]
	private void NumPlayerFulfilled()
	{
		Host.AssertClient();
		HudGameState.OnNumPlayersFulfilled.Invoke();
	}

}
