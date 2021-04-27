using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class FloatingText : MonoBehaviour
{
    public float speed;
    public float fadeDuration;

    private TMP_Text label;
    private Color initialColour;
    private float fadeStart;
    public Game game;
    private Vector3 direction;

    private void Start()
    {
        // Grab the text component
        label = GetComponent<TMP_Text>();

        // Grab the initial colour
        initialColour = label.color;

        // Store the starting time of the fade
        fadeStart = Time.time;

        direction = Random.insideUnitCircle;
    }

    void Update()
    {
        // Move upwards at some speed
        //transform.Translate(0, speed * Time.deltaTime, 0f);
        transform.Translate(direction * speed * Time.deltaTime);

        // Fade out over the lifespan of the floating text
        float timeSinceFade = Time.time - fadeStart;
        label.color = Color.Lerp(initialColour, Color.clear, timeSinceFade / fadeDuration);

        // When it's fully faded out
        if(timeSinceFade > fadeDuration)
        {
            // Destroy it
            Destroy(gameObject);
        }
    }

    public void SetText(string message)
    {
        // Set the text of the text
        GetComponent<TMP_Text>().text = message;
    }
}
