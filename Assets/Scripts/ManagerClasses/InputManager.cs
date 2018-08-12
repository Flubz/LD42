using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager _instance = null;

	void Awake ()
	{
		if (_instance == null) _instance = this;
		else if (_instance != this) Destroy (gameObject);
		_InputMode = _initialInputMode;
	}

	public InputMode _InputMode { get; private set; }
	public Action OnInputModeChanged;

	public void SetInputMode (InputMode mode_)
	{
		_InputMode = mode_;
		if (OnInputModeChanged != null) OnInputModeChanged.Invoke ();
	}
	
	public InputMode _initialInputMode = InputMode.MainMenu;

}

public enum InputMode
{
	Undefined,
	MainMenu,
	Settings,
	Pause,
	Game,
	Loading
}