using Avalonia;
using Avalonia.Markup.Xaml;

namespace Automix_update
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
