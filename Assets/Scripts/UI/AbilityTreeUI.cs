using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTreeUI : MonoBehaviour {

    public static GameObject Canvas;
    public static AbilityTree tree;
    public GameObject NodePrefab;

    public GameObject CurrentSelection;
    public bool created = false;

	// Use this for initialization
	void Start () {
        if (Canvas == null)
            Canvas = GameObject.Find("Canvas");
        if (tree == null)
            return;
        CreateTreeNodes();
	}
	
	// Update is called once per frame
	void Update () {
        if (tree == null) return;
        if (!created)
            CreateTreeNodes();

		//Controls within tree/UI

            //Update currentSelection

	}

    void CreateTreeNodes()
    {
        GameObject g;
        AbilityTreeNode node = tree.GetRoot();
        Queue<AbilityTreeNode> queue = new Queue<AbilityTreeNode>();
        queue.Enqueue(node);
        node = queue.Dequeue();

        //Foreach node, create a button
        while (node != null)
        {
            //Create UI Things
            g = Instantiate(NodePrefab);
            g.transform.SetParent(Canvas.transform);
            g.GetComponent<NodeUI>().treeNode = node;
            g.GetComponent<RectTransform>().anchoredPosition = RuntimeTransforms.GetVector(node.TreeDepth, RuntimeTransforms.GetBranchDirection(node.ability.AbilityClassification));
            //Attach new UI Thing to this script

            foreach (AbilityTreeNode n in node.tree.GetChildren())
                queue.Enqueue(n);
            if (queue.Count > 0)
                node = queue.Dequeue();
            else
                node = null;
        }

        //Attach visually to the parent

        created = true;

    }

    void CalculateTransformPosition(GameObject g)
    {
        NodeUI nodeUI = g.GetComponent<NodeUI>();
        AbilityTreeNode abilityTreeNode = nodeUI.treeNode;
    }
    
}
