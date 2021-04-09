using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MusicStore.Desktop.Views
{
    public class AlbumView : UserControl
    {
        public AlbumView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}