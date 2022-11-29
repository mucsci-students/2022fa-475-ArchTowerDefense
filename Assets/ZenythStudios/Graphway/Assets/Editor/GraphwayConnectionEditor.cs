using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(GraphwayConnection))]
public class GraphwayConnectionEditor : Editor 
{
	private SerializedProperty disabled;
	private SerializedProperty connectionType;
	private SerializedProperty speedWeight;
	
    public bool subnodePlacementEnabled = false;

	private void OnEnable()
	{
        // Assign properties
		disabled = serializedObject.FindProperty("disabled");	
		connectionType = serializedObject.FindProperty("connectionType");	
		speedWeight = serializedObject.FindProperty("speedWeight");	
	}

    public override void OnInspectorGUI()
    {
        GraphwayEditor.CreateLogo();
  
        // Add property edit inputs
        serializedObject.Update();
		
        EditorGUILayout.PropertyField(disabled);
        EditorGUILayout.PropertyField(connectionType);
        EditorGUILayout.PropertyField(speedWeight);
        
        serializedObject.ApplyModifiedProperties();
  
        EditorGUILayout.Space();
        
        // Create button to enable/disable subnode placement
        if (subnodePlacementEnabled)
        {
            EditorGUILayout.HelpBox("Select the Disable Subnode Placement button below to end subnode placement.  Subnode placement always goes from lowest to highest ID regardless of the connection type.", MessageType.Info);
            
            if (GUILayout.Button("Disable Subnode Placement"))
            {
                subnodePlacementEnabled = false;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select the Enable Subnode Placement button below to begin placing subnodes.  Subnode placement always goes from lowest to highest ID regardless of the connection type.", MessageType.Info);
            
            if (GUILayout.Button("Enable Subnode Placement"))
            {
                subnodePlacementEnabled = true;
            }
        }
        
        EditorGUILayout.Space();
    }

    public void OnSceneGUI ()
    {
        GraphwayConnection graphwayConnection = (GraphwayConnection)target;

        if (subnodePlacementEnabled)
        {
            // Disable left click selection on other game objects
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            // Place subnodes
            // NOTE - objects must have colliders attached!
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast (worldRay, out hit))
                {
                    // Create subnode at position
                    CreateSubnodeObject(hit.point);

                    // Mark scene as dirty to trigger 'Save Changes' prompt
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }

                // Use up event
                Event.current.Use();
            }
        }
        else
        {
            // Re-enable left click selection on other game objects
            HandleUtility.Repaint();
        }

        // Update graph
        GraphwayEditor.DrawGraph(graphwayConnection.transform);
    }
    
    public void OnDisable()
    {
        GraphwayConnection graphwayConnection = (GraphwayConnection)target;
     
        if (graphwayConnection != null)
        {
			GraphwayEditor.DisableRenderers(graphwayConnection.transform);
		}
    } 
    
    private void CreateSubnodeObject(Vector3 subnodePosition)
    {
        // Create new subnode object
        GraphwayConnection graphwayConnection = (GraphwayConnection)target;
        
        GameObject subnodeObject = Instantiate(Resources.Load("Prefabs/GraphwaySubnode")) as GameObject;
        subnodeObject.name = graphwayConnection.editorSubnodeCounter.ToString();
        subnodeObject.transform.position = subnodePosition;
        subnodeObject.transform.parent = graphwayConnection.transform;
        subnodeObject.AddComponent<GraphwayObject>();

		// Register undo operation
		Undo.RegisterCreatedObjectUndo(subnodeObject, "Created Subnode");
		
        graphwayConnection.editorSubnodeCounter++;
    }
}