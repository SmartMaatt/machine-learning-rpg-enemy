using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using System.IO;
using Unity.Barracuda.ONNX;
using Unity.MLAgents;
using Unity.MLAgents.Policies;

public class BrainModelReader
{
    public void OverrideModel(Agent m_Agent, string assetPath, string assetName, bool isOnnx)
    {
        bool overrideOk = false;
        string overrideError = null;

        m_Agent.LazyInitialize();

        NNModel nnModel = null;
        try
        {
            nnModel = GetModelForBehaviorName(assetPath, assetName, isOnnx);
        }
        catch (Exception e)
        {
            overrideError = $"Exception calling GetModelForBehaviorName: {e}";
        }

        if (nnModel == null)
        {
            if (string.IsNullOrEmpty(overrideError))
            {
                overrideError =
                    $"Didn't find a model for behaviorName {assetName}. Make " +
                    "sure the behaviorName is set correctly in the commandline " +
                    "and that the model file exists";
            }
        }
        else
        {
            var modelName = nnModel != null ? nnModel.name : "<null>";
            Debug.Log($"Overriding behavior {assetName} for agent with model {modelName}");
            try
            {
                m_Agent.SetModel(assetName, nnModel);
                overrideOk = true;
            }
            catch (Exception e)
            {
                overrideError = $"Exception calling Agent.SetModel: {e}";
            }
        }
    }

    public NNModel GetModelForBehaviorName(string assetPath, string assetName, bool isOnnx)
    {
        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.Log("No override directory set!");
            return null;
        }

        byte[] rawModel = null;
        try
        {
            rawModel = File.ReadAllBytes(assetPath);
        }
        catch (IOException)
        {
            Debug.LogError("Couldn't load file " + assetPath);
        }
        
        if (rawModel == null)
        {
            return null;
        }

        var asset = isOnnx ? LoadOnnxModel(rawModel) : LoadBarracudaModel(rawModel);
        asset.name = assetName;
        return asset;
    }

    NNModel LoadBarracudaModel(byte[] rawModel)
    {
        var asset = ScriptableObject.CreateInstance<NNModel>();
        asset.modelData = ScriptableObject.CreateInstance<NNModelData>();
        asset.modelData.Value = rawModel;
        return asset;
    }

    NNModel LoadOnnxModel(byte[] rawModel)
    {
        var converter = new ONNXModelConverter(true);
        var onnxModel = converter.Convert(rawModel);

        NNModelData assetData = ScriptableObject.CreateInstance<NNModelData>();
        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            ModelWriter.Save(writer, onnxModel);
            assetData.Value = memoryStream.ToArray();
        }
        assetData.name = "Data";
        assetData.hideFlags = HideFlags.HideInHierarchy;

        var asset = ScriptableObject.CreateInstance<NNModel>();
        asset.modelData = assetData;
        return asset;
    }

    
}