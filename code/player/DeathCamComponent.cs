using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OITC;

internal partial class DeathCamComponent : EntityComponent<Player>
{
	[Net] public Player Killer { get; set; }
	private Vector3 LookAtPosition { get; set; }

	[Event.Client.Frame]
	void Frame()
	{
		if ( Entity is not Player player || !Killer.IsValid() )
			return;

		var tr = Trace.Ray( player.EyePosition, Killer.EyePosition )
			.WithoutTags( "player" )
			.WithAnyTags( "solid" )
			.Run();

		if ( !tr.Hit )
			LookAtPosition = Killer.EyePosition;

		Camera.Position = player.EyePosition;
		Camera.Rotation = Rotation.Slerp( Camera.Rotation, Rotation.LookAt( LookAtPosition - Camera.Position ), Time.Delta * 4f );
		Camera.FieldOfView = MathX.Lerp( Camera.FieldOfView, Screen.CreateVerticalFieldOfView( 42 ), Time.Delta * 2 );
	}
}
