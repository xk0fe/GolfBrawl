using Source.Trigger.Base;
using ThirdParty.Editor.TagSelector;
using UnityEngine;

namespace Source.Trigger.Validators
{
    public class TriggerTagValidator : TriggerValidator
    {
        [SerializeField, TagSelector]
        private string _tag;

        public override bool IsValid(Collider other)
        {
            return other.CompareTag(_tag);
        }
    }
}