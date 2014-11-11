using UnityEngine;
using System.Collections;

public class ShootingEnemyController : EnemyController {

	protected float fireRate = 2f;
	private float nextTimeToFire;

	protected override void Start(){
		base.Start();
	}

	protected override void Update(){
		base.Update();
		if ((nextTimeToFire -= Time.deltaTime) <= 0){
			if(playerDetected)
				FireTimedWeapon();
		}
	}

	private void FireTimedWeapon(){
		anim.SetTrigger("Fire Weapon");
		nextTimeToFire = fireRate;
	}
	private void CreateLaser(){
		FireWeapon(GameResources.GetGameObject(GameResources.KeyGreenSmallLaser), 10);

	}
	
}
