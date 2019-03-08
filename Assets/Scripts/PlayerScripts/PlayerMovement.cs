using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private enum EDir
	{
		None,
		Up,
		Down,
		Left,
		Right
	}

	private delegate bool KeyDownBool(KeyCode code);
	KeyDownBool keyDown = Input.GetKeyDown;

	[Header("Set in Inspector")]
	public float timeBetweenMoves = 0.3333f;
	public float interpolationSpeed = 10.0f;

	[Header("Set Dynamically")]
	private Transform _playerTransform;
	private EDir _dir;
	private EDir _lastDir;
	private Vector3 _desiredPosition;

	private void Start()
	{
		_playerTransform = gameObject.transform;
		_desiredPosition = _playerTransform.position;

		SetDirection();
	}

	private void Update()
	{
		GetPlayerInput();
		Move();
	}

	// MOVEMENT
	private void Move()
	{
		_playerTransform.position = Vector3.Lerp(_playerTransform.position, _desiredPosition,
			interpolationSpeed * Time.deltaTime);
	}

	private void SetDirection()
	{
		switch (_dir)
		{
			case EDir.None:
				break;

			case EDir.Up:
				if (_lastDir == EDir.Down)
				{
					_desiredPosition += Vector3.down;
					break;
				}

				_desiredPosition += Vector3.up;
				_lastDir = EDir.Up;
				break;

			case EDir.Down:
				if (_lastDir == EDir.Up)
				{
					_desiredPosition += Vector3.up;
					break;
				}

				_desiredPosition += Vector3.down;
				_lastDir = EDir.Down;
				break;

			case EDir.Left:
				if (_lastDir == EDir.Right)
				{
					_desiredPosition += Vector3.right;
					break;
				}

				_desiredPosition += Vector3.left;
				_lastDir = EDir.Left;
				break;

			case EDir.Right:
				if (_lastDir == EDir.Left)
				{
					_desiredPosition += Vector3.left;
					break;
				}

				_desiredPosition += Vector3.right;
				_lastDir = EDir.Right;
				break;
		}

		Invoke(nameof(SetDirection), timeBetweenMoves);

	}


	// INPUT
	private void GetPlayerInput()
	{
		if (keyDown(KeyCode.S) || keyDown(KeyCode.DownArrow))
		{
			_dir = EDir.Down;
		}

		if (keyDown(KeyCode.W) || keyDown(KeyCode.UpArrow))
		{
			_dir = EDir.Up;
		}

		if (keyDown(KeyCode.A) || keyDown(KeyCode.LeftArrow))
		{
			_dir = EDir.Left;
		}

		if (keyDown(KeyCode.D) || keyDown(KeyCode.RightArrow))
		{
			_dir = EDir.Right;
		}
	}
}
