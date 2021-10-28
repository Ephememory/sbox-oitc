using Sandbox;


partial class BBPlayer : Player
{

	[ConVar.Replicated]
	public static int bb_max_ammo_held { get; set; } = 7;

	[Net]
	public int PistolAmmo { get; private set; } = 1;

	private DamageInfo lastDamage;

	/// <summary>
	/// The clothing container is what dresses the citizen
	/// </summary>
	public Clothing.Container Clothing = new();
	public BBPlayer()
	{
		Inventory = new Inventory( this );
	}

	/// <summary>
	/// Initialize using this client
	/// </summary>
	public BBPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Spawn()
	{
		base.Spawn();
		FlashlightEntity = new SpotLightEntity
		{
			Enabled = false,
			DynamicShadows = true,
			Range = 3200f,
			Falloff = 0.3f,
			LinearAttenuation = 0.3f,
			Brightness = 25f,
			Color = Color.FromBytes( 200, 200, 200, 230 ),
			InnerConeAngle = 9,
			OuterConeAngle = 32,
			FogStength = 1.0f,
			Owner = this,
			LightCookie = Texture.Load( Rand.Int( 1, 420 ) == 69 ? "textures/cookie.vtex" : "materials/effects/lightcookie.vtex" )
		};
		FlashlightPosOffset = 30f;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Host.AssertClient();

	}

	public override void Respawn()
	{
		Camera = new FPSCamera();
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Clothing.DressEntity( this );
		Inventory = new Inventory( this );

		Inventory.Add( new WeaponFists(), false );
		Inventory.Add( new WeaponOITCPistol(), true );

		FlashlightBatteryCharge = 100f;

		//Just to make sure no one gets stuck with an empty pistol.
		if ( PistolAmmo <= 0 )
		{
			SwitchToFists();
		}

		base.Respawn();
	}


	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;



		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );

		if ( IsClient ) return;


		if ( Input.Released( InputButton.View ) )
		{
			if ( Camera is FPSCamera )
			{
				Camera = new ThirdPersonCamera();
			}
			else
			{
				Camera = new FPSCamera();
			}

		}

		TickFlashLight();


	}


	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		//Update the flashlight position on the client in framesim
		//so the movement is nice and smooth.
		if ( FlashlightEntity != null && FlashlightEntity.IsValid() )
		{
			FlashlightEntity.Position = EyePos + EyeRot.Forward * FlashlightPosOffset;
			FlashlightEntity.Rotation = EyeRot;
		}

	}

	public override void OnKilled()
	{
		base.OnKilled();

		BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
		Camera = new SpectateRagdollCamera();
		EnableDrawing = false;
		Controller = null;
		EnableAllCollisions = false;
		EnableDrawing = false;
		var dropped = Inventory.DropActive();
		if ( dropped.IsValid() )
		{
			dropped.DeleteAsync( 2f );
		}

		Inventory.DeleteContents();
	}

	public override void TakeDamage( DamageInfo info )
	{

		lastDamage = info;
		base.TakeDamage( info );
	}



	//could use setter/getters but this seems more clear.
	public void AwardAmmo( int amt )
	{
		Host.AssertServer();

		if ( PistolAmmo > bb_max_ammo_held ) return;
		if ( PistolAmmo + amt > bb_max_ammo_held )
		{
			PistolAmmo = bb_max_ammo_held;
		}
		else
		{
			PistolAmmo += amt;
		}

		//If we are being rewarded ammo and we currently have out fists out
		//by force, switch back to pistol.
		if ( Inventory.Active is WeaponFists )
		{
			SwitchToPistol();
		}

	}

	public void RemoveAmmo( int amtToRemove )
	{
		PistolAmmo -= amtToRemove;
		if ( PistolAmmo <= 0 )
		{
			SwitchToFists();
		}

	}

	//more human readable functions, considering the scope of this mode, its fine.
	public void SwitchToFists()
	{
		Inventory.SetActiveSlot( 0, false );
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

}
