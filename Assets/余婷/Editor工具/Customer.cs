//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;

//public  class Customer:Editor
//    {
//    /// <summary>
//    /// 配置文件放入Resources目录，准备打包
//    /// </summary>
//    [MenuItem("Custom/ConfigToResources")]
//    public static void ConfigToResources()
//    {
//        string srcDir = Application.dataPath + "/../Config";
//        string dstDir = Application.dataPath + "/Resources/Config";

//        if (Directory.Exists(dstDir))
//        {
//            Directory.Delete(dstDir, true);
//        }
//        Directory.CreateDirectory(dstDir);

//        foreach (var filePath in Directory.GetFiles(srcDir))
//        {
//            var fileName = Path.GetFileName(filePath);
//            //Debug.Log(fileName);

//            File.Copy(filePath, dstDir + "/" + fileName + ".bytes");
//        }
//    }

//    [MenuItem("Custom/GotoSetup")]
//    public static void OpenSetup()
//    {
//        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/Login.unity");
//    }
//    [MenuItem("Custom/GotoUIEditor")]
//    public static void OpenUIEditor()
//    {
//        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/GamePlaying.unity");
//    }
//    [MenuItem("Custom/GotoMap")]
//    public static void OpenMap()
//    {
//        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/Map.unity");
//    }
//}