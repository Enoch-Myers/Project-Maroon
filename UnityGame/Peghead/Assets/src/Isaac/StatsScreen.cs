using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelTimes
{
    public string levelName;
    public string[] times;
}

struct LevelResult
{
    public float time;
    public int lives;
    public string levelName;

    public LevelResult(string levelName)
    {
        this.time = float.PositiveInfinity;
        this.lives = 0;
        this.levelName = levelName;
    }

    public readonly override string ToString()
    {
        return $"LevelResult(levelName: {levelName}, time: {time}, lives: {lives})";
    }
}

public class StatsScreen : MonoBehaviour
{
    private LevelResult newLevelResult;
    private LevelResult bestLevelResult;
    private PlayerHealth playerHealth;
    private const string BestTimeKeyFormat = "bestLevelResult_{0}_time";
    private const string BestLivesKeyFormat = "bestLevelResult_{0}_lives";

    [SerializeField]
    private List<LevelTimes> levelTimes = new() {
        new LevelTimes {
            levelName = "Tutorial",
            times = new[] { "1:00", "2:00", "3:00" }
        }
    };

    void Awake()
    {
        string levelName = SceneManager.GetActiveScene().name;
        
        bestLevelResult = new LevelResult {
            time = PlayerPrefs.GetFloat(string.Format(BestTimeKeyFormat, levelName), float.PositiveInfinity),
            lives = PlayerPrefs.GetInt(string.Format(BestLivesKeyFormat, levelName), 0),
            levelName = levelName
        };

        newLevelResult = new LevelResult(levelName);

        PlayerHealth.OnPlayerDied += OnPlayerDied;
        PlayerHealth.OnLivesChanged += OnPlayerLivesChanged;
    }

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        /*
        // Test 3 lives
        GetGrade(new LevelResult {
            time = 60f,
            lives = 3,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 120f,
            lives = 3,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 180f,
            lives = 3,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 181f,
            lives = 3,
            levelName = "Tutorial"
        });
        
        // Test 2 lives
        GetGrade(new LevelResult {
            time = 60f,
            lives = 2,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 120f,
            lives = 2,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 180f,
            lives = 2,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 181f,
            lives = 2,
            levelName = "Tutorial"
        });
        
        // Test 1 lives
        GetGrade(new LevelResult {
            time = 60f,
            lives = 1,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 120f,
            lives = 1,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 180f,
            lives = 1,
            levelName = "Tutorial"
        });
        GetGrade(new LevelResult {
            time = 181f,
            lives = 1,
            levelName = "Tutorial"
        });
        */
        
        gameObject.SetActive(false);

        // Testing
        newLevelResult = new LevelResult {
            time = 60f,
            lives = 2,
            levelName = "Tutorial"
        };
        ShowLevelResults();
    }

    void OnPlayerDied()
    {
        ShowLevelResults();
    }

    void OnPlayerLivesChanged(int lives)
    {
        newLevelResult.lives = lives;
    }

    void Update()
    {
        if (playerHealth.GetCurrentLives() > 0) {
            newLevelResult.time += Time.deltaTime;
        }
    }

    private void ShowLevelResults()
    {
        // print("bestLevelResult: " + bestLevelResult + "\nnewLevelResult: " + newLevelResult);

        // Compare new results with best results
        var bestGradeResult = GetGrade(bestLevelResult);
        var newGradeResult = GetGrade(newLevelResult);

        bool gradeBetter = newGradeResult.alpha > bestGradeResult.alpha;
        bool timeBetter = newLevelResult.time < bestLevelResult.time;
        bool livesBetter = newLevelResult.lives > bestLevelResult.lives;
        
        // Update + save best results
        if (timeBetter) {
            bestLevelResult.time = newLevelResult.time;
        }
        if (livesBetter) {
            bestLevelResult.lives = newLevelResult.lives;
        }
        PlayerPrefs.SetFloat(string.Format(BestTimeKeyFormat, bestLevelResult.levelName), bestLevelResult.time);
        PlayerPrefs.SetInt(string.Format(BestLivesKeyFormat, bestLevelResult.levelName), bestLevelResult.lives);
        PlayerPrefs.Save(); // Ensure changes are flushed

        // Show results on stats screen
        TextWithDots timeText = transform.Find("Panel/Panel/Panel/Time").GetComponent<TextWithDots>();
        TextWithDots hpBonusText = transform.Find("Panel/Panel/Panel/HPBonus").GetComponent<TextWithDots>();
        TextWithDots gradeText = transform.Find("Panel/Panel/Panel/Grade/Text").GetComponent<TextWithDots>();
        GameObject highScoreImage = transform.Find("Panel/HighScore").gameObject;
        
        Color yellowColor;
        ColorUtility.TryParseHtmlString("#FFDD00", out yellowColor);

        timeText.rightText = Sec_To_MMSS(newLevelResult.time);
        if (timeBetter) {
            timeText.rightTextColor = yellowColor;
        }

        hpBonusText.rightText = newLevelResult.lives.ToString();
        if (livesBetter) {
            hpBonusText.rightText = newLevelResult.lives.ToString();
        }

        gradeText.rightText = newGradeResult.grade;
        highScoreImage.SetActive(gradeBetter);

        gameObject.SetActive(true);
    }

    private (string grade, float alpha) GetGrade(LevelResult levelResult)
    {
        List<float> gradeIndicatorAlphas = new();

        float timeBonusAlpha = 0; // [0, 1] Bigger is better
        foreach (var levelTimesEntry in levelTimes) {
            if (levelTimesEntry.levelName == levelResult.levelName) {
                for (int i = 0; i < levelTimesEntry.times.Count(); i++) {
                    string mmss = levelTimesEntry.times[i];
                    float thresholdSec = MMSS_To_Sec(mmss);

                    if (levelResult.time <= thresholdSec) { // Is result time faster than or equal to threshold time
                        timeBonusAlpha = 1f - (float)i/levelTimesEntry.times.Count();
                        break;
                    }
                }

                break;
            }
        }
        gradeIndicatorAlphas.Add(timeBonusAlpha);
        // print("[GetGrade()] timeBonusAlpha: " + timeBonusAlpha);
        
        float livesBonusAlpha = (levelResult.lives-1) / (playerHealth.GetMaxLives()-1);
        gradeIndicatorAlphas.Add(livesBonusAlpha);
        // print("[GetGrade()] livesBonusAlpha: " + livesBonusAlpha);

        float gradeAlpha = gradeIndicatorAlphas.Sum() / gradeIndicatorAlphas.Count();
        // print("[GetGrade()] gradeAlpha: " + gradeAlpha);

        string grade;
        if (gradeAlpha >= 0.95f)
            grade = "S+";
        else if (gradeAlpha >= 0.90f)
            grade = "S";
        else if (gradeAlpha >= 0.85f)
            grade = "A+";
        else if (gradeAlpha >= 0.80f)
            grade = "A";
        else if (gradeAlpha >= 0.75f)
            grade = "A-";
        else if (gradeAlpha >= 0.70f)
            grade = "B+";
        else if (gradeAlpha >= 0.65f)
            grade = "B";
        else if (gradeAlpha >= 0.60f)
            grade = "B-";
        else if (gradeAlpha >= 0.55f)
            grade = "C+";
        else if (gradeAlpha >= 0.50f)
            grade = "C";
        else if (gradeAlpha >= 0.45f)
            grade = "C-";
        else if (gradeAlpha >= 0.40f)
            grade = "D+";
        else if (gradeAlpha >= 0.35f)
            grade = "D";
        else
            grade = "F";
        // print("[GetGrade()] grade: " + grade);

        return (grade, gradeAlpha);
    }

    private static float MMSS_To_Sec(string mmss)
    {
        // Format: "MM:SS" (e.g., "02:15")
        if (string.IsNullOrWhiteSpace(mmss)) return 0f;
        var parts = mmss.Split(':');
        if (parts.Length != 2) return 0f;

        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);

        return minutes * 60f + seconds;
    }

    private static string Sec_To_MMSS(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    [ContextMenu("Clear All Saved Level Results")]
    private void ClearSavedResults()
    {
        foreach (var levelTimesEntry in levelTimes)
        {
            PlayerPrefs.DeleteKey(string.Format(BestTimeKeyFormat, levelTimesEntry.levelName));
            PlayerPrefs.DeleteKey(string.Format(BestLivesKeyFormat, levelTimesEntry.levelName));
        }
        PlayerPrefs.Save();
    }
}
