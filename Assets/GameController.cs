using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isTutorial = true;
    public bool isMult = false;
    public static GameController Instance;
    public Volume CurrProfile;
    public ColorAdjustments ProfColor;

    public bool IsGrayScale = false;

    public GameObject PopupBitcoin;
    public GameObject PopupBioma;
    public GameObject PopupMissionCompleted;

    public GameObject NormalCoin;
    public GameObject PremiumCoin;

    public List<GameObject> NatureImages;

    public DegradableItem[] Degradables;

    public GameObject RankingObject;

    public GameObject CoinParent;

    public Text RecordText;

    public GameObject NewRecordObject;

    public ParticleSystem SmokeParticle;
    public ParticleSystem GreenParticle;

    public GrowAndShrink BadCoinPulse;
    public GrowAndShrink BiomaCoinPulse;
    public GrowAndShrink TreePulse;

    public Animator BrilhoAnim;

    public void SetCoinTo2x(bool state)
    {
        if (state)
        {
            CoinParent.transform.GetChild(0).gameObject.SetActive(false);
            CoinParent.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            CoinParent.transform.GetChild(0).gameObject.SetActive(true);
            CoinParent.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        RecordText.text = PlayfabManager.instance.LeaderboardScores[0];
        Degradables.Initialize();
    }

    public void SetRankingState(bool state)
    {
        RankingObject.SetActive(state);
    }

    public void SetNewRecordState(bool state)
    {
        NewRecordObject.SetActive(state);
    }
    public void SetGrayScale()
    {
        

        if (ProfColor.saturation.value > -100 && !IsGrayScale)
        {
            ProfColor.saturation.value -= 2;
            
            if (NatureImages[3].activeSelf && ProfColor.saturation.value == -100)
            {
                SmokeParticle.emissionRate += 2;
                NatureImages[3].SetActive(false);
                TreePulse.PulseCoin();
            }
            else if (NatureImages[2].activeSelf && ProfColor.saturation.value == -74)
            {
                SmokeParticle.emissionRate += 2;
                NatureImages[2].SetActive(false);
                TreePulse.PulseCoin();
            }
            else if (NatureImages[1].activeSelf && ProfColor.saturation.value == -50)
            {
                SmokeParticle.emissionRate += 2;
                NatureImages[1].SetActive(false);
                TreePulse.PulseCoin();
                foreach (DegradableItem item in Degradables)
                {
                    item.SwitchTree(true);

                }
            }
            else if (NatureImages[0].activeSelf && ProfColor.saturation.value == -26)
            {
                SmokeParticle.emissionRate += 2;
                NatureImages[0].SetActive(false);
                TreePulse.PulseCoin();
            }
        }
        else if (ProfColor.saturation.value < 0 && IsGrayScale)
        {
            ProfColor.saturation.value += 2;


             if (!NatureImages[3].activeSelf && ProfColor.saturation.value == -74)
            {
                SmokeParticle.gameObject.SetActive(false);
                GreenParticle.gameObject.SetActive(true);
                NatureImages[3].SetActive(true);
                TreePulse.PulseCoin();
            }
            else if (!NatureImages[2].activeSelf && ProfColor.saturation.value == -50)
            {
                GreenParticle.emissionRate += 2;
                NatureImages[2].SetActive(true);
                TreePulse.PulseCoin();
            }
            else if (!NatureImages[1].activeSelf && ProfColor.saturation.value == -26)
            {
                GreenParticle.emissionRate += 2;
                NatureImages[1].SetActive(true);
                TreePulse.PulseCoin();
            }
            else if (!NatureImages[0].activeSelf && ProfColor.saturation.value == 0)
            {
                GreenParticle.gameObject.SetActive(false);
                NatureImages[0].SetActive(true);
                TreePulse.PulseCoin();
            }

            if (ProfColor.saturation.value == -50 && IsGrayScale && !PlayerPrefs.HasKey("historyplayed"))
            {
                PopupBioma.SetActive(true);
                Time.timeScale = 0;
            }
            else if (ProfColor.saturation.value == 0 && IsGrayScale && !PlayerPrefs.HasKey("historyplayed"))
            {
                    PopupMissionCompleted.SetActive(true);
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
            isTutorial = false;
        }
        //else if (ProfColor.saturation.value == 0 & IsGrayScale)
        //{
        //    IsGrayScale = false;
        //    ChangeCoin(false);
        //    //PopupBitcoin.SetActive(true);
        //    //Time.timeScale = 0;
        //}
    }

    private IEnumerator AnimationChangeTree(float target)
    {
        for (int i = 0; i < NatureImages.Count; i++)
        {
            //while (Vector2.Distance(new Vector2(target, target), 
           // NatureImages[i].gameObject.GetComponent<RectTranform>().scale) < 0.02)
           // {
           //     NatureImages[i].gameObject.GetComponent<RectTranform>().scale.x += 0.01f;
            //    NatureImages[i].gameObject.GetComponent<RectTranform>().scale.y += 0.01f;
           //     NatureImages[i].gameObject.GetComponent<RectTranform>().scale.z += 0.01f;
            //    yield return null;
        //    }
        }
        yield return null;
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
        

        if (CurrProfile.profile.TryGet<ColorAdjustments>(out tempColor))
        {
            ProfColor = tempColor;
        }
        SetGameState();
    }
    public void SetGameState()
    {
        //if (PlayerPrefs.HasKey("historyplayed"))
        //{
        //    Debug.Log("ja jogou historia  " + PlayerPrefs.GetInt("historyplayed"));

        //    if (GetBool("historyplayed"))
        //    {
        //        StartCoroutine(WaitToChange());
        //        Debug.Log("ja jogou historia");
        //    }
        //}
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
        PopupMissionCompleted.SetActive(false);
        Time.timeScale = 1.0f;
    }

}
