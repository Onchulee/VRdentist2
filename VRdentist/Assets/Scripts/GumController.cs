using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumController : MonoBehaviour
{
    [SerializeField]
    public Transform targetGum_parent;

    [SerializeField]
    public Transform sticthGum_parent;
    [SerializeField]
    public Transform cutGum_parent;
    [SerializeField]
    public Transform widenGum_parent;
    
    void Start()
    {
        SettingGum(targetGum_parent, sticthGum_parent);
    }
    

    public void SettingGum(Transform target, Transform setup) {
        if (target == null || setup == null) return;
        for(int i = 0; i < target.childCount; i++) {
            Transform targetChild = target.GetChild(i);
            Transform setupChild = setup.Find(targetChild.name);
            if (setupChild){
                targetChild.transform.position = setupChild.transform.position;
                targetChild.transform.rotation = setupChild.transform.rotation;
            }
        }

    }
}
