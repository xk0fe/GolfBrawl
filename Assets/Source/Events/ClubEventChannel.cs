using Source.Clubs;
using UnityEngine;

namespace Source.Events
{
    [CreateAssetMenu(fileName = "ClubEventChannel", menuName = "SO/Events/ClubEventChannel", order = 0)]
    public class ClubEventChannel : GenericEventChannel<ClubScriptableObject>
    {
    }
}