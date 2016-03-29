using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level {
  public int id;
  public int gridWidth;
  public int gridHeight;
  public int[] numMarked;
  public string description;

  public Level () {
    id = 0;
    gridWidth = 4;
    gridHeight = 4;
    description = "Align the picked color to the cube";
  }

  public Level (int id, int gridWidth, int gridHeight,
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

  // 0 - easy; 1 - standard; 2 - hard; 3 - insane
  public static string ModeString = "rubic2d-mode";
  public static string MaxModeString = "rubic2d-max-mode";

  public static string CurrentLevelPrefix = "rubic2d-current-level-";
  public static string MaxLevelPrefix = "rubic2d-max-level-";

  public static string AdsCounter = "rubic2d-ads-counter";

  private static void initLevels () {
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
    levels.Add (new Level (16, 6, 6, 2, 1, 2, 1));
    levels.Add (new Level (17, 6, 6, 2, 2, 2, 2));
    levels.Add (new Level (18, 6, 6, 3, 2, 3, 2));
    levels.Add (new Level (19, 6, 6, 3, 3, 3, 3));
  }

  public static Level getLevel (int idx) {
    if (levels == null) {
      initLevels ();
    }
    if (idx >= levels.Count) {
      return levels [levels.Count - 1];
    }
    return levels [idx];
  }

  public static bool isMaxLevel (Level level) {
    initLevels ();
    return level.id >= levels.Count;
  }

  public static int getCurrentMode () {
    if (PlayerPrefs.HasKey (ModeString)) {
      return PlayerPrefs.GetInt (ModeString);
    }
    return 0;
  }

  public static void setMaxMode(int mode) {
    PlayerPrefs.SetInt (MaxModeString, mode);
  }

  public static int getMaxMode () {
    if (!PlayerPrefs.HasKey (MaxModeString)) {
      // Default set to normal.
      PlayerPrefs.SetInt (MaxModeString, 0);
    }
    return PlayerPrefs.GetInt (MaxModeString);
  }

  public static Level getCurrentLevel () {
    int mode = getCurrentMode ();
    string currentLevelStr = CurrentLevelPrefix + mode.ToString ();
    if (PlayerPrefs.HasKey (currentLevelStr)) {
      return getLevel (PlayerPrefs.GetInt (currentLevelStr));
    }
    setCurrentLevel (0);
    return getLevel (0);
  }

  public static void setCurrentLevel (int idx) {
    initLevels ();
    int mode = getCurrentMode ();
    string s = CurrentLevelPrefix + mode.ToString ();
    PlayerPrefs.SetInt (s, Mathf.Min (idx, levels.Count));
  }

  public static void setMaxLevel (int mode, int idx) {
    initLevels ();
    string s = MaxLevelPrefix + mode.ToString ();
    PlayerPrefs.SetInt (s, Mathf.Min (idx, levels.Count));
  }

  public static int getMaxLevelId () {
    int mode = getCurrentMode ();
    string levelStr = MaxLevelPrefix + mode.ToString ();
    if (PlayerPrefs.HasKey (levelStr)) {
      return PlayerPrefs.GetInt (levelStr);
    }
    setMaxLevel (mode, 0);
    return 0;
  }

  public static void setCurrentMode (int m) {
    PlayerPrefs.SetInt (ModeString, m);
  }

  public static float getCurrentMoveTimer () {
    int mode = getCurrentMode ();
    return getMoveTimer (mode);
  }

  public static float getMoveTimer (int mode) {
    switch (mode) {
      case 0:
        // Easy
        return 120f;
      case 1:
        // Normal
        return 60f;
      case 2:
        // Hard
        return 30f;
      case 3:
        // Insane
        return 10f;
    }
    return 40f;
  }

  public static string getModeString (int mode) {
    switch (mode) {
      case 0:
        // Easy
        return "Easy";
      case 1:
        // Normal
        return "Normal";
      case 2:
        // Hard
        return "Hard";
      case 3:
        // insane
        return "Insane";
    }
    return "Mode";
  }

  public static string getModeInfo (int mode) {
    switch (mode) {
      case 0:
        // Easy
        return "2 mins a game";
      case 1:
        // Normal
        return "1 min a game";
      case 2:
        // Hard
        return "30 sec a game";
      case 3:
        // insane
        return "10 sec a game!";
    }
    return "Free game";
  }

  public static bool showAds() {
    int counter = 0;
    if (PlayerPrefs.HasKey (AdsCounter)) {
      counter = PlayerPrefs.GetInt (AdsCounter);
    }
    PlayerPrefs.SetInt (AdsCounter, counter + 1);
    return counter % 4 == 3;
  }
}
