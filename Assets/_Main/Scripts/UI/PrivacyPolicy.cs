﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PrivacyPolicy : MonoBehaviour
{
    public string privacyPolicyLink;
    public string privacyPolicyAcceptedKey = "privacy_policy_accepted";

    [Space(12)]

    public Menu menu;

    CanvasGroup cg;
    
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();

        if (PlayerPrefs.HasKey(privacyPolicyAcceptedKey))
        {
            cg.alpha = 0;
            cg.interactable = cg.blocksRaycasts = false;

            menu.In();
        }
        else
        {
            cg.alpha = 1;
            cg.interactable = cg.blocksRaycasts = true; 
        }

        StartCoroutine(WaitForRebuild());
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyLink);
    }

    public void OnAccept()
    {
        PlayerPrefs.SetInt(privacyPolicyAcceptedKey, 1);
        cg.interactable = cg.blocksRaycasts = false;

        LeanTween.value(gameObject, v => { cg.alpha = v; }, 1, 0, 0.33f)
            .setEaseInOutCubic()
            .setIgnoreTimeScale(true)
            .setOnComplete(() => { menu.In(); });
    }

    IEnumerator WaitForRebuild()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        LayoutRebuilder.MarkLayoutForRebuild(transform.GetChild(0).GetComponent<RectTransform>());
    }
}
