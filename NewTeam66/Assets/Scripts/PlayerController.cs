using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;

//TO FIX: IF THE PLAYER IS NOT MOVING RECORD THE SOURCE EVERY X SECONDS 
//SO THAT THE PLAYER WILL NOT CONTINUE TO WRITE IF HE'S JUST PINCHING
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    [SerializeField]
    OVRHand input;

    // [SerializeField]
    // Transform pointer;

    public bool isHoldingPen = false;
    bool isPinching = false;
    bool isMoving = false;

    public bool isWriting = false;

    public Transform movementSource;
    [SerializeField]
    float newPosThreshold = 0.05f;
    List<Vector3> positionList = new List<Vector3>();

    [SerializeField]
    GameObject debugCube;

    [SerializeField]
    bool creationMode = true;

    [SerializeField]
    string newGestureName;

    [SerializeField]
    float recognitionThreshold = 0.85f;
    [SerializeField]
    int startRecognitionThreshold = 20;

    List<Gesture> trainingSet = new List<Gesture>();
    [SerializeField]
    List<TextAsset> gestureXMLs = new List<TextAsset>();


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] gestureFiles = new string[gestureXMLs.Count];
        for(int i = 0; i < gestureXMLs.Count; i++)
        {
            gestureFiles[i] = gestureXMLs[i].text;
        }
        //string[] gestureFiles = Directory.GetFiles(Application.dataPath+"/Gesture/", "*.xml");
        //string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach(var item in gestureFiles)
        {
            //trainingSet.Add(GestureIO.ReadGestureFromFile(item));
            trainingSet.Add(GestureIO.ReadGestureFromXML(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!input.IsSystemGestureInProgress)
        if (!input.IsSystemGestureInProgress && isHoldingPen)
        {
            //Pinching input for each finger
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Thumb) && input.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                isPinching = true;
            }
            else
            {
                isPinching = false;
            }



            // //Raycasting using pointer vector
            // if (input.IsPointerPoseValid)
            // {
            //     pointer.rotation = input.PointerPose.rotation;
            //     pointer.position = input.PointerPose.position;
            // }


            // RaycastHit hit;
            // if( Physics.Raycast(input.PointerPose.position, input.PointerPose.forward, out hit, spinningCubeLayer) )
            // {
            //     SpinningCube S = hit.collider.GetComponent<SpinningCube>();
            //     if( S )
            //     {
            //         S.Spin();
            //     }
            // }
        }
        //Starts action
        if(!isMoving && isPinching)
        {
            StartMovement();
        }
        //During Action
        else if(isMoving && isPinching)
        {
            UpdateMovement();
        }
        //End Action
        else if(isMoving && !isPinching)
        {
            EndMovement();
        }


    }

    void StartMovement()
    {
        isMoving = true;
        positionList.Clear();
        positionList.Add(movementSource.position);
    }

    void UpdateMovement()
    {
        
        Vector3 lastPos = positionList[positionList.Count - 1];
        if(Vector3.Distance(lastPos, movementSource.position) > newPosThreshold && positionList.Count < startRecognitionThreshold)
        {
            positionList.Add(movementSource.position);
            Destroy(Instantiate(debugCube, movementSource.position, Quaternion.identity), 3);
        }
        else if(Vector3.Distance(lastPos, movementSource.position) > newPosThreshold && positionList.Count == startRecognitionThreshold)
        {
            positionList.RemoveAt(0);
            positionList.Add(movementSource.position);
            Destroy(Instantiate(debugCube, movementSource.position, Quaternion.identity), 3);
            GestureRecognition(false);
        }
    }

    void EndMovement()
    {
        isMoving = false;

        //Create gesture from the position list
        GestureRecognition(creationMode);
        isWriting = false;
    }

    void GestureRecognition(bool isCreation)
    {
        Point[] pointArray = new Point[positionList.Count];

        for(int i = 0; i < positionList.Count; i++)
        {
            Vector3 rightPoint = Vector3.ProjectOnPlane(positionList[i], GameObject.Find("CenterEyeAnchor").transform.right);
            pointArray[i] = new Point(rightPoint.z, rightPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray, newGestureName);

        if(isCreation)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            if(result.Score > recognitionThreshold)
            {
                isWriting = true;
            }
        }
    }

    
}
