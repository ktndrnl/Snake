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
		_playerTransform = transform;
		_segments = new List<Transform>
		{
			_playerTransform
		};
	}

	private void Update()
	{
		if (_segments.Count > 1)
		{
			for (int i = 1; i < _segments.Count; i++)
			{
				_segments[i].position = Vector3.Lerp(_segments[i].position, 
					_segments[i-1].position,
					interpolationSpeed * Time.deltaTime);
			}
		}
	}

	// GAME LOGIC
	private void ResetPlayer()
	{
		_desiredPosition = new Vector3(25, 25,0);
		_playerTransform.position = new Vector3(25, 25,0);
		_dir = EDir.None;
		_lastDir = EDir.None;
		if (_segments.Count > 1)
		{
			for (int i = 1; i < _segments.Count; i++)
			{
				Destroy(_segments[i].gameObject);
			}
		}
		_segments.Clear();
		GameManager.S.score = 0;
		GameManager.S.UpdateGUI();
		FindObjectOfType<AudioManager>().Play("GameOver");
	}
}
