using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class SettingsMenu : Menu
	{
		public static SettingsMenu _instance = null;

		void Awake ()
		{
			if (_instance == null) _instance = this;
		}

		public void OnClickRestart ()
		{
			// SceneTransition._instance.LoadScene (SceneManager.GetActiveScene ().name);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			CloseMenu ();
		}

		private void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Escape) && InputManager._instance._InputMode == InputMode.Game || InputManager._instance._InputMode == InputMode.Settings)
			{
				ToggleMenu ();
			}
		}

		public void OnClickResume ()
		{
			CloseMenu ();
		}

		public void OnClickExit ()
		{
			Debug.Log ("Exiting.");
			Application.Quit ();
		}
	}
}