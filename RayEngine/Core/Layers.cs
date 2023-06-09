﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Core
{
    public struct Layer
    {
        private int LayerID;
        private string Name;
        private Layers? Owner;

        internal Layer(Layers? owner, int layerID, string name = "")
        {
            Owner = owner;
            LayerID = layerID;
            Name = name;
        }

        public void Enable(bool enabled = true) => Owner?.SetLayer(LayerID, enabled);
        public bool IsEnabled() => Owner is not null ? Owner.IsLayerEnabled(LayerID) : false;

        public int GetID() => LayerID;
        public string GetName() => Name;
        public Layers? GetOwner() => Owner;
    }

    public class Layers
    {
        private uint EnabledLayers = 0b1;
        private Dictionary<string, int> LayerNames = new Dictionary<string, int>();

        private const int MaxLayers = 32;

        public Layers()
        {
            AddLayerName("Default", 0);
            AddLayerName("ImGUI", 1);
        }

        public Layer AddLayerName(string name, int layerID)
        {
            if (!ValidLayer(layerID))
            {
                // TODO: MAKE A FRIGGEN LOGGER
                Console.WriteLine($"Layer {layerID} is invalid!");
                return new Layer();
            }
            LayerNames.Add(name, layerID);
            return new Layer(this, layerID);
        }

        public void SetLayer(int layer, bool enabled)
        {
            if (!ValidLayer(layer))
            {
                // TODO: MAKE A FRIGGEN LOGGER
                Console.WriteLine($"Layer {layer} is invalid!");
                return;
            }
            if (enabled)
                EnabledLayers |= (uint)(1 << layer);
            else
                EnabledLayers &= ~(uint)(1 << layer);
        }

        public bool IsLayerEnabled(int layer)
        {
            if (!ValidLayer(layer))
            {
                // TODO: MAKE A FRIGGEN LOGGER
                Console.WriteLine($"Layer {layer} is invalid!");
                return false;
            }
            return ((EnabledLayers >> layer) & 0b1) == 1;
        }

        public bool IsLayerEnabled(string name)
        {
            if (!LayerNames.ContainsKey(name))
            {
                Console.WriteLine($"Layer \"{name}\" is invalid!");
                return false;
            }
            return ((EnabledLayers >> LayerNames[name]) & 0b1) == 1;
        }

        public Layer GetLayer(int layer)
        {
            if (!ValidLayer(layer))
            {
                Console.WriteLine($"Layer {layer} is invalid!");
                return new Layer(null, -1);
            }
            string name = LayerNames.FirstOrDefault(x => x.Value == layer).Key ?? layer.ToString();
            return new Layer(this, layer, name);
        }
        public Layer GetLayer(string name)
        {
            if (!LayerNames.ContainsKey(name))
            {
                Console.WriteLine($"Layer \"{name}\" is invalid!");
                return new Layer(null, -1);
            }
            return new Layer(this, LayerNames[name], name);
        }

        public bool ValidLayer(int layer) => layer >= 0 && layer < MaxLayers;
    }
}
