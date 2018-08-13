using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class MainMenu : Menu
	{

		public static MainMenu _Instance = null;
		[SerializeField] bool _autoStart;
		[SerializeField] AudioSource _audio;
		[SerializeField] float _audioFadeSpeed;
		float _initialVolume;

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
			StartCoroutine (FadeOutAudio ());
			AudioProcessor._instance.audioSource.Stop ();
			AudioProcessor._instance.audioSource.Play ();
			InputManager._instance.SetInputMode (InputMode.Game);
		}

		IEnumerator FadeOutAudio ()
		{
			_initialVolume = _audio.volume;
			while (_audio.volume > 0)
			{
				_audio.volume -= Time.unscaledDeltaTime * _audioFadeSpeed;
				yield return null;
			}
			_audio.Stop ();
			_audio.volume = _initialVolume;
			Debug.Log ("ASD");
		}

		public void OnClickExit ()
		{
			Debug.Log ("Exiting.");
			Application.Quit ();
		}
	}
}