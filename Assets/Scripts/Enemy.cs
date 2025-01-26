using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* To achieve the behavior where the player is pushed back when colliding with the enemy (regardless of the 
 * collision point, such as on top), we can create a script for the enemy. The script will detect collisions 
 * with the player and apply a force to push the player back. Additionally, we'll ensure that the player 
 * cannot use the enemy as a platform by preventing upward movement when colliding.

Here's the script for the enemy:

---

---

### How It Works:

1. **Collision Detection:**
   - The `OnCollisionEnter2D` method is triggered when the player first collides with the 
enemy. It pushes the player back with an impulse force.
   - The `OnCollisionStay2D` method ensures the player continues to be pushed away if they 
remain in contact with the enemy, preventing them from standing on it.

2. **Knockback Direction:**
   - The direction of the push-back is calculated as the normalized vector from the enemy's 
position to the player's position. This ensures the push is always "away" from the enemy, 
regardless of the collision point.

3. **Force Application:**
   - The `AddForce` method applies the knockback force to the player's `Rigidbody2D` using 
the `ForceMode2D.Impulse` mode for an instantaneous push.

4. **Tag Check:**
   - The script ensures it only interacts with GameObjects tagged as "Player." Make sure 
your player GameObject has the `Player` tag assigned.

5. **Preventing Platforming:**
   - By continuously applying force in `OnCollisionStay2D`, the player cannot stand on the 
enemy as a platform. They will always be pushed away.

---

### Usage Instructions:

1. **Assign the Script:**
   - Attach the `Enemy` script to the enemy GameObject (red square).

2. **Setup the Player:**
   - Ensure your player GameObject has:
     - A `Rigidbody2D` component.
     - A `Collider2D` component.
     - The tag "Player."

3. **Adjust Knockback Settings:**
   - You can tweak the `knockbackForce` in the Unity editor to control the strength of 
the push-back effect.

---

### Notes:
- This solution assumes the player uses a `Rigidbody2D` for movement.
- If you use a custom movement script for the player that overrides physics, you might need 
to add logic to momentarily disable player controls during knockback.
 * 
 * To modify the code so that the player moves some units to the left whenever they collide with the enemy, you can 
 * directly set the player's position in the OnCollisionEnter2D and OnCollisionStay2D methods.
 * 
 * Explanation:
•	Added a new method MovePlayerLeft that moves the player 20 units to the left by directly setting the player's position.
•	Updated the OnCollisionEnter2D and OnCollisionStay2D methods to call MovePlayerLeft instead of ApplyKnockback.
This will ensure that the player is moved 20 units to the left whenever they collide with the enemy.
 *
 * 
 *
 *
 * Absolutely! To implement the behavior where the enemy turns into a circle when it is inside the player's hitbox during an attack, we can use a 
 * boolean to keep track of whether the enemy is already a circle. If the player presses the "e" key and the enemy is within the hitbox, the enemy 
 * will transform into a circle GameObject.

Here’s the **snippet** for this logic:

---

### Key Points:

1. **Boolean `isCircle`:**
   - This tracks whether the enemy has already been transformed into a circle. It prevents repeated transformations if the hitbox repeatedly overlaps the enemy.

2. **Hitbox Detection:**
   - The script listens for collisions via the `OnTriggerEnter2D` method. This assumes the player's hitbox is a trigger Collider2D tagged as `PlayerHitbox`.

3. **Transforming into a Circle:**
   - The enemy spawns a new GameObject (a circle) at its current position using `Instantiate()`.
   - After spawning the circle, the original enemy is destroyed using `Destroy(gameObject)` or deactivated with `gameObject.SetActive(false)` if you want to keep the original object around for debugging or other logic.

4. **Circle Prefab:**
   - Create a **circle prefab** in Unity (e.g., a simple circular sprite with a `SpriteRenderer`) and assign it to the `circlePrefab` field in the Unity Editor.

---

### Setup in Unity:

1. **Player Hitbox:**
   - The player's hitbox (yellow square in your game) must have:
     - A `Collider2D` component set to `Is Trigger = true`.
     - The tag `PlayerHitbox`.

2. **Enemy Object:**
   - Attach the `Enemy` script to your enemy (red square).
   - Assign the `circlePrefab` in the Unity Editor to a circular GameObject prefab.

3. **Circle Prefab:**
   - Create a new GameObject in Unity with a circular sprite (or any shape you like).
   - Turn it into a prefab by dragging it into the `Assets` folder.

---

### Optional: Player Script Modification

You’ll also need to ensure the player's hitbox is active when the player presses the "e" key. From the script you provided earlier, this is already handled by enabling and disabling the hitbox.

Make sure the hitbox has the correct tag (`PlayerHitbox`), and everything should work smoothly.

* 
*
*
*
 */

public class Enemy : MonoBehaviour
{

    // Knockback Distance
    [Header("Knockback Settings")]

    // Distance to move the player to the left. I made it public so that I can adjust it in the Unity Editor.
    public float knockbackDistance = 20f;

    //public float knockbackForce = 5f; // How strong the push-back effect is
    //public float verticalKnockbackMultiplier = 1.5f; // Optional: Amplify vertical knockback if needed
    //public float horizontalKnockbackMultiplier = 1f; // Multiplier for horizontal knockback

    // This handles the variables in the unity editor that will be used to transform the enemy into a bubble
    [Header("Enemy Transformation Into Bubble")]
    public bool isCircle = false; // Tracks whether the enemy is currently a circle
    public GameObject bubbleContainerPrefab; // Reference to the circle prefab to spawn

    public GameObject enemysOriginalFormPrefab; // Reference to the enemy's original sprite prefab to spawn

    private GameObject currentBubble; // Reference to the current bubble instance as a "global" variable

    private GameObject currentEnemy; // Reference to the current enemy's original form instance as a "global" variable


    /* This will make it so that, if the player collides with the enemy, the player will be pushed back to the left.
     * 
     * That is, if the player touches the enemy at any angle, be it by touching him from any side, or by jumping on top 
     * of him, the player will be pushed back to the left. This will prevent the player form being able to use the enemy
     * as a platform if the enemy is outside of a bubble.
     * 
     * This should only be executed if the enemy isn't enclosed inside a bubble. Otherwise, the playr will still get knocked back
     * even if the enemy is a bubble, so he won't be able to jump on top of a bubble as a platform.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is the player, nd if the enemy is not in circle/bubble form
        if (collision.gameObject.CompareTag("Player") && !isCircle)
        {
            // Get the Rigidbody2D of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Move the player some units to the left. The units can be adjusted in the Unity Editor.
                playerRb.position = new Vector2(playerRb.position.x - knockbackDistance, playerRb.position.y);

                //playerRb.position = new Vector2(playerRb.position.x - 5f, playerRb.position.y);

                //ApplyKnockback(collision, playerRb);
            }
        }
    }

    /* This method will transform the enemy into a bubble when the player attacks the enemy with his bubble magic attack.
     * 
     * That is, if the enemy is within the player's hitbox from the bubble attack, the enemy will be transformed into a circle.
     * 
     * 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player's hitbox
        if (collision.CompareTag("PlayerHitbox") && !isCircle)
        {
            // Transform the enemy into a circle
            TransformIntoCircle();
        }
    }

    /* This transform the enemy into a circle, that is, it will enclose the enemy within a bubble.
     * 
     */
    private void TransformIntoCircle()
    {
        // Set the isCircle flag to true
        isCircle = true;

        // Instantiate the circle GameObject at the enemy's position
        currentBubble = Instantiate(bubbleContainerPrefab, transform.position, Quaternion.identity);

        //GameObject circle = Instantiate(bubbleContainerPrefab, transform.position, Quaternion.identity);

        //// This destroys the original enemy object with the original sprite, so that only the bubble container is left.
        //Destroy(gameObject);

        // Alternatively, disable the current enemy GameObject
        gameObject.SetActive(false);

        //// Start the coroutine to revert the transformation after 5 seconds
        //StartCoroutine(RevertTransformation());
    }


    /* This will revert the transformation of the enemy back to its original form after a set amount of time.
     * 
     * That is, this will eliminate the bubble container, and re-render the enemy's original sprite.
     * 
     * To ensure the coroutine is executed when isCircle is set to true, you can start the coroutine within the 
     * TransformIntoCircle method. Additionally, you can add a check to ensure the coroutine is only started if isCircle 
     * is true.
     * 
     * Explanation:
        1.	The TransformIntoCircle method sets isCircle to true, instantiates the bubble, disables the enemy GameObject, and starts the RevertTransformation coroutine.
        2.	The RevertTransformation coroutine waits for 5 seconds, sets isCircle to false, destroys the bubble, and re-enables the enemy GameObject.
        3.	The Update method checks if isCircle is true and if the coroutine is not already running, then starts the coroutine.
        This ensures that the coroutine is executed when isCircle is set to true.
     */

    private IEnumerator RevertTransformation()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Set the isCircle flag to false
        isCircle = false;

        // Instantiate the enemy's original form GameObject at the bubble's position
        currentEnemy = Instantiate(enemysOriginalFormPrefab, transform.position, Quaternion.identity);

        // Destroy the current bubble GameObject
        Destroy(gameObject);

        //// Re-enable the current enemy GameObject (the original enemy sprite)
        //gameObject.SetActive(true);
    }

    /* Update method. This is executed 60 times per second.
     *
     * This will check if the enemy is in bubble form, and if it is, it will start the coroutine if not already started. The coroutine 
     * will revert the transformation of the enemy back to its original form after 5 seconds, and make the bubble container to disappear.
     */
    private void Update()
    {
        // Check if the enemy is in circle form and start the coroutine if not already started
        if (isCircle && !IsInvoking(nameof(RevertTransformation)))
        {
            StartCoroutine(RevertTransformation());
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // Continuously push the player away to prevent platforming
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

    //        if (playerRb != null)
    //        {
    //            // Move the player 20 units to the left
    //            playerRb.position = new Vector2(playerRb.position.x - 5f, playerRb.position.y);

    //            //ApplyKnockback(collision, playerRb);
    //        }
    //    }
    //}

    //private void ApplyKnockback(Collision2D collision, Rigidbody2D playerRb)
    //{
    //    // Calculate the direction from the enemy to the player
    //    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

    //    // Adjust the vertical component for extra force if desired
    //    knockbackDirection.y *= verticalKnockbackMultiplier;

    //    // Adjust the horizontal component for custom knockback strength
    //    knockbackDirection.x *= horizontalKnockbackMultiplier;


    //    // Apply knockback force to the player
    //    playerRb.velocity = Vector2.zero; // Reset current velocity for consistent knockback
    //    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    //}





























    //[Header("Knockback Settings")]
    //public float knockbackForce = 5f; // How strong the push-back effect is

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Check if the object colliding is the player
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // Get the Rigidbody2D of the player
    //        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

    //        if (playerRb != null)
    //        {
    //            // Calculate the direction from the enemy to the player
    //            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

    //            // Apply knockback force to the player
    //            playerRb.velocity = Vector2.zero; // Reset player's velocity before applying force
    //            playerRb.AddForce(pushDirection * knockbackForce, ForceMode2D.Impulse);
    //        }
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // Ensure the player doesn't "stand" on top of the enemy
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

    //        if (playerRb != null)
    //        {
    //            // Calculate the direction from the enemy to the player
    //            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

    //            // Apply knockback force to the player continuously
    //            playerRb.velocity = Vector2.zero; // Reset player's velocity before applying force
    //            playerRb.AddForce(pushDirection * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
    //        }
    //    }
    //}
}
