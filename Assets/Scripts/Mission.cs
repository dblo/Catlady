using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public GameObject[] missionPoints;
    public Text missionText;
    public Text timerText;
    public int missionTime;
    private MissionType currentMission = MissionType.None;
    private System.Random rng = new System.Random();
    private float timer = -1;
    private int currentTimerInt;

    enum MissionType { North, East, South, West, Bed, None };

    internal void NewMission()
    {
        int newMission = GetDifferentMission();
        Debug.Assert(newMission != (int)currentMission);

        currentMission = (MissionType) newMission;
        missionPoints[(int)currentMission].gameObject.SetActive(true);
        missionText.text = currentMission.ToString();

        timer = missionTime;
        currentTimerInt = (int)(timer);
        timerText.text = currentTimerInt.ToString();
    }

    private int GetDifferentMission()
    {
        List<int> missions = new List<int>() { 0, 1, 2, 3 };
        missions.Remove((int)currentMission);
        return missions[rng.Next(0, missions.Count)];
    }

    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                MissionFailed();
                NewMission();
            }

            if ((int)timer < currentTimerInt)
            {
                timerText.text = currentTimerInt.ToString();
                currentTimerInt--;
            }
        }
    }

    private void MissionFailed()
    {
        Debug.Assert(currentMission < MissionType.None);

        if (currentMission != MissionType.Bed)
            missionPoints[(int)currentMission].gameObject.SetActive(false);
        missionText.text = "";
    }

    internal bool MissionCompleted()
    {
        Debug.Assert(currentMission < MissionType.None);

        if (currentMission != MissionType.Bed)
            missionPoints[(int)currentMission].gameObject.SetActive(false);
        currentMission = MissionType.None;
        missionText.text = currentMission.ToString();

        if (timer <= 0)
            return false;

        timer = -1; // Freeze timer
        return true;
    }

    internal void BedMission()
    {
        currentMission = MissionType.Bed;
        missionText.text = "I'm sleepy...";
    }
}
