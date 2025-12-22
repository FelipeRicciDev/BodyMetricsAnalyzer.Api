namespace API.Services.Pdf;

public sealed class PdfTextOcrExtractor(
    PdfImageExtractor pdfImageExtractor,
    OcrService ocrService) : IPdfTextExtractor
{
    private readonly PdfImageExtractor _pdfImageExtractor = pdfImageExtractor;
    private readonly OcrService _ocrService = ocrService;

    public async Task<string> ExtractTextAsync(
        Stream pdfStream,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var imageStreams =
            _pdfImageExtractor.ExtractImagesAsStreams(pdfStream);

        var textBuilder = new StringBuilder();

        foreach (var imageStream in imageStreams)
        {
            var text = _ocrService.ReadText(imageStream);
            textBuilder.AppendLine(text);
        }

        return textBuilder.ToString();
    }
}