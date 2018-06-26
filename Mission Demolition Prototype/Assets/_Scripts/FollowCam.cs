using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public FollowCam S;
    public float easing = 0.05f;
    public Vector2 minXY;
    public bool ____________;
    public GameObject poi;  // point of interest
    public float camZ;  

    void Awake() {
        S = this;
        camZ = this.transform.position.z;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Если нет объекта слежки, возвращаем
        if (poi == null) return;

        // Извлекаем позицию точки интереса
        Vector3 destination = poi.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Добавляем туда Z координату камеры
        destination.z = camZ;
        // Переводим камеру в этом место
        transform.position = destination;
        // Поддерживаем размер камеры, чтобы было видно землю
        this.GetComponent<Camera>().orthographicSize = destination.y + 10;
	}
}
