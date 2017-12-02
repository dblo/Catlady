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
    public int missionTimer;
    private Mission currentMission;

    internal void PlayerReachedMissionPoint()
    {
        currentMission.MissionCompleted();
        missionCounter--;
        //StartCoroutine(NewMission());
        NewMission();
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
    }

    void Awake()
    {
        currentMission = GetComponent<Mission>();
    }

    private void StartNextLevel()
    {
        level++;
        missionCounter = missionPerLevel;
        SpawnBoxes();
        ActivatePlayer();
        //StartCoroutine(NewMission());
        NewMission();
    }

    private void NewMission()
    {
        //If enable wait, handle player reaching bed before new mission is given
        //yield return new WaitForSeconds(1);
        if (missionCounter == 0)
            currentMission.BedMission();
        else
        {
            
            currentMission.NewMission();
            missionTimer = 10;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartNextLevel();
        }
    }
}
