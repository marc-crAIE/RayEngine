using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Scenes
{
    public static class SceneManager
    {
        private static Scene? ActiveScene = null;

        public static void LoadScene(Scene scene) => ActiveScene = scene;
        public static Scene? GetScene() => ActiveScene;
    }
}
