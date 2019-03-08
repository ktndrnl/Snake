using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	private GameObject _playerGo;
	private Player _player;

	private void Start()
	{
		_playerGo = gameObject;
		_player = _playerGo.GetComponent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Pickup"))
		{
			// TODO
			// Add to score
			// Play sound
			// Add segment to player
			// Destroy pickup
			// Spawn new pickup

			GameManager.S.UpdateScore(1);
			AudioManager.S.Play("Pickup");
			//
			// GameObject newSegment = Instantiate(segmentPrefab, 
			// 	new Vector3(-10f, -10f, 0f), Quaternion.identity);
			// _segments.Add(newSegment.transform);
			Destroy(other.gameObject);
			GameManager.S.SpawnPickup();
		}

		else if (other.CompareTag("PlayerSegment"))
		{
			// TODO
			// Reset the player position and score

			//print("Segment collision");
			//ResetPlayer();
		}

		else if (other.CompareTag("Wall"))
		{
			// TODO
			// Reset the player position and score

			//print("Wall collision");
			//ResetPlayer();
		}
	}
}
