using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControllerCharacter : MonoBehaviour
{

    Animator charAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
