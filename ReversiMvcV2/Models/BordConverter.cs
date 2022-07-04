using System.Text;

namespace ReversiMvcV2.Models
{
    public class BordConverter
    {
        public static string ConvertBordToString(Kleur[,] bord)
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            for (int i = 0; i < bord.GetLength(1); i++)
            {
                for (int ii = 0; ii < bord.GetLength(0); ii++)
                {
                    switch (bord[ii, i])
                    {
                        case Kleur.Geen:
                            counter++;
                            break;
                        case Kleur.Wit:
                            CheckCounter(sb, ref counter);
                            sb.Append("W");
                            break;
                        case Kleur.Zwart:
                            CheckCounter(sb, ref counter);
                            sb.Append("B");
                            break;
                        default:
                            throw new Exception("Bord had undefined color");
                    }
                }
                CheckCounter(sb, ref counter);
            }
            return sb.ToString();
        }

        private static void CheckCounter(StringBuilder sb, ref int counter)
        {
            if (counter != 0)
            {
                sb.Append(counter);
                counter = 0;
            }
        }

        public static Kleur[,] ConvertStringToBord(string bordString)
        {
            Kleur[,] bord = new Kleur[8, 8];
            int column = 0;
            int row = 0;
            foreach (char pointer in bordString)
            {
                UpdateRow(ref column, ref row);
                if (char.IsLetter(pointer))
                {
                    HandleLetter(pointer, bord, ref column, row);
                }
                if (char.IsDigit(pointer))
                {
                    HandleDigit(pointer, bord, ref column, row);
                }
            }

            return bord;
        }

        private static void UpdateRow(ref int column, ref int row)
        {
            if (column == 8)
            {
                column = 0;
                row++;
            }
        }

        private static void HandleLetter(char letter, Kleur[,] bord, ref int column, int row)
        {
            switch (letter)
            {
                case 'W':
                    bord[column, row] = Kleur.Wit;
                    break;
                case 'B':
                    bord[column, row] = Kleur.Zwart;
                    break;

            }
            column++;
        }

        private static void HandleDigit(char digit, Kleur[,] bord, ref int column, int row)
        {
            int integer = digit - '0';
            for (int i = 0; i < integer; i++)
            {
                bord[column, row] = Kleur.Geen;
                column++;
            }
        }
    }
}
