using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        Debug.Log(transform.childCount);
        Debug.Log(DontDestroy.instance.GetUnlockedLevels());
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0; i < DontDestroy.instance.GetUnlockedLevels(); i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
	
    public void LoadScene(int n) {
        SceneManager.LoadScene(n);
    }
	
}
