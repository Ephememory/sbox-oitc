using Sandbox;


partial class BBPlayer : Player
{

	private DamageInfo lastDamage;
	public BBPlayer()
	{
		Inventory = new Inventory( this );
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
			LightCookie = Texture.Load( "materials/effects/lightcookie.vtex" )
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

		Dress();

		Inventory.Add( new WeaponBanana(), true );
		FlashlightBatteryCharge = 100f;

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
		Inventory.DropActive();
		Inventory.DeleteContents();
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
		{
			info.Damage *= 10.0f;
		}

		lastDamage = info;

		//TookDamage( lastDamage.Flags, lastDamage.Position, lastDamage.Force );

		base.TakeDamage( info );
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{

		Log.Info( eventData.Velocity );
		if ( eventData.Entity.IsValid() && eventData.Velocity.y >= 30f )
		{
			TakeDamage( new DamageInfo
			{
				Flags = DamageFlags.Fall,
				Damage = eventData.Velocity.y
			} );
		}

		base.OnPhysicsCollision( eventData );

	}

	[ClientRpc]
	public void PlayClientSound( string snd )
	{
		PlaySound( snd );
	}

}
