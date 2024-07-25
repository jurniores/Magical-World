using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Omni.Threading.Tasks;
public class CanvasGo : ServiceBehaviour
{
    [SerializeField]
    private Image imageload1;
    [SerializeField]
    private GameObject Canvas1;

    protected override void OnStart()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async UniTask Go(int scene)
    {
        gameObject.SetActive(true);
        Canvas1.SetActive(false);
        await Utils.FakeLoad(LoadSceneLogado);
        await NetworkManager.LoadSceneAsync(scene, LoadSceneMode.Additive).ToUniTask(Progress.Create<float>(LoadSceneLogado));
    }

    private void LoadSceneLogado(float progress)
    {
        float newP = Mathf.Clamp01(progress / 0.9f);
        if (newP < imageload1.fillAmount) return;

        imageload1.fillAmount = newP;
    }
}
