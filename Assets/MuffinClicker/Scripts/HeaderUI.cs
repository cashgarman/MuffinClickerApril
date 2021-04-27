using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public Game game;
    public TMP_Text numberOfMuffinsText;
    public TMP_Text numberOfMuffinsPerSecondText;

    void Update()
    {
        // Make sure the pularization of 'muffins' is correct
        if (game.numberOfMuffins == 1)
        {
            numberOfMuffinsText.text = game.numberOfMuffins.ToString() + " muffin";
        }
        else
        {
            numberOfMuffinsText.text = game.numberOfMuffins.ToString() + " muffins";
        }

        // Update the number of muffins person second
        numberOfMuffinsPerSecondText.text = $"{game.muffinsPerSecond} {(game.muffinsPerSecond == 1 ? " muffin" : " muffins")} / sec";
    }
}
