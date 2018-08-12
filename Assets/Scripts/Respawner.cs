using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Respawner : MonoBehaviour
{
	[SerializeField] Transform _respawnPosition;
	[SerializeField] float _respawnTime = 0.5f;

	private void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			other.gameObject.GetComponent<PlayerController> ()._playerHealth.Damage ();
			StartCoroutine (Respawn (other.transform));
		}
	}

	IEnumerator Respawn (Transform trans_)
	{
		yield return new WaitForSeconds (_respawnTime);
		trans_.position = _respawnPosition.position;
	}
}