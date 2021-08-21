using Sandbox;
using System;

[Library( "banana-battle", Title = "Banana Battle" )]
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

		var player = new BBPlayer();
		cl.Pawn = player;
		player.Respawn();
		if ( IsClient ) return;

		ReCalculateGameState();

		//Only do this on join to avoid 3 - 1 causing a round restart.
		if ( NumPlayers == 2 )
			EndRound();
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
		if ( IsClient ) return;

		ReCalculateGameState();
	}


	public override void OnKilled( Client client, Entity pawn )
	{
		base.OnKilled( client, pawn );
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

		var killedClient = killed.GetClientOwner();
		killedClient.SetScore( "deaths", killedClient.GetScore<int>( "deaths" ) + 1 );

		var killerClient = killer.GetClientOwner();
		var killerKills = killerClient.GetScore<int>( "kills" );
		killerClient.SetScore( "kills", killerKills + 1 );

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

		Log.Info( $"{client.Name} was killed by {killer.GetClientOwner().NetworkIdent} with {weapon}" );
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

}
