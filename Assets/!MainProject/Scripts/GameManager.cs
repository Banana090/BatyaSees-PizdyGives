using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float watchingTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float playingTime;

    [SerializeField] public LayerMask interactableObjects { get; private set; }
    [SerializeField] public int interactableLayerInt { get; private set; }
    [SerializeField] public Vector4 roomBorders { get; private set; }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartWatchingTime());
    }

    private IEnumerator StartWatchingTime()
    {
        float timer = watchingTime;
        while (timer > 0)
        {
            timerText.text = ((int)timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        EnemyController.instance.StartCoroutine(EnemyController.instance.SpawnEnemies(5, attackTime));
    }

    private IEnumerator GameTimer()
    {
        timerText.text = playingTime.ToString();
        float timeLeft = playingTime;

        while (timeLeft > 0)
        {
            timerText.text = ((int)timeLeft).ToString();
            timeLeft -= timeLeft <= 3 ? Time.deltaTime / 2 : Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        List<Transform> wrongObjects;

        if (ObjectsController.CheckForWin(out wrongObjects))
        {
            timerText.text = "WIN";
        }
        else
        {
            //Lost. Highlight wrong objects
            timerText.text = "LOST";
            foreach (var item in wrongObjects)
            {
                Debug.Log(item.name);
            }
        }
    }

    public static void StartGameTimer()
    {
        instance.StartCoroutine(instance.GameTimer());
    }

    public static Vector3 GetRandomPointInRoom()
    {
        Vector3 point = new Vector3(Random.Range(instance.roomBorders.x, instance.roomBorders.z), Random.Range(instance.roomBorders.w, instance.roomBorders.y), 0);
        return point;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(roomBorders.x, roomBorders.y), new Vector2(roomBorders.z, roomBorders.y));
        Gizmos.DrawLine(new Vector2(roomBorders.x, roomBorders.y), new Vector2(roomBorders.x, roomBorders.w));
        Gizmos.DrawLine(new Vector2(roomBorders.z, roomBorders.w), new Vector2(roomBorders.z, roomBorders.y));
        Gizmos.DrawLine(new Vector2(roomBorders.z, roomBorders.w), new Vector2(roomBorders.x, roomBorders.w));
    }
}
