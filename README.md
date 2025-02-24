# Skill Tree Documentation

## Overview
The **Skill Tree Simple** package is a customizable skill tree system for Unity projects. It allows developers to create and manage skill trees with ease, offering both design and development workflows. Key features include:
- Customizable nodes with inspector-driven configuration.
- Save and load functionality for skill tree progress.
- Reset functionality to restore the skill tree to its initial state.

---

## Design Workflow
The design workflow focuses on creating and configuring skill tree skills without requiring extensive coding. This section covers how to instantiate nodes, modify their values, and customize their presentation.

### Instantiating New Skills
To add a new skill to the skill tree:
1. Drag the skill prefab from the **prefab** folder and place within thhe skill tree. 
2. Double click the skill to open it's **inspector**, then assign it's parent and children by dragging other skills into their respective areas.
3. Then they are able to alter other properties of each individual skills.

This process ensures that the new skill is properly integrated into the skill tree structure.

---

### Changing Skill Values
Each node's functionality can be customized using the **Inspector**. The following serialized fields are available for configuration:
- **s_Name**: The name of the skill.
- **s_Description**: The description of the skill.
- **s_Child**: An array of child nodes.
- **s_Cost**: The cost to unlock the node.
- **canPurchased**: Whether the node can be purchased initially.

These values can be modified in the **Inspector** under the skill's script component.

---

### Skill Presentation
The visual presentation of nodes can be customized by modifying the `Button` and `TextMeshProUGUI` components attached to each node. Specifically for the skills developers can alter the appearance by accessing the **skill prefab's inspector**. Since each node inherits from Unity's `Button` class, all customizable properties are available in the [Unity Button Documentation](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-Button.html).

---

## Development Workflow
The development workflow focuses on implementing functionality for each skill. This section covers how to add custom functionality and match skills to nodes.

### Node Functionality
The `skillAbstract` script handles the bulk of the skill tree's functionality. Key methods include:
- **onClick**: Handles the purchase of a skill if the player has enough currency.
- **nextLayer**: Unlocks child nodes when a skill is purchased.
- **skillEffect**: Placeholder for custom skill effects.

---

### Saving and Loading
The skill tree system uses Unity's file input-output (I/O) system to save and load progress. Key points to note:
- **Save File Format**: Skills are saved in JSON format, with each skill's name, unlocked state, and purchase state stored.
- **File Path**: The default save file path is `Application.persistentDataPath/SkillTreeProgress.json`.

#### Save Function
The `SaveSkillTreeProgress` method serializes the skill tree data into a JSON file. It recursively saves each skill's state and its child skills.

#### Load Function
The `LoadSkillTreeProgress` method deserializes the JSON file and updates the skill tree based on the saved data.

---

### Reset Functionality
The `ResetSkillTree` script provides a method to reset the skill tree to its initial state. This includes:
- Deleting the save file.
- Resetting all skills to their initial state (unlocked and purchaseable states).

---

## Known Issues
### Save Functionality Not Working
Currently, the save functionality is not properly saving the state of skills, regardless of whether they were purchased or unlocked. The issue may be due to:
1. **Incorrect JSON Serialization**: The `SkillData` class may not be properly serialized or deserialized.
2. **Recursive Save/Load Issues**: The recursive methods for saving and loading may not be correctly traversing the skill tree.
3. **File Path Issues**: The save file may not be written to or read from the correct location.

#### Debugging Steps
1. **Check JSON Output**: Add debug logs to the `SaveSkillTreeProgress` method to verify the JSON output.
   ```csharp
   Debug.Log("Saving JSON: " + json);
   ```
2. **Verify File Path**: Ensure the save file path is correct and accessible.
   ```csharp
   Debug.Log("Save file path: " + saveFilePath);
   ```
3. **Test Recursive Methods**: Add debug logs to the `SaveSkillState` and `UpdateSkillState` methods to verify that all skills are being processed.

---

## Example Workflow
1. **Design**:
   - Instantiate new nodes and configure their values in the Inspector.
   - Customize the presentation of nodes using Unity's UI tools.

2. **Development**:
   - Add custom functionality to the `skillAbstract` script.
   - Use the `skillEffect` method to define skill behavior.

3. **Saving and Loading**:
   - Save skill tree progress using the `SaveSkillTreeProgress` method.
   - Load progress on game start using the `LoadSkillTreeProgress` method.

4. **Reset**:
   - Use the `ResetSkillTreeProgress` method to reset the skill tree to its initial state.

5. **Toggle**:
   - Press the escape key to toggle the skill tree. 

---

## Conclusion
The **Skill Tree Simple** package provides a flexible and easy-to-use system for creating and managing skill trees in Unity. By separating design and development workflows, it allows developers to focus on both the visual and functional aspects of their skill trees with minimal effort. For further customization, refer to the provided scripts and documentation.

---

