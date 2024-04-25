using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AIGallery
{
    public class GalleryViewModel : ObservableObject
    {
        private ObservableCollection<ImageItemViewModel> _images;
        private int _currentPageIndex;

        static readonly int PageSize = 6;

        public ObservableCollection<ImageItemViewModel> Images
        {
            get { return _images; }
            set { SetProperty(ref _images, value); }
        }

        public RelayCommand NextPageCommand { get; }
        public RelayCommand PrevPageCommand { get; }
        public ICommand ViewImageCommand { get; }

        public GalleryViewModel()
        {
            _currentPageIndex = 0;
            Images = new ObservableCollection<ImageItemViewModel>();

            NextPageCommand = new RelayCommand(NextPage, CanNavigateNext);
            PrevPageCommand = new RelayCommand(PreviousPage, CanNavigatePrev);
            ViewImageCommand = new RelayCommand<ImageItemViewModel>(ViewImage);
            UpdateImages();
        }

        private void NextPage()
        {
            _currentPageIndex++;
            UpdateImages();
        }

        private bool CanNavigateNext()
        {
            return _currentPageIndex < CalculateTotalPages() - 1;
        }

        private void PreviousPage()
        {
            _currentPageIndex--;
            UpdateImages();
        }

        private bool CanNavigatePrev()
        {
            return _currentPageIndex > 0;
        }

        private int CalculateTotalPages()
        {
            using (var context = new AppDBContext())
            {
                return (context.Images.Count() + (PageSize - 1)) / PageSize; // round up
            }
        }

        private void UpdateImages()
        {
            Images.Clear();
            FetchImagesFromDatabase();
            NextPageCommand.NotifyCanExecuteChanged();
            PrevPageCommand.NotifyCanExecuteChanged();
        }

        private void FetchImagesFromDatabase()
        {
            using (var context = new AppDBContext())
            {
                var imageDataList = context.Images
                    .OrderBy(i => i.Id)
                    .Skip(PageSize * _currentPageIndex)
                    .Take(PageSize)
                    .Select(i => new { i.Id, i.ThumbnailData })
                    .ToList();
                foreach (var imageData in imageDataList)
                {
                    Images.Add(new ImageItemViewModel(imageData.Id, imageData.ThumbnailData));
                }
            }
        }

        private void ViewImage(ImageItemViewModel image)
        {
            using (var context = new AppDBContext())
            {
                var fullImage = context.Images.Find(image.Id);
                var imageViewModel = new ImageViewModel(fullImage);
                imageViewModel.ImageDeleted += OnImageDeleted;
                var imageView = new ImageView(imageViewModel);
                
                var window = new Window
                {
                    Content = imageView,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                window.ShowDialog();
            }
        }

        private void OnImageDeleted(object sender, EventArgs e)
        {
            UpdateImages();
        }
    }
}
