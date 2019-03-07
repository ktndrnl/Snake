using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
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
	public float invokeTime = 0.100f;
	public GameObject segmentPrefab;
	public float interpolationSpeed = 10.0f;
	public float timeBetweenMoves = 0.3333f;

	[Header("Set Dynamically")]
	private Transform _playerTransform;
	private List<Transform> _segments;
	private EDir _dir;
	private EDir _lastDir;
	private float _timestamp;
	private Vector3 _desiredPosition;

	private void Start()
	{
		_segments = new List<Transform>();
		_playerTransform = gameObject.transform;
		_desiredPosition = _playerTransform.position;
		//Invoke("Move", invokeTime);
	}

	private void Update()
	{
		GetPlayerInput();
		Move();
		_playerTransform.position = Vector3.Lerp(_playerTransform.position, _desiredPosition,
			interpolationSpeed * Time.deltaTime);
	}

	// COLLISION
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Pickup"))
		{
			GameManager.S.UpdateScore(1);
			FindObjectOfType<AudioManager>().Play("Pickup");

			GameObject newSegment = Instantiate(segmentPrefab, 
				new Vector3(-10f, -10f, 0f), Quaternion.identity);
			_segments.Add(newSegment.transform);
			Destroy(other.gameObject);

			GameManager.S.SpawnPickup();
		}

		else if (other.CompareTag("PlayerSegment"))
		{
			//print("Segment collision");
			ResetPlayer();
		}

		else if (other.CompareTag("Wall"))
		{
			//print("Wall collision");
			ResetPlayer();
		}
	}

	// MOVEMENT
	// TODO Fix tail movement
	private void Move()
	{
		if (Time.time >= _timestamp)
		{
			MoveSegments();
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

			_timestamp = Time.time + timeBetweenMoves;
		}

		//Invoke("Move", invokeTime);

		void MoveSegments()
		{
			if (_segments.Count > 0)
			{
				_segments.Last().gameObject.GetComponent<Collider2D>().enabled = false;
				_segments.Last().position = _playerTransform.position;
				if (_segments.Count > 1)
				{
					_segments[0].gameObject.GetComponent<Collider2D>().enabled = true;
				}
				_segments.Insert(0, _segments.Last());
				_segments.RemoveAt(_segments.Count - 1);
			}
		}
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

	// GAME LOGIC
	private void ResetPlayer()
	{
		_desiredPosition = new Vector3(25, 25,0);
		_playerTransform.position = new Vector3(25, 25,0);
		_dir = EDir.None;
		_lastDir = EDir.None;
		foreach (Transform segment in _segments)
		{
			Destroy(segment.gameObject);
		}
		_segments.Clear();
		GameManager.S.score = 0;
		GameManager.S.UpdateGUI();
		FindObjectOfType<AudioManager>().Play("GameOver");
	}
}
