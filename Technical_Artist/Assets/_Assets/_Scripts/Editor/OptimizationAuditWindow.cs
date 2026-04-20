using UnityEngine;
using UnityEditor;

public class OptimizationAuditWindow : EditorWindow
{
    [MenuItem("Tools/Optimization Audit")]
    public static void ShowWindow()
    {
        GetWindow<OptimizationAuditWindow>("Optimization Audit");
    }

    private void OnGUI()
    {
        GUILayout.Label("GPU Instancing & Batching", EditorStyles.boldLabel);
        
        EditorGUILayout.HelpBox("Automatically enables GPU Instancing on all materials to reduce draw calls for pooled and repeated objects (like the newly added foliage).", MessageType.Info);
        
        if (GUILayout.Button("Enable GPU Instancing on All Materials"))
        {
            EnableGPUInstancing();
        }

        GUILayout.Space(10);
        GUILayout.Label("Mesh Analysis", EditorStyles.boldLabel);
        
        EditorGUILayout.HelpBox("Scans all prefabs for meshes exceeding 1500 vertices to flag potential LOD (Level of Detail) requirements.", MessageType.Info);

        if (GUILayout.Button("Audit High Vertex Count Meshes"))
        {
            AuditMeshes();
        }
    }

    private void EnableGPUInstancing()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int updatedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && !mat.enableInstancing)
            {
                // Note: Unity handles failing gracefully if the shader doesn't support it,
                // but for custom Shader Graphs with Instancing checked, this turns it on per material.
                mat.enableInstancing = true;
                EditorUtility.SetDirty(mat);
                updatedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[Optimization] Enabled GPU Instancing on {updatedCount} materials.");
    }

    private void AuditMeshes()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        Debug.Log("[Optimization] Auditing Prefab Meshes for Vertex Count (> 1500 verts)");
        int highVertCount = 0;

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>(true);
                foreach (MeshFilter mf in filters)
                {
                    if (mf.sharedMesh != null && mf.sharedMesh.vertexCount > 1500)
                    {
                        Debug.LogWarning($"[LOD Warning] {prefab.name} -> {mf.name} has {mf.sharedMesh.vertexCount} vertices. Consider LODs.", prefab);
                        highVertCount++;
                    }
                }
            }
        }
        
        if (highVertCount == 0)
        {
            Debug.Log("[Optimization] All prefab meshes are well within the vertex budget.");
        }
    }
}
