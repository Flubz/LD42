using UnityEngine;

namespace Managers
{
	public class ApplicationManager : MonoBehaviour
	{
		public static ApplicationManager _instance = null;
		[SerializeField] AudioProcessor _audioProcessor;
		[SerializeField] InputManager _inputManager;

		void Awake ()
		{
			if (_instance == null) _instance = this;
			else if (_instance != this) Destroy (gameObject);
			_inputManager.OnInputModeChanged += OnInputModeChange;
		}

		public void OnInputModeChange ()
		{
			if (_inputManager._InputMode != InputMode.Game)
			{
				_audioProcessor.audioSource.Pause ();
			}
			else
			{
				_audioProcessor.audioSource.UnPause ();
			}
		}

		private void OnDestroy ()
		{
			_inputManager.OnInputModeChanged -= OnInputModeChange;
		}
	}
}