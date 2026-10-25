﻿using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using SmootherTerrain.Settings;


namespace SmootherTerrain
{
    [UsedImplicitly]
    public class Mod : IMod
    {
        public static readonly ILog LOG = LogManager.GetLogger(nameof(SmootherTerrain)).SetShowsErrorsInUI(false);
        
        public static Setting Setting { get; private set; }
        
        
        private GameObject _mHookObject;

        public void OnLoad(UpdateSystem updateSystem)
        {

            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEn(Setting));
            Setting.RegisterKeyBindings();
            
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                LOG.Info($"Current mod asset at {asset.path}");

            _mHookObject = new GameObject("SmootherTerrain_Fixer");
            _mHookObject.AddComponent<TerrainShaderModifer>();
            Object.DontDestroyOnLoad(_mHookObject);
        }


        public class TerrainShaderModifer : MonoBehaviour
        {
            private int m_COTerrainNormalSampleRadius;

            private void Start()
            {
                // Get the global shader ID game uses
                m_COTerrainNormalSampleRadius = Shader.PropertyToID("colossal_TerrainNormalSampleRadius");

            }

            private void LateUpdate()
            {
                // Read what the TerrainRenderSystem just calculated
                var originalRadius = Shader.GetGlobalVector(m_COTerrainNormalSampleRadius);

                if (originalRadius != Vector4.zero)
                {
                    // Increase sampling radius
                    var smoothedRadius = originalRadius * (Setting.SmoothingFactor + 1);
                    
                    // Override it before the gpu draws it
                    Shader.SetGlobalVector(m_COTerrainNormalSampleRadius, smoothedRadius);
                }
            }
        }
        

        public void OnDispose()
        {
            LOG.Info(nameof(OnDispose));
            if (_mHookObject is not null) Object.Destroy(_mHookObject);
        }
    }
}