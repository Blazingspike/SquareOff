using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Gamelogic.Grids;
using DG.Tweening;
using UnityEngine.Advertisements;

public class MainController : MonoBehaviour {
  public static MainController Instance;

  private RectGrid<SquareCell> grid;

  private IMap<RectPoint> map;

  private Level level;
  private Dictionary<RectPoint, Marker> markedPoints;

  float timer;
  int moveCount;
  bool isPlaying;
  int pickedColor;

  void initSinglton () {
    if (Instance == null) {
      GameObject.DontDestroyOnLoad (gameObject);
      Instance = this;
    }
  }

  void Awake () {
    DOTween.Init ();
    initSinglton ();
  }

  // Use this for initialization
  void Start () {
    LevelUtils.setLevel (0);
    resetLevel ();
  }
	
  // Update is called once per frame
  void Update () {
    if (isPlaying && moveCount > 0) {
      timer += Time.deltaTime;
      updateTimer ();
    }
    if (isPlaying && Input.GetMouseButtonDown (0)) {
      Vector3 worldPosition = GridBuilderUtils.ScreenToWorld (gameObject, Input.mousePosition);
      RectPoint point = map [worldPosition];
      if (!grid.Contains (point)) {
        return;
      }
      handlePointClicked (point);
    }
  }

  private void updateTimer () {
    Text t = GameObject.Find ("TimeCount").GetComponent<Text> ();
    if (t != null) {
      t.text = " Timer: " + timer.ToString ("0.00");
    }
  }

  private void updateMoveCount () {
    Text t = GameObject.Find ("MoveCount").GetComponent<Text> ();
    if (t != null) {
      t.text = " Moves: " + moveCount.ToString ();
    }
  }

  private void resetCounter () {
    // show levels
    level = LevelUtils.getCurrentLevel ();
    Text currentLevel = GameObject.Find ("CurrentLevel").GetComponent<Text> ();
    currentLevel.text = "Level " + (level.id + 1).ToString ();
    moveCount = 0;
    timer = 0.00f;
    isPlaying = false;
    clearMarker ();
    updateMoveCount ();
    updateTimer ();
  }

  private void clearMarker () {
    if (markedPoints != null && markedPoints.Count > 0) {
      foreach (Marker marker in markedPoints.Values) {
        GameObject.Destroy (marker.obj);
      }
      markedPoints.Clear ();
    }
  }

  private void addOneMove () {
    moveCount += 1;
    updateMoveCount ();
  }

  public void resetLevel () {
    if (grid != null && !LevelUtils.isMaxLevel (level)) {
      clearGrid ();
    }
    Canvas c = GameObject.Find ("WinPanel").GetComponent<Canvas> ();
    c.enabled = false;
    resetCounter ();
    BuildGrid ();
    StartCoroutine (shuffleGrid ());
    //isPlaying = true;
  }

  public void nextLevelHandler() {
    if (Advertisement.IsReady ()) {
      Debug.Log (Advertisement.gameId);
      Advertisement.Show ();
    }
    resetLevel();
  }

  public void backHome () {
    resetCounter ();
    SceneManager.LoadScene ("HomeScene");
  }

  private IEnumerator shuffleGrid () {
    PointList<RectPoint> plist = new PointList<RectPoint> ();
    isPlaying = false;
    foreach (RectPoint rp in grid.ToPointList()) {
      if (Utils.getPointType (rp, level.gridWidth, level.gridHeight) == PointType.CENTER) {
        plist.Add (rp);
      }
    }
    int count = 0;
    while (count < 10) {
      int rnd = Random.Range (0, 3);
      int idx = Random.Range (0, plist.Count);
      switch (rnd) {
        case 0:
          Utils.moveUp (plist [idx], grid, map, level.gridWidth, level.gridHeight);
          break;
        case 1:
          Utils.moveDown (plist [idx], grid, map, level.gridWidth, level.gridHeight);
          break;
        case 2:
          Utils.moveRight (plist [idx], grid, map, level.gridWidth, level.gridHeight);
          break;
        case 3:
          Utils.moveLeft (plist [idx], grid, map, level.gridWidth, level.gridHeight);
          break;
        default:
          break;
      }
      count += 1;
      yield return new WaitForSeconds (0.12f);
    }
    isPlaying = true;
  }

  private void handlePointClicked (RectPoint p) {
    PointType type = Utils.getPointType (p, level.gridWidth, level.gridHeight);
    if (type == PointType.NO_OP || type == PointType.CENTER) {
      return;
    }
    if (type == PointType.MOVE_UP) {
      Utils.moveUp (p, grid, map, level.gridWidth, level.gridHeight);
    }
    if (type == PointType.MOVE_DOWN) {
      Utils.moveDown (p, grid, map, level.gridWidth, level.gridHeight);
    }
    if (type == PointType.MOVE_LEFT) {
      Utils.moveLeft (p, grid, map, level.gridWidth, level.gridHeight);
    }
    if (type == PointType.MOVE_RIGHT) {
      Utils.moveRight (p, grid, map, level.gridWidth, level.gridHeight);
    }
    addOneMove ();
    // Check if winning.
    isPlaying = !isWinning ();
    if (!isPlaying) {
      handleWinning ();
    }
  }

  private void handleWinning () {
    GameObject.Find ("WinPanel").GetComponent<Canvas> ().enabled = true;
    if (LevelUtils.isMaxLevel (level)) {
      Text nextLevel = GameObject.Find ("NextLevel").GetComponent<Text> ();
      nextLevel.text = "Max Level";
    } else {
      LevelUtils.setLevel (level.id + 1);
    }
  }

  private bool isWinning () {
    foreach (RectPoint rp in grid) {
      if (Utils.getPointType (rp, level.gridWidth, level.gridHeight) == PointType.CENTER) {
        if (!grid [rp].IsPicked) {
          return false;
        }

        if (markedPoints.ContainsKey (rp)) {
          if (grid [rp].MarkIndex != markedPoints [rp].index) {
            return false;
          }
        }
      }
    }
    return true;
  }

  private void clearGrid () {
    foreach (RectPoint point in grid) {
      if (grid [point] != null) {
        Destroy (grid [point].gameObject);
      }
    }
    grid = null;
    map = null;
  }

  private void BuildGrid () {
    if (grid != null) {
      return;
    }
    if (markedPoints == null) {
      markedPoints = new Dictionary<RectPoint, Marker> ();
    } else {
      clearMarker ();
    }
    // Creates a grid in a rectangular shape.
    grid = RectGrid<SquareCell>.Rectangle (level.gridWidth, level.gridHeight);
    // Creates a map...
    map = new RectMap (AssetManager.Instance.squarePrefab.Dimensions)
      .WithWindow (Utils.ScreenRect)
      .AlignMiddleCenter (grid);

    // Generate grid and cell.
    foreach (RectPoint point in grid) { //Iterates over all points (coordinates) contained in the grid
      SquareCell cell = Instantiate (AssetManager.Instance.squarePrefab) as SquareCell;

      Vector3 worldPoint = map [point]; //Calculate the world point of the current grid point
      cell.transform.position = worldPoint;
      cell.name = point.X + "," + point.Y;
      grid [point] = cell;
    }

    PointList<RectPoint> points = grid.ToPointList ();
    int pickedColorCount = (level.gridWidth - 2) * (level.gridHeight - 2);
    int numMarkedCell = level.numMarked [0];
    int markerIdx = 0;
    while (points.Count > 0) {
      RectPoint point = points [Random.Range (0, points.Count)];
      PointType type = Utils.getPointType (point, level.gridWidth, level.gridHeight);
      if (type == PointType.NO_OP) {
        points.Remove (point);
        continue;
      }
      if (pickedColorCount > 0) {
        grid [point].IsPicked = true;
        pickedColorCount -= 1;
        // Pick the cell that is marked
        if (markerIdx < 4) {
          if (numMarkedCell > 0) {
            grid [point].MarkIndex = markerIdx;
            grid [point].name = "marked: " + markerIdx.ToString ();
            numMarkedCell -= 1;
          }
          while (numMarkedCell == 0) {
            markerIdx += 1;
            if (markerIdx == 4) {
              // Used all markers.
              break;
            }
            numMarkedCell = level.numMarked [markerIdx];
          }
        }
      } else {
        grid [point].IsPicked = false;
      }
      grid [point].IsBorder = (type != PointType.CENTER);
      grid [point].setSprite ();
      points.Remove (point);
    }

    // Place marker
    points = grid.ToPointList ();
    numMarkedCell = level.numMarked [0];
    markerIdx = 0;
    while (points.Count > 0) {
      RectPoint point = points [Random.Range (0, points.Count)];
      PointType type = Utils.getPointType (point, level.gridWidth, level.gridHeight);
      if (type != PointType.CENTER) {
        points.Remove (point);
        continue;
      }
      if (markerIdx < 4) {
        if (numMarkedCell > 0) {
          GameObject cell = Instantiate (AssetManager.Instance.markerPrefab [markerIdx]) as GameObject;
          Vector3 worldPoint = map [point]; //Calculate the world point of the current grid point
          worldPoint.Set (worldPoint.x, worldPoint.y, -1f);
          cell.transform.position = worldPoint;
          markedPoints.Add (point, new Marker (markerIdx, cell));
          numMarkedCell -= 1;
          points.Remove (point);
        }
        while (numMarkedCell == 0) {
          markerIdx += 1;
          if (markerIdx == 4) {
            // Used all markers.
            break;
          }
          numMarkedCell = level.numMarked [markerIdx];
        }
      } else {
        break;
      }
    }
  }
}
