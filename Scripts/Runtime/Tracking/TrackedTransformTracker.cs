using UnityEngine;

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class TrackedTransformTracker : MonoBehaviour
    {
        [SerializeField] public TrackedTransformBodyPart bodyPart;

        [HideInInspector]
        public TrackedTransform trackedTransform;

        private void LateUpdate()
        {
            transform.position = trackedTransform.transform.position;
            transform.rotation = trackedTransform.transform.rotation;
        }
    }
}
