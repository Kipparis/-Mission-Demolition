using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour {
    public int numClouds = 40;  // Кол-во облаков естесна
    public GameObject[] cloudPrefabs;
    public Vector3 cloudPosMin;
    public Vector3 cloudPosMax;
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 5;
    public float cloudSpeedMult = 0.5f;

    public bool _________________;

    public GameObject[] cloudInstances;

    void Awake() {
        // Создаём массив для содержания всех облачков
        cloudInstances = new GameObject[numClouds];
        // Находим главаря облаков
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Зацикливаем и делаем облака
        GameObject cloud;
        for (int i = 0; i < numClouds; i++) {
            // Выбираем рандомный префаб облака
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            // Располагаем облако
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Изменяем размер
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            // Маленькие облака должны быть ещё ближе к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            // Маленькие облака должны быть гораздо дальше
            cPos.z = 100 - 90 * scaleU;
            // Подгоняем это всё к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Делаем облако дятём бога
            cloud.transform.parent = anchor.transform;
            // Добавляем в массив
            cloudInstances[i] = cloud;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Занимаемся только движением облаков
        // Циклим через облака
        foreach (GameObject cloud in cloudInstances) {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Двигаем большие облака быстрее
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // Если облако зашло слишком далеко переносим
            if (cPos.x < cloudPosMin.x) cPos.x = cloudPosMax.x;
            cloud.transform.position = cPos;
        }
	}
}
