using UnityEngine;

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class TrackedTransformManager : MonoBehaviour
    {
        [SerializeField] private GameObject trackedObjectRoot;

        [SerializeField] public TrackedTransformTracker headTracker;
        [SerializeField] public TrackedTransformTracker leftHandTracker;
        [SerializeField] public TrackedTransformTracker rightHandTracker;

        public GameObject TrackedObjectRoot
        {
            get => trackedObjectRoot;
            set
            {
                trackedObjectRoot = value;
                OnFindTrackedTransforms();
            }
        }

        private void OnEnable()
        {
            if(trackedObjectRoot) OnFindTrackedTransforms();
        }

        protected virtual void OnFindTrackedTransforms()
        {
            var trackedTransforms = trackedObjectRoot.GetComponentsInChildren<TrackedTransform>();

            for (int i = 0; i < trackedTransforms.Length; i++)
            {
                var t = trackedTransforms[i];
                switch (t.BodyPart)
                {
                    case TrackedTransformBodyPart.Head:
                        headTracker.trackedTransform = t;
                        break;
                    case TrackedTransformBodyPart.LeftHand:
                        leftHandTracker.trackedTransform = t;
                        break;
                    case TrackedTransformBodyPart.RightHand:
                        rightHandTracker.trackedTransform = t;
                        break;
                }
            }
        }
    }
}
