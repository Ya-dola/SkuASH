using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Screenshot : EditorWindow
{
    int _resWidth = Screen.width * 4;
    int _resHeight = Screen.height * 4;

    public Camera myCamera;
    int _scale = 1;

    string _path = "";

    // bool showPreview = true;
    RenderTexture _renderTexture;

    bool _isTransparent = false;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Tools/High-Res Screenshot")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(Screenshot));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        // editorWindow.title = "Screenshot";
        editorWindow.titleContent.text = "Screenshot";
    }

    float _lastTime;


    void OnGUI()
    {
        EditorGUILayout.LabelField("Resolution", EditorStyles.boldLabel);
        _resWidth = EditorGUILayout.IntField("Width", _resWidth);
        _resHeight = EditorGUILayout.IntField("Height", _resHeight);

        EditorGUILayout.Space();

        _scale = EditorGUILayout.IntSlider("Scale", _scale, 1, 15);

        EditorGUILayout.HelpBox(
            "The default mode of screenshot is crop - " +
            "so choose a proper width and height. The scale is a factor " +
            "to multiply or enlarge the renders without loosing quality.", MessageType.None);


        EditorGUILayout.Space();


        GUILayout.Label("Save Path", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField(_path, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
            _path = EditorUtility.SaveFolderPanel("Path to Save Images", _path,
                Application.dataPath);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Choose the folder in which to save the screenshots", MessageType.None);
        EditorGUILayout.Space();


        //isTransparent = EditorGUILayout.Toggle(isTransparent,"Transparent Background");


        GUILayout.Label("Select Camera", EditorStyles.boldLabel);


        myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true, null) as Camera;


        if (myCamera == null)
        {
            myCamera = Camera.main;
        }

        _isTransparent = EditorGUILayout.Toggle("Transparent Background", _isTransparent);


        EditorGUILayout.HelpBox(
            "Choose the camera of which to capture the render. " +
            "You can make the background transparent using the transparency option.",
            MessageType.None);

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Default Options", EditorStyles.boldLabel);


        if (GUILayout.Button("Set To Screen Size"))
        {
            _resHeight = (int)Handles.GetMainGameViewSize().y;
            _resWidth = (int)Handles.GetMainGameViewSize().x;
        }


        if (GUILayout.Button("Default Size"))
        {
            _resHeight = 1440;
            _resWidth = 2560;
            _scale = 1;
        }


        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(
            "Screenshot will be taken at " + _resWidth * _scale + " x " + _resHeight * _scale + " px",
            EditorStyles.boldLabel);

        if (GUILayout.Button("Take Screenshot", GUILayout.MinHeight(60)))
        {
            if (_path == "")
            {
                _path = EditorUtility.SaveFolderPanel("Path to Save Images",
                    _path, Application.dataPath);
                Debug.Log("Path Set");
                TakeHiResShot();
            }
            else
            {
                TakeHiResShot();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Open Last Screenshot", GUILayout.MinHeight(40)))
        {
            if (lastScreenshot != "")
            {
                Application.OpenURL("file://" + lastScreenshot);
                Debug.Log("Opening File " + lastScreenshot);
            }
        }

        if (GUILayout.Button("Open Folder", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("file://" + _path);
        }

        EditorGUILayout.EndHorizontal();

        if (takeHiResShot)
        {
            int resWidthN = _resWidth * _scale;
            int resHeightN = _resHeight * _scale;
            RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
            myCamera.targetTexture = rt;

            TextureFormat tFormat;
            if (_isTransparent)
                tFormat = TextureFormat.ARGB32;
            else
                tFormat = TextureFormat.RGB24;


            Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
            myCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
            myCamera.targetTexture = null;
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidthN, resHeightN);

            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            Application.OpenURL(filename);
            takeHiResShot = false;
        }

        EditorGUILayout.HelpBox(
            "(✿◡‿◡) - Yadola",
            MessageType.None);
    }


    private bool takeHiResShot = false;
    public string lastScreenshot = "";


    public string ScreenShotName(int width, int height)
    {
        string strPath = "";

        strPath = string.Format("{0}/screen_{1}x{2}_{3:yyyy-MM-dd_HH-mm-ss}.png",
            _path,
            width, height, System.DateTime.Now);
        lastScreenshot = strPath;

        return strPath;
    }


    public void TakeHiResShot()
    {
        Debug.Log("Taking Screenshot");
        takeHiResShot = true;
    }
}