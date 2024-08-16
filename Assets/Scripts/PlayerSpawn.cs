using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawn : NetworkBehaviour
{
    public int playerID = 0;

    [SerializeField] Vector3 startPositionP1;
    [SerializeField] Vector3 startPositionP2;

    private void Start()
    {
        // Search for players by tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Assign player ID based on number of players
        playerID = players.Length;

        // Set player position based on player ID
        if (playerID == 1)
        {
            transform.position = startPositionP1;
        }
        else if (playerID == 2)
        {
            transform.position = startPositionP2;
        }
    }
}