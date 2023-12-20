using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Assertions;
using GenericChara;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AddClass
{

    public class GenericFunctions : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                DontDestroyOnLoad(gameObject); // 追加
            }
            else
                Destroy(gameObject);
        }
    }
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy(this);
            return false;
        }
    }

    public static class AddFunction
    {
        public static void ChangeDoubleNumber(ref float v1, ref float v2)
        {
            float tmp = v1;
            v1 = v2;
            v2 = tmp;
        }
        public static List<float> SortInDescending(List<float> list)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                for(int j = i + 1; j < list.Count; ++j)
                {
                    if (list[i] < list[j])
                    {
                        (list[i], list[j]) = (list[j], list[i]);    // Turpleで値を入れ替える
                    }
                }
            }

            return list;
        }




        /// <summary>
        /// angleを指定した角度に丸める
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetNormalizedAngle(float angle, float min, float max)
        {
            return Mathf.Repeat(angle - min, max - min) + min;
        }

        public static Vector3 GetNormalizedAngles(Vector3 angles, float min, float max)
        {
            Vector3 retVec3;
            retVec3.x = GetNormalizedAngle(angles.x, min, max);
            retVec3.y = GetNormalizedAngle(angles.y, min, max);
            retVec3.z = GetNormalizedAngle(angles.z, min, max);

            return retVec3;
        }

        /// <summary>
        /// Vector2を角度(360度)に変更
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Vec2ToAngle(Vector2 v)
        {
            return Mathf.Repeat(Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg, 360);
        }

        public static Vector2 DegToVec(float deg)
        {
            Vector2 vec;
            vec.x = MathF.Cos(deg);
            vec.y = MathF.Sin(deg);
            return vec;
        }

        public static float GetAngleByVec3(Vector3 start, Vector3 target)
        {
            float angle;
            Vector3 dt = start - target;
            angle = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

            return angle;
        }
        public static float GetAngleByVec2(Vector2 start, Vector2 target)
        {
            float angle;
            Vector3 dt = start - target;
            angle = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

            return angle;
        }

        /// <summary>
        /// Vector3で各角度を返す
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 Vector3AngleSet(Vector3 v1, Vector3 v2)
        {
            Vector3 angle;

            Vector2 start;
            Vector2 end;

            // xを求める
            start.x = v1.y;
            start.y = v2.z;

            end.x = v1.y;
            end.y = v2.z;

            angle.x = Vector3.Angle(start, end);

            // yを求める
            start.x = v1.x;
            start.y = v2.z;

            end.x = v1.x;
            end.y = v2.z;

            angle.y = Vector3.Angle(start, end);

            // zを求める
            start = v1;
            end = v2;

            angle.z = Vector3.Angle(start, end);

            return angle;
        }
        public static Vector3 Vector3AngleSet(Transform t1, Transform t2)
        {
            Vector3 angle;


            Vector3 v1;
            Vector3 v2;

            // xを求める
            v1 = t1.up - t2.up;
            v2 = t1.forward - t2.forward;

            angle.x = Vector3.Angle(v1, v2);

            // yを求める
            v1 = t1.right - t2.right;
            v2 = t1.forward - t2.forward;

            angle.y = Vector3.Angle(v1, v2);

            // zを求める
            v1 = t1.up - t2.up;
            v2 = t1.right - t2.right;

            angle.z = Vector3.Angle(v1, v2);

            return angle;
        }


        public static Vector3 VecTFloatConvert(VecT<float> vec)
        {
            Vector3 newVec;
            newVec.x = vec.x;
            newVec.y = vec.y;
            newVec.z = vec.z;

            return newVec;
        }
        public static VecT<float> VecTFloatConvert(VecT<float> vec, Vector3 vec3)
        {
            vec.x = vec3.x;
            vec.y = vec3.y;
            vec.z = vec3.z;

            return vec;
        }

        public static Vector3 IndexToDirrection(int index, Transform tra)
        {
            switch (index % 3)
            {
                case 0: return tra.right;
                case 1: return tra.up;
                case 2: return tra.forward;
            }

            Debug.Log("Indexが違います");
            return Vector3.zero;
        }

        public static float IndexToVec3(int index, Vector3 vec3)
        {
            switch(index % 3)
            {
                case 0: return vec3.x;
                case 1: return vec3.y;
                case 2: return vec3.z;
            }

            Debug.Log("Indexが違います");
            return 0.0f;
        }


        public static T IndexToVec3<T>(int index, VecT<T> vec3)
        {
            switch (index)
            {
                case 0: return vec3.x;
                case 1: return vec3.y;
                case 2: return vec3.z;
            }

            Debug.Log("Indexが違います");
            return default;
        }

        public static Vector3 CameraToMouse()
        {
            return new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0.0f);
        }

        /// <summary>
        /// 横軸x、奥行yのVector2を返す
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 GetFPSMoveVec2(Camera cam, Vector2 input)
        {
            Vector3 cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
            Vector3 movePos = cameraForward * input.y + cam.transform.right * input.x;

            return new Vector2(movePos.x, movePos.z);
        }
        public static bool IsEven(int value)
        {
            if (value / 2 == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// タグ名を要素（iなど）にして返す
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static int TagToArray(string tag)
        {
            switch (tag)
            {
                case "Player01":
                    return 0;
                case "Player02":
                    return 1;
            }
            return -1;
        }

        /// <summary>
        /// 要素（iなど）をタグ名にして返す
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayToTag(int array)
        {
            switch (array)
            {
                case 0:
                    return "Player01";
                case 1:
                    return "Player02";
            }
            return "0";
        }

        /// <summary>
        /// Rectの隣
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Neighbor(Rect rect)
        {
            return rect.x + rect.width;
        }
        public static float HorizontalityNeighbor(RectNeo rect)
        {
            return rect.x + rect.width;
        }

        public static float VerticalityNeighbor(RectNeo rect)
        {
            return rect.y + rect.height;
        }

        /// <summary>
        /// Animationの長さを返す
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public static float GetAnimationClipLength(Animator animator, string clipName)
        {
            return Get(animator.runtimeAnimatorController.animationClips, clipName);

            float Get(IEnumerable<AnimationClip> animationClips, string clipName)
            {
                return (from animationClip in animationClips
                        where animationClip.name == clipName
                        select animationClip.length).FirstOrDefault();
            }
        }
        
        public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
        {
            source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

            switch (allign)
            {
                case (AnchorPresets.TopLeft):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.TopCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 1);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.TopRight):
                    {
                        source.anchorMin = new Vector2(1, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.MiddleLeft):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(0, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0.5f);
                        source.anchorMax = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (AnchorPresets.MiddleRight):
                    {
                        source.anchorMin = new Vector2(1, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }

                case (AnchorPresets.BottomLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 0);
                        break;
                    }
                case (AnchorPresets.BottonCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 0);
                        break;
                    }
                case (AnchorPresets.BottomRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.HorStretchTop):
                    {
                        source.anchorMin = new Vector2(0, 1);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
                case (AnchorPresets.HorStretchMiddle):
                    {
                        source.anchorMin = new Vector2(0, 0.5f);
                        source.anchorMax = new Vector2(1, 0.5f);
                        break;
                    }
                case (AnchorPresets.HorStretchBottom):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 0);
                        break;
                    }

                case (AnchorPresets.VertStretchLeft):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(0, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchCenter):
                    {
                        source.anchorMin = new Vector2(0.5f, 0);
                        source.anchorMax = new Vector2(0.5f, 1);
                        break;
                    }
                case (AnchorPresets.VertStretchRight):
                    {
                        source.anchorMin = new Vector2(1, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }

                case (AnchorPresets.StretchAll):
                    {
                        source.anchorMin = new Vector2(0, 0);
                        source.anchorMax = new Vector2(1, 1);
                        break;
                    }
            }
        }

        public static void SetPivot(this RectTransform source, PivotPresets preset)
        {

            switch (preset)
            {
                case (PivotPresets.TopLeft):
                    {
                        source.pivot = new Vector2(0, 1);
                        break;
                    }
                case (PivotPresets.TopCenter):
                    {
                        source.pivot = new Vector2(0.5f, 1);
                        break;
                    }
                case (PivotPresets.TopRight):
                    {
                        source.pivot = new Vector2(1, 1);
                        break;
                    }

                case (PivotPresets.MiddleLeft):
                    {
                        source.pivot = new Vector2(0, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0.5f);
                        break;
                    }
                case (PivotPresets.MiddleRight):
                    {
                        source.pivot = new Vector2(1, 0.5f);
                        break;
                    }

                case (PivotPresets.BottomLeft):
                    {
                        source.pivot = new Vector2(0, 0);
                        break;
                    }
                case (PivotPresets.BottomCenter):
                    {
                        source.pivot = new Vector2(0.5f, 0);
                        break;
                    }
                case (PivotPresets.BottomRight):
                    {
                        source.pivot = new Vector2(1, 0);
                        break;
                    }
            }
        }
        /// <summary>
        /// CanvasのRender Mode が Scene Space - Overlay の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position)
        {
            return position.ToScreenPositionCaseScreenSpaceOverlay(Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Overlay の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="camera">メインカメラ</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceOverlay(this Vector3 position, Camera camera)
        {
            return RectTransformUtility.WorldToScreenPoint(camera, position);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        /// <param name="uiCamera">UIを写すカメラ（CanvasのRenderCameraに設定されているカメラ）</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera)
        {
            return position.ToScreenPositionCaseScreenSpaceCamera(canvas, uiCamera, Camera.main);
        }

        /// <summary>
        /// CanvasのRender Mode が Scene Space - Camera の場合に、ワールド座標をスクリーン座標に変換する
        /// </summary>
        /// <returns>変換されたスクリーン座標</returns>
        /// <param name="position">対象のワールド座標</param>
        /// <param name="canvas">UIのCanvas</param>
        /// <param name="uiCamera">UIを写すカメラ（CanvasのRenderCameraに設定されているカメラ）</param>
        /// <param name="worldCamera">メインカメラ</param>
        public static Vector2 ToScreenPositionCaseScreenSpaceCamera(this Vector3 position, Canvas canvas, Camera uiCamera, Camera worldCamera)
        {
            Assert.IsTrue(
                canvas.renderMode == RenderMode.ScreenSpaceCamera,
                "Canvasのレンダーモードが「Scene Space - Camera」になっていません"
            );

            var p = RectTransformUtility.WorldToScreenPoint(worldCamera, position);
            var retPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                p,
                uiCamera,
                out retPosition
            );
            return retPosition;
        }
        public static List<string> SearchTypes<T>()
        {
            List<string> typeNames = new List<string>();

            // このスクリプトのアセンブリを取得
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // このスクリプトのアセンブリから型を取得
            Type[] types = assembly.GetTypes();

            // ジェネリック型と一致する型を探す
            foreach (Type type in types)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(T))
                {
                    // ジェネリック型の名前をリストに追加
                    typeNames.Add(type.FullName);
                }
            }

            return typeNames;
        }
    }

    /// <summary>
    /// 指定した型を検索し、Listにして返す関数
    /// </summary>
    public class TypeFinder : MonoBehaviour
    {
        [field: SerializeField] public FieldInfo[] fields { get; private set; }
        public List<T> GetAndInList<T>(Type type)
        {
            fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<T> variables = new List<T>();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(T))
                {
                    T variable = (T)field.GetValue(GetComponent(type));
                    variables.Add(variable);

                }
            }

            return variables;
        }
    }

    [Serializable] public class TransformOffset
    {
        [SerializeField] private Vector3 vec3Offset;
        [SerializeField] private Vector2 vec2Offset;

        public void Update(Transform transform)
        {
            transform.position += vec3Offset; 
            transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);

        }
        public void Update(GameObject obj)
        {
            obj.transform.position += vec3Offset;
            obj.transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);
        }
    }

    public enum ExistState
    {
        Disable,
        Start,
        Enable,
        Ending,
    }

    [Serializable]
    public class Exist
    {
        [field: SerializeField, NonEditable] public ExistState state { get; private set; } = ExistState.Disable;
        [field: SerializeField] public Action initialize { get; set; }
        [field: SerializeField] public Action disable { get; set; }
        [field: SerializeField] public Action start { get; set; }
        [field: SerializeField] public Action enable { get; set; }
        [field: SerializeField] public Action toEnd { get; set; }
        [field: SerializeField] public Action ending { get; set; }

        [field: SerializeField, NonEditable] public bool oneShot { get; private set; }

        public void Initialize(bool started = false)
        {
            initialize?.Invoke();
            this.oneShot = started;
            state = ExistState.Disable;
        }

        public void Reset()
        {
            state = ExistState.Disable;
        }

        public void Update()
        {
            switch (state)
            {
                case ExistState.Disable:
                    disable?.Invoke();
                    break;

                case ExistState.Start:
                    state = ExistState.Enable;
                    start?.Invoke();
                    break;

                case ExistState.Enable:
                    enable?.Invoke();
                    break;
                case ExistState.Ending:

                    break;
            }
        }

        public void Stop()
        {
            state = ExistState.Disable;
        }

        public void Start()
        {
            state = ExistState.Start;
        }

        /// <summary>
        /// 一度のみ
        /// </summary>
        public void StartOneShot()
        {
            if (oneShot == false)
            {
                state = ExistState.Start;
                oneShot = true;
            }
        }
        public void OneShotReset()
        {
            oneShot = false;
        }
        public void Finish()
        {
            state = ExistState.Ending;
            toEnd?.Invoke();
        }
    }


    /// <summary>
    /// 移動範囲を円形に制限
    /// </summary>
    [Serializable]
    public class CircleClamp
    {

        [SerializeField] private GameObject center;
        [field: SerializeField] public GameObject moveObject { get; set; }
        [field: SerializeField] public float radius { get; private set; }
        [field: SerializeField, NonEditable] public float currentDistance { get; private set; }
        public void Initialize(GameObject center, GameObject moveObject)
        {
            this.center = center;
            this.moveObject = moveObject;
        }
        public void AdjustByCenter()
        {
            moveObject.transform.position = center.transform.position;
        }
        public void Limit()
        {
            currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);
            
            if (currentDistance > radius)
            {
                Debug.Log("Limitting");

                Vector3 nor = moveObject.transform.position - center.transform.position;
                moveObject.transform.position = center.transform.position + nor.normalized * radius; 
                currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);

            }

        }
    }

    [Serializable] public class MoveCircleSurface
    {
        [field: SerializeField] public Transform centerPos { get; set; }
        [field: SerializeField] public Transform moveObject { get; set; }
        [field: SerializeField] bool lookAtCenter { get; set; } // centerを向くか
        [field: SerializeField] public Vector3 axis { get; set; }   // transform.rightなどで代入する
        [SerializeField] private float distanceFromCenter;
        [SerializeField, NonEditable] private Vector3 norAxis;
        [SerializeField, NonEditable] private Quaternion angleAxis;
        [SerializeField] private float speed;
        [field: SerializeField, NonEditable] public Vector3 angleFromCenter { get; private set; }
        public void Initialize(GameObject moveObject)
        {
            this.moveObject = moveObject.transform;
        }
        public void Initialize(Transform moveObject)
        {
            this.moveObject = moveObject;
        }

        public void SetDistance(Vector3 axis)
        {
            moveObject.position = centerPos.position + (axis * distanceFromCenter);
        }

        /// <summary>
        /// 引数は回転スピード
        /// </summary>
        /// <param name="speed"></param>
        public void Update(float speed )
        {
            if (speed == 0.0f) { return; }
            this.speed = speed;

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            moveObject.position -= centerPos.position;
            moveObject.position = angleAxis * moveObject.position;
            moveObject.position += centerPos.position;

            if(lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

        }
        public Vector3 NewPosUpdate(float speed)
        {
            if (speed == 0.0f) { return Vector3.zero; }
            this.speed = speed;

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            Vector3 newPos;

            newPos = moveObject.position - centerPos.position;
            newPos = angleAxis * newPos;
            newPos += centerPos.position;

            if (lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

            return newPos;
        }
        public void Update()
        {
            if (this.speed == 0.0f) { return; }

            norAxis = axis.normalized;
            angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

            moveObject.position -= centerPos.position;
            moveObject.position = angleAxis * moveObject.position;
            moveObject.position += centerPos.position;

            if (lookAtCenter == true)
            {
                moveObject.rotation = moveObject.rotation * angleAxis;
            }

            angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);
        }


    }

    #region クラスの実行タイプ
    /// <summary>
    /// boolで判断し、trueの場合にメインの処理を行う
    /// </summary>
    [Serializable] public class Traffic
    {
        public bool active;
        public Action launchAction { get; set; }
        public Action enableAction { get; set; }
        public Action disableAction { get; set; }
        public Action closeAction { get; set; }
        public void Initialize()
        {
            active = false;
            enableAction = null;
            disableAction = null;
        }
        public void Update()
        {
            if(active == true)
            {
                enableAction?.Invoke();
            }
            else
            {
                disableAction?.Invoke();
            }
        }

        public void Launch()
        {
            active = true;
            launchAction?.Invoke();
        }
        public void Close()
        {
            active = false;
            closeAction?.Invoke();
        }

    }

    /// <summary>
    /// 間隔制御クラス
    /// </summary>
    [Serializable]
    public class Interval
    {
        public enum IncreseType
        {
            DeltaTime,
            Frame,
            Manual,
        }
        [field: SerializeField, NonEditable] public bool active { get; private set; }
        [field: SerializeField] public float interval { get; private set; }
        [field: SerializeField, NonEditable] public VariedTime time { get; private set; } = new VariedTime();
        private bool autoReset;
        private bool reached;
        public Action reachAction { get; set; }
        public Action activeAction { get; set; }
        public Action lowAction { get; set; }

        /// <summary>
        /// 引数:<br/>
        /// ・最初からactiveにする(interval値とvalueを同じにする)か<br/>
        /// ・valueがinterval値に到達したら0に戻るか<br/>
        /// ・最初のinterval値
        /// </summary>
        /// <param name="start"></param>
        public void Initialize(bool start, bool autoReset = true, float interval = 0.0f)
        {
            if (interval != 0.0f) { this.interval = interval; }
            this.autoReset = autoReset;
            if (start == true)
            {
                time.Initialize(interval);
            }
            else
            {
                time.Initialize();
            }

            active = (time.value >= interval) ? true : false;
            reached = false;
        }


        public void Update(float manualValue = 0.0f)
        {
            time.Update(manualValue);

            if (time.value >= interval)
            {
                if (reached == false)
                {
                    reached = true;
                    reachAction?.Invoke();
                }

                active = true;
                activeAction?.Invoke();
                if (autoReset == true) { Reset(); }
            }
            else
            {
                active = false;
                lowAction?.Invoke();
            }
        }

        /// <summary>
        /// timeを0に戻す
        /// </summary>
        public void Reset()
        {
            reached = false;
            time.Initialize();
        }

        public float ratio
        {
            get { return time.value / interval; }
        }
    }

    /// <summary>
    /// 範囲毎にActionを実行する
    /// </summary>
    [Serializable]
    public class ThresholdRatio
    {
        [field: SerializeField, NonEditable] public bool reaching { get; private set; }
        [field: SerializeField, NonEditable] public bool beforeBool { get; private set; }

        [SerializeField] private Vector2 thresholdRange;
        [SerializeField, NonEditable] private float currentValue;
        [SerializeField, NonEditable] private Vector2 beforeRange;
        public MomentAction withinRangeAction { get; set; } = new MomentAction();   // 範囲内に入る時に一度行われる
        public Action inRangeAction { get; set; }                                   // 範囲内に入っている間に行われる
        public MomentAction exitRangeAction { get; set; } = new MomentAction();     // 範囲外に出る時に一度行われる
        public Action outOfRangeAction { get; set; }                                // 範囲外に出ている間に行われる

        public void Initialize(float min, float max)
        {
            thresholdRange = new Vector2(min, max);
            Reset();
        }
        public void Initialize(Vector2 range = default)
        {
            if (range != default) { thresholdRange = range; }
            Reset();
        }

        public void Reset()
        {
            beforeBool = false;
            beforeRange = Vector2.zero;
            withinRangeAction.Initialize();
            exitRangeAction.Initialize();
        }

        /// <summary>
        /// 引数:現在の割合
        /// </summary>
        /// <param name="value"></param>
        public void Update(float value)
        {
            currentValue = value;

            // 範囲内なら
            if (thresholdRange.x <= currentValue && currentValue <= thresholdRange.y) { reaching = true; }
            else { reaching = false; }


            if (reaching == true)        // 範囲内で
            {
                if (beforeBool == false)    // 入った瞬間なら
                {
                    withinRangeAction.Enable();
                }

                inRangeAction?.Invoke();

            }

            if (beforeBool == true)      // 前回範囲内で
            {
                if (reaching == false)  // 出る瞬間なら
                {
                    exitRangeAction.Enable();
                }
            }

            if (reaching == false)   // 範囲外なら
            {
                outOfRangeAction?.Invoke();
            }

            beforeBool = reaching;
            beforeRange = thresholdRange;
        }

    }
    #endregion

    [Serializable]
    public class SmoothRotate
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject targetObj;
        public void Initialize(GameObject targetObj)
        {
            this.targetObj = targetObj;
        }
        public void Update(Vector3 direction)
        {
            if(direction == Vector3.zero) { return; }
            Quaternion me = targetObj.transform.rotation;
            Quaternion you = Quaternion.LookRotation(direction);
            targetObj.transform.rotation = Quaternion.RotateTowards(me, you, speed * Time.deltaTime);
        }
        public void Update(Transform changeTra)
        {
            Quaternion me = changeTra.rotation;
            Quaternion you = Quaternion.LookRotation(changeTra.position - targetObj.transform.position);
            targetObj.transform.rotation = Quaternion.RotateTowards(me, you, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// AnimatorをEasingで制御できる
    /// </summary>
    [Serializable]
    public class Easing
    {
        [SerializeField] private Traffic traffic;
        [field: SerializeField, NonEditable] public float nowTime { get; private set; }
        [field: SerializeField, NonEditable] public float evaluteValue { get; private set; }
        [field: SerializeField] public AnimationCurve curve { get; set; }

        public void Initialize()
        {
            Reset();

            traffic.Initialize();
            traffic.enableAction += Evalute;
            traffic.disableAction += Reset;
        }
        public void Update()
        {
            traffic.Update();
        }

        public void Reset()
        {
            nowTime = 0.0f;
            evaluteValue *= curve.Evaluate(nowTime);
        }
        public void Evalute()
        {
            evaluteValue = curve.Evaluate(nowTime);
            nowTime += Time.deltaTime;
        }

        public bool active
        {
            get { return traffic.active; }
            set { traffic.active = value; }
        }
    }

    /// <summary>
    /// AnimatorをEasing割合で制御できる
    /// </summary>
    [Serializable]
    public class EasingAnimator
    {
        [SerializeField] private Traffic traffic;
        [field: SerializeField, NonEditable] public float nowRatio { get; private set; }
        [field: SerializeField, NonEditable] public float maxTime { get; private set; }
        [SerializeField] private AnimationCurve curve;
        public Animator animator { get; set; }

        public void Initialize(float maxTime, Animator animator = null)
        {
            if (animator != null) { this.animator = animator; }
            this.maxTime = maxTime;
            nowRatio = 0.0f;


            traffic.Initialize();
            traffic.enableAction += Evalute;
            traffic.disableAction += Reset;
        }
        public void Update()
        {
            traffic.Update();
        }

        public void Reset()
        {
            nowRatio = 0.0f;
        }
        public void Evalute()
        {
            animator.speed *= curve.Evaluate(nowRatio);
            nowRatio += 1 / maxTime * Time.deltaTime;
        }

        public bool active
        {
            get { return traffic.active; }
            set { traffic.active = value; }
        }
    }

    

    /// <summary>
    /// Update内でも一度だけ実行できる
    /// </summary>
    [Serializable]
    public class MomentAction
    {
        public Action action { get; set; }
        [SerializeField, NonEditable] private bool activated;

        public void Initialize()
        {
            activated = false;
        }

        public void Enable()
        {
            if (activated == false)
            {
                action?.Invoke();
                activated = true;
            }
        }
    }

    [Serializable]
    public class Shake
    {
        [field: SerializeField] public Interval interval { get; set; }
        [field: SerializeField] public GameObject targetObj { get; set; }

        public void Initialize()
        {
            interval.Initialize(true);
        }

        public void Update()
        {
            if (interval.active == true)
            {
            }
            interval.Update();

        }

    }

    [Serializable]
    public class EntityAndPlan<T>
    {
        [field: SerializeField] public T entity { get; set; }
        [field: SerializeField] public T plan { get; set; }

        /// <summary>
        /// plan = entity
        /// </summary>
        public void Assign()
        {
            plan = entity;
        }

        public void Default()
        {
            plan = default;
            entity = default;
        }
        public void Reset(T t1)
        {
            plan = t1;
            entity = t1;
        }
        public void PlanDefault()
        {
            plan = default;
        }
    }

    
    [Serializable]
    public class RectNeo
    {
        [field: SerializeField] public float x { get; private set; }
        [field: SerializeField] public float y { get; private set; }
        [field: SerializeField] public float width { get; private set; }
        [field: SerializeField] public float height { get; private set; }
        public Rect entity { get; private set; }

        public RectNeo(Rect rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width;
            height = rect.height;

            entity = rect;
        }

        public void Initialize(Rect rect)
        {
            x = rect.x;
            y = rect.y;
            width = rect.width;
            height = rect.height;

            entity = rect;
        }

        public void SetHorizontal(float x, float width)
        {
            this.x = x;
            this.width = width;

            entity = new Rect(this.x, this.y, this.width, this.height);
        }

        public void SetVertical(float y, float height)
        {
            this.y = y;
            this.height = height;

            entity = new Rect(this.x, this.y, this.width, this.height);
        }
        public float X
        {
            set
            {
                x = value;
                entity = new Rect(x, y, width, height);
            }
        }

        public float Width
        {
            set
            {
                width = value;
                entity = new Rect(x, y, width, height);
            }
        }
        public float Y
        {
            set
            {
                y = value;
                entity = new Rect(x, y, width, height);
            }
        }

        public float Height
        {
            set
            {
                height = value;
                entity = new Rect(x, y, width, height);
            }
        }

    }

    [Serializable] public class VariedTime
    {
        [field: SerializeField, NonEditable] public float value { get; private set; }
        [field: SerializeField] public IncreseType increseType { get; set; }
        [SerializeField] private bool reversalIncrese;
        public void Initialize(float startTime = 0.0f, IncreseType type = default)
        {
            value = startTime;
            if (type != default) { increseType = type; }
        }
        public void Update(float value = 0.0f)
        {
            switch(increseType)
            {
                case IncreseType.DeltaTime:
                    if (reversalIncrese == true) { this.value -= Time.deltaTime; }
                    else { this.value += Time.deltaTime; }
                    break;

                case IncreseType.Frame:
                    if(reversalIncrese == true) { this.value++; }
                    else { this.value--; }
                    break;

                case IncreseType.Manual:
                    this.value = value;
                    break;
            }

        }

    }

    

    [Serializable] public class ValueChecker<T> where T : struct
    {
        [SerializeField] private T value;
        [SerializeField] private T beforeValue;
        [SerializeField] private bool changed;
        public Action changedAction { get; set; }

        public void Initialize(T value)
        {
            Reset(value);
            changedAction = null;
        }

        public void Reset(T value)
        {
            this.value = value;
            beforeValue = value;
            changed = false;
        }

        public void Update(T value)
        {
            this.value = value;
            changed = !value.Equals(beforeValue);   // 変更されていたら

            if (changed == true)
            {
                changedAction?.Invoke();
            }

            beforeValue = value;
        }
    }
    /// <summary>
    /// SpriteRenderer,Image,TMProのすべてを取得する
    /// </summary>
    [Serializable]
    public class SpriteOrImage
    {
        [field: SerializeField] public SpriteRenderer[] sprites { get; set; }
        [field: SerializeField] public Image[] images { get; set; }
        [field: SerializeField] public TextMeshProUGUI[] texts { get; set; }

        /// <summary>
        /// 引数はSpriteRendererまたはImageがアタッチされたGameObject
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Initialize(GameObject obj)
        {
            sprites = obj.GetComponentsInChildren<SpriteRenderer>();
            images = obj.GetComponentsInChildren<Image>();
            texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
            if (sprites.Length == 0 && images.Length == 0 && texts.Length == 0)
            {
                Debug.LogError("いずれもアタッチされていません");
            }
        }

        /// <summary>
        /// SpriteRendererまたはImageのColorを返す<br/>
        /// 両方存在しない場合はエラーメッセージを出してColor.whiteを返す
        /// </summary>
        public Color color
        {
            get
            {
                if (sprites.Length != 0)
                {

                    return sprites[0].color;
                }
                else if (images.Length != 0)
                {
                    return images[0].color;
                }

                Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
                return Color.white;
            }
            set
            {
                if (sprites.Length != 0)
                {
                    foreach (SpriteRenderer sprite in sprites)
                    {
                        sprite.color = value;

                    }
                }
                else if (images.Length != 0)
                {
                    foreach (Image image in images)
                    {

                        image.color = value;
                    }
                }
                else if (texts.Length != 0)
                {
                    foreach (TextMeshProUGUI text in texts)
                    {

                        text.color = value;
                    }
                }
                else
                {
                    Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
                }
            }
        }

        public float Alpha
        {
            get
            {
                if (sprites.Length != 0)
                {

                    return sprites[0].color.a;
                }
                else if (images.Length != 0)
                {
                    return images[0].color.a;
                }

                Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
                return 0.0f;
            }
            set
            {
                if (sprites.Length != 0)
                {
                    foreach (SpriteRenderer sprite in sprites)
                    {
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);

                    }
                }
                else if (images.Length != 0)
                {
                    foreach (Image image in images)
                    {

                        image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                    }
                }
                else if (texts.Length != 0)
                {
                    foreach (TextMeshProUGUI text in texts)
                    {

                        text.color = new Color(text.color.r, text.color.g, text.color.b, value);
                    }
                }
                else
                {
                    Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
                }
            }
        }

    }

    [Serializable] public  class TargetColliders<T> where T : class
    {
        [field: SerializeField] public List<T> targets { get; set; } = new List<T>();
        public Action firstTimeAction { get; set; }
        public TargetColliders()
        {
            Clear();
        }

        public void Clear()
        {
            targets.Clear();
        }

        public void Update(T you)
        {
            bool firstTime = true;

            foreach (T t in targets)  // targetsをループして
            {
                if (t == you)
                {
                    firstTime = false;
                }
            }

            

            if (firstTime == true)          // 同一個体でなければ
            {                               // targetsに追加する
                targets.Add(you);
                firstTimeAction?.Invoke();
            }
        }

        /// <summary>
        /// 引数に対応する配列Indexを返す
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int GetIndex(T t)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                if (targets[i] == t) { return i; }

            }

            return -1;
        }
        public int Count
        {
            get { return targets.Count; }
        }

        public List<T> List
        {
            get { return targets; }
        }

    }

    [Serializable] public class VecT<T>
    {
        public T x;
        public T y;
        public T z;

        private List<T> list = new List<T>();

        public VecT()
        {
            GetList();
        }

        private List<T> GetList()
        {
            list.Clear();
            list = new List<T>() { x, y, z };

            return list;
        }

        public void Assign()
        {
            x = list[0];
            y = list[1];
            z = list[2];
        }
        public void Assign(int index)
        {
            switch (index)
            {
                case 0:
                    x = list[0];
                    break;
                case 1:
                    y = list[1];
                    break;
                case 2:
                    z = list[2];
                    break;
            }
        }

        public void Assign(int index, T t)
        {
            switch (index)
            {
                case 0:
                    x = t;
                    break;
                case 1:
                    y = t;
                    break;
                case 2:
                    z = t;
                    break;
            }
        }

        public T IndexToEntity(int index)
        {
            switch (index)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
            }

            Debug.Log("Indexが違います");
            return default;
        }

        public static int count
        {
            get {  return 3; }
        }

        public List<T> List
        {
            get { return GetList(); }
        }
    }
    [Serializable] public class Vec3Bool
    {
        public bool x;
        public bool y;
        public bool z;

        public VecT<bool> ConvertToVecT()
        {
            VecT<bool> returnVecT = new VecT<bool>();
            returnVecT.x = x;
            returnVecT.y = y;
            returnVecT.z = z;

            return returnVecT;
        }
    }

    /// <summary>
    /// Rangeとそれを評価する値を代入する値を含む
    /// </summary>
    [Serializable] public class ValueInRange
    {
        public enum ThanType
        {
            ThanOrEqual,
            Than,
            LessThanOrEqual,
            Less,
        }
        public ThanType minThan;
        public MinMax range;
        public ThanType maxThan;
        public bool minExcess;
        public bool maxExcess;

        public bool inRange;
        public float currentValue;

        public bool JudgeRange(float value)
        {
            minExcess = false;
            switch (minThan)
            {
                case ThanType.ThanOrEqual:
                    if (range.min> value)
                    {
                        minExcess = true;
                    }
                    break;
                case ThanType.Than:
                    if (range.min>= value)
                    {
                        minExcess = true;
                    }
                    break;

                case ThanType.LessThanOrEqual:
                    if (!(range.min> value))
                    {

                        minExcess = true;
                    }
                    break;
                case ThanType.Less:
                    if (!(range.min>= value))
                    {
                        minExcess = true;
                    }
                    break;

            }


            maxExcess = false;
            switch (maxThan)
            {
                case ThanType.ThanOrEqual:
                    if (value < range.max)
                    {
                        maxExcess = true;
                    }
                    break;
                case ThanType.Than:
                    if (value <= range.max)
                    {
                        maxExcess = true;
                    }
                    break;
                case ThanType.LessThanOrEqual:
                    if (value > range.max)
                    {
                        maxExcess = true;
                    }
                    break;
                case ThanType.Less:
                    if (value >= range.max)
                    {
                        maxExcess = true;
                    }
                    break;
            }

            if (maxExcess == true || minExcess == true)
            {
                return false;
            }

            return true;
        }
        public void Update(float value = 0.0f)
        {
            if (value != 0.0f) { currentValue = value; }

            inRange = JudgeRange(currentValue);
        }

        public float min
        {
            get { return range.min; }
        }
        public float max    
        {
            get { return range.max; }
        }
    }

    
    

    [Serializable] public class Curve
    {
        [SerializeField] private AnimationCurve curve = new AnimationCurve();
        [SerializeField] private VariedTime time = new VariedTime();
        [field: SerializeField] public float currentValue { get; private set; }
        public Curve()
        {
            time = new VariedTime();
        }
        public float Update()
        {
            currentValue = curve.Evaluate(time.value);

            time.Update();

            return currentValue;
        }

        public void Clear()
        {
            time.Initialize();
        }
    }

    [Serializable] public class Vec3Curve
    {
        public AnimationCurve xCurve;
        public AnimationCurve yCurve;
        public AnimationCurve zCurve;
        private List<AnimationCurve> curves = new List<AnimationCurve>();
        
        public void Initialize()
        {
            Reset();
        }

        public void Reset()
        {

            curves.Clear();
            curves.Add(xCurve);
            curves.Add(yCurve);
            curves.Add(zCurve);

        }

        public void ZeroFill()
        {
            for(int i = 0; i < curves.Count; ++i)
            {
                bool artificial = false;
                if (curves[i].length != 0 && curves[i].length != 1)
                {
                    artificial = true;
                }

                if(artificial == false)
                {
                    for (int j = 0; j < curves[i].length; ++j)
                    {

                        curves[i].RemoveKey(j);
                    }

                    curves[i].AddKey(0.0f, 0.0f);
                    curves[i].AddKey(1.0f, 0.0f);

                    for (int j = 0; j < curves[i].length; ++j)
                    {
                        curves[i].MoveKey(j, new Keyframe(j, 0));
                    }

                    curves[i].postWrapMode = WrapMode.Loop;
                    curves[i].preWrapMode = WrapMode.Loop;
                }
            }
        }
        public void Clear()
        {
            for (int i = 0; i < curves.Count; ++i)
            {
                for (int k = 0; k < 3; ++k)
                {
                    for (int j = 0; j < curves[i].length; ++j)
                    {
                        curves[i].MoveKey(j, new Keyframe(0, 0));
                        curves[i].RemoveKey(0);
                    }

                }
            }
        }

        /// <summary>
        /// 現在時間
        /// </summary>
        public Vector3 Eva(float time)
        {
            Vector3 newEva;
            newEva.x = xCurve.Evaluate(time);
            newEva.y = yCurve.Evaluate(time);
            newEva.z = zCurve.Evaluate(time);

            return newEva;
        }

    }

    /// <summary>
    /// 図形のlocalScale.xまたはyを参照値に合わせて拡縮させる
    /// </summary>
    [Serializable]
    public class BarByParam
    {

        [SerializeField] private GameObject bar;
        [SerializeField] private float entity;
        [SerializeField] private float max;
        [SerializeField] private float ratio;
        [SerializeField] private bool warp;

        public void Update(float entity, float max)
        {
            this.entity = entity;
            this.max = max;
            ratio = entity / max;
            if (warp == true)
            {
                bar.transform.localScale = new Vector3(bar.transform.localScale.x, ratio);

            }
            else
            {
                bar.transform.localScale = new Vector3(ratio, bar.transform.localScale.y);


            }
        }
    }




#if UNITY_EDITOR
    /// <summary>
    /// serializedObjectUpdateに関数を追加する
    /// </summary>
    /// <typeparam name="CustomEditorType"></typeparam>
    public class MyEditor<CustomEditorType> : Editor where CustomEditorType : UnityEngine.Object
    {
        protected Action serializedObjectUpdate;
        protected CustomEditorType tg;
        protected void Initialize()
        {
            tg = (CustomEditorType)target;
        }


        public override void OnInspectorGUI()
        {

            SerializedObjectUpdate();
            EndUpdate();

        }

        protected void EndUpdate()
        {

            if (GUI.changed)
            {
                EditorUtility.SetDirty(tg);
            }

        }

        void SerializedObjectUpdate()
        {
            serializedObject.Update();
            {
                serializedObjectUpdate?.Invoke();
            }
            serializedObject.ApplyModifiedProperties();
        }

    }


    /// <summary>
    /// 表示する変数名のstringを用意する<br/>
    /// Updateを上書きする
    /// </summary>
    public class MyPropertyDrawer : PropertyDrawer
    {
        public Rect pos;
        public SerializedProperty prop;
        public float boolWidth = 15;
        public float uniformedLabelWidth;
        public float uniformedFieldWidth;
        public float labelWidthAve;
        public float distance = 4.1f;       // PropertyDrawerの定数
        public Vector2 elements;
        public List<List<LabelAndproperty>> verticalProps;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                pos = position; 
                pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

                prop = property;

                // 子のフィールドをインデントしない 
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                Update(position, property, label);


                // インデントを元通りに戻します
                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();


        }

        /// <summary>
        /// verticalPropsを全て描画する
        /// </summary>

        public void DrawPropsList()
        {
            for (int i = 0; i < verticalProps.Count; ++i)
            {
                SetElements(i);
                Uniform(verticalProps[i]);
                for(int j = 0; j < verticalProps[i].Count; ++j)
                {
                    //if(verticalProps[i][j].useRatioInRemainder == 0.0f)
                    //{
                    //    verticalProps[i][j].propertyRect.Width = UseHorizontalRatio(1.0f);
                    //}
                    //else
                    //{
                    //    verticalProps[i][j].propertyRect.Width = UseHorizontalRatio(verticalProps[i][j].useRatioInRemainder);

                    //}
                    if(i != 0)
                    {
                        verticalProps[i][j].verticalNeighbor = verticalProps[i - 1];
                        verticalProps[i][j].NewLinePosSet();
                    }
                }
                DrawPropsOnHorizontal(verticalProps[i]);
                
            }
        }

        public Vector2 SetElements(int verticalIndex = 0)
        {
            Vector2 newEle;
            newEle.x = verticalProps[verticalIndex].Count;
            newEle.y = verticalProps.Count;
            elements = newEle;

            return elements;
        }

        public void SetPropsList(List<List<LabelAndproperty>> props)
        {
            for(int i = 0; i < props.Count; ++i)
            {
                for(int j = 0; j < props[i].Count; ++j)
                {
                    props[i][j].Set(pos, prop);
                }
            }
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 各変数の高さを計算
            return EditorGUIUtility.singleLineHeight * elements.y + 1.82f;
        }
        protected virtual void Update(Rect pos, SerializedProperty prop, GUIContent label)
        { }


        /// <summary>
        /// Updateの最初に行う<br/>
        /// 引数はLabelAndPropertyList<br/>
        /// Labelの大きさ
        /// </summary>
        /// <param name="horizontalElements"></param>
        public void Uniform(List<LabelAndproperty> horizontalProps, float aveLabelWidth)
        {
            uniformedLabelWidth = aveLabelWidth;
            foreach (var prop in horizontalProps)
            {
                prop.labelRect.Width = aveLabelWidth;
            }
            uniformedFieldWidth = pos.width / horizontalProps.Count - uniformedLabelWidth - distance * 2;
        }


        public void Uniform(List<LabelAndproperty> horizontalProps)
        {
            uniformedLabelWidth = 0.0f;
            foreach (var prop in horizontalProps)
            {
                uniformedLabelWidth += prop.labelRect.width;
            }
            labelWidthAve = uniformedLabelWidth / horizontalProps.Count;
            uniformedLabelWidth = labelWidthAve;
            uniformedFieldWidth = pos.width / horizontalProps.Count - uniformedLabelWidth - distance * 2;

        }

        /// <summary>
        /// 横幅の使用する割合を指定する
        /// </summary>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public float UseHorizontalRatio(float ratio)
        {
            return (pos.width / elements.x - labelWidthAve - distance * 2) * (ratio * elements.x);
        }

        public void DrawPropsOnHorizontal(List<LabelAndproperty> lavProps)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                float ratio;
                if (lavProps[i].useRatioInRemainder == 0.0f)
                {
                    ratio = UseHorizontalRatio(1.0f);
                }
                else
                {
                    ratio = UseHorizontalRatio(lavProps[i].useRatioInRemainder);
                }


                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, lavProps[i].labelRect.width, ratio);

                }
                else
                {
                    lavProps[i].horizontalNeighbor = lavProps[i - 1];
                    lavProps[i].PosSetByHorizontalNeighbor();
                    lavProps[i].propertyRect.Width = ratio;
                }
                lavProps[i].Draw();
            }
        }
        public void UniformedDraw(List<LabelAndproperty> lavProps, float avePropWidth)
        {

            for (int i = 0; i < lavProps.Count; i++)
            {
                if (i == 0)
                {
                    lavProps[i].InitialPosSet(pos.x, lavProps[i].labelRect.width, avePropWidth);

                }
                else
                {
                    lavProps[i].horizontalNeighbor = lavProps[i - 1];
                    lavProps[i].PosSetByHorizontalNeighbor();
                    lavProps[i].propertyRect.Width = avePropWidth;
                }
                lavProps[i].Draw();
            }
        }

        public class LabelAndproperty
        {
            public RectNeo labelRect;
            public string label;
            public RectNeo propertyRect;
            public SerializedProperty property;
            public string propName;
            public float useRatioInRemainder;
            public LabelAndproperty horizontalNeighbor;
            public List<LabelAndproperty> verticalNeighbor;
            public EditType edit;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="rect"></param>
            /// <param name="property"></param>
            
            public LabelAndproperty(string propertyName)
            {
                this.propName = propertyName;
            }
            public void Set(Rect rect, SerializedProperty property)
            {
                labelRect = new RectNeo(rect);
                propertyRect = new RectNeo(rect);
                labelRect.Height = EditorGUIUtility.singleLineHeight;
                propertyRect.Height = EditorGUIUtility.singleLineHeight;
                this.property = property.FindPropertyRelative(propName);
                useRatioInRemainder = 0.0f;

                char[] array = this.property.displayName.ToCharArray();
                array[0] = char.ToUpper(array[0]);  // 戦闘を大文字にする

                label = new string(array);
            }

            public void InitialPosSet(float x, float labelWidth, float fieldRatio = 0.0f)
            {
                if(fieldRatio != 0.0f)
                {
                    labelRect.SetHorizontal(x, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, fieldRatio);

                }
                else
                {
                    labelRect.SetHorizontal(x, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, useRatioInRemainder);
                }
            }

            public void HorizontalityNeighborPosSet(float labelWidth, float fieldWidth = 0.0f)
            {
                if(fieldWidth != 0.0f)
                {
                    labelRect.SetHorizontal(AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, fieldWidth);
                }
                else
                {
                    labelRect.SetHorizontal(AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5, labelWidth);
                    propertyRect.SetHorizontal(AddFunction.HorizontalityNeighbor(labelRect) + 5, useRatioInRemainder);

                }
            }

            public void NewLinePosSet()
            {
                float distanceBeforeLine = 1.82f;
                labelRect.Y = AddFunction.VerticalityNeighbor(verticalNeighbor[0].propertyRect) + distanceBeforeLine;
                propertyRect.Y = labelRect.y;

            }
            public void Draw()
            {
                switch (edit)
                {

                    case EditType.None:
                        DrawPropertyField();
                        break;

                    case EditType.NonEditable:
                        EditorGUI.LabelField(labelRect.entity, label);
                        
                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                        }
                        EditorGUI.EndDisabledGroup();
                        break;

                    case EditType.NonEditableInGame:
                        EditorGUI.LabelField(labelRect.entity, label);

                        if (EditorApplication.isPlaying)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);

                        }

                        break;
                }
            }

            

            /// <summary>
            /// verticalList内をすべて描画する
            /// </summary>
            public void DrawProps()
            {
                switch (edit)
                {

                    case EditType.None:
                        DrawPropertyField();
                        break;

                    case EditType.NonEditable:
                        EditorGUI.LabelField(labelRect.entity, label);

                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                        }
                        EditorGUI.EndDisabledGroup();
                        break;

                    case EditType.NonEditableInGame:
                        EditorGUI.LabelField(labelRect.entity, label);

                        if (EditorApplication.isPlaying)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            {
                                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);

                        }

                        break;
                }
            }
            public void DrawPropertyField()
            {
                EditorGUI.LabelField(labelRect.entity, label);
                EditorGUI.PropertyField(propertyRect.entity, property, GUIContent.none);
            }

            public void PosSetByHorizontalNeighbor()
            {
                labelRect.X = AddFunction.HorizontalityNeighbor(horizontalNeighbor.propertyRect) + 5;
                propertyRect.X = AddFunction.HorizontalityNeighbor(labelRect) + 5;
            }
        }
        public enum EditType
        {
            None,
            NonEditable,
            NonEditableInGame,
        }
    }

#endif

    #region インスペクタープロパティ
    /// <summary>
    /// 数値の中身と最大値を含む<br/>
    /// インスタンス化不要
    /// </summary>
    [Serializable]
    public class Parameter
    {
        public float entity;
        public float max;
        public float autoRecoverValue;
        public void Initialize()
        {
            entity = max;
        }

        public void Update()
        {
            entity += autoRecoverValue;
            ReturnRange();
        }

        public void Update(float changeEntity)
        {
            entity += changeEntity;
            ReturnRange();
        }

        public void ReturnRange()
        {

            if (entity > max) { entity = max; }
            else if (entity < 0.0f) { entity = 0.0f; }
        }

        public bool inRange
        {
            get
            {
                if (entity <= max)
                {
                    if (entity >= 0.0f)
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        public bool overZero    // entityが0以下なら
        {
            get
            {
                if (entity <= 0.0f) { return true; }
                return false;
            }
        }

        /// <summary>
        /// 使用可能なら
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CostJudge(float cost)
        {
            if (entity - cost > 0.0f)
            {
                return true;
            }

            return false;
        }
    }

    [Serializable] public class MinMax
    {
        public float min;
        public float max;

        public void Clear()
        {
            min = 0.0f;
            max = 0.0f;
        }

        public void SymmetryMax()
        {
            min = -max;
        }
        public void SymmetryMin()
        {
            max = -min;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Parameter))]
    public class ParameterDrawer : MyPropertyDrawer
    {

        LabelAndproperty entity = new LabelAndproperty(nameof(entity));
        LabelAndproperty max = new LabelAndproperty(nameof(max));
        LabelAndproperty autoRecoverValue = new LabelAndproperty(nameof(autoRecoverValue));
        protected override void Update(Rect _pos, SerializedProperty property, GUIContent label)
        {
            autoRecoverValue.label = "Auto";

            List<LabelAndproperty> list = new List<LabelAndproperty>() { entity, max, autoRecoverValue };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            entity.useRatioInRemainder = 0.4f;
            max.useRatioInRemainder = 0.3f;
            autoRecoverValue.useRatioInRemainder = 0.3f;

            entity.edit = EditType.NonEditable;

            entity.labelRect.Width = 40;
            max.labelRect.Width = 25;
            autoRecoverValue.labelRect.Width = 30;

            DrawPropsList();
        }
    }

    [CustomPropertyDrawer(typeof(Vec3Bool))]
    public class Vec3BoolDrawer : MyPropertyDrawer 
    {
        LabelAndproperty x = new LabelAndproperty("x");
        LabelAndproperty y = new LabelAndproperty("y");
        LabelAndproperty z = new LabelAndproperty("z");
        protected override void Update(Rect _pos, SerializedProperty _prop, GUIContent _label)
        {
            
            List<LabelAndproperty> list = new List<LabelAndproperty>() { x, y, z };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            foreach (LabelAndproperty l in list)
            {
                l.edit = EditType.NonEditableInGame;
            }


            Uniform(list, 10);
            UniformedDraw(list, boolWidth);
        }
    }

    [CustomPropertyDrawer(typeof(MinMax))]
    public class RangeDrawer : MyPropertyDrawer
    {
        LabelAndproperty min = new LabelAndproperty(nameof(min));
        LabelAndproperty max = new LabelAndproperty(nameof(max));

        protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
        {
            List<LabelAndproperty> list = new List<LabelAndproperty>() { min, max };
            verticalProps = new List<List<LabelAndproperty>>() { list };
            SetElements();
            SetPropsList(verticalProps);

            foreach (LabelAndproperty l in list)
            {
                l.edit = EditType.None;
            }

            min.labelRect.Width = max.labelRect.Width = 25.0f;

            min.useRatioInRemainder = max.useRatioInRemainder = 0.5f;

            DrawPropsList();
        }
    }

    [CustomPropertyDrawer(typeof(Traffic))]
    public class TrafficDrawer : MyPropertyDrawer
    {
        LabelAndproperty active = new LabelAndproperty(nameof(active));
        protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
        {
            List<LabelAndproperty> list = new List<LabelAndproperty> { active };
            verticalProps = new List<List<LabelAndproperty>> { list };
            SetElements();
            SetPropsList(verticalProps);

            active.edit = EditType.NonEditable;
            active.labelRect.Width = 40;


            DrawPropsList();
        }
    }

    //[CustomPropertyDrawer(typeof(VecT<object>))]
    //public class VecTDrawer : MyPropertyDrawer
    //{
    //    LabelAndproperty entity = new LabelAndproperty(nameof(entity));
    //    LabelAndproperty plan  = new LabelAndproperty(nameof(plan));
    //    protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
    //    {
    //        List<LabelAndproperty> list = new List<LabelAndproperty> { entity, plan };
    //        entity.Draw();
    //    }
    //}

    //[CustomPropertyDrawer(typeof(Vec3Curve))]
    //public class Vec3CurveDrawer : MyPropertyDrawer
    //{
    //    LabelAndproperty xCurve = new LabelAndproperty(nameof(xCurve));
    //    LabelAndproperty yCurve = new LabelAndproperty(nameof(yCurve));
    //    LabelAndproperty zCurve = new LabelAndproperty(nameof(zCurve));
    //    protected override void Update(Rect pos, SerializedProperty prop, GUIContent label)
    //    {
    //        List<LabelAndproperty> lavProps = new List<LabelAndproperty>() { xCurve, yCurve, zCurve };
    //        Uniform(lavProps.Count, 10);
    //        foreach(LabelAndproperty l in lavProps)
    //        {
    //            l.edit = EditType.NonEditableInGame;
    //        }
    //        UniformedDraw(lavProps);
    //    }
    //}
#endif

    #endregion
}

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}
public enum IncreseType
{
    DeltaTime,
    Frame,
    Manual,
}
