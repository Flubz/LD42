using DG.Tweening;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
	[SerializeField] float _scaleTime = 1.0f;

	private void OnTriggerEnter (Collider other)
	{
		other.transform.DOScale (Vector3.zero, _scaleTime);
		Destroy (other.gameObject, _scaleTime + 0.5f);
	}
}