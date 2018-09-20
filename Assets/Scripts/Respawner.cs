using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Respawner : MonoBehaviour
{
	[SerializeField] Transform _respawnPosition;
	[SerializeField] float _respawnTime = 0.5f;
	[SerializeField] ScaleDownOnStart _spawnGround;
	[SerializeField] Transform _spawnGroundPos;

	private void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			StartCoroutine (Respawn (other.gameObject.GetComponent<PlayerController> ()));
		}
	}

	IEnumerator Respawn (PlayerController pc_)
	{
		pc_.OnHitGround ();
		yield return new WaitForSeconds (_respawnTime);
		pc_.OnRespawn ();
		pc_.transform.position = _respawnPosition.position;
		Instantiate (_spawnGround, _spawnGroundPos.position, Quaternion.identity);
	}
}