using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Animator blackScreen;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timerNotifyText;
    [SerializeField] private Animator timerAnimator;
    [SerializeField] private float watchingTime;
    [SerializeField] private float attackTime;
    [SerializeField] private int enemyCount;
    [SerializeField] private float playingTime;

    [SerializeField] private int maxWrongItems;
    [SerializeField] private Material wrongMat;
    [SerializeField] private Material normalMat;

    [SerializeField] private Animator king;
    [SerializeField] private Transform kingTargetPoint;

    [SerializeField] public LayerMask interactableObjects { get; private set; }
    [SerializeField] public int interactableLayerInt { get; private set; }
    [SerializeField] public Vector4 roomBorders { get; private set; }

    private PlayerMovement playerMovement;
    private PlayableDirector playableDirector;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        playerMovement = FindObjectOfType<PlayerMovement>();
        playableDirector = GetComponent<PlayableDirector>();
        StartCoroutine(LoadSceneFirst());
    }

    private IEnumerator LoadSceneFirst()
    {
        blackScreen.SetTrigger("Hide");
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(StartWatchingTime());
    }

    private IEnumerator StartWatchingTime()
    {
        playerMovement.canMove = false;
        yield return new WaitForSeconds(1f);
        playableDirector.Play();
        yield return new WaitForSeconds(3f);

        king.SetBool("run", true);

        float mvSpeed = Vector3.Distance(king.transform.position, kingTargetPoint.position) / (watchingTime - 5);
        Vector3 dir = (kingTargetPoint.position - king.transform.position).normalized;
        while (Vector3.Distance(king.transform.position, kingTargetPoint.position) > 0.3f)
        {
            king.transform.position += dir * mvSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(king.gameObject);

        yield return new WaitForSeconds(2f);
        playerMovement.GoSleep();

        EnemyController.instance.StartCoroutine(EnemyController.instance.SpawnEnemies(enemyCount, attackTime));
    }

    private IEnumerator GameTimer()
    {
        timerAnimator.SetTrigger("Show");
        timerText.text = playingTime.ToString();
        float timeLeft = playingTime;

        while (timeLeft > 0)
        {
            timerText.text = ((int)timeLeft).ToString();
            timeLeft -= timeLeft <= 3 ? Time.deltaTime * 0.75f : Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        List<Transform> wrongObjects;

        if (ObjectsController.CheckForWin(out wrongObjects, maxWrongItems))
        {
            if (wrongObjects.Count == 0)
            {
                timerNotifyText.text = "<color=orange>PERFECT!</color>";
                timerText.text = "";
                yield return new WaitForSeconds(3f);
            }
            else
            {
                timerNotifyText.text = "<color=green>VICTORY!</color> (" + wrongObjects.Count.ToString() + "/" + maxWrongItems.ToString() + " wrong)";
                timerText.text = "";

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
        }
        else
        {
            //Lost. Highlight wrong objects
            timerNotifyText.text = "<color=red>DEFEAT</color> (" + wrongObjects.Count.ToString() + "/" + maxWrongItems.ToString() + " wrong)";
            timerText.text = "";
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

        yield return new WaitForSeconds(3f);
        blackScreen.SetTrigger("Show");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
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
