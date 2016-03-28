using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  private Canvas modeChanger;
	// Use this for initialization
	void Start () {
    modeChanger = GameObject.Find ("ModeChanger").GetComponent<Canvas> ();
    modeChanger.enabled = false;
    setMode (LevelUtils.getCurrentMode ());
	}
	
  public void onStartGame() {
    //SceneManager.LoadScene ("GamePlayScene");
    SceneManager.LoadScene("LevelScene");
  }

  public void onModeChangeClicked() {
    modeChanger.enabled = true;
    int maxMode = LevelUtils.getMaxMode ();
    if (maxMode < 4) {
      GameObject.Find ("InsaneBtn").GetComponent<Button> ().interactable = false;
    } else {
      GameObject.Find ("InsaneBtn").GetComponent<Button> ().interactable = true;
    }
    if (maxMode < 3) {
      GameObject.Find ("VeryHardBtn").GetComponent<Button> ().interactable = false;
    } else {
      GameObject.Find ("VeryHardBtn").GetComponent<Button> ().interactable = true;
    }
    if (maxMode < 2) {
      GameObject.Find ("HardBtn").GetComponent<Button> ().interactable = false;
    } else {
      GameObject.Find ("HardBtn").GetComponent<Button> ().interactable = true;
    }
  }

  private void setMode(int m) {
    LevelUtils.setCurrentMode (m);
    modeChanger.enabled = false;
    Button modeBtn = GameObject.Find ("ModeBtn").GetComponent<Button> ();
    modeBtn.GetComponentInChildren<Text> ().text = LevelUtils.getModeString (m);
  }

  public void onEasyClicked() {
    setMode (0);
  }

  public void onNormalClicked() {
    setMode (1);
  }

  public void onHardClicked() {
    setMode (2);
  }

  public void onVeryHardClicked() {
    setMode (3);
  }

  public void onInsaneClicked() {
    setMode (4);
  }
}
