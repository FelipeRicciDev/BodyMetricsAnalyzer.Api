namespace API.Services.Pdf;

public sealed class PdfImageExtractor
{
    public List<Stream> ExtractImagesAsStreams(Stream pdfStream)
    {
        using var memory = new MemoryStream();
        pdfStream.CopyTo(memory);

        var images = new List<Stream>();

        using var reader = DocLib.Instance.GetDocReader(
            memory.ToArray(),
            new PageDimensions(3000, 3000));

        for (int i = 0; i < reader.GetPageCount(); i++)
        {
            using var page = reader.GetPageReader(i);

            var image = CreateImage(page);

            var imageStream = new MemoryStream();
            image.SaveAsPng(imageStream);
            imageStream.Position = 0;

            images.Add(imageStream);
        }

        return images;
    }

    private static Image<Rgba32> CreateImage(IPageReader page)
    {
        var image = new Image<Rgba32>(
            page.GetPageWidth(),
            page.GetPageHeight());

        var raw = page.GetImage();

        image.ProcessPixelRows(accessor =>
        {
            int index = 0;

            for (int y = 0; y < accessor.Height; y++)
            {
                var row = accessor.GetRowSpan(y);

                for (int x = 0; x < row.Length; x++)
                {
                    row[x] = new Rgba32(
                        raw[index + 2],
                        raw[index + 1],
                        raw[index],
                        raw[index + 3]
                    );

                    index += 4;
                }
            }
        });

        return image;
    }
}