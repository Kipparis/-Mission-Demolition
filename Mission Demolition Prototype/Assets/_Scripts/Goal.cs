using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other) {
        // Когда коллайдер ударен, проверим снаряд ли это
        if (other.gameObject.tag == "Projectile") {
            // Если так то делаем что-то
            Goal.goalMet = true;
            // Альфа канал повышаем
            Color c = this.GetComponent<Renderer>().material.color;
            c.a = 1;
            this.GetComponent<Renderer>().material.color = c;
        }
    }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    
} 
