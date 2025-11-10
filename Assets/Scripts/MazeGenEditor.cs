using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGenEditor : Editor
{
    //override the degault inspector GUI rendering
    public override void OnInspectorGUI()
    {
        //cast the target object to MazeGenerator so we can access its fields and methods

        MazeGenerator mazeGen = (MazeGenerator)target;
        GUILayout.Label("------ Maze Generator Custom Inspector ------", EditorStyles.largeLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (DrawDefaultInspector())
        {
           mazeGen.GenerateImageOfNewMaze();
            //limit size value to be between 20 and 100
            if (mazeGen.size < 10 )
            {
                mazeGen.size = 10;
            }
            if (mazeGen.size > 10000)
            {
                mazeGen.size = 10000;
            }
        }

        if (GUILayout.Button("Generate New Maze"))
        {
            mazeGen.GenerateImageOfNewMaze();
        }


    }

}

public class MazeGeneratorWindow : EditorWindow
{
  public int InitialMazeSize = 500;

    [MenuItem("Tools/Generate Maze By Size")]

    public static void ShowWindow()
    {
        GetWindow<MazeGeneratorWindow>();
    }

    private void OnGUI()
    {
        

        EditorGUILayout.Space();
        GUILayout.Label("Maze Generator will create a maze in your scene");
        EditorGUILayout.Space();
        GUILayout.Label("Configure Maze: ");

      InitialMazeSize = EditorGUILayout.IntField("Maze Size: ", InitialMazeSize);

        if (GUILayout.Button("Generate"))
        {
            GameObject newGameObject = new GameObject(InitialMazeSize+"X"+ InitialMazeSize+ "Generated Maze");
            Undo.RegisterCreatedObjectUndo(newGameObject, "Undo Create Object");

            Material mat = new Material(Shader.Find("White"));
            mat.color = Color.white;

            MazeGenerator mg = newGameObject.AddComponent<MazeGenerator>();
            
            newGameObject.GetComponent<MeshRenderer>().material = mat;

            mg.size = InitialMazeSize;
            mg.GenerateImageOfNewMaze();
        }

    }


}