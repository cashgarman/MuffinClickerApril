using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChangingButton : MonoBehaviour
{
    public int numberOfTimesClicked;

    public void OnClicked()
    {
        int number = 1;

        AnotherMethod();

        numberOfTimesClicked += number;
    }

    private void AnotherMethod()
    {
        Debug.Log("Hi!");
    }
}
