using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Spline))]
public class BezierEditor : Editor
{
    public VisualTreeAsset inspectorXML;

    private VisualElement inspector;
    private Spline spline;
    private Tool currentTool;
    private SerializedProperty propHandlesSize;

    private void OnEnable()
    {
        propHandlesSize = serializedObject.FindProperty("handlesSize");
        currentTool = Tools.current;
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        Tools.current = currentTool;
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        if (inspector == null)
            return;

        Button addButton = inspector.Q<Button>("AddBezierButton");
        Button removeButton = inspector.Q<Button>("RemoveBezierButton");

        addButton.visible = spline.editable;
        removeButton.visible = spline.editable;
    }

    public override VisualElement CreateInspectorGUI()
    {
        inspector = new VisualElement();

        inspectorXML.CloneTree(inspector);

        spline = target as Spline;

        Button addButton = inspector.Q<Button>("AddBezierButton");
        Button removeButton = inspector.Q<Button>("RemoveBezierButton");

        addButton.visible = spline.editable;
        removeButton.visible = spline.editable;
        addButton.clicked += () => AddBezier();
        removeButton.clicked += () => RemoveBezier();

        return inspector;
    }

    public void OnSceneGUI()
    {
        if (inspector == null)
            return;

        List<ControlPoint> controlPoints = spline.controlPoints;

        if (controlPoints.Count < 2)
        {
            InitializeBezier();
        }

        BezierHandles();

        if (!spline.editable)
            return;

        UpdateCurrentTool();
    }

    private void InitializeBezier()
    {
        spline.controlPoints.Clear();
        Transform transform = spline.transform;
        ControlPoint start = new(transform, transform.position - transform.forward, transform.rotation);
        ControlPoint end = new(transform, transform.position + transform.forward, transform.rotation);
        spline.AddBezier(start, end);
    }

    private void AddBezier()
    {
        ControlPoint start = spline.controlPoints[^1];
        ControlPoint end = new(spline.transform, start.Position + start.Forward, start.Rotation, start.Scale);

        Undo.RecordObject(spline, "Add bezier");
        spline.AddBezier(start, end);

        EditorWindow.GetWindow<SceneView>().Repaint();
    }

    private void RemoveBezier()
    {
        if (spline.controlPoints.Count == 2)
            return;

        Undo.RecordObject(spline, "Remove bezier");
        spline.RemoveBezier();

        EditorWindow.GetWindow<SceneView>().Repaint();
    }

    #region Handles
    private void UpdateCurrentTool()
    {
        Tool current = Tools.current;

        if (current != Tool.None)
        {
            Tools.current = Tool.None;
            currentTool = current;
        }
    }

    private void BezierHandles()
    {
        List<ControlPoint> controlPoints = spline.controlPoints;

        for (int i = 0; i < controlPoints.Count; i++)
        {
            ControlPoint point = controlPoints[i];

            if (spline.editable)
            {
                Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                ControlPointChange change = DrawControlHandle(point);

                if (PointWasChanged(point, change))
                {
                    Undo.RecordObject(spline, "Change bezier point");
                    UpdatePoint(point, change);
                }
            }

            if (i < controlPoints.Count - 1)
            {
                ControlPoint end = controlPoints[i + 1];
                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                Handles.DrawBezier(point.Position, end.Position, spline.GetStartTangentPoint(point), spline.GetEndTangentPoint(end), Color.white, Texture2D.whiteTexture, 2 * propHandlesSize.floatValue);
            }
        }
    }

    struct ControlPointChange
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public ControlPointChange(ControlPoint p)
        {
            position = p.Position;
            rotation = p.Rotation;
            scale = p.Scale;
        }
    }

    private bool PointWasChanged(ControlPoint p, ControlPointChange change)
    {
        return p.Position != change.position || p.Rotation != change.rotation || p.Scale != change.scale;
    }

    private void UpdatePoint(ControlPoint p, ControlPointChange change)
    {
        p.Position = change.position;
        p.Rotation = change.rotation;
        p.Scale = change.scale;
    }

    private ControlPointChange DrawControlHandle(ControlPoint p)
    {
        float handlesSize = propHandlesSize.floatValue;

        Matrix4x4 matrix = Handles.matrix;
        Handles.matrix = Matrix4x4.Scale(Vector3.one * handlesSize) * matrix;

        ControlPointChange change = new(p);

        // convert these to local space
        if (currentTool == Tool.Move)
        {
            change.position = Handles.PositionHandle(p.Position / handlesSize, p.Rotation) * handlesSize;
        }
        else if (currentTool == Tool.Rotate)
        {
            change.rotation = Handles.RotationHandle(p.Rotation, p.Position / handlesSize);
        }
        else if (currentTool == Tool.Scale)
        {
            change.scale = Handles.ScaleHandle(p.Scale, p.Position / handlesSize, p.Rotation);
        }

        Handles.matrix = matrix;

        return change;
    }
    #endregion
}