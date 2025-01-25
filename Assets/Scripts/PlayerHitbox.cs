using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* To render a hitbox in front of the player while holding down the "E" key, and make its width and height adjustable in the 
 * Unity editor, you can create a simple script for this (source: Unity Helper's plugin from ChatGPT.)

1. **Hitbox Visibility and Size:**
   - The hitbox appears when the player holds down the "E" key, and it's hidden when the key is released.
   - The size of the hitbox can be adjusted in the Unity editor through the `hitboxSize` field.
   - You can also adjust its position relative to the player with `hitboxOffset`.

2. **Components:**
   - A `BoxCollider2D` is used to represent the hitbox for game logic (e.g., detecting collisions with enemies).
   - A `SpriteRenderer` provides a visible square (yellow) to represent the hitbox during development.

3. **Editor Adjustments:**
   - `hitboxSize` and `hitboxOffset` are public and editable in the Unity editor.
   - `hitboxColor` allows you to change the visualization color of the hitbox.

4. **Optimization:**
   - The hitbox object is only activated when the "E" key is pressed, saving resources when it's not in use.


### Usage:

1. Attach the `PlayerHitbox` script to your player GameObject.
2. Adjust `Hitbox Size`, `Hitbox Offset`, and `Hitbox Color` in the Unity editor to suit your needs.
3. Play the game, and hold down "E" to see the hitbox appear in front of the player.
 * 
 * 
 * To ensure the hitbox is rendered correctly on the screen, you need to make sure the SpriteRenderer component is 
 * properly configured. Specifically, you need to set the sprite property of the SpriteRenderer to a valid sprite 
 * and ensure the size property is correctly set.
 * 
 * Changes made:
1.	Added renderer.drawMode = SpriteDrawMode.Sliced; to ensure the sprite can be resized.
2.	Set renderer.size = hitboxSize; in the Start method to initialize the size of the sprite.
3.	Updated the UpdateHitbox method to set the renderer.size to hitboxSize.
This should ensure that the hitbox is rendered correctly on the screen when the "E" key is pressed.
You are using the active document because you have the checkmark checked. You can include additional context using # references. Typing # opens a completion list of available context.
 */

public class PlayerHitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    public Vector2 hitboxSize = new Vector2(1f, 1f); // Adjust width and height in the editor
    public Vector2 hitboxOffset = new Vector2(1f, 0f); // Adjust offset in the editor

    [Header("Hitbox Visualization")]
    public Color hitboxColor = new Color(1f, 1f, 0f, 0.5f); // Semi-transparent yellow for visualization

    private GameObject hitbox;
    private new SpriteRenderer renderer; // Use 'new' keyword to hide inherited member

    // Start is called before the first frame update
    void Start()
    {
        // Create the hitbox object
        hitbox = new GameObject("Hitbox");
        hitbox.transform.SetParent(transform); // Attach it to the player
        hitbox.SetActive(false);

        // Add a BoxCollider2D for the hitbox functionality
        BoxCollider2D boxCollider = hitbox.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        // Add a SpriteRenderer for hitbox visualization (optional)
        renderer = hitbox.AddComponent<SpriteRenderer>();
        renderer.color = hitboxColor;
        renderer.sprite = CreateHitboxSprite(); // Creates a simple square sprite
        renderer.drawMode = SpriteDrawMode.Sliced;
        renderer.size = hitboxSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Enable the hitbox when "E" is held down
        if (Input.GetKey(KeyCode.E))
        {
            hitbox.SetActive(true);
            UpdateHitbox();

            // DEBUG: Log a message when the hitbox is enabled in the console
            Debug.Log("Hitbox enabled. You're shooting bubbles.");
        }
        else
        {
            hitbox.SetActive(false);
        }
    }

    private void UpdateHitbox()
    {
        hitbox.transform.localPosition = (Vector3)hitboxOffset;
        BoxCollider2D boxCollider = hitbox.GetComponent<BoxCollider2D>();
        boxCollider.size = hitboxSize;

        renderer.size = hitboxSize;
    }

    private Sprite CreateHitboxSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        // Create the sprite with Full Rect mesh type
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1, 0, SpriteMeshType.FullRect);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = hitboxColor;
        Gizmos.DrawWireCube(transform.position + (Vector3)hitboxOffset, hitboxSize);
    }
}
