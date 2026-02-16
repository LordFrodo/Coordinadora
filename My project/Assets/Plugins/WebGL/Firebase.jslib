mergeInto(LibraryManager.library, {

    Firestore_SetDocument: function (collectionPtr, docIdPtr, jsonPtr, objectNamePtr, callbackPtr, errorPtr)
    {
        var collection = UTF8ToString(collectionPtr);
        var docId = UTF8ToString(docIdPtr);
        var json = UTF8ToString(jsonPtr);
        var objectName = UTF8ToString(objectNamePtr);
        var callback = UTF8ToString(callbackPtr);
        var errorCallback = UTF8ToString(errorPtr);

        firebase.firestore()
            .collection(collection)
            .doc(docId)
            .set(JSON.parse(json), { merge: true })
            .then(function () {
                unityInstance.SendMessage(objectName, callback, "success");
            })
            .catch(function (error) {
                unityInstance.SendMessage(objectName, errorCallback, error.message);
            });
    },

    Firestore_GetTopScores: function (collectionPtr, limitPtr, objectNamePtr, callbackPtr, errorPtr)
    {
        var collection = UTF8ToString(collectionPtr);
        var limit = parseInt(UTF8ToString(limitPtr));
        var objectName = UTF8ToString(objectNamePtr);
        var callback = UTF8ToString(callbackPtr);
        var errorCallback = UTF8ToString(errorPtr);

        firebase.firestore()
            .collection(collection)
            .orderBy("Score", "desc")
            .limit(limit)
            .get()
            .then(function (querySnapshot) {

                var results = [];

                querySnapshot.forEach(function (doc) {
                    results.push(doc.data());
                });

                unityInstance.SendMessage(objectName, callback, JSON.stringify(results));
            })
            .catch(function (error) {
                unityInstance.SendMessage(objectName, errorCallback, error.message);
            });
    },
       Firestore_StartTopScoresListener: function (collectionPtr, limitPtr, objectNamePtr, callbackPtr, errorPtr)
    {
        var collection = UTF8ToString(collectionPtr);
        var limit = parseInt(UTF8ToString(limitPtr));
        var objectName = UTF8ToString(objectNamePtr);
        var callback = UTF8ToString(callbackPtr);
        var errorCallback = UTF8ToString(errorPtr);

        try {

            window.topScoreUnsubscribe = firebase.firestore()
                .collection(collection)
                .orderBy("Score", "desc")
                .limit(limit)
                .onSnapshot(function (snapshot) {

                    var results = [];

                    snapshot.forEach(function (doc) {
                        results.push(doc.data());
                    });

                    unityInstance.SendMessage(objectName, callback, JSON.stringify(results));
                },
                function (error) {
                    unityInstance.SendMessage(objectName, errorCallback, error.message);
                });

        } catch (error) {
            unityInstance.SendMessage(objectName, errorCallback, error.message);
        }
    },

    Firestore_StopTopScoresListener: function ()
    {
        if (window.topScoreUnsubscribe != null)
        {
            window.topScoreUnsubscribe();
            window.topScoreUnsubscribe = null;
        }
    }
});
