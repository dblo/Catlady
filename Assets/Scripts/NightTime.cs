using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightTime : MonoBehaviour {
    public GameManager gm;
    public Text levelText;
    public Text continueText;

    internal void Show(int currentlevel)
    {
        int newLevel = currentlevel++;
        levelText.text = "Level " + newLevel.ToString();
        StartCoroutine(ShowContinueDelayed());
    }

    private IEnumerator ShowContinueDelayed()
    {
        yield return new WaitForSeconds(1);
        continueText.gameObject.SetActive(true);
    }

    void Update () {
		if(continueText.isActiveAndEnabled && Input.anyKeyDown)
        {
            gm.StartNextLevel();
            continueText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
