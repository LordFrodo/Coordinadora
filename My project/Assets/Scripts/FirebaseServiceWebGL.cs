#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

using UnityEngine;

public class FirebaseServiceWebGL : IFirebaseService
{
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void Firestore_SetDocument(string collection, string docId, string json, string objectName, string callback, string error);

    [DllImport("__Internal")]
    private static extern void Firestore_GetTopScores(string collection, string limit, string objectName, string callback, string error);

    [DllImport("__Internal")]
    private static extern void Firestore_StartTopScoresListener(string collection, string limit, string objectName, string callback, string error);

    [DllImport("__Internal")]
    private static extern void Firestore_StopTopScoresListener();

#endif

    private string collectionName = "Scores";

    public void UploadScore(UserData data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR

    string json = JsonUtility.ToJson(data);

    string randomId = System.Guid.NewGuid().ToString();

    Firestore_SetDocument(
        collectionName,
        randomId,
        json,
        FireBaseManager.Instance.gameObject.name,
        "OnUploadSuccess",
        "OnUploadError"
    );

#endif
    }

    public void GetTopScores(int limit)
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        Firestore_StartTopScoresListener(
            collectionName,
            limit.ToString(),
            FireBaseManager.Instance.gameObject.name,
            "OnScoresReceived",
            "OnScoresError"
        );

#endif
    }

    public void StopListening()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Firestore_StopTopScoresListener();
#endif
    }
}
