/*
used chat gpt for this one since i dident feel like writing descriptions--miles

# EnemyDamageTests

This PlayMode test suite verifies the `EnemyAI` script’s ability to take and respond to damage from projectiles.

## 📄 Test Coverage

| Test # | Description |
|--------|-------------|
| 1      | Verifies the enemy takes damage when hit by a projectile. |
| 2      | Confirms the enemy dies when health reaches zero. |
| 3      | Ensures the projectile is destroyed on impact. |
| 4      | Validates the enemy does not take damage after death. |
| 5      | Tests that multiple projectiles reduce enemy health cumulatively. |
| 6      | Confirms projectiles with incorrect tags do not deal damage. |
| 7      | Verifies that projectiles only deal damage if their collider is a trigger. |

## ✅ Requirements

- The enemy must have the `EnemyAI` script.
- The projectile must have the `Projectile_behavior` script.
- Projectiles should be tagged `"Projectile"` unless testing invalid tags.
- Collisions use `OnTriggerEnter2D`.

## 🧪 Type
**PlayMode** (Physics, GameObject lifecycles)

---

### ✅ **README: ProjectileBehaviorTests.cs** *(Assumed from earlier context)*

```markdown
# ProjectileBehaviorTests

This test suite validates the behavior of projectile GameObjects in response to player input and world interactions.

## 📄 Test Coverage

| Test # | Description |
|--------|-------------|
| 1      | Ensures the projectile moves in the correct direction based on player orientation. |
| 2      | Confirms the projectile spawns at the correct launch offset. |
| 3      | Validates the projectile is destroyed on collision. |
| 4      | Checks that multiple projectiles can be instantiated without issue. |
| 5      | Ensures that projectiles do not spawn if the prefab is null (error prevention). |

## ✅ Requirements

- `Projectile_behavior` script must be present on the projectile prefab.
- `player_attack_controller` must instantiate projectiles using `Instantiate`.

## 🧪 Type
**PlayMode** (Input-based instantiation, movement, collisions)

---

### ✅ **README: CoinPickupTests.cs** *(Assumed from coin system)*

```markdown
# CoinPickupTests

This test suite ensures that the coin pickup and counting system works as intended when the player collides with coin GameObjects.

## 📄 Test Coverage

| Test # | Description |
|--------|-------------|
| 1      | Verifies that coin count increases on coin pickup. |
| 2      | Ensures coin GameObject is destroyed upon pickup. |
| 3      | Confirms that only objects tagged `"Coins"` trigger the pickup. |
| 4      | Validates pickup only occurs when using trigger colliders. |
| 5      | Performance check for picking up many coins. |
| 6      | Verifies that coin count does not increase if CM (Coin_despawner) is missing. |

## ✅ Requirements

- Coin GameObjects must be tagged `"Coins"`.
- Coins should have trigger colliders.
- The `coinPickup` script must be attached to the player or collider.

## 🧪 Type
**PlayMode** (Tag checking, object destruction, UI/game state updates)

---

Let me know if you want combined readmes, auto-generated badges, or markdown previews for Unity's documentation!
*/