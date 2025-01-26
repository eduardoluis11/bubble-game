using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to load scenes

/* Door, which will allow the player go to other levels, the victory screen, or any other scene.
 * 
 * You're welcome! To make the door transition to the "VictoryScreen" scene when the player presses the **up arrow key** while colliding with it, you'll need a script to handle this interaction. The **Collider2D** on the door can be either a trigger or a regular collider—I'll provide options for both approaches.

Here's the script:

---

### Steps to Set Up the Door:

1. **Add a Trigger Collider to the Door:**
   - Add a `BoxCollider2D` to the door GameObject.
   - Set the `Is Trigger` checkbox to **true** so it behaves as a trigger.

2. **Tag the Player:**
   - Make sure your player GameObject has the tag `Player`.

3. **Attach the Script:**
   - Attach the `Door` script to your door GameObject (the rectangle you created).

4. **Assign the Target Scene:**
   - In the Unity Editor, set the `Target Scene` field in the `Door` script to the name of the scene you want to load (e.g., `"VictoryScreen"`).

---

### How the Script Works:

- **Trigger-Based Detection:**
  - When the player enters the door's trigger collider, the `playerIsColliding` flag is set to `true`.
  - When the player exits the trigger, the flag is set to `false`.

- **Scene Transition:**
  - The `Update` method checks if the player is within the trigger (`playerIsColliding == true`) and presses the **up arrow key**. If both conditions are met, it loads the scene specified in the `targetScene` field.

---

### Adding the Scene to Build Settings:

To ensure the scene transition works, do the following:
1. Go to `File > Build Settings`.
2. Add your current scene and the `"VictoryScreen"` scene to the **Scenes in Build** list.
   - Drag and drop the scenes from your `Assets` folder into the list.
   - Make sure the `VictoryScreen` scene name matches what you entered in the `targetScene` field of the script.
 * 
 * 
 * 
 */

public class Door : MonoBehaviour
{
    
    // Door's settings
    [Header("Settings")]
    public string targetScene = "VictoryScreen"; // The name of the scene to load
    private bool playerIsColliding = false; // Tracks if the player is colliding with the door

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        // Check if the player is colliding and presses the Up Arrow key
        if (playerIsColliding && Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Load the target scene
            SceneManager.LoadScene(targetScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player entered the door's trigger
        if (collision.CompareTag("Player"))
        {
            playerIsColliding = true; // Player is in range
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player exited the door's trigger
        if (collision.CompareTag("Player"))
        {
            playerIsColliding = false; // Player left the range
        }
    }

}
