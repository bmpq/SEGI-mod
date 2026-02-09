using UnityEngine;

[CreateAssetMenu(fileName = "SEGIAssets", menuName = "SEGI/Asset Container")]
public class SEGIAssets : ScriptableObject
{
    [Space(10)]
    public Shader sunDepthShader;
    public Shader voxelizationShader;
    public Shader voxelTracingShader;
    public Shader mainSegiShader;

    [Space(10)]
    public ComputeShader clearCompute;
    public ComputeShader transferIntsCompute;
    public ComputeShader mipFilterCompute;

    [Space(10)]
    public Texture2D[] blueNoiseTextures;
}
