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
    AudioSource s = this.GetComponent<AudioSource>();
    s.PlayOneShot (AssetManager.Instance.btnClick);
    SceneManager.LoadScene("LevelScene");
  }
}
