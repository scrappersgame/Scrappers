﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameMaster : MonoBehaviour {
	public static GameMaster gm;
    [Header("Objects")]
	public GameObject CamController;    // the thing that tells the camera where to go.
    public GameObject spawnPoint;       // the thing that tells the player where to (re)start.
    public GameObject pauseMenu;        // the thing that you click to turn the volume down.
    public GameObject pauseBG;          // the thing that makes it easier to read the pause text.
    public GameObject UI;               // the thing that tells you what's up with your dude.
    public GameObject slot1;            // the first thing in your hotbar.
    public Slider volumeSlider;         // the thing that controls the volume.
    [Header("Prefabs")]
    public Transform playerPrefab;      // your dude.
    public Transform spawnPrefab;       // the particles that surround your dude when you (re)spawn.
    public AudioClip spawnClip;         // the sound your dude makes when you (re)spawn.
    [Header("Variables")]
    public bool paused = true;          // how do I know if the game is paused?
    public float masterVolume = 0.5f;   // why is it so loud?
    public Scene currentScene;          // where am I right now?

	void Awake (){
        // setting up gm object so this can be used by any script
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}
    private void Start()
    {
        // fade in on start of game
        gm.GetComponent<Fading>().BeginFade(-1);
        SceneManager.LoadSceneAsync("Intro",LoadSceneMode.Additive); //loading main menu and intro
        currentScene = SceneManager.GetSceneByName("Intro"); //so I can unload later
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
        // getting position/rotation from spawnpoint object
		Vector3 spawnPosition = spawnPoint.transform.position;
		Quaternion spawnRotation = spawnPoint.transform.rotation;
        // cool sounds bro
        AudioSource.PlayClipAtPoint(spawnClip, spawnPosition, masterVolume);
        // creating the player
		Transform playerObj = Instantiate (playerPrefab, spawnPosition, spawnRotation);
        // cool effects bro
		Transform spawnObj = Instantiate (spawnPrefab, spawnPosition, spawnRotation);
		Destroy (spawnObj.gameObject, 3);
        // tell the camera who to follow
		CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj;
        // activate first item in hotbar
        slot1.GetComponent<Button>().onClick.Invoke();
	}
    // we all gotta die some day
	public static void KillPlayer (Player player){
		Destroy (player.gameObject);
		gm.SpawnPlayer (2); // breathing time.
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
        AudioSource _source = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
        _source.clip = spawnClip;
        _source.volume = masterVolume;
        _source.Play();
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
        // finally got there, lets remember where we are...
        gm.currentScene = SceneManager.GetSceneByName(_sceneName);
        // what are the limits to the sky?
        Collider2D _skyBox = GameObject.FindGameObjectWithTag("Sky").GetComponent<BoxCollider2D>();
        CamController.GetComponent<CinemachineConfiner>().m_BoundingShape2D = _skyBox;
        // back to life, back to reality
        gm.GetComponent<Fading>().BeginFade(-1);
    }

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
