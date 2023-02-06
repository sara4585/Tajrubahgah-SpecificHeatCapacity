using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float TransitionTime;
    static bool LOADLEVEL;
    int NoOfScenes;

    static bool LoadMenuLEVEL;

    private void Start()
    {
        LOADLEVEL = false;
        NoOfScenes = 3;
        LoadMenuLEVEL = false;
    }
    void Update()
    {
        if (LoadMenuLEVEL)
        {
            LOADLEVEL = false;
            LoadMenuLEVEL = false;
            LoadMenuFirstLevel();
        }
        else if (LOADLEVEL)
        {
            LOADLEVEL = false;
            LoadNextLevel();
        }
    }
    void LoadNextLevel()
    {
        int index = (SceneManager.GetActiveScene().buildIndex + 1) % NoOfScenes;
        StartCoroutine(LoadLevel(index));
       
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(TransitionTime);

        //load scene
        SceneManager.LoadScene(levelIndex);
    }

    void LoadMenuFirstLevel()
    {
        StartCoroutine(LoadLevel(0));
    }

    public static void ChangeLevel()
    {
        LOADLEVEL = true;
    }

    public static void ChangeMenuLevel()
    {

        LOADLEVEL = false;
        LoadMenuLEVEL = true;
    }
}
