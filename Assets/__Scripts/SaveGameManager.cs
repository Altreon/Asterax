using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGameManager {
	
	public static void SaveGame() {
		Debug.Log("Application ending after " + Time.time + " seconds");

		SaveData saveData = new SaveData ();

		saveData.highScore = GameManager.Instance.HighScore;

		saveData.nbAsteroidsHit = AchievementManager.Instance.NbAsteroidsHit;
		saveData.nbLuckyShot = AchievementManager.Instance.NbLuckyShot;
		saveData.nbBulletFired = AchievementManager.Instance.NbBulletFired;

		Achievement[] achievementList = AchievementManager.Instance.AchievementList;
		saveData.achievementCompletion = new bool[achievementList.Length];
		for (int i = 0; i < achievementList.Length; ++i) {
			saveData.achievementCompletion [i] = achievementList [i].complete;
		}

		saveData.turretModelSelected = PausePanel.Instance.LastTurretId;
		saveData.bodyModelSelected = PausePanel.Instance.LastBodyId;

		string saveDataJSON = JsonUtility.ToJson(saveData);
		System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", saveDataJSON);
	}

	public static void LoadGame() {
		Debug.Log("Application data loading...");

		if (!System.IO.File.Exists (Application.persistentDataPath + "/SaveData.json")) {
			Debug.Log("Application data failed, the save data file is missing");
			return;
		}

		string saveDataJSON = System.IO.File.ReadAllText (Application.persistentDataPath + "/SaveData.json");

		SaveData saveData = JsonUtility.FromJson<SaveData> (saveDataJSON);

		GameManager.Instance.HighScore = saveData.highScore;

		for (int i = 0; i < saveData.achievementCompletion.Length; ++i) {
			AchievementManager.Instance.AchievementList[i].complete = saveData.achievementCompletion [i];
			if (saveData.achievementCompletion [i]) {
				PausePanel.Instance.UnlockChoice (AchievementManager.Instance.AchievementList [i].unlockCosmeticType,
												  AchievementManager.Instance.AchievementList [i].unlockCosmeticId);
			}
		}

		AchievementManager.Instance.NbAsteroidsHit = saveData.nbAsteroidsHit;
		AchievementManager.Instance.NbLuckyShot = saveData.nbLuckyShot;
		AchievementManager.Instance.NbBulletFired = saveData.nbBulletFired;

		PausePanel.Instance.ButtonClick (CosmeticType.Turret, saveData.turretModelSelected);
		PausePanel.Instance.ButtonClick (CosmeticType.Body, saveData.bodyModelSelected);

		Debug.Log("Application data loaded");
	}

	public static void EraseSaveData() {
		GameManager.Instance.HighScore = 0;

		AchievementManager.Instance.NbAsteroidsHit = 0;
		AchievementManager.Instance.NbLuckyShot = 0;
		AchievementManager.Instance.NbBulletFired = 0;

		for (int i = 0; i < AchievementManager.Instance.AchievementList.Length; ++i) {
			AchievementManager.Instance.AchievementList[i].complete = false;
			PausePanel.Instance.LockChoice(AchievementManager.Instance.AchievementList [i].unlockCosmeticType,
										   AchievementManager.Instance.AchievementList [i].unlockCosmeticId);
		}

		PausePanel.Instance.ButtonClick (CosmeticType.Turret, 0);
		PausePanel.Instance.ButtonClick (CosmeticType.Body, 0);

		System.IO.File.Delete(Application.persistentDataPath + "/SaveData.json");

		Debug.Log("Application data erased");
	}
}

[System.Serializable]
class SaveData {
	public int highScore;

	public int nbAsteroidsHit;
	public int nbLuckyShot;
	public int nbBulletFired;

	public bool[] achievementCompletion;

	public int turretModelSelected;
	public int bodyModelSelected;
}
