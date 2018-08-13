using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager _instance = null;
	void Awake ()
	{
		if (_instance == null) _instance = this;
		else if (_instance != this) Destroy (gameObject);
		_canvasFader.FadeCanvasIn ();
		_CurrentMultipier = 1;
	}

	[SerializeField] CanvasFader _canvasFader;
	[SerializeField] Image _multiplierUnlocker;

	public int _CurrentScore { get; private set; }
	public int _CurrentMultipier { get; private set; }

	[SerializeField] int _maxMultiplierAmount = 64;
	[SerializeField] int _scoreForPlatform = 10;

	[SerializeField] Text _multiText;
	[SerializeField] Text _scoreText;

	[SerializeField] int _notesForNextMultiUnlock = 5;
	int _currentNotesHitCount;
	bool _maxMulti;

	public void IncrementScore ()
	{
		_CurrentScore += _scoreForPlatform * _CurrentMultipier;
		_scoreText.text = _CurrentScore.ToString ();
		_currentNotesHitCount++;
		if (!_maxMulti) _multiplierUnlocker.fillAmount = (float) _currentNotesHitCount / (float) _notesForNextMultiUnlock;
		if (_currentNotesHitCount == _notesForNextMultiUnlock) MultiplierStepUp ();
	}

	void MultiplierStepUp ()
	{
		_currentNotesHitCount = 0;
		if (_CurrentMultipier < _maxMultiplierAmount) _CurrentMultipier *= 2;
		else _maxMulti = true;
		_multiText.text = "x" + _CurrentMultipier.ToString ();
	}

	public void MultiplierReset ()
	{
		_maxMulti = false;
		_multiplierUnlocker.fillAmount = 0;
		_CurrentMultipier = 1;
		_multiText.text = "x" + _CurrentMultipier.ToString ();
	}
}