using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    //sources
    //https://www.youtube.com/watch?v=j1-OyLo77ss
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target; //In edit mode get the selected target with "field of view" script
        //draw the range circle of the selected target
        Handles.color = Color.white;

        //NOTE it does not work at the moment as it only draws the traingle up and not in the other three directions
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, fov.ZombieDirection, 360, fov.radius);
        //get the view angles
        //there are two view angels one positive and one negative to create a triangle
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);
        //Draw the two view angle lines
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);
        //if the target can see the player draw a green line from the target to the player
        if(fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }
    
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
