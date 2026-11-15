using UnityEngine;

public class RecordMasAlto : MonoBehaviour
{
    const string BestRecordKey = "BestKillRecord";

    [SerializeField, Min(8)] int fontSize = 28;
    [SerializeField, Min(0f)] float screenMargin = 20f;

    GUIStyle recordStyle;

    public static int BestRecord => PlayerPrefs.GetInt(BestRecordKey, 0);

    public static void RegisterKills(int killsThisRun)
    {
        if (killsThisRun <= BestRecord)
        {
            return;
        }

        PlayerPrefs.SetInt(BestRecordKey, killsThisRun);
        PlayerPrefs.Save();
    }

    void OnGUI()
    {
        if (recordStyle == null)
        {
            recordStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = fontSize,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
        }

        recordStyle.fontSize = fontSize;
        float width = Mathf.Min(420f, Screen.width - screenMargin * 2f);
        float height = fontSize * 2.5f;
        Rect recordRect = new Rect(
            Screen.width - width - screenMargin,
            screenMargin,
            width,
            height);

        Color originalColor = GUI.color;
        GUI.color = Color.black;
        GUI.Label(new Rect(recordRect.x + 2f, recordRect.y + 2f, width, height),
            $"MEJOR RÉCORD\nEliminaciones: {BestRecord}", recordStyle);
        GUI.color = Color.white;
        GUI.Label(recordRect, $"MEJOR RÉCORD\nEliminaciones: {BestRecord}", recordStyle);
        GUI.color = originalColor;
    }
}
