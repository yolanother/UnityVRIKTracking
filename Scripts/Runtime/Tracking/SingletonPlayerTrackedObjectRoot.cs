using System;
using UnityEngine;

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class SingletonPlayerTrackedObjectRoot : MonoBehaviour
    {
        private static SingletonPlayerTrackedObjectRoot instance;

        public static SingletonPlayerTrackedObjectRoot Instance => instance;

        private void OnEnable()
        {
            instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }
    }
}
