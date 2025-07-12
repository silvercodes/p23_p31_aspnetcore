namespace _05_custom_provider;

public static class TextFileConfigurationExtensions
{
    public static IConfigurationBuilder AddTextFile(this IConfigurationBuilder builder, string filePath)
    {
        return builder.Add(new TextFileConfigurationSource(filePath));
    }
}
