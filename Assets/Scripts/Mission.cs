using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour {
    public GameObject[] missionPoints;
    public Text text;
    private MissionType missionType = MissionType.None;
    private System.Random rng = new System.Random();

    enum MissionType { North, East, South, West, Bed, None };

    internal void NewMission()
    {
        missionType = (MissionType)rng.Next(0, 4);
        missionPoints[(int)missionType].gameObject.SetActive(true);
        text.text = missionType.ToString();
    }

    internal void MissionCompleted()
    {
        Debug.Assert(missionType < MissionType.None);

        if(missionType != MissionType.Bed)
            missionPoints[(int)missionType].gameObject.SetActive(false);
        missionType = MissionType.None;
        text.text = missionType.ToString();
    }

    internal void BedMission()
    {
        missionType = MissionType.Bed;
        text.text = "I'm sleepy...";
    }
}
