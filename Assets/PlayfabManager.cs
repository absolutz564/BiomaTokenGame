using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{

    public static PlayfabManager instance;
    public string id;
    public string PlayerName;
    public string Email;
    public List<string> LeaderboardNames = new List<string>();
    public List<string> LeaderboardScores = new List<string>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        //Login();
    }


    public void AddContactEmailToPlayer(string email)
    {
        var loginReq = new LoginWithCustomIDRequest
        {
            CustomId = id, // replace with your own Custom ID
            CreateAccount = true // otherwise this will create an account with that ID
        };


        var emailAddress = email; // Set this to your own email


        PlayFabClientAPI.LoginWithCustomID(loginReq, loginRes =>
        {
            Debug.Log("Successfully logged in player with PlayFabId: " + loginRes.PlayFabId);
            AddOrUpdateContactEmail(emailAddress);//Removed the parameter -- "PlayFabId"
        }, FailureCallback);
    }


    void AddOrUpdateContactEmail(string emailAddress)//Removed the parameter -- "PlayFabId"
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            //[Remove it]PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }


    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void Login(string playername, string email)
    {
        PlayerName = playername;
        Email = email;

        id = SystemInfo.deviceUniqueIdentifier + "AS";// + Random.Range(0, 100000);
        var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnError);

    }

    void OnSucess(LoginResult result)
    {
        Debug.Log("Sucessful login/account create!");
        OnUpdatePlayerName(PlayerName);
        AddOrUpdateContactEmail(Email);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging account!");
        Debug.Log(error.GenerateErrorReport());
    }

    //public void OnUpdatePlayerEmail(string email)
    //{
    //    print("Email " + email);
    //    PlayFabClientAPI.AddOrUpdateContactEmail(new UserPrivateAccountInfo
    //    {
    //        Email = email
    //    }, result =>
    //    {

    //        Debug.Log("The player's display name is now: " + result.CustomData.ToString());
    //    });
    //}

    public void OnUpdatePlayerName(string name)
    {
        print("UserDisplayName" + name);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        }, result =>
        {

            Debug.Log("The player's display name is now: " + result.DisplayName);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    public void SendLeaderboard(int score, string name)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    DisplayName = name,
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Sucessfull Leaderboard send");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {

        LeaderboardNames.Clear();
        LeaderboardScores.Clear();
        foreach (var item in result.Leaderboard)
        {
            //Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
            LeaderboardNames.Add(item.DisplayName);
            LeaderboardScores.Add(item.StatValue.ToString());
        }
    }
}
