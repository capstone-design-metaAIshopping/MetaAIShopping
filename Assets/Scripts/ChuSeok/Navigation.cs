#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using IBM.Watsson.Examples;

public class Navigation : MonoBehaviour
{
    #region
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://api.us-south.speech-to-text.watson.cloud.ibm.com\"")]
    [SerializeField]
    private string _serviceUrl;
   // [Tooltip("Text field to display the results of streaming.")]
    //public Text ResultsField;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;

    [Header("Parameters")]
    // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
    [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
    [SerializeField]
    private string _recognizeModel;

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToTextService _service;
    #endregion
    public static Navigation _Instance;
    NavMeshAgent player;
    //NavMeshPath path;
    LineRenderer line;
    List<Vector3> point;
    [SerializeField]
    Transform tShirt;
    [SerializeField]
    Transform kickBoard;
    [SerializeField]
    Transform wine;
    GameObject[] products;
    private void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        // LogSystem.InstallDefaultReactors();
        // Runnable.Run(CreateService());
        products = GameObject.FindGameObjectsWithTag("product");

        player = GetComponent<NavMeshAgent>();
        //path = new NavMeshPath();
        line = GetComponent<LineRenderer>();
       /// _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "ko-KR_BroadbandModel" : _recognizeModel);
    }

    public void ClickAgent()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }
    public void StopAgent()
    {
        StopRecording();
    }
    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        _service = new SpeechToTextService(authenticator);
        if (!string.IsNullOrEmpty(_serviceUrl))
        {
            _service.SetServiceUrl(_serviceUrl);
        }
        _service.StreamMultipart = true;

        Active = true;
        StartRecording();
    }

    public bool Active
    {
        get { return _service.IsListening; }
        set
        {
            if (value && !_service.IsListening)
            {
                _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "ko-KR_BroadbandModel" : _recognizeModel);
                _service.DetectSilence = true;
                _service.EnableWordConfidence = true;
                _service.EnableTimestamps = true;
                _service.SilenceThreshold = 0.01f;
                _service.MaxAlternatives = 1;
                _service.EnableInterimResults = true;
                _service.OnError = OnError;
                _service.InactivityTimeout = -1;
                _service.ProfanityFilter = false;
                _service.SmartFormatting = true;
                _service.SpeakerLabels = false;
                _service.WordAlternativesThreshold = null;
                _service.EndOfPhraseSilenceTime = null;
                _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _service.IsListening)
            {
                _service.StopListening();
            }
        }
    }

    private void StartRecording()
    {
        if (_recordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            _recordingRoutine = Runnable.Run(RecordingHandler());
        }
    }

    private void StopRecording()
    {
        if (_recordingRoutine != 0)
        {
            Microphone.End(_microphoneID);
            Runnable.Stop(_recordingRoutine);
            _recordingRoutine = 0;
        }
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {
        Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
        _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
        yield return null;      // let _recordingRoutine get set..

        if (_recording == null)
        {
            StopRecording();
            yield break;
        }

        bool bFirstBlock = true;
        int midPoint = _recording.samples / 2;
        float[] samples = null;

        while (_recordingRoutine != 0 && _recording != null)
        {
            int writePos = Microphone.GetPosition(_microphoneID);
            if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
            {
                Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
              || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
                record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                record.Clip.SetData(samples, 0);

                _service.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)_recordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }
        }
        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0}\n", alt.transcript);
                    //                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    Log.Debug("ExampleStreaming.OnRecognize()", text);
                    UICtrl._Instance.ResultsField.text = text;
                    
                    foreach(GameObject product in products)
                    {
                        if(text.Contains(product.name))
                        {
                            player.ResetPath();
                            //ViewCtrl._Instance.Move1F();
                            player.SetDestination(product.transform.position);
                            //transform.LookAt(product.transform);
                            Debug.Log(string.Format("{0}·Î ÀÌµ¿", product.name));
                            break;
                        }
                    }
                    /*
                    if (UICtrl._Instance.ResultsField.text.Contains("T. shirts") || UICtrl._Instance.ResultsField.text.Contains("Æ¼¼ÅÃ÷"))
                    {
                        player.ResetPath();
                        ViewCtrl._Instance.Move1F();
                        player.SetDestination(tShirt.position);
                        transform.LookAt(tShirt);
                        Debug.Log("Æ¼¼ÅÃ÷·Î ÀÌµ¿");
                    }
                    else if (UICtrl._Instance.ResultsField.text.Contains("¿ÍÀÎ"))
                    {
                        player.ResetPath();
                        ViewCtrl._Instance.Move1F();
                        player.SetDestination(wine.position);
                        transform.LookAt(wine);
                        Debug.Log("¿ÍÀÎÀ¸·Î ÀÌµ¿");
                    }
                    */
                    if (UICtrl._Instance.ResultsField.text.Contains("ÀÏ Ãþ") || UICtrl._Instance.ResultsField.text.Contains("ÀÏÃþ"))
                    {
                        player.ResetPath();
                        ViewCtrl._Instance.Move1F();
                        Debug.Log("1ÃþÀÌµ¿");
                        break;
                    }
                    else if (UICtrl._Instance.ResultsField.text.Contains("ÀÌÃþ") || UICtrl._Instance.ResultsField.text.Contains("ÀÌ Ãþ"))
                    {
                        player.ResetPath();
                        ViewCtrl._Instance.Move2F();
                        Debug.Log("2ÃþÀÌµ¿");
                        break;
                    }
                    else if (UICtrl._Instance.ResultsField.text.Contains("»ï Ãþ") || UICtrl._Instance.ResultsField.text.Contains("»ïÃþ"))
                    {
                        player.ResetPath();
                        ViewCtrl._Instance.Move3F();
                        Debug.Log("3ÃþÀÌµ¿");
                        break;
                    }
                    if ( UICtrl._Instance.ResultsField.text.Contains("¸ØÃç") || UICtrl._Instance.ResultsField.text.Contains("±×¸¸") || UICtrl._Instance.ResultsField.text.Contains("¸Ø Ãç"))
                    {
                        player.isStopped = true;
                        player.ResetPath();
                        Debug.Log("¸ØÃç");
                        transform.GetComponent<LineRenderer>().enabled = false;
                        break;
                    }
                    transform.GetComponent<LineRenderer>().enabled = true;
                }
                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                    }
                }
                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                        foreach (var alternative in wordAlternative.alternatives)
                            Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.SetDestination(tShirt.position);
        }
        DisplayLineDestination();
        /*
        if(path.corners.Length>1)
        {
            NavMesh.CalculatePath(transform.position, tShirt.position, NavMesh.AllAreas, path);
            Vector3[] corners = path.corners;
            lineRenderer.SetPositions(corners);
        }
        */
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.SetDestination(kickBoard.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.SetDestination(wine.position);
        }
    }
    private void DisplayLineDestination()
    {
        //transform.GetComponent<LineRenderer>().enabled = true;
        //Debug.Log("¼± ±×¸®±â");
        if (player.path.corners.Length < 2) return;
        int i = 1; 
        while (i < player.path.corners.Length)
        {
            line.positionCount = player.path.corners.Length;
            point = player.path.corners.ToList();
            for (int j = 0; j < point.Count; j++)
            {
                line.SetPosition(j, point[j]);
            }
            i++;
        }
    }
}