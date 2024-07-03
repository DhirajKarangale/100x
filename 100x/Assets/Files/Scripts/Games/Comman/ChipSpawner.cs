using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChipSpawner : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] internal Chip[] chips;
    [SerializeField] internal Transform[] users;
    [SerializeField] internal Transform[] containers;

    [Header("Attributes")]
    [SerializeField] float duration;
    [SerializeField] Image chipPrefab;

    private List<Transform> chipsSpawned;


    private void Start()
    {
        chipsSpawned = new List<Transform>();
        ResetData();
    }


    private IEnumerator IEMoveChip(Transform chip, Vector3 endPos, bool isDestroy)
    {
        if (chip == null) yield break;
        Vector3 stPos = chip.position;
        float startTime = Time.time;

        if (chip == null) yield break;
        while (chip.position != endPos)
        {
            float t = (Time.time - startTime) / duration;
            chip.transform.position = Vector3.Lerp(stPos, endPos, t);

            yield return null;
        }

        if (isDestroy && chip.gameObject) Destroy(chip.gameObject, 0.1f);
    }


    internal void ResetData()
    {
        if (chipsSpawned.Count <= 0) return;

        foreach (Transform chip in chipsSpawned)
        {
            StartCoroutine(IEMoveChip(chip, containers[0].position, true));
        }
    }

    internal void SpawnChip(Chip dvtChip, Vector3 stPos, Vector3 endPos, float offset = 1)
    {
        Image chip = Instantiate(chipPrefab, stPos, Quaternion.identity);
        chip.transform.SetParent(transform);
        chip.transform.position = stPos;
        chip.transform.localScale = Vector3.one;
        chipsSpawned.Add(chip.transform);

        chip.sprite = dvtChip.sprite;
        TMP_Text chipTxt = chip.GetComponentInChildren<TMP_Text>();
        if (chipTxt) chipTxt.text = dvtChip.chipName;

        Vector3 targetPos = endPos;
        targetPos += (new Vector3(Random.Range(-1f, 1f), Random.Range(-0.75f, 0.75f), 0) * offset);

        StartCoroutine(IEMoveChip(chip.transform, targetPos, false));
    }

    internal void SpawnChip(int cntContainer, float offset = 1)
    {
        SoundManager soundManager = SoundManager.instance;
        if (soundManager) soundManager.Play(soundManager.clipCoin);

        for (int i = 0; i < Random.Range(3, 10); i++)
        {
            Transform endContainer = containers[Random.Range(1, cntContainer + 1)];
            SpawnChip(chips[Random.Range(0, chips.Length)], containers[0].position, endContainer.position, offset);
        }

        if (users != null && users.Length > 0)
        {
            for (int i = 0; i < Random.Range(3, 7); i++)
            {
                Transform endContainer = containers[Random.Range(1, cntContainer + 1)];
                SpawnChip(chips[Random.Range(0, chips.Length)], users[Random.Range(0, users.Length)].position, endContainer.position, offset);
            }
        }
    }
}
