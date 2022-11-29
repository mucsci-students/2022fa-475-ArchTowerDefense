using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(GraphwayNode))]
[CanEditMultipleObjects]
public class GraphwayNodeEditor : Editor 
{
	void OnDisable()
    {
        // Hide Graph
        GraphwayNode graphwayNodeData = (GraphwayNode)target;
     
        if (graphwayNodeData != null)
        {
			GraphwayEditor.DisableRenderers(graphwayNodeData.transform);
		}
    }
	
	void OnGUI()
	{
		if (Event.current.type == EventType.ValidateCommand)
		{
		    switch (Event.current.commandName)
		    {
		         case "UndoRedoPerformed":
		            this.Repaint();
		            break;
		    }
		}
	}
	
	public override void OnInspectorGUI()
	{
		GraphwayEditor.CreateLogo();
		
		// Check if two nodes are selected that can be linked
		GameObject[] selectedObjects = Selection.gameObjects;

		bool allHaveNodesScript = true;

		foreach (GameObject selectedObject in selectedObjects) 
		{
			if ( ! selectedObject.GetComponent<GraphwayNode>()) 
			{
				allHaveNodesScript = false;
			}
		}

		// Check if nodes are already linked
		bool selectedObjectsLinked = false;

		if (allHaveNodesScript && selectedObjects.Length == 2)
		{
            GraphwayNode graphwayNodeA = selectedObjects[0].GetComponent<GraphwayNode>();
            GraphwayNode graphwayNodeB = selectedObjects[1].GetComponent<GraphwayNode>();

            if (NodesAreConnected(graphwayNodeA.nodeID, graphwayNodeB.nodeID) || NodesAreConnected(graphwayNodeB.nodeID, graphwayNodeA.nodeID))
			{
				selectedObjectsLinked = true;
			}
		}

		// Show button to link / unlink nodes
		if (selectedObjects.Length != 2)
		{
			EditorGUILayout.HelpBox("Select two nodes to connect or disconnect them.", MessageType.Info);
		}
		
		EditorGUI.BeginDisabledGroup(allHaveNodesScript == false || selectedObjects.Length != 2);

		if (selectedObjectsLinked)
		{
			if (GUILayout.Button("Disconnect Nodes"))
			{
				DisconnectSelectedNodes();
			}
		}
		else
		{
			if (GUILayout.Button("Connect Nodes (Bidirectional)"))
			{
				ConnectSelectedNodes(GraphwayConnectionTypes.Bidirectional);
			}
			else if (GUILayout.Button("Connect Nodes (Unidirectional A To B)"))
			{
				ConnectSelectedNodes(GraphwayConnectionTypes.UnidirectionalAToB);
			}
			else if (GUILayout.Button("Connect Nodes (Unidirectional B To A)"))
			{
				ConnectSelectedNodes(GraphwayConnectionTypes.UnidirectionalBToA);
			}
		}

		EditorGUI.EndDisabledGroup();
		
		EditorGUILayout.Space();
	}
	
	void OnSceneGUI()
	{
        // Allow nodes to be linked/unlinked using the 'C' key
        // Nodes will only be linked Bidirectionally this way
		Event e = Event.current;
		
		switch (e.type)
		{
			case EventType.KeyDown:
			{
				if (Event.current.keyCode == KeyCode.C)
				{
					// Check if two nodes are selected that can be linked
					GameObject[] selectedObjects = Selection.gameObjects;
			
					bool allHaveNodesScript = true;
			
					foreach (GameObject selectedObject in selectedObjects) 
					{
						if ( ! selectedObject.GetComponent<GraphwayNode>()) 
						{
							allHaveNodesScript = false;
						}
					}

					// Check if nodes are already linked
					bool selectedObjectsLinked = false;
			
					if (allHaveNodesScript && selectedObjects.Length == 2)
					{
			            GraphwayNode graphwayNodeA = selectedObjects[0].GetComponent<GraphwayNode>();
			            GraphwayNode graphwayNodeB = selectedObjects[1].GetComponent<GraphwayNode>();
			
			            if (NodesAreConnected(graphwayNodeA.nodeID, graphwayNodeB.nodeID) || NodesAreConnected(graphwayNodeB.nodeID, graphwayNodeA.nodeID))
						{
							selectedObjectsLinked = true;
						}
						
						if (selectedObjectsLinked)
						{
							DisconnectSelectedNodes();
						}
						else
						{
							ConnectSelectedNodes(GraphwayConnectionTypes.Bidirectional);
						}
						
                        // Use up event
						Event.current.Use();
						
						this.Repaint();
					}
				}
			}
			break;
		}
		
		// Redraw graph
		GraphwayNode graphwayNodeData = (GraphwayNode)target;
     
		GraphwayEditor.DrawGraph(graphwayNodeData.transform);
	}

    private void ConnectSelectedNodes(GraphwayConnectionTypes connectionType)
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        
        // Get node IDs of connected nodes
        GraphwayNode graphwayNodeA = selectedObjects[0].GetComponent<GraphwayNode>();
        GraphwayNode graphwayNodeB = selectedObjects[1].GetComponent<GraphwayNode>();

		int nodeIDA = graphwayNodeA.nodeID;
		int nodeIDB = graphwayNodeB.nodeID;

		// NOTE - Nodes are connected by smallest node ID to largest node ID
		// Create connection
		if (nodeIDA < nodeIDB)
		{
			CreateConnectedNodeObj(nodeIDA, nodeIDB, connectionType);
		}
		else {
			CreateConnectedNodeObj(nodeIDB, nodeIDA, connectionType);
		}

		// Mark scene as dirty to trigger 'Save Changes' prompt
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
    private void DisconnectSelectedNodes()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        
        // Get node IDs of connected nodes
        GraphwayNode graphwayNodeA = selectedObjects[0].GetComponent<GraphwayNode>();
        GraphwayNode graphwayNodeB = selectedObjects[1].GetComponent<GraphwayNode>();

		int nodeIDA = graphwayNodeA.nodeID;
		int nodeIDB = graphwayNodeB.nodeID;

		// NOTE - Nodes are connected by smallest node ID to largest node ID
		// Break connection
		if (nodeIDA < nodeIDB)
		{
			RemoveConnectedNodeObj(nodeIDA, nodeIDB);
		}
		else {
            RemoveConnectedNodeObj(nodeIDB, nodeIDA);
		}

		// Mark scene as dirty to trigger 'Save Changes' prompt
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
	private void CreateConnectedNodeObj(int nodeIDA, int nodeIDB, GraphwayConnectionTypes connectionType)
	{
		if ( ! NodesAreConnected(nodeIDA, nodeIDB))
		{
			GraphwayNode graphwayNodeData = (GraphwayNode)target;
			
			Graphway graphway = GraphwayEditor.FindGraphwayParent(graphwayNodeData.transform);
			
			GameObject newConnection = new GameObject();

			newConnection.name = nodeIDA.ToString() + "->" + nodeIDB.ToString();
			newConnection.transform.parent = graphway.transform.Find("Connections").transform;
            newConnection.AddComponent<GraphwayConnection>().SetConnectionData(nodeIDA, nodeIDB, connectionType);
            
            // Register undo operation
			Undo.RegisterCreatedObjectUndo(newConnection, "Graphway Connection");
			
            // Reorder connection hierarchy to keep things tidy
			ReorderConnections();
		}
	}

	private void RemoveConnectedNodeObj(int nodeIDA, int nodeIDB)
	{
		if (NodesAreConnected(nodeIDA, nodeIDB))
		{   
			GraphwayNode graphwayNodeData = (GraphwayNode)target;
			
			Graphway graphway = GraphwayEditor.FindGraphwayParent(graphwayNodeData.transform);
			
			DestroyImmediate(graphway.transform.Find("Connections/" + nodeIDA.ToString() + "->" + nodeIDB.ToString()).gameObject);
		}
	}

	private bool NodesAreConnected(int nodeIDA, int nodeIDB)
	{
		GraphwayNode graphwayNodeData = (GraphwayNode)target;
			
		Graphway graphway = GraphwayEditor.FindGraphwayParent(graphwayNodeData.transform);
		
		return graphway.transform.Find("Connections/" + nodeIDA.ToString() + "->" + nodeIDB.ToString());
	}

	private void ReorderConnections()
	{
		// Rearrange connections into numerical order to make them easier to find and edit
		GraphwayNode graphwayNodeData = (GraphwayNode)target;
		
		Graphway graphway = GraphwayEditor.FindGraphwayParent(graphwayNodeData.transform);
		
		Transform connections = graphway.transform.Find("Connections").transform;
		
		// Create list of connections
		List<string> connectionNames = new List<string>();
		
		foreach (Transform connection in connections)
		{
			connectionNames.Add(connection.name);
		}
		
		// Sort list
		connectionNames.Sort();
		
		// Reorder gameobjects
		for (int i = 0 ; i < connectionNames.Count ; i++)
		{
			string connectionName = connectionNames[i];
			
			connections.Find(connectionName).SetSiblingIndex(i);
		}
	}
}