using System;
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
	[Tooltip ("How much of the initial movement of the player needs to be in the beginning of the jump?")]
	[SerializeField] float _initialJumpMultiplier = 1.0f;
	[SerializeField] float _slamSpeed = 8.0f;
	[SerializeField] Rigidbody _rb;

	Vector3 _moveDir = Vector3.zero;
	Vector3 _jumpVelocity = Vector3.zero;
	Transform _currentPlatform;
	public PlayerHealth _playerHealth;
	bool _gameOver;

	private void Start ()
	{
		_playerHealth.ResetHealth ();
		_playerHealth.OnDeathEvent += OnDeath;
		_playerHealth.OnDamageEvent += OnDamaged;
	}

	void Update ()
	{
		if (InputManager._instance._InputMode != InputMode.Game || _gameOver) return;

		_moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		_moveDir = transform.TransformDirection (_moveDir);

		if (_controller.isGrounded)
		{
			_moveDir *= _speed;
			if (Input.GetButton ("Jump") || Input.GetButton ("Fire1"))
			{
				_jumpVelocity = _moveDir * _initialJumpMultiplier;
				_jumpVelocity.y = _jumpSpeed;
			}
			else
			{
				_jumpVelocity = Vector3.zero;
			}
		}
		else
		{
			if (Input.GetButton ("Fire2"))
			{
				_moveDir *= _airControlAmount;
				_jumpVelocity.y -= _gravity * Time.deltaTime * _slamSpeed;
			}
			else
			{
				_moveDir *= _airControlAmount;
				_jumpVelocity.y -= _gravity * Time.deltaTime;
			}
		}

		if (_moveDir.magnitude > _rotationDeadzone)
			_playerGraphics.rotation = Quaternion.Slerp (_playerGraphics.rotation, Quaternion.LookRotation (_moveDir, Vector3.up), Time.deltaTime * _rotationSpeed);
		_controller.Move ((_moveDir + _jumpVelocity) * Time.deltaTime);
	}

	void OnDeath ()
	{
		// Give up game over UI;
		// TODO
		_gameOver = true;
		_rb.isKinematic = true;
		DisplayHighscores._instance.GameOver ();
	}

	void OnDamaged ()
	{
		// Health UI already updated.
		ScoreManager._instance.MultiplierReset ();
		// Update Combo. 
		// Damaged/Respawn Effect
		// TODO
	}

	private void OnDestroy ()
	{
		_playerHealth.OnDeathEvent -= OnDeath;
		_playerHealth.OnDamageEvent -= OnDamaged;
	}

	private void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.CompareTag ("Platform"))
		{
			ScoreManager._instance.IncrementScore ();
			other.gameObject.GetComponent<Platform> ().TouchEffect ();
		}
	}
}

[System.Serializable]
public class PlayerHealth
{
	int _initialMaxHealth = 3;
	int _health = 3;
	[SerializeField] GameObject[] _healthUI;
	public Action OnDeathEvent;
	public Action OnDamageEvent;

	public void Damage ()
	{
		_health--;
		for (int i = 0; i < _initialMaxHealth; i++)
		{
			if (_healthUI[i].activeSelf)
			{
				_healthUI[i].SetActive (false);
				return;
			}
		}
		if (_health <= 0 && OnDeathEvent != null) OnDeathEvent.Invoke ();
		else OnDamageEvent.Invoke ();
	}

	public void ResetHealth ()
	{
		_health = _initialMaxHealth;

		for (int i = 0; i < _initialMaxHealth; i++)
		{
			_healthUI[i].SetActive (true);
		}
	}
}