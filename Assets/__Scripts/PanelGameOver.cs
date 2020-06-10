using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameOver : MonoBehaviour {
	[SerializeField]
	Image GameOverPanel;

	[SerializeField]
	RectTransform GameOverTransform;

	[SerializeField]
	Text finalLevelText;
	[SerializeField]
	Text finalScoreText;

	[SerializeField]
	float timeToShowPanel;
	[SerializeField]
	float timeToShowGameOver;
	[SerializeField]
	float timeToShowGameScore;

	int appearStep;
	float beginTime;

	public void Start () {
		Color c = new Color (255, 255, 255, 0);
		GameOverPanel.color = c;

		GameOverTransform.rotation = Quaternion.identity;
		GameOverTransform.localScale = new Vector3 (1, 0, 1);

		finalLevelText.color = c;
		finalScoreText.color = c;

		appearStep = 0;
		beginTime = Time.time;

		finalLevelText.text = "Final Level : " + (GameManager.Instance.Level + 1);
		finalScoreText.text = "Final Score : " + GameManager.Instance.Score;
	}

	void Update () {
		switch (appearStep) {
		case 0:
			AppearPanel ();
			break;
		case 1:
			AppearGameOver ();
			break;
		case 2:
			AppearGameScore ();
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
			++appearStep;
		}

		Color c = new Color (0, 0, 0, ratio);

		GameOverPanel.color = c;
	}

	void AppearGameOver () {
		float time = Time.time - beginTime;
		float ratio = time / timeToShowGameOver;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;
			++appearStep;
		}

		GameOverTransform.rotation = Quaternion.identity * Quaternion.AngleAxis (360 * ratio, Vector3.left);
		GameOverTransform.localScale = new Vector3 (1, ratio, 1);
	}

	void AppearGameScore () {
		float time = Time.time - beginTime;
		float ratio = time / timeToShowGameScore;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;
			++appearStep;
		}

		Color c = new Color (255, 255, 255, ratio);

		finalLevelText.color = c;
		finalScoreText.color = c;
	}

	public void HighScoreReach () {
		GameOverTransform.GetComponent<Text> ().text = "High Score !";
	}
}
