using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphwayTest : MonoBehaviour 
{
    public const int MAX_SPEED = 10;
    public const int ACCELERATION = 2;

    [Tooltip("Enable Debug Mode to see algoritm in action slowed down. MAKE SURE GIZMOS ARE ENABLED!")]
    public bool debugMode = false;

	[Header("Player Camera")]
	public Camera cam;

    private GwWaypoint[] waypoints;
	public float speed = 0;
	
	void Update()
	{
		// Handle mouse click
		if (Input.GetMouseButtonDown(0))
		{
            // Check if an object in the scene was targeted
			RaycastHit hit;
			
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if (Physics.Raycast(ray, out hit))
            // {
            //     // Object hit
            //     // Use Graphway to try and find a path to hit position
            //     Graphway.FindPath(transform.position, hit.point, FindPathCallback, true, debugMode);
            // }
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
			{
				// Object hit
				// Use Graphway to try and find a path to hit position
				Graphway.FindPath(transform.position, hit.point, FindPathCallback, true, debugMode);

				gameObject.GetComponent<Animator>().Play("Zoop.Run");
			}
		}
		
		// Move towards waypoints (if has waypoints)
		if (waypoints != null && waypoints.Length > 0)
		{
			// Increase speed
			speed = Mathf.Lerp(speed, MAX_SPEED, Time.deltaTime * ACCELERATION);
            speed = Mathf.Clamp(speed, 0, MAX_SPEED);

            // Look at next waypoint 
            transform.LookAt(waypoints[0].position);
			
			// Move toward next waypoint
			transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position, Time.deltaTime * waypoints[0].speed * speed);
			
            // Check if reach waypoint target
			if (Vector3.Distance(transform.position, waypoints[0].position) < 0.1f)
			{
                // Move on to next waypoint
				NextWaypoint();
			}	
		}
		else
		{
			// Reset speed
			speed = 0;
			gameObject.GetComponent<Animator>().Play("Zoop.Idle");
		}
		
		// Draw Path
		if (debugMode && waypoints != null && waypoints.Length > 0)
		{
			Vector3 startingPoint = transform.position;
			
			for (int i = 0 ; i < waypoints.Length ; i++)
			{
				Debug.DrawLine(startingPoint, waypoints[i].position, Color.green);
				
				startingPoint = waypoints[i].position;
			}
		}
	}
	
	private void NextWaypoint()
	{
		if (waypoints.Length > 1)
		{
			GwWaypoint[] newWaypoints = new GwWaypoint[waypoints.Length - 1];
			
			for (int i = 1 ; i < waypoints.Length ; i++)
			{
				newWaypoints[i-1] = waypoints[i];
			}
			
			waypoints = newWaypoints;
		}
		else
		{
			waypoints = null;
		}  
	}
	
	private void FindPathCallback(GwWaypoint[] path)
	{
        // Graphway returns 'null' if no path found
        // OR GwWaypoint array of waypoints to destination

        if (path == null)
		{
			Debug.Log("Path to target position not found!");
		}
		else
		{
			waypoints = path;
		}
	}
}
