﻿namespace OITC;

partial class BBPlayer : BasePlayer
{
	[Net]
	[Predicted]
	public SpotLightEntity FlashlightEntity { get; private set; }
	[Net]
	public bool FlashlightEnabled { get; private set; }
	[Net]
	[Predicted]
	public float FlashlightPosOffset { get; private set; }
	[Net]
	public float FlashlightBatteryCharge { get; private set; } = 100f;

	[ConVar.Replicated]
	public static float oitc_flashlight_drain_amt { get; set; } = 1;

	private static bool debug { get; set; }

	// [ConCmd.Client( "bf_debug_flashlight" )]
	// public static void DebugFlashlight()
	// {
	// 	debug = !debug;
	// }

	private void FlashlightSimulate()
	{
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
			if ( FlashlightBatteryCharge - oitc_flashlight_drain_amt < 0 )
			{
				FlashlightBatteryCharge = -1;
			}
			else
			{
				FlashlightBatteryCharge -= oitc_flashlight_drain_amt;
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
		FlashlightEntity.Position = EyePosition + EyeRotation.Forward * FlashlightPosOffset;
		FlashlightEntity.Rotation = EyeRotation;

		//FlashlightEntity.Flicker = FlashlightBatteryCharge <= 13f; //jesus christ what was i thinking
		FlashlightEntity.BrightnessMultiplier = FlashlightBatteryCharge <= 13f ? 0.1f : 0.9f; //this is weird
	}

	private void FlashlightFrameSimulate()
	{
		//for client side smooth movement
		if ( FlashlightEntity != null && FlashlightEntity.IsValid() )
		{
			// This calculation is pretty much ripped from Source 1, minus some ladder stuff and a double check they do
			// inside the player hull. Works great for me!

			const float pullLerp = 0.0002f; //yeah :(
			float pullbackCutoff = 128f; // the distance when we start pulling the flashlight behind player
			float far = FlashlightEntity.Range; //the overall max range of the trace and spotlight

			Vector3 origin = EyePosition + (Vector3.Up * -14); // offset to chest height-ish;
			Vector3 dest = origin + EyeRotation.Forward * far;
			Vector3 dir = dest - origin;

			// check for intersections in front
			var tr = Trace.Box( Vector3.One * 4, origin, dest ).Ignore( this ).Ignore( ActiveChild ).Run();

			float distance = (tr.EndPosition - origin).Length;
			float pullBackAmt = 0;
			float pullAmtModifier = 0;

			bool needsPull = distance < pullbackCutoff; // for debug drawing
			bool needsPush = false; // ditto

			// we intersected something, so to stop the flashlight from becoming a tiny dot
			// as we move towards the object, pull it back to make it actually illuminate the object
			if ( distance < pullbackCutoff )
			{
				pullBackAmt = pullbackCutoff - distance;
				pullAmtModifier = MathX.Lerp( pullAmtModifier, pullBackAmt, pullLerp );

				var backTr = Trace.Box( Vector3.One * 4, new Ray( origin, -dir ), 0.02f ).Ignore( this ).Ignore( ActiveChild ).Run();
				needsPush = backTr.Hit; // do debug stuff

				if ( backTr.Hit )
				{
					float max = (backTr.EndPosition - origin).Length * pullLerp;
					if ( pullAmtModifier > max )
						pullAmtModifier = max;

					if ( debug )
						DebugOverlay.Line( origin, backTr.EndPosition, Color.Cyan );
				}
			}
			else
			{
				pullAmtModifier = MathX.Lerp( pullAmtModifier, pullBackAmt, 0 );
			}

			origin -= dir * pullAmtModifier;
			FlashlightEntity.Position = origin;
			FlashlightEntity.Rotation = EyeRotation;

			// debug draw the same way Source does
			if ( debug )
			{
				DebugOverlay.Sphere( origin, 2, needsPush ? Color.Random : Color.Green, depthTest: false );
				DebugOverlay.Line( origin, tr.EndPosition, Color.Red );
				DebugOverlay.Box( tr.EndPosition, Vector3.One * -4, Vector3.One * 4, needsPull ? Color.Random : Color.Blue, depthTest: false );
			}
		}
	}
}
