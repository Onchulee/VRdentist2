using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumController : MonoBehaviour
{
    [System.Serializable]
    public struct StitchArea {
        public Transform spot1;
        public Transform spot2;
    }

    [SerializeField]
    public StitchArea[] stitchAreas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
