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

using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MountFuji.Services;

public class SystemsService: ISystemsService
{
    private readonly IPersistence persistence;
    private readonly ILogger<SystemsService> log;
    private readonly IConfigFileService config;

    private List<AtariConfiguration> store = [];
    private string stateForDirtyCheck;
    public IEnumerable<AtariConfiguration> All() => store.AsEnumerable();

    public SystemsService(IPersistence persistence, ILogger<SystemsService> log, IConfigFileService config)
    {
        this.persistence = persistence;
        this.log = log;
        this.config = config;
    }
    
    public void Load()
    {
        
        log.LogInformation("Attempting to load systems from: {SaveFile}", persistence.MountFujiSystemsFile);
        store = persistence.DeSerialize<List<AtariConfiguration>>(persistence.MountFujiSystemsFile);
        SetStateForDirtyCheck();
        log.LogInformation("Loaded {Count} systems", store.Count);
    }
    
    /// <summary>
    /// Saves the preferences to the the preferences JSON file.
    /// </summary>
    public async Task Save()
    {
        log.LogInformation("Attempting to save systems to: {SaveFile}", persistence.MountFujiSystemsFile);
        await persistence.SerializeAsync(persistence.MountFujiSystemsFile, store);
        SetStateForDirtyCheck();
    }

    public void SetStateForDirtyCheck()
    {
        stateForDirtyCheck = JsonSerializer.Serialize(store);
    }

    public bool IsDirty
    {
        get
        {
            string currentState = JsonSerializer.Serialize(store);
            return !currentState.Equals(stateForDirtyCheck);
        }
    }

    public void Add(AtariConfiguration newConfiguration)
    {
        newConfiguration.Id = Guid.NewGuid().ToString();
        log.LogInformation("Added new system to store: {System}, Id:{Id}", newConfiguration.DisplayName, newConfiguration.Id);
        store.Add(newConfiguration);
    }


    public AtariConfiguration Clone(AtariConfiguration original, string newName)
    {
        // not the fastest approach but this isn't used in a game loop!
        var json = JsonSerializer.Serialize<AtariConfiguration>(original);
        AtariConfiguration clone = JsonSerializer.Deserialize<AtariConfiguration>(json);
        clone.DisplayName = newName;
        
        log.LogInformation("Cloning system: {Name}, Id = {Id} to {Name}", original.DisplayName, original.Id, newName);
        Add(clone);
        
        log.LogInformation("New system: {Name}, has Id = {Id}", clone.DisplayName, clone.Id);
        return clone;
    }
    
    public void Delete(string id)
    {
        AtariConfiguration config = Find(id);
        
        log.LogInformation("Deleting system: {Name}, with Id: {Id}", config.DisplayName, config.Id);

        // technically a moot test as List.Remove(null) just returns false if there
        // are no null entries in the List, which it could be
        if (config is not null)
        {
            store.Remove(config);
        }
    }

    public AtariConfiguration Find(string id)
    {
        log.LogInformation("Findong store by ID: {Id}", id);
        return store.FirstOrDefault(system => system.Id == id);
    }

    public void ReorderByIds(List<string> ids)
    {
        log.LogInformation("Re-odering system");
        store = store.OrderBy(d => ids.IndexOf(d.Id)).ToList();
    }

    /// <summary>
    /// Imports a hatari .cfg file and creates a new system from it
    /// </summary>
    /// <param name="filename">full path to (including name) to the config file we are importing </param>
    /// <param name="name">The name to give to the new system</param>
    /// <returns></returns>
    public async Task<AtariConfiguration> Import(string filename, string name)
    {
        log.LogInformation("Importing config from {FileName} and naming it {Name}",  filename, name);
        var res = await config.Load(filename);
        res.DisplayName = name;
        Add(res);
        return res;
    }
}