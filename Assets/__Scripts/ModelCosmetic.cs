using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCosmetic : MonoBehaviour {
	[SerializeField]
	ShipPartsScriptableObject shipPartsData;
	[SerializeField]
	GameObject model;

	public void SwitchModelCosmetic (int id) {
		if (id >= shipPartsData.partInfos.Length) {
			return;
		}

		GameObject newModel = GameObject.Instantiate(shipPartsData.partInfos [id].prefab, model.transform.position, model.transform.rotation, model.transform.parent);

		Destroy (model);

		model = newModel;
	}
}
