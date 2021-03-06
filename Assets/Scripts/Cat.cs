﻿using System;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private GameBoard board;
    private Vector2 destination;
    public float speed = 0.02f;
    private readonly System.Random rng = new System.Random();
    private float moveBoxCooldown;
    public float mischiefCD;
    public AudioClip[] catClips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        board = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameBoard>();
        GetNewDestination();
        moveBoxCooldown = mischiefCD;
    }

    private void GetNewDestination()
    {
        destination = board.GetRandomWorldCoordWithinOuterWalls();
    }

    void Update()
    {
        if (Vector2.Distance(destination, transform.position) < 0.1f)
        {
            GetNewDestination();
        }
        transform.position = Vector2.MoveTowards(transform.position, destination, speed);

        if (moveBoxCooldown > 0)
        {
            moveBoxCooldown -= Time.deltaTime;
            if (moveBoxCooldown <= 0)
            {
                TryMoveBox();
                moveBoxCooldown = mischiefCD;
            }
        }
    }

    private bool TryMoveBox()
    {
        if (rng.Next(1) == 0)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            if(board.TryMoveNearbyBox(x, y))
            {
                PlaySounds();
            }
        }
        return false;
    }

    private void PlaySounds()
    {
        audioSource.clip = catClips[rng.Next(catClips.Length)];
        audioSource.Play();
    }
}
