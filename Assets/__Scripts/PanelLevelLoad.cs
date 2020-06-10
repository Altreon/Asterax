using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelLoad : MonoBehaviour {
	[SerializeField]
	Image LevelLoadPanel;

	[SerializeField]
	RectTransform LevelLoadTransform;

	[SerializeField]
	Text nbAsteroidsText;
	[SerializeField]
	Text nbChildrenText;

	[SerializeField]
	float timeToShowPanel;
	[SerializeField]
	float timeToShowLevelLoad;
	[SerializeField]
	float timeToShowLevelInfo;
	[SerializeField]
	float timeToDisappear;

	int appearStep;
	float beginTime;

	void OnEnable () {
		Color c = new Color (255, 255, 255, 0);
		LevelLoadPanel.color = c;

		LevelLoadTransform.rotation = Quaternion.identity;
		LevelLoadTransform.localScale = new Vector3 (1, 0, 1);

		nbAsteroidsText.color = c;
		nbChildrenText.color = c;

		appearStep = 0;
		beginTime = Time.time;
	}

	void Update () {
		switch (appearStep) {
		case 0:
			AppearPanel ();
			break;
		case 1:
			AppearLevelLoad ();
			break;
		case 2:
			AppearLevelData ();
			break;
		case 3:
			Disappear ();
			break;
		default :
			break;
		}
	}

	void AppearPanel () {
		float time = Time.time - beginTime;
		float ratio = time / timeToShowPanel;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;

			LevelLoadTransform.GetComponent<Text>().text = "Level " + (GameManager.Instance.Level + 1);

			++appearStep;
		}

		Color c = new Color (0, 0, 0, ratio);

		LevelLoadPanel.color = c;
	}

	void AppearLevelLoad () {
		float time = Time.time - beginTime;
		float ratio = time / timeToShowLevelLoad;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;

			nbAsteroidsText.text = "Asteroids : " + GameManager.Instance.LevelManager.NbAsteroids;
			nbChildrenText.text = "Children : " + GameManager.Instance.LevelManager.NbChildren;

			++appearStep;
		}

		float size = Mathf.Sin ((ratio / 1) * (5 * Mathf.PI/6)) * 2;

		LevelLoadTransform.localScale = new Vector3 (1, size, 1);
	}

	void AppearLevelData () {
		float time = Time.time - beginTime;
		float ratio = time / timeToShowLevelInfo;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;
			++appearStep;
		}

		Color c = new Color (255, 255, 255, ratio);

		nbAsteroidsText.color = c;
		nbChildrenText.color = c;
	}

	void Disappear () {
		float time = Time.time - beginTime;
		float ratio = time / timeToDisappear;

		if (ratio > 1) {
			ratio = 1;
			++appearStep;
			GameManager.Instance.StartNextLevel ();
		}

		float size = Mathf.Sin ((1 - ratio / 1) * (5 * Mathf.PI/6)) * 2;

		LevelLoadTransform.localScale = new Vector3 (1, size, 1);

		Color c = new Color (255, 255, 255, 1 - ratio);

		nbAsteroidsText.color = c;
		nbChildrenText.color = c;
	}
}
