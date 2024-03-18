<h1 align="center">Turret AI System in Unity</h1>
<p align="center"><i>A customizable and versatile turret AI system in Unity inclıding bullet physics calculator</i></p>
  
## Aim of Project

This project aims to create turret system in Unity. Turret AI provides:
- Detection of enemies based on types of enemy, tags and range
- Customizable firing system
- Calculating physics based velocity of target object and hit at the exact point
- Turret settings can easily be changed in the Unity inspector
- Easy to setup and use

> [!TIP]
> Project uses State Machine and delegate functions. Therefore, project can easily be edited using these structs.

## How To Use

> [!IMPORTANT]
> Project uses Unity 2022.3.8f1 and Built - in Rendered Pipeline.

> [!TIP]
> If you have pink material error , you can convert project materials to URP/HDRP in Unity Inspector by Edit/Convert Materials.

You can find game ready turret prefabs in Assets\Prefabs\Turrets and drag to your project
Ready turret prefabs:.
- Anti - air turret
- Cannon turret

For targets, you can drag Target.cs script to your object or drag enemy prefabs in Assets\Prefabs\Enemies.

If you have turret and enemy in the scene, you are ready!
> [!CAUTION]
> Turret aims according to velocity of rigidbody of enemy. If your enemy moves using another way, you must edit TurretLockState.cs.
> To edit , find AimCalculator.CalculateAimByObjectVelocity and pass values of transform of target and velocity of target by Vector3 type

