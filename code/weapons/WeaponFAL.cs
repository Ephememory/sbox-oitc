using Sandbox;


[Library( "weapon_fal", Title = "FAL", Spawnable = true )]
public partial class WeaponFAL : Weapon
{


	public override float PrimaryRate => 6f;
	public override float SecondaryRate => 2f;

	public override string ViewModelPath => "models/weapons/fal/v_fal.vmdl";
	public override float ReloadTime => 1.34f;



	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/fal/weapon_fal.vmdl" );

	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		ViewModelEntity?.SetAnimParameter( "deploy", true );
		if ( IsClient )
		{
			//ViewModelEntity.FieldOfView = 90;
		}

	}

	public override void AttackPrimary()
	{

		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;
		//TimeSinceSecondaryAttack = 0;
		(Owner as AnimEntity)?.SetAnimParameter( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "rust_pumpshotgun.shoot" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0, 1, 5, 1 );

	}

	public override void AttackSecondary()
	{
		base.AttackSecondary();
		TimeSinceSecondaryAttack = 0;
		ViewModelEntity?.SetAnimParameter( "deploy", true );
		if ( IsServer ) return;

	}


	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection" );

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin( 0.5f, 4.0f, 1.0f, 0.5f );

		}


		ViewModelEntity?.SetAnimParameter( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		anim.SetAnimParameter( "aimat_weight", 1f );
	}
}
