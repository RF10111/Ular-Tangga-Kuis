using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private BoardManager boardManager;
    private QuizManager quizManager;
    public int currentPos = 0;
    private bool isQuizActive = false;
    private int targetPos;
    public int playerIndex;
    public float moveSpeed = 4f;
    private RectTransform rectTransform;
    public int points = 0;
    private bool turnEnded = false;

    public void Initialize(BoardManager boardManager, QuizManager quizManager)
    {
        this.boardManager = boardManager;
        this.quizManager = quizManager;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = boardManager.boardTiles[0].anchoredPosition;
        Debug.Log("Initial Player Position: " + rectTransform.anchoredPosition);
    }

    public void MovePlayer(int steps)
    {
        if (isQuizActive || GameManager.instance.IsGameOver() || turnEnded) return;

        targetPos = currentPos + steps;
        Debug.Log("Moving player " + playerIndex + " from position " + currentPos + " to " + targetPos);
        if (targetPos >= boardManager.boardTiles.Length)
        {
            StartCoroutine(MoveToTile99AndThenBackward(targetPos));
        }
        else
        {
            GameManager.instance.diceRollButton.interactable = false;
            StartCoroutine(MoveToTargetPosition(targetPos));
        }
    }

    private IEnumerator MoveToTile99AndThenBackward(int targetPosition)
    {
        Debug.Log("Moving to tile 99 then backward for player " + playerIndex);
        yield return StartCoroutine(MoveToTargetPosition(98));
        int overshoot = targetPosition - 98;
        int backwardPosition = 100 - overshoot;
        yield return StartCoroutine(MoveToTargetPosition(backwardPosition));

        // Setelah gerakan selesai, pastikan turn berakhir
        if (!turnEnded)
        {
            turnEnded = true;
            GameManager.instance.EndTurn();
        }
    }

    private IEnumerator MoveToTargetPosition(int targetPosition)
    {
        Debug.Log("Start moving to target position: " + targetPosition + " for player " + playerIndex);
        while (currentPos != targetPosition)
        {
            int direction = currentPos < targetPosition ? 1 : -1;
            currentPos += direction;
            Vector3 startPos = rectTransform.anchoredPosition;
            Vector3 endPos = boardManager.boardTiles[currentPos].anchoredPosition;
            float journey = 0f;

            while (journey <= 1f)
            {
                journey += Time.deltaTime * moveSpeed;
                rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, journey);
                yield return null;
            }

            rectTransform.anchoredPosition = endPos;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Reached position: " + currentPos + " for player " + playerIndex);

        int newPosition = boardManager.CheckSnakeOrLadder(currentPos);
        if (newPosition != currentPos)
        {
            isQuizActive = true;
            GameManager.instance.diceRollButton.interactable = false;
            quizManager.ShowQuiz(MainMenuManager.selectedTheme, OnQuizResult);
        }
        else if (!turnEnded) // Pastikan turn hanya berakhir sekali
        {
            turnEnded = true;
            GameManager.instance.EndTurn();
        }
    }

    public void OnQuizResult(bool isCorrect)
    {
        isQuizActive = false;
        quizManager.HideQuizPanel();
        GameManager.instance.diceRollButton.interactable = true;

        Debug.Log("Quiz result for player " + playerIndex + ": " + (isCorrect ? "Correct" : "Incorrect"));

        if (isCorrect)
        {
            points += 10;
            GameManager.instance.UpdateTurnText();

            if (boardManager.snakes.ContainsKey(currentPos))
            {
                if (!turnEnded)
                {
                    turnEnded = true;
                    GameManager.instance.EndTurn();
                }
            }
            else if (boardManager.ladders.ContainsKey(currentPos))
            {
                int ladderTop = boardManager.ladders[currentPos];
                StartCoroutine(MoveDiagonallyToPosition(currentPos, ladderTop));
            }
        }
        else
        {
            if (boardManager.snakes.ContainsKey(currentPos))
            {
                int snakeTail = boardManager.snakes[currentPos];
                StartCoroutine(MoveDiagonallyToPosition(currentPos, snakeTail));
            }
            else if (boardManager.ladders.ContainsKey(currentPos))
            {
                if (!turnEnded)
                {
                    turnEnded = true;
                    GameManager.instance.EndTurn();
                }
            }
        }
    }

    private IEnumerator MoveDiagonallyToPosition(int startPos, int endPos)
    {
        Vector3 start = boardManager.boardTiles[startPos].anchoredPosition;
        Vector3 end = boardManager.boardTiles[endPos].anchoredPosition;
        float journey = 0f;

        Debug.Log("Moving diagonally from position " + startPos + " to " + endPos + " for player " + playerIndex);

        while (journey <= 1f)
        {
            journey += Time.deltaTime * moveSpeed;
            rectTransform.anchoredPosition = Vector3.Lerp(start, end, journey);
            yield return null;
        }

        currentPos = endPos;
        rectTransform.anchoredPosition = end;
        Debug.Log("Moved diagonally to position: " + currentPos + " for player " + playerIndex);

        if (!turnEnded) // Pastikan turn hanya berakhir sekali
        {
            turnEnded = true;
            GameManager.instance.EndTurn();
        }
    }


    public void TradePointsForExtraTurn()
    {
        if (points >= 10)
        {
            points -= 10;
            turnEnded = false;
            GameManager.instance.UpdateTurnText();
            GameManager.instance.GrantExtraTurn();
            Debug.Log("Player " + playerIndex + " traded points for an extra turn");
        }
        else
        {
            Debug.Log("Not enough points for extra turn for player " + playerIndex);
        }
    }
    public void StartNewTurn()
    {
        turnEnded = false;
    }
}