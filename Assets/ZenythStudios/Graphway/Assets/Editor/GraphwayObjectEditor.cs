using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GraphwayObject))]

public class GraphwayObjectEditor : Editor 
{   
    public override void OnInspectorGUI()
	{     
		GraphwayEditor.CreateLogo();
	}
	
	void OnSceneGUI()
    {
        GraphwayObject graphwayObject = (GraphwayObject)target;
     
		GraphwayEditor.DrawGraph(graphwayObject.transform);
    }	
    
    void OnDisable()
    {
        GraphwayObject graphwayObject = (GraphwayObject)target;
     
        if (graphwayObject != null)
        {
			GraphwayEditor.DisableRenderers(graphwayObject.transform);
		}
    }
}