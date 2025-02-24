using System.IO;
using UnityEngine;

public class ResetSkillTree : MonoBehaviour
{
    // Reference to the root skill in the skill tree
    [SerializeField] private skillAbstract rootSkill;

    // File path for saving skill tree progress
    private string saveFilePath;

    private void Start()
    {
        // Set the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, "SkillTreeProgress.json");
    }

    // Public method to reset the skill tree
    public void ResetSkillTreeProgress()
    {


        // Delete the save file if it exists
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }

        // Reset all skills to their initial state
        if (rootSkill != null)
        {
            ResetSkillState(rootSkill);

            // Save the reset state of the skill tree
            rootSkill.SaveSkillTreeProgress();
        }
        else
        {
            Debug.LogError("Root skill is not assigned!");
        }
    }

    // Recursively reset the state of each skill in the tree
    private void ResetSkillState(skillAbstract skill)
    {
        if (skill == null) return;

        // Reset the skill's state
        skill.ResetSkill();

        // Recursively reset child skills
        foreach (var child in skill.GetChildSkills())
        {
            ResetSkillState(child);
        }
    }
}
