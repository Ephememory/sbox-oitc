using Sandbox;

[Library( "weapon_fists", Title = "Fists", Spawnable = false )]
public partial class WeaponFists : Weapon
{
	public override string ViewModelPath => "models/weapons/fists/v_fists.vmdl";
	public override float PrimaryRate => 0.9f;
	public override float ReloadTime => 0f;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		if ( !IsClient ) return;
		ViewModelEntity.FieldOfView = 48;
	}
	private async void AttackAsync( float delay )
	{
		Host.AssertServer();
		//this is awesome
		await GameTask.DelaySeconds( delay );
		if ( Melee( 1000f, 80f ) )
		{
			PlaySound( "punch" );
			(Owner as BBPlayer)?.Inventory.SetActiveSlot( 1, false );
		}
		else
		{
			PlaySound( "punch_miss" );
		}

	}
	public override void AttackPrimary()
	{

		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;

		(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );
		ShootEffects();
		if ( IsClient ) return;
		AttackAsync( 0.34f );
	}

	public override void AttackSecondary()
	{
		base.AttackSecondary();
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;
	}


	/// <summary>
	/// Custom ShootBullets function for melee.
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public bool Melee( float damage, float range = DefaultBulletRange )
	{
		var pos = Owner.EyePos;
		var forward = Owner.EyeRot.Forward;
		forward = forward.Normal;


		foreach ( var tr in TraceBullet( pos, pos + forward * range, 0.1f ) )
		{
			if ( tr.Entity.IsValid() )
			{
				tr.Surface.DoBulletImpact( tr );
			}

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;
			//
			// We turn predictiuon off for this, so any exploding effects don't get culled etc
			//
			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100 * 1, damage )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
				return true;
			}
		}

		return false;
	}




	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();


		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin( 0.5f, 4.0f, 1.0f, 0.5f );

		}

		ViewModelEntity?.SetAnimBool( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 3 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1f );
	}

	public override void OnCarryDrop( Entity dropper )
	{
		Delete();
	}
}
