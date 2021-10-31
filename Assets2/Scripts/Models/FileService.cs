using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public static class FileService
    {
        public static string AssetPath = Path.Combine(Application.persistentDataPath, "AssetData");
        public static string MarkerPath = Path.Combine(Application.persistentDataPath, "MarkerData");
        public static string GPSPath = Path.Combine(Application.persistentDataPath, "GPSData");
        public static string RoomPath = Path.Combine(Application.persistentDataPath, "Rooms");
        public static string FileListPath = Path.Combine(Application.persistentDataPath, "AssetData", "List.dat");
        public static string EncodeDoublePoint = "%3A";
        public static string EncodeSlash = "%2F";
        public static string SplitSeperator = "!%";
    }
}
