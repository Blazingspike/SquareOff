using UnityEngine;
using System.Collections;
using Gamelogic.Grids;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

  private RectGrid<LevelCell> grid;

  private IMap<RectPoint> map;

	// Use this for initialization
	void Start () {
    BuildLevelGrid ();
	}

  void Update() {
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
}
