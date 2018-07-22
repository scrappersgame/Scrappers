using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
public class GameMaster : MonoBehaviour {
	public static GameMaster gm;
    [Header("Objects")]
	public GameObject CamController;
	public GameObject spawnPoint;
    public GameObject pauseMenu;
    public GameObject pauseBG;
    public GameObject UI;
    public GameObject slot1;
    public Slider volumeSlider;
    [Header("Prefabs")]
    public Transform playerPrefab;
    public Transform spawnPrefab;
	public AudioClip spawnClip;
    [Header("Variables")]
    public bool paused = true;
    public float masterVolume = 0.5f;
    public Scene currentScene;

	void Awake (){
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}
    private void Start()
    {
        gm.GetComponent<Fading>().BeginFade(-1);
        SceneManager.LoadSceneAsync("Intro",LoadSceneMode.Additive);
        currentScene = SceneManager.GetSceneByName("Intro");
    }
    void LateUpdate(){
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton && !paused) {
            pauseButton = false;
            paused = true;
			PauseGame();
        }
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        if (playerCount >1){
            for (int i = 1; i < playerCount; i++){
                Destroy(GameObject.FindGameObjectsWithTag("Player")[i]);
            }
            CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }

	}

	public void SpawnPlayer(int spawnDelay){
		gm.StartCoroutine(gm.SpawnPlayerRoutine (spawnDelay));

	}
	public IEnumerator SpawnPlayerRoutine (int spawnDelay){
		yield return new WaitForSeconds (spawnDelay);
		Vector3 spawnPosition = spawnPoint.transform.position;
		Quaternion spawnRotation = spawnPoint.transform.rotation;
        AudioSource.PlayClipAtPoint(spawnClip, spawnPosition, masterVolume);
		Transform playerObj = Instantiate (playerPrefab, spawnPosition, spawnRotation);
		Transform spawnObj = Instantiate (spawnPrefab, spawnPosition, spawnRotation);
		Destroy (spawnObj.gameObject, 3);
		CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj;
        slot1.GetComponent<Button>().onClick.Invoke();
	}
	public static void KillPlayer (Player player){
		Destroy (player.gameObject);
		gm.SpawnPlayer (2);
	}
    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseBG.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseBG.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(Unpause(.01f));
    }
    IEnumerator Unpause(float delay){
        yield return delay;
        paused = false;
    }
    public void ChangeVolume(){
        masterVolume = volumeSlider.value;
        AudioSource _source = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
        _source.clip = spawnClip;
        _source.volume = masterVolume;
        _source.Play();
    }
    public void LoadNewScene(string _sceneName){
        StartCoroutine(LoadScene(_sceneName));
    }
    IEnumerator LoadScene(string _sceneName)
    {
        gm.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(gm.currentScene);
        SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        gm.currentScene = SceneManager.GetSceneByName(_sceneName);
        gm.GetComponent<Fading>().BeginFade(-1);
    }
}
