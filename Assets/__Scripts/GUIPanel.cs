using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPanel : MonoBehaviour {
	[SerializeField]
	Text jumpGUIText;
	[SerializeField]
	Text scoreGUIText;

	void Start () {
		UpdateJumpGUI ();
		UpdateScoreGUI ();
	}
	
	public void UpdateJumpGUI () {
		jumpGUIText.text = GameManager.Instance.NbJump + " Jumps";
	}

	public void UpdateScoreGUI () {
		scoreGUIText.text = GameManager.Instance.Score.ToString();
	}
}
