using UnityEngine;

public class BaseEffect : MonoBehaviour {

    public float selfDestroyTime = 2.5f;

    void Awake()
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] sharedMs = renderers[i].sharedMaterials;
            for (int j = 0; j < sharedMs.Length; j++)
            {
                Shader shader = Shader.Find(sharedMs[j].shader.name);
                sharedMs[j].shader = shader;
            }
        }

        LeanTween.delayedCall(selfDestroyTime, SelfDestroy);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
