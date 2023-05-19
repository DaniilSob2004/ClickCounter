namespace ClickCounter
{
    public static class FileWork
    {
        public static string filename = "record.txt";

        public static void WriteFile(string data)
        {
            using (var sw = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                sw.Write(data);
            }
        }

        public static string ReadFile()
        {
            string data;

            using (var sr = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                data = sr.ReadToEnd();
            }

            return data;
        }
    }
}
