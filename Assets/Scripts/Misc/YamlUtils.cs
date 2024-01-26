using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class YamlUtils : MonoBehaviour
{
    public TextAsset dialogLines;
    private void Start()
    {
        ParseData();
    }

    public void ParseData()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        
        var p = deserializer.Deserialize<List<Dialog>>(dialogLines.text);
        p.ForEach(item => Debug.LogError(item.Condition + item.Line));
    }
}
