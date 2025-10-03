using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text turnText;
    public GameObject victoryPopup;
    public BoardManager boardManager;
    public QuizManager quizManager;
    public GameObject[] playerPrefabs;
    private Player[] players;
    private int currentPlayerIndex = 0;
    private bool isQuizActive = false;
    public Button diceRollButton;
    private bool isGameOver = false;
    public GameObject extraTurnPanel;
    public Text extraTurnText;
    public Button acceptButton;
    public Button declineButton;
    public Button mainMenuButton;
    public AudioSource backgroundMusic;
    public AudioSource victorySound; // Reference to the victory sound
    public GameObject boardParent; // Reference to the board parent

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        boardManager.InitializeBoardTiles();
        int playerCount = MainMenuManager.playerCount;
        players = new Player[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerObj = Instantiate(playerPrefabs[i % playerPrefabs.Length], boardManager.boardParent);
            Player player = playerObj.GetComponent<Player>();
            player.Initialize(boardManager, quizManager);
            player.playerIndex = i + 1;
            players[i] = player;
            Debug.Log($"Player {i + 1} initial anchoredPosition: {player.GetComponent<RectTransform>().anchoredPosition}");
        }

        acceptButton.onClick.AddListener(AcceptExtraTurn);
        declineButton.onClick.AddListener(DeclineExtraTurn);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        extraTurnPanel.SetActive(false);
        victoryPopup.SetActive(false);

        // Panggil StartNewTurn untuk pemain pertama
        players[currentPlayerIndex].StartNewTurn();
        UpdateTurnText();
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void MoveCurrentPlayer(int steps)
    {
        if (isQuizActive || isGameOver) return;
        players[currentPlayerIndex].MovePlayer(steps);
    }

    public void CheckForWin(Player currentPlayer)
    {
        if (currentPlayer.currentPos == 99)
        {
            ShowVictoryPopup(currentPlayer);
        }
    }

    public void EndTurn()
    {
        if (isGameOver) return;

        Player currentPlayer = GetCurrentPlayer();
        CheckForWin(currentPlayer);

        if (currentPlayer.currentPos == 8 || currentPlayer.currentPos == 29 || currentPlayer.currentPos == 37 || currentPlayer.currentPos == 50 || currentPlayer.currentPos == 60 || currentPlayer.currentPos == 77 || currentPlayer.currentPos == 93)
        {
            ShowExtraTurnPanel();
        }
        else
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        diceRollButton.interactable = true;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

        // Panggil StartNewTurn untuk pemain berikutnya
        players[currentPlayerIndex].StartNewTurn();
        UpdateTurnText();
    }

    public void UpdateTurnText()
    {
        Player currentPlayer = players[currentPlayerIndex];
        turnText.text = $"Player {currentPlayerIndex + 1}'s Turn\nPoints: {currentPlayer.points}";

        switch (currentPlayerIndex)
        {
            case 0:
                SetTurnTextColor("#006AFB"); // Blue
                break;
            case 1:
                SetTurnTextColor("#00FF00"); // Green
                break;
            case 2:
                SetTurnTextColor("#FF000D"); // Red
                break;
            case 3:
                SetTurnTextColor("#FF6600"); // Orange
                break;
        }
    }

    private void SetTurnTextColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
        {
            turnText.color = color;
        }
        else
        {
            Debug.LogWarning($"Invalid color code: {hexColor}");
        }
    }

    private void ShowVictoryPopup(Player winner)
    {
        isGameOver = true;
        victoryPopup.SetActive(true);
        victoryPopup.GetComponentInChildren<Text>().text = "Selamat Player " + winner.playerIndex + " Kamu Menang!";

        // Play the victory sound
        PlayVictorySound();

        // Hide the element
        boardParent.SetActive(false);
        turnText.gameObject.SetActive(false);
        diceRollButton.gameObject.SetActive(false);
    }

    private void PlayVictorySound()
    {
        if (victorySound != null)
        {
            victorySound.Play();
        }
    }

    public void ShowQuiz()
    {
        isQuizActive = true;
        diceRollButton.interactable = false;
        quizManager.ShowQuiz(MainMenuManager.selectedTheme, OnQuizResult);
    }

    public void OnQuizResult(bool isCorrect)
    {
        isQuizActive = false;
        diceRollButton.interactable = true;

        if (isCorrect)
        {
            players[currentPlayerIndex].points += 10;
            UpdateTurnText();
        }

        GetCurrentPlayer().OnQuizResult(isCorrect);
    }

    public void ShowExtraTurnPanel()
    {
        Player currentPlayer = GetCurrentPlayer();
        diceRollButton.interactable = false;
        if (currentPlayer.points >= 10)
        {
            extraTurnText.text = "Anda ingin menggunakan 10 poin untuk giliran tambahan?";
            acceptButton.interactable = true;
        }
        else
        {
            extraTurnText.text = "Poin Anda tidak cukup untuk giliran tambahan.";
            acceptButton.interactable = false;
        }
        extraTurnPanel.SetActive(true);
    }

    public void AcceptExtraTurn()
    {
        GetCurrentPlayer().TradePointsForExtraTurn();
        extraTurnPanel.SetActive(false);
        GrantExtraTurn();
    }

    public void DeclineExtraTurn()
    {
        NextTurn();
        extraTurnPanel.SetActive(false);
        diceRollButton.interactable = true;
    }

    public void GrantExtraTurn()
    {
        players[currentPlayerIndex].StartNewTurn();
        UpdateTurnText();
        diceRollButton.interactable = true;
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to the main menu.");
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    private void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }
}
