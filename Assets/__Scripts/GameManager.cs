using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System;

public enum GameState {
	MainMenu,
	LevelLoad,
	Playing,
	GameOver
}

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState>{}

public class GameManager : MonoBehaviour {
	static GameManager instance = null;
	GameState gameState = GameState.MainMenu;

	[SerializeField]
	string levelProgressionString;
	string[] levelProgression;
	int level = 0;

	[SerializeField]
	int nbJump;
	[SerializeField]
	int score;
	int highScore = 0;
	[SerializeField]
	float timeToResetGame;
	[SerializeField]
	PlayerShip playerShip;

	[SerializeField]
	AsteroidModelScriptableObject asteroidsModel;

	[SerializeField]
	UnityEvent JumpUpdatedEvent;
	[SerializeField]
	UnityEvent ScoreUpdatedEvent;
	[SerializeField]
	GameStateEvent GameStatEvent;
	[SerializeField]
	UnityEvent highScoreEvent;

	public bool isPaused;

	LevelManager levelManager;

	public static GameManager Instance {
		get { 
			return instance;
		}
	}

	void Awake()
	{
		GameManager otherInstance = FindObjectOfType<GameManager> ();
		if (otherInstance != this) 
		{
			Destroy (gameObject);
			return;
		}

		instance = this;
		//DontDestroyOnLoad (gameObject);
		ParseLevelProgression();

		SaveGameManager.LoadGame ();
	}

	void ParseLevelProgression () {
		levelProgression = levelProgressionString.Split (',');
	}

	public void StartGame () {
		levelManager = new LevelManager ();
		JumpUpdatedEvent.AddListener (levelManager.AsteroidDestroy);
		ScoreUpdatedEvent.AddListener (levelManager.AsteroidDestroy);


		level = -1;
		PrepareNextLevel ();
	}

	public void PrepareNextLevel () {
		++level;

		if (level >= levelProgression.Length) {
			GameOver ();
			return;
		}

		CurrentGameState = GameState.LevelLoad;

		levelManager.PrepareLevel (levelProgression[level]);
	}

	public void StartNextLevel () {
		CurrentGameState = GameState.Playing;

		levelManager.GenerateLevel ();

		playerShip.AppearOnSafeLocation ();

		AnalyticsEvent.LevelStart(level + 1, new Dictionary<string, object>
			{
				{"time", DateTime.Now}
			});
	}

	void GameOver () {
		Destroy (playerShip.gameObject);

		CurrentGameState = GameState.GameOver;

		AnalyticsEvent.GameOver(null, new Dictionary<string, object>
			{
				{"time", DateTime.Now},
				{"score", Score},
				{"level", level + 1},
				{"gotHighScore", AchievementManager.Instance.IsHighScoreReach}
			});

		StartCoroutine (WaitForResetGame());
	}

	IEnumerator WaitForJump(){
		playerShip.gameObject.SetActive (false);
		yield return new WaitForSeconds (playerShip.timeToJump);
		playerShip.AppearOnSafeLocation ();
	}

	IEnumerator WaitForResetGame(){
		yield return new WaitForSeconds (timeToResetGame);

		SaveGameManager.SaveGame ();

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void DeleteSave() {
		SaveGameManager.EraseSaveData ();
	}

	void OnApplicationQuit() {
		SaveGameManager.SaveGame ();
	}

	public GameState CurrentGameState {
		get {
			return gameState;
		}
		set {
			gameState = value;
			GameStatEvent.Invoke (value);
		}
	}
	public int Level {
		get {
			return level;
		}
	}


	public int NbJump {
		get {
			return nbJump;
		}
		set {
			if (gameState == GameState.GameOver) {
				return;
			}

			if (value < 0) {
				GameOver ();
				value = 0;
			}else if (value < nbJump) {
				StartCoroutine (WaitForJump ());
			}
			nbJump = value;

			JumpUpdatedEvent.Invoke ();
		}
	}

	public int Score {
		get {
			return score;
		}
		set {
			if (gameState == GameState.GameOver) {
				return;
			}

			score = value;

			ScoreUpdatedEvent.Invoke ();

			if (score > HighScore) {
				HighScore = score;
				highScoreEvent.Invoke ();
			}
		}
	}

	public int HighScore {
		get {
			return highScore;
		}
		set {
			highScore = value;
		}
	}

	public AsteroidModelScriptableObject AsteroidsModel {
		get {
			return asteroidsModel;
		}
	}

	public LevelManager LevelManager {
		get {
			return levelManager;
		}
	}

}
