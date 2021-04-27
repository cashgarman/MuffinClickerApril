using UnityEngine;
using UnityEngine.EventSystems;

public class CinnamonRoll : MonoBehaviour, IPointerEnterHandler
{
    public float lifespan;
    public float speed;
    public Game game;

    private float timeUntilDestroy;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        // Store the starting time of the fade
        timeUntilDestroy = lifespan;

        // Generate a random direction
        direction = Random.insideUnitCircle;
    }

    // Update is called once per frame
    void Update()
    {
        // Move at some speed
        transform.Translate(direction * speed * Time.deltaTime);

        // When it's fully faded out
        timeUntilDestroy -= Time.deltaTime;
        if (timeUntilDestroy <= 0f)
        {
            // Destroy it
            Destroy(gameObject);
        }
    }

    public void OnRollClicked()
    {
        // Let the game know the roll was clicked
        game.OnRollClicked();

        // Destroy the roll
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnRollClicked();
    }
}