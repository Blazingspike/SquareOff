using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
  public void onStartGame() {
    SceneManager.LoadScene ("GamePlayScene");
  }
}
