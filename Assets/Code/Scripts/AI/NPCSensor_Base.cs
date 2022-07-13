using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Sensors change a NPC_Base status condition
/// </summary>
/*[System.Serializable]
public struct NPCSensor_Condition{
	public NPC_Condition condition;
	public bool value;
}*/

namespace Unity.Ricochet.AI
{
    public class NPCSensor_Base : MonoBehaviour
    {
        public Enemy npcBase;
        //	public List<NPCSensor_Condition> appliedConditons;
        protected List<GameObject> sensedObjects = new List<GameObject>();

        void Start()
        {
            if (npcBase == null)
                npcBase = gameObject.GetComponent<Enemy>();
            StartSensor();
        }

        void Update()
        {
            UpdateSensor();
        }
        protected virtual void StartSensor() { }
        protected virtual void UpdateSensor() { }

        protected List<GameObject> GetSensedObjects()
        {
            return sensedObjects;
        }
    }
}
