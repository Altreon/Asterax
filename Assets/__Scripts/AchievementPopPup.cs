using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopPup : MonoBehaviour {
	[SerializeField]
	RectTransform achievementGUITransform;
	[SerializeField]
	Text achievementGUINameText;
	[SerializeField]
	Text achievementGUIDescriptionText;

	[SerializeField]
	float achievementTimeToShow;
	[SerializeField]
	float achievementTimeStayOnScreen;

	Achievement achievementToShow;
	float beginTime;
	int showStep;

	float screenHeightRatio;

	void Awake () {
		screenHeightRatio = Screen.height/1080f;

		Vector3 curPos = achievementGUITransform.position;
		achievementGUITransform.position = new Vector3(curPos.x, 1140, curPos.z);

		showStep = 4;
	}

	public void ShowAchievementPopup(Achievement achievement) {
		if (!CanShowAchievementPopup ()) {
			return;
		}

		achievementGUINameText.text = achievement.name.ToUpper();

		string description = achievement.description.ToUpper ();
		description = description.Replace ("#", achievement.stepCount.ToString("N0"));
		achievementGUIDescriptionText.text = description;

		showStep = 0;
		beginTime = Time.time;
	}

	public void ShowAchievementPopup(string name, string description) {
		if (!CanShowAchievementPopup ()) {
			return;
		}

		achievementGUINameText.text = name.ToUpper();
		achievementGUIDescriptionText.text = description.ToUpper();

		showStep = 0;
		beginTime = Time.time;
	}

	public bool CanShowAchievementPopup () {
		return showStep == 4;
	}

	void Update () {
		switch (showStep) {
		case 0:
			AppearAchievement ();
			break;
		case 1:
			StartCoroutine (WaitForDiseappearAchievement());
			break;
		case 3:
			DiseappearAchievement ();
			break;
		default :
			break;
		}
	}

	void AppearAchievement () {
		float time = Time.time - beginTime;
		float ratio = time / achievementTimeToShow;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;
			++showStep;
		}

		float posY = 1140 * screenHeightRatio - Mathf.Sin(ratio * Mathf.PI/2) * 130 * screenHeightRatio;

		Vector3 curPos = achievementGUITransform.position;
		achievementGUITransform.position = new Vector3(curPos.x, posY, curPos.z);
	}

	void DiseappearAchievement () {
		float time = Time.time - beginTime;
		float ratio = time / achievementTimeToShow;

		if (ratio > 1) {
			ratio = 1;
			beginTime = Time.time;
			++showStep;
		}

		float posY = 1010 * screenHeightRatio + Mathf.Sin(ratio * Mathf.PI/2) * 130 * screenHeightRatio;

		Vector3 curPos = achievementGUITransform.position;
		achievementGUITransform.position = new Vector3(curPos.x, posY, curPos.z);
	}

	IEnumerator WaitForDiseappearAchievement () {
		++showStep;
		yield return new WaitForSeconds (achievementTimeStayOnScreen);
		beginTime = Time.time;
		++showStep;
	}
}
