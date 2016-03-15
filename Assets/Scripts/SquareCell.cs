using UnityEngine;
using System.Collections;
using Gamelogic.Grids;

public class SquareCell : SpriteCell {

  private bool isPicked;
  private SquareSprite ss;

  public SquareCell() : base() {
    markIndex = -1;
  }

  public bool IsPicked {
    set {
      isPicked = value;
      if (isPicked) {
        ss = AssetManager.Instance.centerSquare;
      } else {
        ss = AssetManager.Instance.edgeSquare;
      }
    }
    get {
      return isPicked;
    }
  }

  private bool isBorder;

  public bool IsBorder {
    set {
      isBorder = value;
      setSprite ();
    }
    get {
      return isBorder;
    }
  }

  private int markIndex;
  public int MarkIndex {
    set {
      markIndex = value;
    }
    get {
      return markIndex;
    }
  }

  public void setSprite () {
    if (ss == null) {
      ss = AssetManager.Instance.centerSquare;
    }
    if (isBorder) {
      if (markIndex >= 0) {
        this.SpriteRenderer.sprite = ss.marked[markIndex].edge;
      } else {
        this.SpriteRenderer.sprite = ss.edge;
      }
    } else {
      if (markIndex >= 0) {
        this.SpriteRenderer.sprite = ss.marked[markIndex].center;
      } else {
        this.SpriteRenderer.sprite = ss.center;
      }
    }
  }
}
