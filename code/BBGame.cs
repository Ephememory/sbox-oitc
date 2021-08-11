using Sandbox;
using System.Linq;

[Library( "banana-battle", Title = "Sticks & Stones" )]
partial class BBGame : Game
{
	[ConVar.Replicated( "bb_debug" )]
	public static bool bb_debug { get; set; } = false;

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
		player.Respawn();
		cl.Pawn = player;
	}


	public override void OnKilled( Client client, Entity pawn )
	{
		base.OnKilled( client, pawn );
		var killer = pawn.LastAttacker;
		var weapon = pawn.LastAttackerWeapon;

		if ( killer == null ) return; //Watch out for suicides!
		if ( pawn is not BBPlayer killed ) return;

		if ( killer.Inventory is BaseInventory inv && killer is BBPlayer ply )
		{
			var bananGun = inv.List.Where( x => x.GetType() == typeof( WeaponBanana ) )
			.FirstOrDefault() as WeaponBanana; //nasty linq
			var amountToAward = weapon.GetType() == typeof( WeaponFists ) ? 2 : 1; //this does assume we only have 2 weapons :)
			ply.AwardAmmo( amountToAward );

			if ( killer.Inventory.Active is WeaponFists )
			{
				ply.Inventory.SetActiveSlot( 1, false );
			}
		}

		var killedClient = killed.GetClientOwner();
		killedClient.SetScore( "deaths", killedClient.GetScore<int>( "deaths" ) + 1 );
		var killerClient = killer.GetClientOwner();
		killerClient.SetScore( "kills", killerClient.GetScore<int>( "kills" ) + 1 );

		Log.Info( $"{client.Name} was killed by {killer.GetClientOwner().NetworkIdent} with {weapon}" );
	}


}
