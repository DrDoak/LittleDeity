using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIHandler : MonoBehaviour {

	public static GUIHandler Instance = null;

	public GameObject MenuProperty;
	public GameObject SelectionProperty;
	public TransferMenu MTransferMenu;

	public Slider P1HealthBar;
	public GameObject CurrentTarget;
	public TextMeshProUGUI ExpText;

	private List<GameObject> PropertyLists;

	public GameObject IconPropertyPrefab;
	public Sprite UnknownPropertyIcon;

    private Ability abilityToUpdate;

	Dictionary<string,GameObject> m_iconList;

	void Awake () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this) {
			Destroy (gameObject);
		}
		PropertyLists = new List<GameObject> ();
		m_iconList = new Dictionary<string,GameObject>();
	}
	void Update() {
		if (CurrentTarget != null) {
			var P1Controller = CurrentTarget.GetComponent<Attackable> ();

			P1HealthBar.value = P1Controller.Health;
			P1HealthBar.maxValue = P1Controller.MaxHealth;
			Vector3 oS = P1HealthBar.GetComponent<RectTransform> ().localScale;
			P1HealthBar.GetComponent<RectTransform> ().localScale =new Vector3((P1Controller.MaxHealth / 100f),oS.y,oS.z);
			if (CurrentTarget.GetComponent<ExperienceHolder> () != null) {
				var exp = CurrentTarget.GetComponent<ExperienceHolder> ();
				ExpText.text = "Data: " + exp.VisualExperience + "\nNext: " + Leveller.Instance.NextLevel;
			}
		}
	}
	public void SetHUD(bool active) {
		if (!active) {
			P1HealthBar.gameObject.SetActive( false);
			ExpText.gameObject.SetActive( false );
			transform.GetChild (0).Find ("PropList").gameObject.SetActive (false);
		} else {
			P1HealthBar.gameObject.SetActive( true);
			ExpText.gameObject.SetActive( true );
			transform.GetChild (0).Find ("PropList").gameObject.SetActive (true);
		}
	}
	public static void CreateTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		Instance.InternalTransferMenu (ph1 , ph2);
	}

	void InternalTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		MTransferMenu.gameObject.SetActive (true);
		MTransferMenu.Clear ();

		MTransferMenu.AddPropertyHolder (ph1,0);
		MTransferMenu.AddPropertyHolder (ph2,1);
		MTransferMenu.init (ph1.NumTransfers);
	}

	public static void ClosePropertyLists() {
		foreach (GameObject go in Instance.PropertyLists) {
			Destroy (go);
		}
		Instance.PropertyLists.Clear ();
	}

    public static void SetAbility(Ability a)
    {
        Instance.abilityToUpdate = a;
    }

    public static void UpdateAbility(List<Property> p1, List<Ability> a1, List<Property> p2)
    {
		if (Instance.abilityToUpdate != null) {
			Instance.abilityToUpdate.SetTransferLists(p1,a1,p2);
		}
    }
    

	public void AddPropIcon(Property p) { 
		if (!m_iconList.ContainsKey(p.GetType().ToString()) ){
			System.Type sysType = p.GetType ();
			Property mp = (Property)GetComponentInChildren (sysType);
			GameObject go = Instantiate (IconPropertyPrefab);
			go.transform.SetParent(transform.GetChild(0).Find ("PropList"),false);
			if (mp != null) {
				go.GetComponent<Image> ().sprite = mp.icon;
			} else {
				go.GetComponent<Image> ().sprite = mp.icon;
			}
			m_iconList [p.GetType().ToString()] = go;
		}
	}

	public void ClearPropIcons() {
		foreach ( GameObject g in m_iconList.Values) {
			Destroy (g);
		}
		m_iconList.Clear ();
	}
	public void RemovePropIcon(Property p) {
		if (m_iconList.ContainsKey (p.GetType().ToString())) {
			Destroy (m_iconList [p.GetType().ToString()]);
			m_iconList.Remove (p.GetType().ToString());
		}
	}

	public void OnSetPlayer(BasicMovement bm) {
		ClearPropIcons ();
		CurrentTarget = bm.gameObject;
		foreach (Property p in bm.GetComponents<Property>()) {
			AddPropIcon (p);
		}
	}
}