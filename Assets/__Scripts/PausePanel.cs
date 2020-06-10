using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.Analytics;

[System.Serializable]
public class CosmeticChoiceEvent : UnityEvent<int>{}

public enum CosmeticType {
	Turret,
	Body
}

public class PausePanel : MonoBehaviour {
	static PausePanel instance = null;

	[SerializeField]
	ChoiceButton[] choiceButtonTurrets;
	[SerializeField]
	ChoiceButton[] choiceButtonBodies;

	[SerializeField]
	CosmeticChoiceEvent cosmeticTurretChoiceEvent;
	[SerializeField]
	CosmeticChoiceEvent cosmeticBodyChoiceEvent;

	int lastTurretId = 0;
	int lastBodyId = 0;

	public static PausePanel Instance {
		get { 
			return instance;
		}
	}

	void Awake()
	{
		PausePanel otherInstance = FindObjectOfType<PausePanel> ();
		if (otherInstance != this) 
		{
			Debug.Log (otherInstance + " : " + gameObject);
			Destroy (gameObject);
			return;
		}

		instance = this;
		//DontDestroyOnLoad (gameObject);

		gameObject.SetActive (false);
	}

	public void LockChoice (CosmeticType cosmeticType, int id) {
		switch (cosmeticType) {
			case CosmeticType.Turret:
				LockTurret (id);
				break;
			case CosmeticType.Body:
				LockBody (id);
				break;
			default:
				break;
			}
	}

	void LockTurret (int id) {
		choiceButtonTurrets [id].ClickButton.interactable = false;
	}

	void LockBody (int id) {
		choiceButtonBodies [id].ClickButton.interactable = false;
	}

	public void UnlockChoice (CosmeticType cosmeticType, int id) {
		switch (cosmeticType) {
		case CosmeticType.Turret:
			UnlockTurret (id);
			break;
		case CosmeticType.Body:
			UnlockBody (id);
			break;
		default:
			break;
		}
	}

	void UnlockTurret (int id) {
		choiceButtonTurrets [id].ClickButton.interactable = true;
	}

	void UnlockBody (int id) {
		choiceButtonBodies [id].ClickButton.interactable = true;
	}

	public void ButtonClick (CosmeticType cosmeticType, int id) {
		switch (cosmeticType) {
			case CosmeticType.Turret:
				ChoiceTurret (id);
				break;
			case CosmeticType.Body:
				ChoiceBody (id);
				break;
			default:
				break;
		}

		AnalyticsEvent.Custom("ShipPartChoice", new Dictionary<string, object>
			{
				{"time", DateTime.Now},
				{"turret", LastTurretId},
				{"body", LastBodyId}
			});
	}

	public void ChoiceTurret (int id) {
		if (id == lastTurretId || id >= choiceButtonTurrets.Length) {
			return;
		}

		choiceButtonTurrets [lastTurretId].CheckMark.enabled = false;
		choiceButtonTurrets [id].CheckMark.enabled = true;

		cosmeticTurretChoiceEvent.Invoke (id);

		lastTurretId = id;
	}

	public void ChoiceBody (int id) {
		if (id == lastBodyId || id >= choiceButtonBodies.Length) {
			return;
		}

		choiceButtonBodies [lastBodyId].CheckMark.enabled = false;
		choiceButtonBodies [id].CheckMark.enabled = true;

		cosmeticBodyChoiceEvent.Invoke (id);

		lastBodyId = id;
	}

	public int LastTurretId {
		get {
			return lastTurretId;
		}
	}

	public int LastBodyId {
		get {
			return lastBodyId;
		}
	}
}

[System.Serializable]
class ChoiceButton {
	[SerializeField]
	Button clickButton;
	[SerializeField]
	RawImage checkMark;

	public Button ClickButton {
		get {
			return clickButton;
		}
	}

	public RawImage CheckMark {
		get {
			return checkMark;
		}
	}
}