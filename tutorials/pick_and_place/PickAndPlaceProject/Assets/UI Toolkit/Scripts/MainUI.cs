using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    Button placeTableButton;

    Button pickAndPlaceButton;

    [SerializeField]
    ARCursor arCursor;

    [SerializeField]
    TrajectoryPlanner trajectoryPlanner;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        placeTableButton = root.Q<Button>("PlaceTableButton");
        placeTableButton.clicked += arCursor.PlaceTable;

        pickAndPlaceButton = root.Q<Button>("PickPlaceButton");
        pickAndPlaceButton.clicked += trajectoryPlanner.PublishJoints;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
