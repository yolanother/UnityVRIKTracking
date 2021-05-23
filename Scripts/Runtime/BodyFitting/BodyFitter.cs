﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DoubTech.SyntyIntegrations.FinalIK
{
    public class BodyFitter : MonoBehaviour
    {
        [SerializeField] private TrackedTransformManager transformManager;

        [Header("Model Components")]
        [SerializeField] private Animator animator;

        [Header("Model Measurements")]
        [ReadOnly] [SerializeField] private float modelHeight;
        [ReadOnly] [SerializeField] private float modelLeftArmLength;
        [ReadOnly] [SerializeField] private float modelRightArmLength;

        private Transform chest;
        private Transform rightLower;
        private Transform rightUpper;
        private Transform rightHand;
        private Transform leftLower;
        private Transform leftUpper;
        private Transform leftHand;

        [Header("Scales")]
        [ReadOnly]
        public float heightScale = 1;
        [ReadOnly]
        public float leftArmScale = 1;
        [ReadOnly]
        public float rightArmScale = 1;

        [Header("Player Measurements")]
        [ReadOnly]
        public BodyFit bodyFit = new BodyFit();

        public float CurrentLeftArmLength
        {
            get
            {
                var chestPosition = new Vector2(chest.position.x, chest.position.z);
                var leftHandPosition = new Vector2(
                    transformManager.leftHandTracker.transform.position.x,
                    transformManager.leftHandTracker.transform.position.z);
                return Vector3.Distance(chestPosition, leftHandPosition);
            }
        }

        public float CurrentRightArmLength
        {
            get
            {
                var chestPosition = new Vector2(chest.position.x, chest.position.z);
                var rightHandPosition = new Vector2(transformManager.rightHandTracker.transform.position.x,
                    transformManager.rightHandTracker.transform.position.z);
                return Vector2.Distance(chestPosition, rightHandPosition);
            }
        }

        public float CurrentHeight
        {
            get
            {
                var trackedHeadPosition =
                    transformManager.headTracker.transform.position;
                var trackedBodyPosition = transformManager.transform.position;

                return Vector3.Distance(trackedHeadPosition, trackedBodyPosition);
            }
        }

        private bool HasAllComponents => animator && transformManager &&
                                         transformManager.headTracker &&
                                         transformManager.leftHandTracker &&
                                         transformManager.rightHandTracker;

        private void OnValidate()
        {
            if(!animator) animator = transformManager.GetComponentInChildren<Animator>();
            if (!transformManager) transformManager = GetComponentInChildren<TrackedTransformManager>();

            chest = animator.GetBoneTransform(HumanBodyBones.Chest);
            rightLower = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            rightUpper = animator.GetBoneTransform(HumanBodyBones.RightUpperArm).parent;
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftLower = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftUpper = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).parent;
            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

            if (HasAllComponents && !Application.isPlaying)
            {
                modelHeight = CurrentHeight;
                modelLeftArmLength = CurrentLeftArmLength;
                modelRightArmLength = CurrentRightArmLength;
            }
        }

        private void OnEnable()
        {
            LoadBodyFit();
        }

        protected virtual void LoadBodyFit()
        {
            bodyFit = OnLoadBodyFit();

            // Validate fit
            if (bodyFit.playerHeight < 0.01) bodyFit.playerHeight = modelHeight;
            if (bodyFit.playerLeftArmLegth < 0.01) bodyFit.playerLeftArmLegth = modelLeftArmLength;
            if (bodyFit.playerRightArmLength < 0.01) bodyFit.playerRightArmLength = modelRightArmLength;

            if (heightScale < .01f) heightScale = 1;
            if (leftArmScale < .01f) leftArmScale = 1;
            if (rightArmScale < .01f) rightArmScale = 1;

            UpdateScales();
        }

        public virtual void SaveBodyFit()
        {
            OnSaveBodyFit(bodyFit);
        }

        protected virtual BodyFit OnLoadBodyFit()
        {
            var bodyFit = new BodyFit()
            {
                playerHeight = PlayerPrefs.GetFloat("PlayerHeight", modelHeight),
                playerLeftArmLegth = PlayerPrefs.GetFloat("PlayerLeftArmLength", modelLeftArmLength),
                playerRightArmLength = PlayerPrefs.GetFloat("PlayerRightArmLength", modelRightArmLength)
            };

            return bodyFit;
        }

        protected virtual void OnSaveBodyFit(BodyFit bodyFit)
        {
            PlayerPrefs.SetFloat("PlayerHeight", bodyFit.playerHeight);
            PlayerPrefs.SetFloat("PlayerLeftArmLength", bodyFit.playerLeftArmLegth);
            PlayerPrefs.SetFloat("PlayerRightArmLength", bodyFit.playerRightArmLength);
            PlayerPrefs.Save();
        }

        [Button]
        public void ResetStoredBodyFit()
        {
            bodyFit.playerHeight = modelHeight;
            bodyFit.playerLeftArmLegth = modelLeftArmLength;
            bodyFit.playerRightArmLength = modelRightArmLength;
            SaveBodyFit();
            UpdateScales();
        }

        [Button]
        public virtual void FitBody()
        {
            if (HasAllComponents)
            {
                bodyFit = new BodyFit()
                {
                    playerHeight = CurrentHeight,
                    playerLeftArmLegth = CurrentLeftArmLength,
                    playerRightArmLength = CurrentRightArmLength
                };
                SaveBodyFit();
                UpdateScales();
            }
        }

        private void UpdateScales()
        {

            heightScale = bodyFit.playerHeight / modelHeight;
            leftArmScale = bodyFit.playerLeftArmLegth / modelLeftArmLength;
            rightArmScale = bodyFit.playerRightArmLength / modelRightArmLength;

            animator.gameObject.transform.localScale = Vector3.one * heightScale;

            var inverseHeightScale = 1 / heightScale;
            rightUpper.transform.localScale = Vector3.one * inverseHeightScale * rightArmScale;
            leftUpper.transform.localScale = Vector3.one * inverseHeightScale * leftArmScale;
            OnUpdatedScales();
        }

        protected virtual void OnUpdatedScales() {}
    }

    [Serializable]
    public class BodyFit
    {
        public float playerHeight;
        public float playerLeftArmLegth;
        public float playerRightArmLength;
    }
}