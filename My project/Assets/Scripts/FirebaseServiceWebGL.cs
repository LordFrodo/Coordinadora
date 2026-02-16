using System.Runtime.InteropServices;

public class FirebaseServiceWebGL : IFirebaseService
{
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void Firestore_StartTopScoresListener(string collection, string limit, string obj, string callback, string error);

    [DllImport("__Internal")]
    private static extern void Firestore_StopTopScoresListener();

#endif

    private const string COLLECTION = "users";

    public void UploadScore(UserData data)
    {
        // Ya lo tienes implementado
    }

    public void GetTopScores(int limit)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Firestore_StartTopScoresListener(COLLECTION, limit.ToString(), "FireBaseManager", "OnScoresReceived", "OnScoresError");
#endif
    }

    public void StopListening()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Firestore_StopTopScoresListener();
#endif
    }
}
