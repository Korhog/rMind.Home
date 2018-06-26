using rMind.Robot;
using Windows.UI.Xaml.Controls;

namespace rMind.RobotEngine
{
    public class RobotMindGraphController : CanvasEx.rMindCanvasController
    {
        public RobotMindGraphController(Canvas canvas, ScrollViewer scroll) : base (canvas, scroll)
        {

        }

        public IRobotMind BuildMind()
        {
            return null;
        }
    }
}
