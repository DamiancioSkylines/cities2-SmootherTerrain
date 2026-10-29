using Game;
using Game.Modding;
using Game.SceneFlow;
using UnityEngine;
using Colossal.Logging;
using Colossal.IO.AssetDatabase;
using Object = UnityEngine.Object;
using JetBrains.Annotations;
using SmootherTerrain.Settings;

namespace SmootherTerrain
{
    [UsedImplicitly]
    public class Mod : IMod
    {
        private static readonly ILog LOG = LogManager.GetLogger(nameof(SmootherTerrain)).SetShowsErrorsInUI(false);

        private static Setting Setting { get; set; }
        
        private GameObject _mHookObject;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEn(Setting));
            Setting.RegisterKeyBindings();
            AssetDatabase.global.LoadSettings(nameof(SmootherTerrain), Setting, new Setting(this));
            
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset)) LOG.Info($"Current mod asset at {asset.path}");
            
            _mHookObject = new GameObject("SmootherTerrain_Fixer");
            _mHookObject.AddComponent<TerrainShaderModifer>();
            Object.DontDestroyOnLoad(_mHookObject);
        }
        
        public class TerrainShaderModifer : MonoBehaviour
        {
            private int _mCoTerrainNormalSampleRadius;

            private void Start()
            {
                // Get the global shader ID game uses
                _mCoTerrainNormalSampleRadius = Shader.PropertyToID("colossal_TerrainNormalSampleRadius");
            }

            private void LateUpdate()
            {
                // Read TerrainRenderSystem normal sample radius
                var originalRadius = Shader.GetGlobalVector(_mCoTerrainNormalSampleRadius);

                if (originalRadius != Vector4.zero)
                {
                    // Increase sampling radius
                    var smoothedRadius = originalRadius * (Setting.SmoothingFactor + 1);
                    
                    // Override it before the gpu draws it
                    Shader.SetGlobalVector(_mCoTerrainNormalSampleRadius, smoothedRadius);
                }
            }
        }

        public void OnDispose()
        {
            // LOG.Info(nameof(OnDispose));
            if (_mHookObject is not null) Object.Destroy(_mHookObject);
            
            if (Setting is not null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }
        }
    }
}