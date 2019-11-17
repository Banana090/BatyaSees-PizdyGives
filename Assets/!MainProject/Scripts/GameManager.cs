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
    [SerializeField] private int enemyCount;
    [SerializeField] private float playingTime;

    [SerializeField] private Material wrongMat;
    [SerializeField] private Material normalMat;

    [SerializeField] public LayerMask interactableObjects { get; private set; }
    [SerializeField] public int interactableLayerInt { get; private set; }
    [SerializeField] public Vector4 roomBorders { get; private set; }

    private PlayerMovement playerMovement;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        playerMovement = FindObjectOfType<PlayerMovement>();
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

        EnemyController.instance.StartCoroutine(EnemyController.instance.SpawnEnemies(enemyCount, attackTime));
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
            List<SpriteRenderer> renderers = new List<SpriteRenderer>();
            foreach (Transform item in wrongObjects)
                renderers.AddRange(item.GetComponentsInChildren<SpriteRenderer>());

            float t = Time.time;
            while (t + 5f >= Time.time)
            {
                foreach (var rend in renderers)
                {
                    if (rend.name == "noColored") continue;
                    rend.material = wrongMat;
                }
                yield return new WaitForSeconds(0.5f);
                foreach (var rend in renderers)
                {
                    if (rend.name == "noColored") continue;
                    rend.material = normalMat;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        //EXIT TO MENU
    }

    public static void StartGameTimer()
    {
        instance.StartCoroutine(instance.GameTimer());
        instance.playerMovement.GoAwakeStopSleep();
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
