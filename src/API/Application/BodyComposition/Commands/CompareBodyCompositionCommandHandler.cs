namespace API.Application.BodyComposition.Commands;

public sealed class CompareBodyCompositionCommandHandler(
    IPdfTextExtractor pdfTextExtractor)
    : IRequestHandler<
        CompareBodyCompositionCommand,
        BodyCompositionComparisonResponse>
{
    public async Task<BodyCompositionComparisonResponse> Handle(
        CompareBodyCompositionCommand request,
        CancellationToken cancellationToken)
    {
        CompareBodyCompositionCommandValidator.Validate(request);

        var pdfA = request.PdfFiles[0];
        var pdfB = request.PdfFiles[1];

        var textA = await pdfTextExtractor.ExtractTextAsync(
            pdfA.OpenReadStream(),
            cancellationToken);

        var textB = await pdfTextExtractor.ExtractTextAsync(
            pdfB.OpenReadStream(),
            cancellationToken);

        var examA = BodyCompositionAnalyzer.Analyze(textA);
        var examB = BodyCompositionAnalyzer.Analyze(textB);

        var dateA = examA.Header.DataExame ?? DateTime.MinValue;
        var dateB = examB.Header.DataExame ?? DateTime.MinValue;

        var olderExam = dateA <= dateB ? examA : examB;
        var newerExam = dateA > dateB ? examA : examB;

        var (compositionComparison, scoreComparison) =
            BodyCompositionComparator.Compare(olderExam, newerExam);

        return new BodyCompositionComparisonResponse(
            User: new UserResponse(
                Name: newerExam.Header.Nome ?? "Não identificado",
                Age: newerExam.Header.Idade ?? 0,
                Sex: newerExam.Header.Sexo ?? "Não informado",
                Height: newerExam.Header.AlturaCm ?? 0
            ),
            Score: scoreComparison,
            Comparison: compositionComparison
        );
    }
}
