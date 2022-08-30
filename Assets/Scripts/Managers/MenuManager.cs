using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.PlayerTurn);
    }
}
