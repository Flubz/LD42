using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class MainMenu : Menu
	{

		public static MainMenu _Instance = null;
		[SerializeField] ParticleSystem _menuEffect;
		[SerializeField] bool _autoStart;

		void Awake ()
		{
			if (_Instance == null) _Instance = this;
			OpenMenu ();
		}

		private void Start ()
		{
			if (_autoStart) OnClickStartGame ();
		}

		// private void Update ()
		// {
		// 	if (InputManager._instance._InputMode == InputMode.MainMenu && Input.GetButtonDown ("Fire1"))
		// 	{
		// 		OnClickStartGame ();
		// 	}
		// }

		public void OnClickStartGame ()
		{
			CloseMenu ();
			InputManager._instance.SetInputMode (InputMode.Game);
			_menuEffect.Stop ();
		}

		public void OnClickExit ()
		{
			Debug.Log ("Exiting.");
			Application.Quit ();
		}

		protected override void OnMenuOpened ()
		{
			base.OnMenuOpened ();
			_menuEffect.Play ();
		}
	}
}