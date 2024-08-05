using System;

namespace Source.Network
{
    public class LocalPlayerData
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        public Action<string> OnNicknameUpdate;
        public Action<int> OnLevelUpdate;
        public Action<int> OnExperienceUpdate;
    }
}