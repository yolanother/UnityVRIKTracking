using System;
using System.Collections;
using DoubTech.SyntyIntegrations.FinalIK;
using TMPro;
using UnityEngine;

namespace DoubTech.VRIkTracking.CalibrationRoom
{
    public class CalibrateButton : MonoBehaviour
    {
        [SerializeField] private BodyFitter bodyFitter;
        [SerializeField] private float buttonPressDepth = .025f;
        [SerializeField] private float buttonPressSpeed = 3;
        [SerializeField] private int calibrationCountdownSeconds = 5;
        [SerializeField] private TextMeshProUGUI countdown;


        private Vector3 buttonPressedPosition;

        private Vector3 targetPosition;
        private Vector3 startPosition;
        private float calibrationTimeout;

        private void Start()
        {
            startPosition = Vector3.zero;
            buttonPressedPosition = Vector3.up * buttonPressDepth;
            targetPosition = startPosition;
        }

        private void OnValidate()
        {
            if (!bodyFitter) bodyFitter = FindObjectOfType<BodyFitter>();
            buttonPressedPosition = Vector3.up * buttonPressDepth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time - calibrationTimeout > calibrationCountdownSeconds)
            {
                calibrationTimeout = Time.time;
                Debug.Log("Hit the trigger.");
                targetPosition = buttonPressedPosition;
                StartCoroutine(StartCalibration());
            }
        }

        private IEnumerator StartCalibration()
        {
            var text = countdown.text;
            Debug.Log("Starting calibration in 5 sec.");
            for (int i = 0; i < calibrationCountdownSeconds; i++)
            {
                calibrationTimeout = Time.time;
                countdown.text = "Calibration will begin in " + (calibrationCountdownSeconds - i) + " seconds.\nHold your hands straight forward.";
                yield return new WaitForSeconds(1);
            }

            bodyFitter.FitBody();
            countdown.text = text;
            calibrationTimeout = Time.time;
        }

        private void OnTriggerExit(Collider other)
        {
            targetPosition = startPosition;
        }

        private void LateUpdate()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * buttonPressSpeed);
        }
    }
}
