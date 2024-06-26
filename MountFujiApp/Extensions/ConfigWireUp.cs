/*
   Mount Fuji - A front end for the Hatari Emulator
   Copyright (C) 2024  David Black

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using MountFuji.Services.ConfigFileSections;

namespace MountFuji.Extensions;

public static class ConfigWireUp
{
    /// <summary>
    /// Adds support for config file processing, import and export
    /// </summary>
    /// <param name="services"></param>
    public static void AddConfigService(this IServiceCollection services)
    {
        services.AddTransient<ILogConfigFileSection, LogConfigFileSection>();
        services.AddTransient<IMemoryConfigFileSection, MemoryConfigFileSection>();
        services.AddTransient<ISystemConfigFileSection, SystemConfigFileSection>();
        services.AddTransient<IRomConfigFileSection, RomConfigFileSection>();
        services.AddTransient<IHardDiskConfigFileSection, HardDiskConfigFileSection>();
        services.AddTransient<IFloppyConfigFileSection, FloppyConfigFileSection>();
        services.AddTransient<IAcsiConfigFileSection, AcsiConfigFileSection>();
        services.AddTransient<IScsiConfigFileSection, ScsiConfigFileSection>();
        services.AddTransient<IIdeConfigFileSection, IdeConfigFileSection>();
        services.AddTransient<IScreenConfigFileSection, ScreenConfigFileSection>();
        services.AddTransient<ISoundConfigFileSection, SoundConfigFileSection>();
        services.AddTransient<IRawHatariConfigFile, RawHatariConfigFile>();
        services.AddTransient<IKeyboardConfigFileSection, KeyboardConfigFileSection>();
    }
}

