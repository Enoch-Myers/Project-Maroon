/*
  Projectile_behavior.cs

A simple Unity C# script that controls the movement and collision behavior of a 2D projectile.

 Overview

This script moves a projectile continuously in the right direction and destroys it upon collision with any object.

---

 Script Details

 Movement
- The projectile moves in the direction of its local right axis (`transform.right`) every frame.
- Movement speed is controlled by the `speed` variable and is frame rate-independent using `Time.deltaTime`.

 Collision
- On colliding with any 2D collider, the projectile is destroyed using `Destroy(gameObject)`.

---

 How to Use

1. Attach the script to a projectile GameObject
2. Ensure the projectile GameObject has:
   - A Rigidbody2D component (set to Kinematic or Dynamic).
   - A Collider2D (like BoxCollider2D or CircleCollider2D).
3. Set the `speed` in the Inspector as needed.

---

 Example

csharp
public GameObject bulletPrefab;

void Fire()
{
    Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
}
*/
