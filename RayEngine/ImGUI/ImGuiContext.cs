using ImGuiNET;
using RayEngine.Core;
using RayEngine.Debug;
using Raylib_cs;
using SharpMaths;
using System.Runtime.InteropServices;

namespace RayEngine.ImGUI
{
    public class ImGuiContext
    {
        internal static IntPtr Context = IntPtr.Zero;
        private static ImGuiMouseCursor CurrentMouseCursor = ImGuiMouseCursor.COUNT;

        private static Dictionary<ImGuiMouseCursor, MouseCursor> MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
        private static Key[]? KeyMap;

        private static Graphics.Texture2D FontTexture;

        internal static void Setup(bool darkTheme = true)
        {
            using var _it = Profiler.Function();

            MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
            KeyMap = Enum.GetValues(typeof(Key)) as Key[];

            BeginImGuiInit();

            if (darkTheme)
                ImGui.StyleColorsDark();
            else
                ImGui.StyleColorsLight();

            EndImGuiInit();
        }

        internal static void BeginImGuiInit()
        {
            using var _it = Profiler.Function();

            Context = ImGui.CreateContext();
            var io = ImGui.GetIO();
        }

        internal static void EndImGuiInit()
        {
            using var _it = Profiler.Function();

            SetupMouseCursors();

            ImGui.SetCurrentContext(Context);
            ImGui.GetIO().Fonts.AddFontDefault();

            SetupKeyMap();
            ReloadFonts();
        }

        private static void SetupMouseCursors()
        {
            MouseCursorMap.Clear();
            MouseCursorMap[ImGuiMouseCursor.Arrow] =        MouseCursor.MOUSE_CURSOR_ARROW;
            MouseCursorMap[ImGuiMouseCursor.TextInput] =    MouseCursor.MOUSE_CURSOR_IBEAM;
            MouseCursorMap[ImGuiMouseCursor.Hand] =         MouseCursor.MOUSE_CURSOR_POINTING_HAND;
            MouseCursorMap[ImGuiMouseCursor.ResizeAll] =    MouseCursor.MOUSE_CURSOR_RESIZE_ALL;
            MouseCursorMap[ImGuiMouseCursor.ResizeEW] =     MouseCursor.MOUSE_CURSOR_RESIZE_EW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNESW] =   MouseCursor.MOUSE_CURSOR_RESIZE_NESW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNS] =     MouseCursor.MOUSE_CURSOR_RESIZE_NS;
            MouseCursorMap[ImGuiMouseCursor.ResizeNWSE] =   MouseCursor.MOUSE_CURSOR_RESIZE_NWSE;
            MouseCursorMap[ImGuiMouseCursor.NotAllowed] =   MouseCursor.MOUSE_CURSOR_NOT_ALLOWED;
        }

        private static unsafe void ReloadFonts()
        {
            using var _it = Profiler.Function();

            ImGui.SetCurrentContext(Context);
            ImGuiIOPtr io = ImGui.GetIO();

            int width, height, bytesPerPixel;
            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out width, out height, out bytesPerPixel);

            Image image = new Image
            {
                data = pixels,
                width = width,
                height = height,
                mipmaps = 1,
                format = PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
            };

            FontTexture = Raylib.LoadTextureFromImage(image);

            io.Fonts.SetTexID(new IntPtr(FontTexture.ID));
        }

        private static void SetupKeyMap()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] =          (int)Key.KEY_TAB;
            io.KeyMap[(int)ImGuiKey.LeftArrow] =    (int)Key.KEY_LEFT;
            io.KeyMap[(int)ImGuiKey.RightArrow] =   (int)Key.KEY_RIGHT;
            io.KeyMap[(int)ImGuiKey.UpArrow] =      (int)Key.KEY_UP;
            io.KeyMap[(int)ImGuiKey.DownArrow] =    (int)Key.KEY_DOWN;
            io.KeyMap[(int)ImGuiKey.PageUp] =       (int)Key.KEY_PAGE_UP;
            io.KeyMap[(int)ImGuiKey.PageDown] =     (int)Key.KEY_PAGE_DOWN;
            io.KeyMap[(int)ImGuiKey.Home] =         (int)Key.KEY_HOME;
            io.KeyMap[(int)ImGuiKey.End] =          (int)Key.KEY_END;
            io.KeyMap[(int)ImGuiKey.Delete] =       (int)Key.KEY_DELETE;
            io.KeyMap[(int)ImGuiKey.Backspace] =    (int)Key.KEY_BACKSPACE;
            io.KeyMap[(int)ImGuiKey.Enter] =        (int)Key.KEY_ENTER;
            io.KeyMap[(int)ImGuiKey.Escape] =       (int)Key.KEY_ESCAPE;
            io.KeyMap[(int)ImGuiKey.Space] =        (int)Key.KEY_SPACE;
            io.KeyMap[(int)ImGuiKey.A] =            (int)Key.KEY_A;
            io.KeyMap[(int)ImGuiKey.C] =            (int)Key.KEY_C;
            io.KeyMap[(int)ImGuiKey.V] =            (int)Key.KEY_V;
            io.KeyMap[(int)ImGuiKey.X] =            (int)Key.KEY_X;
            io.KeyMap[(int)ImGuiKey.Y] =            (int)Key.KEY_Y;
            io.KeyMap[(int)ImGuiKey.Z] =            (int)Key.KEY_Z;
        }

        private static void NewFrame()
        {
            using var _it = Profiler.Function();

            ImGuiIOPtr io = ImGui.GetIO();

            if (Raylib.IsWindowFullscreen())
            {
                int monitor = Raylib.GetCurrentMonitor();
                io.DisplaySize = new Vector2(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
            }
            else
            {
                io.DisplaySize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            io.DisplayFramebufferScale = new Vector2(1, 1);
            io.DeltaTime = Raylib.GetFrameTime();

            io.KeyCtrl = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL);
            io.KeyShift = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT);
            io.KeyAlt = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT);
            io.KeySuper = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER);

            if (io.WantSetMousePos)
            {
                Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);
            }
            else
            {
                io.MousePos = Raylib.GetMousePosition();
            }

            io.MouseDown[0] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);
            io.MouseDown[1] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON);
            io.MouseDown[2] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_MIDDLE_BUTTON);

            if (Raylib.GetMouseWheelMove() > 0)
                io.MouseWheel += 1;
            else if (Raylib.GetMouseWheelMove() < 0)
                io.MouseWheel -= 1;

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
            {
                ImGuiMouseCursor imgui_cursor = ImGui.GetMouseCursor();
                if (imgui_cursor != CurrentMouseCursor || io.MouseDrawCursor)
                {
                    CurrentMouseCursor = imgui_cursor;
                    if (io.MouseDrawCursor || imgui_cursor == ImGuiMouseCursor.None)
                    {
                        Raylib.HideCursor();
                    }
                    else
                    {
                        Raylib.ShowCursor();

                        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
                        {

                            if (!MouseCursorMap.ContainsKey(imgui_cursor))
                                Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                            else
                                Raylib.SetMouseCursor(MouseCursorMap[imgui_cursor]);
                        }
                    }
                }
            }
        }

        private static void FrameEvents()
        {
            using var _it = Profiler.Function();

            ImGuiIOPtr io = ImGui.GetIO();

            foreach (KeyboardKey key in KeyMap!)
            {
                io.KeysDown[(int)key] = Raylib.IsKeyDown(key);
            }

            uint pressed = (uint)Raylib.GetCharPressed();
            while (pressed != 0)
            {
                io.AddInputCharacter(pressed);
                pressed = (uint)Raylib.GetCharPressed();
            }
        }

        internal static void Begin()
        {
            using var _it = Profiler.Function();

            ImGui.SetCurrentContext(Context);

            NewFrame();
            FrameEvents();
            ImGui.NewFrame();
        }

        private static void EnableScissor(float x, float y, float width, float height)
        {
            Rlgl.rlEnableScissorTest();
            Rlgl.rlScissor((int)x, Raylib.GetScreenHeight() - (int)(y + height), (int)width, (int)height);
        }

        private static void TriangleVert(ImDrawVertPtr idx_vert)
        {
            Vector4 color = ImGui.ColorConvertU32ToFloat4(idx_vert.col);

            Rlgl.rlColor4f(color.x, color.y, color.z, color.w);
            Rlgl.rlTexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
            Rlgl.rlVertex2f(idx_vert.pos.X, idx_vert.pos.Y);
        }

        private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer, ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
        {
            using var _it = Profiler.Function();

            if (count < 3)
                return;

            uint textureId = 0;
            if (texturePtr != IntPtr.Zero)
                textureId = (uint)texturePtr.ToInt32();

            Rlgl.rlBegin(DrawMode.TRIANGLES);
            Rlgl.rlSetTexture(textureId);

            for (int i = 0; i <= (count - 3); i += 3)
            {
                if (Rlgl.rlCheckRenderBatchLimit(3))
                {
                    Rlgl.rlBegin(DrawMode.TRIANGLES);
                    Rlgl.rlSetTexture(textureId);
                }

                ushort indexA = indexBuffer[(int)indexStart + i];
                ushort indexB = indexBuffer[(int)indexStart + i + 1];
                ushort indexC = indexBuffer[(int)indexStart + i + 2];

                ImDrawVertPtr vertexA = vertBuffer[indexA];
                ImDrawVertPtr vertexB = vertBuffer[indexB];
                ImDrawVertPtr vertexC = vertBuffer[indexC];

                TriangleVert(vertexA);
                TriangleVert(vertexB);
                TriangleVert(vertexC);
            }
            Rlgl.rlEnd();
        }

        private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

        private static void RenderData()
        {
            using var _it = Profiler.Function();

            Rlgl.rlDrawRenderBatchActive();
            Rlgl.rlDisableBackfaceCulling();

            var data = ImGui.GetDrawData();

            for (int l = 0; l < data.CmdListsCount; l++)
            {
                ImDrawListPtr commandList = data.CmdListsRange[l];

                for (int cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
                {
                    var cmd = commandList.CmdBuffer[cmdIndex];

                    EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y, cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X), cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                    if (cmd.UserCallback != IntPtr.Zero)
                    {
                        Callback cb = Marshal.GetDelegateForFunctionPointer<Callback>(cmd.UserCallback);
                        cb(commandList, cmd);
                        continue;
                    }

                    RenderTriangles(cmd.ElemCount, cmd.IdxOffset, commandList.IdxBuffer, commandList.VtxBuffer, cmd.TextureId);

                    Rlgl.rlDrawRenderBatchActive();
                }
            }
            Rlgl.rlSetTexture(0);
            Rlgl.rlDisableScissorTest();
            Rlgl.rlEnableBackfaceCulling();
        }

        internal static void End()
        {
            using var _it = Profiler.Function();

            ImGui.SetCurrentContext(Context);
            ImGui.Render();
            RenderData();
        }

        internal static void Shutdown()
        {
            using var _it = Profiler.Function();

            Raylib.UnloadTexture(FontTexture);
            ImGui.DestroyContext();
        }

        public static void Image(Graphics.Texture2D image)
        {
            ImGui.Image(new IntPtr(image.ID), new Vector2(image.Width, image.Height));
        }

        public static void ImageSize(Graphics.Texture2D image, int width, int height)
        {
            ImGui.Image(new IntPtr(image.ID), new Vector2(width, height));
        }

        public static void ImageSize(Graphics.Texture2D image, Vector2 size)
        {
            ImGui.Image(new IntPtr(image.ID), size);
        }

        public static void ImageRect(Graphics.Texture2D image, int destWidth, int destHeight, Rectangle sourceRect)
        {
            Vector2 uv0 = new Vector2();
            Vector2 uv1 = new Vector2();

            if (sourceRect.width < 0)
            {
                uv0.x = -(sourceRect.x / image.Width);
                uv1.x = (uv0.x - (float)(Math.Abs(sourceRect.width) / image.Width));
            }
            else
            {
                uv0.x = sourceRect.x / image.Width;
                uv1.x = uv0.x + (float)(sourceRect.width / image.Width);
            }

            if (sourceRect.height < 0)
            {
                uv0.y = -(sourceRect.y / image.Height);
                uv1.y = (uv0.y - (float)(Math.Abs(sourceRect.height) / image.Height));
            }
            else
            {
                uv0.y = sourceRect.y / image.Height;
                uv1.y = uv0.y + (float)(sourceRect.height / image.Height);
            }

            ImGui.Image(new IntPtr(image.ID), new Vector2(destWidth, destHeight), uv0, uv1);
        }
    }
}
