using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {
    static public ProjectileLine S; // Singleton

    public float minDist = 0.1f;
    public bool ______________;

    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    void Awake() {
        S = this;
        // Извлекаем лайн рендерер
        line = GetComponent<LineRenderer>();
        // Выключаем пока не нужен
        line.enabled = false;
        // Создаём список точек
        points = new List<Vector3>();
    }

    public GameObject poi {
        get { return (_poi); }
        set {
            _poi = value;
            if(_poi != null) {
                // Если приравнивается другая точка, всё обнуляется
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint() {
        // Вызывается чтобы добавить точку в линию
        Vector3 pt = _poi.transform.position;
        // Если точка недостаточно далеко от другой точки, возвращаем
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
            return;
        }
        if (points.Count == 0) {
            // Если это страртовая точка
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position; // launchPos
            Vector3 launchPosDiff = pt - launchPos;
            // Линия для лучшего прицелиявания
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            //line.SetVertexCount(2);
            line.positionCount = 2;
            // Задаём первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Включаем прорисовку
            line.enabled = true;
        } else {
            // Это не первая точка
            points.Add(pt);
            //line.SetVertexCount(points.Count);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint {
        get {
            if (points == null) {
                // Если нет точек возвращаем начало
                return (Vector3.zero);
            } else {
                return (points[points.Count - 1]);
            }
        }
    }

    void FixedUpdate() {
        if (poi == null) {
            // Если нет пои, ищем такой
            if (FollowCam.S.poi != null) {
                if (FollowCam.S.poi.tag == "Projectile") {
                    poi = FollowCam.S.poi;
                } else {
                    // Возвращаем если это не тот тег
                    return;
                }
            // Возвращаем если не нашли пои
            } else return;
        }
        // Если там есть пои, его позиция добавляется постоянно
        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping()) {
            // Если пои не двигается, можно очистить
            poi = null;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
