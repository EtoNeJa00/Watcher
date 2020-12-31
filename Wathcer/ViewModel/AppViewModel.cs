using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wathcer.Models;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.IO;

namespace Wathcer.ViewModel
{
    
    public class AppViewModel : INotifyPropertyChanged
    {
        private Random rnd;
        public ObservableCollection<Movie> Movies { get; set; }
        private string text;
        public string NewMovie {
            get { return text; }
            set { text = value; OnPropertyChanged("NewMovie"); } 
        }
        public string celectedMov;
        public string CelectedMov { 
            get { return celectedMov; } 
            set { celectedMov = value; OnPropertyChanged("CelectedMov"); } }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        if (NewMovie == "")
                        {
                            return;
                        }
                        else
                        {
                            Movies.Add(new Movie { Name = NewMovie, Wotched=false });
                            NewMovie = "";
                            Save();
                        }
                        
                    }));
            }
        }
        private RelayCommand watch;
        public RelayCommand Watch
        {
            get
            {
                return watch ??
                    (watch = new RelayCommand(obj =>
                    {
                        var i = rnd.Next( Movies.Count);
                        CelectedMov = Movies[i].Name;
                        Movies[i].Wotched = true;
                    }));
            }
        }

        public AppViewModel()
        {
            NewMovie = "";
            rnd = new Random();
            Movies = Load();
        }

        public void Save()
        {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"movies.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Movies);
            }
        }
        public ObservableCollection<Movie> Load()
        {
            ObservableCollection<Movie> movies;
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@"movies.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                movies = (ObservableCollection<Movie>)serializer.Deserialize(file, typeof(ObservableCollection<Movie>));
            }
            return movies;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
