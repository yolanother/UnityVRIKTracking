using UnityEngine;

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class TrackedTransform : MonoBehaviour
    {
        [SerializeField] private TrackedTransformBodyPart bodyPart;

        public TrackedTransformBodyPart BodyPart => bodyPart;
    }
}
