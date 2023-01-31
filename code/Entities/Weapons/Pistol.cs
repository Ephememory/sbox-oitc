using System.Collections.Generic;

namespace OITC;

public partial class Pistol : Weapon
{
	public override string ViewModelPath => "models/weapons/v_pistol.vmdl";
	public override float SecondaryRate => 0.9f;

	private readonly List<string> _primaryFlavorText = new List<string> { "smoked", "popped", "gunned down", "iced", "spun the block on", "poked a hole in", "shot" };
	private readonly List<string> _secondaryFlavorText = new List<string> { "beat down", "pummeled", "clocked", "clobbered", "milly rocked" };

	public override string GetKillMethod( DamageInfo dmg )
	{
		if ( dmg.HasTag( DamageTags.Bullet ) )
			return Game.Random.FromList( _primaryFlavorText );
		else if ( dmg.HasTag( DamageTags.Blunt ) )
			return Game.Random.FromList( _secondaryFlavorText );

		return string.Empty;

	}

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/pistol/pistol.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();
		TimeSincePrimaryAttack = 0;

		if ( Owner is not BBPlayer player )
			return;

		if ( player.PistolAmmo <= 0 )
			return;


		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
		player.RemoveAmmo( 1 );
		ShootEffects();
		PlaySound( "oitc_pistolsound" );
		ShootBullet( 0, 1, 1000, 1 );
	}

	public override void AttackSecondary()
	{
		Melee( 1000f, 90f );
	}

	/// <summary>
	/// Custom ShootBullets function for melee.
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	public bool Melee( float damage, float range = DefaultBulletRange )
	{
		if ( !Owner.IsValid() || Owner is not BBPlayer ply )
			return false;

		var pos = ply.EyePosition;
		var forward = ply.EyeRotation.Forward;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( pos, pos + forward * range, 15f ) )
		{
			if ( !tr.Entity.IsValid() )
			{
				continue;

			}

			tr.Surface.DoBulletImpact( tr );

			if ( !Game.IsServer )
				continue;

			using ( Prediction.Off() )
			{
				var dmg = new DamageInfo()
					.WithTag( DamageTags.Blunt )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				dmg.Position = tr.EndPosition;
				dmg.Force = forward * 100;
				dmg.Damage = damage;

				tr.Entity.TakeDamage( dmg );
				PlaySound( "punch" );
				return true;
			}
		}

		if ( Game.IsServer )
			PlaySound( "punch_miss" );

		return false;
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
