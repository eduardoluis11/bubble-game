using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* To achieve the behavior where the player is pushed back when colliding with the enemy (regardless of the 
 * collision point, such as on top), we can create a script for the enemy. The script will detect collisions 
 * with the player and apply a force to push the player back. Additionally, we'll ensure that the player 
 * cannot use the enemy as a platform by preventing upward movement when colliding.

Here's the script for the enemy:

---

### Script: `Enemy.cs`

```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackForce = 5f; // How strong the push-back effect is
    public Vector2 knockbackDirection = Vector2.zero; // Direction is calculated dynamically

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Calculate the direction from the enemy to the player
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;

                // Apply knockback force to the player
                playerRb.AddForce(pushDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Ensure the player doesn't "stand" on top of the enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Push the player away continuously
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * knockbackForce * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }
}
```

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

Let me know if you'd like help with any additional adjustments!
 * 
 * 
 * 
 */

public class Enemy : MonoBehaviour
{


    [Header("Knockback Settings")]
    public float knockbackForce = 5f; // How strong the push-back effect is
    public float verticalKnockbackMultiplier = 1.5f; // Optional: Amplify vertical knockback if needed
    public float horizontalKnockbackMultiplier = 1f; // Multiplier for horizontal knockback


    /* This will make it so that, if the player collides with the enemy, the player will be pushed back to the left.
     * 
     * That is, if the player touches the enemy at any angle, be it by touching him from any side, or by jumping on top 
     * of him, the player will be pushed back to the left. This will prevent the player form being able to use the enemy
     * as a platform if the enemy is outside of a bubble.
     * 
     * 
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Move the player some units to the left
                playerRb.position = new Vector2(playerRb.position.x - 5f, playerRb.position.y);

                //ApplyKnockback(collision, playerRb);
            }
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
