using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    [Header("General")]
    [SerializeField] bool isGameActive;
    [SerializeField] Chat chat;

    [Header("Ball")]
    [SerializeField] float ballSpeed;
    [SerializeField] float ballSpeedIncrement;

    [Header("Players")]
    [SerializeField] bool isPlayer1Ready;
    [SerializeField] bool isPlayer2Ready;
    public NetworkVariable<int> player1Score = new(0);
    public NetworkVariable<int> player2Score = new(0);

    [Header("Paddles")]
    [SerializeField] GameObject paddleLeft;
    [SerializeField] GameObject paddleRight;
    [SerializeField] Vector3 paddleLeftStartPosition;
    [SerializeField] Vector3 paddleRightStartPosition;

    // Reference to text objects for displaying scores
    [SerializeField] TMPro.TextMeshProUGUI player1ScoreText;
    [SerializeField] TMPro.TextMeshProUGUI player2ScoreText;

    public void PlayerReadyStatusChanged(bool isPlayer1, bool isReady)
    {
        if (isPlayer1)
        {
            isPlayer1Ready = isReady;
            Debug.Log("Player 1 ready status changed: " + isReady);
        }
        else
        {
            isPlayer2Ready = isReady;
            Debug.Log("Player 2 ready status changed: " + isReady);
        }

        CheckIfBothPlayersReady();
    }

    private void CheckIfBothPlayersReady()
    {
        if (isPlayer1Ready && isPlayer2Ready && !isGameActive)
        {
            // Both players are ready, start the game
            StartGame();
        }
    }

    private void StartGame()
    {
        isGameActive = true;
        Debug.Log("Both players are ready! Starting the game...");
        paddleLeft.transform.position = paddleLeftStartPosition;
        paddleRight.transform.position = paddleRightStartPosition;
        chat.SubmitMessageRPC("Game started!");

        // Start the ball movement
        ballSpeed = 5f;
    }

    private void Start()
    {
        // Attach event listeners for score changes to update the UI
        player1Score.OnValueChanged += UpdatePlayer1ScoreUI;
        player2Score.OnValueChanged += UpdatePlayer2ScoreUI;
    }

    private void Update()
    {
        if (isGameActive)
        {
            // Move the ball
            transform.position += ballSpeed * Time.deltaTime * transform.right;
        }
    }

    private void ResetBallPosition()
    {
        transform.position = Vector3.zero;
        ballSpeed = 5f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isGameActive)
        {
            if (other.collider.CompareTag("Wall"))
            {
                Debug.Log("Wall Hit!");
                transform.right = Vector3.Reflect(transform.right, Vector3.up);
                ballSpeed += ballSpeedIncrement;
                return;
            }

            if (other.collider.CompareTag("WinWall1"))
            {
                player2Score.Value++;
                chat.SubmitMessageRPC("Player 2 scored!");

                if (player2Score.Value >= 1)
                {
                    chat.SubmitMessageRPC("Player 2 wins!");
                    isGameActive = false;
                }
            }

            if (other.collider.CompareTag("WinWall2"))
            {
                player1Score.Value++;
                chat.SubmitMessageRPC("Player 1 scored!");

                if (player1Score.Value >= 1)
                {
                    chat.SubmitMessageRPC("Player 1 wins!");
                    isGameActive = false;
                }
            }

            Debug.Log("Player Hit!");

            // Determine which side the player is on 
            if (other.transform.position.z < transform.position.z)
            {
                transform.right = Vector3.Reflect(transform.right, Vector3.forward);
            }
            else
            {
                transform.right = Vector3.Reflect(transform.right, Vector3.back);
            }

            // Randomness to prevent predictable bounces
            float randomAngle = Random.Range(-5f, 5f);
            transform.Rotate(0, randomAngle, 0);

            ballSpeed += ballSpeedIncrement;
        }
    }

    private void UpdatePlayer1ScoreUI(int previousValue, int newValue)
    {
        player1ScoreText.text = newValue.ToString();
    }

    private void UpdatePlayer2ScoreUI(int previousValue, int newValue)
    {
        player2ScoreText.text = newValue.ToString(); 
    }
}
