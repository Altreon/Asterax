using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
	public GameObject bullet;
	
	public void LookTo (Vector3 pos) {
		transform.LookAt (pos, -Vector3.forward);
	}

	public void Shoot () {
		if (!transform.parent.gameObject.activeSelf) {
			return;
		}

		GameObject newBullet = GameObject.Instantiate (bullet, transform.parent.position, Quaternion.identity);
		Vector3 shootDir = transform.forward;
		shootDir.z = 0;
		newBullet.GetComponent<Bullet> ().Direction = shootDir;

		++AchievementManager.Instance.NbBulletFired;
	}

}
