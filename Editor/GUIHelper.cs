using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MS.Shell.Editor{
    internal class GUIHelper 
    {
      
        private static int _waitSpineTotal = 12;
        public static GUIContent iconRunning{
            get{
                 var index = Mathf.FloorToInt((float)(EditorApplication.timeSinceStartup % 12)).ToString("00");
                 return EditorGUIUtility.IconContent("d_WaitSpin" + index);
            }
        }

        public static GUIContent iconIdle{
            get{
                return EditorGUIUtility.IconContent("d_winbtn_mac_inact");
            }
        }
        public static GUIContent iconFail{
            get{
                return EditorGUIUtility.IconContent("d_winbtn_mac_close");
            }
        }

        public static GUIContent iconSuccess{
            get{
                 return EditorGUIUtility.IconContent("d_winbtn_mac_max");
            }
        }
        
        public static void Status(ShellExecutor.Status status){
            GUIContent content = GUIContent.none;;
            if(status == ShellExecutor.Status.Idle){
                content = iconIdle;
            }else if(status == ShellExecutor.Status.Running){
                content = iconRunning;
            }else if(status == ShellExecutor.Status.Failed){
                content = iconFail;
            }else if(status == ShellExecutor.Status.Success){
                content = iconSuccess;
            }
            GUILayout.Label(content,GUILayout.Width(20));
        }


    }
}
