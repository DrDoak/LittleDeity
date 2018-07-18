using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public Image image;
    public bool greyed;
    public AbilityTreeNode treeNode;

    public Button button;

    void Start()
    {
        if (treeNode == null) return;
        if(!button) button = gameObject.GetComponentInChildren<Button>();
        FillData();

        Debug.Log(treeNode.unlocked);
    }

    void Update()
    {
        button.gameObject.GetComponentInChildren<Text>().text = treeNode.ability.AbilityName;
        button.interactable = treeNode.unlocked || (treeNode.CheckRequisites() && AbilityTree.PointsToSpend > 0);
    }

    void FillData()
    {
        //button.image = image;
        button.gameObject.GetComponentInChildren<Text>().text = treeNode.ability.AbilityName;
        //Fill the description of the ability
        //Add icons if applicable
    }

    public void Hover()
    {
        //Create Halo effect to illustrate selection
    }
    
    /// <summary>
    /// Use for OnClick method of button, selects the ability within the tree
    /// </summary>
    public void Click()
    {
        treeNode.tree.ChooseAbility();
    }
}
