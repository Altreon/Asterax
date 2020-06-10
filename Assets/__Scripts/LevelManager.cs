using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

	int nbAsteroids;
	int nbChildren;

	public void PrepareLevel (string levelData) {
		string[] data = levelData.Split (':', '/');
		nbAsteroids = int.Parse(data[1]);
		nbChildren = int.Parse(data[2]);
	}

	public void GenerateLevel () {
		for(int i = 0; i < nbAsteroids; ++i){
			Vector3 pos = new Vector3 (Random.Range (-12f, 12f), Random.Range (-7f, 7f), 0f);

			CreateRandomAsteroid (3, pos, null);
		}
	}

	void CreateRandomAsteroid (int size, Vector3 pos, Transform parent) {
		if (size <= 0) {
			return;
		}

		int id = Random.Range (0, GameManager.Instance.AsteroidsModel.asteroidsObject.Length);
		GameObject asteroid = GameObject.Instantiate (GameManager.Instance.AsteroidsModel.asteroidsObject[id], pos, Random.rotation);

		id = Random.Range (0, GameManager.Instance.AsteroidsModel.asteroidsExplosionParticles.Length);
		asteroid.GetComponent<Asteroid> ().explosionParticles = GameManager.Instance.AsteroidsModel.asteroidsExplosionParticles [id];

		asteroid.transform.localScale = Vector3.one * size;

		asteroid.GetComponent<Asteroid> ().Point = GameManager.Instance.AsteroidsModel.ptForSize [size-1];

		if (parent) {
			asteroid.GetComponent<Asteroid> ().enabled = false;
			asteroid.GetComponent<ScreenWrap> ().enabled = false;
			asteroid.GetComponent<MeshCollider> ().enabled = false;
			asteroid.transform.parent = parent;
		}

		for (int i = 0; i < nbChildren; ++i) {
			CreateRandomAsteroid (size - 1, pos, asteroid.transform);
		}
	}

	public void AsteroidDestroy () {
		int nbAsteroidsInScene = GameObject.FindGameObjectsWithTag ("Asteroid").Length - 1;
		if (nbAsteroidsInScene == 0) {
			GameManager.Instance.PrepareNextLevel ();
		}
	}

	public int NbAsteroids {
		get {
			return nbAsteroids;
		}
	}

	public int NbChildren {
		get {
			return nbChildren;
		}
	}
}
