using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed;
	public float destroySeconds;

	Vector3 direction;

	bool isLucky;

	void Start () {
		StartCoroutine(WaitForDestroy());
	}
	
	void Update () {
		transform.position += direction * speed * Time.deltaTime;
	}

	IEnumerator WaitForDestroy() {
		yield return new WaitForSeconds(destroySeconds);
		Destroy (gameObject);
	}

	public Vector3 Direction {
		set { 
			direction = value;
		}
	}

	public bool IsLucky {
		get { 
			return isLucky;
		}
	}

	public void Wrap () {
		isLucky = true;
	}
}
