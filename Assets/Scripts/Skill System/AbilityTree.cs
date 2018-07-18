using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTree
{
    public static GameObject Player;
    public static int PointsToSpend = 0;
    public AbilityTreeNode root;
    public AbilityTree LeftBranch;
    public AbilityTree MiddleBranch;
    public AbilityTree RightBranch;
    public bool UltimateChosen = false;

    public AbilityTree()
    {
        root = null;
    }

    public AbilityTree(Ability a, int description = -1)
    {
        root = new AbilityTreeNode(a);
        root.tree = this;
        if (description != -1) root.SetDescription(description);
    }

    public AbilityTree(Ability a, AbilityTreeNode treeNode, int description = -1)
    {
        root = new AbilityTreeNode(a, treeNode);
        root.tree = this;
        if (description != -1) root.SetDescription(description);
    }

    public AbilityTreeNode UnlockAbilitiesAutomatic()
    {
        if (PointsToSpend > 0)
        {
            AbilityTreeNode node;
            Queue<AbilityTreeNode> q = new Queue<AbilityTreeNode>();
            q.Enqueue(root);
            while (q.Count > 0 && PointsToSpend > 0)
            {
                node = q.Dequeue();
                if (!node.unlocked)
                {
                    node.tree.ChooseAbility();
                    return node;
                }
                else
                    foreach (AbilityTreeNode n in node.tree.GetChildren())
                        q.Enqueue(n);

            }
        }
        return null;
    }

    public string DisplayAbility()
    {
        return root.ability.AbilityDescription;
    }
       

    public void ChooseAbility()
    {
        if (!root.CheckRequisites())
            return;

        if (root.Ultimate)
            UltimateChosen = true;
        if (PointsToSpend > 0 && (!root.unlocked || (root.Tiered && root.unlocked && !root.Maxed)))
        {
            root.Unlock();
            PointsToSpend--;
        }
        else
            root.Select();
    }

    public void AddRoot(Ability a, int description = -1)
    {
        root = new AbilityTreeNode(a);
        root.tree = this;
        if (description != -1) root.SetDescription(description);
    }

    private void AddLeft(Ability a, int description = -1)
    {
        if (LeftBranch == null)
            LeftBranch = new AbilityTree(a, root, description);
        else
            LeftBranch.Add(a, Branch.LEFT, description);
    }

    private void AddRight(Ability a, int description = -1)
    {
        if (RightBranch == null)
            RightBranch = new AbilityTree(a, root, description);
        else
            RightBranch.Add(a, Branch.RIGHT, description);
    }

    private void AddMiddle(Ability a, int description = -1)
    {
        if (MiddleBranch == null)
            MiddleBranch = new AbilityTree(a, root, description);
        else
            MiddleBranch.Add(a, Branch.MIDDLE, description);
    }

    public void Add(Ability a, Branch b, int description = -1)
    {
        switch (b)
        {
            case Branch.LEFT:
                AddLeft(a, description);
                break;
            case Branch.MIDDLE:
                AddMiddle(a, description);
                break;
            case Branch.RIGHT:
                AddRight(a, description);
                break;
        }
    }

    public void Add(Ability a, Branch b, AbilityType subtree, int description = -1)
    {
        AbilityTreeNode node = root;

        switch (subtree)
        {
            case AbilityType.COMBAT:
                node = LeftBranch.root;
                break;
            case AbilityType.SPECIAL:
                node = MiddleBranch.root;
                break;
            case AbilityType.ENVRIONMENTAL:
                node = RightBranch.root;
                break;
        }

        if (node != null)
            node.tree.Add(a, b, description);
    }

    public Ability GetAbility(Branch b, int depth, AbilityType subtree)
    {
        int d = root.TreeDepth;
        AbilityTreeNode node = root;

        switch (subtree)
        {
            case AbilityType.COMBAT:
                node = LeftBranch.root;
                break;
            case AbilityType.SPECIAL:
                node = MiddleBranch.root;
                break;
            case AbilityType.ENVRIONMENTAL:
                node = RightBranch.root;
                break;
        }
        if (node != null)
            d = node.TreeDepth;

        while (d != depth && node != null)
        {
            switch (b)
            {
                case Branch.LEFT:
                    node = LeftBranch.root;
                    break;
                case Branch.MIDDLE:
                    node = MiddleBranch.root;
                    break;
                case Branch.RIGHT:
                    node = RightBranch.root;
                    break;
            }

            d = node.TreeDepth;
        }
        return node.ability;
    }

    public void PrintTree()
    {
        AbilityTreeNode node = root;

        if (node != null)
        {
            //Debug.Log(node.ability);
            //Debug.Log(node.TreeDepth);
            if (LeftBranch != null) LeftBranch.PrintTree();
            if (MiddleBranch != null) MiddleBranch.PrintTree();
            if (RightBranch != null) RightBranch.PrintTree();
        }
    }

    public void PassUltimate()
    {
        AbilityTreeNode node = root;
        while (node != null && node.TreeDepth != 1)
        {
            node.tree.UltimateChosen = true;
            node = node.parent;
        }

        if (node != null)
            node.tree.UltimateChosen = true;
    }

    public AbilityTreeNode GetRoot()
    {
        return root;
    }

    public List<AbilityTreeNode> GetChildren()
    {
        List<AbilityTreeNode> children = new List<AbilityTreeNode>();
        if (LeftBranch != null) children.Add(LeftBranch.GetRoot());
        if (MiddleBranch != null) children.Add(MiddleBranch.GetRoot());
        if (RightBranch != null) children.Add(RightBranch.GetRoot());

        return children;
    }

}

public class AbilityTreeNode
{
    public Ability ability;
    public AbilityTree tree;
    public AbilityTreeNode parent;
    public bool unlocked = false;
    public bool Ultimate = false;
    public bool Tiered = false;
    public bool Maxed = true;
    public int TreeDepth;

    void Awake()
    {
        if (parent != null)
        {
            TreeDepth = parent.TreeDepth + 1;
        }
        else
        {
            TreeDepth = 0;
            Unlock();
        }

        if (TreeDepth == 1) Unlock();

        if (ability)
        {
            Ultimate = ability.Ultimate;
            Tiered = ability.Tiered;
            Maxed = ability.Maxed;
        }
        //Select();
    }

    public AbilityTreeNode(Ability a)
    {
        ability = a;
        parent = null;
        Awake();
    }

    public AbilityTreeNode(Ability a, AbilityTreeNode treeNode)
    {
        ability = a;
        parent = treeNode;
        Awake();
    }

    public void Unlock()
    {
        unlocked = true;
        //Apply ability upgrades
        if (ability.Passive)
        {
            ability.UseAbility();
        }
        if (TreeDepth == 0)
            ability.Upgrade();

        Maxed = ability.Maxed;
        //Debug.Log("You unlocked: " + ability.AbilityName);
        Select();
    }

    public void Select()
    {
        //Modify combat control of player by replacing designated ability
        if (ability.RequiresReplacement)
        {
            CombatControl cc = Ability.Player.GetComponent<CombatControl>();

            switch (ability.AbilityClassification)
            {
                case AbilityType.COMBAT:
                    break;
                case AbilityType.SPECIAL:
                    break;
                case AbilityType.ENVRIONMENTAL:
                    break;
            }
        }

        ability.Select();
        //Modify ultimate selection if applicable
        if (Ultimate && tree != null)
            tree.PassUltimate();
    }
    public bool CheckRequisites()
    {
        if (parent == null) return true;
        return parent.unlocked;
    }

    public void SetDescription(int index)
    {
        ability.SetDescriptionInfo(index);
    }
}

public enum Branch
{
    LEFT, MIDDLE, RIGHT
}
