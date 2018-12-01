using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class ResourceTest {

	[Test]
	public void ResourceTestSimplePasses() {
        int a = 3;
        int b = 7;
        float result = a *1f/ b;

        Debug.Log("result="+ Math.Round(result, 2));
    }
}
