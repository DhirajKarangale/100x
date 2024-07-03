using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class AddressablesManager : PersistentSingleton<AddressablesManager>
{
    [SerializeField] AssetReference[] references;
    internal bool isDownloaded;
    private AsyncOperationHandle<GameObject>[] datas;


    private void Start()
    {
        datas = new AsyncOperationHandle<GameObject>[references.Length];
        // StartCoroutine(IEDownload());
    }


    private IEnumerator IEDownload()
    {
        for (int i = 0; i < references.Length; i++)
        {
            datas[i] = references[i].LoadAssetAsync<GameObject>();

            while (!datas[i].IsDone)
            {
                string msg = $"Downloading resourses {i + 1}/{references.Length}";
                float progress = datas[i].GetDownloadStatus().Percent / (references.Length - i);
                Loading.instance.UpdateProgress(progress, msg);
                yield return null;
            }

            yield return datas[i];
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Loading.instance.Disable();
        isDownloaded = true;
    }

    private IEnumerator IEDisableLoading()
    {
        yield return new WaitForSecondsRealtime(1);
        Loading.instance.Disable();
    }

    private void Load(int game)
    {
        if (datas[game].IsValid() && datas[game].Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(datas[game].Result);
            StopAllCoroutines();
            StartCoroutine(IEDisableLoading());
        }
        else
        {
            datas[game] = references[game].LoadAssetAsync<GameObject>();
            datas[game].Completed += OnLoadComplete;
        }
    }

    private void OnLoadComplete(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(operation.Result);
            StopAllCoroutines();
            StartCoroutine(IEDisableLoading());
        }
    }

    internal void ButtonGame(int game)
    {
        string msg = "Loading Game...";
        Loading.instance.LoadLevel(2, 25, msg, game, Load);
    }
}

// https://100-x.s3.ap-south-1.amazonaws.com/Android/