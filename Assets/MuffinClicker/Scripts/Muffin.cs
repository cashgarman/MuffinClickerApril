using System;
using TMPro;
using UnityEngine;

public class Muffin : MonoBehaviour
{
    public Game game;

    public void OnMuffinClicked()
    {
        // Let the game know the muffin was clicked
        game.OnMuffinClicked();
    }
}
