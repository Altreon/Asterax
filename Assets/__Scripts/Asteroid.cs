using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;
	public float minRotateSpeed;
	public float maxRotateSpeed;
	public GameObject explosionParticles;

	float speed;
	float rotateSpeed;
	Vector3 direction;
	Vector3 rotateDirection;

	int point;

	void Start () {
		speed = Random.Range (minSpeed, maxSpeed);
		rotateSpeed = Random.Range (minRotateSpeed, maxRotateSpeed);

		Vector3 direction2D = Random.insideUnitCircle.normalized;
		Vector3 rotateDirection2D = Random.insideUnitCircle.normalized;

		direction = new Vector3 (direction2D.x, direction2D.y, 0);
		rotateDirection = new Vector3 (rotateDirection2D.x, rotateDirection2D.y, 0);
	}

	void Update () {
		transform.position += direction * speed / transform.lossyScale.x * Time.deltaTime;
		transform.rotation *= Quaternion.AngleAxis (rotateSpeed / transform.lossyScale.x * Time.deltaTime, rotateDirection);

		DestroyIfTooFar ();
	}

	//Par sécurité
	void DestroyIfTooFar () {
		if (Mathf.Abs (transform.position.x) >= 30 || Mathf.Abs (transform.position.y) >= 20) {
			Destruct ();
			GameManager.Instance.Score += Point;
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag != "Bullet" || !collider.GetComponent<Bullet> ().enabled) {
			return;
		}

		if (!enabled && transform.parent) {
			transform.parent.GetComponent<Asteroid> ().OnTriggerEnter(collider);
			return;
		}

		Bullet bullet = collider.GetComponent<Bullet> ();
		if (bullet.IsLucky) {
			++AchievementManager.Instance.NbLuckyShot;
		}

		bullet.enabled = false;
		Destroy (collider.gameObject);

		GameManager.Instance.Score += Point;
		++AchievementManager.Instance.NbAsteroidsHit;

		Destruct ();
	}

	public void Destruct () {
		int nbChilds = transform.childCount;
		while (transform.childCount > 0) {
			Transform child = transform.GetChild(0);
			child.parent = null;
			child.GetComponent<Asteroid> ().enabled = true;
			child.GetComponent<ScreenWrap> ().enabled = true;
			child.GetComponent<MeshCollider> ().enabled = true;
		}

		GameObject explosion = GameObject.Instantiate (explosionParticles, transform.position, Quaternion.identity);
		explosion.transform.localScale = Vector3.one * transform.lossyScale.x;

		Destroy (gameObject);
	}

	public int Point {
		get { 
			return point;
		}
		set {
			point = value;
		}
	}
}
