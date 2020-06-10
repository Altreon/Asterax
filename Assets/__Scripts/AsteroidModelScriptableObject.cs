using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroidModel", menuName = "ScriptableObjects/AsteroidModel", order = 1)]
public class AsteroidModelScriptableObject : ScriptableObject
{
    public GameObject[] asteroidsObject;
	public GameObject[] asteroidsExplosionParticles;

	public int[] ptForSize;
	public int nbAsteroids;
	public int nbChilds;
}