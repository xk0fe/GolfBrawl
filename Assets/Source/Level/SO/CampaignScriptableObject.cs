using UnityEngine;

namespace Source.Level.SO
{
    [CreateAssetMenu(fileName = "CampaignScriptableObject", menuName = "SO/Game/CampaignScriptableObject", order = 0)]
    public class CampaignScriptableObject : ScriptableObject
    {
        public string Name;
        public LevelConstructor[] Levels;
    }
}