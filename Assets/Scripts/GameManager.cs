using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Facebook.Unity;

public class GameManager : MonoBehaviour {
  void Start() {
    DOTween.Init ();
    FB.Init ();
  }

  public void onStartGame() {
    //SceneManager.LoadScene ("GamePlayScene");
    SceneManager.LoadScene("LevelScene");
  }
}
