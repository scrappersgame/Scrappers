using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameMaster : MonoBehaviour {
    public static GameMaster gm;

    [Header("Objects")]
    public GameObject CamController;                // the thing that tells the camera where to go.
    public GameObject spawnPoint;                   // the thing that tells the player where to (re)start.
    public GameObject pauseMenu;                    // the thing that you click to turn the volume down.
    public GameObject pauseBG;                      // the thing that makes it easier to read the pause text.
    public GameObject UI;                           // the thing that tells you what's up with your dude.
    public GameObject slot1;                        // the first thing in your hotbar.
    public GameObject playerObj;
    public Slider volumeSlider;                     // the thing that controls the volume.
    public AudioClip introMusic;                    // the music that plays at the start of the game.
    public PolygonCollider2D mainSkyBox;            // the box that defines the sky.
    [Header("Prefabs")]
    public Transform playerPrefab;                  // your dude.
    public Transform scrapperPrefab;                // your dude's scrapper.
    public Transform spawnPrefab;                   // the particles that surround your dude when you (re)spawn.
    public Transform deathPrefab;                   // the particles that surround your dude when you die.
    public AudioClip spawnClip;                     // the sound your dude makes when you (re)spawn.
    [Header("Variables")]
    public bool gameStarted = false;                // has the game started yet?
    public bool paused = true;                      // how do I know if the game is paused?
    public bool speaking = true;                    // how do I know someone is talking?
    public float masterVolume = 0.5f;               // why is it so loud?
    public Scene currentScene;                      // where am I right now?
    private AudioSourceCrossfade _musicSource;      // where is that music coming from?
    private bool playerSpawning = false;            // is the player spawning rn?

    void Awake (){
        // setting up gm object so this can be used by any script
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

    private void Start()
    {
        _musicSource = AudioSourceCrossfade.cf;
        // need to call coroutine to wait for game to load
        StartCoroutine(LoadIntro());
    }

    IEnumerator LoadIntro(){
        // fade in on start of game
        _musicSource.volume = masterVolume;
        _musicSource.SetVolume(masterVolume);
        _musicSource.Play(introMusic);
        SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Additive);   //loading main menu and intro
        currentScene = SceneManager.GetSceneByName("Intro");            //so I can unload later
        while (!SceneLoaded("Intro"))
        {
            yield return new WaitForSeconds(0.5f);
            // maybe put some loading stuff here...
        }
        yield return new WaitForSeconds(0.1f);
        // skybox controls the camera limits, but need to be in persistent space
        UpdateSkybox();
        gm.GetComponent<Fading>().BeginFade(-1);
    }

    void UpdateSkybox(){
        // getting new sky dimensions
        PolygonCollider2D _newSkyBox = GameObject.FindGameObjectWithTag("Sky").GetComponent<PolygonCollider2D>();
        Vector2[] _skypoints = new Vector2[_newSkyBox.points.Length];   // setting up empty array to hold points later
        float yDifference = 0;                                          // setting up offset for later

        // iterating through all the points in the new skybox to get difference in y
        for (int i = 0; i < _newSkyBox.points.Length; i++){

            //transforming points to world coordinates
            _skypoints[i] = new Vector2(_newSkyBox.transform.TransformPoint(_newSkyBox.points[i]).x, _newSkyBox.transform.TransformPoint(_newSkyBox.points[i]).y + yDifference);
            if (_skypoints[i].y < mainSkyBox.transform.position.y)
            {
                // if y of point is lower, set the difference and move the skybox
                yDifference = mainSkyBox.transform.position.y - _skypoints[i].y;
                mainSkyBox.transform.position = new Vector3(mainSkyBox.transform.position.x, _skypoints[i].y, mainSkyBox.transform.position.z);
            }
        }

        // iterating through all the points again, adding the difference
        for (int i = 0; i < _newSkyBox.points.Length; i++)
        {
            _skypoints[i] = new Vector2(_newSkyBox.transform.TransformPoint(_newSkyBox.points[i]).x, _newSkyBox.transform.TransformPoint(_newSkyBox.points[i]).y + yDifference);
        }

        mainSkyBox.points = _skypoints;           // applying the new points to the skybox that is hooked up to the camera
        mainSkyBox.SetPath(0, mainSkyBox.points); // for some reason you need to draw the paths too...
    }

    void LateUpdate(){
        // listening for pause button
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton && !paused) {
            pauseButton = false;
            paused = true;
			PauseGame();
        }
        // removing extra players (for now)
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        if (playerCount >1){
            for (int i = 1; i < playerCount; i++){
                Destroy(GameObject.FindGameObjectsWithTag("Player")[i]);
            }
            CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }

	}


    // Spawn method to call the Spawn coroutine because you can't call a subroutine from another script
	public void SpawnPlayer(int spawnDelay){
		gm.StartCoroutine(gm.SpawnPlayerRoutine (spawnDelay));
	}

    // Spawn player coroutine, coroutine necessary for delay in spawn
	public IEnumerator SpawnPlayerRoutine (int spawnDelay){
		yield return new WaitForSeconds (spawnDelay); //how long to wait before spawn
        CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = spawnPoint.transform;
        // getting position/rotation from spawnpoint object
		Vector3 spawnPosition = spawnPoint.transform.position;
		Quaternion spawnRotation = spawnPoint.transform.rotation;
        // cool sounds bro
        AudioSource.PlayClipAtPoint(spawnClip, spawnPosition, masterVolume);
        // creating the player
        playerObj = Instantiate (playerPrefab, spawnPosition, spawnRotation).gameObject;
        // cool effects bro
		Transform spawnObj = Instantiate (spawnPrefab, spawnPosition, spawnRotation);
		Destroy (spawnObj.gameObject, 3);
        // tell the camera who to follow
		CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj.transform;
        // respawning has a cost
        playerObj.GetComponent<Player>().RemoveScrap(50);
        // reset spawning variable
        playerSpawning = false;
    }
    // Spawn method to call the Spawn coroutine because you can't call a subroutine from another script
    public void SpawnItem(GameObject _spawnItem, int spawnDelay){
        gm.StartCoroutine(gm.SpawnItemRoutine (_spawnItem, spawnDelay));
    }

    // Spawn item coroutine, coroutine necessary for delay in spawn
    public IEnumerator SpawnItemRoutine (GameObject _spawnItem, int spawnDelay){
        Transform prevFollow = CamController.GetComponent<CinemachineVirtualCamera>().m_Follow;
        CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = spawnPoint.transform;
        yield return new WaitForSeconds (spawnDelay); //how long to wait before spawn
        // getting position/rotation from spawnpoint object
        Vector3 spawnPosition = new Vector3 (spawnPoint.transform.position.x, spawnPoint.transform.position.y + 1, spawnPoint.transform.position.z);
        Quaternion spawnRotation = spawnPoint.transform.rotation;
        // cool sounds bro
        AudioSource.PlayClipAtPoint(spawnClip, spawnPosition, masterVolume);
        // creating the player
        Instantiate (_spawnItem.transform, spawnPosition, spawnRotation);

        // cool effects bro
        Transform spawnParticleObj = Instantiate (spawnPrefab, spawnPosition, spawnRotation);
        Destroy (spawnParticleObj.gameObject, 3);
        // tell the camera who to follow
        yield return new WaitForSeconds(spawnDelay); //how long to wait before spawn
        CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = prevFollow;
        // activate first item in hotbar
    }

    // we all gotta die some day
	public static void KillPlayer (Player player){
        if (!gm.playerSpawning)
        {
            gm.playerSpawning = true;
            // Where did I die?
            Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            Quaternion playerRotation = player.transform.rotation;
            // creating the scrapper
            Transform scrapperObj = Instantiate(gm.scrapperPrefab, playerPosition, playerRotation);
            // camera follows scrapper
            gm.CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = scrapperObj;
            // setting scrap value of scrapper
            scrapperObj.GetComponent<Pickup>().ScrapValue = Mathf.Clamp(PlayerMaster.stats.currentScrap + 50, 20, PlayerMaster.stats.maxScrap);
            scrapperObj.GetComponent<ScrapperPickup>().maxScrap = PlayerMaster.stats.maxScrap;
            player.RemoveScrap(PlayerMaster.stats.currentScrap);
            // creating the death particles
            Transform deathParticleObj = Instantiate(gm.deathPrefab, player.transform.position, player.transform.rotation);
            Destroy(deathParticleObj.gameObject, 3);
            Destroy(player.gameObject);
            gm.SpawnPlayer(2); // breathing time.
        }
	}

    // die Die DIE
    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    // stop. pausing time.
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseBG.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0;
    }

    // carry on, nothing to see here
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseBG.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(Unpause(.01f));
    }

    // because human fingers are slow.
    IEnumerator Unpause(float delay){
        yield return delay;
        paused = false;
    }

    // crank it to 11!
    public void ChangeVolume(){
        masterVolume = volumeSlider.value;
        AudioSource _soundsSource = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
        _musicSource.SetVolume(masterVolume);
        _soundsSource.clip = spawnClip;
        _soundsSource.volume = masterVolume;
        _soundsSource.Play();
    }

    // we're going places
    public void LoadNewScene(string _sceneName){
        StartCoroutine(LoadScene(_sceneName));
    }

    // now we're actually going places, I lied to you before.
    IEnumerator LoadScene(string _sceneName)
    {
        // paint it black
        gm.GetComponent<Fading>().BeginFade(1); 
        yield return new WaitForSeconds(1f);
        // we're nowhere now...
        SceneManager.UnloadSceneAsync(gm.currentScene);
        // where are we going again?
        SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        // are we there yet?
        while (!SceneLoaded(_sceneName)){
            Debug.Log("Waiting for " + _sceneName + " to load...");
            yield return new WaitForSeconds(0.5f);
            // maybe put some loading stuff here...
        }
        yield return new WaitForSeconds(0.1f);
        // finally got there, lets remember where we are...
        gm.currentScene = SceneManager.GetSceneByName(_sceneName);
        // what are the limits to the sky?
        UpdateSkybox();
        // back to life, back to reality
        gm.GetComponent<Fading>().BeginFade(-1);
    }

    // how do we know if we're there yet?
    bool SceneLoaded(string _sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == _sceneName)
            {
                //the scene is already loaded
                return true;
            }
        }

        return false;//scene not currently loaded in the hierarchy
    }
}
