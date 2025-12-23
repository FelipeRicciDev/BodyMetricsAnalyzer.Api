namespace Domain.BodyComposition.DomainServices;

public static class BodyCompositionAnalyzer
{
    public static BodyCompositionExam Analyze(string rawText)
    {
        var normalized =
            string.IsNullOrWhiteSpace(rawText)
            ? string.Empty
            : CompositionTextNormalizer.NormalizeOcrText(rawText);

        var section =
            CompositionSectionExtractor.Extract(normalized);

        var skeletal =
            BodyCompositionParser.ParseSkeletalMuscle(normalized);

        var compositionFromSection =
            BodyCompositionParser.Parse(section);

        var composition =
            skeletal is not null
                ? compositionFromSection with
                {
                    MusculoEsqueletico = skeletal
                }
                : compositionFromSection;

        var header =
            BodyCompositionParser.ParseHeader(normalized);

        var score =
            BodyCompositionParser.ParseBodyScore(normalized);

        return new BodyCompositionExam(
            Header: header,
            Score: score,
            Composition: composition
        );
    }
}