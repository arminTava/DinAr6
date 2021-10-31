using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class RoomModel 
{
    public int? DefaultMode { get; set; }
    public int[] ModeToUse { get; set; }

    public bool isDirError { get; set; }
}
