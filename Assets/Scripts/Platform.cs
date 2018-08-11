using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Platform : MonoBehaviour
{
	[SerializeField] float _speed = 1.0f;
	[SerializeField] float _scaleAnimTime = 1.0f;
	[SerializeField] Vector3 _deathPos;
	[SerializeField] Ease _spawnEase = Ease.InOutBounce;

	Vector3 _originalScale;
	bool _destroyed;

	private void Start ()
	{
		_originalScale = transform.localScale;
		transform.localScale = Vector3.zero;
		transform.DOScale (_originalScale, _scaleAnimTime).SetEase (_spawnEase);
	}

	private void Update ()
	{
		if (transform.position.z > _deathPos.z)
		{
			transform.Translate (transform.forward * -1 * _speed);
		}
		else if (!_destroyed)
		{
			DestroyPlatform ();
		}
	}

	void DestroyPlatform ()
	{
		_destroyed = true;
		transform.DOScale (Vector3.zero, _scaleAnimTime);
		Destroy (gameObject, _scaleAnimTime + 0.5f);
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (_deathPos, 0.5f);
	}

	private void OnValidate ()
	{
		_deathPos.x = _deathPos.y = 0;
	}
#endif
}