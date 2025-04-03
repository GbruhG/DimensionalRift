# Dimensional Rift - Level Generation System

## Table of Contents
1. [Theme Structure](#theme-structure)
2. [Room Types](#room-types)
3. [Generation Rules](#generation-rules)
4. [Events & Hazards](#events--hazards)
5. [Loot System](#loot-system)
6. [Mission System](#mission-system)
7. [Multiplayer Systems](#multiplayer-systems)
8. [Quick Play Features](#quick-play-features)
9. [Economy System](#economy-system)
10. [Player Progression](#player-progression)

## Theme Structure

### File Organization
```
Assets/
├── Scenes/
│   ├── SpaceStation/
│   │   ├── SpaceStation_Level.unity
│   │   └── Rooms/
│   │       ├── SpaceStation_Standard/
│   │       ├── SpaceStation_Events/
│   │       ├── SpaceStation_Entrance.prefab
│   │       └── SpaceStation_Exit.prefab
│   │
│   ├── Planet/
│   │   ├── Planet_Level.unity
│   │   └── Rooms/
│   │       ├── Planet_Standard/
│   │       ├── Planet_Events/
│   │       ├── Planet_Entrance.prefab
│   │       └── Planet_Exit.prefab
│   │
│   └── AncientTemple/
        ├── Temple_Level.unity
        └── Rooms/
            ├── Temple_Standard/
            ├── Temple_Events/
            ├── Temple_Entrance.prefab
            └── Temple_Exit.prefab
```

### Theme-Specific Elements
Each theme includes:
- Unique visual style
- Custom lighting and effects
- Theme-specific audio
- Environmental storytelling
- Distinct architecture
- Themed props and decorations

## Room Types

### Space Station
1. **Standard Rooms**
   - Command Center
   - Crew Quarters
   - Engineering Bay
   - Research Lab
   - Storage Bay
   - Mess Hall
   - Observation Deck

2. **Event Rooms**
   - Breached Hull Room
   - Reactor Core
   - Medical Bay
   - Airlock Chamber
   - Security Center
   - Cryogenic Storage

3. **Connection Types**
   - Airlocks
   - Emergency Bulkheads
   - Maintenance Tunnels
   - Elevator Shafts
   - Service Corridors

### Planet Surface
1. **Standard Rooms**
   - Clearings
   - Cave Chambers
   - Ancient Ruins
   - Water Features
   - Forest Sections
   - Rocky Outcrops

2. **Event Rooms**
   - Unstable Cave
   - Toxic Swamp
   - Volcanic Area
   - Quick Sand Pit
   - Ancient Artifact Site
   - Wildlife Den

3. **Connection Types**
   - Natural Tunnels
   - Rope Bridges
   - Fallen Trees
   - Underground Passages
   - Ancient Pathways

### Ancient Temple
1. **Standard Rooms**
   - Prayer Chambers
   - Libraries
   - Treasure Vaults
   - Meditation Rooms
   - Warrior Halls
   - Sacred Gardens

2. **Event Rooms**
   - Ritual Chamber
   - Trap Room
   - Oracle Chamber
   - Sacrifice Altar
   - Portal Room
   - Guardian Arena

3. **Connection Types**
   - Magical Portals
   - Secret Passages
   - Moving Walls
   - Ritual Doorways
   - Hidden Corridors

## Generation Rules

### Space Station
- Symmetrical layouts
- Multiple deck levels
- Central hub system
- Logical room placement
- Service areas connecting major sections

### Planet Surface
- Organic, winding paths
- Height variations
- Natural barriers
- Hidden paths
- Environmental obstacles

### Ancient Temple
- Ceremonial layouts
- Puzzle-based connections
- Symmetrical design
- Ascending/descending levels
- Sacred geometry patterns

## Events & Hazards

### Space Station Events
1. **Breached Hull**
   - Sudden decompression
   - Emergency systems activation
   - Zero-G combat
   - Limited oxygen
   - Time pressure

2. **Reactor Meltdown**
   - Radiation zones
   - System failures
   - Emergency protocols
   - Core stabilization
   - Power management

3. **Quarantine Breach**
   - Virus spread
   - Infected crew
   - Containment procedures
   - Medical emergencies
   - Mutation effects

### Planet Surface Events
1. **Cave Collapse**
   - Falling debris
   - Escape sequence
   - Path blocking
   - New route creation
   - Resource gathering

2. **Toxic Release**
   - Poison clouds
   - Safe zones
   - Antidote finding
   - Wildlife mutation
   - Environmental hazards

### Temple Events
1. **Ritual Activation**
   - Magical effects
   - Summoned enemies
   - Curse mechanics
   - Puzzle solving
   - Time limits

## Loot System

### Space Station Loot
- Advanced Technology
- Medical Supplies
- Access Cards
- Weapon Mods
- Research Data
- Ship Components
- Crew Logs

### Planet Surface Loot
- Natural Resources
- Alien Artifacts
- Survival Gear
- Exotic Materials
- Wildlife Samples
- Ancient Technology
- Environmental Gear

### Temple Loot
- Magical Artifacts
- Ancient Scrolls
- Sacred Weapons
- Ritual Items
- Divine Relics
- Cursed Objects
- Prophecy Pieces

## Mission System

### Space Station Missions
- Contain Outbreaks
- System Repairs
- Data Recovery
- Crew Rescue
- AI Management
- Security Breaches
- Research Protection

### Planet Surface Missions
- Exploration
- Resource Collection
- Wildlife Study
- Survival Challenges
- First Contact
- Base Building
- Emergency Response

### Temple Missions
- Artifact Recovery
- Curse Breaking
- Ritual Prevention
- Guardian Defeat
- Prophecy Discovery
- Sacred Protection
- Power Restoration

## Multiplayer Systems

### Core Co-op Features
- 2-4 player teams (expandable with mods)
- Drop-in/drop-out functionality
- Shared team inventory
- Individual item loadouts
- Voice chat with proximity effects
- Ping system for communication

### Equipment System
- Simple, focused equipment
- Basic self-defense tools (bonk hammer, stun gun)
- Utility items prioritized
- Limited inventory space
- No complex combat gear
- Focus on survival tools

### Invasion System
1. **Invasion Mechanics**
   - Sneaky invasions focused on theft and chaos
   - Invaders can:
     - Steal collected loot
     - Release monsters/hazards
     - Sabotage equipment
     - Hide in vents/dark corners
     - Set up traps
     - Cause distractions
   
2. **Invader Tools**
   - Lockpicks for sealed containers
   - Monster bait/lures
   - Noise makers
   - Smoke bombs
   - EMP devices
   - Fake loot (tricks team into carrying worthless items)
   - Remote door controls

3. **Defender Tools**
   - Motion sensors
   - Light sources
   - Door locks
   - Alarm systems
   - Emergency flares
   - Noise traps (to detect invaders)
   - Quick escape tools

4. **Risk vs Reward**
   - Invaders must escape with stolen loot
   - No direct combat - all about stealth and timing
   - More valuable loot attracts more invaders
   - Both sides rely on hiding and outsmarting
   - Getting caught means having to run and hide

5. **Invasion Variations**
   - Stealth theft missions
   - Chaos creation (releasing all monsters)
   - Objective sabotage
   - Hide and seek style
   - Multi-invader coordinated heists

### Equipment Types
1. **Utility**
   - Flashlights
   - Walkie talkies
   - Scanner devices
   - Repair tools
   - Grappling hooks
   - Lock picks
   - Monster repellent

2. **Defense**
   - Bonk hammer (last resort)
   - Stun devices
   - Noise makers
   - Smoke bombs
   - Bear traps
   - Barricade tools

3. **Special**
   - Monster bait
   - Teleport beacons
   - Hazard suits
   - Night vision
   - Signal scramblers
   - Emergency flares

### Invasion Events
1. **Random Events**
   - Power outages during invasion
   - Monster attacks during theft
   - Multiple invader chaos
   - Environmental disasters
   - System malfunctions

2. **Special Scenarios**
   - Stealth competitions
   - Loot races
   - Hide and seek events
   - Trap setting contests
   - Monster release chaos

3. **Event Combinations**
   - Natural disasters + invasions
   - Monster hordes + thieves
   - Multiple invader teams competing
   - System failures during heists
   - Environmental hazards during escapes

## Quick Play Features

### Session Structure
- 15-30 minute runs
- Random mission generation
- Quick restart options
- Fast loading times
- Save points for longer missions

### Game Loop
1. **Preparation Phase**
   - Equipment selection
   - Mission briefing
   - Team loadout
   - Resource management
   - Quick strategy planning

2. **Mission Phase**
   - Clear objectives
   - Time pressure
   - Dynamic events
   - Escape mechanics
   - Quick extraction options

3. **Reward Phase**
   - Quick loot summary
   - Team performance rating
   - Instant rewards
   - Quick next mission selection
   - Fast shop access

### Quick Start Options
- Preset loadouts
- Recommended team compositions
- Quick tutorial for new players
- Fast matchmaking
- Difficulty presets

## Economy System

### Mission Economy
- Team shared quota (like Lethal Company)
- Daily/Weekly challenges
- Group bonuses
- Failure penalties
- Risk vs reward mechanics

### Shop System
1. **Equipment Shop**
   - Basic survival gear
   - Advanced tech
   - Specialized tools
   - One-use items
   - Team upgrades

2. **Black Market**
   - Rare items
   - Special modifications
   - Experimental gear
   - High-risk items
   - Limited time offers

### Resource Management
- Limited inventory space
- Team weight distribution
- Shared resources
- Emergency supplies
- Quick-use items

## Player Progression

### Team Progression
- Shared base upgrades
- Team ability unlocks
- Group achievements
- Collective milestones
- Shared cosmetics

### Individual Progress
- Personal loadout unlocks
- Role specialization
- Skill improvements
- Custom cosmetics
- Achievement tracking

### Unlock System
- Quick early game progression
- Long-term goals
- Cosmetic rewards
- Special abilities
- Team bonuses

## Randomization Systems

### Per-Run Variables
- Enemy types and placement
- Loot distribution
- Event triggering
- Hazard locations
- Mission objectives

### Dynamic Difficulty
- Team size scaling
- Performance-based adjustment
- Progressive challenge
- Catch-up mechanics
- Risk/reward balance

### Event Probability
- Higher risk = better rewards
- Team size affects event frequency
- Performance influences challenge
- Time-based escalation
- Chain reaction events

## Social Features

### Communication Tools
- Quick chat wheel
- Ping system
- Voice chat zones
- Danger indicators
- Status notifications

### Team Building
- Quick match finder
- Friend preferences
- Role recommendations
- Team statistics
- Performance tracking

### Community Features
- Weekly challenges
- Team leaderboards
- Shared discoveries
- Community events
- Custom lobbies

## Modding Support

### Basic Modding
- Custom rooms
- New events
- Additional items
- Monster variants
- Mission types

### Advanced Modding
- New themes
- Gameplay mechanics
- Custom roles
- Special effects
- Total conversions

## Quality of Life

### Team Convenience
- Quick equipment presets
- Fast travel in hub
- Auto-loot distribution
- Quick command wheel
- Shared waypoints

### Game Flow
- Fast loading
- Quick restarts
- Minimal downtime
- Clear objectives
- Easy extraction

### UI/UX
- Clear danger indicators
- Team status at a glance
- Quick inventory management
- Easy communication tools
- Minimal menu navigation

## Cross-Theme Integration

### Hybrid Areas
- Space Station crashed into Temple
- Ancient Technology in Natural Caves
- Temple Ruins in Space Station
- Alien Architecture in Temple
- Natural Growth in Space Station
- Tech-Enhanced Temple

### Connected Stories
- Multi-theme storylines
- Progressive difficulty
- Interconnected events
- Shared resources
- Theme mixing
- Environmental evolution 

public void GenerateLevel()
{
    // Your existing level generation code...
    
    // After all rooms are placed
    StartCoroutine(BakeNavMeshAfterDelay());
}

private IEnumerator BakeNavMeshAfterDelay()
{
    // Wait a frame to ensure all objects are properly placed
    yield return null;
    
    // Find and bake the NavMesh
    FindObjectOfType<NavMeshBaker>().BakeNavMesh();
} 