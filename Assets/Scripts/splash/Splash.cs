//===================================================
//Author      : DRB
//CreateTime  ：4/13/2017 9:47:44 AM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    void Start() {
        Invoke("gotoNext", 3f);
    }

    private void gotoNext() {
        SceneManager.LoadScene("Scene_Init");
    }
}
