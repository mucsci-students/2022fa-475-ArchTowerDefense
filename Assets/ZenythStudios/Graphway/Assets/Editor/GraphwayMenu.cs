using UnityEngine;
using UnityEditor;

public class GraphwayMenu
{
	[MenuItem("Tools/Graphway")]
	private static void CreateGraphway()
	{
		// Create Graphway object
		GameObject graphwayObject = new GameObject();
		graphwayObject.name = "Graphway";
		graphwayObject.AddComponent<Graphway>();
		
		// Create nodes parent
		GameObject nodesObject = new GameObject("Nodes");
		nodesObject.transform.parent = graphwayObject.transform;
        nodesObject.AddComponent<GraphwayObject>();

		// Create connections parent
		GameObject connectionObject = new GameObject("Connections");
		connectionObject.transform.parent = graphwayObject.transform;
        connectionObject.AddComponent<GraphwayObject>();
		
		// Register undo operation
		Undo.RegisterCreatedObjectUndo(graphwayObject, "Created Graphway");
		
		// Select new object
		Selection.activeGameObject = graphwayObject;
	}
}