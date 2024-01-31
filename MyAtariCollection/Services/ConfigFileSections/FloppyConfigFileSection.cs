namespace MyAtariCollection.Services.ConfigFileSections;

public class FloppyConfigFileSection : ConfigFileSection, IFloppyConfigFileSection
{
    public const string ConfigSectionName = "Floppy";

    private const string AutoInsertDiskBKey = "bAutoInsertDiskB";
    private const string FastFloppyAccessKey = "FastFloppy";
    private const string DriveAEnabledKey = "EnableDriveA";
    private const string DriveBEnabledKey = "EnableDriveB";
    private const string DriveAPathKey = "szDiskAFileName";
    private const string DriveBPathKey = "szDiskBFileName";
    private const string DriveANumberOfHeadsKey = "DriveA_NumberOfHeads";
    private const string DriveBNumberOfHeadsKey = "DriveB_NumberOfHeads";
    private const string WriteProtectionKey = "nWriteProtection";
    
    public void ToHatariConfig(StringBuilder builder, AtariConfiguration config)
    {
        AddSection(builder, ConfigSectionName);
        
        
        AddFlag(builder, AutoInsertDiskBKey, config.FloppyOptions.AutoInsertB);
        AddFlag(builder, FastFloppyAccessKey, config.FloppyOptions.FastFloppyAccess);

        AddFlag(builder, DriveAEnabledKey, config.FloppyOptions.DriveAEnabled);
        AddFlag(builder, DriveAPathKey, config.FloppyOptions.DriveAPath);
        AddFlag(builder, DriveANumberOfHeadsKey, config.FloppyOptions.DriveADoubleSided ? 2 : 1);
        
        
        
        AddFlag(builder, DriveBEnabledKey, config.FloppyOptions.DriveBEnabled);
        AddFlag(builder, DriveBPathKey, config.FloppyOptions.DriveBPath);
        AddFlag(builder, DriveBNumberOfHeadsKey, config.FloppyOptions.DriveBDoubleSided ? 2 : 1);
        
        AddFlag(builder, WriteProtectionKey, (int) config.FloppyOptions.WriteProtection);
        
        builder.AppendLine();
    }

    public void FromHatariConfig(AtariConfiguration to, Dictionary<string, Dictionary<string, string>> sections)
    {
        var section = sections[ConfigSectionName];
        
        to.FloppyOptions.AutoInsertB = ParseBool(AutoInsertDiskBKey, section);
        to.FloppyOptions.FastFloppyAccess = ParseBool(FastFloppyAccessKey, section);
        to.FloppyOptions.WriteProtection = ParseEnumValue<DiskWriteProtection>(WriteProtectionKey, section);
        
        to.FloppyOptions.DriveAEnabled = ParseBool(DriveAEnabledKey, section);
        to.FloppyOptions.DriveAPath = section[DriveAPathKey];
        var numberOfHeadsA = ParseInt(DriveANumberOfHeadsKey, section);
        to.FloppyOptions.DriveADoubleSided = numberOfHeadsA == 2 ? true : false;
        
        to.FloppyOptions.DriveBEnabled = ParseBool(DriveBEnabledKey, section);
        to.FloppyOptions.DriveBPath = section[DriveBPathKey];
        var numberOfHeadsB = ParseInt(DriveBNumberOfHeadsKey, section);
        to.FloppyOptions.DriveBDoubleSided = numberOfHeadsB == 2 ? true : false;
        
    }
}