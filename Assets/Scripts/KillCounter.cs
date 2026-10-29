using System;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance { get; private set; }
    public static event Action<int> KillCountChanged;

    public static int Kills => Instance != null ? Instance.kills : 0;

    [SerializeField] int kills;

    GUIStyle hudStyle;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one KillCounter can be active at a time.", this);
            enabled = false;
            return;
        }

        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void RegisterKill()
    {
        kills++;
        KillCountChanged?.Invoke(kills);
    }

    void OnGUI()
    {
        if (hudStyle == null)
        {
            hudStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
        }

        const float width = 240f;
        const float height = 42f;
        Rect counterRect = new Rect((Screen.width - width) * 0.5f, 16f, width, height);
        GUI.Box(counterRect, GUIContent.none);
        GUI.Label(counterRect, $"Kills: {kills}", hudStyle);
    }
}
