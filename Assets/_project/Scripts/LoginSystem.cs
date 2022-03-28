using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoginSystem : MonoBehaviour
{
    public GameUIManager uiManager;
    
    public int passCode = 1234;
    public PlayerInfo currentPlayer;

    private PlayerInfoList playerInfoList;

    void Start()
    {
        playerInfoList = new PlayerInfoList();
        playerInfoList.players = new List<PlayerInfo>();
        GetPlayers();
    }

    private void GetPlayers()
    {
        string json = PlayerPrefs.GetString("players", "");
        if (json.Length > 1)
            playerInfoList = JsonUtility.FromJson<PlayerInfoList>(json);
    }

    private void SavePlayer(PlayerInfo player)
    {
        playerInfoList.players.Add(player);
        string json = JsonUtility.ToJson(playerInfoList, true);
        PlayerPrefs.SetString("players", json);
    }
    
    public bool LogIn(PlayerInfo player)
    {
        PlayerInfo foundPlayer = playerInfoList.players.Find(info => info.firstName == player.firstName && info.lastName == player.lastName);
        if (foundPlayer == null)
        {
            SavePlayer(player);
            currentPlayer = player;
            GameManager.Instance.Login();
            return true;
        }
        currentPlayer = foundPlayer;
        return foundPlayer.attempts >= 1;
    }

    public void ReduceAttempt()
    {
        playerInfoList.players.Find(info => info.firstName == currentPlayer.firstName && info.lastName == currentPlayer.lastName).attempts--;
        string json = JsonUtility.ToJson(playerInfoList, true);
        PlayerPrefs.SetString("players", json);
    }
    public void LogOut()
    {
        // open login screen
    }
    public bool CheckPass(int pass)
    {
        return pass == passCode;
    }
    public bool IsPassValid()
    {
        return uiManager.passField.text.Length > 0 && CheckPass(int.Parse(uiManager.passField.text));
    }
    public bool CheckNamesFilled()
    {
        PlayerInfo player = new PlayerInfo(uiManager.firstNameField.text, uiManager.lastNameField.text);
        if (uiManager.firstNameField.text.Length > 0 && uiManager.lastNameField.text.Length > 0 && LogIn(player))
            return true;
        uiManager.ShowNotification("Name not filled or out of attempts!");
        return false;
    }
}
[Serializable]
public class PlayerInfoList
{
    public List<PlayerInfo> players;
}
[Serializable]
public class PlayerInfo
{
    public string firstName;
    public string lastName;
    public int attempts;

    public PlayerInfo(string _firstName, string _lastName, int _attempts)
    {
        firstName = _firstName;
        lastName = _lastName;
        attempts = _attempts;
    }
    public PlayerInfo(string _firstName, string _lastName)
    {
        firstName = _firstName;
        lastName = _lastName;
        attempts = 3;
    }
}