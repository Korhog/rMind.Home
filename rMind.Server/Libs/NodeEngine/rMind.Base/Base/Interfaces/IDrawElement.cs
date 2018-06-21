namespace rMind.Draw
{
    /// <summary> Элемент отрисовки </summary>
    public interface IDrawElement
    {
        /// <summary> Инициализация </summary>
        void Init();

        /// <summary> Получение родительского контроллера </summary>
        Elements.rMindBaseController GetController();

        /// <summary> Получение сдвига </summary>
        Types.Vector2 GetOffset();        
    }
}
