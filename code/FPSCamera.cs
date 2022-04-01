using Sandbox;


public partial class FPSCamera : CameraMode
{
	Vector3 lastPos;
	public Rotation ViewKick { get; private set; } = Rotation.From( 0f, 0f, 0f );

	public override void Activated()
	{
		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		Position = pawn.EyePosition;
		Rotation = pawn.EyeRotation;

		lastPos = Position;
	}

	public override void Update()
	{
		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		var EyePosition = pawn.EyePosition;
		if ( EyePosition.Distance( lastPos ) < 300 ) // TODO: Tweak this, or add a way to invalidate lastpos when teleporting
		{
			Position = Vector3.Lerp( EyePosition.WithZ( lastPos.z ), EyePosition, 20.0f * Time.Delta );
		}
		else
		{
			Position = EyePosition;
		}

		Rotation = pawn.EyeRotation;
		FieldOfView = BBGame.PlayerFov;

		Viewer = pawn;
		lastPos = Position;
	}


	public void AddViewKick( Rotation rot )
	{
		ViewKick += rot;
	}
}
