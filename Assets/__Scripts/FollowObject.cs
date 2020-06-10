using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
	[SerializeField]
	Transform follow;
	[SerializeField]
	float zPos;
	
	void Update () {
		transform.position = new Vector3(follow.position.x, follow.position.y, zPos);
	}

	public void End () {
		Destroy (gameObject);
	}
}
