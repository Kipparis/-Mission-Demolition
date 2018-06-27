using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 
    -Сделать так, чтоб при смене вида со снаряда, его линия изменялась
    -Сделать показ линий рогатки с помощью Line Renderer
    -Сделать так, чтобы ближайшие несколько выстрелов так же показывались линиями
    -Если сделано больше трёх выстрелов левел перезагружается
     
     */

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static public MissionDemolition S;

    public GameObject[] castles;    // Все замки
    public Text gtLevel;
    public Text gtScore;
    public Vector3 castlePos;   // Место куда поставить замки

    public bool ____________________;

    public int level;   // Текущий уровень
    public int levelMax;    // Количество уровней
    public int shotsTaken;  // Кол-во выстрелов
    public GameObject castle;   // Текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";    

	// Use this for initialization
	void Start () {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
	}
	
    void StartLevel() {
        // Уничтожаем старый замок, если существует
        if (castle != null) Destroy(castle);
        // Разрушаем старые снаряды если существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject proj in gos) {
            Destroy(proj);
        }

        // Создаём новый замок
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Возвращаем камеру
        SwitchView("Both");
        ProjectileLine.S.Clear();

        // Возвращаем цель
        Goal.goalMet = false;

        ShowGT();

        mode = GameMode.playing;
    }

    void ShowGT() {
        // Показывает инфу в текстовой инфе
        gtLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        gtScore.text = "Shots Taken: " + shotsTaken;
    }

	// Update is called once per frame
	void Update () {
        ShowGT();

        // Проверяем конец уровня
        if (mode == GameMode.playing && Goal.goalMet) {
            // Изменяем чтобы перестать проверять конец игры
            mode = GameMode.levelEnd;
            // Отдаляемся
            SwitchView("Both");
            // Начинаем следующий уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
	} 

    void NextLevel() {
        level++;
        if (level == levelMax) level -= level;
        StartLevel();
    }

    void OnGUI() {
        // Показываем кнопку для переключения вида в верху экрана
        Rect buttonRect = new Rect((Screen.width / 2) - 50, 10, 100, 24);

        switch (showing) {
            case "Slingshot":
                if(GUI.Button(buttonRect,"Show Castle")) {
                    SwitchView("Castle");
                }
                break;
            case "Castle":
                if (GUI.Button(buttonRect, "Show Both")) {
                    SwitchView("Both");
                }
                break;
            case "Both":
                if (GUI.Button(buttonRect, "Show Slingshot")) {
                    SwitchView("Slingshot");
                }
                break;
        }
    }

    static public void SwitchView(string eView) {
        S.showing = eView;
        switch (S.showing) {
            case "Slingshot":
                FollowCam.S.poi = null;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }

    public static void ShotFired() {
        S.shotsTaken++;
    }
}
