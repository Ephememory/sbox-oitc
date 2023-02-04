// Created by Crowbar 0.72

$modelname "player/mafia.mdl"

$bodygroup "mafia"
{
	studio "mafia_reference.smd"
}
$bodygroup "hat"
{
	studio "hat.smd"
	blank
}
$bodygroup "tie"
{
	studio "tie.smd"
}
$bodygroup "glasses"
{
	studio "glasses.smd"
	blank
}
$bodygroup "pipe"
{
	blank
	studio "pipe.smd"
}


$surfaceprop "flesh"

$contents "solid"

$illumposition 0.019 -1.392 39.563

$eyeposition 0 0 73

$opaque

$cdmaterials "models\player\mafia\"

$texturegroup "skinfamilies"
{
	{ "mafia_suit_base_variant_a" }
	{ "mafia_suit_base_variant_b" }
	{ "mafia_suit_base_variant_c" }
	{ "mafia_suit_base_variant_d" }
	{ "mafia_suit_base_variant_e" }
	{ "mafia_suit_base_variant_f" }
	{ "mafia_suit_base_variant_g" }
	{ "mafia_suit_base_variant_h" }
	{ "mafia_suit_base_variant_i" }
	{ "mafia_suit_base_variant_j" }
	{ "mafia_suit_base_variant_k" }
	{ "mafia_suit_base_variant_l" }
	{ "mafia_suit_base_variant_m" }
	{ "mafia_suit_base_variant_n" }
	{ "mafia_suit_base_variant_o" }
	{ "mafia_suit_base_variant_p" }
	{ "mafia_suit_base_variant_q" }
	{ "mafia_suit_base_variant_r" }
	{ "mafia_suit_base_variant_s" }
	{ "mafia_suit_base_variant_t" }
}

$attachment "forward" "ValveBiped.forward" 0 0 0 rotate 0 0 0
$attachment "headwear" "ValveBiped.Bip01_Head1" 8 1 0 rotate 0 -90 -90
$attachment "grenade0" "ValveBiped.Bip01_Pelvis" 7.03 3.57 -5.04 rotate -46.54 -172.45 81.91
$attachment "grenade1" "ValveBiped.Bip01_Pelvis" 3.54 3.74 -5.69 rotate -81.11 178.93 91.12
$attachment "grenade2" "ValveBiped.Bip01_Pelvis" 0.18 3.73 -6.99 rotate -83.65 -166.53 78.36
$attachment "grenade3" "ValveBiped.Bip01_Pelvis" -3.1 3.28 -6.97 rotate -79.99 13.72 -101.78
$attachment "grenade4" "ValveBiped.Bip01_Pelvis" -6.28 4.19 -5.32 rotate -59.27 3.3 -90.98
$attachment "secondary" "ValveBiped.Bip01_R_Thigh" -1.61 3.62 -6.16 rotate -5.76 -9.41 80.11
$attachment "primary" "ValveBiped.Bip01_Spine2" 11.39 -4.34 5.12 rotate -7.8 178.32 -179.54
$attachment "secondary2" "ValveBiped.Bip01_L_Thigh" -1.61 3.62 4.16 rotate -5.76 -9.41 99.89
$attachment "clue1" "ValveBiped.Bip01_Pelvis" 0 -2.62 -7.37 rotate -90 -45 0
$attachment "clue2" "ValveBiped.Bip01_Pelvis" 8.93 -0.95 -0.57 rotate -80 90 -90
$attachment "clue3" "ValveBiped.Bip01_Spine2" 3.45 -8.48 -6.08 rotate 15 -89.16 -90
$attachment "clue4" "ValveBiped.Bip01_Spine1" -8.07 -4.93 0.82 rotate 0 96.42 105
$attachment "clue5" "ValveBiped.Bip01_Pelvis" 9.21 -7.4 -2.98 rotate -55 -90 0
$attachment "lfoot" "ValveBiped.Bip01_L_Foot" 0 5.35 0 rotate 0 0 0
$attachment "rfoot" "ValveBiped.Bip01_R_Foot" 0 5.35 0 rotate 0 0 0
$attachment "muzzle_flash" "ValveBiped.Bip01_L_Hand" 0 0 0 rotate 0 0 0
$attachment "melee" "ValveBiped.Bip01_Pelvis" 9.01 -0.15 -0.47 rotate 55 90 180

$cbox 0 0 0 0 0 0

$bbox -13 -13 0 13 13 72

$hboxset "cstrike"
$hbox 3 "ValveBiped.Bip01_Pelvis" -8.5386 -7.9501 -5.5854 8.5386 2.0651 6.7731
$hbox 6 "ValveBiped.Bip01_L_Thigh" 4.708 -5.5319 -3.531 23.540001 3.8841 4.708
$hbox 6 "ValveBiped.Bip01_L_Calf" -0.2996 -4.4191 -3.531 21.485601 3.2421 3.531
$hbox 6 "ValveBiped.Bip01_L_Foot" -2.354 -1.177 -2.6536 5.885 4.708 2.0651
$hbox 6 "ValveBiped.Bip01_L_Toe0" -2.9425 -2.889 -2.9425 4.1195 1.2305 1.7655
$hbox 7 "ValveBiped.Bip01_R_Thigh" 4.708 -5.5319 -3.531 23.540001 3.8841 4.708
$hbox 7 "ValveBiped.Bip01_R_Calf" -0.2996 -4.4191 -3.531 21.485601 3.2421 3.531
$hbox 7 "ValveBiped.Bip01_R_Foot" -2.354 -1.177 -2.0651 5.885 4.708 2.6536
$hbox 7 "ValveBiped.Bip01_R_Toe0" -2.9425 -2.889 -1.7655 4.1195 1.2305 2.9425
$hbox 3 "ValveBiped.Bip01_Spine1" -9.416 -1.2947 -8.239 4.708 11.652301 8.239
$hbox 2 "ValveBiped.Bip01_Spine2" -3.1565 -3.531 -9.9724 13.963501 10.614401 9.9724
$hbox 1 "ValveBiped.Bip01_Neck1" -0.107 -4.708 -2.9425 5.671 2.354 2.9425
$hbox 1 "ValveBiped.Bip01_Head1" 0 -5.5 -4.5 12 4.5 4.5
$hbox 4 "ValveBiped.Bip01_L_UpperArm" -3 -3.7 -2.75 11 2.3 4.05
$hbox 4 "ValveBiped.Bip01_L_Forearm" -1.46 -2.46 -2.75 10.54 3.02 2.59
$hbox 4 "ValveBiped.Bip01_L_Hand" -1.5 -2.54 -1.77 7.5 1.59 2.94
$hbox 5 "ValveBiped.Bip01_R_UpperArm" -0.66 -3.5 -4.01 11.34 2.5 2.74
$hbox 5 "ValveBiped.Bip01_R_Forearm" -0.88 -2.45 -2.61 11.12 3.03 2.74
$hbox 5 "ValveBiped.Bip01_R_Hand" -1.5 -2.54 -2.94 7.5 1.59 1.77
$skipboneinbbox

$definebone "ValveBiped.Bip01_Pelvis" "" 0 -0.57095 41.266598 0 0 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Thigh" "ValveBiped.Bip01_Pelvis" 4.162778 0 0 2.970442 -89.701127 -89.978726 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Calf" "ValveBiped.Bip01_L_Thigh" 19.097538 -0.000091 -0.000002 0 1.959344 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Foot" "ValveBiped.Bip01_L_Calf" 17.68202 -0.000033 -0.000004 -2.355086 -61.399828 -1.374927 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine" "ValveBiped.Bip01_Pelvis" 0 3.579292 -3.190631 4.76615 89.999982 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.weapon_bone" "ValveBiped.Bip01_Spine" -44.907589 -1.112463 0.000033 -89.999982 -85.233897 0 0 0 0 0 0 0
$definebone "ValveBiped.weapon_bone_RHand" "ValveBiped.weapon_bone" -2 4 -2.354678 -27.017061 2.049635 -84.348383 0 0 0 0 0 0
$definebone "ValveBiped.weapon_bone_LHand" "ValveBiped.weapon_bone_RHand" 5.701225 0 0 52.017245 22.279218 90.808806 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Thigh" "ValveBiped.Bip01_Pelvis" -4.162778 0 0 2.970442 -90.299008 -90.012297 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Calf" "ValveBiped.Bip01_R_Thigh" 19.097549 -0.000104 -0.000002 0 1.958884 0.000115 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Foot" "ValveBiped.Bip01_R_Calf" 17.682024 0.000107 0 2.347236 -61.39976 2.425846 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine1" "ValveBiped.Bip01_Spine" 4.275547 -0.000035 0 0 -1.651034 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine2" "ValveBiped.Bip01_Spine1" 3.766716 -0.00003 0 0 5.574421 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine4" "ValveBiped.Bip01_Spine2" 9.545979 0.00007 -0.000001 0 10.681365 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Neck1" "ValveBiped.Bip01_Spine4" 3.5378 -0.000021 0 0 23.267533 179.999855 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Head1" "ValveBiped.Bip01_Neck1" 3.854145 -0.000031 0.000001 -0.000057 23.20565 0 0 0 0 0 0 0
$definebone "ValveBiped.forward" "ValveBiped.Bip01_Head1" 2.000038 -2.999987 0 0 -75.999358 -89.999927 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Clavicle" "ValveBiped.Bip01_Spine4" 2.203697 1.046362 2.073298 -73.711682 169.030093 91.086003 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_UpperArm" "ValveBiped.Bip01_L_Clavicle" 6.450115 0.000011 -0.000019 2.942138 -33.277785 -82.343668 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Forearm" "ValveBiped.Bip01_L_UpperArm" 12.511032 -0.000043 -0.000004 -0.000005 -3.458888 -0.000229 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Hand" "ValveBiped.Bip01_L_Forearm" 12.285391 -0.000078 -0.000034 -2.217807 2.063738 89.856698 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Clavicle" "ValveBiped.Bip01_Spine4" 2.203697 1.046365 -2.073298 73.711682 169.029984 -95.201615 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_UpperArm" "ValveBiped.Bip01_R_Clavicle" 6.450108 0.000027 0.000026 -0.682794 -33.426925 87.135708 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Forearm" "ValveBiped.Bip01_R_UpperArm" 12.511034 -0.000099 -0.000015 0.000003 -3.459404 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Hand" "ValveBiped.Bip01_R_Forearm" 12.285419 -0.000027 0 2.217233 2.063623 -89.380914 0 0 0 0 0 0
$definebone "ValveBiped.weapon_bone_Clip" "ValveBiped.weapon_bone_LHand" 0 0 0 0 -0.000113 -0.000054 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger0" "ValveBiped.Bip01_L_Hand" 0.897539 -0.321297 1.404672 -38.376654 -41.260688 -71.677139 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger01" "ValveBiped.Bip01_L_Finger0" 1.91507 -0.00001 0.000004 -0.123815 13.099181 -0.000343 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger02" "ValveBiped.Bip01_L_Finger01" 1.291489 -0.000001 -0.000004 -0.189076 20.799278 0.000116 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Wrist" "ValveBiped.Bip01_L_Forearm" 12.285425 -0.00008 0 -0.000003 0.000002 -0.057468 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Ulna" "ValveBiped.Bip01_L_Forearm" 6.142714 -0.00004 -0.000004 -0.000005 -0.000227 -0.030939 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger1" "ValveBiped.Bip01_L_Hand" 4.142275 0.125645 1.391982 -1.88904 -26.57572 20.194808 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger11" "ValveBiped.Bip01_L_Finger1" 1.839783 -0.000015 0.000025 0.291927 -20.798776 0.000342 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger2" "ValveBiped.Bip01_L_Hand" 4.153067 0.229729 0.428291 1.812325 -29.791284 5.02547 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger21" "ValveBiped.Bip01_L_Finger2" 1.839947 0.000008 -0.000011 0.28734 -20.997644 -0.000288 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger22" "ValveBiped.Bip01_L_Finger21" 1.293819 -0.000011 0.000005 0.159684 -12.399558 0.000115 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger2" "ValveBiped.Bip01_R_Hand" 4.153036 0.226181 -0.430154 -2.049472 -29.776278 -5.025643 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger21" "ValveBiped.Bip01_R_Finger2" 1.839962 -0.000011 -0.000008 -0.434415 -20.995942 -0.000168 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger22" "ValveBiped.Bip01_R_Finger21" 1.293819 -0.000002 -0.000005 -0.243104 -12.397558 0.000004 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger1" "ValveBiped.Bip01_R_Hand" 4.142252 0.114101 -1.392947 1.676761 -26.589657 -20.196074 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger11" "ValveBiped.Bip01_R_Finger1" 1.83979 0.000011 0.000017 -0.443868 -20.795339 0.000347 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger12" "ValveBiped.Bip01_R_Finger11" 1.176632 -0.00001 -0.000006 -0.281949 -13.997417 -0.000114 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger0" "ValveBiped.Bip01_R_Hand" 0.897533 -0.332966 -1.40195 38.061878 -41.540828 71.507081 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger01" "ValveBiped.Bip01_R_Finger0" 1.915073 -0.000003 0 0.18792 13.098265 0.000061 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger02" "ValveBiped.Bip01_R_Finger01" 1.291485 0.00001 0.000006 0.286984 20.79837 0.000169 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Wrist" "ValveBiped.Bip01_R_Forearm" 12.285419 -0.000027 0 0.000003 0.000001 0.487643 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Ulna" "ValveBiped.Bip01_R_Forearm" 6.14271 -0.000013 0 0.000005 -0.000228 0.263731 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Toe0" "ValveBiped.Bip01_R_Foot" 7.361016 -0.000008 -0.000005 -0.283557 -33.481529 -4.833415 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Toe0" "ValveBiped.Bip01_L_Foot" 7.361012 -0.000027 0 -0.306648 -33.481591 -0.571697 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger12" "ValveBiped.Bip01_L_Finger11" 1.176628 -0.000002 -0.000002 0.186388 -13.998803 0.000113 0 0 0 0 0 0

$bonemerge "ValveBiped.Bip01_Pelvis"
$bonemerge "ValveBiped.Bip01_Spine"
$bonemerge "ValveBiped.weapon_bone"
$bonemerge "ValveBiped.weapon_bone_RHand"
$bonemerge "ValveBiped.weapon_bone_LHand"
$bonemerge "ValveBiped.Bip01_Spine1"
$bonemerge "ValveBiped.Bip01_Spine2"
$bonemerge "ValveBiped.Bip01_Spine4"
$bonemerge "ValveBiped.Bip01_L_Clavicle"
$bonemerge "ValveBiped.Bip01_L_UpperArm"
$bonemerge "ValveBiped.Bip01_L_Forearm"
$bonemerge "ValveBiped.Bip01_L_Hand"
$bonemerge "ValveBiped.Bip01_R_Clavicle"
$bonemerge "ValveBiped.Bip01_R_UpperArm"
$bonemerge "ValveBiped.Bip01_R_Forearm"
$bonemerge "ValveBiped.Bip01_R_Hand"
$bonemerge "ValveBiped.weapon_bone_Clip"


$poseparameter "move_yaw" -180 180 loop 0
$poseparameter "body_pitch" -90 90 loop 0
$poseparameter "body_yaw" -90 90 loop 0

$ikchain "rhand" "ValveBiped.Bip01_R_Hand" knee 0.707 0.707 0
$ikchain "lhand" "ValveBiped.Bip01_L_Hand" knee 0.707 0.707 0
$ikchain "rfoot" "ValveBiped.Bip01_R_Foot" knee 0.707 -0.707 0
$ikchain "lfoot" "ValveBiped.Bip01_L_Foot" knee 0.707 -0.707 0
$ikchain "ikclip" "ValveBiped.weapon_bone_Clip" knee 0.707 -0.707 0

$ikautoplaylock "rfoot" 1 0.1
$ikautoplaylock "lfoot" 1 0.1

$sequence "ragdoll" {
	"mafia_anims\ragdoll.smd"
	activity "ACT_DIERAGDOLL" 1
	fadein 0.2
	fadeout 0.2
	fps 30
	ikrule "rhand" release contact 0 fakeorigin 0 0 0 fakerotate 0 0 0 floor 0 height 0 radius 0 range 0 0 20 20 target 0
	ikrule "lhand" release contact 0 fakeorigin 0 0 0 fakerotate 0 0 0 floor 0 height 0 radius 0 range 0 0 20 20 target 1
	ikrule "rfoot" release contact 0 fakeorigin 0 0 0 fakerotate 0 0 0 floor 0 height 0 radius 0 range 0 0 20 20 target 2
	ikrule "lfoot" release contact 0 fakeorigin 0 0 0 fakerotate 0 0 0 floor 0 height 0 radius 0 range 0 0 20 20 target 3
	ikrule "ikclip" release contact 0 fakeorigin 0 0 0 fakerotate 0 0 0 floor 0 height 0 radius 0 range 0 0 20 20 target 4
}

$includemodel "player/cs_player_shared.mdl"
$includemodel "player/treason_player_shared.mdl"

$collisionjoints "mafia_physics.smd"
{
	$mass 202.0224
	$inertia 10
	$damping 0.01
	$rotdamping 1.5
	$rootbone " "

	$jointrotdamping "ValveBiped.Bip01_Pelvis" 3

	$jointmassbias "ValveBiped.Bip01_Spine1" 8
	$jointconstrain "ValveBiped.Bip01_Spine1" x limit -10 10 0
	$jointconstrain "ValveBiped.Bip01_Spine1" y limit -16 16 0
	$jointconstrain "ValveBiped.Bip01_Spine1" z limit -20 30 0

	$jointmassbias "ValveBiped.Bip01_Spine2" 9
	$jointconstrain "ValveBiped.Bip01_Spine2" x limit -10 10 0
	$jointconstrain "ValveBiped.Bip01_Spine2" y limit -10 10 0
	$jointconstrain "ValveBiped.Bip01_Spine2" z limit -20 20 0

	$jointmassbias "ValveBiped.Bip01_R_Clavicle" 4
	$jointrotdamping "ValveBiped.Bip01_R_Clavicle" 6
	$jointconstrain "ValveBiped.Bip01_R_Clavicle" x limit -15 15 0
	$jointconstrain "ValveBiped.Bip01_R_Clavicle" y limit -10 10 0
	$jointconstrain "ValveBiped.Bip01_R_Clavicle" z limit 0 45 0

	$jointmassbias "ValveBiped.Bip01_L_Clavicle" 4
	$jointrotdamping "ValveBiped.Bip01_L_Clavicle" 6
	$jointconstrain "ValveBiped.Bip01_L_Clavicle" x limit -15 15 0
	$jointconstrain "ValveBiped.Bip01_L_Clavicle" y limit -10 10 0
	$jointconstrain "ValveBiped.Bip01_L_Clavicle" z limit 0 45 0

	$jointmassbias "ValveBiped.Bip01_L_UpperArm" 5
	$jointrotdamping "ValveBiped.Bip01_L_UpperArm" 2
	$jointconstrain "ValveBiped.Bip01_L_UpperArm" x limit -15 20 0
	$jointconstrain "ValveBiped.Bip01_L_UpperArm" y limit -40 32 0
	$jointconstrain "ValveBiped.Bip01_L_UpperArm" z limit -80 25 0

	$jointmassbias "ValveBiped.Bip01_L_Forearm" 4
	$jointrotdamping "ValveBiped.Bip01_L_Forearm" 4
	$jointconstrain "ValveBiped.Bip01_L_Forearm" x limit -40 15 0
	$jointconstrain "ValveBiped.Bip01_L_Forearm" y limit 0 0 0
	$jointconstrain "ValveBiped.Bip01_L_Forearm" z limit -120 10 0

	$jointrotdamping "ValveBiped.Bip01_L_Hand" 1
	$jointconstrain "ValveBiped.Bip01_L_Hand" x limit -25 25 0
	$jointconstrain "ValveBiped.Bip01_L_Hand" y limit -35 35 0
	$jointconstrain "ValveBiped.Bip01_L_Hand" z limit -50 50 0

	$jointmassbias "ValveBiped.Bip01_R_UpperArm" 5
	$jointrotdamping "ValveBiped.Bip01_R_UpperArm" 2
	$jointconstrain "ValveBiped.Bip01_R_UpperArm" x limit -15 20 0
	$jointconstrain "ValveBiped.Bip01_R_UpperArm" y limit -40 32 0
	$jointconstrain "ValveBiped.Bip01_R_UpperArm" z limit -80 25 0

	$jointmassbias "ValveBiped.Bip01_R_Forearm" 4
	$jointrotdamping "ValveBiped.Bip01_R_Forearm" 4
	$jointconstrain "ValveBiped.Bip01_R_Forearm" x limit -40 15 0
	$jointconstrain "ValveBiped.Bip01_R_Forearm" y limit 0 0 0
	$jointconstrain "ValveBiped.Bip01_R_Forearm" z limit -120 10 0

	$jointrotdamping "ValveBiped.Bip01_R_Hand" 1
	$jointconstrain "ValveBiped.Bip01_R_Hand" x limit -25 25 0
	$jointconstrain "ValveBiped.Bip01_R_Hand" y limit -35 35 0
	$jointconstrain "ValveBiped.Bip01_R_Hand" z limit -50 50 0

	$jointmassbias "ValveBiped.Bip01_R_Thigh" 7
	$jointrotdamping "ValveBiped.Bip01_R_Thigh" 7
	$jointconstrain "ValveBiped.Bip01_R_Thigh" x limit -25 25 0
	$jointconstrain "ValveBiped.Bip01_R_Thigh" y limit -10 15 0
	$jointconstrain "ValveBiped.Bip01_R_Thigh" z limit -55 25 0

	$jointmassbias "ValveBiped.Bip01_R_Calf" 4
	$jointconstrain "ValveBiped.Bip01_R_Calf" x limit -10 25 0
	$jointconstrain "ValveBiped.Bip01_R_Calf" y limit -5 5 0
	$jointconstrain "ValveBiped.Bip01_R_Calf" z limit -10 115 0

	$jointmassbias "ValveBiped.Bip01_L_Thigh" 7
	$jointrotdamping "ValveBiped.Bip01_L_Thigh" 7
	$jointconstrain "ValveBiped.Bip01_L_Thigh" x limit -25 25 0
	$jointconstrain "ValveBiped.Bip01_L_Thigh" y limit -10 15 0
	$jointconstrain "ValveBiped.Bip01_L_Thigh" z limit -55 25 0

	$jointmassbias "ValveBiped.Bip01_L_Calf" 4
	$jointconstrain "ValveBiped.Bip01_L_Calf" x limit -10 25 0
	$jointconstrain "ValveBiped.Bip01_L_Calf" y limit -5 5 0
	$jointconstrain "ValveBiped.Bip01_L_Calf" z limit -10 115 0

	$jointmassbias "ValveBiped.Bip01_Head1" 4
	$jointrotdamping "ValveBiped.Bip01_Head1" 3
	$jointconstrain "ValveBiped.Bip01_Head1" x limit -50 50 0
	$jointconstrain "ValveBiped.Bip01_Head1" y limit -20 20 0
	$jointconstrain "ValveBiped.Bip01_Head1" z limit -26 30 0

	$jointcollide "ValveBiped.Bip01_Head1" "ValveBiped.Bip01_L_Clavicle"
}
