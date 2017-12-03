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
    private Mission mission;
    public NightTime nightPanel;
    private GameBoard gameBoard;
    private readonly int ADDED_BOXES_PER_LEVEL = 4;

    internal void PlayerReachedMissionPoint()
    {
        if (mission.MissionCompleted())
        {
            missionCounter--;
            NewMission();
        }
    }

    internal void PlayerWentToBed()
    {
        if (missionCounter == 0)
        {
            mission.MissionCompleted();
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
        mission = GetComponent<Mission>();
        gameBoard = GetComponent<GameBoard>();
    }

    internal void StartNextLevel()
    {
        level++;
        missionCounter = missionPerLevel;
        MoveBoxes();
        gameBoard.AddBoxes(ADDED_BOXES_PER_LEVEL);
        ActivatePlayer();
        NewMission();
    }

    private void NewMission()
    {
        //If enabling wait, handle player reaching bed before new mission is given
        //yield return new WaitForSeconds(1);
        if (missionCounter <= 0)
            mission.BedMission();
        else
        {
            mission.NewMission();
        }
    }

    private void ActivatePlayer()
    {
        player.gameObject.SetActive(true);
        player.transform.position = spwanPoint.position;
    }

    //private void SpawnBoxes()
    //{
    //    var emptyTiles = gameBoard.GetEmptyTiles();
    //    Debug.Assert(gameBoard.GetBoxCount() == (--level * ADDED_BOXES_PER_LEVEL));
    //    System.Random rng = new System.Random();

    //    for (int i = 0; i < ADDED_BOXES_PER_LEVEL; i++)
    //    {
    //        var randVal = rng.Next(0, emptyTiles.Count);
    //        gameBoard.AddBox()
    //    }
    //}

    private void MoveBoxes()
    {
    }
}