using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Luminosity.IO;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
	private static PauseGame m_instance;

	public static bool isPaused = false;
	public static bool CanPause = true;

	GameObject m_pauseMenuUI;
	GameObject m_saveScreen;
	GameObject m_loadScreen;
	GameObject m_deadScreen;
	GameObject m_warningScreen;
	GameObject m_controlMap;

	float m_slowingSpeed = 0.0f;
	float m_speedingSpeed = 0.0f;

	public delegate void ButtonClickEvent();
	ButtonClickEvent m_buttonEvent;
	ButtonClickEvent m_cancelEvent;
	GameObject WarningPrevious;
	public bool ReturnMain = false;

	[SerializeField] public Scene startMenu;
	public static PauseGame Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	void Awake() {
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
		m_pauseMenuUI = transform.GetChild (0).gameObject;
		m_saveScreen = transform.GetChild (1).gameObject;
		m_loadScreen = transform.GetChild (2).gameObject;
		m_deadScreen = transform.GetChild (3).gameObject;
		m_warningScreen = transform.GetChild (4).gameObject;
		m_controlMap = transform.Find ("KeyboardMap").gameObject;

		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		m_deadScreen.SetActive (false);
		m_warningScreen.SetActive (false);
		m_controlMap.SetActive (false);
	}
		
	void Update () {
		if (CanPause && InputManager.GetButtonDown ("Pause"))
		{
			if (isPaused)
				Resume();
			else
				Pause();
		}
		speedSlowManage ();
	}

	void speedSlowManage() {
		if (m_slowingSpeed > 0.0f && Time.timeScale > 0f) {
			Time.timeScale = Mathf.Max (0f, Time.timeScale - (m_slowingSpeed * Time.unscaledDeltaTime));
			if (Time.timeScale == 0f) {
				m_slowingSpeed = 0f;
			}
		}
		if (m_speedingSpeed > 0.0f && Time.timeScale < 1f) {
			Time.timeScale = Mathf.Min (1f, Time.timeScale + (m_speedingSpeed * Time.unscaledDeltaTime));
			if (Time.timeScale == 1f) {
				m_speedingSpeed = 0f;
			}
		}
	}

	public static void Resume() {
		m_instance.mResume ();
		m_instance.m_slowingSpeed = 0f;
	}

	public void mResume()
	{
		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		m_deadScreen.SetActive (false);
		m_controlMap.SetActive (false);
		Time.timeScale = 1f;
		PauseGame.CanPause = true;
		isPaused = false;
		AudioManager.instance.ResumeMusic ();
	}

	public static void Pause(bool drawMenu = true) {
		m_instance.mPause (drawMenu);
	}
	public static void SlowToPause(float slowSpeed = 1.0f) {
		m_instance.m_slowingSpeed = slowSpeed;
		isPaused = true;
	}
	public static void SpeedToResume(float speedSpeed = 1.0f) {
		m_instance.m_speedingSpeed = speedSpeed;
		isPaused = false;
	}

	void mPause(bool drawMenu = true)
	{
		if (drawMenu) {
			m_pauseMenuUI.SetActive (true);
			SetFirstOption ();
		}
		Time.timeScale = 0f;
		isPaused = true;
		AudioManager.instance.PauseMusic ();
	}
//-------------------------------------------------
	public void MenuResume() {
		mResume ();
	}
	public void MenuNew() {
		SaveObjManager.Instance.resetRoomData ();
		SceneManager.LoadScene ("LB_Intro");
		Resume ();
	}
	public void MenuSave() {
		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (true);
		m_saveScreen.GetComponent<SaveLoadMenu> ().Refresh ();
		m_saveScreen.GetComponent<SaveLoadMenu> ().Reset ();
	}
	public void MenuLoad() {
		m_pauseMenuUI.SetActive(false);
		m_loadScreen.SetActive (true);
		m_loadScreen.GetComponent<SaveLoadMenu> ().Refresh ();
		m_loadScreen.GetComponent<SaveLoadMenu> ().Reset ();
		PauseGame.CanPause = false;
	}
	public void MenuKeyBoardMap() {
		m_pauseMenuUI.SetActive(false);
		m_controlMap.SetActive (true);
		PauseGame.CanPause = false;
		EventSystem.current.SetSelectedGameObject(m_controlMap.transform.Find ("main_panel").Find("back_button").gameObject);
	}
	public void MenuMainMenu() {
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu"); //get rid of this hardcode
	}
	public void MenuExit() {
		Application.Quit();
	}
	public void ReturnToPause() {
		if (!ReturnMain)
			m_pauseMenuUI.SetActive(true);
		else
			EventSystem.current.SetSelectedGameObject(FindObjectOfType<MainMenu>().transform.Find("MainMenu").Find("New Game").gameObject);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		m_deadScreen.SetActive (false);
		m_controlMap.SetActive (false);
		EventSystem.current.SetSelectedGameObject(m_pauseMenuUI.transform.Find("Resume Button").gameObject);
		PauseGame.CanPause = true;
	}
	public static void OnPlayerDeath() {
		SlowToPause ();
		PauseGame.CanPause = false;
		Instance.m_pauseMenuUI.SetActive(false);
		Instance.m_saveScreen.SetActive (false);
		Instance.m_loadScreen.SetActive (false);
		Instance.m_deadScreen.SetActive (true);
		Instance.m_deadScreen.GetComponent<SaveLoadMenu> ().Refresh ();
		Instance.m_deadScreen.GetComponent<SaveLoadMenu> ().Reset ();
		AudioManager.instance.StopMusic ();
	}

	public static void DisplayWarning(string warningMessage, GameObject oldMenu, ButtonClickEvent func, string title="Warning", ButtonClickEvent cancelFunc = null) {
		Instance.m_warningScreen.SetActive (true);
		oldMenu.SetActive (false);
		Instance.WarningPrevious = oldMenu;
		Instance.m_warningScreen.transform.Find ("Message").GetComponent<TextMeshProUGUI> ().SetText (warningMessage);
		Instance.m_warningScreen.transform.Find ("Title").GetComponent<TextMeshProUGUI> ().SetText (title);
		Instance.m_buttonEvent = func;
		EventSystem.current.SetSelectedGameObject(Instance.m_warningScreen.transform.Find ("Cancel").gameObject);
		if (cancelFunc == null) {
			Instance.m_cancelEvent = Instance.genericCancel;
		} else {
			Instance.m_cancelEvent = cancelFunc;
		}
	}
	void genericCancel() {}

	public void OnCancelWarning() {
		m_warningScreen.SetActive (false);
		WarningPrevious.SetActive (true);
		m_cancelEvent ();
	}
	public void OnConfirmWarning() {
		m_warningScreen.SetActive (false);
		WarningPrevious.SetActive (true);
		m_buttonEvent ();
	}
	public static void QuickLoad() {
		PauseGame.Pause (false);
		string w = "Load Last QuickSave? ";
		w += "\n All unsaved Progress will be lost.";
		PauseGame.DisplayWarning (w, Instance.m_pauseMenuUI, Instance.quickLoad,"Warning",Instance.SetFirstOption);
	}
	private void SetFirstOption() {
		EventSystem.current.SetSelectedGameObject(m_pauseMenuUI.transform.Find("Resume Button").gameObject);
	}
	private void quickLoad() {
		bool result = SaveObjManager.Instance.LoadProfile ("QuickSave");
		if (result == false) {
			SaveObjManager.Instance.LoadProfile ("AutoSave");
		}
		PauseGame.Resume ();
	}
}
