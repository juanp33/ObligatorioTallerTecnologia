namespace ObligatorioTallerTecnologia
{
    using ObligatorioTallerTecnologia.Modelo;
    using Microsoft.Maui.Controls;
    using System;
    using System.Collections.ObjectModel;

    public partial class MovieDetailsPage : ContentPage
    {
        private int _currentIndex;

        public MovieDetailsPage(Movie movieDetails)
        {
            InitializeComponent();
            BindingContext = movieDetails;
            _currentIndex = 0;
        }

        private void OnLeftArrowTapped(object sender, EventArgs e)
        {
            ScrollCollectionView(-1);
        }

        private void OnRightArrowTapped(object sender, EventArgs e)
        {
            ScrollCollectionView(1);
        }

        private void ScrollCollectionView(int direction)
        {
            var castList = (BindingContext as Movie)?.Credits?.Cast;
            if (castList != null && castList.Count > 0)
            {
                _currentIndex = Math.Max(0, Math.Min(castList.Count - 1, _currentIndex + direction));
                ActorsCollectionView.ScrollTo(_currentIndex, position: ScrollToPosition.Center, animate: true);
            }
        }
    }
}