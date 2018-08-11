using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager _instance = null;
	void Awake ()
	{
		if (_instance == null) _instance = this;
		else if (_instance != this) Destroy (gameObject);
		_canvasFader.FadeCanvasIn ();
		_canAddToScore.TriggerEvent (this);
		_CurrentMultipier = 1;
	}

	[SerializeField] CanvasFader _canvasFader;
	[SerializeField] TimedEvent _canAddToScore;

	public int _CurrentScore { get; private set; }
	public int _CurrentMultipier { get; private set; }

	[SerializeField] int _maxMultiplierAmount = 256;
	[SerializeField] int _scoreForPlatform = 10;

	[SerializeField] Text _multiText;
	[SerializeField] Text _scoreText;

	public void IncrementScore ()
	{
		if (!_canAddToScore.IsEventReady ()) return;
		_canAddToScore.TriggerEvent (this);
		_CurrentScore += _scoreForPlatform * _CurrentMultipier;
		_scoreText.text = _CurrentScore.ToString ();
		MultiplierStepUp ();
	}

	void MultiplierStepUp ()
	{
		if (_CurrentMultipier < _maxMultiplierAmount)
			_CurrentMultipier *= 2;
		_multiText.text = "x" + _CurrentMultipier.ToString ();
	}

	public void MultiplierReset ()
	{
		_CurrentMultipier = 1;
		_multiText.text = "x" + _CurrentMultipier.ToString ();
	}
}