using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DontDestroy : MonoBehaviour {

    //Singleton init
    public static DontDestroy instance;

    public Canvas canvas;
    public Text livesDisplay;

    public AudioClip pistol;
    public AudioClip sniper;
    public AudioClip ar;

    public AudioClip bGGame;
    public AudioClip bGStartMenu;

    public enum AudioClips { pistol, sniper, ar, bGGame, bGStartMenu }
    public AudioClips selectedClipWeapon = AudioClips.pistol;
    public AudioClips selectedClipBG = AudioClips.bGStartMenu;

    public AudioSource audioSourceBG;
    public AudioSource audioSourceWeapon;

    private bool[] levelsUnlocked = new bool[3];

    private int lives = 3;

    private void Awake() {
        if(instance != null)
            Debug.LogError("More than one DontDestroy in scene");
        else
            instance = this;

        DontDestroyOnLoad(this);

        audioSourceBG = GetComponent<AudioSource>();
        audioSourceBG.clip = bGStartMenu;
        audioSourceBG.Play();

        audioSourceWeapon.clip = pistol;

        levelsUnlocked[0] = true;
    }

    private void Start() {
        SceneManager.LoadScene(1);
    }

    private void Update() {
        if(Input.GetButtonDown("Cancel")) {
            SceneManager.LoadScene("IntroScene");
        }
        if(SceneManager.GetActiveScene().buildIndex < 4) {
            canvas.gameObject.SetActive(false);
        }
        else {
            canvas.gameObject.SetActive(true);
        }
    }

    public void ChangeAudioClip(AudioClips acs) {
        switch(acs) {
            case AudioClips.pistol:
                audioSourceWeapon.clip = pistol;
                break;
            case AudioClips.sniper:
                audioSourceWeapon.clip = sniper;
                break;
            case AudioClips.ar:
                audioSourceWeapon.clip = ar;
                break;
            case AudioClips.bGGame:
                audioSourceBG.clip = bGGame;
                break;
            case AudioClips.bGStartMenu:
                audioSourceBG.clip = bGStartMenu;
                break;
        }
    }

    public void ChangeBG() {
        audioSourceBG.Stop();
        audioSourceBG.Play();
    }

    public void ShootWeapon() {
        audioSourceWeapon.Play();
    }

    public int GetUnlockedLevels() {
        int returnNo = 0;
        for(int i = 0; i < levelsUnlocked.Length; i++) {
            if(levelsUnlocked[i])
                returnNo += 1;
        }
        return returnNo;
    }

    public void UnlockLevel(int num) {
        int tempNum = num - 3;
        if(tempNum < 0)
            tempNum = 0;
        if(tempNum > levelsUnlocked.Length)
            tempNum = levelsUnlocked.Length - 1;
        levelsUnlocked[tempNum] = true;

    }

    public void TakeLife() {
        lives -= 1;
        livesDisplay.text = "Lives: " + lives;
        if(lives <= 0) {
            Debug.Log("You died");
            SceneManager.LoadScene("IntroScene");
        }
    }
}
