using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObraDinnFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class ObraDinnSettings
    {
        public Material ditherMat;
        public Material thresholdMat;
    }

    public ObraDinnSettings settings = new ObraDinnSettings();

    private ObraDinnPass m_ObraDinnPass;

    public override void Create()
    {
        m_ObraDinnPass = new ObraDinnPass(settings.ditherMat, settings.thresholdMat);

        m_ObraDinnPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.ditherMat == null || settings.thresholdMat == null)
        {
            Debug.LogWarning("Obra Dinn materials ไม่ได้ตั้งค่าใน Feature");
            return;
        }

        renderer.EnqueuePass(m_ObraDinnPass);
    }
}
