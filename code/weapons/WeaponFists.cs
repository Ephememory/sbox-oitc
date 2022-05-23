using Sandbox;

public partial class WeaponFists : Weapon
{
	public override string ViewModelPath => "models/weapons/fists/v_fists.vmdl";
	public override float PrimaryRate => 0.9f;
	public override float ReloadTime => 0f;

	public override string GetKilledByText()
	{
		var options = new string[4] { "beat down", "pummeled", "clocked", "clobbered" };
		return Rand.FromArray<string>( options );
	}

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		if ( !IsClient ) return;
		//ViewModelEntity.FieldOfView = 54;
	}

	public override bool CanReload()
	{
		return false;
	}

	private async void AttackAsync( float delay )
	{
		//Async delay so we do attack logic at the right client-side anim frame
		//this is more important than you might think.
		await GameTask.DelaySeconds( delay );
		Melee( 1000f, 90f );

		if ( IsClient && Owner == Local.Pawn )
		{
			//new Sandbox.ScreenShake.Perlin( 1.2f, 1.2f, 1.4f, 1.9f );
		}
	}

	public override void AttackPrimary()
	{

		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		ShootEffects();
		AttackAsync( 0.34f );
	}

	public override void AttackSecondary()
	{
		//Do absolutely nothing
	}


	/// <summary>
	/// Custom ShootBullets function for melee.
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public bool Melee( float damage, float range = DefaultBulletRange )
	{
		//Because this is async, players can die before this is actually executed and be dead while it is.
		//nullcheck the owner!
		if ( Owner == null ) return false;
		var pos = Owner.EyePosition;
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;


		foreach ( var tr in TraceBullet( pos, pos + forward * range, 15f ) )
		{
			if ( tr.Entity.IsValid() )
			{
				tr.Surface.DoBulletImpact( tr );
			}
			else
			{
				continue;
			}

			if ( !IsServer ) continue;
			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100 * 1, damage )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
				PlaySound( "punch" );
				return true;


			}


		}

		if ( IsServer )
		{
			PlaySound( "punch_miss" );
		}


		return false;
	}



	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		//CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 5 ); // TODO this is shit
												//anim.SetAnimParameter( "aimat_weight", 1f );
	}

	public override void OnCarryDrop( Entity dropper )
	{
		Delete();
	}
}
