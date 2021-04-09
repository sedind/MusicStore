using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using JetBrains.Annotations;
using MusicStore.Desktop.Models;
using ReactiveUI;

namespace MusicStore.Desktop.ViewModels
{
    public class MusicStoreViewModel: ViewModelBase
    {
        private bool _isBusy;
        private string? _searchText;
        private AlbumViewModel? _selectedAlbum;
        private CancellationTokenSource? _cancellationTokenSource;
        
        public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }
        
        
        public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
        
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
        
        public AlbumViewModel? SelectedAlbum
        {
            get => _selectedAlbum;
            set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
        }

        public MusicStoreViewModel()
        {
            BuyMusicCommand = ReactiveCommand.Create(() => SelectedAlbum);

            this.WhenAnyValue(x => x.SearchText)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch);
            
        }

        private async void DoSearch(string? s)
        {
            IsBusy = true;
            SearchResults.Clear();
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            var albums = await Album.SearchAsync(s);

            foreach (var album in albums)
            {
                var vm = new AlbumViewModel(album);
                SearchResults.Add(vm);
            }

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                LoadCovers(_cancellationTokenSource.Token);
            }

            IsBusy = false;
        }

        private async void LoadCovers(CancellationToken cancellationToken)
        {
            foreach (var album in SearchResults.ToList())
            {
                await album.LoadCover();

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

    }
}