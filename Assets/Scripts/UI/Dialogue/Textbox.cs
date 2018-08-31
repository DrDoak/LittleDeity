using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum DialogueSound { TYPED, SPOKEN, RECORDED};

public class Textbox : MonoBehaviour {

	public DialogueUnit masterSequence;
	GameObject targetedObj;
	public bool typing;
	public DialogueSound m_sound;
	Vector3 lastPos;
	public string FullText;
	public string CurrentText;
	public float timeBetweenChar = 0.01f;
	float sinceLastChar = 0f;
	float sinceLastSound = 0f;
	float pauseTime = 0f;
	float timeSinceStop = 0f;
	public int lastCharacter;
	public float pauseAfterType = 2f;
	TextMeshProUGUI mText;
	public Color tC;
	public bool conclude = false;
	private List<DialogueAction> m_potentialActions;

	public Dictionary<BasicMovement,bool> FrozenCharacters;

	// Use this for initialization
	void Start () {
		mText = GetComponentInChildren<TextMeshProUGUI> ();
		if (!typing) {
			mText.text = FullText;
		}
		m_potentialActions = new List<DialogueAction> ();
		m_potentialActions.Add(new DAPause());
		m_potentialActions.Add (new DATextSpeed ());
		m_potentialActions.Add (new DAWalkTo ());
		m_potentialActions.Add (new DAControl ());
		m_potentialActions.Add (new DAFacingDirection ());
		m_potentialActions.Add (new DAKeyname ());
		m_potentialActions.Add (new DACameraFocus ());
		m_potentialActions.Add (new DASceneChange ());
		m_potentialActions.Add (new DAAnimation ());

		m_potentialActions.Add (new DAEnd ());
		m_potentialActions.Add (new DAJump ());
		m_potentialActions.Add (new DAQuestion ());
	}
	void OnDestroy() {
		conclude = true;

		if (masterSequence != null) {
			masterSequence.parseNextElement ();
		}
		//TextboxManager.Instance.removeTextbox (gameObject);
		/*if (targetedObj.GetComponent<Character> ()) {
			targetedObj.GetComponent<Character> ().onTBComplete ();
		}*/
	}
	public void initColor() {
		GetComponentInChildren<Image> ().color = tC;
		float avgCol = (1.0f - tC.r + 1.0f - tC.g + 1.0f - tC.b)/3f;
		GetComponentInChildren<TextMeshProUGUI> ().color = new Color(avgCol,avgCol,avgCol,tC.a + 0.5f);
	}
	public void setColor(Color C) {
		tC = C;
	}

	// Update is called once per frame
	/* Cutscene scripting guide:
	 *  Normal text is shown as dialogue for the starting character.
	 * 	Using the '|' character or the enter character will create a new textbox.
	 *  At the start of a new textbox if the colon character is found within the first 18 characters, the game will attempt to search
	 *  For the character and make the dialogue come from that character instead.
	 *  
	 *  The character ` surrounds a special block.
	 * A number will result in a pause for a certain amount of time.
	 * $ will change the text speed
	 * 
	 * --NOT IMPLEMENTED YET--
	 * Any text means the character would try to do an animation
	 * --NOT IMPLEMENTED YET--
	 * */

	private void processSpecialSection() {
		string actStr = "";
		char nextChar = FullText.ToCharArray () [lastCharacter];
		int numSpecials = 1;
		while (numSpecials > 0 && lastCharacter < FullText.Length - 1) {
			actStr += nextChar;
			lastCharacter++;
			nextChar = FullText.ToCharArray () [lastCharacter];
			if (nextChar == '>')
				numSpecials--;
			else if (nextChar == '<')
				numSpecials++;
		}
		//Debug.Log ("Action string: " + actStr);
		//Debug.Log ("Length on execution: " + m_potentialActions.Count);
		List<DialogueAction> executedActions = new List<DialogueAction> ();
		foreach (DialogueAction da in m_potentialActions) {
			if (da.IsExecutionString (actStr))
				executedActions.Add (da);
		}
		foreach (DialogueAction da in executedActions)
			da.PerformAction (actStr, this);
		lastCharacter++;
	}

	private void processNormalChar(char nextChar) {
		if (sinceLastSound > 0.15f) {
			sinceLastSound = 0f;
			playSound ();
		}
		CurrentText += nextChar;
		mText.text = CurrentText;
		sinceLastChar = 0f;
	}

	private void processChar() {
		lastCharacter++;
		char nextChar = FullText.ToCharArray () [lastCharacter - 1];
		if (nextChar == '<') {
			processSpecialSection ();
		} else {
			processNormalChar (nextChar);
		}
	}

	void Update () {
		if (targetedObj != null) {
			transform.position += targetedObj.transform.position-lastPos;
			lastPos = targetedObj.transform.position;
		}
		if (typing ) {
			if (lastCharacter < FullText.Length) { 
				sinceLastChar += Time.deltaTime;
				sinceLastSound += Time.deltaTime;
				if (pauseTime > 0f) {
					pauseTime -= Time.deltaTime;
				} else if (sinceLastChar > timeBetweenChar) {
					processChar ();
				}
			} else {
				timeSinceStop += Time.deltaTime;
				if (timeSinceStop > pauseAfterType) {
					Destroy (gameObject);
				}
			}

		}
	}
	private void playSound() {
		switch (m_sound) {
		case DialogueSound.RECORDED:
			FindObjectOfType<AudioManager> ().PlayClipAtPos (UIList.Instance.SFXDialogueStatic, FindObjectOfType<CameraFollow> ().transform.position, 0.15f, 0f, 0.1f);
			break;
		case DialogueSound.TYPED:
			FindObjectOfType<AudioManager> ().PlayClipAtPos (UIList.Instance.SFXDialogueClick, FindObjectOfType<CameraFollow> ().transform.position, 0.2f, 0f, 0.25f);
			break;
		case DialogueSound.SPOKEN:
			FindObjectOfType<AudioManager> ().PlayClipAtPos (UIList.Instance.SFXDialogueSpeak, FindObjectOfType<CameraFollow> ().transform.position, 0.2f, 0f, 0.25f);
			break;
		default:
			break;
		}
	}
	public void SetPause(float pt) {
		pauseTime = pt;
	}
	public void setTargetObj(GameObject gameObj) {
		targetedObj = gameObj;
		if (targetedObj != null)
			lastPos = gameObj.transform.position;
	}
	public void setTypeMode(bool type) {
		typing = type;
		if (type) {
			CurrentText = "";
			lastCharacter = 0;
		} else {
			CurrentText = FullText;
		}
	}
	public void setText(string text) {
		FullText = text;
	}

	private void playAnimation(string targetChar) {
		string[] chars = targetChar.Split(',');
		if (chars.Length < 2)
			return;
		GameObject character = GameObject.Find (chars [0]);
		string anim = chars [1];
		if (character != null && character.GetComponent<AnimatorSprite>()) {
			bool res = character.GetComponent<AnimatorSprite> ().Play (anim);
		}
	}

	public void FreezeCharacter(BasicMovement bm, bool isFrozen = true) {
		if (!FrozenCharacters.ContainsKey (bm))
			FrozenCharacters.Add (bm, bm.IsCurrentPlayer);
		bm.SetAutonomy (!isFrozen);
	}
}