namespace MyAtariCollection.Services.OptionsGenerators;

public interface ISystemOptionsGenerator
{
    void Generate(AtariConfiguration config, StringBuilder builder);
}