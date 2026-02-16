using UnityEngine;
using System.Collections.Generic;
using Proyecto26;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager Instance;

    private IFirebaseService service;

    public delegate void ScoreboardUpdated(List<UserData> users);
    public event ScoreboardUpdated OnScoreboardUpdated;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

#if UNITY_WEBGL && !UNITY_EDITOR
        service = new FirebaseServiceWebGL();
#else
        service = new FirebaseServiceNative();
#endif
    }

    public void StartListeningScoreboard()
    {
        service.GetTopScores(8);
    }

    private void OnDestroy()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    (service as FirebaseServiceWebGL)?.StopListening();
#endif
    }

    public void UploadUserScore(UserData data)
    {
        service.UploadScore(data);
    }

    // 🔹 CALLBACKS DESDE JS
    public void OnScoresReceived(string json)
    {
        UserData[] array = JsonHelper2  .FromJson<UserData>(json);
        List<UserData> users = new List<UserData>(array);

        OnScoreboardUpdated?.Invoke(users);
    }
    public void InvokeScoreUpdate(List<UserData> users)
    {
        OnScoreboardUpdated?.Invoke(users);
    }

    public void OnScoresError(string error)
    {
        Debug.LogError(error);
    }

    public void OnUploadSuccess(string msg)
    {
        Debug.Log("Upload success");
    }

    public void OnUploadError(string error)
    {
        Debug.LogError(error);
    }
}
    