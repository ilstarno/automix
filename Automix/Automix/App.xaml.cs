using Avalonia;
using Avalonia.Markup.Xaml;

namespace Automix
{
    public class App : Application
    {
        /// <summary>
        /// initializes loader class of avalonia
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
