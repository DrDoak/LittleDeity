using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProperty : MonoBehaviour {

	public Property SelectedProperty;

	public void OnSelection() {
		Debug.Log ("Button Selected!:" + SelectedProperty);
        //GUIHandler.UpdateAbility(SelectedProperty);
	}
}
