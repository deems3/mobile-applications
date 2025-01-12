using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Models
{
    public class Player : INotifyPropertyChanged
    {
        public int Id { get; private set; }
        public string Name { get; set; } = null!;

        public string? imageContents { get; private set; }

        public string? ImageContents { get => imageContents; set
            {
                if (imageContents != value)
                {
                    imageContents = value;
                    OnPropertyChanged(nameof(ImageContents));
                }
            }
        }

        [NotMapped]
        public ImageSource? Image
        {
            get
            {
                if (ImageContents is null)
                {
                    return null;
                }

                var imageBytes = Convert.FromBase64String(ImageContents);
                var memoryStream = new MemoryStream(imageBytes);

                return ImageSource.FromStream(() => memoryStream);
            }
        }

        // relation to game entity
        public ICollection<Game> Games { get; set; } = [];

        // relation to question entity, icollection because a player is able to have one or more questions
        public ICollection<Question> Questions { get; set; } = [];

        public ICollection<GameQuestion> GameQuestions { get; set; } = [];

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
