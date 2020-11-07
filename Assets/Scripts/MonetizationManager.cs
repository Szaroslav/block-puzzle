using UnityEngine;
using UnityEngine.Monetization;
using System.Collections;

public class MonetizationManager : MonoBehaviour
{
    public static MonetizationManager ins;

    public bool testMode = false;
    public string placementId = "rewardedVideo";

    [HideInInspector]
    public bool waitingForAd = false;

#if UNITY_ANDROID
    private string gameId = "3064621";
#elif UNITY_IOS
    private string gameId = "3064620";
#endif

    public static bool CheckNetworkConnection()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }

    public void ShowAd()
    {
        if (!waitingForAd)
        {
            waitingForAd = true;
            StartCoroutine(WaitForAd());
        }
    }

    private void Start()
	{
        if (!ins)
            ins = this;

        if (Monetization.isSupported)
            Monetization.Initialize(gameId, testMode);
    }

    private IEnumerator WaitForAd()
    {
        while (!Monetization.IsReady(placementId))
            yield return null;

        ShowAdPlacementContent ad;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if (ad != null)
            ad.Show(OnAdFinish);
    }

    private void OnAdFinish(ShowResult result)
    {
        if (result == ShowResult.Finished)
            GameManager.ins.ContinueGame();

        waitingForAd = false;
    }
}
