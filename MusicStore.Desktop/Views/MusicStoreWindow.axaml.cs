using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MusicStore.Desktop.ViewModels;
using ReactiveUI;
using System;

namespace MusicStore.Desktop.Views
{
    public class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
    {
        public MusicStoreWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel.BuyMusicCommand.Subscribe(Close)));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}