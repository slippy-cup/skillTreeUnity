using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class skillAbstract : MonoBehaviour
{
    [SerializeField] string s_Name;
    [SerializeField] string s_Descirption;
    [SerializeField] GameObject[] s_Child;

    [SerializeField] TextMeshProUGUI c_Name;
    [SerializeField] TextMeshProUGUI c_Descirption;

    bool isUnlocked = false;
    [SerializeField] bool canPurchased;

    [SerializeField] int s_Cost;

    public GameObject parent;

    Button s_Button;
    LineRenderer c_LineRenderer;
    moneyMan playerMoney;

    private string saveFilePath;

    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public bool isUnlocked;
        public bool canPurchased;
        public List<SkillData> childSkills;
    }

    void Start()
    {
        s_Button = GetComponent<Button>();
        s_Button.onClick.AddListener(onClick);

        c_Name.text = s_Name;
        c_Descirption.text = s_Descirption;

        c_LineRenderer = GetComponent<LineRenderer>();
        c_LineRenderer.sortingOrder = -10;
        ButtonColor();

        playerMoney = FindObjectOfType<moneyMan>();

        saveFilePath = Path.Combine(Application.persistentDataPath, "SkillTreeProgress.json");
        Debug.Log("Save file path: " + saveFilePath);
        LoadSkillTreeProgress();
    }

    void Update()
    {
        drawLine();
        ButtonColor();
    }

    //Chnages the color of the button based on whethere they are unlocked or not
    void ButtonColor()
    {
        ColorBlock buttonColors = s_Button.colors;

        if (!isUnlocked && !canPurchased)
        {
            buttonColors.normalColor = Color.magenta;
            buttonColors.highlightedColor = Color.magenta;
            buttonColors.pressedColor = Color.magenta;
            buttonColors.disabledColor = Color.magenta;

            s_Button.interactable = false;
        }
        else
        {
            buttonColors.normalColor = Color.white;
            buttonColors.highlightedColor = Color.white;
            buttonColors.pressedColor = Color.white;
            buttonColors.disabledColor = Color.white;

            s_Button.interactable = true;
        }

        s_Button.colors = buttonColors;
    }

    //Uses line renderer to save lines between the skill trees 
    void drawLine()
    {
        if (s_Child.Length > 0)
        {
            foreach (GameObject child in s_Child)
            {
                child.GetComponent<skillAbstract>().parent = this.gameObject;
            }
        }

        if (parent != null)
        {
            Vector3 offset = new Vector3(0.0f, 0.0f, 90.0f);
            c_LineRenderer.positionCount = 2;
            c_LineRenderer.SetPosition(0, this.transform.position - offset);
            c_LineRenderer.SetPosition(1, parent.transform.position - offset);
        }
    }

    //Purchases  the skills on click checking if its purchaseable 
    void onClick()
    {
        if (canPurchased && playerMoney.playerCurrency >= s_Cost)
        {
            isUnlocked = true;
            canPurchased = false;
            playerMoney.playerCurrency = playerMoney.playerCurrency - s_Cost;
            playerMoney.UpdateText();
            nextLayer();
            skillEffect();
            Debug.Log("Skill unlocked: " + s_Name);

            SaveSkillTreeProgress();
        }
        else
        {
            Debug.Log("Not not unlocked");
        }
    }

    void nextLayer()
    {
        foreach (GameObject child in s_Child)
        {
            if (child == this.s_Button) continue;
            if (child.TryGetComponent<skillAbstract>(out skillAbstract childSkill))
            {
                childSkill.canPurchased = true;
            }
        }
    }

    void skillEffect()
    {
        Debug.Log("Skill is in Effect: " + s_Name);
    }

    public void SaveSkillTreeProgress()
    {
        Debug.Log("Saving skill tree progress...");

        List<SkillData> allSkillData = new List<SkillData>();
        SaveSkillState(allSkillData, this);  // Start saving from this skill

        string json = JsonUtility.ToJson(new SkillTreeWrapper { skills = allSkillData }, true);

        // Debugging the structure before saving
        Debug.Log("Saving JSON: " + json);

        // Write JSON to file
        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Skill tree progress saved.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving skill tree progress: " + ex.Message);
        }
    }

    void SaveSkillState(List<SkillData> allSkillData, skillAbstract skill)
    {
        if (skill == null) return;

        Debug.Log($"Saving skill: {skill.s_Name}");

        SkillData skillData = new SkillData
        {
            skillName = skill.s_Name,
            isUnlocked = skill.isUnlocked,
            canPurchased = skill.canPurchased,
            childSkills = new List<SkillData>()
        };

        // Recursively save child skills
        foreach (GameObject child in skill.s_Child)
        {
            if (child.TryGetComponent<skillAbstract>(out skillAbstract childSkill))
            {
                SaveSkillState(skillData.childSkills, childSkill);  // Add child recursively
            }
        }

        allSkillData.Add(skillData);
    }

    void LoadSkillTreeProgress()
    {
        if (File.Exists(saveFilePath))
        {
            Debug.Log("Loading skill tree progress...");
            try
            {
                string json = File.ReadAllText(saveFilePath);
                SkillTreeWrapper skillTreeWrapper = JsonUtility.FromJson<SkillTreeWrapper>(json);

                // Debugging loaded data
                Debug.Log("Loaded JSON: " + json);

                // Loop through all loaded skills
                foreach (SkillData skillData in skillTreeWrapper.skills)
                {
                    UpdateSkillState(skillData.skillName, skillData.isUnlocked, skillData.canPurchased);
                }

                Debug.Log("Skill tree progress loaded.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error loading skill tree progress: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("No save file found to load.");
        }
    }

    void UpdateSkillState(string skillName, bool isUnlocked, bool canPurchased)
    {
        if (this.s_Name == skillName)
        {
            this.isUnlocked = isUnlocked;
            this.canPurchased = canPurchased;
            if (isUnlocked)
            {
                nextLayer();
            }
            Debug.Log($"Updated skill: {skillName}, Unlocked: {isUnlocked}, CanPurchased: {canPurchased}");
            return;
        }

        foreach (GameObject child in s_Child)
        {
            if (child.TryGetComponent<skillAbstract>(out skillAbstract childSkill))
            {
                childSkill.UpdateSkillState(skillName, isUnlocked, canPurchased);
            }
        }
    }

    [System.Serializable]
    public class SkillTreeWrapper
    {
        public List<SkillData> skills;
    }

    public void ResetSkill()
    {
        isUnlocked = false;
        canPurchased = (parent == null);
    }

    public List<skillAbstract> GetChildSkills()
    {
        List<skillAbstract> childSkills = new List<skillAbstract>();
        foreach (GameObject child in s_Child)
        {
            if (child.TryGetComponent<skillAbstract>(out skillAbstract childSkill))
            {
                childSkills.Add(childSkill);
            }
        }
        return childSkills;
    }
}




