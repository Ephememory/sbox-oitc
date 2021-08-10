using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

[Library( "banana-battle", Title = "Sticks & Stones" )]
partial class BBGame : Game
{

	[ConVar.Replicated( "sas_self_damage", Help = "Whether weapons can hurt their owner." )]
	public static bool sas_self_damage { get; set; } = true;

	[ConVar.Replicated( "sas_debug" )]
	public static bool sas_debug { get; set; } = false;

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


}
