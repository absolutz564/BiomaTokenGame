using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public Volume CurrProfile;
    public ColorAdjustments ProfColor;

    bool IsGrayScale = false;

    public GameObject PopupBitcoin;
    public GameObject PopupBioma;

    public GameObject NormalCoin;
    public GameObject PremiumCoin;

    public Image NatureImage;

    public void SetGrayScale()
    {
        if (ProfColor.saturation.value > -100 && !IsGrayScale)
        {
            ProfColor.saturation.value -= 2;
            NatureImage.fillAmount -= 0.02f;
            if (NatureImage.color == Color.green && NatureImage.fillAmount < 0.7f)
            {
                NatureImage.color = Color.yellow;
            }
            else if (NatureImage.color == Color.yellow && NatureImage.fillAmount < 0.4f)
            {
                NatureImage.color = Color.red;
            }
        }
        else if (ProfColor.saturation.value < 0 && IsGrayScale)
        {
            ProfColor.saturation.value += 2;

            NatureImage.fillAmount += 0.02f;
            if (NatureImage.color == Color.red && NatureImage.fillAmount > 0.3f)
            {
                NatureImage.color = Color.yellow;
            }
            else if (NatureImage.color == Color.yellow && NatureImage.fillAmount > 0.7f)
            {
                NatureImage.color = Color.green;
            }

            if (ProfColor.saturation.value == -50 & IsGrayScale)
            {
                PopupBioma.SetActive(true);
                Time.timeScale = 0;
            }
        }

        else if (ProfColor.saturation.value == -100 & !IsGrayScale)
        {
            IsGrayScale = true;
            PopupBitcoin.SetActive(true);
            Time.timeScale = 0;
            ChangeCoin(true);
            PlayerPrefs.SetInt("historyplayed", 1);
        }
        //else if (ProfColor.saturation.value == 0 & IsGrayScale)
        //{
        //    IsGrayScale = false;
        //    ChangeCoin(false);
        //    //PopupBitcoin.SetActive(true);
        //    //Time.timeScale = 0;
        //}
    }

    public void ChangeCoin(bool toPremium)
    {
        if (toPremium)
        {
            foreach (GameObject coin in TrackManager.instance.CoinList)
            {
                Coin CoinController = coin.GetComponent<Coin>();
                if (CoinController != null)
                {
                    CoinController.isPremium = true;
                    coin.transform.GetChild(0).gameObject.SetActive(false);
                    coin.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            NormalCoin.SetActive(false);
            PremiumCoin.SetActive(true);
        }
        else
        {
            foreach (GameObject coin in TrackManager.instance.CoinList)
            {
                Coin CoinController = coin.GetComponent<Coin>();
                if (CoinController != null)
                {
                    CoinController.isPremium = false;
                    coin.transform.GetChild(0).gameObject.SetActive(true);
                    coin.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
            NormalCoin.SetActive(true);
            PremiumCoin.SetActive(false);
        }
    }
    private void Awake()
    {
        Instance = this;

        ColorAdjustments tempColor;
        NatureImage.color = Color.green;

        if (CurrProfile.profile.TryGet<ColorAdjustments>(out tempColor))
        {
            ProfColor = tempColor;
        }
        SetGameState();
    }
    public void SetGameState()
    {
        if (PlayerPrefs.HasKey("historyplayed"))
        {
            Debug.Log("ja jogou historia  " + PlayerPrefs.GetInt("historyplayed"));

            if (GetBool("historyplayed"))
            {
                StartCoroutine(WaitToChange());
                Debug.Log("ja jogou historia");
            }
        }
    }

    private IEnumerator WaitToChange()
    {
        yield return new WaitForSeconds(1f);
        ChangeCoin(true);
        IsGrayScale = true;
    }

    public static bool GetBool(string key)
    {
        int value = PlayerPrefs.GetInt(key);

        if (value == 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public void ReturnToGame()
    {
        PopupBitcoin.SetActive(false);
        PopupBioma.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
