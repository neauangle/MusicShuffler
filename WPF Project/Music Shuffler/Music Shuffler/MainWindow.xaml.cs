﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Music_Shuffler {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Playlist playlist;
        List<String> musicFileExtensions = new List<string>() { ".mp3", ".wav" };
        public MainWindow() {
            InitializeComponent();
        }



        public void btnMakePlaylistClicked(object sender, RoutedEventArgs e) {
            foreach (Album album in playlist.albums) {
                if (album.randomiseSongs) {
                    album.shuffle();
                }
            }

            //etc
        }
            

        public void btnChooseRootClicked(object sender, RoutedEventArgs e) {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
                    return;
                }
                txtRootFolder.Text = dialog.FileName;
            }
        }

            
        public void btnGetAlbumsClicked(object sender, RoutedEventArgs eevent) {
            String rootFolder = txtRootFolder.Text;
            if (!Directory.Exists(rootFolder)) {
                MessageBox.Show("Folder does not exist");
                return;
            }
            //generates albums
            playlist = new Playlist(rootFolder, musicFileExtensions);

            this.clearGUIAlbums();
            this.populateGUIAlbums();
        }

        public void clearGUIAlbums() {
            lstbxAlbums.Items.Clear();
        }

        public void populateGUIAlbums() {
            foreach (Album album in playlist.albums) {
                //Make checkboxes
                CheckBox includeAlbum = new CheckBox();
                includeAlbum.Content = Path.GetFileName(album.albumRoot);
                includeAlbum.ToolTip = album.albumRoot;
                CheckBox shuffleAlbum = new CheckBox();
                shuffleAlbum.Content = "Shuffle";
                //shuffle checkbox will not be enabled when the album is not being included
                Binding shuffletoIncludeAlbumBinding = new Binding();
                shuffletoIncludeAlbumBinding.Source = includeAlbum;
                shuffletoIncludeAlbumBinding.Path = new PropertyPath("IsChecked");
                BindingOperations.SetBinding(shuffleAlbum, CheckBox.IsEnabledProperty, shuffletoIncludeAlbumBinding);

                //Make grid to hold checkboxes
                Grid albumGrid = new Grid();
                ColumnDefinition c1 = new ColumnDefinition();
                c1.Width = new GridLength(2, GridUnitType.Star);
                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = new GridLength(1, GridUnitType.Star);
                albumGrid.ColumnDefinitions.Add(c1);
                albumGrid.ColumnDefinitions.Add(c2);
                //add the two checkboxes to the grid
                Grid.SetColumn(includeAlbum, 0);
                albumGrid.Children.Add(includeAlbum);
                Grid.SetColumn(shuffleAlbum, 1);
                albumGrid.Children.Add(shuffleAlbum);

                //Make ListBoxItem to hold grid
                ListBoxItem albumItem = new ListBoxItem();
                albumItem.Content = albumGrid;

                //add ListBoxItem to ListBox
                lstbxAlbums.Items.Add(albumItem);
            }
        }
    }
}