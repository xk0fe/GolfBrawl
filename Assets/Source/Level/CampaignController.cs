using Source.Level.SO;

namespace Source.Level
{
    public class CampaignController
    {
        private CampaignScriptableObject _currentCampaignScriptableObject;
        private LevelController _levelController;
        
        public CampaignController(LevelController levelController)
        {
            _levelController = levelController;
        }
        
        public void StartCampaign(CampaignScriptableObject campaignScriptableObject)
        {
            _currentCampaignScriptableObject = campaignScriptableObject;
            _levelController.SetLevels(campaignScriptableObject.Levels);
            _levelController.Start();
        }
        
        public void LoadNextLevel()
        {
            _levelController.LoadNextLevel();
        }
    }
}