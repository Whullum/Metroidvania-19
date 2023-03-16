using UnityEngine;
     
     namespace DebugStuff
     {
         public class DebugLogger : MonoBehaviour
         {
     //#if !UNITY_EDITOR
             static string myLog = "";
             private string output;
             private string stack;
     
             void OnEnable()
             {
                 Application.logMessageReceived += Log;
             }
     
             void OnDisable()
             {
                 Application.logMessageReceived -= Log;
             }
     
             public void Log(string logString, string stackTrace, LogType type)
             {
                 if (type == LogType.Error || type == LogType.Warning) {
                    output = logString;
                    stack = stackTrace;
                    myLog = output + "\n" + myLog;
                    if (myLog.Length > 5000)
                    {
                        myLog = myLog.Substring(0, 4000);
                    }
                 }
             }
     
             void OnGUI()
             {
                 //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
                 {
                     myLog = GUI.TextArea(new Rect(10, 10, Screen.width / 3, Screen.height / 3), myLog);
                 }
             }
     //#endif
         }
     }