using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level {
  public int id;
  public int gridWidth;
  public int gridHeight;
  public int[] numMarked;
  public string description;

  public Level() {
    id = 0;
    gridWidth = 4;
    gridHeight = 4;
    description = "Align the picked color to the cube";
  }

  public Level(int id, int gridWidth, int gridHeight,
    int mark1, int mark2, int mark3, int mark4) {
    this.id = id;
    this.gridWidth = gridWidth;
    this.gridHeight = gridHeight;
    numMarked = new int[4];
    numMarked [0] = mark1;
    numMarked [1] = mark2;
    numMarked [2] = mark3;
    numMarked [3] = mark4;
    if (mark1 > 0 || mark2 > 0 || mark3 > 0 || mark4 > 0) {
      this.description = "Center the blue squares";
    } else {
      this.description = "Center the blue squares and align the marked ones";
    }
  }
}

public static class LevelUtils {
  private static List<Level> levels;
  private static string LevelString = "rubic2d-level";

  private static void initLevels() {
    if (levels != null) {
      return;
    }
    levels = new List<Level> ();
    levels.Add (new Level (0, 5, 5, 0, 0, 0, 0));
    levels.Add (new Level (1, 6, 6, 0, 0, 0, 0));
    levels.Add (new Level (2, 5, 5, 1, 0, 0, 0));
    levels.Add (new Level (3, 5, 5, 2, 0, 0, 0));
    levels.Add (new Level (4, 5, 5, 3, 0, 0, 0));
    levels.Add (new Level (5, 5, 5, 4, 0, 0, 0));
    levels.Add (new Level (6, 5, 5, 1, 1, 0, 0));
    levels.Add (new Level (7, 5, 5, 2, 2, 0, 0));
    levels.Add (new Level (8, 5, 5, 3, 3, 0, 0));
    levels.Add (new Level (9, 5, 5, 1, 1, 0, 0));
    levels.Add (new Level (10, 5, 5, 1, 1, 1, 0));
    levels.Add (new Level (11, 5, 5, 1, 1, 1, 1));
    levels.Add (new Level (12, 6, 6, 1, 0, 0, 0));
    levels.Add (new Level (13, 6, 6, 1, 1, 0, 0));
    levels.Add (new Level (14, 6, 6, 1, 1, 1, 0));
    levels.Add (new Level (15, 6, 6, 1, 1, 1, 1));
  }

  public static Level getLevel(int idx) {
    if (levels == null) {
      initLevels ();
    }
    if (idx >= levels.Count) {
      return levels[levels.Count - 1];
    }
    return levels [idx];
  }

  public static Level getCurrentLevel() {
    if (PlayerPrefs.HasKey (LevelString)) {
      return getLevel(PlayerPrefs.GetInt (LevelString));
    }
    setLevel (0);
    return getLevel(0);
  }

  public static void setLevel(int idx) {
    initLevels ();
    PlayerPrefs.SetInt (LevelString, Mathf.Min (idx, levels.Count));
  }

  public static bool isMaxLevel(Level level) {
    initLevels ();
    return level.id >= levels.Count;
  }
}
