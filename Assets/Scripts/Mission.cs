using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public GameObject[] missionPoints;
    public Text missionText;
    public Text timerText;
    private MissionType currentMission = MissionType.None;
    private System.Random rng = new System.Random();
    private float timer = -1;
    private int currentTimerInt;
    private bool missionOngoing = false;
    private GameManager gm;
    public Player player;

    private string[] familyOccurances =
    {
        "What's the fuss over at @'s house?",
        "So annoying.. I bet those @ kids are causing the ruckus!",
        "Is that the lunatic Mr. @ walking by?",
        "Have the @s put up that fence yet?",
        "Was that the @'s car I heard?",
    };

    private string[] directionOccurances =
    {
          "I thought I heard a strange noice to the @.",
          "Was that a dog barking near to the @?",
    };


    enum MissionType { North, East, South, West, Bed, None };

    private void Awake()
    {
        gm = GetComponent<GameManager>();
    }

    private string GetMissionText()
    {
        string msg;
        if (rng.Next(1) == 0)
        {
            msg = familyOccurances[rng.Next(familyOccurances.Length)];
            string family = DirectionToFamily(currentMission);
            msg = msg.Replace("@", family);
        }
        else
        {
            msg = directionOccurances[rng.Next(directionOccurances.Length)];
            msg = msg.Replace("@", currentMission.ToString().ToLower());
        }
        return msg;
    }

    string DirectionToFamily(MissionType type)
    {
        switch (type)
        {
            case MissionType.North:
                return "Northman";
            case MissionType.East:
                return "Eastface";
            case MissionType.South:
                return "Southerland";
            case MissionType.West:
                return "Westfoot";
        }
        return "";
    }

    internal void NewMission()
    {
        int newMission = GetDifferentMission();
        Debug.Assert(newMission != (int)currentMission);

        currentMission = (MissionType)newMission;
        missionPoints[(int)currentMission].gameObject.SetActive(true);
        missionText.text = GetMissionText();

        timer = gm.GetMissionTime();
        currentTimerInt = (int)(timer);
        timerText.text = currentTimerInt.ToString();
        missionOngoing = true;
    }

    internal void BedMission()
    {
        currentMission = MissionType.Bed;
        missionText.text = "I'm sleepy...";

        timer = gm.GetMissionTime();
        currentTimerInt = (int)(timer);
        timerText.text = currentTimerInt.ToString();
        missionOngoing = true;
    }

    private int GetDifferentMission()
    {
        List<int> missions = new List<int>() { 0, 1, 2, 3 };
        missions.Remove((int)currentMission);
        return missions[rng.Next(0, missions.Count)];
    }

    private void Update()
    {
        if (timer >= 0 && missionOngoing)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                MissionFailed();

                if (gm.TimerExpired())
                {
                    if (currentMission == MissionType.Bed)
                        BedMission();
                    else
                        NewMission();
                }
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
        missionOngoing = false;
    }

    internal bool MissionCompleted()
    {
        Debug.Assert(currentMission < MissionType.None);

        if (!missionOngoing)
            return false; // Reached missionLocation same frame as timer expired

        if (currentMission != MissionType.Bed)
            missionPoints[(int)currentMission].gameObject.SetActive(false);
        missionText.text = currentMission.ToString();
        missionOngoing = false;
        return true;
    }
}
