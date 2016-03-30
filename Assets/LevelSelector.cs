using UnityEngine;
using System.Collections;
using Gamelogic.Grids;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class LevelSelector : MonoBehaviour {

  private RectGrid<LevelCell> grid;

  private IMap<RectPoint> map;

  private Canvas modeChanger;

  private AudioSource audioSource;
  // Use this for initialization
  void Start () {
    modeChanger = GameObject.Find ("ModeChanger").GetComponent<Canvas> ();
    modeChanger.enabled = false;
    GameObject.Find ("MessageBox").GetComponent<Text> ().enabled = false;
    setMode (LevelUtils.getCurrentMode ());
    BuildLevelGrid ();
  }

  void Update () {
    if (modeChanger != null && modeChanger.enabled) {
      return;
    }
    if (Input.GetMouseButtonDown (0)) {
      Vector3 worldPosition = GridBuilderUtils.ScreenToWorld (gameObject, Input.mousePosition);
      RectPoint point = map [worldPosition];
      if (!grid.Contains (point)) {
        return;
      }
      handleLevelClicked (grid [point]);
    }
  }

  private void playBtnClicked() {
    if (audioSource == null) {
      audioSource = this.GetComponent<AudioSource> ();
    }
    audioSource.PlayOneShot(AssetManager.Instance.btnClick);
  }

  void handleLevelClicked (LevelCell cell) {
    playBtnClicked ();
    if (cell.IsPlayable) {
      LevelUtils.setCurrentLevel (cell.Level - 1);
      if (LevelUtils.showAds () && Advertisement.IsReady ()) {
        Advertisement.Show ();
      }
      SceneManager.LoadScene ("GamePlayScene");
    } else {
      showMessage (
        "Level is locked, you need to complete all lower levels to unlock.",
        4f);
    }
  }

  void BuildLevelGrid () {
    // Creates a grid in a rectangular shape.
    grid = RectGrid<LevelCell>.Rectangle (4, 5);
    // Creates a map...
    map = new RectMap (AssetManager.Instance.levelPrefab.Dimensions * 1.15f)
      .WithWindow (Utils.ScreenRect)
      .AlignMiddleCenter (grid)
      .FlipXY ()
      .Rotate (-90f);

    // Generate grid and cell.
    int count = 0;
    int maxLvl = LevelUtils.getMaxLevelId ();
    foreach (RectPoint point in grid) { //Iterates over all points (coordinates) contained in the grid
      LevelCell cell = Instantiate (AssetManager.Instance.levelPrefab) as LevelCell;
      Vector3 worldPoint = map [point]; //Calculate the world point of the current grid point
      cell.transform.position = worldPoint;
      cell.Level = count + 1;
      if (count > maxLvl) {
        cell.IsPlayable = false;
      } else {
        cell.IsPlayable = true;
      }
      grid [point] = cell;
      count += 1;
    }
  }

  void UpdateLevelGrid () {
    int maxLvl = LevelUtils.getMaxLevelId ();
    foreach (RectPoint point in grid) {
      LevelCell cell = grid [point];
      int lvl = cell.Level - 1;
      if (lvl > maxLvl) {
        cell.IsPlayable = false;
      } else {
        cell.IsPlayable = true;
      }
      grid [point] = cell;
    }
  }

  public void onModeChangeClicked () {
    modeChanger.enabled = true;
    int maxMode = LevelUtils.getMaxMode ();
    if (maxMode < 3) {
      GameObject.Find ("InsaneBtn").GetComponent<Button> ().interactable = false;
    } else {
      GameObject.Find ("InsaneBtn").GetComponent<Button> ().interactable = true;
    }
    if (maxMode < 2) {
      GameObject.Find ("HardBtn").GetComponent<Button> ().interactable = false;
    } else {
      GameObject.Find ("HardBtn").GetComponent<Button> ().interactable = true;
    }
  }

  private void setMode (int m) {
    LevelUtils.setCurrentMode (m);
    modeChanger.enabled = false;
    Button modeBtn = GameObject.Find ("ModeBtn").GetComponent<Button> ();
    modeBtn.GetComponentInChildren<Text> ().text = LevelUtils.getModeString (m);
    GameObject.Find ("ModeMessage").GetComponent<Text> ().text =
      LevelUtils.getModeInfo (m);
    if (grid != null) {
      UpdateLevelGrid ();
    }
  }

  public void onEasyClicked () {
    playBtnClicked ();
    setMode (0);
  }

  public void onNormalClicked () {
    playBtnClicked ();
    setMode (1);
  }

  public void onHardClicked () {
    playBtnClicked ();
    setMode (2);
  }

  public void onInsaneClicked () {
    playBtnClicked ();
    setMode (3);
  }

  public void showMessage (string message, float delay) {
    StartCoroutine (displayMessage (message, delay));
  }

  IEnumerator displayMessage (string message, float delay) {
    Text txt = GameObject.Find ("MessageBox").GetComponent<Text> ();
    txt.text = message;
    txt.enabled = true;
    yield return new WaitForSeconds (delay);
    txt.enabled = false;
  }
}
