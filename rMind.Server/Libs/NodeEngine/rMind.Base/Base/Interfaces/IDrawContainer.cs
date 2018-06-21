namespace rMind.Draw
{
    using Types;
    
    public interface IDrawContainer : IDrawElement
    {
        Vector2 SetPosition(double x, double y);
        void Translate(Vector2 vector);
    }
}
