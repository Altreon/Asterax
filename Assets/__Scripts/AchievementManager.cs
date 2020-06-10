using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class AchievementManager : MonoBehaviour {
	static AchievementManager instance = null;

	[SerializeField]
	AchievementScriptableObject achievementScriptableObject;

	[SerializeField]
	AchievementPopPup achievementPopPup;

	Queue<Achievement> achievementToPopPup;

	int nbAsteroidsHit;
	int nbLuckyShot;
	int nbBulletFired;

	bool highScoreReach = false;
	bool highScoreReachShowed = false;

	public static AchievementManager Instance {
		get { 
			return instance;
		}
	}

	void Awake()
	{
		AchievementManager otherInstance = FindObjectOfType<AchievementManager> ();
		if (otherInstance != this) 
		{
			Destroy (gameObject);
			return;
		}

		instance = this;
		//DontDestroyOnLoad (gameObject);

		achievementToPopPup = new Queue<Achievement> ();
	}

	void Update () {
		if (!achievementPopPup.CanShowAchievementPopup ()) {
			return;
		}

		if(highScoreReach && !highScoreReachShowed){
			achievementPopPup.ShowAchievementPopup (achievementScriptableObject.highScoreAchievementName,
													achievementScriptableObject.highScoreAchievementDescription);
			highScoreReachShowed = true;
			return;
		}

		if (achievementToPopPup.Count == 0) {
			return;
		}

		achievementPopPup.ShowAchievementPopup (achievementToPopPup.Dequeue());
	}

	void checkAchievementsCompletion () {
		foreach (Achievement achievement in AchievementList) {
			if (!achievement.complete && achievement.checkCompletion()) {
				achievement.complete = true;

				PausePanel.Instance.UnlockChoice (achievement.unlockCosmeticType, achievement.unlockCosmeticId);

				achievementToPopPup.Enqueue (achievement);

				AnalyticsEvent.AchievementUnlocked(achievement.name, new Dictionary<string, object>
					{
						{"time", DateTime.Now}
					});

				Debug.Log("Achievement " + achievement.name + " is done!");
			}
		}
	}

	public Achievement[] AchievementList {
		get {
			return achievementScriptableObject.achievements;
		}
	}

	public int NbAsteroidsHit {
		get {
			return nbAsteroidsHit;
		}
		set {
			nbAsteroidsHit = value;
			checkAchievementsCompletion ();
		}
	}

	public int NbLuckyShot {
		get {
			return nbLuckyShot;
		}
		set {
			nbLuckyShot = value;
			checkAchievementsCompletion ();
		}
	}

	public int NbBulletFired {
		get {
			return nbBulletFired;
		}
		set {
			nbBulletFired = value;
			checkAchievementsCompletion ();
		}
	}

	public bool IsHighScoreReach {
		get {
			return highScoreReach;
		}
	}

	public void JumpOrScoreChanged () {
		checkAchievementsCompletion ();
	}

	public void GameStateChanged(GameState gameState) {
		if (gameState == GameState.LevelLoad) {
			checkAchievementsCompletion ();
		}
	}

	public void HighScoreReach () {
		highScoreReach = true;
	}
}