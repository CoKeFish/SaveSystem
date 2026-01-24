namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Defines the different types of save files that can exist within a slot.
    ///     Each type represents a different context or lifecycle in the game.
    /// </summary>
    public enum SlotFileType
    {
        /// <summary>
        ///     Metadata file containing slot information (creation date, last played, etc.).
        ///     Small, persistent, always loaded.
        /// </summary>
        Meta,

        /// <summary>
        ///     Player profile data (stats, inventory, characters).
        ///     Small to medium, persistent, always loaded.
        /// </summary>
        Player,

        /// <summary>
        ///     Lobby/menu state data.
        ///     Medium size, contextual, loaded only in lobby.
        /// </summary>
        Lobby,

        /// <summary>
        ///     Dungeon exploration data.
        ///     Large, contextual, loaded only during dungeon.
        /// </summary>
        Dungeon,

        /// <summary>
        ///     Battle state data (temporary).
        ///     Medium size, ephemeral, deleted after battle ends.
        /// </summary>
        Battle
    }
}
