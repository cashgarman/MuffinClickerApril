using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public int numberOfMuffins;
    public int muffinsPerClick = 1;
    public int muffinsPerSecond;
    public float passiveMuffinInterval = 1f;

    public FloatingText floatingTextPrefab;
    public Transform floatingTextContainer;
    private float timeUntilNextCookie;
    private float timeUntilNextMuffin;
    public Transform cookieContainer;
    public Cookie cookiePrefab;
    public UpgradeUI upgrade1;
    public UpgradeUI upgrade2;
    public UpgradeUI upgrade3;
    public int numCinnamonRollsPerLevel = 10;
    public CinnamonRoll cinnamonRollPrefab;
    [SerializeField] private MusicController _music;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _muffinClickSound;
    [SerializeField] private GameObject _littleMuffinPrefab;

    private void OnApplicationFocus(bool status)
    {
        Debug.Log($"Focus: {status}");
    }

    private void Start()
    {
        // Load the game
        LoadGame();

        timeUntilNextCookie = Random.Range(1f, 3f);

        //timeUntilNextMuffin = passiveMuffinInterval;

        //InvokeRepeating("AwardPassiveMuffins", 1f, 1f);
        //InvokeRepeating(nameof(AwardPassiveMuffins), 1f, 1f);

        StartCoroutine(StartAwardingPassiveMuffins());

        //_music.PlayTrack("CalmLoop1");
        _music.QueueTrackLoop("CalmLoop1", "CalmLoop2");
    }

    private IEnumerator StartAwardingPassiveMuffins()
    {
        // So forever
        while(true)
        {
            // Wait 1 second
            yield return new WaitForSeconds(passiveMuffinInterval);

            // Award the passive muffins
            AwardPassiveMuffins();
        }
    }

    private void Update()
    {
        // Handle spawning cookies
        timeUntilNextCookie -= Time.deltaTime;
        if(timeUntilNextCookie <= 0)
        {
            SpawnCookie();

            timeUntilNextCookie = Random.Range(1f, 3f);
        }

        // Handle passive muffins
        /*timeUntilNextMuffin -= Time.deltaTime;
        if (timeUntilNextMuffin <= 0)
        {
            AwardPassiveMuffins();

            // Reset the countdown
            timeUntilNextMuffin = passiveMuffinInterval;
        }*/

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void AwardPassiveMuffins()
    {
        // Award the player the number of muffins earnt per second
        numberOfMuffins += muffinsPerSecond;

        // If the number of muffins per second is greater than zero
        if (muffinsPerSecond > 0)
        {
            // Show a floating text for the number of muffins earnt
            CreateFloatingText("+" + muffinsPerSecond.ToString());
        }
    }

    internal bool TryToPurchaseUpgrade(UpgradeType type, int cost)
    {
        // If the upgrade is too expensive
        if(cost > numberOfMuffins)
        {
            return false;
        }

        // Depending on the upgrade type
        switch(type)
        {
            case UpgradeType.MuffinsPerClick:
                // Increase the number of muffins to be awarded per click
                muffinsPerClick++;
                break;
            case UpgradeType.MuffinPerSecond:
                // Increase the number of muffins to be awarded per second
                muffinsPerSecond++;
                break;
            case UpgradeType.Cinnabomb:
                // Spawn a number of cinnamon rolls on the screen
                SpawnCinnamonRolls((upgrade3.level + 1) * numCinnamonRollsPerLevel);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        /*if(type == UpgradeType.MuffinsPerClick)
        {

        }
        else if(type == UpgradeType.CriticalClickChance)
        {

        }
        else if (type == UpgradeType.AnotherUpgrade)
        {

        }
        else if (type == UpgradeType.SureOneMore)
        {

        }
        else
        {
            // Not a real upgrade?!
        }*/

        // Spend the muffins
        numberOfMuffins -= cost;

        // We could afford the upgrade
        return true;
    }

    private void SpawnCinnamonRolls(int number)
    {
        

        StartCoroutine(PlayTensionMusic());
        
        for (int i = 0; i < number; i++)
        {
            // Create the roll
            CinnamonRoll roll = Instantiate(cinnamonRollPrefab, cookieContainer);
            roll.game = this;

            // Positions the roll
            roll.transform.localPosition = GenerateRandomPosition(-Screen.width / 2f, Screen.width / 2f, -Screen.height / 2f, Screen.height / 2f);
        }
    }

    private IEnumerator PlayTensionMusic()
    {
        // Play the tension track
        _music.StartTrackInterrupt("TensionLoop1");
        
        // Wait for 10 second
        yield return new WaitForSeconds(cinnamonRollPrefab.lifespan);
        
        // Stop the tension track
        _music.StopTrackInterrupt();
    }

    private void ResetGame()
    {
        numberOfMuffins = 0;
        muffinsPerClick = 1;
        muffinsPerSecond = 0;
        upgrade1.level = 0;
        upgrade2.level = 0;
        upgrade3.level = 0;

        SaveGame();
    }

    private void SpawnCookie()
    {
        // Create the cookie
        Cookie cookie = Instantiate(cookiePrefab, cookieContainer);
        cookie.game = this;

        // Positions the cookie
        cookie.transform.localPosition = GenerateRandomPosition(-Screen.width / 2f, Screen.width / 2f, -Screen.height / 2f, Screen.height / 2f);
    }

    public void OnCookieClicked()
    {
        // Award the muffins
        numberOfMuffins += 100;

        // Create a floating text to indicate how many muffins were collected
        CreateFloatingText("+100");
    }

    internal void OnRollClicked()
    {
        // Award the muffins
        numberOfMuffins += 20;

        // Create a floating text to indicate how many muffins were collected
        CreateFloatingText("+20");
    }

    public void OnMuffinClicked()
    {
        _audioSource.PlayOneShot(_muffinClickSound);

        SpawnLittleMuffin();
        
        // Initially assume this is a normal click
        int muffinAwarded = muffinsPerClick;

        // If this is a critical click
        if (Random.value < 0.1f)
        {
            // Adjust the number of muffin to award
            muffinAwarded = muffinsPerClick * 10;
        }

        // Award the muffins
        numberOfMuffins += muffinAwarded;

        // Create a floating text to indicate how many muffins were collected
        CreateFloatingText("+" + muffinAwarded.ToString());
    }

    private void SpawnLittleMuffin()
    {
        // Generate a random position for the floating text near the muffin
        Vector3 randomPosition = GenerateRandomPosition(-200f, 200f, 100f, 250f);

        // Instantiate a new floating text at that position
        var littleMuffin = Instantiate(_littleMuffinPrefab, floatingTextContainer);

        // Set the position of the floating text
        littleMuffin.transform.localPosition = randomPosition;
    }

    private void CreateFloatingText(string message)
    {
        // Generate a random position for the floating text near the muffin
        Vector3 randomPosition = GenerateRandomPosition(-200f, 200f, 100f, 250f);

        // Instantiate a new floating text at that position
        FloatingText newFloatingText = Instantiate(floatingTextPrefab, floatingTextContainer);
        newFloatingText.game = this;

        // Set the position of the floating text
        newFloatingText.transform.localPosition = randomPosition;

        // Set the text of the new floating text to the number of muffins collected
        newFloatingText.SetText(message);
    }

    private Vector3 GenerateRandomPosition(float xMin, float xMax, float yMin, float yMax)
    {
        // Generate random X and Y coordinates
        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        // Return the random position
        return new Vector3(x, y);
    }

    private void OnApplicationQuit()
    {
        // Save the game
        SaveGame();
    }

    private void SaveGame()
    {
        // Create the SaveData object
        SaveData saveData = new SaveData();

        // Fill in the save data
        saveData.numberOfMuffins = numberOfMuffins;
        saveData.muffinsPerClick = muffinsPerClick == 0 ? 1 : muffinsPerClick;
        saveData.muffinsPerSecond = muffinsPerSecond;
        saveData.upgrade1Level = upgrade1.level;
        saveData.upgrade2Level = upgrade2.level;
        saveData.upgrade3Level = upgrade3.level;

        // Convert the save data into a string
        string saveDataString = JsonUtility.ToJson(saveData);
        Debug.Log(saveDataString);

        // Save the data to PlayerPrefs
        PlayerPrefs.SetString("savedata", saveDataString);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        // Create the SaveData object
        SaveData saveData = new SaveData();

        // Get the save data from PlayerPrefs
        string saveDataString = PlayerPrefs.GetString("savedata", "{}");
        Debug.Log(saveDataString);

        // Convert the save data back into a SaveData object
        saveData = JsonUtility.FromJson<SaveData>(saveDataString);

        // Restore the game state using the save data
        numberOfMuffins = saveData.numberOfMuffins;
        muffinsPerClick = saveData.muffinsPerClick;
        muffinsPerSecond = saveData.muffinsPerSecond;
        upgrade1.level = saveData.upgrade1Level;
        upgrade2.level = saveData.upgrade2Level;
        upgrade3.level = saveData.upgrade3Level;
    }
}

public class SaveData
{
    public int numberOfMuffins;
    public int muffinsPerClick = 1;
    public int muffinsPerSecond;
    public int upgrade1Level;
    public int upgrade2Level;
    public int upgrade3Level;
}