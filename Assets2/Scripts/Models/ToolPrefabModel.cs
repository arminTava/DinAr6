using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable()]
public class ToolPrefabModel
{
    public Transform LoadingCircle { get; set; }
    public Button ToolBtn { get; set; }
    public Sprite ImageRun { get; set; }
    public Sprite ImageNotRun { get; set; }

}
