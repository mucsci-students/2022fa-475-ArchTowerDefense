using UnityEngine;

public enum GraphwayConnectionTypes {
	Bidirectional, 
    UnidirectionalAToB, 
    UnidirectionalBToA
};

public class GraphwayConnection : MonoBehaviour 
{
	public int nodeIDA = -1;
    public int nodeIDB = -1;
    
	[Tooltip("Toggle connection enabled/disabled.  Disabled connections will not be used for pathfinding until enabled.")]
	public bool disabled = false;
    
    [Tooltip("How fast this path can be travelled relative to other paths.  This will be taken into account when searching for the fastest path.")]
    public float speedWeight = 1;
    
    [Tooltip("The type of connection.  Bidirectional = two way.  Unidirectional = one way only.")]
	public GraphwayConnectionTypes connectionType = GraphwayConnectionTypes.Bidirectional;
	
    public int editorSubnodeCounter = 0; // Keep public so value is saved upon editor close

    public void SetConnectionData(int nodeIDA, int nodeIDB, GraphwayConnectionTypes connectionType)
    {
        this.nodeIDA = nodeIDA;
        this.nodeIDB = nodeIDB;
        this.connectionType = connectionType;
    }
}
