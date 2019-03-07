using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager S;

	[Header("Set in Inspector")]
	// UI
	public Text uiScoreText;
	public Text uiBestScoreText;
	public GameObject pickupPrefab;

	[Header("Set Dynamically")]
	public int score;
	public int bestScore;

	private void Awake()
	{
		if (S == null)
		{
			S = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if (!PlayerPrefs.HasKey("bestScore"))
		{
			PlayerPrefs.SetInt("bestScore", 0);
		}

		bestScore = PlayerPrefs.GetInt("bestScore");
		UpdateGUI();

		SpawnPickup();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void UpdateScore(int points)
	{
		score += points;
		if (score > bestScore)
		{
			bestScore = score;
			PlayerPrefs.SetInt("bestScore", score);
		}
		UpdateGUI();
	}

	public void UpdateGUI()
	{
		uiScoreText.text = $"SCORE: {score}";
		uiBestScoreText.text = $"BEST: {bestScore}";
	}

	public void SpawnPickup()
	{
		Instantiate(pickupPrefab, new Vector3(Random.Range(2, 49),
			Random.Range(2, 49), 0), Quaternion.identity);
	}
}
