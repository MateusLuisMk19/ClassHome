namespace ClassHome.Extensions
{
    public static class StringExtensions
    {
        private const string V = "\n";
        private const string W = "\r";

        public static string PrimeiraPalavra(this string texto)
        {
            return texto.Substring(0, texto.IndexOf(" "));
        }

        public static int NRows(this string texto)
        {
            var num = 1;
            var cont = 0;

            for (int i = 0; i < texto.Length; i++)
            {
                var c = texto.Substring(i, 1);
                if (c == V || c == W)
                {
                    cont++;
                    if (cont == 2)
                    {
                        num++;
                        cont = 0;
                    }

                }
            }
            return num;
        }
    }
}