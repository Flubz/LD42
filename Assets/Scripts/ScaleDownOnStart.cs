using DG.Tweening;
using UnityEngine;

public class ScaleDownOnStart : MonoBehaviour
{
	[SerializeField] float _scaleTime = 1.0f;

	private void Start ()
	{
		transform.DOScale (Vector3.zero, _scaleTime);
		Destroy (gameObject, _scaleTime + 0.5f);
	}
}