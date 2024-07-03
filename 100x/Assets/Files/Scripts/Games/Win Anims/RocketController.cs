using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    [SerializeField] CrUserCashout userCashout;
    [SerializeField] CrBetManager betManager;

    [Header("Score")]
    [SerializeField] TMP_Text txtScore;
    [SerializeField] float scoreStep;
    private float currScoreStep;
    internal float currScore;
    private float score;

    [Header("Rocket")]
    [SerializeField] GameObject objRocket;
    [SerializeField] Transform rocket;
    [SerializeField] Transform stPos;
    [SerializeField] Transform endPos;
    [SerializeField] Transform controlPoint;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem ps;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float duration = 2f;
    private float elapsedTime = 0f;

    [Header("Graph Vertical")]
    [SerializeField] Transform graphVertical;
    [SerializeField] Transform stPosVertical;
    [SerializeField] Transform endPosVertical;
    [SerializeField] float verticalDuration;
    private float currVerticalDuration;
    private float verticalTime;

    [Header("Graph Horrizontal")]
    [SerializeField] Transform graphHorrizontal;
    [SerializeField] Transform stPosHorrizontal;
    [SerializeField] Transform endPosHorrizontal;
    [SerializeField] float horrizontalDuration;
    private float currHorrizontalDuration;

    internal bool isStarted;
    private float horrizontalTime;


    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (isStarted) Begin();
    }


    private void InStop()
    {
        CancelInvoke();
        Invoke(nameof(End), 2);
    }

    private void End()
    {
        GameManager.OnFinishWinAnim?.Invoke();
    }

    private void Begin()
    {
        MoveRocket();
        CalculateScore();
    }

    private void CalculateScore()
    {
        betManager.UpdateCash();
        txtScore.text = currScore.ToString("F2") + "x";
        currScore += (Time.deltaTime / currScoreStep);
        if (currScore >= score) Stop();

        if (Random.value > 0.99f && currScore > 0.7f) userCashout.CashOut();
    }

    private void Stop()
    {
        ps.transform.position = rocket.position;
        objRocket.SetActive(false);
        ps.Play();
        audioSource.Play();

        isStarted = false;
        CancelInvoke();
        Invoke(nameof(InStop), 0.5f);
    }

    private void MoveGraph()
    {
        MoveVertical();
        MoveHorrizontal();
    }

    private void MoveRocket()
    {
        elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime, 0f, duration);
        float t = elapsedTime / duration;
        rocket.position = CalculateBezierPoint(t, stPos.position, controlPoint.position, endPos.position);

        if (elapsedTime == duration) MoveGraph();
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * endPos.position;

        return p;
    }

    private void MoveVertical()
    {
        verticalTime = Mathf.Clamp(verticalTime + Time.deltaTime, 0f, currVerticalDuration);
        float t = verticalTime / currVerticalDuration;
        graphVertical.position = Vector3.Lerp(stPosVertical.position, endPosVertical.position, t);
    }

    private void MoveHorrizontal()
    {
        horrizontalTime = Mathf.Clamp(horrizontalTime + Time.deltaTime, 0f, currHorrizontalDuration);
        float t = horrizontalTime / currHorrizontalDuration;
        graphHorrizontal.position = Vector3.Lerp(stPosHorrizontal.position, endPosHorrizontal.position, t);
    }


    internal void Begin(float value, float threadId)
    {
        Reset();

        score = value;
        isStarted = true;
        trail.time = 100;

        // currScoreStep = scoreStep * 7 / (threadId - 1);
        // currVerticalDuration = verticalDuration * 7 / (threadId - 1);
        // currHorrizontalDuration = horrizontalDuration * 7 / (threadId - 1);

        // s1t1 = s2t2
        // s2 = s1t1 / t2

        float orgTime = (5 * value);
        float currTime = orgTime - 2 * (7 - threadId);
        currScoreStep = scoreStep * orgTime / currTime;
        currVerticalDuration = verticalDuration * orgTime / currTime;
        currHorrizontalDuration = horrizontalDuration * orgTime / currTime;
    }

    internal void Reset()
    {
        isStarted = false;

        currScoreStep = scoreStep;
        currHorrizontalDuration = horrizontalDuration;
        currVerticalDuration = verticalDuration;
        currScore = 0;
        elapsedTime = 0;
        horrizontalTime = 0;
        verticalTime = 0;

        trail.time = 0;

        ps.Stop();
        objRocket.SetActive(true);
        rocket.position = stPos.position;
        graphVertical.position = stPosVertical.position;
        graphHorrizontal.position = stPosHorrizontal.position;
        txtScore.text = "0x";
        betManager.UpdateCash();

        rocket.gameObject.SetActive(true);
    }
}