global using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace OITC;

partial class OITCGame : GameManager
{
	[Net]
	public GameStateInfo State { get; private set; }

	public static new OITCGame Current => GameManager.Current as OITCGame;

	public OITCGame()
	{
		if ( Game.IsClient )
		{
			_ = new Hud();
		}
	}

	public override bool ShouldConnect( long playerId )
	{
#if DEBUG
		if ( DevOnly )
			return false;
#endif
		return base.ShouldConnect( playerId );
	}

	public override void ClientJoined( IClient cl )
	{
		NumPlayers++;
		var player = new Player( cl );
		cl.Pawn = player;
		player.Respawn();

		base.ClientJoined( cl );

		Chat.AddChatEntry( To.Everyone, "", $"{cl.Name} joined the game.", cl.SteamId.ToString() );

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

		if ( pawn is not Player killed )
			return;

		if ( pawn.LastAttacker != null )
		{
			if ( pawn.LastAttacker.Client != null )
			{
				var killedByText = (pawn.LastAttackerWeapon as Weapon).GetKillMethod( killed.LastDamage );

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

		if ( Game.IsClient )
			return;

		var killer = pawn.LastAttacker;
		var weapon = pawn.LastAttackerWeapon;

		// Watch out for suicides.
		if ( killer == null )
			return;

		if ( killer is Player ply )
		{
			ply.AwardAmmo( killed.LastDamage.HasTag( DamageTags.Blunt ) ? 2 : 1 );
		}

		if ( State.Tier != GameState.MidGame )
			return;

		if ( killer.Client.GetValue<int>( "kills" ) >= ScoreLimit - 1 )
		{
			State.Tier = GameState.RoundOver;
			State.Text = "GAME OVER!";
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
		GameStateDisplay.OnNumPlayersFulfilled?.Invoke();
	}

}
