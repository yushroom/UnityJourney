using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MNetwork))]
public class MNetworkEditor : Editor {
	private MNetwork edit;		
	public string playerInGameName = "in game";
	public string playerInWaitRoomName = "in waitroom";
	
	private bool showParameter = false;
	private bool showGameMenu = false;
	private bool showErrorMessages = false;
	
	
	public void OnEnable(){
		edit = (MNetwork) target;		
	}
	
	public override void OnInspectorGUI () {	
		EditorGUILayout.LabelField("Global parameters", EditorStyles.boldLabel);
		showParameter = EditorGUILayout.Foldout(showParameter, "Show global parameters");
		if(showParameter){
			edit.mainMenuName = EditorGUILayout.TextField("Main menu scene name:", edit.mainMenuName);
			edit.rebuildTime = EditorGUILayout.IntField("Sec. to find new host:", edit.rebuildTime);
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Game menu", EditorStyles.boldLabel);	
		showGameMenu = EditorGUILayout.Foldout(showGameMenu, "Show game menu");
		if(showGameMenu) {
			EditorGUILayout.LabelField("Server rebuilding", EditorStyles.boldLabel);			
			edit.rebuildTitle = EditorGUILayout.TextField("Title:", edit.rebuildTitle);
			edit.rebuildSubTitle = EditorGUILayout.TextField("SubTitle: ", edit.rebuildSubTitle);
			edit.rebuildInfoTxt = EditorGUILayout.TextField("Message on host search:", edit.rebuildInfoTxt);
			edit.rebuildSearchTxt = EditorGUILayout.TextField("Info on host search:", edit.rebuildSearchTxt);
			edit.rebuildSearchWaitTxt = EditorGUILayout.TextField("Wait text:", edit.rebuildSearchWaitTxt);
			edit.rebuildSearchWaitSecTxt = EditorGUILayout.TextField("Seconds text:", edit.rebuildSearchWaitSecTxt);
			edit.rebuildFailedInfoText = EditorGUILayout.TextField("Message on failed rebuild:", edit.rebuildFailedInfoText);
			edit.rebuildFailedTxt = EditorGUILayout.TextField("Info on failed rebuild:", edit.rebuildFailedTxt);
			edit.rebuildFailedTryButton = EditorGUILayout.TextField("Try again button:", edit.rebuildFailedTryButton);
			edit.rebuildFailedExitButton = EditorGUILayout.TextField("Exit game button:", edit.rebuildFailedExitButton);
			
			EditorGUILayout.LabelField("Success rebuilding messages:");		
			edit.rebuildNewHostTxt = EditorGUILayout.TextField("For the new host:", edit.rebuildNewHostTxt);
			edit.rebuildConnexionTxt= EditorGUILayout.TextField("For the players:", edit.rebuildConnexionTxt);
			EditorGUILayout.LabelField("Exit game", EditorStyles.boldLabel);	
			edit.exitTitle = EditorGUILayout.TextField("Title:", edit.exitTitle);
			edit.exitMessage = EditorGUILayout.TextField("Confirmation message:", edit.exitMessage);
			edit.exitButton = EditorGUILayout.TextField("Confirm button:", edit.exitButton);
			edit.exitCancelButton = EditorGUILayout.TextField("Cancel button:", edit.exitCancelButton);
		}
		
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Errors and info messages", EditorStyles.boldLabel);	
		showErrorMessages = EditorGUILayout.Foldout(showErrorMessages, "Show errors and info messages");
		if(showErrorMessages){
			edit.errorGameCreation = EditorGUILayout.TextField("Failed game creation:", edit.errorGameCreation);		
			EditorGUILayout.LabelField("Failed game creation causes:");	
			edit.errorUsedPort = EditorGUILayout.TextField("Port already used:", edit.errorUsedPort);
			edit.errorConnexion = EditorGUILayout.TextField("Failed connexion:", edit.errorConnexion);	
			EditorGUILayout.LabelField("Failed connexion causes:");	
			edit.errorMaxPlayers = EditorGUILayout.TextField("Players number:", edit.errorMaxPlayers);
			edit.errorStartedGame = EditorGUILayout.TextField("Game is starting:", edit.errorStartedGame);
			edit.errorPrivateGame = EditorGUILayout.TextField("Game is private:", edit.errorPrivateGame);
			edit.errorOnlineGame = EditorGUILayout.TextField("Game is online:", edit.errorOnlineGame);	
		}		
	}
}
