using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class RequestManager : MonoBehaviour
{
    public string PlayerName;
    public int Score;

    public Text Mytext;
    private void Start()
    {
        Mytext.text = PlayerName;
        StartCoroutine(WaitToScore());
    }
    IEnumerator WaitToScore()
    {
        yield return new WaitForSeconds(2);
        PlayfabManager.instance.SendLeaderboard(Score, PlayerName);
        PlayfabManager.instance.OnUpdatePlayerName(Mytext.text);
    }
}
