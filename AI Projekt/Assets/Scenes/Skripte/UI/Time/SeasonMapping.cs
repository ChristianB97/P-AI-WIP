using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Translation;
using UnityEngine;

public class SeasonMapping : MonoBehaviour
{
    private string SEASON_ONE_KEY_NAME = "spring";

    private string SEASON_TWO_KEY_NAME = "summer";

    private string SEASON_THREE_KEY_NAME = "fall";

    private string SEASON_FOUR_KEY_NAME = "winter";

    private Dictionary<int, string> seasons;

    private Translator translator;

    private void Awake()
    {
        translator = TranslationEngine.Instance.Translator;
        seasons = new Dictionary<int, string>();
        seasons.Add(0, SEASON_ONE_KEY_NAME);
        seasons.Add(1, SEASON_TWO_KEY_NAME);
        seasons.Add(2, SEASON_THREE_KEY_NAME);
        seasons.Add(3, SEASON_FOUR_KEY_NAME);
    }

    public string GetSeasonName(int seasonID)
    {
        if (seasons.ContainsKey(seasonID))
        {
            string key = seasons[seasonID];
            return GetAndTranslateString(key);
        }
        return null;
    }

    private string GetAndTranslateString(string key)
    {
        if (translator == null)
        {
            translator = TranslationEngine.Instance.Translator;
        }
        return translator[key];
    }
}
