using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour
{
	public HighscoreField[] highscoreFields;
	[SerializeField] Highscores highscoresManager;
	[SerializeField] CanvasFader _canvasFader;

	[SerializeField] Text _scoreField;
	[SerializeField] Text _nameField;
	[SerializeField] Button _addButton;

	public static DisplayHighscores _instance = null;

	void Awake ()
	{
		if (_instance == null) _instance = this;
	}

	void Start ()
	{
		for (int i = 0; i < highscoreFields.Length; i++)
		{
			highscoreFields[i].name.text = i + 1 + ". Fetching...";
		}

		StartCoroutine ("RefreshHighscores");
	}

	public void OnHighscoresDownloaded (Highscore[] highscoreList)
	{
		for (int i = 0; i < highscoreFields.Length; i++)
		{
			highscoreFields[i].name.text = i + 1 + ". ";
			if (i < highscoreList.Length)
			{
				highscoreFields[i].name.text += highscoreList[i].username.ToUpper ();
				highscoreFields[i].score.text = highscoreList[i].score.ToString ();
			}
		}
	}

	public void GameOver ()
	{
		_canvasFader.FadeCanvasIn ();
		_addButton.interactable = true;
		_scoreField.text = ScoreManager._instance._CurrentScore.ToString ();
	}

	public void OnClickRestart ()
	{
		SettingsMenu._instance.OnClickRestart ();
	}

	public void OnClickAdd ()
	{
		if (!String.IsNullOrEmpty (_nameField.text.ToString ()))
		{
			Highscores.AddNewHighscore (_nameField.text.ToString ().ToUpper (), Int32.Parse (_scoreField.text.ToString ()));
		}
		_addButton.interactable = false;
	}

	[System.Serializable]
	public struct HighscoreField
	{
		public Text name;
		public Text score;
	}

	IEnumerator RefreshHighscores ()
	{
		while (true)
		{
			highscoresManager.DownloadHighscores ();
			yield return new WaitForSeconds (30);
		}
	}
}