using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
class GameManagerInspector : Editor
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = target as GameManager;
    }

    public override void OnInspectorGUI()
    {
        if (gameManager == null)
            return;
        
        base.DrawDefaultInspector();
    }
}
