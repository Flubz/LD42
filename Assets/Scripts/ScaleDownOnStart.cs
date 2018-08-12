using DG.Tweening;
using UnityEngine;

public class ScaleDownOnStart : MonoBehaviour
{
	[SerializeField] float _scaleTime = 1.0f;

	private void Start ()
	{
		transform.DOScaleX (Vector3.zero.x, _scaleTime);
		transform.DOScaleZ (Vector3.zero.z, _scaleTime);
		Destroy (gameObject, _scaleTime + 0.5f);
	}
}