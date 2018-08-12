using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour
{
	[SerializeField] Transform _shadow;
	[SerializeField] LayerMask _layerMask;

	private void Update ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.up * -1, out hit, Mathf.Infinity, _layerMask))
		{
			_shadow.localScale = Vector3.one;
			_shadow.position = hit.point;
		}
		else
		{
			_shadow.localScale = Vector3.zero;
			_shadow.position = Vector3.zero;
		}
	}
}