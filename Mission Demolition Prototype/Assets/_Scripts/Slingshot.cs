using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {
    public static Slingshot S;
    public GameObject prefabProjectile;
    public float velocityMult = 4f;
    public bool ______________________;
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake() {
        S = this;
        //Transform launchPointTrans = transform.Find("LaunchPoint");
        //launchPoint = launchPointTrans.gameObject;
        launchPoint = GameObject.Find("LaunchPoint");
        launchPoint.SetActive(false);
        launchPos = launchPoint.transform.position;
    }

    void OnMouseEnter() {
        print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        // Игрок нажал кнопку
        aimingMode = true;
        // Создаём снаряд
        projectile = Instantiate(prefabProjectile) as GameObject;
        // Перемещаем в точку запуска
        projectile.transform.position = launchPos;
        // Задаём кинематику
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Если мы не целимся, ничего не делаем
        if (!aimingMode) return;
        // Извлекаем координаты мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Находим разницу в позиции запуска и мышкой
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Ограничиваем максимальную разницу
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Перемещаем снаряд в эту позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {
            // Если кнопку отпустили
            aimingMode = false;
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
            FollowCam.S.poi = projectile;
            projectile = null;
        }
	}
}
