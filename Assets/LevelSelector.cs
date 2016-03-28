using UnityEngine;
using System.Collections;
using Gamelogic.Grids;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

  private RectGrid<LevelCell> grid;

  private IMap<RectPoint> map;

  private Canvas modeChanger;
	// Use this for initialization
	void Start () {
    modeChanger = GameObject.Find ("ModeChanger").GetComponent<Canvas> ();
    modeChanger.enabled = false;
    setMode (LevelUtils.getCurrentMode ());
    BuildLevelGrid ();
	}

  void Update() {
    if (modeChanger.enabled) {
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

  void handleLevelClicked(LevelCell cell) {
    LevelUtils.setLevel (cell.Level - 1);
    SceneManager.LoadScene("GamePlayScene");
  }

  void BuildLevelGrid() {
    // Creates a grid in a rectangular shape.
    grid = RectGrid<LevelCell>.Rectangle(4, 5);
    // Creates a map...
    map = new RectMap (AssetManager.Instance.levelPrefab.Dimensions * 1.2f)
      .WithWindow (Utils.ScreenRect)
      .AlignMiddleCenter (grid)
      .FlipXY()
      .Rotate(-90f);

    // Generate grid and cell.
    int count = 1;
    foreach (RectPoint point in grid) { //Iterates over all points (coordinates) contained in the grid
      LevelCell cell = Instantiate (AssetManager.Instance.levelPrefab) as LevelCell;
      Vector3 worldPoint = map [point]; //Calculate the world point of the current grid point
      cell.transform.position = worldPoint;
      cell.Level = count;
      cell.IsPlayable = true;
      grid [point] = cell;
      count += 1;
    }
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
