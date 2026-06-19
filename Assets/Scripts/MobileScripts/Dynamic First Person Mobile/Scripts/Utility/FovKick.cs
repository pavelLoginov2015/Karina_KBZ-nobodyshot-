using UnityEngine;
using FirstPersonMobileTools.DynamicFirstPerson;

namespace FirstPersonMobileTools.Utility
{

    public class FovKick : MonoBehaviour {
            
        public float m_Ammount = 10.0f;
        public float m_Delay = 1.0f;

        [HideInInspector] public float m_OriginalFov = 0;

        private Camera m_Camera;
        private float m_CurrentFov;

      
        public void Start()
        {
           
        }

        private void FixedUpdate() 
        {

            
        }

        public void AdjustFov(float time)
        {   

            m_CurrentFov += (m_Ammount / m_Delay ) * time;
            m_CurrentFov = Mathf.Clamp(m_CurrentFov, m_OriginalFov, m_OriginalFov + m_Ammount);
            m_Camera.fieldOfView = m_CurrentFov;

        }

    }

}