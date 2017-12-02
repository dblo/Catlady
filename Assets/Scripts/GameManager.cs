using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level = 0;
    public Player player;
    public Transform spwanPoint;
    private int missionCounter = 0;
    public int missionPerLevel = 2;
    private Mission currentMission;
    public NightTime nightPanel;
    
    internal void PlayerReachedMissionPoint()
    {
        if (currentMission.MissionCompleted())
        {
            missionCounter--;
            NewMission();
        }
    }

    internal void PlayerWentToBed()
    {
        if (missionCounter == 0)
        {
            currentMission.MissionCompleted();
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        player.gameObject.SetActive(false);
        SleepScreen();
    }

    private void SleepScreen()
    {
        nightPanel.gameObject.SetActive(true);
        nightPanel.Show(level);
    }

    void Start()
    {
        SleepScreen();
    }

    void Awake()
    {
        currentMission = GetComponent<Mission>();
    }

    internal void StartNextLevel()
    {
        level++;
        missionCounter = missionPerLevel;
        SpawnBoxes();
        ActivatePlayer();
        NewMission();
    }

    internal Vector2 GetTileGridPosNearest(Vector3 vector3)
    {
        var res = new Vector2();



        return res;
    }

    private void NewMission()
    {
        //If enabling wait, handle player reaching bed before new mission is given
        //yield return new WaitForSeconds(1);
        if (missionCounter == 0)
            currentMission.BedMission();
        else
        {
            currentMission.NewMission();
        }
    }

    private void ActivatePlayer()
    {
        player.gameObject.SetActive(true);
        player.transform.position = spwanPoint.position;
    }

    private void SpawnBoxes()
    {

    }
}