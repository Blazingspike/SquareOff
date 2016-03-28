using UnityEngine;
using System.Collections;
using Gamelogic.Grids;

public class LevelCell : SpriteCell {

  private bool isPlayable;
  public bool IsPlayable {
    set {
      isPlayable = value;
      if (isPlayable) {
        this.SpriteRenderer.sprite =
          AssetManager.Instance.playableLevelSprite;
      } else {
        this.SpriteRenderer.sprite =
          AssetManager.Instance.nonplayableLevelSprite;
      }
    }
    get {
      return isPlayable;
    }
  }

  private int level;
  public int Level {
    set {
      level = value;
      this.gameObject.GetComponent<TextMesh> ().text = level.ToString ();
    }
    get {
      return level;
    }
  }
}
