# **Ranged Enemy Documentation**

## **General Description**
The **Ranged Enemy** is an enemy type that attacks the player by firing projectiles. It detects the player within a specified range and periodically shoots projectiles in their direction. The system is designed to be modular and adaptable.

---

## **Unity Components**
1. **Transform**: Defines the enemy's position, rotation, and scale.
2. **SpriteRenderer**: Displays the enemy's sprite.
3. **Rigidbody2D**: Handles physics interactions.
4. **BoxCollider2D**: Detects collisions.
5. **RangedEnemy (Script)**: Controls the enemy's behavior.

---

## **Key Properties (RangedEnemy Script)**
- **`projectilePrefab`**: The projectile GameObject to instantiate.
- **`projectileSpeed`**: Speed of the projectile.
- **`attackCooldown`**: Time between attacks.
- **`playerDetectionRange`**: Distance within which the enemy detects the player.
- **`playerLayer`**: Layer used to identify the player.

---

## **Key Methods**
- **`Update()`**: Detects the player and triggers attacks based on cooldown.
- **`Attack()`**: Instantiates and fires a projectile toward the player.
- **`ShootProjectile(Vector2 direction)`**: Handles projectile instantiation and velocity.
- **`PlayerDetected()`**: Checks if the player is within detection range.

---

## **Setup Instructions**
### 1. **Create the Ranged Enemy**
   - Add a sprite to the scene and attach `Rigidbody2D`, `BoxCollider2D`, and the `RangedEnemy` script.
   - Configure the `Rigidbody2D` as `Kinematic`.

### 2. **Assign the Projectile Prefab**
   - Create a projectile prefab with:
     - `Rigidbody2D` (set to `Kinematic`).
     - `CircleCollider2D`.
     - A separate **Projectile Script** for modular behavior (e.g., damage, lifetime).
   - Drag the prefab into the `projectilePrefab` field of the `RangedEnemy` script.

### 3. **Configure the RangedEnemy Script**
   - Set `projectileSpeed`, `attackCooldown`, and `playerDetectionRange` in the Inspector.
   - Assign the `playerLayer` to detect the player.

---

## **Modularity**
The **Ranged Enemy** is designed to be modular:
- Attach any projectile prefab with its own **Projectile Script** to define unique behaviors (e.g., homing, explosive, piercing).
- The `RangedEnemy` script only handles instantiation and firing, leaving projectile behavior entirely up to the attached script.

This modular approach allows you to reuse the **RangedEnemy** script with different projectile types, making it highly flexible and adaptable.
