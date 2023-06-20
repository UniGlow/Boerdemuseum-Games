using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuenstlerportraits
{
    public class GameEvents
    {
        public delegate void ArtistSelectionHandler(Artist artist);
        public static event ArtistSelectionHandler OnArtistSelectionStarted;

        public static void ArtistSelectionStarted(Artist artist)
        {
            string debugLog = "Invoking GameEvent \"OnArtistSelectionStarted\" with artist: ";
            debugLog += artist != null ? artist.Name : "null";
            Debug.Log(debugLog + ".");
            OnArtistSelectionStarted?.Invoke(artist);
        }
        public static event ArtistSelectionHandler OnArtistSelectionFinished;

        public static void ArtistSelectionFinished(Artist artist)
        {
            string debugLog = "Invoking GameEvent \"OnArtistSelectionFinished\" with artist: ";
            debugLog += artist != null ? artist.Name : "null";
            Debug.Log(debugLog + ".");
            OnArtistSelectionFinished?.Invoke(artist);
        }

        public delegate void CategorySelectionHandler(Category category);
        public static event CategorySelectionHandler OnCategorySelectionStarted;
        public static void CategorySelectionStarted(Category category)
        {
            Debug.Log("Invoking GameEvent \"OnCategorySelectionStarted\" with category: " + category.Name + ".");
            OnCategorySelectionStarted?.Invoke(category);
        }
        public static event CategorySelectionHandler OnCategorySelectionFinished;
        public static void CategorySelectionFinished(Category category)
        {
            Debug.Log("Invoking GameEvent \"OnCategorySelectionFinished\" with category: " + category.Name + ".");
            OnCategorySelectionFinished?.Invoke(category);
        }

        public delegate void ArtworkSelectionHandler(Artwork artwork);
        public static event ArtworkSelectionHandler OnArtworkSelectionFinished;
        public static void ArtworkSelectionFinished(Artwork artwork)
        {
            Debug.Log("Invoking GameEvent \"OnArtworkSelectionFinished\" with artwork: " + artwork.Description + ".");
            OnArtworkSelectionFinished?.Invoke(artwork);
        }
        public static event ArtworkSelectionHandler OnArtworkDescriptionCloseStarted;
        public static void ArtworkDescriptionCloseStarted(Artwork artwork)
        {
            if (artwork != null) Debug.Log("Invoking GameEvent \"OnArtworkDescriptionCloseStarted\" with artwork: " + artwork.Description + ".");
            else Debug.Log("Invoking GameEvent \"OnArtworkDescriptionCloseStarted\" with artwork: null.");
            OnArtworkDescriptionCloseStarted?.Invoke(artwork);
        }
        public static event ArtworkSelectionHandler OnArtworkDescriptionCloseFinished;
        public static void ArtworkDescriptionCloseFinished(Artwork artwork)
        {
            if (artwork != null) Debug.Log("Invoking GameEvent \"OnArtworkDescriptionCloseFinished\" with artwork: " + artwork.Description + ".");
            else Debug.Log("Invoking GameEvent \"OnArtworkDescriptionCloseFinished\" with artwork: null.");
            OnArtworkDescriptionCloseFinished?.Invoke(artwork);
        }

        public delegate void ArtworkSelectionHandlerExpanded(Artwork artwork, Category category, int artworkNo);
        public static event ArtworkSelectionHandlerExpanded OnArtworkSelectionStarted;
        public static void ArtworkSelectionStarted(Artwork artwork, Category category, int artworkNo)
        {
            Debug.Log("Invoking GameEvent \"OnArtworkSelectionStarted\" with artwork: " + artwork.Description + ", category: " + category.NameEnglish + ", artworkNo: " + artworkNo + ".");
            OnArtworkSelectionStarted?.Invoke(artwork, category, artworkNo);
        }

        public delegate void ArtworkPageHandler();
        public static event ArtworkPageHandler OnLoadNextArtwork;
        public static void LoadNextArtwork()
        {
            Debug.Log("Invoking GameEvent \"OnLoadNextArtwork\"");
            OnLoadNextArtwork?.Invoke();
        }

        public static event ArtworkPageHandler OnLoadPreviousArtwork;
        public static void LoadPreviousArtwork()
        {
            Debug.Log("Invoking GameEvent \"OnLoadNextArtwork\"");
            OnLoadPreviousArtwork?.Invoke();
        }
    }
}
