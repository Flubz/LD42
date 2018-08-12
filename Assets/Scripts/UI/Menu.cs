using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
	[RequireComponent (typeof (CanvasFader))]
	public class Menu : MonoBehaviour
	{
		[SerializeField] protected CanvasFader _canvasFader;
		[SerializeField] bool _disableCanvasGroupOnStart;
		[SerializeField] InputMode _menuInputMode;
		InputMode _prevInputMode = InputMode.Game;
		bool _menuIsOpen;

		public void ToggleMenu ()
		{
			if (_menuIsOpen) CloseMenu ();
			else OpenMenu ();
		}

		public void OpenMenu ()
		{
			if (_menuIsOpen) return;
			_prevInputMode = InputManager._instance._InputMode;
			InputManager._instance.SetInputMode (_menuInputMode);
			_canvasFader.FadeCanvasIn ();
			_canvasFader.OnFadeInComplete += OnMenuOpened;
			_menuIsOpen = true;
		}

		public void CloseMenu ()
		{
			if (!_menuIsOpen) return;
			_canvasFader.FadeCanvasOut ();
			_canvasFader.OnFadeOutComplete += OnMenuClosed;
		}

		protected virtual void OnMenuOpened ()
		{
			_canvasFader.OnFadeInComplete -= OnMenuOpened;
		}

		protected virtual void OnMenuClosed ()
		{
			_canvasFader.OnFadeOutComplete -= OnMenuClosed;
			_menuIsOpen = false;
		}

		private void OnDestroy ()
		{
			InputManager._instance.SetInputMode (_prevInputMode);
			_canvasFader.OnFadeInComplete -= OnMenuOpened;
			_canvasFader.OnFadeOutComplete -= OnMenuClosed;
		}
	}
}