using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Transform _target;
	[SerializeField] float _damping = 5f;
	Vector3 _offset;

	void Start ()
	{
		_offset = transform.position - _target.position;
	}

	void LateUpdate ()
	{
		if (_target == null) return;
		Vector3 _targetCamPos = _target.position + _offset;
		transform.position = Vector3.Lerp (transform.position, _targetCamPos, _damping * Time.deltaTime);
	}
}