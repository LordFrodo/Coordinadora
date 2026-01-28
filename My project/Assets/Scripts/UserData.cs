using Firebase.Firestore;

[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Mail { get; set; }

    [FirestoreProperty]
    public int Score { get; set; }
}
