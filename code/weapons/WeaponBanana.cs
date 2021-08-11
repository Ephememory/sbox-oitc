using Sandbox;

[Library( "weapon_banana", Title = "Banana", Spawnable = true )]
public partial class WeaponBanana : Weapon
{
	public override string ViewModelPath => "models/weapons/banana/v_banana.vmdl";



	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/banana/banana.vmdl" );

	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		if ( !IsClient ) return;
		ViewModelEntity.FieldOfView = 50;
	}

	public override void AttackPrimary()
	{

		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;
		if ( Owner is not BBPlayer player ) return;
		if ( player.BananaAmmo <= 0 )
		{
			return;
		}

		(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );
		player.BananaAmmo--;
		ShootEffects();
		PlaySound( "kersplat" );

		ShootBullet( 0, 1, 1000, 1 );
		if ( player.BananaAmmo <= 0 )
		{
			(Owner as BBPlayer)?.Inventory.SetActiveSlot( 0, false );
		}


	}

	public override void AttackSecondary()
	{
		base.AttackSecondary();
		TimeSinceSecondaryAttack = 0;
		if ( IsClient ) return;
	}


	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		//TODO: Banana peel particles :)

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin( 0.5f, 4.0f, 1.0f, 0.5f );

		}

		ViewModelEntity?.SetAnimBool( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1f );
	}
}
