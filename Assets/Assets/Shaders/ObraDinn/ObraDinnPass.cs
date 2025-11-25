using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObraDinnPass : ScriptableRenderPass
{
    private Material m_DitherMat;
    private Material m_ThresholdMat;

    private int m_LargeTempID;
    private int m_MainTempID;

    private Vector3[] m_FrustumCorners = new Vector3[4];

    public ObraDinnPass(Material ditherMat, Material thresholdMat)
    {
        m_DitherMat = ditherMat;
        m_ThresholdMat = thresholdMat;

        m_LargeTempID = Shader.PropertyToID("_LargeTemp");
        m_MainTempID = Shader.PropertyToID("_MainTemp");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("ObraDinn Post Effect");

        Camera cam = renderingData.cameraData.camera;
        Transform camTransform = cam.transform;

        var sourceIdentifier = renderingData.cameraData.renderer.cameraColorTargetHandle;

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, m_FrustumCorners);

        for (int i = 0; i < 4; i++)
        {
            m_FrustumCorners[i] = camTransform.TransformVector(m_FrustumCorners[i]);
            m_FrustumCorners[i].Normalize();
        }

        m_DitherMat.SetVector("_BL", m_FrustumCorners[0]);
        m_DitherMat.SetVector("_TL", m_FrustumCorners[1]);
        m_DitherMat.SetVector("_TR", m_FrustumCorners[2]);
        m_DitherMat.SetVector("_BR", m_FrustumCorners[3]);

        descriptor.width = 1640;
        descriptor.height = 940;
        cmd.GetTemporaryRT(m_LargeTempID, descriptor, FilterMode.Bilinear);

        descriptor.width = 820;
        descriptor.height = 470;
        cmd.GetTemporaryRT(m_MainTempID, descriptor, FilterMode.Bilinear);

        cmd.Blit(sourceIdentifier, m_LargeTempID, m_DitherMat);

        cmd.Blit(m_LargeTempID, m_MainTempID, m_ThresholdMat);

        cmd.Blit(m_MainTempID, sourceIdentifier);

        cmd.ReleaseTemporaryRT(m_LargeTempID);
        cmd.ReleaseTemporaryRT(m_MainTempID);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
