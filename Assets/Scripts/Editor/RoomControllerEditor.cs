using UnityEngine;
using UnityEditor;
using Cinemachine;

[CustomEditor(typeof(RoomController))]
public class RoomControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        // Temporary variables
        RoomController roomController = (RoomController) target;


        // If the button is pressed, use the function
        if(GUILayout.Button("Reset Room Camera Boundaries"))
            roomController.ResetRoomCameraBoundaries();

        
        // Update room boundaries
        // Only if camera is not locked
        if (!roomController.lockedCamera && roomController.GetComponent<PolygonCollider2D>() != null)
        {
            PolygonCollider2D _boundary = roomController.GetComponent<PolygonCollider2D>();
            Vector2[] _boundaryPoints = new Vector2[4]; 


            _boundaryPoints[0] = roomController.topRightPoint;
            _boundaryPoints[1] = new Vector2(roomController.bottomLeftPoint.x, roomController.topRightPoint.y);
            _boundaryPoints[2] = roomController.bottomLeftPoint;
            _boundaryPoints[3] = new Vector2(roomController.topRightPoint.x, roomController.bottomLeftPoint.y);


            _boundary.points = _boundaryPoints;
        }
    }
}
