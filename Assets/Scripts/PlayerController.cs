using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header ("Movement")]
	[SerializeField] Transform _playerGraphics;

	[SerializeField] float _speed = 6.0F;
	[SerializeField] float _jumpSpeed = 8.0F;
	[SerializeField] float _gravity = 20.0F;
	[SerializeField] CharacterController _controller;
	[SerializeField] float _rotationDeadzone = 0.01f;
	[SerializeField] float _rotationSpeed = 1.0f;
	[SerializeField] float _airControlAmount = 0.01f;

	Vector3 _moveDir = Vector3.zero;
	Vector3 _jumpVelocity = Vector3.zero;

	void Update ()
	{
		_moveDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		_moveDir = transform.TransformDirection (_moveDir);

		if (_controller.isGrounded)
		{
			_moveDir *= _speed;
			if (Input.GetButton ("Jump"))
			{
				_jumpVelocity = _moveDir;
				_jumpVelocity.y = _jumpSpeed;
			}
			else
			{
				_jumpVelocity = Vector3.zero;
			}
		}
		else
		{
			_moveDir *= _airControlAmount;
			_jumpVelocity.y -= _gravity * Time.deltaTime;
		}

		if (_moveDir.magnitude > _rotationDeadzone) _playerGraphics.rotation = Quaternion.Slerp (_playerGraphics.rotation, Quaternion.LookRotation (_moveDir, Vector3.up), Time.deltaTime * _rotationSpeed);
		_controller.Move ((_moveDir + _jumpVelocity) * Time.deltaTime);
	}

}