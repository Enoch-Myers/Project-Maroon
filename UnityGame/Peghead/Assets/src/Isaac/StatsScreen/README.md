# Stats Screen Class

## Description:

This class is a component of the `StatsScreen` prefab. The `StatsScreen` prefab gets added to the scene by the `GameManager` whenever a **level scene** loads. A scene is identified as a **level scene** if its path starts with `Assets/Scenes/Levels`.

Upon the `StatsScreen` prefab being added to the scene, this class does the following:

In `StatsScreen.Awake()`

1. Queries the best result from saved data for the current level into `bestLevelResult`.
2. Initializes `newLevelResult` with default values for the upcoming result of the current level.
3. Subscribes to the `PlayerHealth.OnPlayerDied` event to automatically show the level results when the player dies.
4. Subscribes to the `PlayerHealth.OnLivesChanged` event to automatically update the `newLevelResult.lives` statistic whenever the player loses a life.

In `StatsScreen.Start()`

5. Hides the `StatsScreen` ui because it should not be visible until the player dies or until the player reaches the end of a level.

In `StatsScreen.Update()`

6. Begins accumulating the time it takes for the player to finish the level, which is stored in `newLevelResult.time`.

## Editor Configuration:

### `Level Times`
This class has a serialized field `Level Times` which can be modified through the inspect window after clicking on the `StatsScreen` prefab. The `Level Times` field is a list which defines how *good* certain finish times are for each level (i.e. how level finish times influence the grade calculation for each level).
    
### `Clear Saved Results`
This class has a context menu method `Clear Saved Results` which can be invoked through the inspect window after clicking the `StatsScreen` prefab, then clicking the three dots in the top right of the `Stats Screen (Script)` component, and finally clicking the `Clear Saved Results` option at the bottom of the just-appeared context menu. This will permanently clear all saved best results from all levels, which is helpful for testing.

## Public methods:

```cs
StatsScreen.ShowLevelResults()
```

This method does the following:
1. Saves the `newLevelResult` stats if they are better than the `bestLevelResult` stats.
2. Calculates a grade (from `F (<50%) to (98%<=) S+`) based on the `newLevelResult` stats.
3. Displays the `newLevelResult.time`, `newLevelResult.lives`, and calculated grade on the stats screen with the `NEW HIGH SCORE` banner if the calculated grade is better than the old best grade.

This method currently gets called by the `Finish_Line` class when the player reaches the end of a level.