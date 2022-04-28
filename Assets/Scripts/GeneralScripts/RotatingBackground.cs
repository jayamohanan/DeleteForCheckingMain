using UnityEngine;
using System.Collections;

namespace Mog {
	public class RotatingBackground : MonoBehaviour {
		
		// rotation per second
		public float rotationSpeed = 10F;
		public bool flip = false;
		// Set to lower value for pendulum effect
		public float maxAngleTilt = 360;
		private float angleTilt = 0F;
        private bool isIncreasing = true;
		private float initialAngle;

        void Start () {
			initialAngle = transform.rotation.z;
		}

        float GetTime() {
            if (Time.timeScale == 0) {
                return Time.unscaledDeltaTime;
            }
            return Time.deltaTime;
        }
		
		// Update is called once per frame
		void Update () {
			
			if (isIncreasing)
                angleTilt += rotationSpeed * GetTime();
			else 
                angleTilt -= rotationSpeed * GetTime();
			
			if (angleTilt >= maxAngleTilt) {
				angleTilt -= maxAngleTilt;
				if (flip)
					isIncreasing = false;
			}
			if (angleTilt <= -maxAngleTilt) {
				angleTilt += maxAngleTilt;
				if (flip)
					isIncreasing = true;
			}
			transform.rotation = Quaternion.Euler (0, 0, initialAngle + angleTilt);
		}
	}
}
