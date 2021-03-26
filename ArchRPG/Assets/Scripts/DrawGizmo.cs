using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public bool show_bound_box;
    public string file_path;

    public Vector2 offset;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, file_path, true);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, transform.lossyScale);
    }
}
