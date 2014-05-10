using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MMenu))]
public class MMenuEditor : Editor {
	private MMenu edit;	
	
	private string[] subMenuProfiButtons = new string[2]{"Player", "Online account"};
	private string[] multiButtonsOptions = new string[4]{"Online games", "Network games", "Join game from IP", "Create game"};	
	
	private bool showParam = false;
	private bool showTxt = false;
	private bool showSize = false;
	public void OnEnable(){
		edit = (MMenu) target;	
	}
	
	public override void OnInspectorGUI () {	
		EditorGUILayout.LabelField("Global parameters", EditorStyles.boldLabel);
		showParam = EditorGUILayout.Foldout(showParam, "Show global parameters content");
		if(showParam) {	
			edit.canJoinStartedGame = EditorGUILayout.Toggle("Can join started game: ", edit.canJoinStartedGame);
			edit.defaultPort = EditorGUILayout.IntField("Default game port: ", edit.defaultPort);
			edit.waitRoomName = EditorGUILayout.TextField("Wait room name: ", edit.waitRoomName);
			edit.networkPrefab = (GameObject)EditorGUILayout.ObjectField("Network prefab: ", edit.networkPrefab, typeof(GameObject), false);
			edit.playerDataPrefab = (GameObject)EditorGUILayout.ObjectField("Player data prefab: ", edit.playerDataPrefab, typeof(GameObject), false);
		}
		
	
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Main menu content", EditorStyles.boldLabel);
		showTxt = EditorGUILayout.Foldout(showTxt, "Show main menu content");
		if(showTxt) {	
			EditorGUILayout.LabelField("Menu buttons", EditorStyles.boldLabel);
			edit.menuProfilButton = EditorGUILayout.TextField("Profil button : ", edit.menuProfilButton);
			edit.menuMultiButton = EditorGUILayout.TextField("Multiplayer button : ", edit.menuMultiButton);
			edit.menuExitButton = EditorGUILayout.TextField("Exit button : ", edit.menuExitButton);
					
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Titles", EditorStyles.boldLabel);
			edit.menuTitle = EditorGUILayout.TextField("Main menu title : ", edit.menuTitle);
			edit.profilTitle = EditorGUILayout.TextField("Profil title : ", edit.profilTitle);
			edit.mutliTitle = EditorGUILayout.TextField("Multiplayer title : ", edit.mutliTitle);
			
				
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Submenus buttons", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Submenus Profil");
			for(int i = 0; i < edit.subMenuProfiButtons.Length; i++){
				edit.subMenuProfiButtons[i] = EditorGUILayout.TextField("Button \""+subMenuProfiButtons[i]+"\": ", edit.subMenuProfiButtons[i]);
			}
			EditorGUILayout.LabelField("Submenus MultiPlayer");
			for(int i = 0; i < edit.multiButtonsOptions.Length; i++){
				edit.multiButtonsOptions[i] = EditorGUILayout.TextField("Button \""+multiButtonsOptions[i]+"\": ", edit.multiButtonsOptions[i]);
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Global messages", EditorStyles.boldLabel);
			edit.waitMessage = EditorGUILayout.TextField("Wait : ", edit.waitMessage);
			edit.errorIncompleteFields = EditorGUILayout.TextField("Error incomplet form: ", edit.errorIncompleteFields);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Specific messages for using of Lan & Online", EditorStyles.boldLabel);
			edit.createChooseTxt = EditorGUILayout.TextField("Choose game type: ", edit.createChooseTxt);
			edit.multiCreateLanButton = EditorGUILayout.TextField("Create network game: ", edit.multiCreateLanButton);
			edit.multiCreateOnlineButton = EditorGUILayout.TextField("Create online game: ", edit.multiCreateOnlineButton);
			
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Fields size parameters", EditorStyles.boldLabel);
		showSize = EditorGUILayout.Foldout(showSize, "Show fields size parameters");
		if(showSize) {		
			EditorGUILayout.LabelField("Sidebar", EditorStyles.boldLabel);
			edit.sizeMenuButtonX = EditorGUILayout.IntField("Menu buttons width: ", edit.sizeMenuButtonX);
			edit.sizeMenuButtonY = EditorGUILayout.IntField("Menu buttons height: ", edit.sizeMenuButtonY);
			
			edit.sizeSubMenuButtonX = EditorGUILayout.IntField("Submenu buttons width: ", edit.sizeSubMenuButtonX);
			edit.sizeSubMenuButtonY = EditorGUILayout.IntField("Submenu buttons height: ", edit.sizeSubMenuButtonY);
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Page content", EditorStyles.boldLabel);
	
			edit.sizeTitleX = EditorGUILayout.IntField("Page title width: ", edit.sizeTitleX);
			edit.sizeTitleY = EditorGUILayout.IntField("Page title height: ", edit.sizeTitleY);
			
			edit.sizePageButtonX = EditorGUILayout.IntField("Page buttons width: ", edit.sizePageButtonX);
			edit.sizePageButtonY = EditorGUILayout.IntField("Page buttons type height: ", edit.sizePageButtonY);
			
			edit.sizeButtonX = EditorGUILayout.IntField("Forms buttons width: ", edit.sizeButtonX);
			edit.sizeButtonY = EditorGUILayout.IntField("Forms buttons height: ", edit.sizeButtonY);
						
			edit.sizeFieldX = EditorGUILayout.IntField("Forms fields width: ", edit.sizeFieldX);
			edit.sizeSmallFieldX = EditorGUILayout.IntField("Forms small fields width: ", edit.sizeSmallFieldX);
			edit.sizeFieldY = EditorGUILayout.IntField("Forms fields height: ", edit.sizeFieldY);
			
			edit.sizeLabelX = EditorGUILayout.IntField("Forms labels width: ", edit.sizeLabelX);
			edit.sizeLabelY = EditorGUILayout.IntField("Forms labels height: ", edit.sizeLabelY);	
		}
	}	
}
