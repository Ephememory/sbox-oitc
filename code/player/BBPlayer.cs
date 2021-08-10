using Sandbox;


partial class BBPlayer : Player
{


	public BBPlayer()
	{
		Inventory = new Inventory( this );
	}

	public override void Spawn()
	{
		base.Spawn();
		Log.Info( "Spawning player!" );
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
		FlashlightPosOffset = 23f;
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		Host.AssertClient();

		Log.Info( $"TEXTURE: {FlashlightEntity.LightCookie}" );
		var texture_data = new byte[4096 * 4];
		for ( int i = 0; i < texture_data.Length; i++ )
		{
			texture_data[i] = (byte)Rand.Int( 0, 255 );
		}
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


		Inventory.Add( new WeaponFAL(), true );
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

		if ( Input.Released( InputButton.Flashlight ) && FlashlightBatteryCharge > 0 )
		{
			FlashlightEnabled = !FlashlightEnabled;
			using ( Prediction.Off() )
			{
				PlayClientSound( FlashlightEntity.Enabled ? "flashlight-on" : "flashlight-off" );
			}
		}


		if ( !FlashlightEntity.IsValid() )
			return;

		FlashlightEntity.Enabled = FlashlightEnabled && FlashlightBatteryCharge > 0f;

		if ( FlashlightEnabled && FlashlightBatteryCharge > 0f && Time.Tick % 14 == 1 )
		{
			if ( FlashlightBatteryCharge - sas_flashlight_drain_amt < 0 )
			{
				FlashlightBatteryCharge = -1;
			}
			else
			{
				FlashlightBatteryCharge -= sas_flashlight_drain_amt;
			}

			if ( FlashlightBatteryCharge < 1 )
			{
				FlashlightEnabled = false;
				FlashlightEntity.TurnOff();
				using ( Prediction.Off() )
				{
					PlayClientSound( FlashlightEntity.Enabled ? "flashlight-on" : "flashlight-off" );
				}
			}
		}

		if ( !FlashlightEnabled && FlashlightBatteryCharge < 100 && Time.Tick % 7 == 1 )
		{
			FlashlightBatteryCharge++;
		}

		//Setting the position of the flashlight serverside
		//basically, the position for other players the client is seeing.
		FlashlightEntity.Position = EyePos + EyeRot.Forward * FlashlightPosOffset;
		FlashlightEntity.Rotation = EyeRot;

		FlashlightEntity.Flicker = FlashlightBatteryCharge <= 13f;


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
		Inventory.DeleteContents();
		EnableDrawing = false;
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
	public virtual void PlayFailSound()
	{
		Host.AssertClient();

		PlaySound( "player_use_fail" );
	}


	[ClientRpc]
	public void PlayClientSound( string snd )
	{
		PlaySound( snd );
	}



}
