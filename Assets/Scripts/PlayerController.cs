using System;
using System.Collections;
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
	[SerializeField] Rigidbody _rb;
	[SerializeField] Collider _collider;

	CalibratedAccelerometer _calibratedAccel;

	[SerializeField] bool _PCBuild;

	Vector3 _moveDir = Vector3.zero;
	Vector3 _jumpVelocity = Vector3.zero;
	Transform _currentPlatform;
	public PlayerHealth _playerHealth;
	bool _gameOver;

	private void Start ()
	{
		_playerHealth.ResetHealth ();
		ScoreManager._instance.MultiplierReset ();
		_playerHealth.OnDeathEvent += OnDeath;
		_playerHealth.OnDamageEvent += OnDamaged;
	}

	public void OnGameStart ()
	{
		_calibratedAccel = new CalibratedAccelerometer ();
		_calibratedAccel.CalibrateAccelerometer ();
	}

	void Update ()
	{
		if (InputManager._instance._InputMode != InputMode.Game || _gameOver) return;

		if (_PCBuild) _moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		else
		{
			_moveDir = _calibratedAccel.GetAccelerometer (Input.acceleration);
			_moveDir.z = _moveDir.y;
			_moveDir.Remap (-1, 1, -0.1f, 0.1f);
		}

		Debug.Log (_moveDir);

		_moveDir = transform.TransformDirection (_moveDir);

		if (_controller.isGrounded)
		{
			_moveDir *= _speed;
			if (Input.GetButton ("Jump") || Input.GetButton ("Fire1") || Input.touchCount == 1)
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
			_moveDir *= _airControlAmount;
			_jumpVelocity.y -= _gravity * Time.deltaTime;
		}

		if (_moveDir.magnitude > _rotationDeadzone)
			_playerGraphics.rotation = Quaternion.Slerp (_playerGraphics.rotation, Quaternion.LookRotation (_moveDir, Vector3.up), Time.deltaTime * _rotationSpeed);
		_controller.Move ((_moveDir + _jumpVelocity) * Time.deltaTime);
	}

	void OnDeath ()
	{
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

	public void OnHitGround ()
	{
		_playerHealth.Damage ();
		_collider.enabled = false;
	}

	public void OnRespawn ()
	{
		_collider.enabled = true;
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
	int _health;
	[SerializeField] GameObject[] _healthUI;
	public Action OnDeathEvent;
	public Action OnDamageEvent;
	public bool _invulnerable;

	public void Damage ()
	{
		if (!_invulnerable)
		{
			_health--;
			for (int i = 0; i < _healthUI.Length; i++)
			{
				if (_healthUI[i].activeSelf)
				{
					_healthUI[i].SetActive (false);
					break;
				}
			}
		}
		if (OnDamageEvent != null) OnDamageEvent.Invoke ();
		if (_health <= 0 && OnDeathEvent != null) OnDeathEvent.Invoke ();
	}

	public void ResetHealth ()
	{
		_health = _healthUI.Length;

		for (int i = 0; i < _healthUI.Length; i++)
		{
			_healthUI[i].SetActive (true);
		}
	}

}