using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using GPUInstancer;

public class GpuMeshPrefab : MonoBehaviour
{
    // info
    public GPUInstancerPrefab prefab;
    public Vector3 posAdjust;
    public Vector3 scaleAdjust;
    public float randomRotation;
    public Color[] colors;

    // gpu instancing
    private int _bufferSize = 0;
    private int _count;
    private Matrix4x4[] _matrix4x4Array;
    private Vector4[] _colorVariation;
    private bool[] _matrixHole;
    private int _firstHole;
    private bool init = false;

    public void inizialize()
    {
        _bufferSize = 0;
        _count = 0;
        _matrix4x4Array = new Matrix4x4[0];
        _colorVariation = new Vector4[0];
        _matrixHole = new bool[0];
        _firstHole = 0;
        init = true;
    }

    public int addInstance(GpuMeshInstancer si, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
    {
        if (!init)
        {
            // inizialize
            inizialize();
            increaseBuffer(si);
        }
        else
        {
            if (findHole() < 0) increaseBuffer(si);
        }
        _count++;
        int i = addToMatrix(si, position, rotation, scale, color);
        return i;
    }

    public void removeInstance(GpuMeshInstancer si, int index)
    {
        _count--;

        _matrix4x4Array[index] = Matrix4x4.zero;
        _colorVariation[index] = Color.clear;
        _matrixHole[index] = false;

        GPUInstancerAPI.UpdateVisibilityBufferWithMatrix4x4Array(si.prefabManager, prefab.prefabPrototype, _matrix4x4Array,
            index, index, 1);
        GPUInstancerAPI.UpdateVariationFromArray(si.prefabManager, prefab.prefabPrototype, "colorBuffer", _colorVariation,
            index, index, 1);
        GPUInstancerAPI.SetInstanceCount(si.prefabManager, prefab.prefabPrototype, _count);
    }

    private void increaseBuffer(GpuMeshInstancer si)
    {
        //
        _firstHole = _bufferSize;
        if (_bufferSize == 0) _bufferSize = 5;
        _bufferSize = _bufferSize * 2;
        // update arrays
        Matrix4x4[] _tempMatrix = new Matrix4x4[_bufferSize];
        Vector4[] _tempColors = new Vector4[_bufferSize];
        bool[] _tempHoles = new bool[_bufferSize];
        for (int i = 0; i < _firstHole; i++)
        {
            // copy the half to new one
            _tempMatrix[i] = _matrix4x4Array[i];
            _tempColors[i] = _colorVariation[i];
            _tempHoles[i] = _matrixHole[i];
        }
        _matrix4x4Array = _tempMatrix;
        _colorVariation = _tempColors;
        _matrixHole = _tempHoles;
        // initialize the buffers with array
        GPUInstancerAPI.InitializeWithMatrix4x4Array(si.prefabManager, prefab.prefabPrototype, _matrix4x4Array);
        GPUInstancerAPI.SetInstanceCount(si.prefabManager, prefab.prefabPrototype, _count);
        GPUInstancerAPI.DefineAndAddVariationFromArray(si.prefabManager, prefab.prefabPrototype, "colorBuffer", _colorVariation);
        // 

    }

    private int addToMatrix(GpuMeshInstancer si, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
    {
        _firstHole = findHole();
        _matrix4x4Array[_firstHole] = Matrix4x4.TRS(position, rotation,
            new Vector3(scaleAdjust.x * scale.x, scaleAdjust.y * scale.y, scaleAdjust.z * scale.z));
        _colorVariation[_firstHole] = color;
        _matrixHole[_firstHole] = true;
        //Debug.Log(_bufferSize + " " + _count + " " + _firstHole);
        GPUInstancerAPI.UpdateVisibilityBufferWithMatrix4x4Array(si.prefabManager, prefab.prefabPrototype, _matrix4x4Array,
            _firstHole, _firstHole, 1);
        GPUInstancerAPI.UpdateVariationFromArray(si.prefabManager, prefab.prefabPrototype, "colorBuffer", _colorVariation,
            _firstHole, _firstHole, 1);
        GPUInstancerAPI.SetInstanceCount(si.prefabManager, prefab.prefabPrototype, _count);

        return _firstHole;
    }

    private int findHole()
    {
        for (int i = 0; i < _matrixHole.Length; i++)
        {
            if (!_matrixHole[i])
            {
                _firstHole = i;
                return i;
            }
        }
        return -1;
    }
}*/
