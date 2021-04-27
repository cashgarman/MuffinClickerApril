using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Cookie : MonoBehaviour
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

        // Fade the cookie in
        GetComponent<Image>().DOFade(1f, 0.5f);
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
            // GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() =>
            // {
            //     Destroy(gameObject);
            // });
        }
    }

    public void OnCookieClicked()
    {
        // Let the game know the cookie was clicked
        game.OnCookieClicked();
        
        // Destroy the cookie
        Destroy(gameObject);
    }
}
