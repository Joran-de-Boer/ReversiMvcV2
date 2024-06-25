namespace ReversiMvcV2.Models
{
    public class Spel
    {

        public string? ID { get; set; }
        public string Omschrijving { get; set; }
        public Kleur[,]? Bord { get; set; }
        public Kleur? AanDeBeurt { get; set; }
        public string Speler1 { get; set; }
        public string? Speler2 { get; set; }
        public Speler Owner { get; set; }

        public Spel(string speler, string omschrijving)
        {
            Omschrijving = omschrijving;
            Speler1 = speler;
        }


        public Spel(string speler1, string? speler2, string bord, Kleur aanDeBeurt, string omschrijving, string iD)
        {
            Speler1 = speler1;
            Speler2 = speler2;
            Bord = BordConverter.ConvertStringToBord(bord);
            AanDeBeurt = aanDeBeurt;
            Omschrijving = omschrijving;
            ID = iD;
        }

        public Spel(SpelJson spelJson)
        {
            ID = spelJson.Token;
            Omschrijving = spelJson.Omschrijving;
            Bord = BordConverter.ConvertStringToBord(spelJson.Bord);
            Speler1 = spelJson.Speler1Token;
            Speler2 = spelJson.Speler2Token;
            AanDeBeurt = spelJson.AandeBeurt;
        }
    }   
}
