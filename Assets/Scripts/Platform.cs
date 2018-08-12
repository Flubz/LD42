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
	[SerializeField] MeshRenderer _meshRenderer;
	[SerializeField] Material _touchEffectMaterial;
	[SerializeField] ShakeEffect _shake;

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
			transform.Translate (transform.forward * -1 * _speed * Time.deltaTime);
		}
		else if (!_destroyed)
		{
			DestroyPlatform ();
		}
	}

	public void TouchEffect ()
	{
		_meshRenderer.material = _touchEffectMaterial;
		transform.DOShakeScale (_shake._duration, _shake._strength, _shake._vibratio, _shake._randomness, _shake._fadeOut);
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

[System.Serializable]
public class ShakeEffect
{
	public float _duration = 1;
	public float _strength = 1;
	public int _vibratio = 1;
	public float _randomness = 1;
	public bool _fadeOut = true;
}