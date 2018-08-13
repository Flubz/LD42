using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlatformSpawner : MonoBehaviour
{
	[SerializeField] Platform _platformPrefab;
	[SerializeField] List<Transform> _spawnPoints;
	[SerializeField] List<PlatformsPerRowAtTime> _platformsAtTime;
	[SerializeField] List<PlatformsSpawnRateAtTime> _platformSpawnRate;
	AudioProcessor _processor;
	int _currentRowSpawnIndex = 0;
	int _currentSpawnRateIndex = 0;

	[SerializeField] Vector2 _randRangePlatformSpawnRate;
	[SerializeField] float _spawnTimeModifier = -1.0f;

	void Start ()
	{
		_processor = AudioProcessor.FindObjectOfType<AudioProcessor> ();
		// _processor.onBeat.AddListener (onOnbeatDetected);
		_currentRowSpawnIndex = 0;
		StartCoroutine (SpawnPlatformsByTime ());
	}

	void SpawnPlatforms ()
	{
		UpdatePlatformPerRowCountIndex ();
		_spawnPoints.Shuffle ();

		for (int i = 0; i < _platformsAtTime[_currentRowSpawnIndex]._numberOfPlatformsPerRow; i++)
		{
			Instantiate (_platformPrefab, _spawnPoints[i].position, Quaternion.identity, transform);
		}
	}

	private void Update ()
	{
		Debug.Log (_processor.audioSource.time);
	}

	IEnumerator SpawnPlatformsByTime ()
	{
		float t;
		while (true)
		{
			t = (Random.Range (_randRangePlatformSpawnRate.x, _randRangePlatformSpawnRate.y));
			SpawnPlatforms ();
			UpdatePlatformSpawnRateIndex ();
			yield return new WaitForSeconds (t + _platformSpawnRate[_currentSpawnRateIndex]._spawnDelay);
		}
	}

	void UpdatePlatformSpawnRateIndex ()
	{
		if (_currentSpawnRateIndex + 1 == _platformSpawnRate.Count) return;
		if (_processor.audioSource.time > _platformSpawnRate[_currentSpawnRateIndex + 1]._time + _spawnTimeModifier)
		{
			_currentSpawnRateIndex++;
		}
	}

	void UpdatePlatformPerRowCountIndex ()
	{
		if (_currentRowSpawnIndex + 1 == _platformsAtTime.Count) return;
		if (_processor.audioSource.time > _platformsAtTime[_currentRowSpawnIndex + 1]._time)
		{
			_currentRowSpawnIndex++;
			RenderSettings.fogColor = Color.Lerp (RenderSettings.fogColor, _platformsAtTime[_currentRowSpawnIndex]._fogColorChange, 1.0f);
		}
	}

	[System.Serializable]
	class PlatformsPerRowAtTime
	{
		[Range (0, 256.2f)]
		public float _time = 0;
		public Color _fogColorChange = Color.white;
		public int _numberOfPlatformsPerRow = 3;
	}

	[System.Serializable]
	class PlatformsSpawnRateAtTime
	{
		[Header ("")]
		[Range (0, 256.2f)]
		public float _time = 0;
		[Range (0, 1.5f)] public float _spawnDelay = 0.1f;
	}

	//this event will be called every time a beat is detected.
	//Change the threshold parameter in the inspector
	//to adjust the sensitivity
	// void onOnbeatDetected ()
	// {
	// 	SpawnPlatforms ();
	// }

}