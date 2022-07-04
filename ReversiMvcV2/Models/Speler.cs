using System.ComponentModel.DataAnnotations;

namespace ReversiMvcV2.Models
{
    public class Speler
    {
        [Key]
        public string Guid { get; set; }
        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }
        
        public Speler(string naam)
        {
            Guid = System.Guid.NewGuid().ToString();
            Naam = naam;
            AantalGelijk = 0;
            AantalVerloren = 0;
            AantalGewonnen = 0;
        }

        public Speler(string guid, string naam)
        {
            Guid = guid;
            Naam = naam;
            AantalGelijk = 0;
            AantalGewonnen = 0;
            AantalVerloren = 0;
        }

    }
}
