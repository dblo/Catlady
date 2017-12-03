using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {
    public GameManager gm;
    public Text resultText;
    public Text continueText;

    internal void Show(int day, float time)
    {
        var ts = TimeSpan.FromSeconds(time);
        var timeString = "Reached day " + day + " in ";
        timeString += ts.Hours + ":";
        timeString += ts.Minutes + ":";
        timeString += ts.Seconds + ".";
        timeString += ts.Milliseconds / 10;

        resultText.text = timeString;
        StartCoroutine(ShowContinueDelayed());
    }

    private IEnumerator ShowContinueDelayed()
    {
        yield return new WaitForSeconds(1);
        continueText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (continueText.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("level 1");
            }
        }
    }
}
