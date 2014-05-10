using UnityEngine;
using UnityEditor; 
using System.Collections;
using System;

[CustomEditor(typeof(MWaitRoom))]
public class MWaitRoomEditor : Editor {		
	private MWaitRoom edit;	
	private bool showParameter = false;
	private bool showMaps = false;
	private bool showPageContent = false;
		
	public void OnEnable(){
		edit = (MWaitRoom) target;	
	}
	
	public override void OnInspectorGUI () {	
		EditorGUILayout.LabelField("Global parameters", EditorStyles.boldLabel);
		showParameter = EditorGUILayout.Foldout(showParameter, "Show global parameters");
		if(showParameter) {		
			edit.useLoadTimer  = EditorGUILayout.Toggle("Use load timer:", edit.useLoadTimer);	
			if(edit.useLoadTimer){
				edit.loadTimerTime = EditorGUILayout.IntField("Timer time:", edit.loadTimerTime);
				edit.useLoadChatTimer  = EditorGUILayout.Toggle("Display on chat:", edit.useLoadChatTimer);	
			}				
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Game maps scenes", EditorStyles.boldLabel);
		showMaps = EditorGUILayout.Foldout(showMaps, "Show / Add game maps scenes");
		if(showMaps){
			edit.displayMapScreen  = EditorGUILayout.Toggle("Display maps screen :", edit.displayMapScreen);	
			if(edit.displayMapScreen){
				edit.mapScreenX = EditorGUILayout.IntField("Maps screen width:", edit.mapScreenX);
				edit.mapScreenY = EditorGUILayout.IntField("Maps screen height:", edit.mapScreenY);
			}
			
			GUILayout.Space(10); 
			if(GUILayout.Button(" Add new map", GUILayout.Width(100))){	
				AddMap();
			}	
			GUILayout.Space(10); 
			for(int i = 0 ; i < edit.maps.Length; i++){
				edit.maps[i] = EditorGUILayout.TextField("Map "+(i+1)+" scene name:", edit.maps[i]);
				edit.mapsScreen[i] = EditorGUILayout.ObjectField(edit.mapsScreen[i], typeof(Texture), false, GUILayout.Width(100)) as Texture;
				if(GUILayout.Button("Delete map "+(i+1), GUILayout.Width(100))){	
					DeleteMap(i);						
				}		
			}
		}
		
			
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Page content", EditorStyles.boldLabel);
		showPageContent = EditorGUILayout.Foldout(showPageContent, "Show page content");
		if(showPageContent){				
			
			EditorGUILayout.LabelField("Player list panel",  EditorStyles.boldLabel);
			edit.playerListTitle= EditorGUILayout.TextField("Panel title:", edit.playerListTitle);
			edit.playerListHostTxt = EditorGUILayout.TextField("Player host text:", edit.playerListHostTxt);
			edit.playerListClientTxt= EditorGUILayout.TextField("Player client text:", edit.playerListClientTxt);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game panel",  EditorStyles.boldLabel);
			edit.gamePanelTitle= EditorGUILayout.TextField("Panel title:", edit.gamePanelTitle);
			edit.gamePanelPlayersRequireTxt= EditorGUILayout.TextField("Require players text:", edit.gamePanelPlayersRequireTxt);
			edit.gamePanelPlayersMaxTxt= EditorGUILayout.TextField("Max players text:", edit.gamePanelPlayersMaxTxt);
			edit.gamePanelPlayersNbrTxt= EditorGUILayout.TextField("Total players text:", edit.gamePanelPlayersNbrTxt);
			edit.gamePanelMapTxt= EditorGUILayout.TextField("Game map text:", edit.gamePanelMapTxt);
			edit.gamePanelLoadButton= EditorGUILayout.TextField("Load button:", edit.gamePanelLoadButton);
			edit.gamePanelJoinButton= EditorGUILayout.TextField("Join game button:", edit.gamePanelJoinButton);
			edit.gamePanelWaitButton= EditorGUILayout.TextField("Wait for loading button:", edit.gamePanelWaitButton);
			edit.gamePanelLoadMessage= EditorGUILayout.TextField("Loading game message:", edit.gamePanelLoadMessage);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Select map panel",  EditorStyles.boldLabel);
			edit.gamePanelMapTitle= EditorGUILayout.TextField("Panel title:", edit.gamePanelMapTitle);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Chat panel",  EditorStyles.boldLabel);
			edit.chatTitle= EditorGUILayout.TextField("Panel title:", edit.chatTitle);
			edit.chatButtonTxt= EditorGUILayout.TextField("Send button:", edit.chatButtonTxt);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Exit panel",  EditorStyles.boldLabel);
			edit.exitButtonTxt= EditorGUILayout.TextField("Exit button", edit.exitButtonTxt);
			edit. exitTitle= EditorGUILayout.TextField("Panel title:", edit. exitTitle);
			edit.exitTxt= EditorGUILayout.TextField("Exit text:", edit.exitTxt);
			edit.exitConfirmButtonTxt= EditorGUILayout.TextField("Confirm button", edit.exitConfirmButtonTxt);
			edit. exitCancelButtonTxt= EditorGUILayout.TextField("Cancel button", edit. exitCancelButtonTxt);
		}
	}
	
	private void DeleteMap(int mapKey){
		string[] maps = new string[edit.maps.Length-1];
		Texture[] mapsScreen = new Texture[edit.maps.Length-1];
		int j = 0;
		for(int i = 0; i < edit.maps.Length; i++){
			if(i != mapKey){
				maps[j] = edit.maps[i];
				mapsScreen[j] = edit.mapsScreen[i];
				j++;
			}
		}
		edit.maps = maps;
		edit.mapsScreen = mapsScreen;
	}
	
	private void AddMap(){
		if(edit.maps.Length > 0){
			string[] maps = new string[edit.maps.Length+1];
			Texture[] mapsScreen = new Texture[edit.maps.Length+1];
			int j = 0;
			for(int i = 0; i < edit.maps.Length; i++){
				maps[j] = edit.maps[i];
				mapsScreen[j] = edit.mapsScreen[i];
				j++;			
			}
			maps[j] = "";
			mapsScreen[j] = null;
			edit.maps = maps;
			edit.mapsScreen = mapsScreen;
		} else {
			edit.maps = new string[1];
			edit.mapsScreen = new Texture[1];
			
		}
	}
	
}
