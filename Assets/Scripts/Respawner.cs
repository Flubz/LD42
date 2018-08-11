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
			StartCoroutine (Respawn (other.transform));
			other.gameObject.GetComponent<PlayerController> ()._playerHealth.Damage ();
		}
	}

	IEnumerator Respawn (Transform trans_)
	{
		yield return new WaitForSeconds (_respawnTime);
		trans_.position = _respawnPosition.position;
	}
}