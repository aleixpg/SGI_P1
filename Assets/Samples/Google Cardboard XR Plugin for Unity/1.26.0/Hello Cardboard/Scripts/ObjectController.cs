//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    private Material[] _originalMaterials; // Store the original materials
    private Renderer _myRenderer;
    private Color _originalColor;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _myRenderer = GetComponent<Renderer>();

        // Store the original color of the material.
        if (_myRenderer != null)
        {
            // Clone the original materials to preserve their initial state
            _originalMaterials = new Material[_myRenderer.materials.Length];
            for (int i = 0; i < _myRenderer.materials.Length; i++)
            {
                _originalMaterials[i] = new Material(_myRenderer.materials[i]); // Create a new instance to keep original properties
            }
        }

        SetMaterial(false);
    }

    /// <summary>
    /// Sets this instance's material or color according to gazedAt status.
    /// </summary>
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        if (_myRenderer != null)
        {
            if (gazedAt)
            {
                // Set all materials' color to green
                foreach (Material material in _myRenderer.materials)
                {
                    material.color = Color.green;
                }
            }
            else
            {
                // Restore the original materials
                _myRenderer.materials = _originalMaterials;
            }
        }
    }


    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnterXR()
    {
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExitXR()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClickXR()
    {
        DisableObject();
    }
}
