using UnityEngine;
using System.Collections;

public class Marker {
  public GameObject obj;
  public int index;

  public Marker(int idx, GameObject obj) {
    this.index = idx;
    this.obj = obj;
  }
}
