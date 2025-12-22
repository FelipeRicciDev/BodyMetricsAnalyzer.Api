namespace API.Services.Ocr;

public sealed class OcrService
{
    private const string DefaultLanguage = "por";

    private readonly string _tessdataPath =
        Path.Combine(AppContext.BaseDirectory, "tessdata");

    public async Task<string> ReadTextAsync(
        Image<Rgba32> image,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var engine = new TesseractEngine(
            _tessdataPath,
            DefaultLanguage,
            EngineMode.LstmOnly);

        engine.DefaultPageSegMode = PageSegMode.SingleColumn;

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new PngEncoder(), cancellationToken);
        ms.Position = 0;

        using var pix = Pix.LoadFromMemory(ms.ToArray());
        using var page = engine.Process(pix);

        return page.GetText();
    }
}