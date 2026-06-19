using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using kube;
public class TouchingCameraScript : MonoBehaviour
{		

	    public bool OnCameraRotated;
		public float m_HorizontalRot;
		public float m_VerticalRot;
        float RightFinId = -1;
		public static TouchingCameraScript instance;
		public void Awake() => instance = this;
        private void Update() 
		{
			/*foreach (var touch in ControlFreak2.CF2Input.touches)
			{
				float touchX = (touch.position.x > Screen.width / 2) ? touch.deltaPosition.x : 0.0f;
				float touchY = (touch.position.x > Screen.width / 2) ? touch.deltaPosition.y : 0.0f;
				float rotateX = touchX * Kube.GPS.mouseSens + 0.25f * Time.deltaTime;
				float rotateY = touchY * Kube.GPS.mouseSens + 0.25f  * Time.deltaTime;
                
				m_HorizontalRot = rotateX;
				m_VerticalRot = rotateY;
				if (touch.phase  == TouchPhase.Began && touch.position.x > Screen.width/ 2 && RightFinId == -1) {
				RightFinId = touch.fingerId;
			   }
				if (touch.phase == TouchPhase.Moved && touch.position.x > Screen.width / 2 && touch.fingerId != -1)
				{
					OnCameraRotated = true;
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					OnCameraRotated = false;
				}		
				if (touch.phase == TouchPhase.Ended) {
					if(touch.fingerId == RightFinId) {

					RightFinId = -1;

				}
				}
			}*/
		}
}
