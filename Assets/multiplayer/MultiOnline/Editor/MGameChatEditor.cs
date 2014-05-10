using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MGameChat))]
public class MGameChatEditor : Editor {

	private MGameChat edit;
	
	public void OnEnable(){
		edit = (MGameChat) target;		
	}
	
	public override void OnInspectorGUI () {	
		edit.chatButtonTxt = EditorGUILayout.TextField("Send button:", edit.chatButtonTxt);
		
		
	
	}
}
