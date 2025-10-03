using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiceRoller : MonoBehaviour
{
    public Sprite[] diceSprites;
    public Image diceImage;
    public GameManager gameManager;
    public AudioSource diceRollSound; // Tambahkan ini

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void RollDice()
    {
        if (diceRollSound != null)
        {
            diceRollSound.Play(); // Mainkan suara di sini
        }

        StartCoroutine(RollDiceAnimation());
    }

    private IEnumerator RollDiceAnimation()
    {
        int rollCount = 10; // Number of times to change the dice face
        float rollSpeed = 0.1f; // Time between each change

        for (int i = 0; i < rollCount; i++)
        {
            int randomIndex = Random.Range(0, diceSprites.Length);
            diceImage.sprite = diceSprites[randomIndex];
            yield return new WaitForSeconds(rollSpeed);
            GameManager.instance.diceRollButton.interactable = false;
        }

        int diceRoll = Random.Range(1, 7);
        diceImage.sprite = diceSprites[diceRoll - 1];

        Player currentPlayer = gameManager.GetCurrentPlayer();
        currentPlayer.MovePlayer(diceRoll); // Move player based on dice roll
    }
}