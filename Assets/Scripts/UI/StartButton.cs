using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

public class StartButton : MonoBehaviour
{
    public Text PlayerNameText;
    public Text PlayerEmailText;

    public Button SaveButton;

    public GameObject AccountObject;
    public GameObject PlayGameObject;

    public Button SaveNameButton;
    public GameObject NameObject;
    public GameObject EmailObject;

    public GameObject RankingObject;

    public void SetRankingState(bool state)
    {
        RankingObject.SetActive(state);
    }

    public void StartGame()
    {
        if (PlayerData.instance.ftueLevel == 0)
        {
            PlayerData.instance.ftueLevel = 1;
            PlayerData.instance.Save();
#if UNITY_ANALYTICS
            AnalyticsEvent.FirstInteraction("start_button_pressed");
#endif
        }

#if UNITY_PURCHASING
        var module = StandardPurchasingModule.Instance();
#endif
        SceneManager.LoadScene("main");
    }

    public void ValueChangedBtName()
    {
        StartCoroutine(WaitToCheckName());
    }
    IEnumerator WaitToCheckName()
    {
        yield return new WaitForSeconds(0.3f);
        if (PlayerNameText.text != string.Empty && !SaveNameButton.IsInteractable())
        {
            SaveNameButton.interactable = true;
        }

        else if (PlayerNameText.text == string.Empty)
        {
            SaveNameButton.interactable = false;
        }
    }

    public void GoToInputEmail()
    {
        NameObject.SetActive(false);
        EmailObject.SetActive(true);
    }
    public void ValueChanged()
    {
        StartCoroutine(WaitToCheck());
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("playername") && PlayerPrefs.HasKey("playeremail"))
        {
           // GetData();
            SetAccountInfo();
        }
    }

    IEnumerator WaitToCheck()
    {
        yield return new WaitForSeconds(0.3f);
        if (PlayerNameText.text != string.Empty && PlayerEmailText.text != string.Empty && !SaveButton.IsInteractable())
        {
            SaveButton.interactable = true;
        }
        else if (PlayerNameText.text == string.Empty || PlayerEmailText.text == string.Empty)
        {
            SaveButton.interactable = false;
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("playername", PlayerNameText.text);
        PlayerPrefs.SetString("playeremail", PlayerEmailText.text);
    }
    void GetData()
    {
        PlayerNameText.text = PlayerPrefs.GetString("playername");
        PlayerEmailText.text = PlayerPrefs.GetString("playeremail");
    }
    public void SetAccountInfo()
    {
        Debug.Log(PlayerPrefs.GetString("playername"));
        Debug.Log(PlayerPrefs.GetString("playeremail"));
        PlayfabManager.instance.Email = PlayerPrefs.GetString("playeremail");
        PlayfabManager.instance.PlayerName = PlayerPrefs.GetString("playername");
        PlayfabManager.instance.RegisterButton();

        AccountObject.SetActive(false);
        PlayGameObject.SetActive(true);
    }

}
