namespace RayEngine.GameObjects.Components
{
    public class TagComponent
    {
        public string Tag { get; set; }

        public TagComponent()
        {
            Tag = "GameObject";
        }

        public TagComponent(string tag)
        {
            Tag = tag;
        }

        public static implicit operator string(TagComponent component) => component.Tag;

        public override string ToString() => Tag;
    }
}
