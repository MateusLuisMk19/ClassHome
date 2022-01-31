namespace ClassHome.Extensions
{
    public static class StringExtensions
    {
        public static string PrimeiraPalavra(this string texto)
        {
            return texto.Substring(0, texto.IndexOf(" "));
        }

        public static string ResumoName(this string texto)
        {
            var count = 0;
            var i = 0;
            var text = "";
            if (texto.Length > 25)
            {
                for (i = 0; i < texto.Length; i++)
                {
                    var c = texto.Substring(i, 1);
                    if (c == " ")
                    {
                        count++;
                    }
                    if (count == 4)
                    {
                        text = texto.Substring(0,i);
                        break;
                    }    
                }
            }
            else
            {
                text = texto;

            }
            return text;
        }
    }
}