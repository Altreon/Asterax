using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StepType {
	HitAsteroid,
	LuckyShot,
	BulletFired,
	ScoreAttained,
	LevelUp
}

[System.Serializable]
public class Achievement {

	public string name;
	public string description;
	public StepType stepType;
	public int stepCount;
	public Image unlockablePart;
	public bool complete;

	public CosmeticType unlockCosmeticType;
	public int unlockCosmeticId;

	public delegate bool CheckCompletionDelegate();
	public CheckCompletionDelegate checkCompletion;

	public void assignCheckCompletionDelegate () {
		switch (stepType) {
		case StepType.HitAsteroid:
				checkCompletion = () => AchievementManager.Instance.NbAsteroidsHit >= stepCount;
				break;
			case StepType.LuckyShot:
				checkCompletion = () => AchievementManager.Instance.NbLuckyShot >= stepCount;
				break;
			case StepType.BulletFired:
				checkCompletion = () => AchievementManager.Instance.NbBulletFired >= stepCount;
				break;
			case StepType.ScoreAttained:
				checkCompletion = () => GameManager.Instance.Score >= stepCount;
				break;
			case StepType.LevelUp:
				checkCompletion = () => GameManager.Instance.Level + 1 >= stepCount;
				break;
		}
	}
}
