using ImGuiNET;
using RayEngine.Core;
using RayEngine.Debug;
using RayEngine.GameObjects;
using RayEngine.GameObjects.Components;
using RayEngine.Graphics;
using RayEngine.ImGUI;
using RayEngine.Scenes;
using Sandbox.Assets.Scripts;
using SharpMaths;
using static System.Formats.Asn1.AsnWriter;

namespace Sandbox
{
    internal class SandboxApp : Application
    {
        private GameObject? SelectedGameObject = null;

        public SandboxApp(ApplicationSpecification specification) : base(specification)
        {
            using var _it = Profiler.Function();

            Scene scene = new Scene();
            SceneManager.LoadScene(scene);

            GameObject manager = new GameObject("Manager");
            manager.AddComponent<ScriptComponent>().Bind<ManagerScript>();

            GameObject parent = new GameObject("Parent");
            GameObject child1 = new GameObject("Child 1");
            GameObject child2 = new GameObject("Child 2");
            GameObject child3 = new GameObject("Child 3");

            child1.SetParent(ref parent);
            child2.SetParent(ref child1);
            child3.SetParent(ref child2);

            //parent.AddComponent<SpriteComponent>(new Colour(255, 0, 255, 255));
            var parentTransform = parent.GetComponent<TransformComponent>();
            parentTransform.Translation = new Vector3(10, 10, 0);
            parentTransform.Scale = new Vector3(30.0f);

            parent.AddComponent<ScriptComponent>().Bind<ParentScript>();

            var child1Transform = child1.GetComponent<TransformComponent>();
            child1Transform.Translation = new Vector3(5, 7.5f, 0.0f);

            var child1Sprite = child1.AddComponent<SpriteComponent>(new Colour(255, 0, 0, 255));
            child1Sprite.Texture = new Texture2D("Assets/Textures/Raylib_icon.png");
            child1.AddComponent<ScriptComponent>().Bind<ChildScript>();

            var child2Transform = child2.GetComponent<TransformComponent>();
            child2Transform.Translation = new Vector3(2, 2, 0.0f);
            child2Transform.Rotation = SharpMath.ToRadians(new Vector3(0.0f, 0.0f, 45.0f));

            child2.AddComponent<SpriteComponent>(new Colour(0, 0, 255, 255));
            child2.AddComponent<ScriptComponent>().Bind<ChildScript>();

            var child3Transform = child3.GetComponent<TransformComponent>();
            child3Transform.Translation = new Vector3(3, 3, 0.0f);

            child3.AddComponent<SpriteComponent>(new Texture2D("Assets/Textures/Raylib_icon.png"));
            child3.AddComponent<ScriptComponent>().Bind<ChildScript>();

            scene.OnImGUIRender = OnImGUIRender;
        }

        #region ImGUI

        private float OverlayWidth = 0.0f;

        private void OnImGUIRender()
        {
            DrawOverlayWindow();
            DrawComponentsWindow();
        }

        #region Overlay Window

        private void DrawComponentsWindow()
        {
            if (SelectedGameObject is null)
                return;

            ImGuiWindowFlags overlayFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize
            | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;

            var viewport = ImGui.GetMainViewport();
            Vector2 windowPos = new Vector2(viewport.WorkPos.X + OverlayWidth + 20, viewport.WorkPos.Y + 10);
            ImGui.SetNextWindowPos(windowPos);
            ImGui.SetNextWindowViewport(viewport.ID);

            overlayFlags |= ImGuiWindowFlags.NoMove;

            ImGui.SetNextWindowBgAlpha(0.35f);
            ImGui.Begin("Components", overlayFlags);

            ImGui.Text($"UUID: {SelectedGameObject.GetID()}");
            ImGui.Text($"Tag: {SelectedGameObject.GetTag()}");

            //TODO: Make tags editable without freaking out the tree
            //ImGui.SameLine();
            //ImGui.InputText("##Tag", ref SelectedGameObject.GetComponent<TagComponent>().Tag, 32);

            if (SelectedGameObject.HasComponent<TransformComponent>())
            {
                ImGui.SeparatorText("Transform Component");

                ref var transform = ref SelectedGameObject.GetComponent<TransformComponent>();
                System.Numerics.Vector3 translation = transform.Translation;
                System.Numerics.Vector3 rotation = SharpMath.ToDegrees(transform.Rotation);
                System.Numerics.Vector3 scale = transform.Scale;

                if (ImGui.DragFloat3("Translation", ref translation, 0.1f))
                    transform.Translation = translation;
                if (ImGui.DragFloat3("Rotation", ref rotation, 0.1f))
                    transform.Rotation = SharpMath.ToRadians((Vector3)rotation);
                if (ImGui.DragFloat3("Scale", ref scale, 0.1f))
                    transform.Scale = scale;

                ImGui.Spacing();
            }

            if (SelectedGameObject.HasComponent<SpriteComponent>())
            {
                ImGui.SeparatorText("Sprite Component");

                ref var sprite = ref SelectedGameObject.GetComponent<SpriteComponent>();
                System.Numerics.Vector4 colour = (Vector4)sprite.Colour;
                if (ImGui.ColorEdit4("Colour", ref colour))
                    sprite.Colour = (Vector4)colour;

                if (sprite.Texture is not null)
                {
                    ImGuiContext.ImageSize(sprite.Texture, 128, 128);
                }

                ImGui.Spacing();
            }

            if (SelectedGameObject.HasComponent<ScriptComponent>())
            {
                ImGui.SeparatorText("Script Component");

                var instance = SelectedGameObject.GetComponent<ScriptComponent>().Instance;
                ImGui.Text($"{instance?.GetType()}");

                ImGui.Spacing();
            }

            ImGui.End();
        }

        private void DrawOverlayWindow()
        {
            ImGuiWindowFlags overlayFlags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize
                | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;

            var viewport = ImGui.GetMainViewport();
            Vector2 windowPos = new Vector2(viewport.WorkPos.X + 10, viewport.WorkPos.Y + 10);
            ImGui.SetNextWindowPos(windowPos);
            ImGui.SetNextWindowViewport(viewport.ID);

            overlayFlags |= ImGuiWindowFlags.NoMove;

            ImGui.SetNextWindowBgAlpha(0.35f);
            ImGui.Begin("Render Stats", overlayFlags);

            ImGui.Text("FPS:");
            ImGui.SameLine();
            ImGui.Text(Renderer.GetFPS().ToString());

            ImGui.Separator();

            ImGui.TreePush("Object Hierarchy");

            ImGui.TreePop();

            Scene? scene = SceneManager.GetScene();
            if (scene is not null)
            {
                int index = 0;
                foreach (GameObject gameObject in scene.GetGameObjects())
                {
                    DrawGameObjectTreeNode(gameObject, ref index);
                    index++;
                }
            }

            OverlayWidth = ImGui.GetWindowWidth();

            ImGui.End();
        }

        private void DrawGameObjectTreeNode(GameObject gameObject, ref int index)
        {
            if (ImGui.TreeNode(gameObject.GetTag() + $"##node{index}"))
            {
                index++;
                SelectedGameObject = gameObject;
                foreach (GameObject child in gameObject.GetChildren())
                {
                    DrawGameObjectTreeNode(child, ref index);
                }
                ImGui.TreePop();
            }
        }

        #endregion

        #endregion
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationSpecification spec = new ApplicationSpecification("Sandbox App");

            Profiler.BeginSession("Startup", "startup.json");

            SandboxApp app = new SandboxApp(spec);

            Profiler.EndSession();

            Profiler.BeginSession("Runtime", "runtime.json");

            app.Run();

            Profiler.EndSession();
        }
    }
}