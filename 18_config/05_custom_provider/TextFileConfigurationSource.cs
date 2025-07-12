﻿namespace _05_custom_provider;

public class TextFileConfigurationSource : IConfigurationSource
{
    public string FilePath { get; }
    public TextFileConfigurationSource(string filePath)
    {
        this.FilePath = filePath;
    }
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException($"Configuration file not found: {FilePath}");
        }

        return new TextFileConfigurationProvider(FilePath);
    }
}
