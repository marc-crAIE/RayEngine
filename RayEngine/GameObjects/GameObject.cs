namespace RayEngine.GameObjects
{
    public class GameObject
    {
        public GameObject? Parent { get; private set; } = null;
        public List<GameObject> Children { get; private set; } = new List<GameObject>();

        public GameObject() { }

        public void SetParent(ref GameObject parent)
        {
            Parent = parent;
            Parent.Children.Add(this);
        }
    }
}
