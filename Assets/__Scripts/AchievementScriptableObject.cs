using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievementList", menuName = "ScriptableObjects/AchievementList", order = 2)]
public class AchievementScriptableObject : ScriptableObject {
	public Achievement[] achievements;

	public string highScoreAchievementName;
	public string highScoreAchievementDescription;

	void OnEnable () {
		foreach (Achievement achievement in achievements) {
			achievement.complete = false;
			achievement.assignCheckCompletionDelegate ();
		}
	}
}
