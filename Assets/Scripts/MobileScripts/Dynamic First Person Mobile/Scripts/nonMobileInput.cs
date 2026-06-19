using UnityEngine;

namespace FirstPersonMobileTools.DynamicFirstPerson
{

    [RequireComponent(typeof(CameraLook))]
    public class nonMobileInput : MonoBehaviour {

        [HideInInspector] public float Sensitivity_X { private get { return _Sensitivity.x; } set { _Sensitivity.x = value * 50 / 3; }}
        [HideInInspector] public float Sensitivity_Y { private get { return _Sensitivity.y; } set { _Sensitivity.y = value * 50 / 3; }}

        [SerializeField] private KeyCode JumpInput;
        [SerializeField] private KeyCode SprintInput;
        [SerializeField] private KeyCode CrouchInput;
        [SerializeField] private bool LockCursor;
        [SerializeField] private Vector2 _Sensitivity = new Vector2(50f, 50f);

        private CameraLook cameraLook;
        private Camera _camera;
        
        Quaternion y;
        Quaternion x;
        Vector2 delta = Vector2.zero;
        
        private void Start() {
            
            if (Camera.main != null)
                _camera = Camera.main;
            else Debug.LogError($"Can't find any main camera in scene!\n(Set your camera tag as MainCamera)", this);

           
            cameraLook = GetComponent<CameraLook>();
            
        }

        private void Update() {

        }

        void OnValidate()
        {

            if (LockCursor)
            {
                ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
                ControlFreak2.CFCursor.visible = false;
            }
            else
            {
                ControlFreak2.CFCursor.lockState = CursorLockMode.None;
                ControlFreak2.CFCursor.visible = true;
            }
        }

    }

}