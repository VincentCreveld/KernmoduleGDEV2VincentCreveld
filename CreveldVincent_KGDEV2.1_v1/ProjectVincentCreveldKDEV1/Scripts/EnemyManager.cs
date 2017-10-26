using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	private void Start() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<Enemy>().enemyDeath += CheckChildren;
        }
    }

    public void CheckChildren() {
        StartCoroutine(CheckIfFinished());
    }

    private IEnumerator CheckIfFinished() {
        Debug.Log("Reached code");
        yield return new WaitForSeconds(2.1f);
        Debug.Log("Reached code2");
        if(transform.childCount <= 0) {
            GameManager.instance.FinishLevel();
        }
        else if(transform.childCount <= 1) {
            for(int i = 0; i < transform.childCount; i++) {
                Debug.Log("Reached code3");
                Debug.Log(transform.childCount);
                transform.GetChild(i).GetComponent<Enemy>().enrageEvent();
            }
        }
    }
}
