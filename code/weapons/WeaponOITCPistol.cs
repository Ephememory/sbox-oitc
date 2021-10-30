using Sandbox;

[Library( "weapon_oitcpistol", Title = "Pistol", Spawnable = true )]
public partial class WeaponOITCPistol : Weapon
{
	public override string ViewModelPath => "models/weapons/pistol/v_oitc_pistol.vmdl";



	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/pistol/oitc_pistol.vmdl" );

	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		if ( !IsClient ) return;
		ViewModelEntity.FieldOfView = 78;
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{

		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;
		if ( Owner is not BBPlayer player ) return;
		if ( player.PistolAmmo <= 0 )
		{
			return;
		}

		(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );
		player.RemoveAmmo( 1 );
		ShootEffects();
		PlaySound( "oitc_pistolsound" );
		ShootBullet( 0, 1, 1000, 1 );



	}

	public override void AttackSecondary()
	{

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
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection" );
		base.ShootEffects();
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 );
		anim.SetParam( "aimat_weight", 1.0f );
		anim.SetParam( "holdtype_handedness", 1 );
	}
}
