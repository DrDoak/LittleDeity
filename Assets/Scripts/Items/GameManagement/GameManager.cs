/*
 * Singleton game manager intended to be persistent across all scenes.
 * Singleton maintained by creating a static instance of the class recursively.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private static GameManager m_instance;
	//public GameObject FXExplosionPrefab;
	public GameObject FXPropertyPrefab;
	public GameObject FXPropertyGetPrefab;
	public GameObject FXExperience;
	public GameObject FXWaterSplash;

	public GameObject CurrentPlayer;
	public GameObject Canvas;
	public GameObject PauseCanvas;

	public static GameManager Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	void Awake()
	{
		
		if (m_instance == null)
		{
			m_instance = this;
			SceneManager.sceneLoaded += InitCanvasOnSceneLoad;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		FindPlayer ();
		Reset ();

	}
	public void FindPlayer() {
		foreach (BasicMovement bm in FindObjectsOfType<BasicMovement>()) {
			if (bm.IsCurrentPlayer) {
				SetPlayer (bm);
				break;
			}
		}
	}
	public void LoadRoom(string roomName) {
		Debug.Log ("---LOADING ROOM: " + roomName);
		GetComponent<SaveObjManager>().ResaveRoom ();
		SceneManager.LoadScene (roomName, LoadSceneMode.Single);
	}

	public void SetPlayer(BasicMovement bm) {
		GetComponent<CameraFollow> ().target = bm.GetComponent<PhysicsSS>();
		GetComponent<CameraFollow> ().initFunct ();
		FindObjectOfType<GUIHandler> ().OnSetPlayer (bm);
		CurrentPlayer = bm.gameObject;
	}

	public static void Reset() {
		SaveObjManager.charContainer = new CharacterSaveContainer ();
		Instance.GetComponent<SaveObjManager> ().SetDirectory ("AutoSave");
		Instance.GetComponent<SaveObjManager>().resetRoomData ();
	}
	public Property GetPropInfo(Property p ) {
		System.Type sysType = p.GetType ();
		return (Property)GetComponentInChildren (sysType);
	}

	public void InitCanvasOnSceneLoad(Scene scene, LoadSceneMode mode) {
		Debug.Log ("Init canvas on scene load");
		InitializeCanvas ();
	}
	public void InitializeCanvas() {
		Debug.Log ("INit canvas");
		Instantiate (Canvas);
		Instantiate (PauseCanvas);
	}
}

/*
 * if (InputManager.GetButtonDown ("Quicksave")) {
			SaveObjManager.Instance.SaveProfile ("QuickSave");
			TextboxManager.StartSequence ("~QuickSave Successful");
		} else if (InputManager.GetButtonDown ("Quickload")) {
			PauseGame.QuickLoad ();
		}*/
