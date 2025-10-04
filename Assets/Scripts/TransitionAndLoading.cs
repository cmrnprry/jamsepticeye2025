using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransistionsAndLoading : MonoBehaviour
{
    public static TransistionsAndLoading instance;

    [Header("Loading Screen")]
    public Image LoadScreen;
    public GameObject text;
    //public Slider progressBar;

    [Header("Transition Screen")]
    public Image TransitionScreen;

    private AsyncOperation loadingOperation;
    private bool loading_cutscene = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DOTween.Init();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// I didn't wanna use the update loop so we have this
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateLoading()
    {
        yield return new WaitUntil(() => loadingOperation != null);

        //progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);

        if (loadingOperation.isDone)
        {
            yield return new WaitForSeconds(.5f);

            EndSceneLoad();

            yield break;
        }

        StartCoroutine(UpdateLoading());
    }

    /// <summary>
    /// Called when the player clicks a button that warrents loading
    /// </summary>
    /// <param name="scene">string name of the scene we are loading in</param>
    public void StartCutSceneLoad(string scene)
    {
        print("start scene load");
        loading_cutscene = true;
        LoadScreen.gameObject.SetActive(true);
        LoadScreen.DOFade(1, 1).OnComplete(() =>
        {
            text.SetActive(true);
            StartCoroutine(UpdateLoading());
            loadingOperation = SceneManager.LoadSceneAsync(scene);
        });
    }

    public void StartGameSceneLoad(string scene)
    {
        print("start scene load");
        loading_cutscene = false;
        LoadScreen.gameObject.SetActive(true);
        LoadScreen.DOFade(1, 1).OnComplete(() =>
        {
            text.SetActive(true);
            StartCoroutine(UpdateLoading());
            loadingOperation = SceneManager.LoadSceneAsync(scene);
        });
    }

    public void StartSceneLoad(string scene, bool isCutscene)
    {
        print("start scene load");
        loading_cutscene = isCutscene;
        LoadScreen.gameObject.SetActive(true);
        LoadScreen.DOFade(1, 1).OnComplete(() =>
        {
            text.SetActive(true);
            StartCoroutine(UpdateLoading());
            loadingOperation = SceneManager.LoadSceneAsync(scene);
        });
    }

    /// <summary>
    /// Ends the scene load
    /// </summary>
    private void EndSceneLoad()
    {
        text.SetActive(false);

        PauseMenu.SetPause(false);

        if (!loading_cutscene)
            GameManager.instance.SetDataOnLoad();

        LoadScreen.DOFade(0, 1.5f).OnComplete(() =>
        {
            LoadScreen.gameObject.SetActive(false);
        });

        print("end scene load");
    }

    /// <summary>
    /// Transitions to show a menu
    /// </summary>
    /// <param name="Menu">Gameobject we want to view</param>
    public void TransitionToMenu(GameObject Menu)
    {
        TransitionScreen.gameObject.SetActive(true);
        TransitionScreen.DOFade(1, 1).OnComplete(() =>
        {
            Menu.SetActive(!Menu.activeSelf);
            TransitionScreen.DOFade(0, 1).OnComplete(() =>
            {
                TransitionScreen.gameObject.SetActive(false);
            });
        });
    }

    /// <summary>
    ///Hides menu before load
    /// </summary>
    /// <param name="Menu">Gameobject we want to view</param>
    async Task HideScreen()
    {
        TransitionScreen.gameObject.SetActive(true);
        TransitionScreen.DOFade(1, 1);

        var task = Task.Delay(1500);
        await task;

        print("hideen");
    }

    /// <summary>
    /// Shows the menu after load
    /// </summary>
    async Task ShowScreen()
    {

        TransitionScreen.DOFade(0, 1).OnComplete(() =>
        {
            TransitionScreen.gameObject.SetActive(false);
        });

        var task = Task.Yield();
        await task;

        print("seeing");
    }
}
