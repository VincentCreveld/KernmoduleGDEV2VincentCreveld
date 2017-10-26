using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {


    //Singleton init
    public static GameManager instance;
    public PlayerController player;

    private void Awake() {
        if(instance != null)
            Debug.LogError("More than one GameManager in scene");
        else
            instance = this;

        if(player == null) {
            Debug.Log("INCOMPLETE LEVEL");
            Application.Quit();
        }

        DontDestroy.instance.ChangeAudioClip(DontDestroy.AudioClips.bGGame);
        DontDestroy.instance.ChangeBG();

    }

    private void Start() {
        player.playerDeath += RestartLevel;
    }

    public void RestartLevel() {
        StartCoroutine(Restart());
    }

    public void FinishLevel() {
        StartCoroutine(Finish());
    }

    private IEnumerator Finish() {
        yield return new WaitForSeconds(5);
        Debug.Log("Finishing level!");
        //Load next scene
        //DontDestroy gameManager unlocked levels += 1;
        DontDestroy.instance.UnlockLevel(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator Restart() {
        DontDestroy.instance.TakeLife();
        yield return new WaitForSeconds(5);
        Debug.Log("Restarting level!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
