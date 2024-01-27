﻿using System;
using System.Collections.Generic;
using Eflatun.SceneReference;

namespace Game.Scenes
{
    public class Scene
    {
        private List<SceneReference> _otherScenes;

        public SceneReference MainScene { get; }

        public SceneReference[] OtherScenes
        {
            get => _otherScenes.ToArray();
            set => _otherScenes = new List<SceneReference>(value);
        }

        public Scene(SceneReference mainScene)
        {
            MainScene = mainScene;
            OtherScenes = Array.Empty<SceneReference>();
        }

        public Scene(SceneReference mainScene, SceneReference[] otherScenes)
        {
            MainScene = mainScene;
            OtherScenes = otherScenes;
        }

        public Scene (SceneReference mainScene, ICollection<SceneReference> otherScenes)
        {
            MainScene = mainScene;
            _otherScenes = new List<SceneReference>(otherScenes.Count);
            foreach (SceneReference otherScene in otherScenes)
                _otherScenes.Add(otherScene);
        }

        public static bool operator ==(Scene a, Scene b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;

            if (a.MainScene.BuildIndex != b.MainScene.BuildIndex)
                return false;

            if (a._otherScenes.Count != b._otherScenes.Count)
                return false;
            for (int i = 0; i < a._otherScenes.Count; i++) {
                SceneReference otherScene = a._otherScenes[i];
                if (a._otherScenes[i].BuildIndex != b._otherScenes[i].BuildIndex)
                    return false;
            }

            return true;
        }

        public static bool operator != (Scene a, Scene b)
        {
            return !(a == b);
        }

        private bool Equals (Scene other)
        {
            return Equals(_otherScenes, other._otherScenes) && Equals(MainScene, other.MainScene);
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((Scene)obj);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine(OtherScenes, MainScene);
        }
    }
}
