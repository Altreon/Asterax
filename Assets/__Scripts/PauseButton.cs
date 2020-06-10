using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

	[SerializeField]
	Sprite spritePause;
	[SerializeField]
	Sprite spriteResume;

	[SerializeField]
	Color colorPause;
	[SerializeField]
	Color colorResume;

	Image image;

	void Start() {
		image = GetComponent<Image> ();
	}

	public void Click () {
		if (GameManager.Instance.isPaused) {
			Resume ();
		} else {
			Pause ();
		}
	}
		

	void Pause () {
		Time.timeScale = 0;

		image.sprite = spriteResume;
		image.color = colorResume;

		GameManager.Instance.isPaused = true;

		PausePanel.Instance.gameObject.SetActive (true);
	}

	void Resume () {
		Time.timeScale = 1;

		image.sprite = spritePause;
		image.color = colorPause;

		GameManager.Instance.isPaused = false;

		PausePanel.Instance.gameObject.SetActive (false);
	}
}
