using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Prefill the info on the player data, as they will be used to populate the leadboard.
public class Leaderboard : MonoBehaviour
{
	public RectTransform entriesRoot;
	public int entriesCount;

	public HighscoreUI playerEntry;
	public bool forcePlayerDisplay;
	public bool displayPlayer = true;

	public Image RankingColorBackground;

	public List<Color> RankingColors = new List<Color>();

	public Button StoryButton;
	public Button CompButton;
	public Image StoryImage;
	public Image CompImage;
	public GameObject LoadingObject;
	public void SetRanking(int index)
    {
		LoadingObject.SetActive(true);
		if (index == 0)
        {
			//PlayfabManager.instance.GetLeaderboard();
			StoryButton.interactable = false;
			CompButton.interactable = true;
			CompImage.gameObject.SetActive(false);
			StoryImage.gameObject.SetActive(true);
		}
        else
        {
			//PlayfabManager.instance.GetLeaderboardMonthly();
			StoryButton.interactable = true;
			CompButton.interactable = false;
			CompImage.gameObject.SetActive(true);
			StoryImage.gameObject.SetActive(false);
		}
		RankingColorBackground.color = RankingColors[index];
		//Invoke("Populate", 1);
		//Invoke("DisableLoading", 1.5f);
		StartCoroutine(DisableLoading());
	}

	IEnumerator DisableLoading()
    {
		yield return new WaitForSecondsRealtime(1f);
		Populate();
		yield return new WaitForSecondsRealtime(0.5f);
		LoadingObject.SetActive(false);
    }

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
		Debug.Log("preencheiu");
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

		//if (localPlace >= 0 && localPlace < entriesCount && displayPlayer)
		//{
		//	playerEntry.gameObject.SetActive(true);
		//	playerEntry.transform.SetSiblingIndex(localPlace-1);
		//}

		if (!forcePlayerDisplay || PlayerData.instance.highscores.Count < entriesCount)
			entriesRoot.GetChild(entriesRoot.transform.childCount - 1).gameObject.SetActive(false);

		int currentHighScore = localStart;

		List<string> rankingNames;
		List<string> rankingValues;
		if (StoryButton.IsInteractable())
        {
			rankingNames = new List<string>(PlayfabManager.instance.LeaderboardNamesMonthly);
			rankingValues = new List<string>(PlayfabManager.instance.LeaderboardScoresMonthly);
		}
        else
        {
			rankingNames = new List<string>(PlayfabManager.instance.LeaderboardNames);
			rankingValues = new List<string>(PlayfabManager.instance.LeaderboardScores);
		}

		for (int i = 0; i < rankingNames.Count; ++i)
		{
			HighscoreUI hs = entriesRoot.GetChild(i).GetComponent<HighscoreUI>();

			if (hs == playerEntry || hs == null)
			{
				// We skip the player entry.
				continue;
			}
			//Leaderboard do servidor
			hs.number.text = (i + 1).ToString();
			if (hs.number.text == "1" || hs.number.text == "2" || hs.number.text == "3")
            {
				hs.number.text = " ";
			}

			hs.playerName.text = rankingNames[i];
			hs.score.text = rankingValues[i];

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
		//if (forcePlayerDisplay) 
			//playerEntry.gameObject.SetActive(true);

		//playerEntry.number.text = (place).ToString();
	}
}
