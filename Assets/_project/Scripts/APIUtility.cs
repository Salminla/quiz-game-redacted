using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class APIUtility : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    #region Not supported by WebGL
    public static async Task PostScores(Score score)
    {
        
        string json = JsonUtility.ToJson(score);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        //var response = await client.PostAsync("", content);
        var response = await client.PostAsync("http://localhost:3000/api/score", content);

        var responseString = await response.Content.ReadAsStringAsync();

        Debug.Log(responseString);
        
    }
    public static async Task PostMail(Score score)
    {
        string json = JsonUtility.ToJson(score);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        //var response = await client.PostAsync("", content);
        var response = await client.PostAsync("http://localhost:3000/api/mail", content);

        var responseString = await response.Content.ReadAsStringAsync();

        Debug.Log(responseString);
    }
    #endregion
    
    public void PostScoresUnity(Score score)
    {
        string json = JsonUtility.ToJson(score);
        StartCoroutine(UploadScore(json));
    }
    public void PostMailUnity(Score score)
    {
        string json = JsonUtility.ToJson(score);
        StartCoroutine(UploadMail(json));
    }
    
    static IEnumerator UploadScore(string json)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("", "POST");
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
    static IEnumerator UploadMail(string json)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("", "POST");
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
    /*
    [ContextMenu("Test post")]
    public async Task TestPostScore()
    {
        var score = new Score("Testi Teppo", 34);
        await PostScores(score);
    }
    [ContextMenu("Test mail")]
    public async Task TestPostMail()
    {
        var score = new Score("Testi Teppo", 34);
        await PostMail(score);
    }
    [ContextMenu("Test post Unity")]
    public void TestPostScoreUnity()
    {
        var score = new Score("Testi Teppo", 34);
        PostScoresUnity(score);
    }
    [ContextMenu("Test mail Unity")]
    public void TestPostMailUnity()
    {
        var score = new Score("Testi Teppo", 34);
        PostMailUnity(score);
    }
    */
}

public class Score
{
    public string name;
    public int score;

    public Score(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
