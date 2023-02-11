using Sandbox.UI;

namespace OITC;

public partial class Player : BasePlayer
{
	[ConVar.Replicated( "oitc_max_ammo" )]
	public static int MaxAmmo { get; set; } = 2;

	[Net, Change( nameof( OnChangePistolAmmo ) )]
	public int PistolAmmo { get; private set; } = 1;

	public DamageInfo LastDamage { get; private set; }

	[Net]
	public Player LastPlayerAttacker { get; private set; }

	// TODO: Move DeathCam to an ephemeral component.
	/// <summary>
	/// For when the LastPlayerAttacker becomes obstructed during death cam.
	/// </summary>
	private Vector3 _deathCamLookPosition;

	/// <summary>
	/// The clothing container is what dresses the citizen
	/// </summary>
	public ClothingContainer Clothing = new();

	public Player()
	{
		Inventory = new Inventory( this );
	}

	/// <summary>
	/// Initialize using this client
	/// </summary>
	public Player( IClient cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );
	}

	public override void Respawn()
	{
		Controller = new WalkController();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Clothing.DressEntity( this );
		Inventory = new Inventory( this );
		Inventory.Add( new Pistol(), true );

		PistolAmmo = 1;
		base.Respawn();
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );
		SimulateAnimation( (WalkController)Controller );
	}

	public override void FrameSimulate( IClient cl )
	{
		if ( LifeState == LifeState.Dead )
		{
			if ( !LastPlayerAttacker.IsValid() )
				return;

			var tr = Trace.Ray( EyePosition, LastPlayerAttacker.EyePosition )
				.WithoutTags( "player" )
				.WithAnyTags( "solid" )
				.Run();

			if ( !tr.Hit )
				_deathCamLookPosition = LastPlayerAttacker.EyePosition;

			Camera.Position = EyePosition;
			Camera.Rotation = Rotation.Slerp( Camera.Rotation, Rotation.LookAt( _deathCamLookPosition - Camera.Position ), Time.Delta * 4f );
			Camera.FieldOfView = MathX.Lerp( Camera.FieldOfView, Screen.CreateVerticalFieldOfView( 42 ), Time.Delta * 2 );

			return;
		}

		base.FrameSimulate( cl );
	}

	private void SimulateAnimation( WalkController controller )
	{
		if ( controller == null )
			return;

		// where should we be rotated to
		var turnSpeed = 0.02f;

		Rotation rotation;

		// If we're a bot, spin us around 180 degrees.

		rotation = ViewAngles.ToRotation();

		var idealRotation = Rotation.LookAt( rotation.Forward.WithZ( 0 ), Vector3.Up );
		Rotation = Rotation.Slerp( Rotation, idealRotation, controller.WishVelocity.Length * Time.Delta * turnSpeed );
		Rotation = Rotation.Clamp( idealRotation, 45.0f, out var shuffle ); // lock facing to within 45 degrees of look direction

		var animHelper = new CitizenAnimationHelper( this );

		animHelper.WithWishVelocity( controller.WishVelocity );
		animHelper.WithVelocity( Velocity );
		animHelper.WithLookAt( EyePosition + EyeRotation.Forward * 100.0f, 1.0f, 1.0f, 0.5f );
		animHelper.AimAngle = rotation;
		animHelper.FootShuffle = shuffle;
		animHelper.DuckLevel = MathX.Lerp( animHelper.DuckLevel, controller.HasTag( "ducked" ) ? 1 : 0, Time.Delta * 10.0f );
		animHelper.VoiceLevel = (Game.IsClient && Client.IsValid()) ? Client.Voice.LastHeard < 0.5f ? Client.Voice.CurrentLevel : 0.0f : 0.0f;
		animHelper.IsGrounded = GroundEntity != null;
		animHelper.IsSitting = controller.HasTag( "sitting" );
		animHelper.IsNoclipping = controller.HasTag( "noclip" );
		animHelper.IsClimbing = controller.HasTag( "climbing" );
		animHelper.IsSwimming = this.GetWaterLevel() >= 0.5f;
		animHelper.IsWeaponLowered = false;

		if ( controller.HasEvent( "jump" ) )
			animHelper.TriggerJump();

		//if ( ActiveChild != LastActiveChild )
		//	animHelper.TriggerDeploy();

		if ( ActiveChild is not null && ActiveChild is Weapon wpn )
			wpn.SimulateAnimator( animHelper );
		else
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.None;
			animHelper.AimBodyWeight = 0.5f;
		}
	}

	public override void OnKilled()
	{
		base.OnKilled();

		EnableDrawing = false;
		EnableAllCollisions = false;
		EnableDrawing = false;

		Inventory.DeleteContents();
	}

	public override void TakeDamage( DamageInfo info )
	{
		LastDamage = info;

		if ( info.Attacker is Player player )
			LastPlayerAttacker = player;

		base.TakeDamage( info );
	}

	public void AwardAmmo( int amt )
	{
		Game.AssertServer();

		if ( PistolAmmo >= MaxAmmo )
			return;

		var result = PistolAmmo + amt;

		if ( result > MaxAmmo )
		{
			PistolAmmo = MaxAmmo;
		}
		else
		{
			PistolAmmo += amt;
		}
	}

	public void RemoveAmmo( int amtToRemove )
	{
		PistolAmmo -= amtToRemove;
	}

	public void SetAmmo( int value )
	{
		Game.AssertServer();
		if ( value >= MaxAmmo )
		{
			PistolAmmo = MaxAmmo;
			return;
		}

		PistolAmmo = value;
	}

	public void SwitchToPistol()
	{
		Inventory.SetActiveSlot( 1, false );
	}

	[ClientRpc]
	public void PlayClientSound( string snd )
	{
		PlaySound( snd );
	}

	public void OnChangePistolAmmo( int oldValue, int newValue )
	{
		if ( Game.IsServer )
			return;

		Game.AssertClient();
		Event.Run( "oitc.player_ammo_change", oldValue, newValue );
	}

}
