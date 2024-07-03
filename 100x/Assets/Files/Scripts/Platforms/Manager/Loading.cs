using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : PersistentSingleton<Loading>
{
    [SerializeField] Image obj;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text txtMsg;
    [SerializeField] TMP_Text txtProgress;
    [SerializeField] Sprite[] sprites;


    private void Start()
    {
        Active();
    }


    private IEnumerator IELoadLevel(int scene, int extraTime, string msg, int param, Action<int> callBack)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);

        float progress = 0;

        while (!loadOperation.isDone)
        {
            progress = Mathf.Clamp01(loadOperation.progress / 0.9f) / 2;
            UpdateProgress(progress, msg);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.1f);
        callBack?.Invoke(param);

        while (progress < 1)
        {
            progress += (0.5f / extraTime);
            UpdateProgress(progress, msg);
            yield return new WaitForSecondsRealtime(1);
        }

        UpdateProgress(1, msg);
        yield return new WaitForSecondsRealtime(0.5f);
        Disable();
    }


    internal void UpdateProgress(float val, string msg)
    {
        if (!obj.gameObject.activeInHierarchy) obj.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        obj.gameObject.SetActive(true);
        txtMsg.text = msg;
        slider.value = val;
        txtProgress.text = (val * 100).ToString("F0") + "%";
    }

    internal void Disable()
    {
        StopAllCoroutines();
        txtMsg.text = "";
        txtProgress.text = "";
        slider.value = 0;
        obj.gameObject.SetActive(false);
    }

    internal void Active()
    {
        txtMsg.text = "";
        txtProgress.text = "";
        slider.value = 0;

        if (!obj.gameObject.activeInHierarchy) obj.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        obj.gameObject.SetActive(true);
    }

    internal void LoadLevel(int scene, int extraTime, string msg, int param, Action<int> callBack)
    {
        StartCoroutine(IELoadLevel(scene, extraTime, msg, param, callBack));
    }
}