using UnityEngine;
using System.Collections;

public class ShootingEnemyController : EnemyController {

	protected float fireRate = 2f;
	private float nextTimeToFire;

	protected override void Update(){
		base.Update();
		if ((nextTimeToFire -= Time.deltaTime) <= 0){
			if(playerDetected)
				FireTimedWeapon();
		}
	}

	private void FireTimedWeapon(){
		anim.SetTrigger("Fire Weapon");
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
		Pause(state.length * state.normalizedTime);
		FireWeapon(GameResources.GetGameObject(GameResources.KeyGreenSmallLaser), 10);
		nextTimeToFire = fireRate;
	}
}
