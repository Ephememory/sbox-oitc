namespace OITC;

public partial class WeaponOITCPistol : Weapon
{
	public string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl_c";

	public override string GetKilledByText()
	{
		var options = new string[5] { "smoked", "popped", "gunned down", "iced", "spun the block on" };
		return Game.Random.FromArray<string>( options );
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

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
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
		Game.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection" );
		base.ShootEffects();
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
		anim.Handedness = CitizenAnimationHelper.Hand.Right;
	}
}
