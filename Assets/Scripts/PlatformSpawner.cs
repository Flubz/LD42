using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
	[SerializeField] Platform _platformPrefab;
	[SerializeField] List<Transform> _spawnPoints;
	[SerializeField] List<PlatformsToSpawnAtTime> _platformsAtTime;
	AudioProcessor _processor;
	int _currentIndex = 0;

	void Start ()
	{
		Debug.Log(this);
		_processor = AudioProcessor.FindObjectOfType<AudioProcessor> ();
		_processor.onBeat.AddListener (onOnbeatDetected);
		_processor.onSpectrum.AddListener (onSpectrum);
		_currentIndex = 0;
	}

	private void Update() {
		Debug.Log(_processor);
	}

	void SpawnPlatforms ()
	{
		Debug.Log("Spawning platforms");
		UpdatePlatformCountIndex ();
		_spawnPoints.Shuffle ();

		for (int i = 0; i < _platformsAtTime[_currentIndex]._numberOfPlatforms; i++)
		{
			Instantiate (_platformPrefab, _spawnPoints[i].position, Quaternion.identity, transform);
		}
	}

	//this event will be called every time a beat is detected.
	//Change the threshold parameter in the inspector
	//to adjust the sensitivity
	void onOnbeatDetected ()
	{
		SpawnPlatforms ();
	}

	//This event will be called every frame while music is playing
	void onSpectrum (float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands

		for (int i = 0; i < spectrum.Length; ++i)
		{
			Vector3 start = new Vector3 (i, 0, 0);
			Vector3 end = new Vector3 (i, spectrum[i], 0);
			Debug.DrawLine (start, end);
		}
	}

	void UpdatePlatformCountIndex ()
	{
		if (_currentIndex + 1 == _platformsAtTime.Count) return;
		if (_processor.audioSource.time > _platformsAtTime[_currentIndex + 1]._time)
		{
			_currentIndex++;
		}
	}

	[System.Serializable]
	class PlatformsToSpawnAtTime
	{
		[Range (0, 256.2f)]
		public float _time = 0;
		public Color _color = Color.white;
		public int _numberOfPlatforms = 0;
	}

}