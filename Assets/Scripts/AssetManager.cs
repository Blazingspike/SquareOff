using UnityEngine;
using System.Collections;

[System.Serializable]
public class SquareSprite {
  public Sprite center;
  public Sprite edge;
  public MarkedSprite[] marked;
}

[System.Serializable]
public class MarkedSprite {
  public Sprite center;
  public Sprite edge;
}

public class AssetManager : MonoBehaviour {

  public static AssetManager Instance;

  public SquareSprite centerSquare;
  public SquareSprite edgeSquare;

  public SquareCell squarePrefab;
  public LevelCell levelPrefab;
  public GameObject[] markerPrefab;

  public Sprite playableLevelSprite;
  public Sprite nonplayableLevelSprite;
  // Use this for initialization
  void Awake () {
    Instance = this;
  }
}
