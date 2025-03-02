namespace Lab3.Tests
{
    public static class Utils
    {
        public static async Task<IEnumerable<string>> ReadWordsInFileAsync(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"../../../TestData/{fileName}");

            return await File.ReadAllLinesAsync(path);
        }
    }
}