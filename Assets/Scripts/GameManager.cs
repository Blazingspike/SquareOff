using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  public void onStartGame() {
    //SceneManager.LoadScene ("GamePlayScene");
    SceneManager.LoadScene("LevelScene");
  }
}
