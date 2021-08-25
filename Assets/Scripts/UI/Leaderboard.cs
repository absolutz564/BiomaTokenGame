﻿using UnityEngine;

// Prefill the info on the player data, as they will be used to populate the leadboard.
public class Leaderboard : MonoBehaviour
{
	public RectTransform entriesRoot;
	public int entriesCount;

	public HighscoreUI playerEntry;
	public bool forcePlayerDisplay;
	public bool displayPlayer = true;

	public void Open()
	{
		gameObject.SetActive(true);

		Populate();
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void Populate()
	{
		// Start by making all entries enabled & putting player entry last again.
		playerEntry.transform.SetAsLastSibling();
		for(int i = 0; i < entriesCount; ++i)
		{
			entriesRoot.GetChild(i).gameObject.SetActive(true);
		}

		// Find all index in local page space.
		int localStart = 0;
		int place = -1;
		int localPlace = -1;

		if (displayPlayer)
		{
			place = PlayerData.instance.GetScorePlace(int.Parse(playerEntry.score.text));
			localPlace = place - localStart;
		}

		if (localPlace >= 0 && localPlace < entriesCount && displayPlayer)
		{
			playerEntry.gameObject.SetActive(true);
			playerEntry.transform.SetSiblingIndex(localPlace-1);
		}

		if (!forcePlayerDisplay || PlayerData.instance.highscores.Count < entriesCount)
			entriesRoot.GetChild(entriesRoot.transform.childCount - 1).gameObject.SetActive(false);

		int currentHighScore = localStart;

		for (int i = 0; i < PlayfabManager.instance.LeaderboardNames.Count; ++i)
		{
			HighscoreUI hs = entriesRoot.GetChild(i).GetComponent<HighscoreUI>();

			if (hs == playerEntry || hs == null)
			{
				// We skip the player entry.
				continue;
			}
			//Leaderboard do servidor
			hs.number.text = (localStart + i + 1).ToString();
			hs.playerName.text = PlayfabManager.instance.LeaderboardNames[i];
			hs.score.text = PlayfabManager.instance.LeaderboardScores[i];

			if (PlayerData.instance.highscores.Count > currentHighScore)
			{

				//hs.gameObject.SetActive(true);
				//hs.playerName.text = PlayerData.instance.highscores[currentHighScore].name;
				//hs.score.text = PlayerData.instance.highscores[currentHighScore].score.ToString();
				//hs.number.text = (localStart + i + 1).ToString();

				//currentHighScore++;
			}
			else
				Debug.Log("Erro");
		       // hs.gameObject.SetActive(false);
		}

		// If we force the player to be displayed, we enable it even if it was disabled from elsewhere
		if (forcePlayerDisplay) 
			playerEntry.gameObject.SetActive(true);

		playerEntry.number.text = (place).ToString();
	}
}
