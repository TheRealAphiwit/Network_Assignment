using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PaddlePossesion : MonoBehaviour
{
    // Paddles
    [SerializeField] private GameObject paddles;

    // Start is called before the first frame update
    void Start()
    {
        // Search for paddleholder using tag
        paddles = GameObject.FindGameObjectWithTag("PaddleHolder");

        // Search for available paddle
        foreach (Transform paddle in paddles.transform)
        {
            // If the paddle is not possessed
            if (!paddle.GetComponent<PaddleStatus>().isPossessed)
            {
                // Possess the paddle
                paddle.GetComponent<PaddleStatus>().isPossessed = true;
                paddle.SetParent(transform);
                paddle.GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
                break;
            }
        }
    }
}
