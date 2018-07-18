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
	int lastCharacter;
	public float pauseAfterType = 2f;
	TextMeshProUGUI mText;
	public Color tC;
	public bool conclude = false;

	public Dictionary<BasicMovement,bool> FrozenCharacters;

	// Use this for initialization
	void Start () {
		mText = GetComponentInChildren<TextMeshProUGUI> ();
		if (!typing) {
			mText.text = FullText;
		}
		//initColor ();
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
	 * > will make a character walk to another character. Needs to be followed by the character name.
	 * < will make them walk away 
	 * ] will make them face a character
	 * [ will make them face away.
	 * Any text means the character would try to do an animation
	 * --NOT IMPLEMENTED YET--
	 * */
	void Update () {
		if (targetedObj != null) {
			transform.position += targetedObj.transform.position-lastPos;
			//transform.position = targetedObj.transform.position;
			lastPos = targetedObj.transform.position;
		}
		if (typing ) {
			if (lastCharacter < FullText.Length) { 
				sinceLastChar += Time.deltaTime;
				sinceLastSound += Time.deltaTime;
				if (sinceLastChar > timeBetweenChar) {
					if (pauseTime > 0f) {
						pauseTime -= Time.deltaTime;
					} else {
						lastCharacter++;
						char nextChar = FullText.ToCharArray () [lastCharacter - 1];
						if (nextChar == '`') {
							//Debug.Log ("Start special section");
							string actStr = "";
							lastCharacter++;
							nextChar = FullText.ToCharArray () [lastCharacter - 1];
							//Debug.Log (nextChar);
							string action = "pause";
							float res;
							string test = "";
							test += nextChar;
							if (float.TryParse (test,out res)) {
							} else {
								if (nextChar == '!') {
									action = "control";
								} else if (nextChar == '@') {
									action = "camera";
								} else if (nextChar == '#') {
									action = "scene";
								} else if (nextChar == ']') {
									action = "faceTowards";
								} else if (nextChar == '[') {
									//Debug.Log ("Correct Char");
									action = "faceAway";
								} else if (nextChar == '>') {
									action = "walkTowards";
								} else if (nextChar == '<') {
									action = "walkAway";
								} else if (nextChar == '$') {
									action = "textSpeed";
								} else if (nextChar == '&'){
									action = "key";
								} else {
									lastCharacter--;
									action = "animation";
								}
								lastCharacter++;
								nextChar = FullText.ToCharArray () [lastCharacter - 1];
							}
							bool numFound = false;
							string num = "";
							//string targetChar = null;
							while (nextChar != '`') {
								if ((action == "walkTowards" || action == "walkAway") && nextChar == '-') {
									numFound = true;
								} else {
									if (numFound == true) {
										num += nextChar;
									} else {
										actStr += nextChar;
									}
									lastCharacter++;
									nextChar = FullText.ToCharArray () [lastCharacter - 1];
								}

							}
							if (action == "control") {
								toggleControl (actStr);
							} else if (action == "camera") {
								cameraTarget (actStr);
							} else if (action == "walkTowards") {
								//Debug.Log ("Walking towards: " + actStr);
								walkToPoint (actStr);
								/*if (num.Length < 1) {
									//masterSequence.walkToChar (targetChar, actStr, 1f);
								} else {
									//masterSequence.walkToChar (targetChar, actStr, float.Parse(num));
								}*/
							} else if (action == "walkAway") {
							} else if (action == "faceTowards") {
								facePoint (actStr);
								//Debug.Log ("Facing towards");
								//masterSequence.turnTowards (targetChar, actStr, true);

							} else if (action == "faceAway") {
								facePoint (actStr, true);
								//Debug.Log ("facing away");
								//masterSequence.turnTowards (targetChar, actStr, false);
							} else if (action == "animation") {
								playAnimation (actStr);
							} else if (action == "textSpeed") {
								timeBetweenChar = float.Parse (actStr);
							} else if (action == "key") {
								CurrentText += TextboxManager.GetKeyString (actStr);
							} else if (action == "scene") {
								Initiate.Fade (actStr, Color.white, 2.0f);
							} else {
								pauseTime = float.Parse (actStr);
							}
						} else {
							if (sinceLastSound > 0.15f) {
								sinceLastSound = 0f;
								playSound ();
							}
							CurrentText += nextChar;
							mText.text = CurrentText;
							sinceLastChar = 0f;
						}
					}
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

	private void cameraTarget(string targetChar) {
		GameObject target = GameObject.Find (targetChar);
		if (target != null && target.GetComponent<PhysicsSS>()) {
			FindObjectOfType<CameraFollow> ().target = target.GetComponent<PhysicsSS>();
		}
	}

	private void walkToPoint(string targetChar) {
		string[] chars = targetChar.Split(',');
		if (chars.Length < 2)
			return;
		GameObject character = GameObject.Find (chars[0]);
		GameObject target = GameObject.Find (chars[1]);
		if (character != null && character.GetComponent<BasicMovement>()) {
			character.GetComponent<BasicMovement> ().SetTargetPoint (target.transform.position,1.0f);
		}
	}

	private void facePoint(string targetChar, bool invert = false) {
		string[] chars = targetChar.Split('-');
		if (chars.Length < 2)
			return;
		GameObject character = GameObject.Find (chars[0]);
		GameObject target = GameObject.Find (chars[1]);
		if (character != null && character.GetComponent<PhysicsSS>()) {
			if (invert)
				character.GetComponent<PhysicsSS> ().SetDirection (target.transform.position.x > character.transform.position.x);
			else
				character.GetComponent<PhysicsSS> ().SetDirection (target.transform.position.x < character.transform.position.x);
		}
	}

	private void toggleControl(string targetChar) {
		GameObject target = GameObject.Find (targetChar);
		if (target != null && target.GetComponent<BasicMovement>() != null) {
			BasicMovement bm = target.GetComponent<BasicMovement>();
			if (!FrozenCharacters.ContainsKey (bm))
				FrozenCharacters.Add (bm, bm.IsCurrentPlayer);
			bm.IsCurrentPlayer = !bm.IsCurrentPlayer;
		}		
	}

	private void playAnimation(string targetChar) {
		string[] chars = targetChar.Split(',');
		if (chars.Length < 2)
			return;
		GameObject character = GameObject.Find (chars [0]);
		string anim = chars [1];
		if (character != null && character.GetComponent<AnimatorSprite>()) {
			Debug.Log ("Playing anim: " + anim);
			bool res = character.GetComponent<AnimatorSprite> ().Play (anim);
			Debug.Log ("res: " + res);
		}
	}
}
