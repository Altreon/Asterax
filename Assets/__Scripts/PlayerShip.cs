using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
	public float maxSpeed;
	public float angleMaxTilt;

	public float timeToJump;
	public float sizeLimitOfSafeZone;

	public ParticleSystem AppearParticles;
	public ParticleSystem DiseappearParticles;
	public ParticleSystem TrailParticles;

	Vector2 velocity;

	bool canMove = false;

	public void MoveHorizontal (float amount) {
		if (!canMove) {
			return;
		}

		transform.position += -Vector3.left * amount * Time.deltaTime * maxSpeed;

		velocity.x = amount;
	}

	public void MoveVertical (float amount) {
		if (!canMove) {
			return;
		}

		transform.position += Vector3.up * amount * Time.deltaTime * maxSpeed;

		velocity.y = amount;
	}

	public void LateUpdate () {
		if (!canMove) {
			return;
		}

		if(TrailParticles.isStopped){
			TrailParticles.Play ();
		}

		transform.rotation = Quaternion.AngleAxis (velocity.x * angleMaxTilt, -Vector3.up);
		transform.rotation *= Quaternion.AngleAxis (velocity.y * angleMaxTilt, -Vector3.left);
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag != "Asteroid") {
			return;
		}

		Asteroid asteroid = collider.GetComponent<Asteroid> ();
		while (!asteroid.enabled && asteroid.transform.parent) {
			asteroid = asteroid.transform.parent.GetComponent<Asteroid> ();
		}

		asteroid.Destruct ();

		GameManager.Instance.NbJump -= 1;

		DiseappearParticles.gameObject.SetActive (true);

		canMove = false;
	}

	public void AppearOnSafeLocation () {
		Vector3 pos = new Vector3 (Random.Range (-12f, 12f), Random.Range (-7f, 7f), 0f);

		TrailParticles.Stop ();

		if (Physics.OverlapSphere (pos, sizeLimitOfSafeZone, LayerMask.GetMask("Asteroid")).Length > 0) {
			AppearOnSafeLocation ();
			return;
		}
			
		transform.position = pos;

		gameObject.SetActive (true);

		AppearParticles.gameObject.SetActive (true);

		canMove = true;
	}
}
